using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Cores.Base.Apps;
using Cores.Cate.Caches;
using Cores.Cate.Enum;
using Cores.Major.Caches;
using Cores.Major.Models;
using Cores.Sys.Caches.Sys;
using FastMember;
using Modules.Major.Areas.Major.Models;
using Modules.Major.Providers;
using Newtonsoft.Json;
using TSFramework.App.Attributes;
using TSFramework.App.Models;
using TSFramework.App.Processors;
using TSFramework.Core.Enums;

namespace Modules.Major.Areas.Major.Controllers
{
    public class ProcedureController : AppController
    {
        #region Inits

        private readonly MajorProcedureCache _procedureCache = new MajorProcedureCache();
        private readonly MajorProcedureStepCache _stepCache = new MajorProcedureStepCache();

        private readonly CateCategoryCache _categoryCache = new CateCategoryCache();
        private readonly CateUnionCache _unionCache = new CateUnionCache();
        private readonly CatePositionCache _positionCache = new CatePositionCache();
        private readonly CatePurPoseCache _purposeCache = new CatePurPoseCache();
        private readonly CateContractTypeCache _contractTypeCache = new CateContractTypeCache();

        private readonly SysConfigCache _sysConfigCache = new SysConfigCache();

        private readonly string _procedureTitle = AppProcessor.Messagor.GetMessage("Procedure_Title");
        private readonly string _stepTitle = AppProcessor.Messagor.GetMessage("Step_Title");

        private static readonly string notificationLibrariesPathFolder = ConfigurationManager.AppSettings["NotificationsFolderPath"] ?? "/Libraries/Notifications";

        private const string CONFIG_NOTIFICATIONS_CUS_ACTIVE = "CONFIG_NOTIFICATIONS_CUS_ACTIVE";
        private const string CONFIG_NOTIFICATIONS_STAFF_ACTIVE = "CONFIG_NOTIFICATIONS_STAFF_ACTIVE";

        private readonly List<ListItem> _listNotificationConfigs;
        private readonly List<string> _listCusActiveNotifications;
        private readonly List<string> _listStaffActiveNotifications;

        #endregion

        public ProcedureController()
        {
            _listNotificationConfigs = string.IsNullOrEmpty(notificationLibrariesPathFolder)
                ? new List<ListItem>()
                : MajorProvider.LoadNotifications(notificationLibrariesPathFolder)
                    .Select(n => new ListItem(n.Description, n.Name)).ToList();

            _listCusActiveNotifications = _sysConfigCache.GetViaKey(CONFIG_NOTIFICATIONS_CUS_ACTIVE)?.ConfigValue?.Split(';').ToList();

            _listStaffActiveNotifications = _sysConfigCache.GetViaKey(CONFIG_NOTIFICATIONS_STAFF_ACTIVE)?.ConfigValue?.Split(';').ToList();
        }

        // GET: Cate/Procedure
        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        public ActionResult Index()
        {
            SearchProcedureModel searchModel = new SearchProcedureModel
            {
                ListUnions = _unionCache.GetAll(userName: null, belongUnions: null, typeUnions: $"{(int)EnumTypeUnion.Unit}").Where(u => u.IsActive).Select(u => new SelectListItem
                {
                    Text = u.UnionName,
                    Value = u.UnionId.ToString().ToUpper(),
                    Group = new SelectListGroup { Name = string.IsNullOrEmpty(u.BelongUnionName) ? u.TypeUnionName : u.BelongUnionName }
                }).ToList(),
                //ListTypeContracts = _contractTypeCache.GetAll().Select(ct => new ListItem
                //{
                //    Text = ct.ContractTypeName,
                //    Value = $"{ct.ContractTypeId}"
                //    //Value = $"{(int)Enum.Parse(typeof(EnumContractType), ct.ContractTypeCode)}"
                //}).OrderBy(i => i.Value).ToList()
            };
            return View(searchModel);
        }

        #region Main Actions

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult Get(SearchProcedureModel searchModel)
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
            var data = _procedureCache.Get(out var total, searchModel.UnionIds, searchModel.TypeContractIds, dataSearch);

            var result = Json(
                new { draw = Convert.ToInt32(draw), recordsTotal = total, recordsFiltered = total, data },
                JsonRequestBehavior.AllowGet);
            return result;
        }

        #region Add

        [AjaxOnly]
        [ActionType(Type = EnumActionType.Add)]
        [HttpGet]
        public ActionResult Add()
        {
            var lstContractTypes = _contractTypeCache.GetAll();

            var model = new MajorProcedureModel
            {
                //ListUnions = _unionCache.GetAll(userName: null, belongUnions: null, typeUnions: $"{(int)EnumTypeUnion.Unit}")
                //    .Where(u => u.IsActive && _unionCache.GetNotUsingProc(null, $"{(int)EnumTypeUnion.Unit}").Exists(up => up.UnionId == u.UnionId))
                //    .Select(u => new SelectListItem
                //    {
                //        Text = u.UnionName,
                //        Value = u.UnionId.ToString().ToUpper(),
                //        Group = new SelectListGroup { Name = string.IsNullOrEmpty(u.BelongUnionName) ? u.TypeUnionName : u.BelongUnionName }
                //    }).ToList(),

                ListContractTypes = lstContractTypes.Select(ct => new ListItem
                {
                    Text = ct.ContractTypeName,
                    Value = $"{ct.ContractTypeId}"
                    //Value = $"{(int)Enum.Parse(typeof(EnumContractType), ct.ContractTypeCode)}"
                }).OrderBy(i => i.Value).ToList()
            };
            return PartialView("_Add", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Add)]
        [ValidateInput(false)]
        public ActionResult Add(MajorProcedureModel model)
        {
            if (!ModelState.IsValid)
            {
                var lstContractTypes = _contractTypeCache.GetAll();

                //model.ListUnions = _unionCache.GetAll(userName: null, belongUnions: null, typeUnions: $"{(int)EnumTypeUnion.Unit}")
                //    .Where(u => u.IsActive && _unionCache.GetNotUsingProc(null, $"{(int)EnumTypeUnion.Unit}").Exists(up => up.UnionId == u.UnionId))
                //    .Select(u => new SelectListItem
                //    {
                //        Text = u.UnionName,
                //        Value = u.UnionId.ToString().ToUpper(),
                //        Group = new SelectListGroup { Name = string.IsNullOrEmpty(u.BelongUnionName) ? u.TypeUnionName : u.BelongUnionName }
                //    }).ToList();

                model.ListContractTypes = lstContractTypes.Select(ct => new ListItem
                {
                    Text = ct.ContractTypeName,
                    Value = $"{ct.ContractTypeId}"
                    //Value = $"{(int)Enum.Parse(typeof(EnumContractType), ct.ContractTypeCode)}"
                }).OrderBy(i => i.Value).ToList();

                return PartialView("_Procedure", model);
            }

            string response;
            var procedureId = _procedureCache.Save(new MajorProcedureModel
            {
                ProcedureId = Guid.Empty,
                ProcedureCode = model.ProcedureCode,
                ProcedureName = model.ProcedureName,
                ProcedureDesc = model.ProcedureDesc,
                ApplyFrom = model.ApplyFrom,
                ExpiredOn = model.ExpiredOn,
                Version = model.Version,
                ContractTypeId = model.ContractTypeId,
                ContractTypeName = model.ContractTypeName,
                SelectedUnions = string.Join(",", model.UnionIds),
                Reason = "Thêm mới",
                UpdatedBy = User.UserName
            });
            if (procedureId == 0)
                response = CreateMessage($"{_procedureTitle} [{model.ProcedureName}]", EnumProcessType.Add, EnumMsgIcon.Error);
            else if (procedureId == -9)
                response = CreateMessage($"{_procedureTitle} [{model.ProcedureName}] ({model.ApplyFrom} - {model.ExpiredOn})", EnumProcessType.DataExisted, EnumMsgIcon.Error);
            else
                response = CreateMessage($"{_procedureTitle} [{model.ProcedureName}]", EnumProcessType.Add, EnumMsgIcon.Success);
            return Json(new { status = true, message = response }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Edit

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult Edit(Guid? id)
        {
            var model = _procedureCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_procedureTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var lstContractTypes = _contractTypeCache.GetAll();

            //model.ListUnions = _unionCache.GetAll(userName: null, belongUnions: null, typeUnions: $"{(int)EnumTypeUnion.Unit}")
            //    .Where(u => u.IsActive && _unionCache.GetNotUsingProc(id, $"{(int)EnumTypeUnion.Unit}").Exists(up => up.UnionId == u.UnionId))
            //    .Select(u => new SelectListItem
            //    {
            //        Text = u.UnionName,
            //        Value = u.UnionId.ToString().ToUpper(),
            //        Group = new SelectListGroup { Name = string.IsNullOrEmpty(u.BelongUnionName) ? u.TypeUnionName : u.BelongUnionName }
            //    }).ToList();

            model.UnionIds = model.SelectedUnions.Split(',').Select(Guid.Parse).ToList();
            model.ListContractTypes = lstContractTypes.Select(ct => new ListItem
            {
                Text = ct.ContractTypeName,
                Value = $"{ct.ContractTypeId}"
                //Value = $"{(int)Enum.Parse(typeof(EnumContractType), ct.ContractTypeCode)}"
            }).OrderBy(i => i.Value).ToList();

            return PartialView("_Edit", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionType(Type = EnumActionType.Edit)]
        [ValidateInput(false)]
        public ActionResult Edit(MajorProcedureModel model)
        {
            if (!ModelState.IsValid)
            {
                var lstContractTypes = _contractTypeCache.GetAll();
                //model.ListUnions = _unionCache.GetAll(userName: null, belongUnions: null, typeUnions: $"{(int)EnumTypeUnion.Unit}")
                //    .Where(u => u.IsActive && _unionCache.GetNotUsingProc(model.ProcedureId, $"{(int)EnumTypeUnion.Unit}").Exists(up => up.UnionId == u.UnionId))
                //    .Select(u => new SelectListItem
                //    {
                //        Text = u.UnionName,
                //        Value = u.UnionId.ToString().ToUpper(),
                //        Group = new SelectListGroup { Name = string.IsNullOrEmpty(u.BelongUnionName) ? u.TypeUnionName : u.BelongUnionName }
                //    }).ToList();

                model.ListContractTypes = lstContractTypes.Select(ct => new ListItem
                {
                    Text = ct.ContractTypeName,
                    Value = $"{ct.ContractTypeId}"
                    //Value = $"{(int)Enum.Parse(typeof(EnumContractType), ct.ContractTypeCode)}"
                }).OrderBy(i => i.Value).ToList();
                return PartialView("_Procedure", model);
            }
            string response;
            var procedureId = _procedureCache.Save(new MajorProcedureModel
            {
                ProcedureId = model.ProcedureId,
                ProcedureCode = model.ProcedureCode,
                ProcedureName = model.ProcedureName,
                ProcedureDesc = model.ProcedureDesc,
                ApplyFrom = model.ApplyFrom,
                ExpiredOn = model.ExpiredOn,
                Version = model.Version,
                ContractTypeId = model.ContractTypeId,
                ContractTypeName = model.ContractTypeName,
                SelectedUnions = string.Join(",", model.UnionIds),
                //SelectedUnions = model.SelectedUnions,
                Reason = model.Reason,
                UpdatedBy = User.UserName
            });
            if (procedureId == 0)
                response = CreateMessage($"{_procedureTitle} [{model.ProcedureName}]", EnumProcessType.Edit,
                    EnumMsgIcon.Error);
            else if (procedureId == -9)
                response = CreateMessage($"{_procedureTitle} [{model.ProcedureName}]", EnumProcessType.DataExisted,
                    EnumMsgIcon.Error
                );
            else
                response = CreateMessage($"{_procedureTitle} [{model.ProcedureName}]", EnumProcessType.Edit,
                    EnumMsgIcon.Success
                );
            return Json(new { status = true, message = response }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Delete

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(Guid? id)
        {
            var model = _procedureCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_procedureTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Delete"),
                $"<b>{_procedureTitle} [{model.ProcedureName}]</b>");
            return PartialView("_Delete", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(MajorProcedureModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Delete"),
                    $"<b>{_procedureTitle} [{model.ProcedureName}]</b>");
                return PartialView("_DeleteBody", model);
            }

            model.UpdatedBy = User.UserName;
            var ret = _procedureCache.Delete(model);
            string response;
            if (ret == -19)
            {
                response = CreateMessage(string.Format(AppProcessor.Messagor.GetMessage("Data_Was_Used"), $"{_procedureTitle} [{model.ProcedureName}]"),
                    EnumProcessType.NonFormat, EnumMsgIcon.Error);
                return Json(new { status = true, message = response });
            }
            response = CreateMessage($"{_procedureTitle} [{model.ProcedureName}]",
                EnumProcessType.Delete, ret > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response });
        }

        #endregion

        #region Clone

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Add)]
        public ActionResult Clone(Guid? id)
        {
            var model = _procedureCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_procedureTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var lstContractTypes = _contractTypeCache.GetAll();

            model.UnionIds = model.SelectedUnions.Split(',').Select(Guid.Parse).ToList();
            model.ListContractTypes = lstContractTypes.Select(ct => new ListItem
            {
                Text = ct.ContractTypeName,
                Value = $"{ct.ContractTypeId}"
            }).OrderBy(i => i.Value).ToList();

            model.IsClone = true;
            model.Reason = string.Format(AppProcessor.Messagor.GetMessage("Modal_Title_Clone"),
                $"{AppProcessor.Messagor.GetMessage("Procedure_Title")} [{model.ProcedureName}]");

            return PartialView("_Clone", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionType(Type = EnumActionType.Add)]
        [ValidateInput(false)]
        public ActionResult Clone(MajorProcedureModel model)
        {
            if (!ModelState.IsValid)
            {
                var lstContractTypes = _contractTypeCache.GetAll();

                model.ListContractTypes = lstContractTypes.Select(ct => new ListItem
                {
                    Text = ct.ContractTypeName,
                    Value = $"{ct.ContractTypeId}"
                }).OrderBy(i => i.Value).ToList();
                return PartialView("_Procedure", model);
            }
            string response;
            var procedureId = _procedureCache.Clone(new MajorProcedureModel
            {
                ProcedureId = model.ProcedureId,
                ProcedureCode = model.ProcedureCode,
                ProcedureName = model.ProcedureName,
                ProcedureDesc = model.ProcedureDesc,
                ApplyFrom = model.ApplyFrom,
                ExpiredOn = model.ExpiredOn,
                Version = model.Version,
                ContractTypeId = model.ContractTypeId,
                ContractTypeName = model.ContractTypeName,
                UpdatedBy = User.UserName
            });
            if (procedureId == 0)
                response = CreateMessage($"{_procedureTitle} [{model.ProcedureName}]", EnumProcessType.Edit,
                    EnumMsgIcon.Error);
            else if (procedureId == -9)
                response = CreateMessage($"{_procedureTitle} [{model.ProcedureName}]", EnumProcessType.DataExisted,
                    EnumMsgIcon.Error);
            else
                response = CreateMessage($"{_procedureTitle} [{model.ProcedureName}]", EnumProcessType.Edit,
                    EnumMsgIcon.Success);
            return Json(new { status = true, message = response }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Status

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Edit)]
        public Task<ActionResult> ToggleStatus(MajorProcedureModel model)
        {
            var procedureModel = _procedureCache.GetById(model.ProcedureId);
            if (procedureModel == null)
                return Task.FromResult<ActionResult>(Json(new
                {
                    status = true,
                    message = CreateMessage($"{_procedureTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                }));
            model.UpdatedBy = User.UserName;
            var isSuccess = _procedureCache.ToggleStatus(model);
            var response = CreateMessage($"{_procedureTitle} [{model.ProcedureName}]",
                EnumProcessType.Edit, isSuccess ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Task.FromResult<ActionResult>(Json(new { status = true, message = response }));
        }

        #endregion

        #endregion

        #region Handler

        private readonly string _handlerTitle = AppProcessor.Messagor.GetMessage("Step_Handler");

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult AddHandler(Guid? stepId, Guid? procedureId)
        {
            var lstStepHandlers = Session[$"StepHandlers-{User.UserName}-{stepId}"] as List<MajorProcedureStepHandlerModel>;
            lstStepHandlers = lstStepHandlers ?? new List<MajorProcedureStepHandlerModel>();

            var lstUnionUsing = _unionCache.GetUsingProc(procedureId, $"{(int)EnumTypeUnion.Unit}")
                .Where(u => lstStepHandlers != null && !lstStepHandlers.Exists(us => us.UnionId == u.UnionId));

            var handleModel = new MajorProcedureStepHandlerModel
            {
                StepId = stepId,
                ProcedureId = procedureId,
                ListUnions = lstUnionUsing.Where(u => u.IsActive)
                    .Select(u => new SelectListItem
                    {
                        Text = u.UnionName,
                        Value = u.UnionId.ToString().ToUpper(),
                        Group = new SelectListGroup { Name = string.IsNullOrEmpty(u.BelongUnionName) ? u.TypeUnionName : u.BelongUnionName }
                    }).ToList(),

                ListPositions = _positionCache.GetAll()
                    .Select(u => new ListItem
                    {
                        Text = u.PositionName,
                        Value = u.PositionID.ToString()
                    }).ToList(),
                ListStepsChangeHandlers = _stepCache.GetAll($"{procedureId}")
                .OrderBy(s => s.ProcedureName).ThenByDescending(s => s.Ordinal).ThenBy(s => s.StepName)
                .Where(s => s.StepType == "Step")
                .Select(c => new SelectListItem
                {
                    Text = c.StepName,
                    Value = c.StepId.ToString(),
                    Group = new SelectListGroup { Name = c.ProcedureName }
                }).ToList()
            };

            return PartialView("_AddHandler", handleModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Add)]
        [ValidateInput(false)]
        public ActionResult AddHandler(MajorProcedureStepHandlerModel model)
        {
            var lstStepHandlers = Session[$"StepHandlers-{User.UserName}-{model.StepId}"] as List<MajorProcedureStepHandlerModel>;
            var lstSteps = _stepCache.GetAll($"{model.ProcedureId}")
                .OrderBy(s => s.ProcedureName).ThenByDescending(s => s.Ordinal).ThenBy(s => s.StepName);

            if (!ModelState.IsValid)
            {
                var lstUnionUsing = _unionCache.GetUsingProc(model.ProcedureId, $"{(int)EnumTypeUnion.Unit}")
                    .Where(u => lstStepHandlers != null && !lstStepHandlers.Exists(us => us.UnionId == u.UnionId));

                model.ListUnions = lstUnionUsing.Where(u => u.IsActive)
                    .Select(u => new SelectListItem
                    {
                        Text = u.UnionName,
                        Value = u.UnionId.ToString().ToUpper(),
                        Group = new SelectListGroup
                        { Name = string.IsNullOrEmpty(u.BelongUnionName) ? u.TypeUnionName : u.BelongUnionName }
                    }).ToList();

                model.ListPositions = _positionCache.GetAll()
                    .Select(u => new ListItem
                    {
                        Text = u.PositionName,
                        Value = u.PositionID.ToString()
                    }).ToList();

                model.ListStepsChangeHandlers = lstSteps
                    .Where(s => s.StepType == "Step")
                    .Select(c => new SelectListItem
                    {
                        Text = c.StepName,
                        Value = c.StepId.ToString(),
                        Group = new SelectListGroup { Name = c.ProcedureName }
                    }).ToList();

                return PartialView("_Handler", model);
            }

            model.StepsChangeHandler = string.Join(";", model.ListStepsChangeHandler);
            model.ListStepNameChanges = lstSteps
                .Where(s => model.ListStepsChangeHandler.Exists(sc => sc == s.StepId)).Select(s => s.StepName)
                .ToList();

            lstStepHandlers = lstStepHandlers ?? new List<MajorProcedureStepHandlerModel>();
            lstStepHandlers.Add(model);

            Session[$"StepHandlers-{User.UserName}-{model.StepId}"] = lstStepHandlers.Select(d => d).ToList();

            var jsonHandlers = JsonConvert.SerializeObject(lstStepHandlers);
            var response = CreateMessage($"{_handlerTitle}", EnumProcessType.Add, EnumMsgIcon.Success);
            return Json(new { status = true, message = response, data = jsonHandlers }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult EditHandler(Guid? stepId, Guid? unionId)
        {
            var lstStepHandlers = Session[$"StepHandlers-{User.UserName}-{stepId}"] as List<MajorProcedureStepHandlerModel>;
            lstStepHandlers = lstStepHandlers ?? new List<MajorProcedureStepHandlerModel>();
            var stepHandler = lstStepHandlers.FirstOrDefault(t => t.StepId == stepId && t.UnionId == unionId);

            if (stepHandler == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_handlerTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var lstUnionUsing = _unionCache.GetUsingProc(stepHandler.ProcedureId, $"{(int)EnumTypeUnion.Unit}");
            stepHandler.ListUnions = lstUnionUsing.Where(u => u.IsActive)
                .Select(u => new SelectListItem
                {
                    Text = u.UnionName,
                    Value = u.UnionId.ToString().ToUpper(),
                    Group = new SelectListGroup
                    { Name = string.IsNullOrEmpty(u.BelongUnionName) ? u.TypeUnionName : u.BelongUnionName }
                }).ToList();

            stepHandler.ListPositions = _positionCache.GetAll()
                .Select(u => new ListItem
                {
                    Text = u.PositionName,
                    Value = u.PositionID.ToString()
                }).ToList();

            var lstSteps = _stepCache.GetAll($"{stepHandler.ProcedureId}")
                .OrderBy(s => s.ProcedureName).ThenByDescending(s => s.Ordinal).ThenBy(s => s.StepName).ToList();

            stepHandler.ListStepsChangeHandlers = lstSteps
                .Where(s => s.StepType == "Step")
                .Select(c => new SelectListItem
                {
                    Text = c.StepName,
                    Value = c.StepId.ToString(),
                    Group = new SelectListGroup { Name = c.ProcedureName }
                }).ToList();
            stepHandler.StepsChangeHandler = string.Join(";", stepHandler.ListStepsChangeHandler);
            stepHandler.ListStepNameChanges = lstSteps
                .Where(s => stepHandler.ListStepsChangeHandler.Exists(sc => sc == s.StepId)).Select(s => s.StepName)
                .ToList();

            stepHandler.IsEdit = true;

            return PartialView("_EditHandler", stepHandler);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Add)]
        [ValidateInput(false)]
        public ActionResult EditHandler(MajorProcedureStepHandlerModel model)
        {
            var lstStepHandlers = Session[$"StepHandlers-{User.UserName}-{model.StepId}"] as List<MajorProcedureStepHandlerModel>;
            if (lstStepHandlers == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_handlerTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var lstSteps = _stepCache.GetAll($"{model.ProcedureId}")
                .OrderBy(s => s.ProcedureName).ThenByDescending(s => s.Ordinal).ThenBy(s => s.StepName);

            if (!ModelState.IsValid)
            {
                var lstUnionUsing = _unionCache.GetUsingProc(model.ProcedureId, $"{(int)EnumTypeUnion.Unit}")
                    .Where(u => !lstStepHandlers.Exists(us => us.UnionId == u.UnionId));
                model.ListUnions = lstUnionUsing.Where(u => u.IsActive)
                    .Select(u => new SelectListItem
                    {
                        Text = u.UnionName,
                        Value = u.UnionId.ToString().ToUpper(),
                        Group = new SelectListGroup
                        { Name = string.IsNullOrEmpty(u.BelongUnionName) ? u.TypeUnionName : u.BelongUnionName }
                    }).ToList();

                model.ListPositions = _positionCache.GetAll()
                    .Select(u => new ListItem
                    {
                        Text = u.PositionName,
                        Value = u.PositionID.ToString()
                    }).ToList();

                model.ListStepsChangeHandlers = lstSteps
                    .Where(s => s.StepType == "Step")
                    .Select(c => new SelectListItem
                    {
                        Text = c.StepName,
                        Value = c.StepId.ToString(),
                        Group = new SelectListGroup { Name = c.ProcedureName }
                    }).ToList();

                model.StepsChangeHandler = string.Join(";", model.ListStepsChangeHandler);

                return PartialView("_Handler", model);
            }

            var stepHandler = lstStepHandlers.FirstOrDefault(t => t.StepId == model.StepId && t.UnionId == model.UnionId);
            if (stepHandler == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_handlerTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            stepHandler.DeptId = model.DeptId;
            stepHandler.DeptName = model.DeptName;
            stepHandler.PositionID = model.PositionID;
            stepHandler.PositionName = model.PositionName;
            stepHandler.StaffId = model.StaffId;
            stepHandler.StaffName = model.StaffName;
            stepHandler.AllowSwitchHandler = model.AllowSwitchHandler;
            stepHandler.AllowChangeHandler = model.AllowChangeHandler;
            stepHandler.ListStepsChangeHandler = model.ListStepsChangeHandler;
            stepHandler.StepsChangeHandler = string.Join(";", model.ListStepsChangeHandler);
            stepHandler.ListStepNameChanges = lstSteps
                .Where(s => model.ListStepsChangeHandler.Exists(sc => sc == s.StepId)).Select(s => s.StepName)
                .ToList();

            Session[$"StepHandlers-{User.UserName}-{model.StepId}"] = lstStepHandlers.Select(d => d).ToList();

            var jsonHandlers = JsonConvert.SerializeObject(lstStepHandlers);
            var response = CreateMessage($"{_handlerTitle}", EnumProcessType.Edit, EnumMsgIcon.Success);
            return Json(new { status = true, message = response, data = jsonHandlers }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult DeleteHandler(Guid? stepId, Guid? unionId)
        {
            var lstStepHandlers = Session[$"StepHandlers-{User.UserName}-{stepId}"] as List<MajorProcedureStepHandlerModel>;
            lstStepHandlers = lstStepHandlers ?? new List<MajorProcedureStepHandlerModel>();
            var stepHandler = lstStepHandlers.FirstOrDefault(t => t.StepId == stepId && t.UnionId == unionId);
            if (stepHandler == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_handlerTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Delete"),
                $"<b>{_handlerTitle} [{stepHandler.UnionName}]</b>");

            return PartialView("_DeleteHandler", stepHandler);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult DeleteHandler(MajorProcedureStepHandlerModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Delete"),
                    $"<b>{_handlerTitle} [{model.UnionName}]</b>");
                return PartialView("_DeleteHandlerBody", model);
            }

            var lstStepHandlers = Session[$"StepHandlers-{User.UserName}-{model.StepId}"] as List<MajorProcedureStepHandlerModel>;
            if (lstStepHandlers == null)
                return Json(new
                {
                    status = false,
                    message = CreateMessage($"{_handlerTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var stepHandler = lstStepHandlers.FirstOrDefault(t => t.StepId == model.StepId && t.UnionId == model.UnionId);
            if (stepHandler == null)
                return Json(new
                {
                    status = false,
                    message = CreateMessage($"{_handlerTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var unionName = stepHandler.UnionName;

            var deleted = lstStepHandlers.RemoveAll(t => t.StepId == model.StepId && t.UnionId == model.UnionId);

            if (deleted > -1)
            {
                Session[$"StepHandlers-{User.UserName}-{model.StepId}"] = lstStepHandlers.Select(d => d).ToList();
            }
            var jsonHandlers = JsonConvert.SerializeObject(lstStepHandlers);

            var response = CreateMessage($"{_handlerTitle} [{unionName}]",
                EnumProcessType.Delete, deleted > -1 ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response, data = jsonHandlers });
        }

        #endregion

        #region HandlingTime

        private readonly string _handlingTimeTitle = AppProcessor.Messagor.GetMessage("Step_HandlingTime");

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult AddHandlingTime(Guid? stepId, Guid? procedureId)
        {
            var procModel = _procedureCache.GetById(procedureId);
            if (procModel == null)
                return Json(new
                {
                    status = false,
                    message = CreateMessage($"{_procedureTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var lstStepHandlingTimes = Session[$"StepHandlingTimes-{User.UserName}-{stepId}"] as List<MajorProcedureStepHandlingTimeModel>;
            lstStepHandlingTimes = lstStepHandlingTimes ?? new List<MajorProcedureStepHandlingTimeModel>();

            var selectedPurposeIds = lstStepHandlingTimes.SelectMany(s => s.ListPurposeIds).ToList();

            var lstPurposes = _purposeCache.GetAll(contractTypeIds: $"{procModel.ContractTypeId}");
            var lstHandlingTimesViaStep = _stepCache.GetHandlingTimes(stepId);

            var handlingTimeModel = new MajorProcedureStepHandlingTimeModel
            {
                HandlingTimeId = Guid.NewGuid(),
                StepId = stepId,
                ProcedureId = procedureId,
                ListPurposes = lstPurposes
                    .Where(h => !lstHandlingTimesViaStep.Exists(ht => ht.PurposeIds?.Split(',').ToList().Exists(p => int.Parse(p) == h.PurPoseId) == true) && !selectedPurposeIds.Exists(pi => pi == h.PurPoseId))
                    //.Where(p => !selectedPurposeIds.Exists(pi => pi == p.PurPoseId))
                    .OrderBy(p => p.PurPoseName)
                    .Select(d => new SelectListItem
                    {
                        Text = d.PurPoseName,
                        Value = $"{d.PurPoseId}",
                        Group = new SelectListGroup { Name = d.ContractTypeName }
                    }).ToList()
            };

            return PartialView("_AddHandlingTime", handlingTimeModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Edit)]
        [ValidateInput(false)]
        public ActionResult AddHandlingTime(MajorProcedureStepHandlingTimeModel model)
        {
            var lstStepHandlingTimes = Session[$"StepHandlingTimes-{User.UserName}-{model.StepId}"] as List<MajorProcedureStepHandlingTimeModel>;
            lstStepHandlingTimes = lstStepHandlingTimes ?? new List<MajorProcedureStepHandlingTimeModel>();

            var procModel = _procedureCache.GetById(model.ProcedureId);
            var lstPurposes = _purposeCache.GetAll(contractTypeIds: $"{procModel.ContractTypeId}");

            if (!ModelState.IsValid)
            {
                var selectedPurposeIds = lstStepHandlingTimes.SelectMany(s => s.ListPurposeIds).ToList();
                var lstHandlingTimesViaStep = _stepCache.GetHandlingTimes(model.StepId);

                model.ListPurposes = lstPurposes
                    .Where(h => !lstHandlingTimesViaStep.Exists(ht => ht.PurposeIds?.Split(',').ToList().Exists(p => int.Parse(p) == h.PurPoseId) == true) && !selectedPurposeIds.Exists(pi => pi == h.PurPoseId))
                    .OrderBy(p => p.PurPoseName)
                    .Select(d => new SelectListItem
                    {
                        Text = d.PurPoseName,
                        Value = $"{d.PurPoseId}",
                        Group = new SelectListGroup { Name = d.ContractTypeName }
                    }).ToList();
                return PartialView("_HandlingTime", model);
            }

            var purposeItemHtmls = string.Empty;

            if (lstStepHandlingTimes.Exists(t => t.HandlingTime == model.HandlingTime))
            {
                //return Json(new
                //{
                //    status = false,
                //    message = CreateMessage($"{_handlingTimeTitle} [{model.HandlingTime}]", EnumProcessType.DataExisted, EnumMsgIcon.Error)
                //});

                var existStepHandlingTime = lstStepHandlingTimes.First(t => t.HandlingTime == model.HandlingTime);
                existStepHandlingTime.ListPurposeIds = existStepHandlingTime.PurposeIds.Split(',').Select(int.Parse).ToList();
                existStepHandlingTime.ListPurposeIds.AddRange(model.ListPurposeIds);
                existStepHandlingTime.ListPurposeIds = existStepHandlingTime.ListPurposeIds.Distinct().ToList();

                existStepHandlingTime.PurposeNames = string.Join(",",
                    lstPurposes.Where(p => existStepHandlingTime.ListPurposeIds.Exists(pi => pi == p.PurPoseId)).Select(p => p.PurPoseName).ToList());
                existStepHandlingTime.PurposeIds = string.Join(",", existStepHandlingTime.ListPurposeIds);

                lstPurposes.Where(p => existStepHandlingTime.ListPurposeIds.Exists(pi => pi == p.PurPoseId)).ToList().ForEach(p =>
                {
                    purposeItemHtmls += $"<li>{p.PurPoseName}</li>";
                });

                existStepHandlingTime.ViewPurposeNames = $"<ul class='pl-3 ml-1 text-dark-tp3'>{purposeItemHtmls}</ul>";
            }
            else
            {
                model.PurposeNames = string.Join(",",
                    lstPurposes.Where(p => model.ListPurposeIds.Exists(pi => pi == p.PurPoseId)).Select(p => p.PurPoseName).ToList());
                model.PurposeIds = string.Join(",", model.ListPurposeIds);

                lstPurposes.Where(p => model.ListPurposeIds.Exists(pi => pi == p.PurPoseId)).ToList().ForEach(p =>
                {
                    purposeItemHtmls += $"<li>{p.PurPoseName}</li>";
                });

                model.ViewPurposeNames = $"<ul class='pl-3 ml-1 text-dark-tp3'>{purposeItemHtmls}</ul>";
                lstStepHandlingTimes.Add(model);
            }

            Session[$"StepHandlingTimes-{User.UserName}-{model.StepId}"] = lstStepHandlingTimes.Select(d => d).ToList();

            var jsonHandlingTimes = JsonConvert.SerializeObject(lstStepHandlingTimes);
            var response = CreateMessage($"{_handlingTimeTitle}", EnumProcessType.Add, EnumMsgIcon.Success);
            return Json(new { status = true, message = response, data = jsonHandlingTimes }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult EditHandlingTime(Guid? stepId, Guid? handlingTimeId)
        {
            var lstStepHandlingTimes = Session[$"StepHandlingTimes-{User.UserName}-{stepId}"] as List<MajorProcedureStepHandlingTimeModel>;
            lstStepHandlingTimes = lstStepHandlingTimes ?? new List<MajorProcedureStepHandlingTimeModel>();
            var stepHandlingTime = lstStepHandlingTimes.FirstOrDefault(t => t.HandlingTimeId == handlingTimeId);

            //var stepModel = _stepCache.GetById(stepId);
            //var stepHandlingTime = _stepCache.GetHandlingTimeById(handlingTimeId);
            
            if (stepHandlingTime == null)
                return Json(new
                { 
                    status = false,
                    message = CreateMessage($"{_handlerTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var procModel = _procedureCache.GetById(stepHandlingTime.ProcedureId);
            var lstPurposes = _purposeCache.GetAll(contractTypeIds: $"{procModel.ContractTypeId}");
            var lstHandlingTimesViaStep = _stepCache.GetHandlingTimes(stepId).Where(ht => ht.HandlingTimeId != handlingTimeId).ToList();

            lstPurposes = lstPurposes.Where(p => !lstHandlingTimesViaStep.Exists(ht =>
                ht.PurposeIds?.Split(',').ToList().Exists(pi => int.Parse(pi) == p.PurPoseId) == true)).ToList();

            stepHandlingTime.ListPurposes = lstPurposes
                //.Where(p => !selectedPurposeIds.Exists(pi => pi == p.PurPoseId))
                .Where(h => !lstHandlingTimesViaStep.Exists(ht => ht.PurposeIds?.Split(',').ToList().Exists(p=> int.Parse(p) == h.PurPoseId) == true))
                .OrderBy(p => p.PurPoseName)
                .Select(d => new SelectListItem
                {
                    Text = d.PurPoseName,
                    Value = $"{d.PurPoseId}",
                    Group = new SelectListGroup { Name = d.ContractTypeName }
                }).ToList();
            stepHandlingTime.IsEdit = true;

            return PartialView("_EditHandlingTime", stepHandlingTime);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Edit)]
        [ValidateInput(false)]
        public ActionResult EditHandlingTime(MajorProcedureStepHandlingTimeModel model)
        {
            var lstStepHandlingTimes = Session[$"StepHandlingTimes-{User.UserName}-{model.StepId}"] as List<MajorProcedureStepHandlingTimeModel>;
            if (lstStepHandlingTimes == null)
                return Json(new
                {
                    status = false,
                    message = CreateMessage($"{_handlerTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var stepHandlingTime = lstStepHandlingTimes.FirstOrDefault(t => t.HandlingTimeId == model.HandlingTimeId);
            if (stepHandlingTime == null)
                return Json(new
                {
                    status = false,
                    message = CreateMessage($"{_handlerTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var procModel = _procedureCache.GetById(stepHandlingTime.ProcedureId);
            var lstPurposes = _purposeCache.GetAll(contractTypeIds: $"{procModel.ContractTypeId}");
            var lstHandlingTimesViaStep = _stepCache.GetHandlingTimes(model.StepId).Where(ht => ht.HandlingTimeId != model.HandlingTimeId).ToList();

            lstPurposes = lstPurposes.Where(p => !lstHandlingTimesViaStep.Exists(ht =>
                ht.PurposeIds?.Split(',').ToList().Exists(pi => int.Parse(pi) == p.PurPoseId) == true)).ToList();

            if (!ModelState.IsValid)
            {
                model.ListPurposes = lstPurposes
                    //.Where(p => !selectedPurposeIds.Exists(pi => pi == p.PurPoseId))
                    .Where(h => !lstHandlingTimesViaStep.Exists(ht => ht.PurposeIds?.Split(',').ToList().Exists(p => int.Parse(p) == h.PurPoseId) == true))
                    .OrderBy(p => p.PurPoseName)
                    .Select(d => new SelectListItem
                    {
                        Text = d.PurPoseName,
                        Value = $"{d.PurPoseId}",
                        Group = new SelectListGroup { Name = d.ContractTypeName }
                    }).ToList();

                return PartialView("_HandlingTime", model);
            }

            var purposeItemHtmls = string.Empty;

            if (lstStepHandlingTimes.Exists(t =>
                    t.HandlingTimeId != model.HandlingTimeId && t.HandlingTime == model.HandlingTime))
            {
                var existStepHandlingTime = lstStepHandlingTimes.First(t =>
                    t.HandlingTimeId != model.HandlingTimeId && t.HandlingTime == model.HandlingTime);

                existStepHandlingTime.ListPurposeIds = existStepHandlingTime.PurposeIds.Split(',').Select(int.Parse).ToList();
                existStepHandlingTime.ListPurposeIds.AddRange(model.ListPurposeIds);
                existStepHandlingTime.ListPurposeIds = existStepHandlingTime.ListPurposeIds.Distinct().ToList();

                existStepHandlingTime.PurposeNames = string.Join(",",
                    lstPurposes.Where(p => existStepHandlingTime.ListPurposeIds.Exists(pi => pi == p.PurPoseId)).Select(p => p.PurPoseName).ToList());
                existStepHandlingTime.PurposeIds = string.Join(",", existStepHandlingTime.ListPurposeIds);

                lstPurposes.Where(p => existStepHandlingTime.ListPurposeIds.Exists(pi => pi == p.PurPoseId)).ToList().ForEach(p =>
                {
                    purposeItemHtmls += $"<li>{p.PurPoseName}</li>";
                });

                existStepHandlingTime.ViewPurposeNames = $"<ul class='pl-3 ml-1 text-dark-tp3'>{purposeItemHtmls}</ul>";
                lstStepHandlingTimes.Remove(stepHandlingTime);
            }
            else
            {
                stepHandlingTime.HandlingTime = model.HandlingTime;

                //stepHandlingTime.PurposeIds = model.PurposeIds;
                //stepHandlingTime.PurposeNames = model.PurposeNames;

                stepHandlingTime.PurposeNames = string.Join(",",
                    lstPurposes.Where(p => model.ListPurposeIds.Exists(pi => pi == p.PurPoseId)).Select(p => p.PurPoseName).ToList());
                stepHandlingTime.PurposeIds = string.Join(",", model.ListPurposeIds);

                lstPurposes.Where(p => model.ListPurposeIds.Exists(pi => pi == p.PurPoseId)).ToList().ForEach(p =>
                {
                    purposeItemHtmls += $"<li>{p.PurPoseName}</li>";
                });

                stepHandlingTime.ViewPurposeNames = $"<ul class='pl-3 ml-1 text-dark-tp3'>{purposeItemHtmls}</ul>";
            }

            Session[$"StepHandlingTimes-{User.UserName}-{model.StepId}"] = lstStepHandlingTimes.Select(d => d).ToList();

            var jsonHandlingTimes = JsonConvert.SerializeObject(lstStepHandlingTimes);
            var response = CreateMessage($"{_handlingTimeTitle}", EnumProcessType.Edit, EnumMsgIcon.Success);
            return Json(new { status = true, message = response, data = jsonHandlingTimes }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult DeleteHandlingTime(Guid? stepId, Guid? handlingTimeId)
        {
            var lstStepHandlingTimes = Session[$"StepHandlingTimes-{User.UserName}-{stepId}"] as List<MajorProcedureStepHandlingTimeModel>;
            if (lstStepHandlingTimes == null)
                return Json(new
                {
                    status = false,
                    message = CreateMessage($"{_handlerTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var stepHandlingTime = lstStepHandlingTimes.FirstOrDefault(t => t.HandlingTimeId == handlingTimeId);
            if (stepHandlingTime == null)
                return Json(new
                {
                    status = false,
                    message = CreateMessage($"{_handlerTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Delete"),
                $"<b>{_handlingTimeTitle} [{stepHandlingTime.HandlingTime}]</b>");

            return PartialView("_DeleteHandlingTime", stepHandlingTime);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult DeleteHandlingTime(MajorProcedureStepHandlingTimeModel model)
        {
            ModelState.Remove("ListPurposeIds");
            if (!ModelState.IsValid)
            {
                ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Delete"),
                    $"<b>{_handlingTimeTitle} [{model.HandlingTime}]</b>");
                return PartialView("_DeleteHandlingTimeBody", model);
            }

            var lstStepHandlingTimes = Session[$"StepHandlingTimes-{User.UserName}-{model.StepId}"] as List<MajorProcedureStepHandlingTimeModel>;
            if (lstStepHandlingTimes == null)
                return Json(new
                {
                    status = false,
                    message = CreateMessage($"{_handlingTimeTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var stepHandlingTime = lstStepHandlingTimes.FirstOrDefault(t => t.HandlingTimeId == model.HandlingTimeId);
            if (stepHandlingTime == null)
                return Json(new
                {
                    status = false,
                    message = CreateMessage($"{_handlingTimeTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            //var deleted = lstStepHandlingTimes.RemoveAll(t => t.HandlingTimeId == model.HandlingTimeId);
            var deleted = lstStepHandlingTimes.Remove(stepHandlingTime);

            if (deleted)
            {
                Session[$"StepHandlingTimes-{User.UserName}-{model.StepId}"] = lstStepHandlingTimes.Select(d => d).ToList();
            }
            var jsonHandlingTimes = JsonConvert.SerializeObject(lstStepHandlingTimes.OrderBy(t => t.HandlingTime).ToList());

            var response = CreateMessage($"{_handlingTimeTitle} [{model.HandlingTime}]",
                EnumProcessType.Delete, deleted ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response, data = jsonHandlingTimes });
        }

        #endregion

        #region Steps Actions

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult View(Guid? id)
        {
            var model = _procedureCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_procedureTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            return PartialView("_View", model);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult GetTreeViewData(Guid? id)
        {
            var procModel = _procedureCache.GetById(id);
            var lstProcStep = _stepCache.GetAll(id.ToString()).OrderBy(s => s.Ordinal).ThenBy(s => s.StepName);
            var lstStepTreeView = lstProcStep
                .Select(s => new ViewStepTreeViewModel
                {
                    Id = s.StepId.ToString(),
                    Name = s.StepName,
                    TypeElement = s.StepType,
                    Icons = new Dictionary<string, List<string>> {
                                {
                                    "default", new List<string> {
                                        s.StepType == "Step" ? "<i class='fas fa-tasks'></i>" :
                                            (s.StepType == "Start" ? "<i class='fas fa-sign-out-alt'></i>" :"<i class='fas fa-sign-in-alt'></i>"),
                                        s.StepType == "Start" ? "text-green-m1" : (s.StepType == "End" ? "text-danger-m1" : "text-blue-m1")
                                    }
                                }
                            }
                }).ToList();

            var procTreeViews = new List<ViewStepTreeViewModel>
            {
                new ViewStepTreeViewModel
                {
                    Id = procModel.ProcedureId.ToString(),
                    Name = procModel.ProcedureName,
                    TypeElement = "Procedure",
                    Icons = new Dictionary<string, List<string>> { { "default", new List<string> { "<i class='fas fa-tags'></i>", "text-danger-m1" } } },
                    Children = lstStepTreeView
                }
            };
            var stepsTreeView = JsonConvert.SerializeObject(procTreeViews);

            return Content(stepsTreeView);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult AddStep(Guid? procedureId, Guid? prevStepId)
        {
            var procedureModel = _procedureCache.GetById(procedureId);

            if (procedureModel == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_procedureTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            //var lstStepHandlers = new List<MajorProcedureStepHandlerModel>();

            var model = new MajorProcedureStepModel
            {
                StepId = Guid.NewGuid(),
                ListProcedures = _procedureCache.GetAll()
                    .OrderBy(t => t.ProcedureName).ThenBy(t => t.ProcedureCode)
                    .Select(c => new ListItem { Text = c.ProcedureName, Value = c.ProcedureId.ToString() })
                    .ToList(),
                ListPrevSteps = _stepCache.GetAll($"{procedureId}")
                    .OrderBy(s => s.ProcedureName).ThenByDescending(s => s.Ordinal).ThenBy(s => s.StepName)
                    .Select(c => new SelectListItem { Text = c.StepName, Value = c.StepId.ToString(), Group = new SelectListGroup { Name = c.ProcedureName } })
                    .ToList(),
                ListNextSteps = _stepCache.GetAll($"{procedureId}")
                    .Where(s => s.StepId != prevStepId)
                    .OrderBy(s => s.ProcedureName).ThenByDescending(s => s.Ordinal).ThenBy(s => s.StepName)
                    .Select(c => new SelectListItem { Text = c.StepName, Value = c.StepId.ToString(), Group = new SelectListGroup { Name = c.ProcedureName } })
                    .ToList(),

                ProcedureId = procedureId,
                ProcedureName = procedureModel.ProcedureName,
                PrevStep = prevStepId,
                PrevStepName = prevStepId != null ? _stepCache.GetById(prevStepId)?.StepName : null,
                Reason = "Thêm mới",

                NotificationConfigs = _listNotificationConfigs,
                CusActiveNotifications = _listCusActiveNotifications,
                StaffActiveNotifications = _listStaffActiveNotifications,
                //ProcUnionId = procedureModel.UnionUsing,
                //ProcUnionName = procedureModel.UnionUsingName,
                ListUnionsUsingProc = _unionCache.GetUsingProc(procedureModel.ProcedureId, $"{(int)EnumTypeUnion.Unit}"),
                //DataHandlers = lstStepHandlers
            };

            //Session[$"StepHandlers-{User.UserName}-{model.StepId}"] = lstStepHandlers;

            return PartialView("_AddStep", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Add)]
        [ValidateInput(false)]
        public ActionResult AddStep(MajorProcedureStepModel model)
        {
            var lstStepHandlingTimes = Session[$"StepHandlingTimes-{User.UserName}-{model.StepId}"] as List<MajorProcedureStepHandlingTimeModel>;
            var lstStepHandlers = Session[$"StepHandlers-{User.UserName}-{model.StepId}"] as List<MajorProcedureStepHandlerModel>;
            var lstStepSituations = Session[$"StepSituations-{User.UserName}-{model.StepId}"] as List<MajorProcedureStepSituationModel>;

            if (!ModelState.IsValid)
            {
                model.ListProcedures = _procedureCache.GetAll()
                    .OrderBy(t => t.ProcedureName).ThenBy(t => t.ProcedureCode)
                    .Select(c => new ListItem { Text = c.ProcedureName, Value = c.ProcedureId.ToString() })
                    .ToList();
                model.ListPrevSteps = _stepCache.GetAll($"{model.ProcedureId}")
                    .OrderBy(s => s.ProcedureName).ThenByDescending(s => s.Ordinal).ThenBy(s => s.StepName)
                    .Select(c => new SelectListItem
                    {
                        Text = c.StepName,
                        Value = c.StepId.ToString(),
                        Group = new SelectListGroup { Name = c.ProcedureName }
                    }).ToList();
                model.ListNextSteps = _stepCache.GetAll($"{model.ProcedureId}")
                    .Where(s => s.StepId != model.PrevStep)
                    .OrderBy(s => s.ProcedureName).ThenByDescending(s => s.Ordinal).ThenBy(s => s.StepName)
                    .Select(c => new SelectListItem
                    {
                        Text = c.StepName,
                        Value = c.StepId.ToString(),
                        Group = new SelectListGroup { Name = c.ProcedureName }
                    }).ToList();

                var procedureModel = _procedureCache.GetById(model.ProcedureId);
                //model.ProcUnionId = procedureModel?.UnionUsing;
                //model.ProcUnionName = procedureModel?.UnionUsingName;
                model.ListUnionsUsingProc =
                    _unionCache.GetUsingProc(procedureModel?.ProcedureId, $"{(int)EnumTypeUnion.Unit}");

                model.NotificationConfigs = _listNotificationConfigs;
                model.CusActiveNotifications = _listCusActiveNotifications;
                model.StaffActiveNotifications = _listStaffActiveNotifications;

                model.ListCusNotificationConfigs = string.IsNullOrEmpty(model.CusNotificationConfigs) ? new List<string>() : model.CusNotificationConfigs.Split(';').ToList();
                model.ListStaffNotificationConfigs = string.IsNullOrEmpty(model.StaffNotificationConfigs) ? new List<string>() : model.StaffNotificationConfigs.Split(';').ToList();

                model.DataHandlingTimes = lstStepHandlingTimes ?? new List<MajorProcedureStepHandlingTimeModel>();
                model.DataHandlers = lstStepHandlers ?? new List<MajorProcedureStepHandlerModel>();
                model.DataSituations = lstStepSituations ?? new List<MajorProcedureStepSituationModel>();

                return PartialView("_AddStep", model);
            }

            #region Data Handlers

            lstStepHandlers = lstStepHandlers ?? new List<MajorProcedureStepHandlerModel>();

            var dataStepHandlers = new DataTable();
            using (var reader = ObjectReader.Create(lstStepHandlers, "UnionId", "DeptId", "PositionID", "StaffId", "AllowChangeHandler", "StepsChangeHandler", "AllowSwitchHandler"))
            {
                dataStepHandlers.Load(reader);
            }

            #endregion

            #region Data Handling Time

            lstStepHandlingTimes = lstStepHandlingTimes ?? new List<MajorProcedureStepHandlingTimeModel>();

            var dataStepHandlingTimes = new DataTable();
            using (var reader = ObjectReader.Create(lstStepHandlingTimes, "HandlingTime", "PurposeIds", "PurposeNames"))
            {
                dataStepHandlingTimes.Load(reader);
            }

            #endregion

            #region Data Situations

            lstStepSituations = lstStepSituations ?? new List<MajorProcedureStepSituationModel>();

            var dataStepSituations = new DataTable();
            using (var reader = ObjectReader.Create(lstStepSituations, "SituationName", "NextStep", "NextStepName"))
            {
                dataStepSituations.Load(reader);
            }

            #endregion

            string response;
            var stepId = _stepCache.Save(new MajorProcedureStepModel
            {
                StepId = Guid.Empty,
                ProcedureId = model.ProcedureId,
                StepName = model.StepName,
                StepDesc = model.StepDesc,
                StepType = "Step",

                CusNotificationConfigs = string.Join(";", model.ListCusNotificationConfigs),
                StaffNotificationConfigs = string.Join(";", model.ListStaffNotificationConfigs),
                AttachResultFile = model.AttachResultFile,

                PrevStep = model.PrevStep,
                NextStep = model.NextStep,

                ContractStatus = model.ContractStatus,
                ContractStatusName = model.ContractStatusName,

                TableHandlers = dataStepHandlers,
                TableHandlingTimes = dataStepHandlingTimes,
                TableSituations = dataStepSituations,

                Reason = "Thêm mới",
                UpdatedBy = User.UserName
            });

            if (stepId == 0)
                response = CreateMessage($"{_stepTitle} [{model.StepName}] - {_procedureTitle} [{model.ProcedureName}]", EnumProcessType.Add, EnumMsgIcon.Error);
            else if (stepId == -9)
                response = CreateMessage($"{_stepTitle} [{model.StepName}] - {_procedureTitle} [{model.ProcedureName}]", EnumProcessType.DataExisted, EnumMsgIcon.Error);
            else if (stepId == -8)
                response = CreateMessage($"{_stepTitle} [{AppProcessor.Messagor.GetMessage("Step_Start")}]", EnumProcessType.DataExisted, EnumMsgIcon.Error);
            else if (stepId == -7)
                response = CreateMessage($"{_stepTitle} [{AppProcessor.Messagor.GetMessage("Step_Finish")}]", EnumProcessType.DataExisted, EnumMsgIcon.Error);
            else
                response = CreateMessage($"{_stepTitle} [{model.StepName}] - {_procedureTitle} [{model.ProcedureName}]", EnumProcessType.Add, EnumMsgIcon.Success);
            return Json(new { status = true, message = response, data = new { typeEle = "Step", objId = model.ProcedureId, objName = model.ProcedureName } }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult EditStep(Guid? id)
        {
            var model = _stepCache.GetById(id);

            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_stepTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            model.ListProcedures = _procedureCache.GetAll()
                .OrderBy(t => t.ProcedureName).ThenBy(t => t.ProcedureCode)
                .Select(c => new ListItem { Text = c.ProcedureName, Value = c.ProcedureId.ToString() })
            .ToList();

            model.ListPrevSteps = _stepCache.GetAll($"{model.ProcedureId}")
                .OrderBy(s => s.ProcedureName).ThenByDescending(s => s.Ordinal).ThenBy(s => s.StepName)
                .Where(s => s.StepId != id)
                .Select(c => new SelectListItem
                {
                    Text = c.StepName,
                    Value = c.StepId.ToString(),
                    Group = new SelectListGroup { Name = c.ProcedureName }
                })
                .ToList();

            model.ListNextSteps = _stepCache.GetAll($"{model.ProcedureId}")
                .OrderBy(s => s.ProcedureName).ThenByDescending(s => s.Ordinal).ThenBy(s => s.StepName)
                .Where(s => s.StepId != id)
                .Select(c => new SelectListItem
                {
                    Text = c.StepName,
                    Value = c.StepId.ToString(),
                    Group = new SelectListGroup { Name = c.ProcedureName }
                })
                .ToList();

            var procedureModel = _procedureCache.GetById(model.ProcedureId);

            //model.ProcUnionId = procedureModel?.UnionUsing;
            //model.ProcUnionName = procedureModel?.UnionUsingName;

            model.ListUnionsUsingProc =
                _unionCache.GetUsingProc(procedureModel?.ProcedureId, $"{(int)EnumTypeUnion.Unit}");

            model.NotificationConfigs = _listNotificationConfigs;
            model.CusActiveNotifications = _listCusActiveNotifications;
            model.StaffActiveNotifications = _listStaffActiveNotifications;

            model.ListCusNotificationConfigs = string.IsNullOrEmpty(model.CusNotificationConfigs) ? new List<string>() : model.CusNotificationConfigs.Split(';').ToList();
            model.ListStaffNotificationConfigs = string.IsNullOrEmpty(model.StaffNotificationConfigs) ? new List<string>() : model.StaffNotificationConfigs.Split(';').ToList();

            model.NextProcId = model.ProcedureId;

            var procModel = _procedureCache.GetById(model.ProcedureId);
            var lstPurposes = _purposeCache.GetAll(contractTypeIds: $"{procModel.ContractTypeId}");

            var dataHandlingTimes = _stepCache.GetHandlingTimes(model.StepId);
            dataHandlingTimes.ForEach(handleTime =>
            {
                handleTime.StepId = model.StepId;
                handleTime.ProcedureId = model.ProcedureId;

                handleTime.ListPurposeIds = handleTime.PurposeIds.Split(',').Select(int.Parse).ToList();
                handleTime.PurposeNames = string.Join(",",
                    lstPurposes.Where(p => handleTime.ListPurposeIds.Exists(pi => pi == p.PurPoseId)).Select(p => p.PurPoseName).ToList());
                handleTime.PurposeIds = string.Join(",", handleTime.ListPurposeIds);

                var purposeItemHtmls = string.Empty;

                lstPurposes.Where(p => handleTime.ListPurposeIds.Exists(pi => pi == p.PurPoseId)).ToList().ForEach(p =>
                {
                    purposeItemHtmls += $"<li>{p.PurPoseName}</li>";
                });

                handleTime.ViewPurposeNames = $"<ul class=\'pl-3 ml-1 text-dark-tp3\'>{purposeItemHtmls}</ul>";
            });

            model.DataHandlingTimes = dataHandlingTimes;
            var dataHandlers = _stepCache.GetHandlers(model.StepId);
            var lstSteps = _stepCache.GetAll($"{model.ProcedureId}")
                .OrderBy(s => s.ProcedureName).ThenByDescending(s => s.Ordinal).ThenBy(s => s.StepName);

            dataHandlers.ForEach(d =>
            {
                if (d.AllowChangeHandler)
                {
                    if (!string.IsNullOrEmpty(d.StepsChangeHandler))
                    {
                        d.ListStepsChangeHandler = d.StepsChangeHandler.Split(';').Select(Guid.Parse).ToList();
                        d.ListStepNameChanges = lstSteps
                            .Where(s => d.ListStepsChangeHandler.Exists(sc => sc == s.StepId)).Select(s => s.StepName)
                            .ToList();
                    }
                }
            });
            model.DataHandlers = dataHandlers;

            var dataSituations = _stepCache.GetSituations(model.StepId);
            model.DataSituations = dataSituations;

            Session[$"StepHandlers-{User.UserName}-{model.StepId}"] = model.DataHandlers.Select(d => d).ToList();
            Session[$"StepHandlingTimes-{User.UserName}-{model.StepId}"] = model.DataHandlingTimes.Select(d => d).ToList();
            Session[$"StepSituations-{User.UserName}-{model.StepId}"] = model.DataSituations.Select(d => d).ToList();

            return PartialView("_EditStep", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionType(Type = EnumActionType.Edit)]
        [ValidateInput(false)]
        public ActionResult EditStep(MajorProcedureStepModel model)
        {
            var lstStepHandlers = Session[$"StepHandlers-{User.UserName}-{model.StepId}"] as List<MajorProcedureStepHandlerModel>;
            var lstStepHandlingTimes = Session[$"StepHandlingTimes-{User.UserName}-{model.StepId}"] as List<MajorProcedureStepHandlingTimeModel>;
            var lstStepSituations = Session[$"StepSituations-{User.UserName}-{model.StepId}"] as List<MajorProcedureStepSituationModel>;

            if (!ModelState.IsValid)
            {
                model.ListProcedures = _procedureCache.GetAll()
                    .OrderBy(t => t.ProcedureName).ThenBy(t => t.ProcedureCode)
                    .Select(c => new ListItem { Text = c.ProcedureName, Value = c.ProcedureId.ToString() })
                    .ToList();

                model.ListPrevSteps = _stepCache.GetAll($"{model.ProcedureId}")
                    .OrderBy(s => s.ProcedureName).ThenByDescending(s => s.Ordinal).ThenBy(s => s.StepName)
                    .Where(s => s.StepId != model.StepId)
                    .Select(c => new SelectListItem
                    {
                        Text = c.StepName,
                        Value = c.StepId.ToString(),
                        Group = new SelectListGroup { Name = c.ProcedureName }
                    }).ToList();

                model.ListNextSteps = _stepCache.GetAll($"{model.ProcedureId}")
                    .OrderBy(s => s.ProcedureName).ThenByDescending(s => s.Ordinal).ThenBy(s => s.StepName)
                    .Where(s => s.StepId != model.StepId)
                    .Select(c => new SelectListItem
                    {
                        Text = c.StepName,
                        Value = c.StepId.ToString(),
                        Group = new SelectListGroup { Name = c.ProcedureName }
                    }).ToList();

                var procedureModel = _procedureCache.GetById(model.ProcedureId);
                //model.ProcUnionId = procedureModel?.UnionUsing;
                //model.ProcUnionName = procedureModel?.UnionUsingName;
                model.ListUnionsUsingProc =
                    _unionCache.GetUsingProc(procedureModel?.ProcedureId, $"{(int)EnumTypeUnion.Unit}");

                model.NotificationConfigs = _listNotificationConfigs;
                model.CusActiveNotifications = _listCusActiveNotifications;
                model.StaffActiveNotifications = _listStaffActiveNotifications;

                model.ListCusNotificationConfigs = string.IsNullOrEmpty(model.CusNotificationConfigs) ? new List<string>() : model.CusNotificationConfigs.Split(';').ToList();
                model.ListStaffNotificationConfigs = string.IsNullOrEmpty(model.StaffNotificationConfigs) ? new List<string>() : model.StaffNotificationConfigs.Split(';').ToList();

                model.DataHandlingTimes = lstStepHandlingTimes ?? new List<MajorProcedureStepHandlingTimeModel>();
                model.DataHandlers = lstStepHandlers ?? new List<MajorProcedureStepHandlerModel>();
                model.DataSituations = lstStepSituations ?? new List<MajorProcedureStepSituationModel>();

                return PartialView("_EditStep", model);
            }

            string response;

            #region Data Handlers

            lstStepHandlers = (lstStepHandlers ?? new List<MajorProcedureStepHandlerModel>()).Distinct().ToList();

            var dataStepHandlers = new DataTable();
            using (var reader = ObjectReader.Create(lstStepHandlers, "UnionId", "DeptId", "PositionID", "StaffId", "AllowChangeHandler", "StepsChangeHandler", "AllowSwitchHandler"))
            {
                dataStepHandlers.Load(reader);
            }

            #endregion

            #region Data Handling Time

            lstStepHandlingTimes = (lstStepHandlingTimes ?? new List<MajorProcedureStepHandlingTimeModel>()).Distinct().ToList();

            var dataStepHandlingTimes = new DataTable();
            using (var reader = ObjectReader.Create(lstStepHandlingTimes, "HandlingTime", "PurposeIds", "PurposeNames"))
            {
                dataStepHandlingTimes.Load(reader);
            }

            #endregion

            #region Data Situations

            lstStepSituations = (lstStepSituations ?? new List<MajorProcedureStepSituationModel>()).Distinct().ToList();

            var dataStepSituations = new DataTable();
            using (var reader = ObjectReader.Create(lstStepSituations, "SituationName", "NextStep", "NextStepName"))
            {
                dataStepSituations.Load(reader);
            }

            #endregion

            var stepId = _stepCache.Save(new MajorProcedureStepModel
            {
                StepId = model.StepId,
                ProcedureId = model.ProcedureId,
                StepName = model.StepName,
                StepDesc = model.StepDesc,
                StepType = model.StepType,

                CusNotificationConfigs = string.Join(";", model.ListCusNotificationConfigs),
                StaffNotificationConfigs = string.Join(";", model.ListStaffNotificationConfigs),
                AttachResultFile = model.AttachResultFile,

                PrevStep = model.PrevStep,
                NextStep = model.NextStep,

                ContractStatus = model.ContractStatus,
                ContractStatusName = model.ContractStatusName,

                TableHandlers = dataStepHandlers,
                TableHandlingTimes = dataStepHandlingTimes,
                TableSituations = dataStepSituations,

                Reason = model.Reason,
                UpdatedBy = User.UserName
            });

            if (stepId == 0)
                response = CreateMessage($"{_stepTitle} [{model.StepName}] - {_procedureTitle} [{model.ProcedureName}]", EnumProcessType.Edit,
                    EnumMsgIcon.Error);
            else if (stepId == -9)
                response = CreateMessage($"{_stepTitle} [{model.StepName}] - {_procedureTitle} [{model.ProcedureName}]", EnumProcessType.DataExisted,
                    EnumMsgIcon.Error);
            else if (stepId == -8)
                response = CreateMessage($"{_stepTitle} [{AppProcessor.Messagor.GetMessage("Step_Start")}]", EnumProcessType.DataExisted,
                    EnumMsgIcon.Error);
            else if (stepId == -7)
                response = CreateMessage($"{_stepTitle} [{AppProcessor.Messagor.GetMessage("Step_Finish")}]", EnumProcessType.DataExisted,
                    EnumMsgIcon.Error);
            else
                response = CreateMessage($"{_stepTitle} [{model.StepName}] - {_procedureTitle} [{model.ProcedureName}]", EnumProcessType.Edit,
                    EnumMsgIcon.Success
                );
            return Json(new { status = true, message = response }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult DeleteStep(Guid? id)
        {
            var model = _stepCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_stepTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Delete"),
                $"<b>{_stepTitle} [{model.StepName}] - {model.ProcedureName}</b>");
            return PartialView("_DeleteStep", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult DeleteStep(MajorProcedureStepModel model)
        {
            ModelState.Remove("PrevStep");
            ModelState.Remove("UnionId");
            if (!ModelState.IsValid)
            {
                ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Delete"),
                    $"<b>{_stepTitle} [{model.StepName}]</b>");
                return PartialView("_DeleteStepBody", model);
            }
            model.UpdatedBy = User.UserName;
            var deleted = _stepCache.Delete(model);

            var response = CreateMessage($"{_stepTitle} [{model.StepName}] - {model.ProcedureName}",
                EnumProcessType.Delete, deleted ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response });
        }

        #endregion

        #region Situations

        private readonly string _situationTitle = AppProcessor.Messagor.GetMessage("Situation_Title");

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Add)]
        public ActionResult AddSituation(Guid? stepId, Guid? procedureId)
        {
            //var stepModel = _stepCache.GetById(stepId);
            //if (stepModel == null)
            //    return Json(new
            //    {
            //        status = false,
            //        message = CreateMessage($"{_stepTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
            //    });
            //var lstStepSituations = Session[$"StepSituations-{User.UserName}-{stepId}"] as List<MajorProcedureStepSituationModel>;

            var situationModel = new MajorProcedureStepSituationModel
            {
                SituationId = Guid.NewGuid(),
                StepId = stepId,
                //StepName = stepModel.StepName,
                ProcedureId = procedureId,
                ListNextSteps = _stepCache.GetAll($"{procedureId}")
                    .Where(s => s.StepId != stepId)
                    .OrderBy(s => s.ProcedureName).ThenByDescending(s => s.Ordinal).ThenBy(s => s.StepName)
                    .Select(c => new SelectListItem
                    {
                        Text = c.StepName,
                        Value = c.StepId.ToString(),
                        Group = new SelectListGroup { Name = c.ProcedureName }
                    }).ToList()
            };

            return PartialView("_AddSituation", situationModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Add)]
        [ValidateInput(false)]
        public ActionResult AddSituation(MajorProcedureStepSituationModel model)
        {
            var lstStepSituations = Session[$"StepSituations-{User.UserName}-{model.StepId}"] as List<MajorProcedureStepSituationModel>;
            lstStepSituations = lstStepSituations ?? new List<MajorProcedureStepSituationModel>();

            if (!ModelState.IsValid)
            {
                model.ListNextSteps = _stepCache.GetAll($"{model.ProcedureId}")
                    .Where(s => s.StepId != model.StepId)
                    .OrderBy(s => s.ProcedureName).ThenByDescending(s => s.Ordinal).ThenBy(s => s.StepName)
                    .Select(c => new SelectListItem
                    {
                        Text = c.StepName,
                        Value = c.StepId.ToString(),
                        Group = new SelectListGroup { Name = c.ProcedureName }
                    }).ToList();
                return PartialView("_Situation", model);
            }

            lstStepSituations.Add(model);

            Session[$"StepSituations-{User.UserName}-{model.StepId}"] = lstStepSituations.Select(d => d).ToList();

            var jsonSituations = JsonConvert.SerializeObject(lstStepSituations);
            var response = CreateMessage($"{_situationTitle}", EnumProcessType.Add, EnumMsgIcon.Success);
            return Json(new { status = true, message = response, data = jsonSituations }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult EditSituation(Guid? stepId, Guid? situationId)
        {
            var lstStepSituations = Session[$"StepSituations-{User.UserName}-{stepId}"] as List<MajorProcedureStepSituationModel>;
            lstStepSituations = lstStepSituations ?? new List<MajorProcedureStepSituationModel>();
            var stepSituation = lstStepSituations.FirstOrDefault(t => t.SituationId == situationId);

            if (stepSituation == null)
                return Json(new
                {
                    status = false,
                    message = CreateMessage($"{_situationTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            stepSituation.ListNextSteps = _stepCache.GetAll($"{stepSituation.ProcedureId}")
                //.Where(s => s.StepId != stepSituation.StepId)
                .OrderBy(s => s.ProcedureName).ThenByDescending(s => s.Ordinal).ThenBy(s => s.StepName)
                .Select(c => new SelectListItem
                {
                    Text = c.StepName,
                    Value = c.StepId.ToString(),
                    Group = new SelectListGroup { Name = c.ProcedureName }
                }).ToList();

            stepSituation.IsEdit = true;

            return PartialView("_EditSituation", stepSituation);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Edit)]
        [ValidateInput(false)]
        public ActionResult EditSituation(MajorProcedureStepSituationModel model)
        {
            var lstStepSituations = Session[$"StepSituations-{User.UserName}-{model.StepId}"] as List<MajorProcedureStepSituationModel>;
            if (lstStepSituations == null)
                return Json(new
                {
                    status = false,
                    message = CreateMessage($"{_handlerTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var stepSituation = lstStepSituations.FirstOrDefault(t => t.SituationId == model.SituationId);
            if (stepSituation == null)
                return Json(new
                {
                    status = false,
                    message = CreateMessage($"{_handlerTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            if (!ModelState.IsValid)
            {
                stepSituation.ListNextSteps = _stepCache.GetAll($"{stepSituation.ProcedureId}")
                    //.Where(s => s.StepId != stepSituation.StepId)
                    .OrderBy(s => s.ProcedureName).ThenByDescending(s => s.Ordinal).ThenBy(s => s.StepName)
                    .Select(c => new SelectListItem
                    {
                        Text = c.StepName,
                        Value = c.StepId.ToString(),
                        Group = new SelectListGroup { Name = c.ProcedureName }
                    }).ToList();

                return PartialView("_Situation", model);
            }

            stepSituation.SituationName = model.SituationName;
            stepSituation.NextStep = model.NextStep;
            stepSituation.NextStepName = model.NextStepName;

            Session[$"StepSituations-{User.UserName}-{model.StepId}"] = lstStepSituations.Select(d => d).ToList();

            var jsonSituations = JsonConvert.SerializeObject(lstStepSituations);
            var response = CreateMessage($"{_situationTitle}", EnumProcessType.Edit, EnumMsgIcon.Success);
            return Json(new { status = true, message = response, data = jsonSituations }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult DeleteSituation(Guid? stepId, Guid? situationId)
        {
            var lstStepSituations = Session[$"StepSituations-{User.UserName}-{stepId}"] as List<MajorProcedureStepSituationModel>;
            if (lstStepSituations == null)
                return Json(new
                {
                    status = false,
                    message = CreateMessage($"{_handlerTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var stepSituation = lstStepSituations.FirstOrDefault(t => t.SituationId == situationId);
            if (stepSituation == null)
                return Json(new
                {
                    status = false,
                    message = CreateMessage($"{_handlerTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Delete"),
                $"<b>{_situationTitle} [{stepSituation.SituationName}]</b>");

            return PartialView("_DeleteSituation", stepSituation);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult DeleteSituation(MajorProcedureStepSituationModel model)
        {
            var lstStepSituations = Session[$"StepSituations-{User.UserName}-{model.StepId}"] as List<MajorProcedureStepSituationModel>;
            if (lstStepSituations == null)
                return Json(new
                {
                    status = false,
                    message = CreateMessage($"{_situationTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var stepSituation = lstStepSituations.FirstOrDefault(t => t.SituationId == model.SituationId);
            if (stepSituation == null)
                return Json(new
                {
                    status = false,
                    message = CreateMessage($"{_situationTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var deleted = lstStepSituations.RemoveAll(t => t.SituationId == model.SituationId);

            if (deleted > -1)
            {
                Session[$"StepSituations-{User.UserName}-{model.StepId}"] = lstStepSituations.Select(d => d).ToList();
            }
            var jsonSituations = JsonConvert.SerializeObject(lstStepSituations.OrderBy(t => t.SituationName).ToList());

            var response = CreateMessage($"{_situationTitle} [{model.SituationName}]",
                EnumProcessType.Delete, deleted > -1 ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response, data = jsonSituations });
        }

        #endregion

        #region Ajax

        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        [AjaxOnly]
        public ActionResult StepsViaProcedure(Guid? id)
        {
            var lstSteps = _stepCache.GetAll($"{id}").OrderBy(s => s.StepName).ToList();
            return Json(lstSteps);
        }

        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        [AjaxOnly]
        public ActionResult ProcTypesViaFieldViolated(Guid? id)
        {
            var lstProcTypes = _categoryCache.GetAll($"{(int)EnumCateType.ProcedureType}")
                .Where(p => p.CateParentId == id).OrderBy(s => s.CateName).ToList();
            return Json(lstProcTypes);
        }

        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        [AjaxOnly]
        public ActionResult StaffsViaUnionAndPosition(Guid? unionId, int? positionId)
        {
            var lstStaffs = _unionCache.GetMembers(unionId)
                .Where(s => positionId == null || s.PositionId == positionId)
                .OrderBy(s => s.FullName).ToList();
            return Json(lstStaffs);
        }

        #endregion
    }
}