using System;
using System.Web.Mvc;
using Cores.Base.Apps;
using Cores.Cate.Caches;
using Cores.Cate.Models;
using Modules.Cate.Areas.Cate.Models;
using TSFramework.App.Attributes;
using TSFramework.App.Models;
using TSFramework.App.Processors;
using TSFramework.Core.Enums;

namespace Modules.Cate.Areas.Cate.Controllers
{
    public class FieldController : AppController
    {
        private readonly CateFieldCache _fieldCache = new CateFieldCache();
        private readonly string _fieldTitle = AppProcessor.Messagor.GetMessage("Field_Title");

        // GET: Cate/Field
        public ActionResult Index()
        {
            var searchModel = new SearchFieldModel();
            return View(searchModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult Get(SearchFieldModel searchModel)
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
            var data = _fieldCache.Get(out int total, searchModel?.Key, dataSearch);
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
            return PartialView("_Add", new CateFieldModel());
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Add)]
        [ValidateInput(false)]
        public ActionResult Add(CateFieldModel model)
        {
            ModelState.Remove("FieldId");
            if (!ModelState.IsValid)
            {
                return PartialView("_Field", model);
            }

            var fieldId = _fieldCache.Save(model, User.UserName);

            string response = CreateMessage($"{_fieldTitle} [{model.FieldName}]",
                fieldId == (int)EnumStatus.Existed ? EnumProcessType.DataExisted : EnumProcessType.Add,
                fieldId > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = fieldId > 0, message = response }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult Edit(int id)
        {
            var model = _fieldCache.GetById(id);
            if (model == null)
            {
                return Json(new
                {
                    status = false,
                    message = CreateMessage(_fieldTitle, EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                }, JsonRequestBehavior.AllowGet);
            }
            return PartialView("_Edit", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Edit)]
        [ValidateInput(false)]
        public ActionResult Edit(CateFieldModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_Field", model);
            }

            var fieldId = _fieldCache.Save(model, User.UserName);

            string response = CreateMessage($"{_fieldTitle} [{model.FieldName}]",
                fieldId == (int)EnumStatus.Existed ? EnumProcessType.DataExisted : EnumProcessType.Edit,
                fieldId > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = fieldId > 0, message = response }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(int id)
        {
            var model = _fieldCache.GetById(id);
            if (model == null)
            {
                return Json(new
                {
                    status = false,
                    message = CreateMessage(_fieldTitle, EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                }, JsonRequestBehavior.AllowGet);
            }
            ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Common_ConfirmMessage"),
                $"<b>{_fieldTitle} [{model.FieldName}]</b>");
            return PartialView("_Delete", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(CateFieldModel model)
        {
            var isSuccess = _fieldCache.Delete(model, User.UserName);
            string response = CreateMessage($"{_fieldTitle} [{model.FieldName}]", EnumProcessType.Delete,
                isSuccess ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = isSuccess, message = response });
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            var list = _fieldCache.GetAll();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}
