using Cores.Cate.Caches;
using Cores.Cate.Models;

using Modules.Cate.Areas.Cate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Cores.Base.Apps;
using TSFramework.App.Attributes;
using TSFramework.App.Models;
using TSFramework.App.Processors;
using TSFramework.Core.Enums;

namespace Modules.Cate.Areas.Cate.Controllers
{
    public class LandCalculationController : AppController
    {
        private readonly CateLandCalculationCache _landCalculationCache = new CateLandCalculationCache();
        private readonly CateContentLandCache _contentLandCache = new CateContentLandCache();

        private readonly string _landCalculationTitle = AppProcessor.Messagor.GetMessage("LandCalculation_Label");

        // GET: Cate/LandCalculation
        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        public ActionResult Index()
        {
            var model = new SearchLandCalculationModel
            {
                //ListContentLands = _contentLandCache.GetAll().Select(u => new SelectListItem
                //{
                //    Text = u.ContentLandName,
                //    Value = u.ContentLandId.ToString(),
                //    Group = new SelectListGroup { Name = u.ContractTypeName }
                //}).ToList()
                ListContentLands = _contentLandCache.GetAll()
            };
            return View(model);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult Get(SearchLandCalculationModel searchModel)
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
            var data = _landCalculationCache.Get(out var total, searchModel.ContentLandIds, dataSearch);

            var result = Json(
                new { draw = Convert.ToInt32(draw), recordsTotal = total, recordsFiltered = total, data },
                JsonRequestBehavior.AllowGet);
            return result;
        }

        /// <summary>
        /// Thêm mới đơn giá
        /// </summary>
        /// <returns></returns>
        [AjaxOnly]
        [ActionType(Type = EnumActionType.Add)]
        [HttpGet]
        public ActionResult Add()
        {
            var model = new CateLandCalculationModel
            {
                //ListContentLands = _contentLandCache.GetAll()
            };
            return PartialView("_Add", model);
        }

        /// <summary>
        /// Thêm mới đơn giá
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionType(Type = EnumActionType.Add)]
        public ActionResult Add(CateLandCalculationModel model)
        {
            if (!ModelState.IsValid)
            {
                //model.ListContentLands = _contentLandCache.GetAll();
                return PartialView("_LandCalculation", model);
            }

            var data = _landCalculationCache.Save(model, User.UserName);

            string response = CreateMessage($"[{model.Condition}]",
              data == (int)EnumStatus.Existed ? EnumProcessType.DataExisted : EnumProcessType.Add,
              data > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);

            return Json(new { status = true, message = response });
        }

        /// <summary>
        /// Cập nhật đơn giá
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult Edit(Guid id)
        {
            var model = _landCalculationCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_landCalculationTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            //model.ListContentLands = _contentLandCache.GetAll();
            return PartialView("_Edit", model);
        }

        /// <summary>
        /// Cập nhật đơn giá
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult Edit(CateLandCalculationModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ListContentLands = _contentLandCache.GetAll();
                return PartialView("_LandCalculation", model);
            }
            var data = _landCalculationCache.Save(model, User.UserName);

            string response = CreateMessage($"[ {model.Condition} ]",
              data == (int)EnumStatus.Existed ? EnumProcessType.DataExisted : EnumProcessType.Edit,
              data > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);

            return Json(new { status = true, message = response });
        }

        /// <summary>
        /// Xóa đơn giá
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(Guid id)
        {
            var model = _landCalculationCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_landCalculationTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Delete"),
                $"<b>[{model.Condition}] </b>");
            return PartialView("_Delete", model);
        }

        /// <summary>
        /// Xóa đơn giá
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(CateLandCalculationModel model)
        {
            var deleted = _landCalculationCache.Delete(model);

            var response = CreateMessage($"[ {model.Condition} ]", EnumProcessType.Delete,
                deleted > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response });
        }

        #region Ajax Function

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.View)]
        public JsonResult GetConditionsViaContentLand(Guid? contentLandId)
        {
            var lstConditions = _landCalculationCache.GetAll($"{contentLandId}");
            var dicConditions = new Dictionary<string, List<CateLandCalculationModel>>();

            lstConditions.GroupBy(d => d.ContentLandName).ToList().ForEach(g => { dicConditions.Add(g.Key, g.OrderBy(a => a.Condition).ToList()); });

            return Json(new { Conditions = dicConditions }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.View)]
        public JsonResult GetDetailConditionViaId(Guid? conditionId)
        {
            var conditionModel = _landCalculationCache.GetById(conditionId);
            return Json(new { Condition = conditionModel }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}