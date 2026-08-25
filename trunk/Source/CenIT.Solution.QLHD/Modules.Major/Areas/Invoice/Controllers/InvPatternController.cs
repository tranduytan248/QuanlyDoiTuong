using Core.Inv.Caches;
using Core.Inv.Models;
using System;
using System.Web.Mvc;
using Core.Inv.Providers;
using TSFramework.App.Attributes;
using TSFramework.App.Models;
using TSFramework.App.Processors;
using TSFramework.Core.Enums;
using Cores.Sys.Caches.Sys;
using System.Collections.Generic;
using System.Linq;
using Cores.Base.Apps;
using Modules.Major.Areas.Invoice.Data;
using TSFramework.Core.Utils;

namespace Modules.Major.Areas.Invoice.Controllers
{
    public class InvPatternController : AppController
    {
        private readonly MajorInvPatternCache _invPatternCache = new MajorInvPatternCache();
        private readonly SysElnvAccountCache _invAccCache = new SysElnvAccountCache();

        private readonly SysConfigCache _sysConfigCache = new SysConfigCache();

        private readonly string _funcName = AppProcessor.Messagor.GetMessage("InvPattern_Title");

        private const string CONFIG_INV_HOST_INV_SERVICE = "CONFIG_INV_HOST_INV_SERVICE";
        private const string CONFIG_INV_SERVICE_ACCOUNT_NAME = "CONFIG_INV_SERVICE_ACCOUNT_NAME";
        private const string CONFIG_INV_SERVICE_ACCOUNT_PASS = "CONFIG_INV_SERVICE_ACCOUNT_PASS";

        private readonly InvProvider _invProvider;

        private readonly string _serviceAccName = "";
        private readonly string _serviceAccPass = "";

        public InvPatternController()
        {
            string hostInvService = "";
            var configModel = _sysConfigCache.GetViaKey(CONFIG_INV_HOST_INV_SERVICE);
            if (configModel != null && !string.IsNullOrEmpty(configModel.ConfigValue))
            {
                hostInvService = configModel.ConfigValue;
            }

            configModel = _sysConfigCache.GetViaKey(CONFIG_INV_SERVICE_ACCOUNT_NAME);
            if (configModel != null && !string.IsNullOrEmpty(configModel.ConfigValue))
            {
                _serviceAccName = configModel.ConfigValue;
            }

            configModel = _sysConfigCache.GetViaKey(CONFIG_INV_SERVICE_ACCOUNT_PASS);
            if (configModel != null && !string.IsNullOrEmpty(configModel.ConfigValue))
            {
                _serviceAccPass = configModel.ConfigValue;
            }

            _invProvider = new InvProvider($"{hostInvService}PortalService.asmx", $"{hostInvService}BusinessService.asmx", $"{hostInvService}PublishService.asmx");
        }

        #region Main Actions

        // GET: 
        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        public ActionResult Index()
        {
            var eInvAcc = _invAccCache.GetByUserName(User.UserName);

            return View(new CheckingInvModel
            {
                HasNotSysInvAccount = eInvAcc == null,
                IsInvServiceAccountIncorrect = !_invProvider.IsCorrectUser(_serviceAccName, _serviceAccPass)
            });
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult Get()
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
            var data = _invPatternCache.Get(out int total, dataSearch);
            data.ForEach(p =>
            {
                var lstPatternSerials = Session[$"InvPatternSerial-{User.UserName}-{p.Pattern}"] as List<InvPatternSerialInfoModel>;
                lstPatternSerials = lstPatternSerials ?? GetListPatterns(p.Pattern);
                Session[$"InvPatternSerial-{User.UserName}-{p.Pattern}"] = lstPatternSerials;
                p.TotalRemainingInv =
                    lstPatternSerials?.FirstOrDefault(ps => ps.Serial == p.Serial)?.TotalRemainingInv ?? 0;
            });
            var result = Json(
                new { draw = Convert.ToInt32(draw), recordsTotal = total, recordsFiltered = total, data },
                JsonRequestBehavior.AllowGet);
            return result;
        }

        [AjaxOnly]
        [ActionType(Type = EnumActionType.Add)]
        [HttpGet]
        public ActionResult Add()
        {
            var model = new MajorInvPatternModel();
            return PartialView("_Add", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Add)]
        public ActionResult Add(MajorInvPatternModel model)
        {
            if (!ModelState.IsValid) return PartialView("_InvPattern", model);
            string response;
            var configId = _invPatternCache.Save(new MajorInvPatternModel
            {
                Pattern = model.Pattern,
                Serial = model.Serial,
                IsActive = model.IsActive,
                UpdatedBy = User.UserName
            });

            if (configId == 0)
                response = CreateMessage($"{_funcName} [{model.Pattern} - {model.Pattern}]", EnumProcessType.Add, EnumMsgIcon.Success);
            else if (configId == -9)
                response = CreateMessage($"{_funcName} [{model.Pattern} - {model.Pattern}]", EnumProcessType.DataExisted, EnumMsgIcon.Error);
            else
            {
                response = CreateMessage($"{_funcName} [{model.Pattern} - {model.Pattern}]", EnumProcessType.Add, EnumMsgIcon.Success);
            }

            return Json(new
            {
                status = true,
                message = response
            }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult Edit(Guid? id)
        {
            var model = _invPatternCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_funcName}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            return PartialView("_Edit", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult Edit(MajorInvPatternModel model)
        {
            if (!ModelState.IsValid) return PartialView("_InvPattern", model);

            var configId = _invPatternCache.Save(new MajorInvPatternModel
            {
                PatternId = model.PatternId,
                Pattern = model.Pattern,
                Serial = model.Serial,
                IsActive = model.IsActive,
                UpdatedBy = User.UserName
            });

            string response;
            if (configId == 0)
                response = CreateMessage($"{_funcName} [{model.Pattern} - {model.Pattern}]", EnumProcessType.Add, EnumMsgIcon.Success);
            else if (configId == -9)
                response = CreateMessage($"{_funcName} [{model.Pattern} - {model.Pattern}]", EnumProcessType.DataExisted, EnumMsgIcon.Error);
            else
            {
                response = CreateMessage($"{_funcName} [{model.Pattern} - {model.Pattern}]", EnumProcessType.Add, EnumMsgIcon.Success);
            }

            return Json(new
            {
                status = true,
                message = response
            }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(Guid? id)
        {
            var model = _invPatternCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_funcName}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Delete"),
                $"<b>{_funcName} [{model.Pattern} - {model.Pattern}]</b>");

            return PartialView("_Delete", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(MajorInvPatternModel model)
        {
            var invPattern = _invPatternCache.GetById(model.PatternId);
            invPattern.UpdatedBy = User.UserName;

            var deleted = _invPatternCache.Delete(invPattern);

            var response = CreateMessage($"{_funcName} [{model.Pattern} - {model.Pattern}]", EnumProcessType.Delete,
                deleted ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response });
        }

        #endregion

        #region Ajax Actions

        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetInfoViaPattern(string pattern, string serial)
        {
            if (string.IsNullOrEmpty(pattern)) return Json(new { data = "" });

            var lstInvPatternSerial = Session[$"InvPatternSerial-{User.UserName}-{pattern}"] as List<InvPatternSerialInfoModel>;
            if (lstInvPatternSerial == null)
            {
                lstInvPatternSerial = GetListPatterns(pattern);
                Session[$"InvPatternSerial-{User.UserName}-{pattern}"] = lstInvPatternSerial;
            }

            var patternInfo = lstInvPatternSerial?.FirstOrDefault(i => i.Serial == serial);
            if (patternInfo == null) return Json(new { data = "" });
            return Json(new { data = patternInfo.AsDictionary() });
        }

        private List<InvPatternSerialInfoModel> GetListPatterns(string pattern)
        {
            var lstInvPatternSerial = new List<InvPatternSerialInfoModel>();
            if (string.IsNullOrEmpty(pattern)) return lstInvPatternSerial;

            var resServiceInfo = _invProvider.GetSerialByPattern(out var sErrMsg, _serviceAccName, _serviceAccPass, pattern);
            if (!string.IsNullOrEmpty(sErrMsg) || string.IsNullOrEmpty(resServiceInfo)) return null;

            resServiceInfo.Split(';').ToList().ForEach(i =>
            {
                //9 Giá trị
                //1: STT
                //2: Serial
                //3: TotalInv
                //4: InvNoFrom
                //5: InvNoTo
                //6: CurrentInvNo
                //7: TotalRemainingInv
                //8: BeginUsedFrom
                //9: Status: 1-Chưa sử dụng; 2-Đang sử dụng

                var lstInfo =
                    _invProvider.GetValueFromStringBaseOnTemplate(
                        "(.*?)-(.*?)-(.*?)-(.*?)-(.*?)-(.*?)-(.*?)-(.*?)-(.*?)", i);
                lstInvPatternSerial.Add(new InvPatternSerialInfoModel
                {
                    Serial = lstInfo[1],
                    TotalInv = int.Parse(lstInfo[2] ?? "0"),
                    InvNoFrom = lstInfo[3],
                    InvNoTo = lstInfo[4],
                    CurrentInvNo = lstInfo[5],
                    TotalRemainingInv = int.Parse(lstInfo[6] ?? "0"),
                    BeginUsedFrom = lstInfo[7],
                });
            });

            return lstInvPatternSerial;
        }

        #endregion

    }
}