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
    public class HolidayController : AppController
    {
        private readonly CateHolidayCache _cateHolidayCache = new CateHolidayCache();
        private readonly string _cateHolidayTitle = AppProcessor.Messagor.GetMessage("Holiday_Title");

        // GET: Cate/Holiday
        [ActionType(Type = EnumActionType.View)]
        public ActionResult Index()
        {
            var searchModel = new SearchHolidayModel();
            return View(searchModel);
        }

        /// <summary>
        /// Tìm kiếm 
        /// </summary>
        /// <returns></returns>
        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult Get(SearchHolidayModel searchModel)
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
            var data = _cateHolidayCache.Get(total: out var total, lunarCalendar: searchModel.LunarCalendar, search: dataSearch);

            var result = Json(
                new { draw = Convert.ToInt32(draw), recordsTotal = total, recordsFiltered = total, data },
                JsonRequestBehavior.AllowGet);
            return result;
        }

        /// <summary>
        /// Thêm mới 
        /// </summary>
        /// <returns></returns>
        [AjaxOnly]
        [ActionType(Type = EnumActionType.Add)]
        [HttpGet]
        public ActionResult Add()
        {
            var model = new CateHolidayModel();
            return PartialView("_Add", model);
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionType(Type = EnumActionType.Add)]
        public ActionResult Add(CateHolidayModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_Holiday", model);
            }

            model.Date = model.Day + "/" + model.Month + (model.IsPermanent ? "" : "/" + model.Year);

            var holidayID = _cateHolidayCache.Save(model, User.UserName);

            var response = CreateMessage($"[{model.HolidayName}]",
                holidayID == -9 ? EnumProcessType.DataExisted : EnumProcessType.Add,
                holidayID > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);

            return Json(new { status = true, message = response });
        }

        /// <summary>
        /// Cập nhật
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult Edit(int id)
        {
            var model = _cateHolidayCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_cateHolidayTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            // Phân tích chuỗi Date thành Day, Month, và Year
            //if (!string.IsNullOrEmpty(model.Date))
            //{
            //    var parts = model.Date.Split('/');
            //    if (parts.Length >= 2)
            //    {
            //        model.Day = parts[0];
            //        model.Month = parts[1];
            //        if (parts.Length == 3)
            //        {
            //            model.Year = parts[2];
            //        }
            //    }
            //}

            model.Day = $"{model.RealDate.Day}".PadLeft(2, '0');
            model.Month = $"{model.RealDate.Month}".PadLeft(2, '0');
            model.Year = $"{model.RealDate.Year}";

            return PartialView("_Edit", model);
        }

        /// <summary>
        /// Cập nhật 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult Edit(CateHolidayModel model)
        {

            if (!ModelState.IsValid)
            {
                return PartialView("_Holiday", model);
            }

            model.Date = model.Day + "/" + model.Month + (model.IsPermanent ? "" : "/" + model.Year);

            var holidayID = _cateHolidayCache.Save(model, User.UserName);

            var response = CreateMessage($"{_cateHolidayTitle} [{model.HolidayName}]",
             holidayID == -9 ? EnumProcessType.DataExisted : EnumProcessType.Edit,
                holidayID > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);

            return Json(new { status = true, message = response });
        }

        /// <summary>
        /// Xóa 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(int id = 0)
        {
            var model = _cateHolidayCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_cateHolidayTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Delete"),
                $"<b>{_cateHolidayTitle} [{model.HolidayName}]</b>");
            return PartialView("_Delete", model);
        }

        /// <summary>
        /// Xóa 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(CateHolidayModel model)
        {
            var deleted = _cateHolidayCache.Delete(model, User.UserName);

            var response = CreateMessage($"{_cateHolidayTitle} [{model.HolidayName}]", EnumProcessType.Delete,
                deleted > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response });
        }

        #region Test GetListHolidays

        //[HttpGet]
        //public ActionResult GetListHolidays()
        //{
        //    List<DateTime> listdate = HolidayHelper.GetListHolidays(2024);
        //    return Json(new { status = true, message = listdate });
        //}

        #endregion

        #region Test send email

        //[HttpGet]
        //public ActionResult SendEmail()
        //{
        //    ContentNotificationModel model = new ContentNotificationModel();

        //    model.customer.Email = "chi.pa.devit@gmail.com";
        //    model.customer.CusName = "Pham An Chi";
        //    model.contract.ContractNo = "123";
        //    model.contract.ContractSignal = "HD";

        //    SendNotificationHelper.Send(model);

        //    return Json(new
        //    {
        //        status = true,
        //        message = ""
        //    }, JsonRequestBehavior.AllowGet);
        //}

        #endregion
    }
}