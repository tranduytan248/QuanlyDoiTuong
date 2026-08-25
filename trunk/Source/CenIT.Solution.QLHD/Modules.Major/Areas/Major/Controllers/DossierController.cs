using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Cores.Base.Apps;
using Cores.Base.Enums;
using Cores.Base.Helpers;
using Cores.Base.Interfaces;
using Cores.Base.Models;
using Cores.Cate.Caches;
using Cores.Cate.Enum;
using Cores.Cate.Models;
using Cores.Major.Caches;
using Cores.Major.Enums;
using Cores.Major.Models;
using Cores.Sys.Caches.Sys;
using Cores.VNPT.SmsMarketing.Consts;
using Cores.VNPT.SmsMarketing.Providers;
using FastMember;
using Modules.Major.Areas.Major.Models;
using Modules.Major.Providers;
using Newtonsoft.Json;
using TSFramework.App.Attributes;
using TSFramework.App.Enums;
using TSFramework.App.Models;
using TSFramework.App.Processors;
using TSFramework.Core.Enums;
using TSFramework.Core.Helpers;
using ContractModel = Modules.Major.Areas.Major.Models.ContractModel;
using Image = System.Drawing.Image;

namespace Modules.Major.Areas.Major.Controllers
{
    public class DossierController : AppController
    {
        #region Inits

        //private readonly MajorProcedureStepCache _stepCache = new MajorProcedureStepCache();
        private readonly MajorContractCache _contractCache = new MajorContractCache();
        private readonly SysUserCache _userCache = new SysUserCache();

        private readonly CateUnionCache _unionCache = new CateUnionCache();
        private readonly CatePositionCache _positionCache = new CatePositionCache();
        private readonly CateDocCache _docCache = new CateDocCache();

        private readonly MajorDossierCache _dossierCache = new MajorDossierCache();
        private static readonly SysConfigCache sysConfigCache = new SysConfigCache();

        private const string CONFIG_KEY_NOTIFICATION_LIBS_FOLDER_PATH = "CONFIG_KEY_NOTIFICATION_LIBS_FOLDER_PATH";
        private const string CONFIG_TAX_INFO_IN_CONTRACT = "CONFIG_TAX_INFO_IN_CONTRACT";

        private readonly string _stepTitle = AppProcessor.Messagor.GetMessage("Step_Title");
        private readonly string _dossierTitle = AppProcessor.Messagor.GetMessage("Dossier_Title");
        private readonly string _dossierTaskTitle = AppProcessor.Messagor.GetMessage("Dossier_Task_Title");

        private readonly string _situationTitle = AppProcessor.Messagor.GetMessage("Situation_Title");

        private readonly string _refDossierDocsFolderPath = "/Contents/Modules/Major/RefDocs/";
        private readonly string _dossiersFolderName = "Dossiers";

        private readonly string _notificationLibrariesPathFolder = "/Libraries/Notifications";

        private readonly List<INotify> _listNotificationProviders;
        private readonly string _taxInfoContract = "";

        #endregion

        public DossierController()
        {
            var configModel = sysConfigCache.GetViaKey(CONFIG_KEY_NOTIFICATION_LIBS_FOLDER_PATH);
            if (configModel != null && !string.IsNullOrEmpty(configModel.ConfigValue))
            {
                _notificationLibrariesPathFolder = configModel.ConfigValue;
            }
            _listNotificationProviders = MajorProvider.LoadNotifications(_notificationLibrariesPathFolder);

            configModel = sysConfigCache.GetViaKey(CONFIG_TAX_INFO_IN_CONTRACT);
            if (configModel != null && !string.IsNullOrEmpty(configModel.ConfigValue))
            {
                _taxInfoContract = configModel.ConfigValue;
            }
        }

        private static string[] _arrPermissionViaUser;

        // GET: Major/Dossier
        [ActionType(Type = EnumActionType.View)]
        public ActionResult Index(string q)
        {
            _arrPermissionViaUser = GetPermissionViaUser(User.UserName);

            var belongUnion = _unionCache.GetUnionByMember(User.UserName);
            var lstUnionsManagerByUser = _unionCache.GetUnionsViaManager(User.UserName);
            SearchDossierModel searchModel = new SearchDossierModel
            {
                SearchValue = q,
                ListUnions = lstUnionsManagerByUser
                    .Select(u => new ListItem(text: u.UnionName, value: $"{u.UnionId}")).ToList(),
                UnionIds = $"{belongUnion.UnionId}",
                DossierStatus = $"{(int)EnumDossierTaskStatus.Handling}",
                //ListUnionIds = new List<string> { $"{belongUnion.UnionId}" },
                ListDossierStatusIds = new List<int> { (int)EnumDossierTaskStatus.Handling },
                Permissions = _arrPermissionViaUser
            };
            return View(searchModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.View)]
        public Task<ActionResult> Get(SearchDossierModel searchModel)
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
            var data = _dossierCache.Get(out var total, User.UserName, searchModel.UnionIds, searchModel.SearchValue, searchModel.FromDate, searchModel.ToDate, searchModel.GiveResultFromDate, searchModel.GiveResultToDate, searchModel.DossierStatus, searchModel.HandleTypes, searchModel.TypeContractIds, searchModel.TypeCusIds, dataSearch);

            #region Tính số ngày còn lại so với ngày trễ hạn

            var dossierLate = sysConfigCache.GetViaKey("CONFIG_KEY_DOSSIER_LATE");
            int dateLateConfig = int.Parse(dossierLate.ConfigValue);

            data.ForEach(c =>
            {
                if (c.DelayDay > dateLateConfig)
                {
                    c.CheckContractLate = 1;
                }
                else if (c.DelayDay < 0)
                {
                    c.CheckContractLate = -1;
                }
                else
                {
                    c.CheckContractLate = 0;
                }
            });

            //foreach (var dossier in data)
            //{
            //    if (dossier.GiveResultOn.HasValue)
            //    {
            //        // Lấy danh sách ngày nghỉ
            //        List<DateTime> holidayList = HolidayHelper.GetListHolidays(dossier.GiveResultOn.Value.Year);

            //        DateTime currentDate = DateTime.Today;
            //        TimeSpan delayTimeSpan = dossier.GiveResultOn.Value.Subtract(currentDate);
            //        // Lấy số ngày làm việc còn lại, không tính ngày nghỉ
            //        for (DateTime date = currentDate; date <= dossier.GiveResultOn.Value; date = date.AddDays(1))
            //        {
            //            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday || holidayList.Contains(date))
            //            {
            //                delayTimeSpan = delayTimeSpan.Subtract(TimeSpan.FromDays(1));
            //            }
            //        }
            //        dossier.DelayDay = delayTimeSpan.Days;
            //        if (dossier.DelayDay > dateLateConfig)
            //        {
            //            dossier.CheckContractLate = 1;
            //        }
            //        else if (dossier.DelayDay < 0)
            //        {
            //            dossier.CheckContractLate = -1;
            //        }
            //        else
            //        {
            //            dossier.CheckContractLate = 0;
            //        }
            //    }
            //    else
            //    {
            //        dossier.DelayDay = 0;
            //        dossier.CheckContractLate = 1;
            //    }
            //}
            #endregion

            var result = Json(
                new { draw = Convert.ToInt32(draw), recordsTotal = total, recordsFiltered = total, data, permission = _arrPermissionViaUser },
                JsonRequestBehavior.AllowGet);
            return Task.FromResult<ActionResult>(result);
        }

        #region Main Action

        [AjaxOnly]
        [ActionType(Type = EnumActionType.Edit)]
        [HttpGet]
        public ActionResult Approve(Guid? id)
        {
            var dossierModel = _dossierCache.GetById(id);
            if (dossierModel == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_dossierTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            ContractModel contract = new ContractModel
            {
                DossierId = dossierModel.DossierId,
                DossierCode = dossierModel.DossierCode,
                DossierName = dossierModel.DossierName,
                ReceivedOn = dossierModel.CreatedOn,
                ContractNo = "22/HD",
                ConfirmOn = DateTime.Now,
                HandleTime = dossierModel.HandlingTime,
                GiveResultOn = dossierModel.CreatedOn.AddDays(dossierModel.HandlingTime ?? 0)
            };

            return PartialView("_Approve", contract);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Add)]
        [ValidateInput(false)]
        public ActionResult Approve(ContractModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_ApproveBody", model);
            }

            var dossierModel = _dossierCache.GetById(model.DossierId);
            if (dossierModel == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_dossierTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var procedureView = JsonConvert.DeserializeObject<ViewProcedureStructureModel>(dossierModel.ProcConfigs);
            var currentStepView = procedureView.Steps.FirstOrDefault(s => s.StepId == dossierModel.InStep);
            if (currentStepView == null)
            {
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_stepTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            }

            var nextStepView = procedureView.Steps.FirstOrDefault(s => s.StepId == currentStepView.NextStep);
            if (nextStepView == null)
            {
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_stepTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            }

            var handlerNextStep = nextStepView.Handlers.FirstOrDefault(h => h.UnionId == currentStepView.UnionHandle);

            MajorApproveDossierModel approveDossierModel = new MajorApproveDossierModel
            {
                DossierId = model.DossierId,
                Status = (int)EnumDossierStatus.Handling,
                StatusName = AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(EnumDossierStatus.Handling)),
                NextStepId = nextStepView.StepId,
                NextStepName = nextStepView.StepName,
                //UnionHandled = nextStepView.UnionHandle,
                //UnionHandledName = nextStepView.UnionHandleName,

                UnionHandled = handlerNextStep.UnionId,
                UnionHandledName = handlerNextStep.UnionName,

                HandledBy = handlerNextStep.StaffId,
                PositionId = handlerNextStep.PositionId,
                HandlingTime = nextStepView.TotalHandlingTimes(),

                CurrentTaskStatus = (int)EnumDossierTaskStatus.Completed,
                CurrentTaskStatusName = AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(EnumDossierTaskStatus.Completed)),
                TaskStatus = (int)EnumDossierTaskStatus.Handling,
                TaskStatusName = AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(EnumDossierTaskStatus.Handling)),
                AllowSwitchHandler = nextStepView.AllowSwitchHandler ?? false,
                UpdatedBy = User.UserName
            };

            var saveResult = _dossierCache.Approve(approveDossierModel);

            var response = CreateMessage($"{_dossierTitle} [{model.DossierName}]", EnumProcessType.Edit, saveResult == 0 ? EnumMsgIcon.Error : EnumMsgIcon.Success);
            return Json(new { status = true, message = response }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(Guid? id)
        {
            var model = _dossierCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_dossierTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Delete"),
                $"<b>{_dossierTitle} [{model.DossierName}]</b>");
            model.ActionType = "DELETE";

            return PartialView("_Delete", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(MajorDossierModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Delete"),
                    $"<b>{_dossierTitle} [{model.DossierName}]</b>");
                return PartialView("_DeleteBody", model);
            }

            model.UpdatedBy = User.UserName;
            var ret = _dossierCache.Delete(model);
            string response;
            if (ret == -19)
            {
                response = CreateMessage(string.Format(AppProcessor.Messagor.GetMessage("Data_Was_Used"), $"{_dossierTitle} [{model.DossierName}]"),
                    EnumProcessType.NonFormat, EnumMsgIcon.Error);
                return Json(new { status = true, message = response });
            }
            response = CreateMessage($"{_dossierTitle} [{model.DossierName}]",
                EnumProcessType.Delete, ret > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response });
        }

        [AjaxOnly]
        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        public ActionResult Activity(Guid? id)
        {
            var dossierModel = _dossierCache.GetById(id);
            if (dossierModel == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_dossierTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            dossierModel.ListTasks = _dossierCache.GetTasks(id);
            dossierModel.ListTasks.ForEach(t =>
            {
                var lstRefFiles = _docCache.GetByObjectId($"{t.TaskId}");
                t.ListRefImgs = lstRefFiles
                    .Where(f => ConstMIMEType.ImageMIMETypes.Keys.Contains(f.ContentType))
                    .Select((img, idx) => new
                    {
                        idx = idx % 3,
                        img = new CateDocModel
                        {
                            FileId = img.FileId,
                            FilePath = $@".{img.FilePath}\{img.FileId}{img.FileExt}",
                            Dimensions = img.Dimensions
                        }
                    })
                    .GroupBy(g => g.idx)
                    .ToDictionary(g => g.Key, g => g.Select(a => a.img).ToList());
            });

            return PartialView("_Activity", dossierModel);
        }

        #endregion

        #region Handle Dossier Task

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult Handle(Guid? id)
        {
            var dossierTask = _dossierCache.GetTaskById(id);
            if (dossierTask == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_dossierTaskTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var dossierModel = _dossierCache.GetById(dossierTask.DossierId);
            if (dossierModel == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_dossierTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var procedureView = JsonConvert.DeserializeObject<ViewProcedureStructureModel>(dossierModel.ProcConfigs);
            dossierTask.ListSteps = procedureView.Steps.Where(s => s.StepType == "Step").OrderBy(s => s.Ordinal).ToList();
            dossierTask.InStepView = procedureView.Steps.FirstOrDefault(s => s.StepType == "Step" && s.StepId == dossierTask.InStep);
            return PartialView("_Handle", dossierTask);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Edit)]
        [ValidateInput(false)]
        public ActionResult Handle(MajorDossierTaskModel model)
        {
            var dossierModel = _dossierCache.GetById(model.DossierId);
            if (dossierModel == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_dossierTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            if (!ModelState.IsValid)
            {
                var procedureView = JsonConvert.DeserializeObject<ViewProcedureStructureModel>(dossierModel.ProcConfigs);
                model.ListSteps = procedureView.Steps;

                return PartialView("_HandleBody", model);
            }

            var dossierTaskModel = _dossierCache.GetTaskById(model.TaskId);
            if (dossierTaskModel == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_dossierTaskTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            model.Status = (int)EnumDossierTaskStatus.Handling;
            model.StatusName = AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(EnumDossierTaskStatus.Handling));
            model.HandledBy = User.UserName;
            model.UpdatedBy = User.UserName;

            var saveResult = _dossierCache.Handle(model);

            var response = CreateMessage(string.Format(AppProcessor.Messagor.GetMessage("Modal_Title_Handle"), $"{_dossierTaskTitle} [{model.InStepName}]"), EnumProcessType.NonFormat, saveResult == 0 ? EnumMsgIcon.Error : EnumMsgIcon.Success);
            return Json(new { status = true, message = response }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult Complete(Guid? id)
        {
            var dossierTask = _dossierCache.GetTaskById(id);
            if (dossierTask == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_dossierTaskTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var dossierModel = _dossierCache.GetById(dossierTask.DossierId);

            #region Procedure Structure View

            var procedureView = JsonConvert.DeserializeObject<ViewProcedureStructureModel>(dossierModel.ProcConfigs);
            var currentStepView = procedureView.Steps.FirstOrDefault(s => s.StepId == dossierModel.InStep);
            if (currentStepView == null)
            {
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_stepTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            }

            #endregion

            var handlerNextStep = currentStepView.Handlers.FirstOrDefault(h => h.UnionId == currentStepView.UnionHandle);

            if (handlerNextStep != null)
            {
                dossierTask.AllowChangeHandler = handlerNextStep.AllowChangeHandler;
                dossierTask.StepsChangeHandler = handlerNextStep.StepsChangeHandler;
            }

            #region NextStep Handler

            var nextStepView = procedureView.Steps.FirstOrDefault(s => s.StepId == currentStepView.NextStep);
            if (nextStepView != null)
            {
                if (nextStepView.HandledBy == null && nextStepView.Handlers?.Count > 0)
                {
                    var handler = nextStepView.Handlers.FirstOrDefault(h => h.UnionId == nextStepView.UnionHandle);
                    if (handler != null)
                    {
                        nextStepView.HandledBy = handler.StaffId;
                    }
                }
                if (nextStepView.UnionHandle != null && nextStepView.DeptHandle != null)
                {
                    var lstStaffsInUnionHandleNextStep = _unionCache.GetMembersViaUnion(nextStepView.DeptHandle);
                    dossierTask.ListNextStepHandlers = lstStaffsInUnionHandleNextStep;
                    dossierTask.NextStepHandler = nextStepView.HandledBy;
                }
                else if (nextStepView.Handlers?.Count > 0)
                {
                    var handler = nextStepView.Handlers.FirstOrDefault(h => h.UnionId == nextStepView.UnionHandle);
                    if (handler != null)
                    {
                        var lstStaffsInUnionHandleNextStep =
                            _unionCache.GetMembersViaUnion(handler.DeptId);
                        dossierTask.ListNextStepHandlers = lstStaffsInUnionHandleNextStep;
                        dossierTask.NextStepHandler = handler.StaffId;
                    }
                    else
                    {
                        var lstStaffsInUnionHandleNextStep = _unionCache.GetMembersViaUnion(nextStepView.DeptHandle);
                        dossierTask.ListNextStepHandlers = lstStaffsInUnionHandleNextStep;
                        dossierTask.NextStepHandler = nextStepView.HandledBy;
                    }
                }
                else
                {
                    var lstStaffsInUnionHandleNextStep = _unionCache.GetMembersViaUnion(nextStepView.DeptHandle);
                    dossierTask.ListNextStepHandlers = lstStaffsInUnionHandleNextStep;
                    dossierTask.NextStepHandler = nextStepView.HandledBy;
                }
            }

            #endregion

            dossierTask.AttachResultFile = currentStepView.AttachResultFile ?? false;
            dossierTask.InStepView = currentStepView;

            if (dossierTask.AllowChangeHandler && !string.IsNullOrEmpty(dossierTask.StepsChangeHandler))
            {
                var lstStepsChangeHandler = dossierTask.StepsChangeHandler.Split(';').ToList();
                dossierTask.ListStepsChangeHandler = procedureView.Steps
                    .Where(s => lstStepsChangeHandler.Exists(sch => $"{sch}".ToUpper() == $"{s.StepId}".ToUpper()))
                    .OrderBy(s => s.Ordinal)
                    .Select(s => new MajorStepChangeHandleModel
                    {
                        DossierId = dossierTask.DossierId,
                        UnionId = s.UnionHandle,
                        UnionName = s.UnionHandleName,
                        TaskId = dossierTask.TaskId,
                        StepId = s.StepId,
                        StepName = s.StepName,
                        StepDesc = s.StepDesc
                    }
                    ).ToList();

                dossierTask.ListStaffs = new List<CateUnionMemberModel>();

                var procUnionModel = _unionCache.GetById(currentStepView.UnionHandle);
                dossierTask.ProcUnionId = procedureView.ProcUnionId;

                var lstUnions = _unionCache
                    .GetBelong(currentStepView.UnionHandle, (int)EnumTypeUnion.Department)
                    .Where(u => u.IsActive).Select(u => new SelectListItem
                    {
                        Text = u.UnionName,
                        Value = u.UnionId.ToString(),
                        Group = new SelectListGroup { Name = u.BelongUnionName }
                    }).OrderBy(u => u.Group.Name).ToList();

                lstUnions.Insert(0, new SelectListItem { Text = procUnionModel.UnionName, Value = $"{procUnionModel.UnionId}", Group = new SelectListGroup { Name = procUnionModel.BelongUnionName } });
                dossierTask.ListUnions = lstUnions;
            }

            ViewBag.ConfirmRollbackPrev = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_RollbackPrev"),
                $"<b>[{dossierTask.DossierName}]</b>");

            return PartialView("_Complete", dossierTask);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Edit)]
        [ValidateInput(false)]
        public Task<ActionResult> Complete(MajorDossierTaskModel model)
        {
            #region Valid

            var dossierModel = _dossierCache.GetById(model.DossierId);
            if (dossierModel == null)
                return Task.FromResult<ActionResult>(Json(new
                {
                    status = true,
                    message = CreateMessage($"{_dossierTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                }));
            var dossierTaskModel = _dossierCache.GetTaskById(model.TaskId);
            if (dossierTaskModel == null)
                return Task.FromResult<ActionResult>(Json(new
                {
                    status = true,
                    message = CreateMessage($"{_dossierTaskTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                }));

            var procedureView = JsonConvert.DeserializeObject<ViewProcedureStructureModel>(dossierModel.ProcConfigs);
            var currentStepView = procedureView.Steps.FirstOrDefault(s => s.StepId == dossierModel.InStep);
            if (currentStepView == null)
            {
                return Task.FromResult<ActionResult>(Json(new
                {
                    status = true,
                    message = CreateMessage($"{_stepTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                }));
            }

            #region Next Step

            var nextStepView = procedureView.Steps.FirstOrDefault(s => s.StepId == currentStepView.NextStep);
            if (nextStepView == null)
            {
                return Task.FromResult<ActionResult>(Json(new
                {
                    status = true,
                    message = CreateMessage($"{_stepTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                }));
            }

            if (model.IsRollbackPrev)
            {
                nextStepView = procedureView.Steps.FirstOrDefault(s => s.StepId == model.PrevStep);
                if (nextStepView == null)
                {
                    return Task.FromResult<ActionResult>(Json(new
                    {
                        status = true,
                        message = CreateMessage($"{_stepTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                    }));
                }
            }

            #endregion

            if (!ModelState.IsValid)
            {
                if (model.AllowChangeHandler && !string.IsNullOrEmpty(model.StepsChangeHandler))
                {
                    //if (model.StepHandlers == null || model.StepHandlers.Count <= 0)
                    //{
                    //    ModelState.AddModelError("StepHandlers", AppProcessor.Messagor.GetMessage("Task_HandledBy"));
                    //}

                    //var lstStepsChangeHandler = model.StepsChangeHandler.Split(';').ToList();
                    //model.ListStepsChangeHandler = procedureView.Steps
                    //    .Where(s => lstStepsChangeHandler.Exists(sch => $"{sch}".ToUpper() == $"{s.StepId}".ToUpper()))
                    //    .Select(s => new MajorStepChangeHandleModel
                    //    {
                    //        DossierId = model.DossierId,
                    //        UnionId = s.UnionHandle,
                    //        UnionName = s.UnionHandleName,
                    //        TaskId = model.TaskId,
                    //        StepId = s.StepId,
                    //        StepName = s.StepName,
                    //        StepDesc = s.StepDesc
                    //    }
                    //    ).ToList();

                    model.ListStaffs = new List<CateUnionMemberModel>();

                    var procUnionModel = _unionCache.GetById(procedureView.ProcUnionId);
                    model.ProcUnionId = procedureView.ProcUnionId;

                    var lstUnions = _unionCache
                        .GetBelong(procedureView.ProcUnionId, (int)EnumTypeUnion.Department)
                        .Where(u => u.IsActive).Select(u => new SelectListItem
                        {
                            Text = u.UnionName,
                            Value = u.UnionId.ToString(),
                            Group = new SelectListGroup { Name = u.BelongUnionName }
                        }).OrderBy(u => u.Group.Name).ToList();

                    lstUnions.Insert(0, new SelectListItem { Text = procUnionModel.UnionName, Value = $"{procUnionModel.UnionId}", Group = new SelectListGroup { Name = procUnionModel.BelongUnionName } });
                    model.ListUnions = lstUnions;

                    model.InStepView = currentStepView;

                    #region NextStep Handler

                    if (nextStepView.HandledBy == null && nextStepView.Handlers?.Count > 0)
                    {
                        var handler = nextStepView.Handlers.FirstOrDefault(h => h.UnionId == nextStepView.UnionHandle);
                        if (handler != null)
                        {
                            nextStepView.HandledBy = handler.StaffId;
                        }
                    }

                    if (nextStepView.UnionHandle != null && nextStepView.DeptHandle != null)
                    {
                        var lstStaffsInUnionHandleNextStep = _unionCache.GetMembersViaUnion(nextStepView.DeptHandle);
                        model.ListNextStepHandlers = lstStaffsInUnionHandleNextStep;
                        model.NextStepHandler = nextStepView.HandledBy;
                    }
                    else if (nextStepView.Handlers?.Count > 0)
                    {
                        var handler = nextStepView.Handlers.FirstOrDefault(h => h.UnionId == nextStepView.UnionHandle);
                        if (handler != null)
                        {
                            var lstStaffsInUnionHandleNextStep =
                                _unionCache.GetMembersViaUnion(handler.DeptId);
                            model.ListNextStepHandlers = lstStaffsInUnionHandleNextStep;
                            model.NextStepHandler = handler.StaffId;
                        }
                        else
                        {
                            var lstStaffsInUnionHandleNextStep = _unionCache.GetMembersViaUnion(nextStepView.DeptHandle);
                            model.ListNextStepHandlers = lstStaffsInUnionHandleNextStep;
                            model.NextStepHandler = nextStepView.HandledBy;
                        }
                    }
                    else
                    {
                        var lstStaffsInUnionHandleNextStep = _unionCache.GetMembersViaUnion(nextStepView.DeptHandle);
                        model.ListNextStepHandlers = lstStaffsInUnionHandleNextStep;
                        model.NextStepHandler = nextStepView.HandledBy;
                    }

                    #endregion
                }

                return Task.FromResult<ActionResult>(PartialView("_CompleteBody", model));
            }

            #endregion

            bool isFinishDossier = false;

            #region Allow Change Handler For Task

            if (model.AllowChangeHandler)
            {
                procedureView.Steps.ForEach(s =>
                {
                    if (s.Handlers == null)
                    {
                        s.Handlers = new List<ViewHandlerStepStructureModel>();
                    }

                    var stepChangeHandler =
                        model.ListStepsChangeHandler.FirstOrDefault(step => step.StepId == s.StepId);
                    if (stepChangeHandler != null)
                    {
                        if (!s.Handlers.Exists(h =>
                                h.UnionId == model.UnionHandle &&
                                h.StaffId == stepChangeHandler.StaffId))
                        {
                            s.Handlers.Add(new ViewHandlerStepStructureModel
                            {
                                UnionId = model.UnionHandle,
                                UnionName = model.UnionHandleName,
                                DeptId = stepChangeHandler.UnionId,
                                DeptName = stepChangeHandler.UnionName,
                                StaffId = stepChangeHandler.StaffId,
                                StaffName = stepChangeHandler.StaffName,
                                AllowSwitchHandler = stepChangeHandler.AllowSwitchHandler
                            });
                        }

                        s.UnionHandle = model.UnionHandle;
                        s.UnionHandleName = model.UnionHandleName;
                        s.DeptHandle = stepChangeHandler.UnionId;
                        s.DeptHandleName = stepChangeHandler.UnionName;
                        s.HandledBy = stepChangeHandler.StaffId;
                        s.AllowSwitchHandler = stepChangeHandler.AllowSwitchHandler;
                    }

                    //if (model.StepHandlers.ContainsKey($"{s.StepId}"))
                    //{


                    //    if (!s.Handlers.Exists(h =>
                    //            h.UnionId == model.UnionHandle &&
                    //            h.StaffId == model.StepHandlers[$"{s.StepId}"].HandledBy))
                    //    {
                    //        s.Handlers.Add(new ViewHandlerStepStructureModel
                    //        {
                    //            UnionId = model.UnionHandle,
                    //            UnionName = model.UnionHandleName,
                    //            DeptId = model.StepHandlers[$"{s.StepId}"].DeptHandle,
                    //            DeptName = model.StepHandlers[$"{s.StepId}"].DeptHandleName,
                    //            StaffId = model.StepHandlers[$"{s.StepId}"].HandledBy,
                    //            StaffName = model.StepHandlers[$"{s.StepId}"].HandledByName,
                    //            AllowSwitchHandler = model.StepHandlers[$"{s.StepId}"].AllowSwitchHandler
                    //        });
                    //    }

                    //    s.UnionHandle = model.UnionHandle;
                    //    s.UnionHandleName = model.UnionHandleName;
                    //    s.DeptHandle = model.StepHandlers[$"{s.StepId}"].DeptHandle;
                    //    s.DeptHandleName = model.StepHandlers[$"{s.StepId}"].DeptHandleName;
                    //    s.HandledBy = model.StepHandlers[$"{s.StepId}"].HandledBy;
                    //    s.AllowSwitchHandler = model.StepHandlers[$"{s.StepId}"].AllowSwitchHandler;
                    //}
                });

                var procConfigs = JsonConvert.SerializeObject(procedureView);
                var retUpdateProcConfig = _dossierCache.UpdateProcConfig(new MajorDossierModel
                {
                    DossierId = model.DossierId,
                    ProcConfigs = procConfigs
                });

                if (retUpdateProcConfig <= 0)
                {
                    return Task.FromResult<ActionResult>(Json(new { status = true, message = CreateMessage(string.Format(AppProcessor.Messagor.GetMessage("Modal_Title_Complete"), $"{_dossierTaskTitle} [{model.InStepName}]"), EnumProcessType.NonFormat, EnumMsgIcon.Error) }, JsonRequestBehavior.AllowGet));
                }
            }

            #endregion

            #region Incase Selected Condition

            if (!model.IsRollbackPrev && model.SelectedSituation != null && model.SelectedSituation != Guid.Empty)
            {
                var selectedSituationModel =
                    currentStepView.Situations.FirstOrDefault(si => si.SituationId == model.SelectedSituation);
                if (selectedSituationModel == null)
                {
                    return Task.FromResult<ActionResult>(Json(new
                    {
                        status = true,
                        message = CreateMessage($"{_situationTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                    }));
                }

                nextStepView = procedureView.Steps.FirstOrDefault(s => s.StepId == selectedSituationModel.NextStep);
                if (nextStepView == null)
                {
                    return Task.FromResult<ActionResult>(Json(new
                    {
                        status = true,
                        message = CreateMessage($"{_stepTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                    }));
                }
                if (nextStepView.Handlers?.Count > 0)
                {
                    var handler = nextStepView.Handlers.FirstOrDefault(h => h.UnionId == nextStepView.UnionHandle);
                    model.NextStepHandler = handler != null ? handler.StaffId : nextStepView.HandledBy;
                }
                else
                {
                    model.NextStepHandler = nextStepView.HandledBy;
                }
            }

            #endregion

            if (nextStepView.StepType == "End")
            {
                isFinishDossier = true;
            }

            if (!model.AllowChangeHandler && !model.IsRollbackPrev && nextStepView.HandledBy != model.NextStepHandler)
            {
                nextStepView.HandledBy = model.NextStepHandler;

                var procConfigs = JsonConvert.SerializeObject(procedureView);
                var retUpdateProcConfig = _dossierCache.UpdateProcConfig(new MajorDossierModel
                {
                    DossierId = model.DossierId,
                    ProcConfigs = procConfigs
                });

                if (retUpdateProcConfig <= 0)
                {
                    return Task.FromResult<ActionResult>(Json(new { status = true, message = CreateMessage(string.Format(AppProcessor.Messagor.GetMessage("Modal_Title_Complete"), $"{_dossierTaskTitle} [{model.InStepName}]"), EnumProcessType.NonFormat, EnumMsgIcon.Error) }, JsonRequestBehavior.AllowGet));
                }
            }

            #region Process Actions

            model.IsFinish = isFinishDossier;

            model.NextStep = nextStepView.StepId;
            model.NextStepName = nextStepView.StepName;
            model.UnionHandle = nextStepView.Handlers.FirstOrDefault(h => h.UnionId == nextStepView.UnionHandle)?.DeptId ?? (nextStepView.DeptHandle ?? nextStepView.UnionHandle);
            //model.HandledBy = isFinishDossier ? currentStepView.HandledBy : nextStepView.HandledBy;

            model.HandledBy = isFinishDossier ? User.UserName : nextStepView.HandledBy ?? nextStepView.Handlers.FirstOrDefault(h => h.DeptId == nextStepView.DeptHandle)?.StaffId;

            model.PositionId = nextStepView.PositionId;
            model.HandlingTime = nextStepView.TotalHandlingTimes();

            model.Status = (int)EnumDossierTaskStatus.Completed;
            model.StatusName = AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(EnumDossierTaskStatus.Completed));
            model.NextStatus = (int)EnumDossierTaskStatus.Handling;
            model.NextStatusName = AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(EnumDossierTaskStatus.Handling));

            model.AllowSwitchHandler = nextStepView.AllowSwitchHandler ?? false;

            model.UpdatedBy = User.UserName;

            var saveResult = _dossierCache.Complete(model);

            #endregion

            //Thành công thì lưu các tệp đính kèm
            if (saveResult > 0)
            {
                var sender = User.UserName;
                var urlSearchContract = Request.Url.GetLeftPart(UriPartial.Authority);
                var urlSearchDossier = Url.Action("Index", "Dossier", new { q = dossierModel.DossierCode });

                var retSaveFiles = SaveRefFiles(model.ResultFiles, model.DossierId, model.TaskId, sender, out string errMsg);
                if (!retSaveFiles)
                {
                    AppProcessor.Logger.Message($"{errMsg} - {model.DossierId} - {model.TaskId}");
                }

                Task.Run(() =>
                    {
                        try
                        {
                            var contractModel = _contractCache.GetById(model.DossierId);
                            var cusInfo = _contractCache.GetCus(contractModel.ContractId);
                            var handleInfo = _userCache.GetByUserName(model.HandledBy);
                            //if (isFinishDossier)
                            //{
                            //    var lstStepsInProc = _stepCache.GetAll(model.ProcedureId.ToString());
                            //    var startStep = lstStepsInProc.FirstOrDefault(s => s.PrevStep == null && s.StepType == "Start");
                            //    var firstStep = lstStepsInProc.FirstOrDefault(s => s.StepId == startStep?.NextStep);
                            //    if (firstStep != null)
                            //    {
                            //        var firstStepView = lstStepsInProc.FirstOrDefault(s => s.StepId == firstStep.StepId);
                            //        if (firstStepView != null)
                            //        {
                            //            if (!string.IsNullOrEmpty(firstStepView.StaffNotificationConfigs))
                            //            {
                            //                firstStepView.StaffNotificationConfigs.Split(';').ToList().ForEach(t =>
                            //                {
                            //                    var libNotify = _listNotificationProviders.FirstOrDefault(n => n.Name == t);
                            //                    if (libNotify != null)
                            //                    {
                            //                        libNotify.Push(new ContentNotifyModel
                            //                        {
                            //                            TypeEmail = EnumTypeEmail.ContractResult,
                            //                            ContractInfo = new Contract
                            //                            {
                            //                                ContractNo = contractModel.ContractNo,
                            //                                ContractSignal = contractModel.ContractSignal,
                            //                                SearchContractDetailUrl = urlSearchContract,
                            //                                SearchContractUrl = urlSearchContract
                            //                            },
                            //                            CusInfo = new Customer
                            //                            {
                            //                                CusName = handleInfo.FullName,
                            //                                Email = handleInfo.Email,
                            //                                Phone = handleInfo.Phone
                            //                            },
                            //                            InsiteNotification = new InsiteNotificationModel
                            //                            {
                            //                                Icon = EnumMsgIcon.Info,
                            //                                Title = AppProcessor.Messagor.GetMessage("Dossier_Notify_Title_Handing"),
                            //                                Message = string.Format(
                            //                                    AppProcessor.Messagor.GetMessage("Notify_Title_Handing_Contract"),
                            //                                    contractModel.ContractNoInfo),
                            //                                Placement = "tr",
                            //                                Url = urlSearchDossier,
                            //                                Sender = sender,
                            //                                Receiver = model.HandledBy
                            //                            }
                            //                        });
                            //                    }
                            //                });
                            //            }
                            //        }
                            //    }
                            //}
                            //else
                            if (!isFinishDossier)
                            {
                                if (!string.IsNullOrEmpty(nextStepView.StaffNotificationConfigs))
                                {
                                    currentStepView.StaffNotificationConfigs.Split(';').ToList().ForEach(t =>
                                    {
                                        var libNotify = _listNotificationProviders.FirstOrDefault(n => n.Name == t);
                                        if (libNotify != null)
                                        {
                                            libNotify.Push(new ContentNotifyModel
                                            {
                                                TypeEmail = EnumTypeEmail.ContractPending,
                                                ContractInfo = new Contract
                                                {
                                                    ContractNo = contractModel.ContractNo,
                                                    ContractSignal = contractModel.ContractSignal,
                                                    SearchContractDetailUrl = urlSearchContract,
                                                    SearchContractUrl = urlSearchContract
                                                },
                                                CusInfo = new Customer
                                                {
                                                    CusName = handleInfo.FullName,
                                                    Email = handleInfo.Email,
                                                    Phone = handleInfo.Phone
                                                },
                                                InsiteNotification = new InsiteNotificationModel
                                                {
                                                    Icon = EnumMsgIcon.Info,
                                                    Title = AppProcessor.Messagor.GetMessage("Dossier_Notify_Title_Handing"),
                                                    Message = string.Format(
                                                        AppProcessor.Messagor.GetMessage("Notify_Title_Handing_Contract"),
                                                        contractModel.ContractNoInfo),
                                                    Placement = "tr",
                                                    Url = urlSearchDossier,
                                                    Sender = sender,
                                                    Receiver = model.HandledBy
                                                }
                                            });
                                        }
                                    });
                                }
                            }

                            #region Gửi tin nhắn thông báo tới khách hàng

                            // thong tin doser sau khi luu                
                            var unionParent = _unionCache.GetById(contractModel.UnionId);
                            var newContract = _contractCache.GetById(model.DossierId);
                            if (newContract.Status == 3 || newContract.Status == 99)
                            {
                                Dictionary<string, string> dictUnionInfo = null;
                                var unionInfo = unionParent.UnionInfo;
                                var nameUnion = "";
                                var addressUnion = "";
                                var emailUnion = "";
                                var phoneUnion = "";

                                // Deserialize thông tin từ đối tượng JSON thành một từ điển
                                if (!string.IsNullOrEmpty(unionInfo))
                                {
                                    dictUnionInfo = JsonConvert.DeserializeObject<Dictionary<string, string>>(unionInfo);
                                }

                                if (dictUnionInfo != null)
                                {
                                    foreach (var kvp in dictUnionInfo)
                                    {
                                        var key = kvp.Key;
                                        var value = kvp.Value;

                                        switch (key)
                                        {
                                            case "EnterpriseName":
                                                nameUnion = value;
                                                break;
                                            case "Email":
                                                emailUnion = value;
                                                break;
                                            case "Phone":
                                                phoneUnion = value;
                                                break;
                                            case "EnterpriseAddress":
                                                addressUnion = value;
                                                break;
                                        }
                                    }
                                }

                                if (!string.IsNullOrEmpty(cusInfo.Email))
                                {
                                    var modelNotification = new ContentNotificationModel
                                    {
                                        TypeEmail = newContract.Status == 3 ? EnumTypeEmail.ContractResult : EnumTypeEmail.ContractRejection,
                                        ContractInfo = new Contract
                                        {
                                            ContractNo = contractModel.ContractNo,
                                            ContractSignal = contractModel.ContractSignal,
                                            ContractNoInfo = contractModel.ContractNoInfo,
                                            SearchContractDetailUrl = urlSearchContract,
                                            SearchContractUrl = urlSearchContract
                                        },
                                        CusInfo = new Customer
                                        {
                                            CusName = contractModel.CusName,
                                            Email = cusInfo.Email,
                                            Phone = cusInfo.Phone,
                                            TypeCus = cusInfo.TypeCus
                                        },
                                        UnionInfo = new Union
                                        {
                                            UnionName = nameUnion,
                                            Address = addressUnion,
                                            Email = emailUnion,
                                            Phone = phoneUnion
                                        }
                                    };

                                    SendNotificationHelper.Send(modelNotification);
                                }

                                if (newContract.Status == 3)
                                {
                                    // gửi tin nhắn
                                    var isSuccess = SmsProvider.Send(out string msgErr, cusInfo.Phone, EnumContractStatusHandle.Resolved, contractModel.ContractNoInfo);
                                    AppProcessor.Logger.Message($"[{contractModel.ContractNoInfo}] - Gửi tin nhắn hoàn thành hợp đồng tới khách hàng: {(isSuccess ? "Thành công" : $"Thất bại ({msgErr})")}");

                                    //SMSProvider.Send_SMS_Contract_Resolved(cusInfo.Phone, contractModel.ContractNoInfo);
                                }
                                else
                                {
                                    // gửi tin nhắn
                                    var isSuccess = SmsProvider.Send(out string msgErr, cusInfo.Phone, EnumContractStatusHandle.Refuse, contractModel.ContractNoInfo);
                                    AppProcessor.Logger.Message($"[{contractModel.ContractNoInfo}] - Gửi tin nhắn huỷ hợp đồng tới khách hàng: {(isSuccess ? "Thành công" : $"Thất bại ({msgErr})")}");

                                    //SMSProvider.Send_SMS_Contract_Refuse(cusInfo.Phone, contractModel.ContractNoInfo);
                                }
                            }

                            #endregion

                        }
                        catch (Exception ex)
                        {
                            AppProcessor.Logger.Error(ex);
                            if (ex.InnerException != null)
                                AppProcessor.Logger.Error(ex.InnerException);
                        }
                    });
            }

            var response = CreateMessage(string.Format(AppProcessor.Messagor.GetMessage("Modal_Title_Complete"), $"{_dossierTaskTitle} [{model.InStepName}]"), EnumProcessType.NonFormat, saveResult == 0 ? EnumMsgIcon.Error : EnumMsgIcon.Success);
            return Task.FromResult<ActionResult>(Json(new { status = true, message = response }, JsonRequestBehavior.AllowGet));
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult PauseTask(Guid? id)
        {
            var dossierTask = _dossierCache.GetTaskById(id);
            if (dossierTask == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_dossierTaskTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var dossier = _dossierCache.GetById(dossierTask.DossierId);
            if (dossier == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_dossierTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Pause"),
                $"<b>{_dossierTaskTitle} [{dossierTask.InStepName}]</b>");

            dossierTask.IsPause = true;

            return PartialView("_PauseTask", dossierTask);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult PauseTask(MajorDossierTaskModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Pause"),
                    $"<b>{_dossierTaskTitle} [{model.InStepName}]</b>");
                return PartialView("_PauseTaskBody", model);
            }

            model.Status = (int)EnumDossierTaskStatus.Paused;
            model.StatusName = AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(EnumDossierTaskStatus.Paused));

            model.ContractStatus = (int)EnumContractStatus.Paused;
            model.ContractStatusName = AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(EnumContractStatus.Paused));

            model.UpdatedBy = User.UserName;

            var ret = _dossierCache.PauseTask(model);
            var response = CreateMessage($"{_dossierTaskTitle} [{model.InStepName}]",
                EnumProcessType.Edit, ret > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response });
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult ContinueTask(Guid? id)
        {
            var dossierTask = _dossierCache.GetTaskById(id);
            if (dossierTask == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_dossierTaskTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var dossier = _dossierCache.GetById(dossierTask.DossierId);
            if (dossier == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_dossierTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Continue"),
                $"<b>{_dossierTaskTitle} [{dossierTask.InStepName}]</b>");

            return PartialView("_ContinueTask", dossierTask);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult ContinueTask(MajorDossierTaskModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Continue"),
                    $"<b>{_dossierTaskTitle} [{model.InStepName}]</b>");
                return PartialView("_ContinueTaskBody", model);
            }

            model.Status = (int)EnumDossierTaskStatus.Handling;
            model.StatusName = AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(EnumDossierTaskStatus.Handling));

            model.ContractStatus = (int)EnumContractStatus.Handling;
            model.ContractStatusName = AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(EnumContractStatus.Handling));

            model.UpdatedBy = User.UserName;
            var ret = _dossierCache.ContinueTask(model);
            var response = CreateMessage($"{_dossierTaskTitle} [{model.InStepName}]",
                EnumProcessType.Edit, ret > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response });
        }

        #endregion

        #region Handlers

        private readonly string _handlerByTitle = AppProcessor.Messagor.GetMessage("Task_HandledBy");

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult SwitchHandlers(Guid? id)
        {
            var dossierTask = _dossierCache.GetTaskById(id);
            if (dossierTask == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_dossierTaskTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            Session[$"DossierStepHandlers-{User.UserName}-{dossierTask.TaskId}"] = null;

            //var lstDossierStepHandlers = Session[$"DossierStepHandlers-{User.UserName}-{dossierTask.TaskId}"] as List<MajorDossierTaskSwitchHandlerModel>;

            var lstDossierStepHandlers = new List<MajorDossierTaskSwitchHandlerModel>();
            MajorDossierSwitchHandlerTaskModel switchHandlerTaskModel = new MajorDossierSwitchHandlerTaskModel
            {
                TaskId = dossierTask.TaskId,
                TaskName = dossierTask.InStepName,
                InStepName = dossierTask.InStepName,
                HandelBy = dossierTask.HandledBy,
                HandelByName = dossierTask.HandledByName,
                DossierName = dossierTask.DossierName,
                DossierCode = dossierTask.DossierCode,
                DataTaskHandlers = lstDossierStepHandlers
            };

            return PartialView("_SwitchHandlers", switchHandlerTaskModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Edit)]
        [ValidateInput(false)]
        public ActionResult SwitchHandlers(MajorDossierSwitchHandlerTaskModel model)
        {
            var dossierTask = _dossierCache.GetTaskById(model.TaskId);
            if (dossierTask == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_dossierTaskTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var lstDossierStepHandlers = Session[$"DossierStepHandlers-{User.UserName}-{model.TaskId}"] as List<MajorDossierTaskSwitchHandlerModel>;
            //lstDossierStepHandlers = lstDossierStepHandlers ?? new List<MajorDossierTaskSwitchHandlerModel>();

            if (lstDossierStepHandlers == null)
            {
                return Json(new
                {
                    status = true,
                    message = CreateMessage("Bạn chưa chọn nhân viên để chuyển xử lý", EnumProcessType.NonFormat, EnumMsgIcon.Error)
                });
            }

            var dataDossierStepHandlers = new DataTable();

            using (var reader = ObjectReader.Create(lstDossierStepHandlers, "Handler", "IsPrimary"))
            {
                dataDossierStepHandlers.Load(reader);
            }

            var saveResult = _dossierCache.SwitchHandler(model.TaskId, model.HandlingComments, dataDossierStepHandlers, User.UserName);

            var dossierModel = _dossierCache.GetById(dossierTask.DossierId);

            var procedureView = JsonConvert.DeserializeObject<ViewProcedureStructureModel>(dossierModel.ProcConfigs);
            var currentStepView = procedureView.Steps.FirstOrDefault(s => s.StepId == dossierModel.InStep);
            if (currentStepView == null)
            {
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_stepTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            }
            var nextStepView = procedureView.Steps.FirstOrDefault(s => s.StepId == currentStepView.NextStep);
            if (nextStepView == null)
            {
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_stepTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            }
            var contractModel = _contractCache.GetById(dossierModel.DossierId);
            if (!string.IsNullOrEmpty(nextStepView.StaffNotificationConfigs))
            {
                var sender = User.UserName;
                var urlSearchContract = Request.Url.GetLeftPart(UriPartial.Authority);
                var urlSearchDossier = Url.Action("Index", "Dossier", new { q = contractModel.ContractNoInfo });

                Task.Run(() =>
                {
                    try
                    {
                        currentStepView.StaffNotificationConfigs.Split(';').ToList().ForEach(t =>
                        {
                            var libNotify = _listNotificationProviders.FirstOrDefault(n => n.Name == t);
                            foreach (DataRow row in dataDossierStepHandlers.Rows)
                            {
                                if (libNotify != null)
                                {
                                    var handler = row["Handler"].ToString();
                                    var emailHandler = _userCache.GetByUserName(handler);
                                    libNotify.Push(new ContentNotifyModel
                                    {
                                        TypeEmail = EnumTypeEmail.ContractPending,
                                        ContractInfo = new Contract
                                        {
                                            ContractNo = contractModel.ContractNo,
                                            ContractSignal = contractModel.ContractSignal,
                                            SearchContractDetailUrl = urlSearchContract,
                                            SearchContractUrl = urlSearchContract
                                        },
                                        CusInfo = new Customer
                                        {
                                            CusName = emailHandler.FullName,
                                            Email = emailHandler.Email,
                                            Phone = emailHandler.Phone
                                        },
                                        InsiteNotification = new InsiteNotificationModel
                                        {
                                            Icon = EnumMsgIcon.Info,
                                            Title = AppProcessor.Messagor.GetMessage("Dossier_Notify_Title_Handing"),
                                            Message = string.Format(AppProcessor.Messagor.GetMessage("Notify_Title_Handing_Contract"), contractModel.ContractNoInfo),
                                            Placement = "tr",
                                            Url = urlSearchDossier,
                                            Sender = sender,
                                            Receiver = handler
                                        }
                                    });

                                }
                            }
                        });
                    }
                    catch (Exception ex)
                    {
                        AppProcessor.Logger.Error(ex);
                        if (ex.InnerException != null)
                            AppProcessor.Logger.Error(ex.InnerException);
                    }
                });
            }
            var response = CreateMessage($"{_handlerByTitle} [{model.DossierName}]", EnumProcessType.Add, saveResult == 0 ? EnumMsgIcon.Error : EnumMsgIcon.Success);
            return Json(new { status = true, message = response }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult AddHandler(Guid? taskId)
        {
            var dossierTask = _dossierCache.GetTaskById(taskId);
            if (dossierTask == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_dossierTaskTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var lstDossierStepHandlers = Session[$"DossierStepHandlers-{User.UserName}-{taskId}"] as List<MajorDossierTaskSwitchHandlerModel>;
            lstDossierStepHandlers = lstDossierStepHandlers ?? new List<MajorDossierTaskSwitchHandlerModel>();

            //var unionViaCurrentHandle = _unionCache.GetMemberByKey(null, User.UserName);
            //if (unionViaCurrentHandle == null)
            //{
            //    return Json(new
            //    {
            //        status = true,
            //        message = CreateMessage(AppProcessor.Messagor.GetMessage("Err_Account_Not_Belong_Union"), EnumProcessType.NonFormat, EnumMsgIcon.Error)
            //    });
            //}

            ////Trực thuộc Đơn vị
            //var belongUnion = _unionCache.GetParents(unionViaCurrentHandle.UnionId, (int)EnumTypeUnion.Unit)
            //    ?.FirstOrDefault();
            //if (belongUnion == null)
            //{
            //    return Json(new
            //    {
            //        status = true,
            //        message = CreateMessage(AppProcessor.Messagor.GetMessage("Err_Account_Not_Belong_Union"), EnumProcessType.NonFormat, EnumMsgIcon.Error)
            //    });
            //}

            //Thông tin phòng ban
            var unionMemberInfo = _unionCache.GetMemberInfo(User.UserName);
            if (unionMemberInfo == null)
            {
                return Json(new
                {
                    status = true,
                    message = CreateMessage(AppProcessor.Messagor.GetMessage("Err_Account_Not_Belong_Union"), EnumProcessType.NonFormat, EnumMsgIcon.Error)
                });
            }

            //Trực thuộc Đơn vị
            var unionViaMember = _unionCache.GetUnionByMember(User.UserName);
            if (unionViaMember == null)
            {
                return Json(new
                {
                    status = true,
                    message = CreateMessage(AppProcessor.Messagor.GetMessage("Err_Account_Not_Belong_Union"), EnumProcessType.NonFormat, EnumMsgIcon.Error)
                });
            }

            var handleModel = new MajorDossierTaskSwitchHandlerModel
            {
                HandlerId = Guid.NewGuid(),
                TaskId = taskId,
                TaskName = dossierTask.InStepName,

                UnionId = unionViaMember.UnionId,
                UnionName = unionViaMember.UnionName,
                DeptId = unionMemberInfo.UnionId,
                DeptName = unionMemberInfo.UnionName,
                PositionId = unionMemberInfo.PositionId,
                PositionName = unionMemberInfo.PositionName,
                //StaffId = User.UserName,
                //StaffName = User.FullName,

                ListPositions = _positionCache.GetAll()
                    .Select(u => new ListItem
                    {
                        Text = u.PositionName,
                        Value = u.PositionID.ToString()
                    }).ToList(),
                StaffsViaDept = _unionCache.GetMembersViaUnion(unionMemberInfo.UnionId)
                    .Where(s => !lstDossierStepHandlers.Exists(h => h.StaffId == s.UserName))
                    .OrderBy(s => s.FullName).ToList(),

                //ListStaffs = _unionCache.GetMembers(unionViaCurrentHandle.UnionId)
                //    .Where(s => s.UserName != User.UserName && !lstDossierStepHandlers.Exists(h => h.StaffId == s.UserName))
                //    .OrderBy(s => s.FullName)
                //    .Select(u => new SelectListItem
                //    {
                //        Text = u.FullName,
                //        Value = u.UserName,
                //        Group = new SelectListGroup { Name = u.UnionName }
                //    }).ToList(),

                HasPrimary = lstDossierStepHandlers.Exists(h => h.IsPrimary)
            };

            return PartialView("_AddHandler", handleModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Add)]
        [ValidateInput(false)]
        public ActionResult AddHandler(MajorDossierTaskSwitchHandlerModel model)
        {
            var lstDossierStepHandlers = Session[$"DossierStepHandlers-{User.UserName}-{model.TaskId}"] as List<MajorDossierTaskSwitchHandlerModel>;
            lstDossierStepHandlers = lstDossierStepHandlers ?? new List<MajorDossierTaskSwitchHandlerModel>();

            var staffsViaDept = _unionCache.GetMembersViaUnion(model.DeptId)
                .OrderBy(s => s.FullName).ToList();

            model.HasPrimary = lstDossierStepHandlers.Exists(h => h.IsPrimary);

            if (!ModelState.IsValid)
            {
                model.ListPositions = _positionCache.GetAll()
                    .Select(u => new ListItem
                    {
                        Text = u.PositionName,
                        Value = u.PositionID.ToString()
                    }).ToList();

                model.StaffsViaDept = staffsViaDept;

                return PartialView("_Handler", model);
            }

            if (string.IsNullOrEmpty(model.PrimaryHandler) && !model.HasPrimary)
            {
                model.PrimaryHandler = model.TaskHandlers.First();
            }

            model.TaskHandlers.ForEach(h =>
            {
                var staffModel = staffsViaDept.FirstOrDefault(s => s.UserName == h);
                if (staffModel != null)
                {
                    lstDossierStepHandlers.Add(new MajorDossierTaskSwitchHandlerModel
                    {
                        HandlerId = Guid.NewGuid(),
                        TaskId = model.TaskId,
                        StaffName = staffModel.FullName,
                        StaffId = staffModel.UserName,
                        IsPrimary = staffModel.UserName == model.PrimaryHandler,
                        PositionName = staffModel.PositionName
                    });
                }
            });

            Session[$"DossierStepHandlers-{User.UserName}-{model.TaskId}"] = lstDossierStepHandlers;

            var jsonDossierStepHandlers = JsonConvert.SerializeObject(lstDossierStepHandlers);
            var response = CreateMessage($"{_handlerByTitle}", EnumProcessType.Add, EnumMsgIcon.Success);
            return Json(new { status = true, message = response, data = jsonDossierStepHandlers }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult DeleteHandler(Guid? taskId, Guid? handlerId)
        {
            var lstDossierStepHandlers = Session[$"DossierStepHandlers-{User.UserName}-{taskId}"] as List<MajorDossierTaskSwitchHandlerModel>;
            lstDossierStepHandlers = lstDossierStepHandlers ?? new List<MajorDossierTaskSwitchHandlerModel>();
            var stepHandler = lstDossierStepHandlers.FirstOrDefault(t => t.HandlerId == handlerId);

            if (stepHandler == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_handlerByTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Delete"),
                $"<b>{_handlerByTitle} [{stepHandler.StaffName}]</b>");

            return PartialView("_DeleteHandler", stepHandler);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult DeleteHandler(MajorDossierTaskSwitchHandlerModel model)
        {
            var lstDossierStepHandlers = Session[$"DossierStepHandlers-{User.UserName}-{model.TaskId}"] as List<MajorDossierTaskSwitchHandlerModel>;
            if (lstDossierStepHandlers == null)
                return Json(new
                {
                    status = false,
                    message = CreateMessage($"{_handlerByTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var stepHandler = lstDossierStepHandlers.FirstOrDefault(t => t.HandlerId == model.HandlerId);
            if (stepHandler == null)
                return Json(new
                {
                    status = false,
                    message = CreateMessage($"{_handlerByTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var staffName = stepHandler.StaffName;

            var deleted = lstDossierStepHandlers.RemoveAll(t => t.HandlerId == model.HandlerId);

            if (deleted > -1)
            {
                Session[$"DossierStepHandlers-{User.UserName}-{model.TaskId}"] = lstDossierStepHandlers;
            }
            var jsonDossierStepHandlers = JsonConvert.SerializeObject(lstDossierStepHandlers);

            var response = CreateMessage($"{_handlerByTitle} [{staffName}]",
                EnumProcessType.Delete, deleted > -1 ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response, data = jsonDossierStepHandlers });
        }

        #endregion

        #region Supervisor

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult ChangeHandler(Guid? id)
        {
            var dossierTask = _dossierCache.GetTaskById(id);
            if (dossierTask == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_dossierTaskTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var dossierModel = _dossierCache.GetById(dossierTask.DossierId);

            #region Procedure Structure View

            var procedureView = JsonConvert.DeserializeObject<ViewProcedureStructureModel>(dossierModel.ProcConfigs);
            var currentStepView = procedureView.Steps.FirstOrDefault(s => s.StepId == dossierModel.InStep);
            if (currentStepView == null)
            {
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_stepTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            }

            #endregion

            #region NextStep Handler

            //var nextStepView = procedureView.Steps.FirstOrDefault(s => s.StepId == currentStepView.NextStep);
            //if (nextStepView != null)
            {
                if (currentStepView.HandledBy == null && currentStepView.Handlers?.Count > 0)
                {
                    var handler = currentStepView.Handlers.FirstOrDefault(h => h.UnionId == currentStepView.UnionHandle);
                    if (handler != null)
                    {
                        currentStepView.HandledBy = handler.StaffId;
                    }
                }

                if (currentStepView.UnionHandle != null && currentStepView.DeptHandle != null)
                {
                    var lstStaffsInUnionHandleNextStep = _unionCache.GetMembersViaUnion(currentStepView.UnionHandle, true);
                    dossierTask.ListNextStepHandlers = lstStaffsInUnionHandleNextStep;
                    dossierTask.NextStepHandler = dossierTask.HandledBy ?? currentStepView.HandledBy;
                }
                else if (currentStepView.Handlers?.Count > 0)
                {
                    var handler = currentStepView.Handlers.FirstOrDefault(h => h.UnionId == currentStepView.UnionHandle);
                    if (handler != null)
                    {
                        var lstStaffsInUnionHandleNextStep =
                            _unionCache.GetMembersViaUnion(handler.DeptId);
                        dossierTask.ListNextStepHandlers = lstStaffsInUnionHandleNextStep;
                        dossierTask.NextStepHandler = handler.StaffId;
                    }
                    else
                    {
                        var lstStaffsInUnionHandleNextStep = _unionCache.GetMembersViaUnion(currentStepView.DeptHandle);
                        dossierTask.ListNextStepHandlers = lstStaffsInUnionHandleNextStep;
                        dossierTask.NextStepHandler = currentStepView.HandledBy;
                    }
                }
                else
                {
                    var lstStaffsInUnionHandleNextStep = _unionCache.GetMembersViaUnion(currentStepView.DeptHandle);
                    dossierTask.ListNextStepHandlers = lstStaffsInUnionHandleNextStep;
                    dossierTask.NextStepHandler = currentStepView.HandledBy;
                }
            }

            #endregion

            dossierTask.InStepView = currentStepView;
            dossierTask.DossierCode = dossierModel.DossierCode;

            ViewBag.ConfirmRollbackPrev = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_RollbackPrev"),
                $"<b>[{dossierTask.DossierName}]</b>");

            return PartialView("_ChangeHandler", dossierTask);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Edit)]
        [ValidateInput(false)]
        public ActionResult ChangeHandler(MajorDossierTaskModel model)
        {
            #region Valid

            var dossierModel = _dossierCache.GetById(model.DossierId);
            if (dossierModel == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_dossierTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            var dossierTaskModel = _dossierCache.GetTaskById(model.TaskId);
            if (dossierTaskModel == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_dossierTaskTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var procedureView = JsonConvert.DeserializeObject<ViewProcedureStructureModel>(dossierModel.ProcConfigs);
            var currentStepView = procedureView.Steps.FirstOrDefault(s => s.StepId == dossierModel.InStep);
            if (currentStepView == null)
            {
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_stepTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            }

            #region Next Step

            var nextStepView = procedureView.Steps.FirstOrDefault(s => s.StepId == currentStepView.NextStep);
            if (nextStepView == null)
            {
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_stepTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            }

            if (model.IsRollbackPrev)
            {
                nextStepView = procedureView.Steps.FirstOrDefault(s => s.StepId == model.PrevStep);
                if (nextStepView == null)
                {
                    return Json(new
                    {
                        status = true,
                        message = CreateMessage($"{_stepTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                    });
                }
            }

            #endregion

            if (!ModelState.IsValid)
            {
                if (nextStepView.HandledBy == null && nextStepView.Handlers?.Count > 0)
                {
                    var handler = nextStepView.Handlers.FirstOrDefault(h => h.UnionId == nextStepView.UnionHandle);
                    if (handler != null)
                    {
                        nextStepView.HandledBy = handler.StaffId;
                    }
                }

                if (nextStepView.UnionHandle != null && nextStepView.DeptHandle != null)
                {
                    var lstStaffsInUnionHandleNextStep = _unionCache.GetMembersViaUnion(nextStepView.UnionHandle, true);
                    model.ListNextStepHandlers = lstStaffsInUnionHandleNextStep;
                    model.NextStepHandler = nextStepView.HandledBy;
                }
                else if (nextStepView.Handlers?.Count > 0)
                {
                    var handler = nextStepView.Handlers.FirstOrDefault(h => h.UnionId == nextStepView.UnionHandle);
                    if (handler != null)
                    {
                        var lstStaffsInUnionHandleNextStep =
                            _unionCache.GetMembersViaUnion(handler.DeptId);
                        model.ListNextStepHandlers = lstStaffsInUnionHandleNextStep;
                        model.NextStepHandler = handler.StaffId;
                    }
                    else
                    {
                        var lstStaffsInUnionHandleNextStep = _unionCache.GetMembersViaUnion(nextStepView.DeptHandle);
                        model.ListNextStepHandlers = lstStaffsInUnionHandleNextStep;
                        model.NextStepHandler = nextStepView.HandledBy;
                    }
                }
                else
                {
                    var lstStaffsInUnionHandleNextStep = _unionCache.GetMembersViaUnion(nextStepView.DeptHandle);
                    model.ListNextStepHandlers = lstStaffsInUnionHandleNextStep;
                    model.NextStepHandler = nextStepView.HandledBy;
                }

                return PartialView("_ChangeHandlerBody", model);
            }

            #endregion

            {
                nextStepView.HandledBy = model.NextStepHandler;

                var procConfigs = JsonConvert.SerializeObject(procedureView);
                var retUpdateProcConfig = _dossierCache.UpdateProcConfig(new MajorDossierModel
                {
                    DossierId = model.DossierId,
                    ProcConfigs = procConfigs
                });

                if (retUpdateProcConfig <= 0)
                {
                    return Json(new { status = true, message = CreateMessage(string.Format(AppProcessor.Messagor.GetMessage("Modal_Title_Complete"), $"{_dossierTaskTitle} [{model.InStepName}]"), EnumProcessType.NonFormat, EnumMsgIcon.Error) }, JsonRequestBehavior.AllowGet);
                }
            }

            #region Process Update

            model.HandledBy = nextStepView.HandledBy;
            model.UpdatedBy = User.UserName;

            var saveResult = _dossierCache.ChangeHandler(model);

            if (saveResult > 0)
            {
                if (!string.IsNullOrEmpty(nextStepView.StaffNotificationConfigs))
                {
                    var contractModel = _contractCache.GetById(dossierModel.DossierId);
                    var sender = User.UserName;
                    var urlSearchContract = Request.Url.GetLeftPart(UriPartial.Authority);
                    var urlSearchDossier = Url.Action("Index", "Dossier", new { q = contractModel.ContractNoInfo });

                    Task.Run(() =>
                    {
                        try
                        {
                            currentStepView.StaffNotificationConfigs.Split(';').ToList().ForEach(t =>
                            {
                                var libNotify = _listNotificationProviders.FirstOrDefault(n => n.Name == t);
                                {
                                    if (libNotify != null)
                                    {
                                        var handler = model.HandledBy;
                                        var emailHandler = _userCache.GetByUserName(handler);
                                        libNotify.Push(new ContentNotifyModel
                                        {
                                            TypeEmail = EnumTypeEmail.ContractPending,
                                            ContractInfo = new Contract
                                            {
                                                ContractNo = contractModel.ContractNo,
                                                ContractSignal = contractModel.ContractSignal,
                                                SearchContractDetailUrl = urlSearchContract,
                                                SearchContractUrl = urlSearchContract
                                            },
                                            CusInfo = new Customer
                                            {
                                                CusName = emailHandler.FullName,
                                                Email = emailHandler.Email,
                                                Phone = emailHandler.Phone
                                            },
                                            InsiteNotification = new InsiteNotificationModel
                                            {
                                                Icon = EnumMsgIcon.Info,
                                                Title = AppProcessor.Messagor.GetMessage("Dossier_Notify_Title_Handing"),
                                                Message = string.Format(AppProcessor.Messagor.GetMessage("Notify_Title_Handing_Contract"), contractModel.ContractNoInfo),
                                                Placement = "tr",
                                                Url = urlSearchDossier,
                                                Sender = sender,
                                                Receiver = handler
                                            }
                                        });

                                    }
                                }
                            });
                        }
                        catch (Exception ex)
                        {
                            AppProcessor.Logger.Error(ex);
                            if (ex.InnerException != null)
                                AppProcessor.Logger.Error(ex.InnerException);
                        }
                    });
                }
            }

            #endregion

            var response = CreateMessage(string.Format(AppProcessor.Messagor.GetMessage("Modal_Title_ChangeHandler"), $"{_dossierTaskTitle} [{model.InStepName}]"), EnumProcessType.NonFormat, saveResult == 0 ? EnumMsgIcon.Error : EnumMsgIcon.Success);
            return Json(new { status = true, message = response }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult ChangeNextStep(Guid? id)
        {
            var dossierTask = _dossierCache.GetTaskById(id);
            if (dossierTask == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_dossierTaskTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var dossierModel = _dossierCache.GetById(dossierTask.DossierId);

            #region Procedure Structure View

            var procedureView = JsonConvert.DeserializeObject<ViewProcedureStructureModel>(dossierModel.ProcConfigs);
            var currentStepView = procedureView.Steps.FirstOrDefault(s => s.StepId == dossierModel.InStep);
            if (currentStepView == null)
            {
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_stepTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            }

            #endregion

            dossierTask.DossierCode = dossierModel.DossierCode;
            dossierTask.NextStep = currentStepView.NextStep;
            dossierTask.NextStepName = currentStepView.NextStepName;

            dossierTask.ListSteps = procedureView.Steps
                //.Where(s => s.StepId != dossierModel.InStep && !new List<string>{ "Start","End" }.Exists(t => t == s.StepType))
                .Where(s => s.StepId != dossierModel.InStep && !new List<string>{ "Start" }.Exists(t => t == s.StepType))
                .OrderBy(s => s.Ordinal)
                .ToList();

            return PartialView("_ChangeNextStep", dossierTask);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Edit)]
        [ValidateInput(false)]
        public ActionResult ChangeNextStep(MajorDossierTaskModel model)
        {
            #region Valid

            var dossierModel = _dossierCache.GetById(model.DossierId);
            if (dossierModel == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_dossierTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            var dossierTaskModel = _dossierCache.GetTaskById(model.TaskId);
            if (dossierTaskModel == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_dossierTaskTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var procedureView = JsonConvert.DeserializeObject<ViewProcedureStructureModel>(dossierModel.ProcConfigs);
            var currentStepView = procedureView.Steps.FirstOrDefault(s => s.StepId == dossierModel.InStep);
            if (currentStepView == null)
            {
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_stepTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            }

            #region Check Next Step

            var nextStepView = procedureView.Steps.FirstOrDefault(s => s.StepId == currentStepView.NextStep);
            if (nextStepView == null)
            {
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_stepTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            }

            if (model.NextStep == null)
            {
                ModelState.AddModelError("NextStep", $"Dữ liệu [{AppProcessor.Messagor.GetMessage("Step_NextStep")}] bắt buộc nhập");
            }

            #endregion

            if (!ModelState.IsValid)
            {
                model.ListSteps = procedureView.Steps
                    .Where(s => s.StepId != dossierModel.InStep && !new List<string> { "Start", "End" }.Exists(t => t == s.StepType))
                    .ToList();
                return PartialView("_ChangeNextStepBody", model);
            }

            #endregion

            currentStepView.NextStep = model.NextStep;
            currentStepView.NextStepName = model.NextStepName;

            var procConfigs = JsonConvert.SerializeObject(procedureView);
            var saveResult = _dossierCache.UpdateProcConfig(new MajorDossierModel
            {
                DossierId = model.DossierId,
                ProcConfigs = procConfigs
            });

            var response = CreateMessage($"{AppProcessor.Messagor.GetMessage("Modal_Title_ChangeNextStep")} {_dossierTitle} [{model.DossierCode}]", EnumProcessType.NonFormat, saveResult == 0 ? EnumMsgIcon.Error : EnumMsgIcon.Success);
            return Json(new { status = true, message = response }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Extends Functions

        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        [AjaxOnly]
        public Task<ActionResult> StaffsViaUnion(Guid? unionId)
        {
            var lstStaffs = _unionCache.GetMembersViaUnion(unionId)
                .OrderBy(s => s.FullName).ToList();
            return Task.FromResult<ActionResult>(Json(lstStaffs));
        }

        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        [AjaxOnly]
        public Task<ActionResult> StepHandlerViaSituation(Guid? dossierId, Guid? situationId)
        {
            var dossierModel = _dossierCache.GetById(dossierId);
            if (dossierModel != null)
            {
                var procedureView = JsonConvert.DeserializeObject<ViewProcedureStructureModel>(dossierModel.ProcConfigs);
                var currentStepView = procedureView.Steps.FirstOrDefault(s => s.StepId == dossierModel.InStep);
                if (currentStepView != null)
                {
                    if (situationId != null)
                    {
                        var selectedSituationModel =
                            currentStepView.Situations.FirstOrDefault(si => si.SituationId == situationId);
                        if (selectedSituationModel != null)
                        {
                            var nextStepView =
                                procedureView.Steps.FirstOrDefault(s => s.StepId == selectedSituationModel.NextStep);
                            if (nextStepView != null)
                            {
                                if (nextStepView.Handlers?.Count > 0)
                                {
                                    var handler =
                                        nextStepView.Handlers.FirstOrDefault(h =>
                                            h.UnionId == nextStepView.UnionHandle);
                                    if (handler != null)
                                    {
                                        var lstStaffsInUnionHandleNextStep =
                                            _unionCache.GetMembersViaUnion(handler.DeptId);
                                        return Task.FromResult<ActionResult>(Json(
                                            new
                                            {
                                                Unions = lstStaffsInUnionHandleNextStep.GroupBy(s => s.UnionName)
                                                    .Select(g => new { UnionName = g.Key, Handlers = g.Select(h => h) })
                                                    .ToList(),
                                                Handler = handler.StaffId
                                            },
                                            JsonRequestBehavior.AllowGet));
                                    }
                                    else
                                    {
                                        var lstStaffsInUnionHandleNextStep =
                                            _unionCache.GetMembersViaUnion(nextStepView.DeptHandle, true);
                                        return Task.FromResult<ActionResult>(Json(
                                            new
                                            {
                                                Unions = lstStaffsInUnionHandleNextStep.GroupBy(s => s.UnionName)
                                                    .Select(g => new { UnionName = g.Key, Handlers = g.Select(h => h) })
                                                    .ToList(),
                                                Handler = nextStepView.HandledBy
                                            }, JsonRequestBehavior.AllowGet));
                                    }
                                }

                                {
                                    var lstStaffsInUnionHandleNextStep =
                                        _unionCache.GetMembersViaUnion(nextStepView.DeptHandle, true);

                                    return Task.FromResult<ActionResult>(Json(new
                                    {
                                        Unions = lstStaffsInUnionHandleNextStep.GroupBy(s => s.UnionName)
                                            .Select(g => new { UnionName = g.Key, Handlers = g.Select(h => h) })
                                            .ToList(),
                                        Handler = nextStepView.HandledBy
                                    }, JsonRequestBehavior.AllowGet));
                                }
                            }
                        }
                    }
                    else
                    {
                        var nextStepView =
                            procedureView.Steps.FirstOrDefault(s => s.StepId == currentStepView.NextStep);
                        if (nextStepView != null)
                        {
                            if (nextStepView.Handlers?.Count > 0)
                            {
                                var handler =
                                    nextStepView.Handlers.FirstOrDefault(h =>
                                        h.UnionId == nextStepView.UnionHandle);
                                if (handler != null)
                                {
                                    var lstStaffsInUnionHandleNextStep =
                                        _unionCache.GetMembersViaUnion(handler.DeptId);
                                    return Task.FromResult<ActionResult>(Json(
                                        new
                                        {
                                            Unions = lstStaffsInUnionHandleNextStep.GroupBy(s => s.UnionName)
                                                .Select(g => new { UnionName = g.Key, Handlers = g.Select(h => h) })
                                                .ToList(),
                                            Handler = handler.StaffId
                                        },
                                        JsonRequestBehavior.AllowGet));
                                }
                                else
                                {
                                    var lstStaffsInUnionHandleNextStep =
                                        _unionCache.GetMembersViaUnion(nextStepView.DeptHandle, true);
                                    return Task.FromResult<ActionResult>(Json(
                                        new
                                        {
                                            Unions = lstStaffsInUnionHandleNextStep.GroupBy(s => s.UnionName)
                                                .Select(g => new { UnionName = g.Key, Handlers = g.Select(h => h) })
                                                .ToList(),
                                            Handler = nextStepView.HandledBy
                                        }, JsonRequestBehavior.AllowGet));
                                }
                            }

                            {
                                var lstStaffsInUnionHandleNextStep =
                                    _unionCache.GetMembersViaUnion(nextStepView.DeptHandle, true);

                                return Task.FromResult<ActionResult>(Json(new
                                {
                                    Unions = lstStaffsInUnionHandleNextStep.GroupBy(s => s.UnionName)
                                        .Select(g => new { UnionName = g.Key, Handlers = g.Select(h => h) })
                                        .ToList(),
                                    Handler = nextStepView.HandledBy
                                }, JsonRequestBehavior.AllowGet));
                            }
                        }
                    }
                }
            }

            return Task.FromResult<ActionResult>(Json(new { Unions = string.Empty, Handler = string.Empty }, JsonRequestBehavior.AllowGet));
        }

        private readonly string _contractTitle = AppProcessor.Messagor.GetMessage("Contract_Title");

        [AjaxOnly]
        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        public ActionResult ViewContract(Guid? contractId)
        {
            var contractModel = _contractCache.GetById(contractId);
            if (contractModel == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_contractTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            contractModel.ContractTypeName =
                AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription((EnumContractType)contractModel.ContractTypeId));
            contractModel.CusInfo = _contractCache.GetCus(contractId);

            contractModel.ListTasks = _contractCache.GetTask(contractId);
            contractModel.HasTaxForContract = contractModel.TaxRate > 0;

            contractModel.TaxInfo = string.Format(_taxInfoContract, contractModel.TaxRate, contractModel.TaxAmount.ToString("#,### đ"));

            return PartialView("_ViewContract", contractModel);
        }

        #region Extend Function For RefFiles

        private bool SaveRefFiles(List<HttpPostedFileBase> refFiles, Guid dossierId, Guid taskId, string saveBy, out string errMsg)
        {
            errMsg = string.Empty;
            if (refFiles == null || refFiles.Count <= 0 || refFiles[0] == null)
            {
                errMsg = "Không có tệp đính kèm";
                return false;
            }

            var lstDocContracts = new List<CateDocModel>();

            var refContractsFolderPath = $"{_refDossierDocsFolderPath}/{_dossiersFolderName}/{dossierId}/{taskId}";
            var refContractsFolderAbsolutePath = Server.MapPath(refContractsFolderPath);

            if (!Directory.Exists(refContractsFolderAbsolutePath))
                Directory.CreateDirectory(refContractsFolderAbsolutePath);

            foreach (var refFile in refFiles)
            {
                Image image = null;
                if (ConstMIMEType.IsImage(refFile.ContentType))
                {
                    image = Image.FromStream(refFile.InputStream);
                }

                var cateDoc = new CateDocModel
                {
                    FileId = Guid.NewGuid(),
                    TypeObject = "Major_Dossiers_Tasks",
                    FilePath = refContractsFolderPath,
                    FileName = Path.GetFileNameWithoutExtension(refFile.FileName),
                    FileExt = Path.GetExtension(refFile.FileName),
                    ContentType = refFile.ContentType,
                    Dimensions = image != null ? $"{image.Width}x{image.Height}" : null
                };

                lstDocContracts.Add(cateDoc);
            }

            if (lstDocContracts.Count <= 0)
            {
                errMsg = "Không có tệp đính kèm";
                return false;
            }
            var tableRefFiles = CreateTableRefFiles(lstDocContracts);

            var retSaveFile = _dossierCache.SaveRefFiles(new MajorDossierTaskModel { TaskId = taskId, TableRefFiles = tableRefFiles, UpdatedBy = saveBy });

            if (retSaveFile > 0)
            {
                refFiles.ForEach(refFile =>
                {
                    var cateDoc = lstDocContracts.FirstOrDefault(c =>
                        c.FileName == Path.GetFileNameWithoutExtension(refFile.FileName) &&
                        c.FileExt == Path.GetExtension(refFile.FileName) && c.ContentType == refFile.ContentType);
                    if (cateDoc != null)
                    {
                        refFile.SaveAs(Path.Combine(refContractsFolderAbsolutePath, $"{cateDoc.FileId.ToString().ToUpper()}{cateDoc.FileExt}"));
                    }
                });
            }

            return retSaveFile > 0;
        }

        private DataTable CreateTableRefFiles(List<CateDocModel> lstDocs)
        {
            var dataRefImgs = new DataTable();
            using (var reader = ObjectReader.Create(lstDocs, "FileId", "TypeObject", "FilePath", "FileName", "FileExt", "ContentType", "Dimensions", "Version"))
            {
                dataRefImgs.Load(reader);
            }

            return dataRefImgs;
        }

        #endregion

        #endregion

    }
}