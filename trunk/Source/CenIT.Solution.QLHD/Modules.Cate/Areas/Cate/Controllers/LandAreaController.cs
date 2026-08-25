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
    public class LandAreaController : AppController
    {
        private readonly CateLandAreaCache _landAreaCache = new CateLandAreaCache();
        private readonly CateLandTypeCache _landTypeCache = new CateLandTypeCache();

        private readonly string _landAreaTitle = AppProcessor.Messagor.GetMessage("LandArea_Label");

        // GET: Cate/LandArea
        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        public ActionResult Index()
        {
            var model = new CateLandAreaSearchModel
            {
                ListLandTypes = _landTypeCache.GetAll()
            };
            return View(model);
        }

        /// <summary>
        /// Tìm kiếm diện tích đất
        /// </summary>
        /// <returns></returns>
        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult Get(CateLandAreaSearchModel searchModel)
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
            var data = _landAreaCache.Get(out var total, searchModel, dataSearch);

            var result = Json(
                new { draw = Convert.ToInt32(draw), recordsTotal = total, recordsFiltered = total, data },
                JsonRequestBehavior.AllowGet);
            return result;
        }

        /// <summary>
        /// Thêm mới diện tích đất
        /// </summary>
        /// <returns></returns>
        [AjaxOnly]
        [ActionType(Type = EnumActionType.Add)]
        [HttpGet]
        public ActionResult Add()
        {
            var model = new CateLandAreaModel
            {
                ListLandTypes = _landTypeCache.GetAll()
            };
            return PartialView("_Add", model);
        }

        /// <summary>
        /// Thêm mới diện tích đất
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionType(Type = EnumActionType.Add)]
        public ActionResult Add(CateLandAreaModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ListLandTypes = _landTypeCache.GetAll();
                return PartialView("_LandArea", model);
            }

            var data = _landAreaCache.Save(model, User.UserName);

            string response = CreateMessage($"{model.LandSize} [{model.LandTypeName}]",
              data == (int)EnumStatus.Existed ? EnumProcessType.DataExisted : EnumProcessType.Add,
              data > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);

            return Json(new { status = true, message = response });
        }

        /// <summary>
        /// Cập nhật diện tích đất
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult Edit(int id)
        {
            var model = _landAreaCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_landAreaTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            model.ListLandTypes = _landTypeCache.GetAll();
            return PartialView("_Edit", model);
        }

        /// <summary>
        /// Cập nhật diện tích đất
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult Edit(CateLandAreaModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ListLandTypes = _landTypeCache.GetAll();
                return PartialView("_LandArea", model);
            }
            var data = _landAreaCache.Save(model, User.UserName);

            string response = CreateMessage($"{model.LandSize} [{model.LandTypeName}]",
              data == (int)EnumStatus.Existed ? EnumProcessType.DataExisted : EnumProcessType.Edit,
              data > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);

            return Json(new { status = true, message = response });
        }

        /// <summary>
        /// Xóa diện tích đất
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(int id = 0)
        {
            var model = _landAreaCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_landAreaTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Delete"),
                $"<b>{model.LandSize} [{model.LandTypeName}]</b>");
            return PartialView("_Delete", model);
        }

        /// <summary>
        /// Xóa diện tích đất
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(CateLandAreaModel model)
        {
            var deleted = _landAreaCache.Delete(model);

            var response = CreateMessage($"{model.LandSize} [{model.LandTypeName}]", EnumProcessType.Delete,
                deleted > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response });
        }
    }
}