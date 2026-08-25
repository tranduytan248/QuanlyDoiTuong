using Cores.Cate.Caches;
using Cores.Cate.Models;

using Modules.Cate.Areas.Cate.Models;
using System;
using System.Web.Mvc;
using Cores.Base.Apps;
using TSFramework.App.Attributes;
using TSFramework.App.Models;
using TSFramework.App.Processors;
using TSFramework.Core.Enums;

namespace Modules.Cate.Areas.Cate.Controllers
{
    public class PositionController : AppController
    {
        private readonly CatePositionCache _positionCache = new CatePositionCache();
        private readonly string _positionTitle = AppProcessor.Messagor.GetMessage("Position_Title");
        // GET: Cate/Position
        public ActionResult Index()
        {
            var searchModel = new SearchPositionModel();
            return View(searchModel);
        }

        /// <summary>
        /// Get danh sách Chức vụ
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult Get(SearchPositionModel searchModel)
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
            var data = _positionCache.Get(out int total, searchModel.Key, dataSearch);
            var result = Json(
                new { draw = Convert.ToInt32(draw), recordsTotal = total, recordsFiltered = total, data },
                JsonRequestBehavior.AllowGet);
            return result;
        }

        /// <summary>
        /// Thêm mới chức vụ
        /// </summary>
        /// <returns></returns>
        [AjaxOnly]
        [ActionType(Type = EnumActionType.Add)]
        [HttpGet]
        public ActionResult Add()
        {
            return PartialView("_Add");
        }

        /// <summary>
        /// Thêm mới quy định
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Add)]
        [ValidateInput(false)]
        public ActionResult Add(CatePositionModel model)
        {
            ModelState.Remove("PositionID");
            if (!ModelState.IsValid)
            {
                return PartialView("_Position", model);
            }

            var positionId = _positionCache.Save(model, User.UserName);

            string response = CreateMessage($"{_positionTitle} [{model.PositionName}]",
             positionId == (int)EnumStatus.Existed ? EnumProcessType.DataExisted : EnumProcessType.Add,
             positionId > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Giao diện cập nhật chuc vu
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult Edit(int id = 0)
        {
            var model = _positionCache.GetById(id);
            return PartialView("_Edit", model);
        }

        /// <summary>
        /// Cập nhật chuc vu
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionType(Type = EnumActionType.Edit)]
        [ValidateInput(false)]
        public ActionResult Edit(CatePositionModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_position", model);
            }

            var positionId = _positionCache.Save(model, User.UserName);
            string response = CreateMessage($"{_positionTitle} [{model.PositionName}]",
             positionId == (int)EnumStatus.Existed ? EnumProcessType.DataExisted : EnumProcessType.Add,
             positionId > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Giao diện xóa chuc vu
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(int id = 0)
        {
            var model = _positionCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_positionTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Delete"),
                $"<b>{_positionTitle} [{model.PositionName}]</b>");
            return PartialView("_Delete", model);
        }

        /// <summary>
        /// Xóa chuc vu
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(CatePositionModel model)
        {
            var deleted = _positionCache.Delete(model, User.UserName);

            var response = CreateMessage($"{_positionTitle} [{model.PositionName}]",
                EnumProcessType.Delete, deleted ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response });
        }
    }
}