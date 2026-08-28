using System;
using System.Web.Mvc;
using Cores.Base.Apps;
using Cores.Cate.Caches;
using Cores.Cate.Models;
using TSFramework.App.Attributes;
using TSFramework.App.Models;
using TSFramework.App.Processors;
using TSFramework.Core.Enums;

namespace Modules.Cate.Areas.Cate.Controllers
{
    public class SubjectTypeController : AppController
    {
        private readonly CateSubjectTypeCache _subjectTypeCache = new CateSubjectTypeCache();
        private readonly string _subjectTypeTitle = AppProcessor.Messagor.GetMessage("SubjectType_Title") ?? "Loại đối tượng";

        // GET: Cate/SubjectType
        public ActionResult Index()
        {
            var searchModel = new SearchSubjectTypeModel();
            return View(searchModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult Get(SearchSubjectTypeModel searchModel)
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
            var data = _subjectTypeCache.Get(out int total, searchModel?.Key, dataSearch);
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
            return PartialView("_Add", new CateSubjectTypeModel { IsActive = true, SortOrder = 0 });
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Add)]
        [ValidateInput(false)]
        public ActionResult Add(CateSubjectTypeModel model)
        {
            ModelState.Remove("SubjectTypeId");
            if (!ModelState.IsValid)
            {
                return PartialView("_SubjectType", model);
            }

            var subjectTypeId = _subjectTypeCache.Save(model, User.UserName);

            string response = CreateMessage($"{_subjectTypeTitle} [{model.SubjectTypeName}]",
                subjectTypeId == (int)EnumStatus.Existed ? EnumProcessType.DataExisted : EnumProcessType.Add,
                subjectTypeId > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = subjectTypeId > 0, message = response }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult Edit(int id)
        {
            var model = _subjectTypeCache.GetById(id);
            if (model == null)
            {
                return Json(new
                {
                    status = false,
                    message = CreateMessage(_subjectTypeTitle, EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                }, JsonRequestBehavior.AllowGet);
            }
            return PartialView("_Edit", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Edit)]
        [ValidateInput(false)]
        public ActionResult Edit(CateSubjectTypeModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_SubjectType", model);
            }

            var result = _subjectTypeCache.Save(model, User.UserName);

            string response = CreateMessage($"{_subjectTypeTitle} [{model.SubjectTypeName}]",
                result == (int)EnumStatus.Existed ? EnumProcessType.DataExisted : EnumProcessType.Edit,
                result > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);

            return Json(new { status = result > 0, message = response }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(int id)
        {
            var model = _subjectTypeCache.GetById(id);
            if (model == null)
            {
                return Json(new
                {
                    status = false,
                    message = CreateMessage(_subjectTypeTitle, EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                }, JsonRequestBehavior.AllowGet);
            }
            return PartialView("_Delete", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(CateSubjectTypeModel model)
        {
            var result = _subjectTypeCache.Delete(model.SubjectTypeId, User.UserName);

            string response = CreateMessage($"{_subjectTypeTitle} [{model.SubjectTypeName}]",
                EnumProcessType.Delete,
                result > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = result > 0, message = response }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult ToggleStatus(int id)
        {
            var model = _subjectTypeCache.GetById(id);
            if (model == null)
            {
                return Json(new
                {
                    status = false,
                    message = CreateMessage(_subjectTypeTitle, EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                }, JsonRequestBehavior.AllowGet);
            }
            return PartialView("_ToggleStatus", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult ToggleStatus(CateSubjectTypeModel model)
        {
            var result = _subjectTypeCache.ToggleStatus(model.SubjectTypeId, User.UserName);
            string response = CreateMessage($"{_subjectTypeTitle} [{model.SubjectTypeName}]",
                EnumProcessType.Edit,
                result > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = result > 0, message = response }, JsonRequestBehavior.AllowGet);
        }
    }
}
