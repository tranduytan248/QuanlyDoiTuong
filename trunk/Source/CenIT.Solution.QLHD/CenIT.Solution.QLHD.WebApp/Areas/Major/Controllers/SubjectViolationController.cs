using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cores.Base.Apps;
using Cores.Cate.Caches;
using Cores.Major.Caches;
using Cores.Major.Models;
using Modules.Major.Areas.Major.Models;
using TSFramework.App.Attributes;
using TSFramework.App.Models;
using TSFramework.App.Processors;
using TSFramework.Core.Enums;

namespace Modules.Major.Areas.Major.Controllers
{
    public class SubjectViolationController : AppController
    {
        private readonly MajorSubjectViolationCache _violationCache = new MajorSubjectViolationCache();
        private readonly MajorSubjectCache _subjectCache = new MajorSubjectCache();
        private readonly CateFieldCache _fieldCache = new CateFieldCache();
        private readonly CateViolationBehaviorCache _behaviorCache = new CateViolationBehaviorCache();
        private readonly string _violationTitle = AppProcessor.Messagor.GetMessage("SubjectViolation_Title") ?? "Lịch sử vi phạm";

        // GET: Major/SubjectViolation
        public ActionResult Index()
        {
            var searchModel = new SearchSubjectViolationModel();
            ViewBag.ListFields = _fieldCache.GetAll();
            ViewBag.ListSubjects = _subjectCache.GetAll();
            return View(searchModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult Get(SearchSubjectViolationModel searchModel)
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
            var data = _violationCache.Get(out int total, searchModel?.Key, searchModel?.SubjectId, searchModel?.FieldId, dataSearch);
            var result = Json(
                new { draw = Convert.ToInt32(draw), recordsTotal = total, recordsFiltered = total, data },
                JsonRequestBehavior.AllowGet);
            return result;
        }

        [AjaxOnly]
        [ActionType(Type = EnumActionType.Add)]
        [HttpGet]
        public ActionResult Add(Guid? subjectId)
        {
            var model = new MajorSubjectViolationModel();
            if (subjectId.HasValue && subjectId.Value != Guid.Empty)
            {
                model.SubjectId = subjectId.Value;
                var subj = _subjectCache.GetById(subjectId.Value);
                if (subj != null)
                {
                    model.SubjectName = subj.FullName;
                    model.IdentityCardNumber = subj.IdentityCardNumber;
                }
            }
            ViewBag.ListSubjects = _subjectCache.GetAll();
            ViewBag.ListBehaviors = _behaviorCache.GetAll();
            return PartialView("_Add", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Add)]
        [ValidateInput(false)]
        public ActionResult Add(MajorSubjectViolationModel model)
        {
            try
            {
                ModelState.Remove("ViolationId");
                if (model.SubjectId == Guid.Empty)
                {
                    ModelState.AddModelError("SubjectId", "Vui lòng chọn đối tượng vi phạm.");
                }
                if (string.IsNullOrEmpty(model.BehaviorIds))
                {
                    ModelState.AddModelError("BehaviorIds", "Vui lòng chọn ít nhất một hành vi vi phạm.");
                }

                if (!ModelState.IsValid)
                {
                    ViewBag.ListSubjects = _subjectCache.GetAll();
                    ViewBag.ListBehaviors = _behaviorCache.GetAll();
                    return PartialView("_Violation", model);
                }

                var result = _violationCache.Save(model, User.UserName);
                bool isSuccess = !string.IsNullOrEmpty(result);

                string response = CreateMessage($"{_violationTitle}",
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
            var model = _violationCache.GetById(id);
            if (model == null)
            {
                return Json(new
                {
                    status = false,
                    message = CreateMessage(_violationTitle, EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                }, JsonRequestBehavior.AllowGet);
            }
            ViewBag.ListSubjects = _subjectCache.GetAll();
            ViewBag.ListBehaviors = _behaviorCache.GetAll();
            return PartialView("_Edit", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Edit)]
        [ValidateInput(false)]
        public ActionResult Edit(MajorSubjectViolationModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.BehaviorIds))
                {
                    ModelState.AddModelError("BehaviorIds", "Vui lòng chọn ít nhất một hành vi vi phạm.");
                }

                if (!ModelState.IsValid)
                {
                    ViewBag.ListSubjects = _subjectCache.GetAll();
                    ViewBag.ListBehaviors = _behaviorCache.GetAll();
                    return PartialView("_Violation", model);
                }

                var result = _violationCache.Save(model, User.UserName);
                bool isSuccess = !string.IsNullOrEmpty(result);

                string response = CreateMessage($"{_violationTitle}",
                    EnumProcessType.Edit, isSuccess ? EnumMsgIcon.Success : EnumMsgIcon.Error);
                return Json(new { status = isSuccess, message = response }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "alert('ERROR: " + ex.Message.Replace("'", "\\'") + "');" }, JsonRequestBehavior.AllowGet);
            }
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(Guid id)
        {
            var model = _violationCache.GetById(id);
            if (model == null)
            {
                return Json(new
                {
                    status = false,
                    message = CreateMessage(_violationTitle, EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                }, JsonRequestBehavior.AllowGet);
            }
            ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Common_ConfirmMessage"),
                $"<b>Lần vi phạm ngày [{model.ViolationDate:dd/MM/yyyy HH:mm}] của đối tượng [{model.SubjectName}]</b>");
            return PartialView("_Delete", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(MajorSubjectViolationModel model)
        {
            if (!model.ViolationId.HasValue) return Json(new { status = false });
            var isSuccess = _violationCache.Delete(model.ViolationId.Value, User.UserName);
            string response = CreateMessage($"{_violationTitle}", EnumProcessType.Delete,
                isSuccess ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = isSuccess, message = response });
        }

        [HttpPost]
        public ActionResult UploadViolationImages()
        {
            try
            {
                var uploadedUrls = new List<string>();
                if (Request.Files.Count > 0)
                {
                    var folder = Server.MapPath("~/Uploads/Violations/");
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        var file = Request.Files[i];
                        if (file != null && file.ContentLength > 0)
                        {
                            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                            var path = Path.Combine(folder, fileName);
                            file.SaveAs(path);
                            uploadedUrls.Add($"/Uploads/Violations/{fileName}");
                        }
                    }
                    return Json(new { status = true, urls = uploadedUrls });
                }
                return Json(new { status = false, message = "Không có tệp tải lên." });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
    }
}
