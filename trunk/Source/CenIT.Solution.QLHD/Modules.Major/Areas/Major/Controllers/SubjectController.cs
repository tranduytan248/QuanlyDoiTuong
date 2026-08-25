using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cores.Base.Apps;
using Cores.Cate.Caches;
using Cores.Major.Caches;
using Cores.Major.Models;
using Cores.Sys.Caches.Sys;
using Modules.Major.Areas.Major.Models;
using TSFramework.App.Attributes;
using TSFramework.App.Models;
using TSFramework.App.Processors;
using TSFramework.Core.Enums;

namespace Modules.Major.Areas.Major.Controllers
{
    public class SubjectController : AppController
    {
        private readonly MajorSubjectCache _subjectCache = new MajorSubjectCache();
        private readonly MajorSubjectViolationCache _violationCache = new MajorSubjectViolationCache();
        private readonly CateFieldCache _fieldCache = new CateFieldCache();
        private readonly CateViolationBehaviorCache _behaviorCache = new CateViolationBehaviorCache();
        private readonly CateUnionCache _unionCache = new CateUnionCache();
        private readonly SysUserCache _userCache = new SysUserCache();
        private readonly string _subjectTitle = AppProcessor.Messagor.GetMessage("Subject_Title") ?? "Đối tượng";

        // GET: Major/Subject
        public ActionResult Index()
        {
            var searchModel = new SearchSubjectModel();
            return View(searchModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult Get(SearchSubjectModel searchModel)
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
            var data = _subjectCache.Get(out int total, searchModel?.Key, searchModel?.Gender, dataSearch);
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
            _fieldCache.InvalidateAll();
            _behaviorCache.InvalidateAll();
            ViewBag.ListFields = _fieldCache.GetAll();
            ViewBag.ListBehaviors = _behaviorCache.GetAll();
            
            var userName = User?.UserName;
            var currentUser = !string.IsNullOrEmpty(userName) ? _userCache.GetByUserName(userName) : null;
            var userDept = !string.IsNullOrEmpty(userName) ? _unionCache.GetDeptByMember(userName) : null;
            var userUnion = !string.IsNullOrEmpty(userName) ? _unionCache.GetUnionByMember(userName) : null;
            var memberInfo = !string.IsNullOrEmpty(userName) ? _unionCache.GetMemberByKey(userName) : null;

            string fullUnit = "";
            if (userDept != null && userUnion != null && userDept.UnionName != userUnion.UnionName)
            {
                fullUnit = $"{userDept.UnionName} - {userUnion.UnionName}";
            }
            else
            {
                fullUnit = userDept?.UnionName ?? userUnion?.UnionName ?? currentUser?.OfficeName ?? "Văn phòng Đăng ký Đất đai Khánh Hòa";
            }

            string positionName = memberInfo?.PositionName ?? "Cán bộ tiếp nhận";

            var model = new MajorSubjectModel
            {
                InitialViolationDate = DateTime.Now,
                ReporterName = currentUser?.FullName ?? User?.UserName ?? string.Empty,
                ReporterUnit = fullUnit,
                ReporterPosition = positionName,
                ReporterPhone = currentUser?.Phone ?? string.Empty
            };
            return PartialView("_Add", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Add)]
        [ValidateInput(false)]
        public ActionResult Add(MajorSubjectModel model)
        {
            try
            {
                ModelState.Remove("SubjectId");
                if (!ModelState.IsValid)
                {
                    ViewBag.ListFields = _fieldCache.GetAll();
                    ViewBag.ListBehaviors = _behaviorCache.GetAll();
                    return PartialView("_Subject", model);
                }

                var result = _subjectCache.Save(model, User.UserName);

                if (result == "EXISTED")
                {
                    string responseExisted = CreateMessage($"{_subjectTitle} với số CCCD [{model.IdentityCardNumber}]",
                        EnumProcessType.DataExisted, EnumMsgIcon.Error);
                    return Json(new { status = false, message = responseExisted }, JsonRequestBehavior.AllowGet);
                }

                bool isSuccess = !string.IsNullOrEmpty(result);
                if (isSuccess && Guid.TryParse(result, out Guid newSubjectId))
                {
                    if (!string.IsNullOrEmpty(model.InitialBehaviorIds) && model.InitialViolationDate.HasValue)
                    {
                        var violationModel = new MajorSubjectViolationModel
                        {
                            SubjectId = newSubjectId,
                            ViolationDate = model.InitialViolationDate.Value,
                            BehaviorIds = model.InitialBehaviorIds,
                            TreatmentMeasures = model.InitialTreatmentMeasures,
                            RelatedDocuments = model.InitialRelatedDocuments,
                            Notes = model.InitialNotes,
                            Images = model.InitialImages
                        };
                        _violationCache.Save(violationModel, User.UserName);
                    }
                }

                string response = CreateMessage($"{_subjectTitle} [{model.FullName}]",
                    EnumProcessType.Add, isSuccess ? EnumMsgIcon.Success : EnumMsgIcon.Error);
                return Json(new { status = isSuccess, message = response }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "alert('ERROR: " + ex.Message.Replace("'", "\\'") + "');" }, JsonRequestBehavior.AllowGet);
            }
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult Edit(Guid id)
        {
            var model = _subjectCache.GetById(id);
            if (model == null)
            {
                return Json(new
                {
                    status = false,
                    message = CreateMessage(_subjectTitle, EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                }, JsonRequestBehavior.AllowGet);
            }
            model.ListViolations = _violationCache.GetBySubjectId(id) ?? new System.Collections.Generic.List<MajorSubjectViolationModel>();
            model.InitialViolationDate = DateTime.Now;
            _fieldCache.InvalidateAll();
            _behaviorCache.InvalidateAll();
            ViewBag.ListFields = _fieldCache.GetAll();
            ViewBag.ListBehaviors = _behaviorCache.GetAll();
            return PartialView("_Edit", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Edit)]
        [ValidateInput(false)]
        public ActionResult Edit(MajorSubjectModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    if (model.SubjectId.HasValue && model.SubjectId.Value != Guid.Empty)
                    {
                        model.ListViolations = _violationCache.GetBySubjectId(model.SubjectId.Value) ?? new System.Collections.Generic.List<MajorSubjectViolationModel>();
                    }
                    ViewBag.ListFields = _fieldCache.GetAll();
                    ViewBag.ListBehaviors = _behaviorCache.GetAll();
                    return PartialView("_Subject", model);
                }

                var result = _subjectCache.Save(model, User.UserName);

                if (result == "EXISTED")
                {
                    string responseExisted = CreateMessage($"{_subjectTitle} với số CCCD [{model.IdentityCardNumber}]",
                        EnumProcessType.DataExisted, EnumMsgIcon.Error);
                    return Json(new { status = false, message = responseExisted }, JsonRequestBehavior.AllowGet);
                }

                if (model.SubjectId.HasValue && model.SubjectId.Value != Guid.Empty && !string.IsNullOrEmpty(model.InitialBehaviorIds) && model.InitialViolationDate.HasValue)
                {
                    var violationModel = new MajorSubjectViolationModel
                    {
                        SubjectId = model.SubjectId.Value,
                        ViolationDate = model.InitialViolationDate.Value,
                        BehaviorIds = model.InitialBehaviorIds,
                        TreatmentMeasures = model.InitialTreatmentMeasures,
                        RelatedDocuments = model.InitialRelatedDocuments,
                        Notes = model.InitialNotes,
                        Images = model.InitialImages
                    };
                    _violationCache.Save(violationModel, User.UserName);
                }

                bool isSuccess = !string.IsNullOrEmpty(result);
                string response = CreateMessage($"{_subjectTitle} [{model.FullName}]",
                    EnumProcessType.Edit, isSuccess ? EnumMsgIcon.Success : EnumMsgIcon.Error);
                return Json(new { status = isSuccess, message = response }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "alert('ERROR: " + ex.Message.Replace("'", "\\'") + "');" }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Major/Subject/Detail/Guid
        [ActionType(Type = EnumActionType.View)]
        public ActionResult Detail(Guid id)
        {
            var model = _subjectCache.GetById(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.ListViolations = _violationCache.GetBySubjectId(id);
            return View("Detail", model);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(Guid id)
        {
            var model = _subjectCache.GetById(id);
            if (model == null)
            {
                return Json(new
                {
                    status = false,
                    message = CreateMessage(_subjectTitle, EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                }, JsonRequestBehavior.AllowGet);
            }
            ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Common_ConfirmMessage"),
                $"<b>{_subjectTitle} [{model.FullName} - CCCD: {model.IdentityCardNumber}]</b>");
            return PartialView("_Delete", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(MajorSubjectModel model)
        {
            if (!model.SubjectId.HasValue) return Json(new { status = false });
            var isSuccess = _subjectCache.Delete(model.SubjectId.Value, User.UserName);
            string response = CreateMessage($"{_subjectTitle} [{model.FullName}]", EnumProcessType.Delete,
                isSuccess ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = isSuccess, message = response });
        }

        [HttpPost]
        public ActionResult UploadFile()
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];
                    if (file != null && file.ContentLength > 0)
                    {
                        var folder = Server.MapPath("~/Uploads/Subjects/");
                        if (!Directory.Exists(folder))
                        {
                            Directory.CreateDirectory(folder);
                        }
                        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                        var path = Path.Combine(folder, fileName);
                        file.SaveAs(path);
                        var fileUrl = $"/Uploads/Subjects/{fileName}";
                        return Json(new { status = true, url = fileUrl, fileName = file.FileName, fileSize = file.ContentLength });
                    }
                }
                return Json(new { status = false, message = "Không có tệp tải lên." });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            var list = _subjectCache.GetAll();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}
