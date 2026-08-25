using Cores.Cate.Caches;
using Cores.Cate.Models;

using System;
using System.Web.Mvc;
using Cores.Base.Apps;
using TSFramework.App.Attributes;
using TSFramework.App.Models;
using TSFramework.App.Processors;
using TSFramework.Core.Enums;

namespace Modules.Cate.Areas.Cate.Controllers
{
    public class LandTypeController : AppController
    {
        private readonly CateLandTypeCache _landTypeCache = new CateLandTypeCache();
        private readonly string _landTypeTitle = AppProcessor.Messagor.GetMessage("LandType_Label");
        private readonly string _landTypeExisted = AppProcessor.Messagor.GetMessage("LandType_Label_Existed");

        // GET: Cate/LandType
        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Tìm kiếm loại đất
        /// </summary>
        /// <returns></returns>
        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult Get()
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
            var data = _landTypeCache.Get(out var total, dataSearch);
            var result = Json(
                new { draw = Convert.ToInt32(draw), recordsTotal = total, recordsFiltered = total, data },
                JsonRequestBehavior.AllowGet);
            return result;
        }

        /// <summary>
        /// Thêm mới loại đất
        /// </summary>
        /// <returns></returns>
        [AjaxOnly]
        [ActionType(Type = EnumActionType.Add)]
        [HttpGet]
        public ActionResult Add()
        {
            var model = new CateLandTypeModel();
            return PartialView("_Add", model);
        }

        /// <summary>
        /// Thêm mới loại đất
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionType(Type = EnumActionType.Add)]
        public ActionResult Add(CateLandTypeModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_LandType", model);
            }

            var data = _landTypeCache.Save(model, User.UserName);

            string response = CreateMessage($"{_landTypeExisted}",
              data == (int)EnumStatus.Existed ? EnumProcessType.DataExisted : EnumProcessType.Add,
              data > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);

            return Json(new { status = true, message = response });
        }

        /// <summary>
        /// Cập nhật loại đất
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult Edit(int id)
        {
            var model = _landTypeCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_landTypeTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            return PartialView("_Edit", model);
        }

        /// <summary>
        /// Cập nhật loại đất
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult Edit(CateLandTypeModel model)
        {
            if (!ModelState.IsValid) return PartialView("_LandType", model);
            var data = _landTypeCache.Save(model, User.UserName);

            string response = CreateMessage($"{_landTypeExisted}",
              data == (int)EnumStatus.Existed ? EnumProcessType.DataExisted : EnumProcessType.Edit,
              data > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);

            return Json(new { status = true, message = response });
        }

        /// <summary>
        /// Xóa loại đất
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(int id = 0)
        {
            var model = _landTypeCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_landTypeTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Delete"),
                $"<b>{_landTypeTitle} [{model.LandTypeName}]</b>");
            return PartialView("_Delete", model);
        }

        /// <summary>
        /// Xóa loại đất
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(CateLandTypeModel model)
        {
            var deleted = _landTypeCache.Delete(model);

            var response = CreateMessage($"{_landTypeTitle} [{model.LandTypeName}]", EnumProcessType.Delete,
                deleted > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response });
        }
    }
}