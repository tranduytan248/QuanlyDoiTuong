using Core.Inv.Caches;
using Core.Inv.Providers;
using Cores.Sys.Caches.Sys;
using Modules.Major.Areas.Major.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Core.Inv.Models.Invs;
using Modules.Major.Areas.Invoice.Data;
using TSFramework.App.Attributes;
using TSFramework.App.Models;
using TSFramework.App.Processors;
using TSFramework.Core.Enums;
using Core.Inv.Enums;
using Core.Inv.Models;
using Cores.eContract.Consts;
using Cores.Major.Caches;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Cores.Base.Apps;
using Cores.Cate.Caches;

namespace Modules.Major.Areas.Invoice.Controllers
{
    public class InvPendingController : AppController
    {
        #region Inits

        private readonly SysConfigCache _sysConfigCache = new SysConfigCache();
        private readonly SysElnvAccountCache _invAccCache = new SysElnvAccountCache();

        private readonly CateUnionCache _unionCache = new CateUnionCache();

        private readonly MajorInvPatternCache _invPatternCache = new MajorInvPatternCache();
        private readonly MajorInvCache _invCache = new MajorInvCache();
        private readonly MajorInvCusCache _invCusCache = new MajorInvCusCache();
        private readonly MajorInvProductCache _invProductCache = new MajorInvProductCache();

        private readonly MajorContractCache _contractCache = new MajorContractCache();

        private readonly string _invTitle = AppProcessor.Messagor.GetMessage("Invoice_Title");

        private const string CONFIG_INV_HOST_INV_SERVICE = "CONFIG_INV_HOST_INV_SERVICE";
        private const string CONFIG_INV_SERVICE_ACCOUNT_NAME = "CONFIG_INV_SERVICE_ACCOUNT_NAME";
        private const string CONFIG_INV_SERVICE_ACCOUNT_PASS = "CONFIG_INV_SERVICE_ACCOUNT_PASS";

        private const string CONFIG_INV_OLD_HOST_INV_SERVICE = "CONFIG_INV_OLD_HOST_INV_SERVICE";
        private const string CONFIG_INV_OLD_SERVICE_ACCOUNT_NAME = "CONFIG_INV_OLD_SERVICE_ACCOUNT_NAME";
        private const string CONFIG_INV_OLD_SERVICE_ACCOUNT_PASS = "CONFIG_INV_OLD_SERVICE_ACCOUNT_PASS";

        private readonly InvProvider _invProvider;
        private readonly string _invServiceAccName = "";
        private readonly string _invServiceAccPass = "";

        private static string[] _arrPermissionViaUser;

        #endregion

        public InvPendingController()
        {
            #region Config Inv

            string hostInvService = "";
            var configModel = _sysConfigCache.GetViaKey(CONFIG_INV_HOST_INV_SERVICE);
            if (configModel != null && !string.IsNullOrEmpty(configModel.ConfigValue))
            {
                hostInvService = configModel.ConfigValue;
            }

            configModel = _sysConfigCache.GetViaKey(CONFIG_INV_SERVICE_ACCOUNT_NAME);
            if (configModel != null && !string.IsNullOrEmpty(configModel.ConfigValue))
            {
                _invServiceAccName = configModel.ConfigValue;
            }

            configModel = _sysConfigCache.GetViaKey(CONFIG_INV_SERVICE_ACCOUNT_PASS);
            if (configModel != null && !string.IsNullOrEmpty(configModel.ConfigValue))
            {
                _invServiceAccPass = configModel.ConfigValue;
            }

            _invProvider = new InvProvider($"{hostInvService}PortalService.asmx", $"{hostInvService}BusinessService.asmx", $"{hostInvService}PublishService.asmx");

            #endregion
        }

        // GET: Invoice/InvPending
        public ActionResult Index()
        {
            _arrPermissionViaUser = GetPermissionViaUser(User.UserName);

            var lstUnionsManagerByUser = _unionCache.GetUnionsViaManager(User.UserName);

            var eInvAcc = _invAccCache.GetByUserName(User.UserName);

            var model = new SearchInvoiceModel
            {
                ListUnions = lstUnionsManagerByUser.Select(u => new ListItem(text: u.UnionName, value: $"{u.UnionId}")).ToList(),
                Permissions = _arrPermissionViaUser,

                ListInvPatterns = _invPatternCache.GetAll().OrderBy(d => d.Pattern)
                    .Where(d => d.IsActive)
                    .Select(d => new ListItem(d.Pattern, d.Pattern.ToString())).Distinct().ToList(),
                ListUsers = _invAccCache.GetAll().Select(d => new ListItem($"{d.FullName} - {d.EmpAccount}", d.EmpAccount.ToString())).ToList(),
                HasNotSysInvAccount = eInvAcc == null,
                IsInvServiceAccountIncorrect = !_invProvider.IsCorrectUser(_invServiceAccName, _invServiceAccPass)
            };
            return View(model);
        }

        #region Get Pending Invs

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult Get(SearchInvoiceModel searchModel)
        {
            var search = Request.Form.GetValues("search[value]")?[0];
            var draw = Request.Form.GetValues("draw")?[0];
            var order = Request.Form.GetValues("order[0][column]")?[0];
            var orderDir = Request.Form.GetValues("order[0][dir]")?[0];
            var startRec = Convert.ToInt32(Request.Form.GetValues("start")?[0]);
            var pageSize = Convert.ToInt32(Request.Form.GetValues("length")?[0]);
            var dataSearch = new BaseSearchModel
            {
                Search = string.IsNullOrEmpty(search) ? null : search,
                Order = order,
                OrderDir = orderDir,
                StartIndex = startRec,
                PageSize = pageSize
            };

            Session[$"InvPending_SearchInv_{User.UserName}"] = searchModel;
            Session[$"InvPending_BaseSearch_{User.UserName}"] = dataSearch;

            var data = _invCache.GetPendingInvs(total: out int total, userName: User.UserName, managerUnions: searchModel.UnionIds, pattern: searchModel.Pattern, serials: searchModel.Serials, invTypes: searchModel.InvTypes, createdFrom: searchModel.CreatedFrom, createdTo: searchModel.CreatedTo, creators: searchModel.Creators, cusName: searchModel.CusName, cusCode: searchModel.CusCode, cusTaxCode: searchModel.CusTaxCode, search: dataSearch);

            var result = Json(
                new { draw = Convert.ToInt32(draw), recordsTotal = total, recordsFiltered = total, data },
                JsonRequestBehavior.AllowGet);
            return result;
        }

        #endregion

        #region Sync Inv

        [ActionType(Type = EnumActionType.Edit)]
        [AjaxOnly]
        [HttpGet]
        public ActionResult SyncInv(string invKey)
        {
            var eInvAcc = _invAccCache.GetByUserName(User.UserName);
            if (eInvAcc == null)
            {
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{AppProcessor.Messagor.GetMessage("Err_InvAccount_Empty")}", EnumProcessType.NonFormat, EnumMsgIcon.Error)
                });
            }

            var invModel = _invCache.GetByKey(invKey);
            if (invModel == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_invTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            ViewBag.ConfirmMessage =
                $"Bạn muốn thực hiện kiểm tra và đồng bộ thông tin cho hoá đơn <b class='text-danger-d1'>[{invModel.Pattern};{invModel.Serial}-{invModel.InvKey}]</b> ?";

            var modelConfirm = new ConfirmInvModel
            {
                InvKey = invModel.InvKey,
                InvId = invModel.InvId,
                InvNo = invModel.InvNo,
                Pattern = invModel.Pattern,
                Serial = invModel.Serial,
                IsOldVersion = invModel.IsOldVersion,
                CreatedOn = invModel.CreatedOn
            };

            return PartialView("_SyncInv", modelConfirm);
        }

        [ActionType(Type = EnumActionType.Edit)]
        [AjaxOnly]
        [HttpPost]
        public Task<ActionResult> SyncInv(ConfirmInvModel model)
        {
            string response;

            var eInvAcc = _invAccCache.GetByUserName(User.UserName);
            if (eInvAcc == null)
            {
                return Task.FromResult<ActionResult>(Json(new
                {
                    status = true,
                    message = CreateMessage($"{AppProcessor.Messagor.GetMessage("Err_InvAccount_Empty")}", EnumProcessType.NonFormat, EnumMsgIcon.Error)
                }));
            }

            int retAction;
            string errMsg;
            
            if (model.IsOldVersion)
            {
                var oldInvProvider = new InvProvider(
                    $"{_sysConfigCache.GetViaKey(CONFIG_INV_OLD_HOST_INV_SERVICE)?.ConfigValue}PortalService.asmx",
                    $"{_sysConfigCache.GetViaKey(CONFIG_INV_OLD_HOST_INV_SERVICE)?.ConfigValue}BusinessService.asmx",
                    $"{_sysConfigCache.GetViaKey(CONFIG_INV_OLD_HOST_INV_SERVICE)?.ConfigValue}PublishService.asmx");
                string oldInvServiceAccName = _sysConfigCache.GetViaKey(CONFIG_INV_OLD_SERVICE_ACCOUNT_NAME)?.ConfigValue;
                string oldInvServiceAccPass = _sysConfigCache.GetViaKey(CONFIG_INV_OLD_SERVICE_ACCOUNT_PASS)?.ConfigValue;


                retAction = oldInvProvider.SyncInvoice(out errMsg, model.InvKey, model.CreatedOn, oldInvServiceAccName, oldInvServiceAccPass, $"Thực hiện kiểm tra đồng bộ thông tin cho hoá đơn [{model.Pattern};{model.Serial}-{model.InvKey}]", User.UserName);

                if (retAction == 1)
                    response = CreateMessage(
                        $"Hoàn thành kiểm tra và đã thực hiện đồng bộ thông tin hoá đơn <b>[{model.Pattern};{model.Serial}-{model.InvKey}]</b> vào hệ thống thành công", EnumProcessType.NonFormat,
                        EnumMsgIcon.Success);

                else if (retAction == 0)
                    response = CreateMessage(
                        $"Hoàn thành kiểm tra thông tin hoá đơn <b>[{model.Pattern};{model.Serial}-{model.InvKey}]</b>. Không tồn tại hoá đơn <b>[{model.Pattern};{model.Serial}-{model.InvKey}]</b> trên hệ thống hoá đơn điện tử. Có thể thực hiện <b>[Phát hành lại]<b/> hoặc [<b>Huỷ trạng thái</b>] hoá đơn", EnumProcessType.NonFormat, EnumMsgIcon.Success);
                else
                {
                    AppProcessor.Logger.Message($"Sync Inv [{model.Pattern};{model.Serial}-{model.InvKey}]: Error => {errMsg}");
                    response = CreateMessage(
                        $"Kiểm tra thông tin hoá đơn <b>[{model.Pattern};{model.Serial}-{model.InvKey}]</b> thất bại. Có lỗi phát sinh => {errMsg}", EnumProcessType.NonFormat, EnumMsgIcon.Error);
                }

                return Task.FromResult<ActionResult>(Json(new { status = true, message = response }, JsonRequestBehavior.AllowGet));
            }

            retAction = _invProvider.SyncInvoice(out errMsg, model.InvKey, model.CreatedOn, _invServiceAccName, _invServiceAccPass, $"Thực hiện kiểm tra đồng bộ thông tin cho hoá đơn [{model.Pattern};{model.Serial}-{model.InvKey}]", User.UserName);

            if (retAction == 1)
                response = CreateMessage(
                    $"Hoàn thành kiểm tra và đã thực hiện đồng bộ thông tin hoá đơn <b>[{model.Pattern};{model.Serial}-{model.InvKey}]</b> vào hệ thống thành công", EnumProcessType.NonFormat,
                    EnumMsgIcon.Success);

            else if (retAction == 0)
                response = CreateMessage(
                    $"Hoàn thành kiểm tra thông tin hoá đơn <b>[{model.Pattern};{model.Serial}-{model.InvKey}]</b>. Không tồn tại hoá đơn <b>[{model.Pattern};{model.Serial}-{model.InvKey}]</b> trên hệ thống hoá đơn điện tử. Có thể thực hiện <b>[Phát hành lại]<b/> hoặc [<b>Huỷ trạng thái</b>] hoá đơn", EnumProcessType.NonFormat, EnumMsgIcon.Success);
            else
            {
                AppProcessor.Logger.Message($"Sync Inv [{model.Pattern};{model.Serial}-{model.InvKey}]: Error => {errMsg}");
                response = CreateMessage(
                    $"Kiểm tra thông tin hoá đơn <b>[{model.Pattern};{model.Serial}-{model.InvKey}]</b> thất bại. Có lỗi phát sinh => {errMsg}", EnumProcessType.NonFormat, EnumMsgIcon.Error);
            }

            return Task.FromResult<ActionResult>(Json(new { status = true, message = response }, JsonRequestBehavior.AllowGet));
        }

        #endregion

        #region Sync Invs

        [ActionType(Type = EnumActionType.Edit)]
        [AjaxOnly]
        [HttpGet]
        public ActionResult SyncInvs()
        {
            var eInvAcc = _invAccCache.GetByUserName(User.UserName);
            if (eInvAcc == null)
            {
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{AppProcessor.Messagor.GetMessage("Err_InvAccount_Empty")}", EnumProcessType.NonFormat, EnumMsgIcon.Error)
                });
            }

            var searchModel = (SearchInvoiceModel)Session[$"InvPending_SearchInv_{User.UserName}"];
            var dataSearch = (BaseSearchModel)Session[$"InvPending_BaseSearch_{User.UserName}"];

            _invCache.GetPendingInvs(total: out int total, userName: User.UserName, managerUnions: searchModel.UnionIds, pattern: searchModel.Pattern, serials: searchModel.Serials, invTypes: searchModel.InvTypes, createdFrom: searchModel.CreatedFrom, createdTo: searchModel.CreatedTo, creators: searchModel.Creators, cusName: searchModel.CusName, cusCode: searchModel.CusCode, cusTaxCode: searchModel.CusTaxCode, search: dataSearch);

            ViewBag.ConfirmMessage =
                $"Bạn muốn thực hiện kiểm tra và đồng bộ thông tin cho <b>[{total}]</b> hoá đơn?";

            var modelConfirm = new ConfirmInvModel
            {
                CreatedOn = DateTime.Now,
                TotalInvs = total
            };

            return PartialView("_SyncInv", modelConfirm);
        }

        [ActionType(Type = EnumActionType.Edit)]
        [AjaxOnly]
        [HttpPost]
        public async Task<ActionResult> SyncInvs(ConfirmInvModel model)
        {
            var eInvAcc = _invAccCache.GetByUserName(User.UserName);
            if (eInvAcc == null)
            {
                return await Task.FromResult<ActionResult>(Json(new
                {
                    status = true,
                    message = CreateMessage($"{AppProcessor.Messagor.GetMessage("Err_InvAccount_Empty")}", EnumProcessType.NonFormat, EnumMsgIcon.Error)
                }));
            }

            var searchModel = (SearchInvoiceModel)Session[$"InvPending_SearchInv_{User.UserName}"];
            var dataSearch = (BaseSearchModel)Session[$"InvPending_BaseSearch_{User.UserName}"];

            var lstPendingInvs = _invCache.GetPendingInvs(total: out int total, userName: User.UserName, managerUnions: searchModel.UnionIds, pattern: searchModel.Pattern, serials: searchModel.Serials, invTypes: searchModel.InvTypes, createdFrom: searchModel.CreatedFrom, createdTo: searchModel.CreatedTo, creators: searchModel.Creators, cusName: searchModel.CusName, cusCode: searchModel.CusCode, cusTaxCode: searchModel.CusTaxCode, search: dataSearch);

            var syncBy = User.UserName;

            var tasks = lstPendingInvs.Select(pendingInv =>
                Task.Run(() =>
                {
                    try
                    {
                        string response;

                        var retAction = _invProvider.SyncInvoice(out var errMsg, pendingInv.InvKey, pendingInv.CreatedOn, _invServiceAccName, _invServiceAccPass, $"Thực hiện kiểm tra đồng bộ thông tin cho hoá đơn [{pendingInv.Pattern};{pendingInv.Serial}-{pendingInv.InvKey}]", syncBy);

                        if (retAction == 1)
                        {
                            response =
                                $"Hoàn thành kiểm tra và đã thực hiện đồng bộ thông tin hoá đơn <b>[{pendingInv.Pattern};{pendingInv.Serial}-{pendingInv.InvKey}]</b> thành công";
                        }
                        else if (retAction == 0)
                        {
                            response =
                                $"Hoàn thành kiểm tra hoá đơn <b>[{pendingInv.Pattern};{pendingInv.Serial}-{pendingInv.InvKey}]</b>. Không tồn tại trên hệ thống.";
                        }
                        else
                        {
                            AppProcessor.Logger.Message($"Sync Inv [{pendingInv.Pattern};{pendingInv.Serial}-{pendingInv.InvKey}]: Error => {errMsg}");

                            response =
                                $"Kiểm tra hoá đơn <b>[{pendingInv.Pattern};{pendingInv.Serial}-{pendingInv.InvKey}]</b> thất bại: {errMsg}";
                        }

                        AppProcessor.Notifider.PushNotifyToUser("sys", syncBy, response);
                    }
                    catch (Exception ex)
                    {
                        AppProcessor.Logger.Error(ex);
                    }
                })
            );

            await Task.WhenAll(tasks);


            return await Task.FromResult<ActionResult>(Json(new
            {
                status = true,
                message = CreateMessage(
                    $"Hệ thống đang thực hiện kiểm tra thông tin cho <b>[{total}]</b> hoá đơn. Vui lòng đợi.", EnumProcessType.NonFormat, EnumMsgIcon.Success)
            }, JsonRequestBehavior.AllowGet));
        }

        #endregion

        #region Rollback Inv

        [ActionType(Type = EnumActionType.Edit)]
        [AjaxOnly]
        [HttpGet]
        public ActionResult RollbackInv(string invKey)
        {
            var invModel = _invCache.GetByKey(invKey);
            if (invModel == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_invTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            ViewBag.ConfirmMessage =
                $"Bạn muốn thực hiện huỷ bỏ trạng thái chờ của hoá đơn <b>[{invModel.Pattern};{invModel.Serial}-{invModel.InvKey}]</b> ?";

            var modelConfirm = new ConfirmInvModel
            {
                InvKey = invModel.InvKey,
                InvId = invModel.InvId,
                InvNo = invModel.InvNo,
                Pattern = invModel.Pattern,
                Serial = invModel.Serial,
                CreatedOn = invModel.CreatedOn
            };

            return PartialView("_RollbackInv", modelConfirm);
        }

        [ActionType(Type = EnumActionType.Edit)]
        [AjaxOnly]
        [HttpPost]
        public Task<ActionResult> RollbackInv(ConfirmInvModel model)
        {
            string response;

            var eInvAcc = _invAccCache.GetByUserName(User.UserName);
            if (eInvAcc == null)
            {
                return Task.FromResult<ActionResult>(Json(new
                {
                    status = true,
                    message = CreateMessage($"{AppProcessor.Messagor.GetMessage("Err_InvAccount_Empty")}", EnumProcessType.NonFormat, EnumMsgIcon.Error)
                }));
            }

            var retAction = _invCache.Rollback(new InvStatusModel
            {
                InvKey = model.InvKey,
                Reason = "Huỷ trạng thái chờ của hoá đơn",
                SavedBy = User.UserName
            });

            if (retAction == 1)
                response = CreateMessage(
                    $"Huỷ trạng thái chờ của hoá đơn <b>[{model.Pattern};{model.Serial}-{model.InvKey}]</b> thành công", EnumProcessType.NonFormat, EnumMsgIcon.Success);
            else
                response = CreateMessage(
                    $"Huỷ trạng thái chờ của hoá đơn <b>[{model.Pattern};{model.Serial}-{model.InvKey}]</b> thất bại.", EnumProcessType.NonFormat, EnumMsgIcon.Error);

            return Task.FromResult<ActionResult>(Json(new { status = true, message = response }, JsonRequestBehavior.AllowGet));
        }

        #endregion

        #region Rollback Invs

        [ActionType(Type = EnumActionType.Edit)]
        [AjaxOnly]
        [HttpGet]
        public ActionResult RollbackInvs()
        {
            var eInvAcc = _invAccCache.GetByUserName(User.UserName);
            if (eInvAcc == null)
            {
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{AppProcessor.Messagor.GetMessage("Err_InvAccount_Empty")}", EnumProcessType.NonFormat, EnumMsgIcon.Error)
                });
            }

            var searchModel = (SearchInvoiceModel)Session[$"InvPending_SearchInv_{User.UserName}"];
            var dataSearch = (BaseSearchModel)Session[$"InvPending_BaseSearch_{User.UserName}"];

            _invCache.GetPendingInvs(total: out int total, userName: User.UserName, managerUnions: searchModel.UnionIds, pattern: searchModel.Pattern, serials: searchModel.Serials, invTypes: searchModel.InvTypes, createdFrom: searchModel.CreatedFrom, createdTo: searchModel.CreatedTo, creators: searchModel.Creators, cusName: searchModel.CusName, cusCode: searchModel.CusCode, cusTaxCode: searchModel.CusTaxCode, search: dataSearch);

            ViewBag.ConfirmMessage =
                $"Bạn muốn thực hiện huỷ bỏ trạng thái chờ của <b>[{total}]</b> hoá đơn?";

            var modelConfirm = new ConfirmInvModel
            {
                CreatedOn = DateTime.Now
            };

            return PartialView("_RollbackInv", modelConfirm);
        }

        [ActionType(Type = EnumActionType.Edit)]
        [AjaxOnly]
        [HttpPost]
        public Task<ActionResult> RollbackInvs(ConfirmInvModel model)
        {
            var eInvAcc = _invAccCache.GetByUserName(User.UserName);
            if (eInvAcc == null)
            {
                return Task.FromResult<ActionResult>(Json(new
                {
                    status = true,
                    message = CreateMessage($"{AppProcessor.Messagor.GetMessage("Err_InvAccount_Empty")}", EnumProcessType.NonFormat, EnumMsgIcon.Error)
                }));
            }

            var searchModel = (SearchInvoiceModel)Session[$"InvPending_SearchInv_{User.UserName}"];
            var dataSearch = (BaseSearchModel)Session[$"InvPending_BaseSearch_{User.UserName}"];

            var lstPendingInvs = _invCache.GetPendingInvs(total: out int total, userName: User.UserName, managerUnions: searchModel.UnionIds, pattern: searchModel.Pattern, serials: searchModel.Serials, invTypes: searchModel.InvTypes, createdFrom: searchModel.CreatedFrom, createdTo: searchModel.CreatedTo, creators: searchModel.Creators, cusName: searchModel.CusName, cusCode: searchModel.CusCode, cusTaxCode: searchModel.CusTaxCode, search: dataSearch);

            var queueSyncInvTasks = new Queue<Task>();
            var rollbackBy = User.UserName;

            foreach (var pendingInv in lstPendingInvs)
            {
                queueSyncInvTasks.Enqueue(new Task(() =>
                {
                    var retAction = _invCache.Rollback(new InvStatusModel
                    {
                        InvKey = pendingInv.InvKey,
                        Reason = "Huỷ trạng thái chờ của hoá đơn",
                        SavedBy = User.UserName
                    });

                    var response = retAction == 1 ? $"Huỷ trạng thái chờ của hoá đơn <b>[{pendingInv.Pattern};{pendingInv.Serial}-{pendingInv.InvKey}]</b> thành công" : $"Huỷ trạng thái chờ của hoá đơn <b>[{pendingInv.Pattern};{pendingInv.Serial}-{pendingInv.InvKey}]</b> thất bại.";

                    AppProcessor.Notifider.PushNotifyToUser("sys", rollbackBy, response);

                }));
            }

            Task.Run(() =>
            {
                try
                {
                    while (queueSyncInvTasks.Count > 0)
                    {
                        var taskCreateInv = queueSyncInvTasks.Dequeue();
                        taskCreateInv.Start();
                    }
                }
                catch (Exception ex)
                {
                    AppProcessor.Logger.Error(ex);
                }
            });

            return Task.FromResult<ActionResult>(Json(new
            {
                status = true,
                message = CreateMessage(
                    $"Hệ thống đang thực hiện huỷ trạng thái chờ của <b>[{total}]</b> hoá đơn. Vui lòng đợi.", EnumProcessType.NonFormat, EnumMsgIcon.Success)
            }, JsonRequestBehavior.AllowGet));
        }

        #endregion

        #region Republish Inv

        [ActionType(Type = EnumActionType.Edit)]
        [AjaxOnly]
        [HttpGet]
        public ActionResult RepublishInv(string invKey)
        {
            var invModel = _invCache.GetByKey(invKey);
            if (invModel == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_invTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            if (!invModel.CanRepublish)
                return Json(new
                {
                    status = true,
                    message = CreateMessage(
                        $"Không thể phát hành lại. Hoá đơn {invModel.Pattern} - {invModel.Serial} - {invModel.InvKey} không thuộc hợp đồng nào", EnumProcessType.NonFormat, EnumMsgIcon.Error)
                }, JsonRequestBehavior.AllowGet);

            ViewBag.ConfirmMessage =
                $"Bạn muốn thực hiện phát hành lại hoá đơn <b>[{invModel.Pattern};{invModel.Serial}-{invModel.InvKey}]</b> ?";

            var modelConfirm = new ConfirmInvModel
            {
                InvKey = invModel.InvKey,
                InvId = invModel.InvId,
                InvNo = invModel.InvNo,
                Pattern = invModel.Pattern,
                Serial = invModel.Serial,
                CreatedOn = invModel.CreatedOn
            };

            return PartialView("_RepublishInv", modelConfirm);
        }

        [ActionType(Type = EnumActionType.Edit)]
        [AjaxOnly]
        [HttpPost]
        public Task<ActionResult> RepublishInv(ConfirmInvModel model)
        {
            if (!ModelState.IsValidField("Reason"))
            {
                ViewBag.ConfirmMessage =
                    $"Bạn muốn thực hiện phát hành lại hoá đơn <b>[{model.Pattern};{model.Serial}-{model.InvKey}]</b> ?";

                return Task.FromResult<ActionResult>(PartialView("_RepublishInv", model));
            }

            var invModel = _invCache.GetByKey(model.InvKey);
            if (invModel == null)
                return Task.FromResult<ActionResult>(Json(new
                {
                    status = true,
                    message = CreateMessage($"{_invTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                }));

            var eInvAcc = _invAccCache.GetByUserName(User.UserName);
            if (eInvAcc == null)
            {
                return Task.FromResult<ActionResult>(Json(new
                {
                    status = true,
                    message = CreateMessage($"{AppProcessor.Messagor.GetMessage("Err_InvAccount_Empty")}", EnumProcessType.NonFormat, EnumMsgIcon.Error)
                }));
            }

            #region Generate Inv Model

            var cusContract = _contractCache.GetCus(invModel.ContractId);

            #region Cus Inv

            var cusInv = _invCusCache.GetByInvKey(invModel.InvKey);

            InvCustomerModel invCusInfo = new InvCustomerModel
            {
                Code = cusInv.CusCode,
                Name = cusInv.CusName,
                Address = cusInv.CusAddress,
                Phone = cusInv.CusPhone,
                TaxCode = cusInv.CusTaxCode,
                Email = cusContract.Email,
                RepresentPerson = cusContract.TypeCus == ConstsCusType.BUSINESS ? cusContract.RepresenterName : null,
                CusType = cusInv.TypeCus == ConstsCusType.BUSINESS ? "1" : "0"
            };

            #endregion

            #region Products Inv

            var productInv = _invProductCache.GetProductsViaKey(invModel.InvKey);

            #endregion

            #region Inv

            invModel.InvCusInfo = cusInv;
            invModel.InvProductInfo = productInv;
            var invInfo = GenInvInfo(invModel);

            #endregion

            #region Republish Inv

            string publishBy = User.UserName;
            Task.Run(() =>
            {
                try
                {
                    _invProvider.CreateInvoice(out var errMsg, invCusInfo, invInfo, eInvAcc.ElnvAccount, eInvAcc.ElnvACPassword, _invServiceAccName, _invServiceAccPass, invModel.Pattern, invModel.Serial, publishBy);

                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        StringBuilder logBuilder = new StringBuilder();
                        logBuilder.AppendLine();

                        logBuilder.AppendLine($"InvPending - RepublishInv - {model.Pattern};{model.Serial}-{model.InvKey} - InvKey [{invModel.InvKey}]");
                        logBuilder.AppendLine($"        => Error: {errMsg}");

                        AppProcessor.Logger.Message(logBuilder.ToString());

                        AppProcessor.Notifider.PushNotifyToUser("System", publishBy, $"Phát hành hoá đơn {model.Pattern};{model.Serial}-{model.InvKey} thất bại. Vui lòng kiểm tra lại");
                    }
                }
                catch (Exception ex)
                {
                    AppProcessor.Logger.Error(ex);

                    #region Thông báo người dùng

                    AppProcessor.Notifider.PushNotifyToUser("System", publishBy, $"Phát hành hoá đơn {model.Pattern};{model.Serial}-{model.InvKey} thất bại. Vui lòng kiểm tra lại");

                    #endregion
                }
            });

            #endregion

            #endregion

            string response = CreateMessage($"Đang thực hiện phát hành lại hoá đơn <b>[{model.Pattern};{model.Serial}-{model.InvNo}]</b>. Vui lòng đợi", EnumProcessType.NonFormat, EnumMsgIcon.Success);

            return Task.FromResult<ActionResult>(Json(new { status = true, message = response }, JsonRequestBehavior.AllowGet));
        }

        #endregion

        #region Republish Invs

        [ActionType(Type = EnumActionType.Edit)]
        [AjaxOnly]
        [HttpGet]
        public ActionResult RepublishInvs()
        {
            var eInvAcc = _invAccCache.GetByUserName(User.UserName);
            if (eInvAcc == null)
            {
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{AppProcessor.Messagor.GetMessage("Err_InvAccount_Empty")}", EnumProcessType.NonFormat, EnumMsgIcon.Error)
                });
            }

            var searchModel = (SearchInvoiceModel)Session[$"InvPending_SearchInv_{User.UserName}"];
            var dataSearch = (BaseSearchModel)Session[$"InvPending_BaseSearch_{User.UserName}"];

            _invCache.GetPendingInvs(total: out int total, userName: User.UserName, managerUnions: searchModel.UnionIds, pattern: searchModel.Pattern, serials: searchModel.Serials, invTypes: searchModel.InvTypes, createdFrom: searchModel.CreatedFrom, createdTo: searchModel.CreatedTo, creators: searchModel.Creators, cusName: searchModel.CusName, cusCode: searchModel.CusCode, cusTaxCode: searchModel.CusTaxCode, search: dataSearch);

            ViewBag.ConfirmMessage = $"Bạn muốn thực hiện phát hành lại <b>[{total}]</b> hoá đơn?";

            var modelConfirm = new ConfirmInvModel
            {
                CreatedOn = DateTime.Now
            };

            return PartialView("_RepublishInv", modelConfirm);
        }

        [ActionType(Type = EnumActionType.Edit)]
        [AjaxOnly]
        [HttpPost]
        public Task<ActionResult> RepublishInvs(ConfirmInvModel model)
        {
            var searchModel = (SearchInvoiceModel)Session[$"InvPending_SearchInv_{User.UserName}"];
            var dataSearch = (BaseSearchModel)Session[$"InvPending_BaseSearch_{User.UserName}"];

            var lstPendingInvs = _invCache.GetPendingInvs(total: out int total, userName: User.UserName, managerUnions: searchModel.UnionIds, pattern: searchModel.Pattern, serials: searchModel.Serials, invTypes: searchModel.InvTypes, createdFrom: searchModel.CreatedFrom, createdTo: searchModel.CreatedTo, creators: searchModel.Creators, cusName: searchModel.CusName, cusCode: searchModel.CusCode, cusTaxCode: searchModel.CusTaxCode, search: dataSearch);

            if (!ModelState.IsValidField("Reason"))
            {
                ViewBag.ConfirmMessage =
                    $"Bạn muốn thực hiện phát hành lại <b>[{total}]</b> hoá đơn?";

                return Task.FromResult<ActionResult>(PartialView("_RepublishInv", model));
            }

            var eInvAcc = _invAccCache.GetByUserName(User.UserName);
            if (eInvAcc == null)
            {
                return Task.FromResult<ActionResult>(Json(new
                {
                    status = true,
                    message = CreateMessage($"{AppProcessor.Messagor.GetMessage("Err_InvAccount_Empty")}", EnumProcessType.NonFormat, EnumMsgIcon.Error)
                }));
            }

            var queueSyncInvTasks = new Queue<Task>();
            var publishBy = User.UserName;

            foreach (var pendingInv in lstPendingInvs)
            {
                var invModel = _invCache.GetByKey(pendingInv.InvKey);
                if (invModel == null)
                {
                    AppProcessor.Notifider.PushNotifyToUser("System", publishBy, $"Hoá đơn {pendingInv.Pattern};{pendingInv.Serial}-{pendingInv.InvKey} không tồn tại. Vui lòng kiểm tra lại");
                }
                else
                {
                    if (!invModel.CanRepublish)
                    {
                        AppProcessor.Notifider.PushNotifyToUser("System", publishBy,
                            $"Hoá đơn {invModel.Pattern} - {invModel.Serial} - {invModel.InvKey} không thuộc hợp đồng nào. Không thể phát hành lại. ");
                    }
                    else
                    {
                        queueSyncInvTasks.Enqueue(new Task(() =>
                        {
                            try
                            {
                                #region Generate Inv Model

                                var cusContract = _contractCache.GetCus(invModel.ContractId);

                                #region Cus Inv

                                var cusInv = _invCusCache.GetByInvKey(invModel.InvKey);

                                InvCustomerModel invCusInfo = new InvCustomerModel
                                {
                                    Code = cusInv.CusCode,
                                    Name = cusInv.CusName,
                                    Address = cusInv.CusAddress,
                                    Phone = cusInv.CusPhone,
                                    TaxCode = cusInv.CusTaxCode,
                                    Email = cusContract.Email,
                                    RepresentPerson = cusContract.TypeCus == ConstsCusType.BUSINESS
                                        ? cusContract.RepresenterName
                                        : null,
                                    CusType = cusInv.TypeCus == ConstsCusType.BUSINESS ? "1" : "0"
                                };

                                #endregion

                                #region Products Inv

                                var productInv = _invProductCache.GetProductsViaKey(invModel.InvKey);

                                #endregion

                                #region Inv

                                invModel.InvCusInfo = cusInv;
                                invModel.InvProductInfo = productInv;
                                var invInfo = GenInvInfo(invModel);

                                #endregion

                                #region Republish Inv

                                _invProvider.CreateInvoice(out var errMsg, invCusInfo, invInfo, eInvAcc.ElnvAccount,
                                    eInvAcc.ElnvACPassword, _invServiceAccName, _invServiceAccPass, invModel.Pattern,
                                    invModel.Serial, publishBy);

                                if (!string.IsNullOrEmpty(errMsg))
                                {
                                    StringBuilder logBuilder = new StringBuilder();
                                    logBuilder.AppendLine();

                                    logBuilder.AppendLine(
                                        $"InvPending - RepublishInv - {model.Pattern};{model.Serial}-{model.InvKey} - InvKey [{invModel.InvKey}]");
                                    logBuilder.AppendLine($"        => Error: {errMsg}");

                                    AppProcessor.Logger.Message(logBuilder.ToString());

                                    AppProcessor.Notifider.PushNotifyToUser("System", publishBy,
                                        $"Phát hành hoá đơn {model.Pattern};{model.Serial}-{model.InvKey} thất bại. Vui lòng kiểm tra lại");
                                }


                                #endregion

                                #endregion
                            }
                            catch (Exception ex)
                            {
                                AppProcessor.Logger.Error(ex);

                                #region Thông báo người dùng

                                AppProcessor.Notifider.PushNotifyToUser("System", publishBy,
                                    $"Phát hành hoá đơn {model.Pattern};{model.Serial}-{model.InvKey} thất bại. Vui lòng kiểm tra lại");

                                #endregion
                            }
                        }));
                    }
                }

            }

            Task.Run(() =>
            {
                try
                {
                    while (queueSyncInvTasks.Count > 0)
                    {
                        var taskCreateInv = queueSyncInvTasks.Dequeue();
                        taskCreateInv.Start();
                    }
                }
                catch (Exception ex)
                {
                    AppProcessor.Logger.Error(ex);
                }
            });

            string response = CreateMessage($"Đang thực hiện phát hành lại <b>[{total}]</b> hoá đơn. Vui lòng đợi", EnumProcessType.NonFormat, EnumMsgIcon.Success);

            return Task.FromResult<ActionResult>(Json(new { status = true, message = response }, JsonRequestBehavior.AllowGet));
        }

        #endregion

        #region Extend Functions

        private InvInv GenInvInfo(MajorInvModel inv)
        {
            InvInv invInfo = new InvInv
            {
                ContractId = inv.ContractId,
                FKey = inv.InvKey,
                Invoice = new InvInvoice
                {
                    Buyer = inv.InvCusInfo.TypeCus == ConstsCusType.BUSINESS ? string.Empty : inv.InvCusInfo.CusName,
                    CusName = inv.InvCusInfo.TypeCus == ConstsCusType.BUSINESS ? inv.InvCusInfo.CusName : string.Empty,
                    CusAddress = inv.InvCusInfo.CusAddress,
                    CusCode = inv.InvCusInfo.CusCode,
                    CusPhone = inv.InvCusInfo.CusPhone,
                    CusTaxCode = inv.InvCusInfo.CusTaxCode,

                    CusType = inv.InvCusInfo.TypeCus == ConstsCusType.BUSINESS ? "1" : "0",

                    IsPayed = true,

                    CurrencyUnit = inv.CurrencyUnit,
                    PaymentMethod = inv.PaymentMethod,
                    PaymentStatus = $"{(int)EnumInvPaymentStatus.Paid}",

                    Total = $"{inv.Total}",
                    TaxRate = $"{inv.TaxRate}",
                    TaxAmount = $"{inv.TaxAmount}",
                    Amount = $"{inv.Amount}",
                    AmountInWords = inv.AmountInWord,

                    KindOfService = inv.KindOfService,
                    DiscountAmount = $"{inv.DiscountAmount}",
                    Extra9 = $"{inv.TaxRate}",
                    Extra10 = $"{inv.DiscountAmount}",

                    VatAmount0 = $"{inv.TaxAmount}",
                    GrossValue0 = $"{inv.Total}",
                    VatAmount5 = $"{inv.TaxAmount}",
                    GrossValue5 = $"{inv.Total}",
                    VatAmount8 = $"{inv.TaxAmount}",
                    GrossValue8 = $"{inv.Total}",
                    VatAmount10 = $"{inv.TaxAmount}",
                    GrossValue10 = $"{inv.Total}",

                    Products = new InvProducts
                    {
                        ListProducts = new List<InvProduct>
                        {
                            new InvProduct
                            {
                                ProdId = inv.InvProductInfo.ProductId,
                                ProdName = inv.InvProductInfo.ProductName,
                                ProdPrice = string.Empty,
                                ProdUnit = " ",
                                Price = inv.Total,
                                Amount =  $"{inv.InvProductInfo.Amount}",
                                Total = $"{inv.InvProductInfo.Amount}",
                                ProdQuantity = string.Empty,
                                TaxAmount = $"{inv.TaxAmount}",
                                IsSum = $"{inv.InvProductInfo.Issum ?? 1}",
                                TaxRate = $"{inv.InvProductInfo.TaxRate}"
                            }
                        }
                    }
                }
            };

            return invInfo;
        }

        #endregion
    }
}