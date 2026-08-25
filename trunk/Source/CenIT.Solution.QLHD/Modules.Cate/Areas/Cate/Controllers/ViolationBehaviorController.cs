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
    public class ViolationBehaviorController : AppController
    {
        private readonly CateViolationBehaviorCache _behaviorCache = new CateViolationBehaviorCache();
        private readonly CateFieldCache _fieldCache = new CateFieldCache();
        private readonly string _behaviorTitle = AppProcessor.Messagor.GetMessage("ViolationBehavior_Title");

        // GET: Cate/ViolationBehavior
        public ActionResult Index()
        {
            var searchModel = new SearchViolationBehaviorModel();
            ViewBag.ListFields = _fieldCache.GetAll();
            return View(searchModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult Get(SearchViolationBehaviorModel searchModel)
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
            var data = _behaviorCache.Get(out int total, searchModel?.Key, searchModel?.FieldId, dataSearch);
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
            ViewBag.ListFields = _fieldCache.GetAll();
            return PartialView("_Add", new CateViolationBehaviorModel());
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Add)]
        [ValidateInput(false)]
        public ActionResult Add(CateViolationBehaviorModel model)
        {
            ModelState.Remove("BehaviorId");
            if (!ModelState.IsValid)
            {
                ViewBag.ListFields = _fieldCache.GetAll();
                return PartialView("_ViolationBehavior", model);
            }

            var behaviorId = _behaviorCache.Save(model, User.UserName);

            string response = CreateMessage($"{_behaviorTitle} [{model.BehaviorName}]",
                behaviorId == (int)EnumStatus.Existed ? EnumProcessType.DataExisted : EnumProcessType.Add,
                behaviorId > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = behaviorId > 0, message = response }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult Edit(int id)
        {
            var model = _behaviorCache.GetById(id);
            if (model == null)
            {
                return Json(new
                {
                    status = false,
                    message = CreateMessage(_behaviorTitle, EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                }, JsonRequestBehavior.AllowGet);
            }
            ViewBag.ListFields = _fieldCache.GetAll();
            return PartialView("_Edit", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Edit)]
        [ValidateInput(false)]
        public ActionResult Edit(CateViolationBehaviorModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ListFields = _fieldCache.GetAll();
                return PartialView("_ViolationBehavior", model);
            }

            var behaviorId = _behaviorCache.Save(model, User.UserName);

            string response = CreateMessage($"{_behaviorTitle} [{model.BehaviorName}]",
                behaviorId == (int)EnumStatus.Existed ? EnumProcessType.DataExisted : EnumProcessType.Edit,
                behaviorId > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = behaviorId > 0, message = response }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(int id)
        {
            var model = _behaviorCache.GetById(id);
            if (model == null)
            {
                return Json(new
                {
                    status = false,
                    message = CreateMessage(_behaviorTitle, EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                }, JsonRequestBehavior.AllowGet);
            }
            ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Common_ConfirmMessage"),
                $"<b>{_behaviorTitle} [{model.BehaviorName}]</b>");
            return PartialView("_Delete", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(CateViolationBehaviorModel model)
        {
            var isSuccess = _behaviorCache.Delete(model, User.UserName);
            string response = CreateMessage($"{_behaviorTitle} [{model.BehaviorName}]", EnumProcessType.Delete,
                isSuccess ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = isSuccess, message = response });
        }

        [HttpGet]
        public ActionResult GetAll(int? fieldId = null)
        {
            var list = _behaviorCache.GetAll(fieldId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}
