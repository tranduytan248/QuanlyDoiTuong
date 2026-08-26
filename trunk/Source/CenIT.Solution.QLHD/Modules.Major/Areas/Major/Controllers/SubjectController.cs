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
    public class SubjectController : AppController
    {
        private readonly MajorSubjectCache _subjectCache = new MajorSubjectCache();
        private readonly MajorSubjectViolationCache _violationCache = new MajorSubjectViolationCache();
        private readonly CateFieldCache _fieldCache = new CateFieldCache();
        private readonly CateViolationBehaviorCache _behaviorCache = new CateViolationBehaviorCache();
        private readonly CateUnionCache _unionCache = new CateUnionCache();
        private readonly SysUserCache _userCache = new SysUserCache();
        private readonly MajorSubjectChangeLogCache _changeLogCache = new MajorSubjectChangeLogCache();
        private readonly string _subjectTitle = AppProcessor.Messagor.GetMessage("Subject_Title") ?? "Đối tượng";

        /// <summary>Thư mục lưu ảnh định danh của đối tượng (ảnh chân dung, CCCD).</summary>
        private const string SUBJECT_UPLOAD_FOLDER = "~/Contents/File/Subjects/";

        /// <summary>Đường dẫn truy cập ảnh từ trình duyệt, phải khớp với SUBJECT_UPLOAD_FOLDER.</summary>
        private const string SUBJECT_UPLOAD_URL = "/Contents/File/Subjects/";

        private const string DEFAULT_REPORTER_UNIT = "Văn phòng Đăng ký Đất đai Khánh Hòa";
        private const string DEFAULT_REPORTER_POSITION = "Cán bộ tiếp nhận";

        // GET: Major/Subject
        public ActionResult Index()
        {
            // Danh mục phục vụ bộ lọc tra cứu theo lĩnh vực / hành vi vi phạm
            ViewBag.ListFields = _fieldCache.GetAll();
            ViewBag.ListBehaviors = _behaviorCache.GetAll();

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
            var data = _subjectCache.Get(out int total,
                searchModel?.IdentityCardNumber,
                searchModel?.FullName,
                searchModel?.BehaviorIds,
                searchModel?.Gender,
                User?.UserName,
                dataSearch);
            var result = Json(
                new { draw = Convert.ToInt32(draw), recordsTotal = total, recordsFiltered = total, data },
                JsonRequestBehavior.AllowGet);
            return result;
        }

        /// <summary>
        /// Xác định thông tin người khai báo và đơn vị khai báo của người đang đăng nhập.
        /// Dùng chung cho cả việc lưu Đối tượng và lưu Lịch sử vi phạm.
        /// </summary>
        private ReporterInfo ResolveReporterInfo()
        {
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

            return new ReporterInfo
            {
                Name = currentUser?.FullName ?? userName ?? string.Empty,
                Unit = fullUnit,
                Position = memberInfo?.PositionName ?? DEFAULT_REPORTER_POSITION,
                Phone = currentUser?.Phone ?? string.Empty,
                // Ưu tiên đơn vị trực tiếp (tổ) làm khoá phân quyền dữ liệu,
                // vì đây là cấp nhỏ nhất mà người dùng thuộc về.
                UnionId = userDept?.UnionId ?? userUnion?.UnionId
            };
        }

        /// <summary>
        /// Ghi một dòng log cập nhật. Lỗi khi ghi log KHÔNG được làm hỏng thao tác
        /// chính, nên toàn bộ được bọc trong try/catch.
        /// </summary>
        private void WriteChangeLog(Guid subjectId, Guid? violationId, string entityType,
            string actionType, List<SubjectFieldChangeModel> changes, string description)
        {
            try
            {
                var actor = ResolveReporterInfo();

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
                    EntityType = entityType,
                    ActionType = actionType,
                    ChangedFields = changedFieldsJson,
                    ChangedFieldNames = changedFieldNames,
                    Description = description,
                    ActorUserName = User?.UserName,
                    ActorName = actor.Name,
                    ActorPosition = actor.Position,
                    ActorUnit = actor.Unit,
                    ActorUnionId = actor.UnionId
                });
            }
            catch (Exception ex)
            {
                // Không để lỗi ghi log ảnh hưởng tới nghiệp vụ chính
                System.Diagnostics.Trace.TraceError($"Ghi log cập nhật đối tượng thất bại: {ex.Message}");
            }
        }

        /// <summary>Thông tin người khai báo, dùng nội bộ trong controller.</summary>
        private class ReporterInfo
        {
            public string Name { get; set; }
            public string Unit { get; set; }
            public string Position { get; set; }
            public string Phone { get; set; }
            public Guid? UnionId { get; set; }
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
            
            var reporter = ResolveReporterInfo();

            var model = new MajorSubjectModel
            {
                InitialViolationDate = DateTime.Now,
                ReporterName = reporter.Name,
                ReporterUnit = reporter.Unit,
                ReporterPosition = reporter.Position,
                ReporterPhone = reporter.Phone,
                ReporterUnionId = reporter.UnionId
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

                // Khi người dùng tra cứu CCCD và nạp lại đối tượng đã có trong CSDL,
                // SubjectId khác Guid.Empty => bản ghi được cập nhật thay vì thêm mới.
                bool isUpdateExisting = model.SubjectId.HasValue && model.SubjectId.Value != Guid.Empty;

                // Luôn xác định lại thông tin khai báo từ tài khoản đang đăng nhập,
                // không tin vào giá trị người dùng gửi lên từ form.
                var reporter = ResolveReporterInfo();
                model.ReporterName = reporter.Name;
                model.ReporterUnit = reporter.Unit;
                model.ReporterPosition = reporter.Position;
                model.ReporterPhone = reporter.Phone;
                model.ReporterUnionId = reporter.UnionId;

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
                            Images = model.InitialImages,
                            // Thông tin khai báo không hiển thị trên giao diện, chỉ lưu xuống CSDL
                            ReporterName = reporter.Name,
                            ReporterUnit = reporter.Unit,
                            ReporterPosition = reporter.Position,
                            ReporterPhone = reporter.Phone,
                            ReporterUnionId = reporter.UnionId
                        };
                        _violationCache.Save(violationModel, User.UserName);
                    }
                }

                string response = CreateMessage($"{_subjectTitle} [{model.FullName}]",
                    isUpdateExisting ? EnumProcessType.Edit : EnumProcessType.Add,
                    isSuccess ? EnumMsgIcon.Success : EnumMsgIcon.Error);
                return Json(new { status = isSuccess, message = response }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "alert('ERROR: " + ex.Message.Replace("'", "\\'") + "');" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// BƯỚC 1 của màn hình thêm mới: lưu thông tin định danh đối tượng.
        /// - Nếu là đối tượng đã có và KHÔNG có gì thay đổi thì không ghi CSDL,
        ///   chỉ trả về thành công để giao diện mở khoá phần nhập vi phạm.
        /// - Nếu thêm mới thì vẫn kiểm tra trùng số CCCD.
        /// </summary>
        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Add)]
        [ValidateInput(false)]
        public ActionResult SaveSubject(MajorSubjectModel model)
        {
            try
            {
                ModelState.Remove("SubjectId");

                if (string.IsNullOrWhiteSpace(model?.IdentityCardNumber) || string.IsNullOrWhiteSpace(model.FullName))
                {
                    return Json(new
                    {
                        status = false,
                        message = "alert('Vui lòng nhập đầy đủ Số CCCD và Họ tên.');"
                    }, JsonRequestBehavior.AllowGet);
                }

                // Luôn xác định lại thông tin khai báo từ tài khoản đăng nhập
                var reporter = ResolveReporterInfo();
                model.ReporterName = reporter.Name;
                model.ReporterUnit = reporter.Unit;
                model.ReporterPosition = reporter.Position;
                model.ReporterPhone = reporter.Phone;
                model.ReporterUnionId = reporter.UnionId;

                bool isUpdateExisting = model.SubjectId.HasValue && model.SubjectId.Value != Guid.Empty;

                if (isUpdateExisting)
                {
                    var current = _subjectCache.GetByIdFresh(model.SubjectId.Value);
                    var changes = SubjectChangeDetector.Diff(current, model);

                    if (changes.Count == 0)
                    {
                        // Không có gì thay đổi -> KHÔNG ghi xuống CSDL
                        return Json(new
                        {
                            status = true,
                            unchanged = true,
                            isUpdate = true,
                            subjectId = model.SubjectId.Value.ToString(),
                            message = string.Empty
                        }, JsonRequestBehavior.AllowGet);
                    }

                    var updateResult = _subjectCache.Save(model, User.UserName);
                    if (updateResult == "EXISTED")
                    {
                        return Json(new
                        {
                            status = false,
                            message = CreateMessage($"{_subjectTitle} với số CCCD [{model.IdentityCardNumber}]",
                                EnumProcessType.DataExisted, EnumMsgIcon.Error)
                        }, JsonRequestBehavior.AllowGet);
                    }

                    WriteChangeLog(model.SubjectId.Value, null, ConstsChangeLogEntity.Subject,
                        ConstsChangeLogAction.Update, changes, null);

                    return Json(new
                    {
                        status = true,
                        unchanged = false,
                        isUpdate = true,
                        subjectId = model.SubjectId.Value.ToString(),
                        message = CreateMessage($"{_subjectTitle} [{model.FullName}]",
                            EnumProcessType.Edit, EnumMsgIcon.Success)
                    }, JsonRequestBehavior.AllowGet);
                }

                // Thêm mới
                var result = _subjectCache.Save(model, User.UserName);
                if (result == "EXISTED")
                {
                    return Json(new
                    {
                        status = false,
                        message = CreateMessage($"{_subjectTitle} với số CCCD [{model.IdentityCardNumber}]",
                            EnumProcessType.DataExisted, EnumMsgIcon.Error)
                    }, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(result) || !Guid.TryParse(result, out Guid newSubjectId))
                {
                    return Json(new
                    {
                        status = false,
                        message = CreateMessage(_subjectTitle, EnumProcessType.Add, EnumMsgIcon.Error)
                    }, JsonRequestBehavior.AllowGet);
                }

                WriteChangeLog(newSubjectId, null, ConstsChangeLogEntity.Subject,
                    ConstsChangeLogAction.Add, null, $"Khai báo đối tượng mới [{model.FullName}]");

                return Json(new
                {
                    status = true,
                    unchanged = false,
                    isUpdate = false,
                    subjectId = newSubjectId.ToString(),
                    message = CreateMessage($"{_subjectTitle} [{model.FullName}]",
                        EnumProcessType.Add, EnumMsgIcon.Success)
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "alert('ERROR: " + ex.Message.Replace("'", "\\'") + "');" },
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// BƯỚC 2 của màn hình thêm mới: ghi nhận một lần vi phạm cho đối tượng
        /// đã lưu ở bước 1. Luôn là THÊM MỚI một lần vi phạm, không sửa lần cũ.
        /// </summary>
        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Add)]
        [ValidateInput(false)]
        public ActionResult SaveViolation(MajorSubjectModel model)
        {
            try
            {
                if (model?.SubjectId == null || model.SubjectId.Value == Guid.Empty)
                {
                    return Json(new
                    {
                        status = false,
                        message = "alert('Vui lòng lưu thông tin đối tượng trước khi ghi nhận vi phạm.');"
                    }, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrWhiteSpace(model.InitialBehaviorIds) || !model.InitialViolationDate.HasValue)
                {
                    return Json(new
                    {
                        status = false,
                        message = "alert('Vui lòng chọn ít nhất một hành vi vi phạm và nhập thời gian vi phạm.');"
                    }, JsonRequestBehavior.AllowGet);
                }

                var reporter = ResolveReporterInfo();
                var violationModel = new MajorSubjectViolationModel
                {
                    SubjectId = model.SubjectId.Value,
                    ViolationDate = model.InitialViolationDate.Value,
                    BehaviorIds = model.InitialBehaviorIds,
                    TreatmentMeasures = model.InitialTreatmentMeasures,
                    RelatedDocuments = model.InitialRelatedDocuments,
                    Notes = model.InitialNotes,
                    Images = model.InitialImages,
                    ReporterName = reporter.Name,
                    ReporterUnit = reporter.Unit,
                    ReporterPosition = reporter.Position,
                    ReporterPhone = reporter.Phone,
                    ReporterUnionId = reporter.UnionId
                };

                var result = _violationCache.Save(violationModel, User.UserName);
                bool isSuccess = !string.IsNullOrEmpty(result);

                if (isSuccess)
                {
                    Guid? newViolationId = Guid.TryParse(result, out Guid parsedId) ? parsedId : (Guid?)null;
                    WriteChangeLog(model.SubjectId.Value, newViolationId, ConstsChangeLogEntity.Violation,
                        ConstsChangeLogAction.Add, null, "Ghi nhận lần vi phạm mới");
                }

                return Json(new
                {
                    status = isSuccess,
                    message = CreateMessage("Thông tin vi phạm",
                        EnumProcessType.Add, isSuccess ? EnumMsgIcon.Success : EnumMsgIcon.Error)
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "alert('ERROR: " + ex.Message.Replace("'", "\\'") + "');" },
                    JsonRequestBehavior.AllowGet);
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
            model.ListViolations = _violationCache.GetBySubjectId(id, User?.UserName) ?? new System.Collections.Generic.List<MajorSubjectViolationModel>();
            model.InitialViolationDate = DateTime.Now;
            _fieldCache.InvalidateAll();
            _behaviorCache.InvalidateAll();
            ViewBag.ListFields = _fieldCache.GetAll();
            ViewBag.ListBehaviors = _behaviorCache.GetAll();

            // Man hinh Cap nhat chi cho sua thong tin dinh danh. Viec ghi nhan vi pham
            // da co man hinh Lich su vi pham rieng.
            ViewBag.ShowViolationPanel = false;

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
                        model.ListViolations = _violationCache.GetBySubjectId(model.SubjectId.Value, User?.UserName) ?? new System.Collections.Generic.List<MajorSubjectViolationModel>();
                    }
                    ViewBag.ListFields = _fieldCache.GetAll();
                    ViewBag.ListBehaviors = _behaviorCache.GetAll();
                    ViewBag.ShowViolationPanel = false;
                    return PartialView("_Subject", model);
                }

                // So sánh với dữ liệu hiện tại: nếu không có gì thay đổi thì KHÔNG ghi CSDL
                var current = _subjectCache.GetByIdFresh(model.SubjectId.Value);
                var changes = SubjectChangeDetector.Diff(current, model);
                bool hasViolationPayload = !string.IsNullOrEmpty(model.InitialBehaviorIds)
                                           && model.InitialViolationDate.HasValue;

                if (changes.Count == 0 && !hasViolationPayload)
                {
                    return Json(new
                    {
                        status = true,
                        message = CreateMessage("Không có thông tin nào thay đổi", EnumProcessType.Edit, EnumMsgIcon.Info)
                    }, JsonRequestBehavior.AllowGet);
                }

                string result = null;
                if (changes.Count > 0)
                {
                    result = _subjectCache.Save(model, User.UserName);

                    if (result == "EXISTED")
                    {
                        string responseExisted = CreateMessage($"{_subjectTitle} với số CCCD [{model.IdentityCardNumber}]",
                            EnumProcessType.DataExisted, EnumMsgIcon.Error);
                        return Json(new { status = false, message = responseExisted }, JsonRequestBehavior.AllowGet);
                    }

                    WriteChangeLog(model.SubjectId.Value, null, ConstsChangeLogEntity.Subject,
                        ConstsChangeLogAction.Update, changes, null);
                }

                if (hasViolationPayload)
                {
                    var reporter = ResolveReporterInfo();
                    var violationModel = new MajorSubjectViolationModel
                    {
                        SubjectId = model.SubjectId.Value,
                        ViolationDate = model.InitialViolationDate.Value,
                        BehaviorIds = model.InitialBehaviorIds,
                        TreatmentMeasures = model.InitialTreatmentMeasures,
                        RelatedDocuments = model.InitialRelatedDocuments,
                        Notes = model.InitialNotes,
                        Images = model.InitialImages,
                        // Thông tin khai báo không hiển thị trên giao diện, chỉ lưu xuống CSDL
                        ReporterName = reporter.Name,
                        ReporterUnit = reporter.Unit,
                        ReporterPosition = reporter.Position,
                        ReporterPhone = reporter.Phone,
                        ReporterUnionId = reporter.UnionId
                    };
                    var violationResult = _violationCache.Save(violationModel, User.UserName);
                    if (!string.IsNullOrEmpty(violationResult))
                    {
                        Guid? newViolationId = Guid.TryParse(violationResult, out Guid parsedId) ? parsedId : (Guid?)null;
                        WriteChangeLog(model.SubjectId.Value, newViolationId, ConstsChangeLogEntity.Violation,
                            ConstsChangeLogAction.Add, null, "Ghi nhận lần vi phạm mới");
                    }
                }

                // Coi là thành công khi đã cập nhật được đối tượng hoặc đã ghi nhận vi phạm mới
                bool isSuccess = (changes.Count > 0 && !string.IsNullOrEmpty(result)) || hasViolationPayload;
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
            ViewBag.ListViolations = _violationCache.GetBySubjectId(id, User?.UserName);
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
            // Lấy dữ liệu gốc trước khi xoá để ghi log cho chính xác
            var currentSubject = _subjectCache.GetByIdFresh(model.SubjectId.Value);

            var isSuccess = _subjectCache.Delete(model.SubjectId.Value, User.UserName);

            if (isSuccess)
            {
                var subjectName = currentSubject?.FullName ?? model.FullName;
                var cardNumber = currentSubject?.IdentityCardNumber ?? model.IdentityCardNumber;
                WriteChangeLog(model.SubjectId.Value, null, ConstsChangeLogEntity.Subject,
                    ConstsChangeLogAction.Delete, null, $"Xoá đối tượng [{subjectName}] - CCCD [{cardNumber}]");
            }
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
                        var folder = Server.MapPath(SUBJECT_UPLOAD_FOLDER);
                        if (!Directory.Exists(folder))
                        {
                            Directory.CreateDirectory(folder);
                        }
                        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                        var path = Path.Combine(folder, fileName);
                        file.SaveAs(path);
                        var fileUrl = $"{SUBJECT_UPLOAD_URL}{fileName}";
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

        /// <summary>
        /// Màn hình Lịch sử vi phạm của một đối tượng.
        /// Danh sách đã được proc lọc theo phân quyền đơn vị + lĩnh vực và sắp xếp
        /// từ lần vi phạm gần nhất tới xa nhất.
        /// </summary>
        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult ViolationHistory(Guid id)
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

            model.ListViolations = _violationCache.GetBySubjectId(id, User?.UserName)
                                   ?? new List<MajorSubjectViolationModel>();
            ViewBag.CurrentUserName = User?.UserName;
            return PartialView("_ViolationHistory", model);
        }

        /// <summary>Màn hình Log cập nhật của một đối tượng.</summary>
        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult ChangeLog(Guid id)
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
            return PartialView("_ChangeLog", model);
        }

        /// <summary>Nguồn dữ liệu DataTables cho màn hình Log cập nhật.</summary>
        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult GetChangeLog(Guid subjectId)
        {
            var search = Request.Form.GetValues("search[value]")?[0];
            var draw = Request.Form.GetValues("draw")?[0];
            var orderDir = Request.Form.GetValues("order[0][dir]")?[0];
            var startRec = Convert.ToInt32(Request.Form.GetValues("start")?[0]);
            var pageSize = Convert.ToInt32(Request.Form.GetValues("length")?[0]);
            var dataSearch = new BaseSearchModel
            {
                Search = string.IsNullOrEmpty(search) ? null : search,
                Order = "0",
                OrderDir = string.IsNullOrEmpty(orderDir) ? "DESC" : orderDir,
                StartIndex = startRec,
                PageSize = pageSize
            };

            var data = _changeLogCache.Get(out int total, subjectId, null, User?.UserName, dataSearch);
            return Json(new { draw = Convert.ToInt32(draw), recordsTotal = total, recordsFiltered = total, data },
                JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            var list = _subjectCache.GetAll();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Tra cứu đối tượng đã có trong CSDL theo số CCCD/CMND để tự động đổ dữ liệu lên form thêm mới.
        /// </summary>
        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult LookupByIdentityCard(string identityCardNumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(identityCardNumber))
                {
                    return Json(new { status = false, message = "Vui lòng nhập số CCCD/CMND cần tra cứu." },
                        JsonRequestBehavior.AllowGet);
                }

                var subject = _subjectCache.GetByIdentityCardNumber(identityCardNumber);
                if (subject == null || !subject.SubjectId.HasValue || subject.SubjectId.Value == Guid.Empty)
                {
                    return Json(new
                    {
                        status = false,
                        isNotFound = true,
                        message = $"Không tìm thấy {_subjectTitle.ToLower()} nào có số CCCD [{identityCardNumber.Trim()}]. Bạn có thể khai báo mới."
                    }, JsonRequestBehavior.AllowGet);
                }

                var violations = _violationCache.GetBySubjectId(subject.SubjectId.Value, User?.UserName)
                                 ?? new System.Collections.Generic.List<MajorSubjectViolationModel>();

                var listViolations = violations.Select(item => new
                {
                    violationDateStr = item.ViolationDateStr,
                    behaviorNames = item.BehaviorNames,
                    treatmentMeasures = item.TreatmentMeasures,
                    relatedDocuments = item.RelatedDocuments,
                    images = item.Images,
                    notes = item.Notes,
                    createdBy = item.CreatedBy
                }).ToList();

                var data = new
                {
                    subjectId = subject.SubjectId.Value.ToString(),
                    identityCardNumber = subject.IdentityCardNumber,
                    fullName = subject.FullName,
                    otherName = subject.OtherName,
                    dateOfBirth = subject.DateOfBirth?.ToString("yyyy-MM-dd"),
                    gender = subject.Gender,
                    ethnicity = subject.Ethnicity,
                    religion = subject.Religion,
                    nationality = subject.Nationality,
                    phoneNumber = subject.PhoneNumber,
                    placeOfOrigin = subject.PlaceOfOrigin,
                    currentResidence = subject.CurrentResidence,
                    avatarUrl = subject.AvatarUrl,
                    identityCardFrontUrl = subject.IdentityCardFrontUrl,
                    identityCardBackUrl = subject.IdentityCardBackUrl,
                    violationCount = listViolations.Count,
                    listViolations
                };

                return Json(new { status = true, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "Lỗi tra cứu: " + ex.Message },
                    JsonRequestBehavior.AllowGet);
            }
        }
    }
}
