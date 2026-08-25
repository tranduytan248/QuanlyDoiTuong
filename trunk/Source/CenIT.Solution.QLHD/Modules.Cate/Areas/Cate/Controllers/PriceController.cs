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
    public class PriceController : AppController
    {
        private readonly CatePriceCache _priceCache = new CatePriceCache();
        //private readonly CateSubSectionCache _subSectionCache = new CateSubSectionCache();

        private readonly string _priceTitle = AppProcessor.Messagor.GetMessage("Price_Label");

        // GET: Cate/Price
        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        public ActionResult Index()
        {
            //var model = new CatePriceSearchModel
            //{
            //    lst_SubSection = _SubSectionCache.GetAll()
            //};
            //return View(model);
            return View();
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult Get(CatePriceSearchModel searchModel)
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
            var data = _priceCache.Get(out var total, searchModel, dataSearch);

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
            //var model = new CatePriceModel
            //{
            //    lst_SubSection = _SubSectionCache.GetAll()
            //};
            //return PartialView("_Add", model);
            return PartialView("_Add");
        }

        /// <summary>
        /// Thêm mới đơn giá
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionType(Type = EnumActionType.Add)]
        public ActionResult Add(CatePriceModel model)
        {
            if (!ModelState.IsValid)
            {
                //model.lst_SubSection = _SubSectionCache.GetAll();
                //return PartialView("_Price", model);
                return PartialView("_Price");
            }

            var data = _priceCache.Save(model, User.UserName);

            string response = CreateMessage($"[{model.SubSectionName}] {model.Price}",
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
        public ActionResult Edit(int id)
        {
            var model = _priceCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_priceTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            //model.lst_SubSection = _SubSectionCache.GetAll();
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
        public ActionResult Edit(CatePriceModel model)
        {
            if (!ModelState.IsValid)
            {
                //model.lst_SubSection = _SubSectionCache.GetAll();
                return PartialView("_Price", model);
            }
            var data = _priceCache.Save(model, User.UserName);

            string response = CreateMessage($"[ {model.SubSectionName} ]  {model.Price}",
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
        public ActionResult Delete(int id = 0)
        {
            var model = _priceCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_priceTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Delete"),
                $"<b>[{model.SubSectionName}] {model.Price} </b>");
            return PartialView("_Delete", model);
        }

        /// <summary>
        /// Xóa đơn giá
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(CatePriceModel model)
        {
            var deleted = _priceCache.Delete(model);

            var response = CreateMessage($"[ {model.SubSectionName} ]  {model.Price}", EnumProcessType.Delete,
                deleted > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response });
        }
    }
}