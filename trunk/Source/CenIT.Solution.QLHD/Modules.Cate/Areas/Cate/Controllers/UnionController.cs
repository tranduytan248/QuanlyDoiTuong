using Cores.Cate.Caches;
using Cores.Cate.Enum;
using Cores.Cate.Models;

using Cores.Sys.Caches.Sys;
using Modules.Cate.Areas.Cate.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Cores.Base.Apps;
using TSFramework.App.Attributes;
using TSFramework.App.Models;
using TSFramework.App.Processors;
using TSFramework.Core.Enums;
using TSFramework.Core.Helpers;

namespace Modules.Cate.Areas.Cate.Controllers
{
    public class UnionController : AppController
    {
        private readonly CateUnionCache _unionCache = new CateUnionCache();
        private readonly CatePositionCache _positionCache = new CatePositionCache();
        private readonly SysUserCache _userCache = new SysUserCache();

        private readonly string _unionTitle = AppProcessor.Messagor.GetMessage("Union_Title");
        private readonly string _memberTitle = AppProcessor.Messagor.GetMessage("Union_Member_Title");
        private readonly string _userTitle = AppProcessor.Messagor.GetMessage("User_Title");

        // GET: Cate/Union
        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        public ActionResult Index()
        {
            var searchModel = new SearchUnionModel
            {
                ListUnions = _unionCache.GetAll()
                    .Where(u => u.IsActive)
                    .OrderBy(u => u.TypeUnion)
                    .Select(u => new SelectListItem
                    {
                        Text = u.UnionName,
                        Value = u.UnionId.ToString(),
                        Group = new SelectListGroup { Name = u.BelongUnionName }
                    }).ToList()
            };

            return View("IndexTreeGrid",searchModel);
        }

        #region TreeGrid

        [ActionType(Type = EnumActionType.View)]
        public ActionResult TreeGrid(string belongUnions, string typeUnions)
        {
            var dataUnions = _unionCache.GetAll(null, belongUnions, typeUnions);
            List<CateUnionModel> lstUnions = new List<CateUnionModel>();

            var rootUnion = dataUnions.FirstOrDefault(u => u.BelongUnion == null);
            if (rootUnion != null)
            {
                rootUnion.ListChildrens = ChilrenUnionViaId(rootUnion.UnionId, dataUnions);
                lstUnions.Add(rootUnion);
            }

            return View("_TreeGrid", lstUnions);
        }

        [ActionType(Type = EnumActionType.View)]
        public ActionResult GetDataUnions(string belongUnions = null, string typeUnions = null)
        {
            belongUnions = string.IsNullOrEmpty(belongUnions) ? null : belongUnions;
            typeUnions = string.IsNullOrEmpty(typeUnions) ? null : typeUnions;
            var dataUnions = _unionCache.GetAll(null, belongUnions, typeUnions);
            List<CateUnionModel> lstUnions = new List<CateUnionModel>();

            var rootUnions = dataUnions.Where(u => belongUnions == null || belongUnions.Contains(u.BelongUnion?.ToString())).ToList();
            rootUnions.ForEach(u => 
            {
                u.ListChildrens = ChilrenUnionViaId(u.UnionId, dataUnions);
                lstUnions.Add(u);
            });

            return Json(lstUnions);
        }

        private List<CateUnionModel> ChilrenUnionViaId(Guid? childUnion, List<CateUnionModel> lstUnions)
        {
            var lstChilds = lstUnions.Where(u => u.BelongUnion == childUnion).ToList();
            lstChilds.ForEach(c =>
            {
                c.ListChildrens = ChilrenUnionViaId(c.UnionId, lstUnions);
            });
            return lstChilds;
        }

        #endregion

        #region Main Actions

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult Get(SearchUnionModel searchModel)
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

            var data = _unionCache.Get(userName: null, belongUnions: searchModel.BelongUnions, typeUnions: searchModel.TypeUnions, out var total, dataSearch);
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
            var model = new CateUnionModel
            {
                ListUnions = _unionCache.GetAll().Where(u => u.IsActive).Select(u => new SelectListItem
                {
                    Text = u.UnionName,
                    Value = u.UnionId.ToString(),
                    Group = new SelectListGroup { Name = u.BelongUnionName }
                }).ToList()
            };

            return PartialView("_Add", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Add)]
        [ValidateInput(false)]
        public ActionResult Add(CateUnionModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ListUnions = _unionCache.GetAll().Where(u => u.IsActive).Select(u => new SelectListItem
                {
                    Text = u.UnionName,
                    Value = u.UnionId.ToString(),
                    Group = new SelectListGroup { Name = u.BelongUnionName }
                }).ToList();

                return PartialView("_Union", model);
            }

            string response;
            var unionId = _unionCache.Save(new CateUnionModel
            {
                UnionId = Guid.Empty,
                UnionName = model.UnionName,
                UnionCode = model.UnionCode,
                TypeUnion = model.TypeUnion,
                TypeUnionName = model.TypeUnionName,
                BelongUnionName = model.BelongUnionName,
                BelongUnion = model.BelongUnion,
                Note = model.Note,
                Reason = "Thêm mới",
                UpdatedBy = User.UserName
            });
            if (unionId == 0)
                response = CreateMessage($"{_unionTitle} [{model.UnionName} - {model.BelongUnionName}]",
                    EnumProcessType.Add,
                    EnumMsgIcon.Error);
            else if (unionId == -9)
                response = CreateMessage($"{_unionTitle} [{model.UnionName} - {model.BelongUnionName}]",
                    EnumProcessType.DataExisted,
                    EnumMsgIcon.Error);
            else
                response = CreateMessage($"{_unionTitle} [{model.UnionName} - {model.BelongUnionName}]",
                    EnumProcessType.Add,
                    EnumMsgIcon.Success);
            return Json(new { status = true, message = response }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult Edit(Guid? id)
        {
            var model = _unionCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_unionTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            model.ListUnions = _unionCache.GetAll().Where(u => u.IsActive).Select(u => new SelectListItem
            {
                Text = u.UnionName,
                Value = u.UnionId.ToString(),
                Group = new SelectListGroup { Name = u.BelongUnionName }
            }).ToList();

            return PartialView("_Edit", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionType(Type = EnumActionType.Edit)]
        [ValidateInput(false)]
        public ActionResult Edit(CateUnionModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ListUnions = _unionCache.GetAll().Where(u => u.IsActive).Select(u => new SelectListItem
                {
                    Text = u.UnionName,
                    Value = u.UnionId.ToString(),
                    Group = new SelectListGroup { Name = u.BelongUnionName }
                }).ToList();

                return PartialView("_Union", model);
            }

            string response;
            var unionId = _unionCache.Save(new CateUnionModel
            {
                UnionId = model.UnionId,
                UnionName = model.UnionName,
                UnionCode = model.UnionCode,
                TypeUnion = model.TypeUnion,
                TypeUnionName = model.TypeUnionName,
                BelongUnionName = model.BelongUnionName,
                BelongUnion = model.BelongUnion,
                Note = model.Note,
                Reason = model.Reason,
                UpdatedBy = User.UserName
            });
            if (unionId == 0)
                response = CreateMessage($"{_unionTitle} [{model.UnionName} - {model.BelongUnionName}]",
                    EnumProcessType.Edit,
                    EnumMsgIcon.Error);
            else if (unionId == -9)
                response = CreateMessage($"{_unionTitle} [{model.UnionName} - {model.BelongUnionName}]",
                    EnumProcessType.DataExisted,
                    EnumMsgIcon.Error
                );
            else
                response = CreateMessage($"{_unionTitle} [{model.UnionName} - {model.BelongUnionName}]",
                    EnumProcessType.Edit,
                    EnumMsgIcon.Success
                );
            return Json(new { status = true, message = response }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(Guid? id)
        {
            var model = _unionCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_unionTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Delete"),
                $"<b>{_unionTitle} [{model.UnionName} - {model.BelongUnionName}]</b>");
            return PartialView("_Delete", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(CateUnionModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_DelBody", model);
            }

            model.UpdatedBy = User.UserName;
            var deleted = _unionCache.Delete(model);

            var response = CreateMessage($"{_unionTitle} [{model.UnionName} - {model.BelongUnionName}]",
                EnumProcessType.Delete, deleted ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response });
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult ToggleStatus(Guid? id)
        {
            var model = _unionCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_unionTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            ViewBag.Title = string.Format(model.IsActive ? AppProcessor.Messagor.GetMessage("Modal_Title_Deactive") : AppProcessor.Messagor.GetMessage("Modal_Title_ReActive"), $"<b>{_unionTitle} [{model.UnionName} - {model.BelongUnionName}]</b>");
            model.Reason = string.Format(model.IsActive ? AppProcessor.Messagor.GetMessage("Modal_Title_Deactive") : AppProcessor.Messagor.GetMessage("Modal_Title_ReActive"), $"{_unionTitle} [{model.UnionName} - {model.BelongUnionName}]");

            ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Action"), ViewBag.Title);
            return PartialView("_ToggleStatus", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult ToggleStatus(CateUnionModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_ToggleStatusBody", model);
            }
            var infoAction =
                string.Format(
                    model.IsActive
                        ? AppProcessor.Messagor.GetMessage("Modal_Title_Deactive")
                        : AppProcessor.Messagor.GetMessage("Modal_Title_ReActive"),
                    $"{_unionTitle} [{model.UnionName} - {model.BelongUnionName}]");

            model.UpdatedBy = User.UserName;
            model.IsActive = !model.IsActive;
            var isSuccess = _unionCache.ToggleStatus(model);

            var response = CreateMessage(infoAction, EnumProcessType.NonFormat, isSuccess ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response });
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult Info(Guid? id)
        {
            var model = _unionCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_unionTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            return PartialView("_Info", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Add)]
        [ValidateInput(false)]
        public ActionResult Info(Guid? unionId, string unionName)
        {
            NameValueCollection formData = new NameValueCollection(Request.Form);
            var jsonFormData = JsonConvert.SerializeObject(formData.AllKeys.ToDictionary(k => k, k => formData[k]));

            string response;
            var retSave = _unionCache.SaveInfo(new CateUnionModel
            {
                UnionId = unionId,
                UnionInfo = jsonFormData,
                UpdatedBy = User.UserName
            });

            if (retSave == 0)
                response = CreateMessage($"{_unionTitle} [{unionName}]",
                    EnumProcessType.Edit,
                    EnumMsgIcon.Error);
            else if (retSave == -9)
                response = CreateMessage($"{_unionTitle} [{unionName}]",
                    EnumProcessType.DataExisted,
                    EnumMsgIcon.Error);
            else
                response = CreateMessage($"{_unionTitle} [{unionName}]",
                    EnumProcessType.Edit,
                    EnumMsgIcon.Success);
            return Json(new { status = true, message = response }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Members

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult Members(Guid? id)
        {
            var model = _unionCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_unionTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            return PartialView("_Members", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult GetMembers(Guid unionId)
        {
            var lstMembers = _unionCache.GetMembers(unionId);
            lstMembers.ForEach(m =>
            {
                m.PermitName = AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription((EnumUnionMemberPermit)m.Permit));
            });

            int total = lstMembers.Count;
            var result = Json(
                new { draw = 0, recordsTotal = total, recordsFiltered = total, data = lstMembers },
                JsonRequestBehavior.AllowGet);
            return result;
        }

        [AjaxOnly]
        [ActionType(Type = EnumActionType.Add)]
        [HttpGet]
        public ActionResult AddMember(Guid unionId)
        {
            var unionModel = _unionCache.GetById(unionId);
            if (unionModel == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_unionTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var membersBelongUnion = _unionCache.GetMembers(unionId);

            var model = new CateUnionMemberModel
            {
                ListUsers = _userCache.GetAll()
                    .Where(u => u.IsActive
                                && membersBelongUnion.All(em => em.UserName != u.UserName)
                                && _unionCache.GetMemberByKey(null, u.UserName) == null)
                    .OrderBy(u => u.FullName)
                    .Select(u => new ListItem
                    {
                        Text = u.FullName,
                        Value = u.UserName,
                    }).ToList(),
                UnionId = unionId,
                UnionName = unionModel.UnionName,
                ListPositions = _positionCache.GetAll()
                    .Select(u => new ListItem
                    {
                        Text = u.PositionName,
                        Value = u.PositionID.ToString()
                    }).ToList()
            };

            return PartialView("_AddMember", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Add)]
        [ValidateInput(false)]
        public ActionResult AddMember(CateUnionMemberModel model)
        {
            if (!ModelState.IsValid)
            {
                var membersBelongUnion = _unionCache.GetMembers(model.UnionId);
                model.ListUsers = _userCache.GetAll()
                    .Where(u => u.IsActive
                                && membersBelongUnion.All(em => em.UserName != u.UserName)
                                && _unionCache.GetMemberByKey(null, u.UserName) == null)
                    .OrderBy(u => u.FullName)
                    .Select(u => new ListItem
                    {
                        Text = u.FullName,
                        Value = u.UserName,
                    }).ToList();
                model.ListPositions = _positionCache.GetAll()
                    .Select(u => new ListItem
                    {
                        Text = u.PositionName,
                        Value = u.PositionID.ToString()
                    }).ToList();

                return PartialView("_Member", model);
            }

            string response;

            var retSave = _unionCache.SaveMember(new CateUnionMemberModel
            {
                UnionId = model.UnionId,
                UserName = model.UserName,
                PositionId = model.PositionId,
                Permit = model.Permit,
                UpdatedBy = User.UserName
            });

            if (retSave == 0)
                response = CreateMessage($"{_memberTitle} [{model.FullName}]", EnumProcessType.Add, EnumMsgIcon.Error);
            else if (retSave == -9)
                response = CreateMessage($"{_memberTitle} [{model.FullName}]", EnumProcessType.DataExisted, EnumMsgIcon.Error);
            else
                response = CreateMessage($"{_memberTitle} [{model.FullName}]", EnumProcessType.Add, EnumMsgIcon.Success);
            return Json(new { status = true, message = response }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [ActionType(Type = EnumActionType.Add)]
        [HttpGet]
        public ActionResult EditMember(int? userId)
        {
            var userModel = _userCache.GetById(userId ?? 0);
            if (userModel == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_userTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var unionMemberModel = _unionCache.GetMemberByKey(userModel.UserName);

            var model = new CateUnionMemberModel
            {
                ListUnions = _unionCache.GetAll().Where(u => u.IsActive).Select(u => new SelectListItem
                {
                    Text = u.UnionName,
                    Value = u.UnionId.ToString(),
                    Group = new SelectListGroup { Name = u.BelongUnionName }
                }).ToList(),

                UnionId = unionMemberModel?.UnionId,
                UnionName = unionMemberModel?.UnionName,
                UserName = userModel.UserName,
                FullName = userModel.FullName,
                Email = userModel.Email,
                ListPositions = _positionCache.GetAll()
                .Select(u => new ListItem
                {
                    Text = u.PositionName,
                    Value = u.PositionID.ToString()
                }).ToList(),
                PositionId = unionMemberModel?.PositionId,
                PositionName = unionMemberModel?.PositionName,
                CanEdit = true
            };
            return PartialView("_EditMember", model);

        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionType(Type = EnumActionType.Edit)]
        [ValidateInput(false)]
        public ActionResult EditMember(CateUnionMemberModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ListUnions = _unionCache.GetAll().Where(u => u.IsActive).Select(u => new SelectListItem
                {
                    Text = u.UnionName,
                    Value = u.UnionId.ToString(),
                    Group = new SelectListGroup { Name = u.BelongUnionName }
                }).ToList();
                model.ListPositions = _positionCache.GetAll()
                    .Select(u => new ListItem
                    {
                        Text = u.PositionName,
                        Value = u.PositionID.ToString()
                    }).ToList();

                return PartialView("_Member", model);
            }

            string response;
            var retSave = _unionCache.SaveMember(new CateUnionMemberModel
            {
                UnionId = model.UnionId,
                UserName = model.UserName,
                PositionId = model.PositionId,
                Permit = model.Permit,
                UpdatedBy = User.UserName
            });
            if (retSave == 0)
                response = CreateMessage($"{_unionTitle} [{model.UnionName}] - [{model.FullName}]", EnumProcessType.Edit, EnumMsgIcon.Error);
            else if (retSave == -9)
                response = CreateMessage($"{_unionTitle} [{model.UnionName}] - [{model.FullName}]", EnumProcessType.DataExisted, EnumMsgIcon.Error);
            else
                response = CreateMessage($"{_unionTitle} [{model.UnionName}] - [{model.FullName}]", EnumProcessType.Edit, EnumMsgIcon.Success);
            return Json(new { status = true, message = response }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult DeleteMember(Guid unionId, string userName)
        {
            var model = _unionCache.GetMemberByKey(unionId, userName);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_memberTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Delete"),
                $"<b>{_memberTitle} [{model.FullName}]</b>");
            return PartialView("_DeleteMember", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult DeleteMember(CateUnionMemberModel model)
        {
            model.UpdatedBy = User.UserName;
            var deleted = _unionCache.DeleteMember(model);

            var response = CreateMessage($"{_memberTitle} [{model.FullName}",
                EnumProcessType.Delete, deleted ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response });
        }

        #endregion

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.View)]
        public JsonResult GetUnionsViaType(int typeUnion)
        {
            var lstUnions = _unionCache.GetAll(userName: null, belongUnions: null, typeUnions: $"{typeUnion}").Where(u => u.IsActive);
            var dictUnions = new Dictionary<string, List<CateUnionModel>>();
            lstUnions.GroupBy(d => new { d.BelongUnionName }).ToList()
                .ForEach(g => { dictUnions.Add($"{g.Key.BelongUnionName}", g.ToList()); });

            return Json(dictUnions, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.View)]
        public JsonResult GetDeptsBelong(Guid? unionId)
        {
            //var lstDepts = _unionCache.GetAll(userName: null, belongUnions: unionId != null ? $"{unionId}" : null, typeUnions: $"{(int)EnumTypeUnion.Department}")
            //    .Where(u => u.IsActive && u.TypeUnion == (int)EnumTypeUnion.Department);
            var lstDepts = _unionCache.GetBelong(unionId, (int)EnumTypeUnion.Department);
            var dictUnions = new Dictionary<string, List<CateUnionModel>>();
            lstDepts.GroupBy(d => new { d.BelongUnionName }).ToList()
                .ForEach(g => { dictUnions.Add($"{g.Key.BelongUnionName}", g.ToList()); });

            return Json(dictUnions, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.View)]
        public JsonResult GetUnionsNotUsingProc(Guid? procId, int contractType)
        {
            var lstUnions = _unionCache.GetNotUsingProc(procId, $"{(int)EnumTypeUnion.Unit}", contractType)
                .Where(u => u.IsActive && u.TypeUnion == (int)EnumTypeUnion.Unit);
            var dictUnions = new Dictionary<string, List<CateUnionModel>>();
            lstUnions.GroupBy(u => new { BelongUnionName = string.IsNullOrEmpty(u.BelongUnionName) ? u.TypeUnionName : u.BelongUnionName }).ToList()
                .ForEach(g => { dictUnions.Add($"{g.Key.BelongUnionName}", g.ToList()); });

            return Json(dictUnions, JsonRequestBehavior.AllowGet);
        }

        #region Managers

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult UnionsBelong(int id)
        {
            var model = _userCache.GetById(id);

            return PartialView("_UnionsBelong", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult GetUnionsViaManager(string userName)
        {
            var lstUnions = _unionCache.GetUnionsViaManager(userName);
            int total = lstUnions.Count;
            var result = Json(
                new { draw = 0, recordsTotal = total, recordsFiltered = total, data = lstUnions },
                JsonRequestBehavior.AllowGet);
            return result;
        }

        [AjaxOnly]
        [ActionType(Type = EnumActionType.Add)]
        [HttpGet]
        public ActionResult AddUnion(string userName)
        {
            var userModel = _userCache.GetByUserName(userName);
            if (userModel == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_memberTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var lstUnionsBelong = _unionCache.GetUnionsViaManager(userName);

            var model = new CateUnionManagerModel
            {
                ListUnions = _unionCache.GetAll(userName: null, belongUnions: null, typeUnions: $"{(int)EnumTypeUnion.Unit}")
                    .Where(u => u.IsActive && lstUnionsBelong.All(em => em.UnionId != u.UnionId))
                    .Select(u => new SelectListItem
                    {
                        Text = u.UnionName,
                        Value = u.UnionId.ToString()
                    }).ToList(),
                UserName = userName,
                FullName = userModel.FullName
            };

            return PartialView("_AddUnion", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Add)]
        [ValidateInput(false)]
        public ActionResult AddUnion(CateUnionManagerModel model)
        {
            ModelState.Remove("PositionId");

            if (!ModelState.IsValid)
            {
                var lstUnionsBelong = _unionCache.GetUnionsViaManager(model.UserName);
                model.ListUnions = _unionCache.GetAll(userName: null, belongUnions: null, typeUnions: $"{(int)EnumTypeUnion.Unit}")
                    .Where(u => u.IsActive && lstUnionsBelong.All(em => em.UnionId != u.UnionId))
                    .Select(u => new SelectListItem
                    {
                        Text = u.UnionName,
                        Value = u.UnionId.ToString()
                    }).ToList();

                return PartialView("_UnionMember", model);
            }

            string response;
            var retSave = _unionCache.SaveManager(new CateUnionManagerModel
            {
                UnionId = model.UnionId,
                UnionName = model.UnionName,
                UserName = model.UserName,
                Reason = "Thêm mới",
                UpdatedBy = User.UserName
            });

            if (retSave == 0)
                response = CreateMessage($"{_memberTitle} [{model.FullName}]", EnumProcessType.Add, EnumMsgIcon.Error);
            else if (retSave == -9)
                response = CreateMessage($"{_memberTitle} [{model.FullName}]", EnumProcessType.DataExisted, EnumMsgIcon.Error);
            else
                response = CreateMessage($"{_memberTitle} [{model.FullName}]", EnumProcessType.Add, EnumMsgIcon.Success);
            return Json(new { status = true, message = response }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult EditUnion(Guid unionId, string userName)
        {
            var model = _unionCache.GetManagerByKey(unionId, userName);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_unionTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var lstUnionsBelong = _unionCache.GetUnionsViaManager(model.UserName);
            model.ListUnions = _unionCache.GetAll(userName: null, belongUnions: null, typeUnions: $"{(int)EnumTypeUnion.Unit}")
                .Where(u => u.IsActive && lstUnionsBelong.All(em => em.UnionId != u.UnionId))
                .Select(u => new SelectListItem
                {
                    Text = u.UnionName,
                    Value = u.UnionId.ToString()
                }).ToList();
            return PartialView("_EditUnion", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionType(Type = EnumActionType.Edit)]
        [ValidateInput(false)]
        public ActionResult EditUnion(CateUnionManagerModel model)
        {
            ModelState.Remove("PositionId");

            if (!ModelState.IsValid)
            {
                var lstUnionsBelong = _unionCache.GetUnionsViaManager(model.UserName);
                model.ListUnions = _unionCache.GetAll(userName: null, belongUnions: null, typeUnions: $"{(int)EnumTypeUnion.Unit}")
                    .Where(u => u.IsActive && lstUnionsBelong.All(em => em.UnionId != u.UnionId))
                    .Select(u => new SelectListItem
                    {
                        Text = u.UnionName,
                        Value = u.UnionId.ToString()
                    }).ToList();

                return PartialView("_UnionMember", model);
            }

            string response;
            var retSave = _unionCache.SaveManager(new CateUnionManagerModel
            {
                UnionId = model.UnionId,
                UnionName = model.UnionName,
                UserName = model.UserName,
                Reason = "Cập nhật",
                UpdatedBy = User.UserName
            });
            if (retSave == 0)
                response = CreateMessage($"{_unionTitle} [{model.UnionName}] - [{model.FullName}]", EnumProcessType.Edit, EnumMsgIcon.Error);
            else if (retSave == -9)
                response = CreateMessage($"{_unionTitle} [{model.UnionName}] - [{model.FullName}]", EnumProcessType.DataExisted, EnumMsgIcon.Error);
            else
                response = CreateMessage($"{_unionTitle} [{model.UnionName}] - [{model.FullName}]", EnumProcessType.Edit, EnumMsgIcon.Success);
            return Json(new { status = true, message = response }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult DeleteUnion(Guid unionId, string userName)
        {
            var model = _unionCache.GetManagerByKey(unionId, userName);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_memberTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Delete"),
                $"<b>{_unionTitle} {model.UnionName} - [{model.FullName}]</b>");
            return PartialView("_DeleteUnion", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult DeleteUnion(CateUnionManagerModel model)
        {
            model.UpdatedBy = User.UserName;
            var deleted = _unionCache.DeleteManager(model);

            var response = CreateMessage($"{_memberTitle} [{model.FullName} - {model.UnionName}]",
                EnumProcessType.Delete, deleted ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response });
        }

        #endregion
    }
}