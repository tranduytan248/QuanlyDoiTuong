using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cores.Base.Apps;
using Cores.Cate.Caches;
using Cores.Major.Caches;
using Cores.Major.Helpers;
using Cores.Major.Models;
using Cores.Sys.Caches.Sys;
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
        private readonly CateUnionCache _unionCache = new CateUnionCache();
        private readonly SysUserCache _userCache = new SysUserCache();
        private readonly MajorSubjectChangeLogCache _changeLogCache = new MajorSubjectChangeLogCache();
        private readonly string _violationTitle = AppProcessor.Messagor.GetMessage("SubjectViolation_Title") ?? "Lịch sử vi phạm";

        /// <summary>Thư mục lưu hình ảnh, văn bản đính kèm của lần vi phạm.</summary>
        private const string VIOLATION_UPLOAD_FOLDER = "~/Contents/File/Violations/";

        /// <summary>Đường dẫn truy cập tệp từ trình duyệt, phải khớp với VIOLATION_UPLOAD_FOLDER.</summary>
        private const string VIOLATION_UPLOAD_URL = "/Contents/File/Violations/";

        private const string DEFAULT_REPORTER_UNIT = "Văn phòng Đăng ký Đất đai Khánh Hòa";
        private const string DEFAULT_REPORTER_POSITION = "Cán bộ tiếp nhận";

        /// <summary>
        /// Kiểm tra người đang đăng nhập có phải người đã khai báo lần vi phạm này không.
        /// Cờ IsOwner trả về từ CSDL chỉ dùng để hiển thị nút; quyền thật phải được
        /// kiểm tra lại tại đây trước khi cho phép Sửa / Xoá.
        /// </summary>
        private bool IsViolationOwner(MajorSubjectViolationModel violation)
        {
            if (violation == null) return false;
            if (string.IsNullOrEmpty(violation.CreatedBy)) return false;

            return string.Equals(violation.CreatedBy.Trim(), User?.UserName?.Trim(),
                StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Ghi log thay đổi của một lần vi phạm. Lỗi ghi log không làm hỏng nghiệp vụ chính.
        /// </summary>
        private void WriteViolationChangeLog(Guid subjectId, Guid? violationId, string actionType,
            List<SubjectFieldChangeModel> changes, string description)
        {
            try
            {
                var userName = User?.UserName;
                var currentUser = !string.IsNullOrEmpty(userName) ? _userCache.GetByUserName(userName) : null;
                var userDept = !string.IsNullOrEmpty(userName) ? _unionCache.GetDeptByMember(userName) : null;
                var userUnion = !string.IsNullOrEmpty(userName) ? _unionCache.GetUnionByMember(userName) : null;
                var memberInfo = !string.IsNullOrEmpty(userName) ? _unionCache.GetMemberByKey(userName) : null;

                string changedFieldsJson = null;
                string changedFieldNames = null;
                if (changes != null && changes.Count > 0)
                {
                    changedFieldsJson = Newtonsoft.Json.JsonConvert.SerializeObject(changes);
                    changedFieldNames = string.Join(", ", changes.Select(item => item.Label));
                }

                _changeLogCache.Save(new MajorSubjectChangeLogModel
                {
                    SubjectId = subjectId,
                    ViolationId = violationId,
                    EntityType = ConstsChangeLogEntity.Violation,
                    ActionType = actionType,
                    ChangedFields = changedFieldsJson,
                    ChangedFieldNames = changedFieldNames,
                    Description = description,
                    ActorUserName = userName,
                    ActorName = currentUser?.FullName ?? userName,
                    ActorPosition = memberInfo?.PositionName ?? DEFAULT_REPORTER_POSITION,
                    ActorUnit = userDept?.UnionName ?? userUnion?.UnionName ?? DEFAULT_REPORTER_UNIT,
                    ActorUnionId = userDept?.UnionId ?? userUnion?.UnionId
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError($"Ghi log cập nhật vi phạm thất bại: {ex.Message}");
            }
        }

        /// <summary>
        /// Gán thông tin người khai báo và đơn vị khai báo của người đang đăng nhập vào bản ghi vi phạm.
        /// Thông tin này không hiển thị trên giao diện, chỉ lưu xuống CSDL và dùng để phân quyền dữ liệu.
        /// </summary>
        private void ApplyReporterInfo(MajorSubjectViolationModel model)
        {
            if (model == null) return;

            var userName = User?.UserName;
            var currentUser = !string.IsNullOrEmpty(userName) ? _userCache.GetByUserName(userName) : null;
            var userDept = !string.IsNullOrEmpty(userName) ? _unionCache.GetDeptByMember(userName) : null;
            var userUnion = !string.IsNullOrEmpty(userName) ? _unionCache.GetUnionByMember(userName) : null;
            var memberInfo = !string.IsNullOrEmpty(userName) ? _unionCache.GetMemberByKey(userName) : null;

            string fullUnit;
            if (userDept != null && userUnion != null && userDept.UnionName != userUnion.UnionName)
            {
                fullUnit = $"{userDept.UnionName} - {userUnion.UnionName}";
            }
            else
            {
                fullUnit = userDept?.UnionName ?? userUnion?.UnionName ?? currentUser?.OfficeName ?? DEFAULT_REPORTER_UNIT;
            }

            model.ReporterName = currentUser?.FullName ?? userName ?? string.Empty;
            model.ReporterUnit = fullUnit;
            model.ReporterPosition = memberInfo?.PositionName ?? DEFAULT_REPORTER_POSITION;
            model.ReporterPhone = currentUser?.Phone ?? string.Empty;
            model.ReporterUnionId = userDept?.UnionId ?? userUnion?.UnionId;
        }

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
            var data = _violationCache.Get(out int total, searchModel?.Key, searchModel?.SubjectId,
                searchModel?.FieldId, User?.UserName, dataSearch);
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

                ApplyReporterInfo(model);

                var result = _violationCache.Save(model, User.UserName);
                bool isSuccess = !string.IsNullOrEmpty(result);

                if (isSuccess)
                {
                    Guid? newViolationId = Guid.TryParse(result, out Guid parsedId) ? parsedId : (Guid?)null;
                    WriteViolationChangeLog(model.SubjectId, newViolationId,
                        ConstsChangeLogAction.Add, null, "Ghi nhận lần vi phạm mới");
                }

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

                // Chỉ người đã khai báo mới được sửa
                var currentViolation = model.ViolationId.HasValue
                    ? _violationCache.GetById(model.ViolationId.Value) : null;

                if (currentViolation == null)
                {
                    return Json(new
                    {
                        status = false,
                        message = CreateMessage(_violationTitle, EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                    }, JsonRequestBehavior.AllowGet);
                }

                if (!IsViolationOwner(currentViolation))
                {
                    return Json(new
                    {
                        status = false,
                        message = "alert('Bạn không có quyền chỉnh sửa lần vi phạm do người khác khai báo.');"
                    }, JsonRequestBehavior.AllowGet);
                }

                var changes = SubjectChangeDetector.DiffViolation(currentViolation, model);
                if (changes.Count == 0)
                {
                    return Json(new
                    {
                        status = true,
                        message = CreateMessage("Không có thông tin nào thay đổi", EnumProcessType.Edit, EnumMsgIcon.Info)
                    }, JsonRequestBehavior.AllowGet);
                }

                ApplyReporterInfo(model);

                var result = _violationCache.Save(model, User.UserName);
                bool isSuccess = !string.IsNullOrEmpty(result);

                if (isSuccess)
                {
                    WriteViolationChangeLog(model.SubjectId, model.ViolationId,
                        ConstsChangeLogAction.Update, changes, null);
                }

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

            // Chỉ người đã khai báo mới được xoá
            var currentViolation = _violationCache.GetById(model.ViolationId.Value);
            if (currentViolation == null)
            {
                return Json(new
                {
                    status = false,
                    message = CreateMessage(_violationTitle, EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            }

            if (!IsViolationOwner(currentViolation))
            {
                return Json(new
                {
                    status = false,
                    message = "alert('Bạn không có quyền xoá lần vi phạm do người khác khai báo.');"
                });
            }

            var isSuccess = _violationCache.Delete(model.ViolationId.Value, User.UserName);

            if (isSuccess)
            {
                WriteViolationChangeLog(currentViolation.SubjectId, model.ViolationId,
                    ConstsChangeLogAction.Delete, null,
                    $"Xoá lần vi phạm ngày [{currentViolation.ViolationDateStr}]");
            }

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
                    var folder = Server.MapPath(VIOLATION_UPLOAD_FOLDER);
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
                            uploadedUrls.Add($"{VIOLATION_UPLOAD_URL}{fileName}");
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
