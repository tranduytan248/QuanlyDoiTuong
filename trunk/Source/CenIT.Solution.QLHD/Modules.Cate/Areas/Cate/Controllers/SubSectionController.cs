using Cores.Cate.Caches;
using Cores.Cate.Models;

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
    public class SubSectionController : AppController
    {
        private readonly CateSubSectionCache _subSectionCache = new CateSubSectionCache();
        private readonly CateMainSectionCache _mainSectionCache = new CateMainSectionCache();

        private readonly string _subSectionTitle = AppProcessor.Messagor.GetMessage("SubSection_Label_Title");

        // GET: Cate/SubSection
        //[AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult Index(int id)
        {
            var mainSection = _mainSectionCache.GetById(id);
            var model = new CateSubSectionModel
            {
                Cate_MainSectionId = id,
                MainSectionName = mainSection?.MainSectionName
            };
            return View(model);
        }

        /// <summary>
        /// Tìm kiếm điều kiện lập đơn giá hợp đồng
        /// </summary>
        /// <returns></returns>
        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult Get(int mainSection)
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
            var data = _subSectionCache.Get(out int total, mainSection, dataSearch);

            var result = Json(
                new { draw = Convert.ToInt32(draw), recordsTotal = total, recordsFiltered = total, data },
                JsonRequestBehavior.AllowGet);
            return result;
        }

        /// <summary>
        /// Thêm mới điều kiện lập đơn giá hợp đồng
        /// </summary>
        /// <returns></returns>
        [AjaxOnly]
        [ActionType(Type = EnumActionType.Add)]
        [HttpGet]
        public ActionResult Add(int id)
        {
            var mainSection = _mainSectionCache.GetById(id);
            var model = new CateSubSectionModel
            {
                Cate_MainSectionId = id,
                MainSectionName = mainSection?.MainSectionName
            };
            return PartialView("_Add", model);
        }

        /// <summary>
        /// Thêm mới điều kiện lập đơn giá hợp đồng
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionType(Type = EnumActionType.Add)]
        public ActionResult Add(CateSubSectionModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_SubSection", model);
            }

            var data = _subSectionCache.Save(model, User.UserName);

            string response = CreateMessage($"{model.SubSectionName}",
              data == (int)EnumStatus.Existed ? EnumProcessType.DataExisted : EnumProcessType.Add,
              data > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);

            return Json(new { status = true, message = response });
        }

        /// <summary>
        /// Cập nhật điều kiện lập đơn giá hợp đồng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult Edit(int id)
        {
            var model = _subSectionCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_subSectionTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            return PartialView("_Edit", model);
        }

        /// <summary>
        /// Cập nhật điều kiện lập đơn giá hợp đồng
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult Edit(CateSubSectionModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_SubSection", model);
            }
            var data = _subSectionCache.Save(model, User.UserName);

            string response = CreateMessage($"{model.SubSectionName}",
              data == (int)EnumStatus.Existed ? EnumProcessType.DataExisted : EnumProcessType.Edit,
              data > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);

            return Json(new { status = true, message = response });
        }

        /// <summary>
        /// Xóa điều kiện lập đơn giá hợp đồng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(int id = 0)
        {
            var model = _subSectionCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_subSectionTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Delete"),
                $"<b>{model.SubSectionName}</b>");
            return PartialView("_Delete", model);
        }

        /// <summary>
        /// Xóa điều kiện lập đơn giá hợp đồng
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(CateSubSectionModel model)
        {
            var deleted = _subSectionCache.Delete(model.SubSectionId);

            var response = CreateMessage($"{model.SubSectionName}", EnumProcessType.Delete,
                deleted > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response });
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.View)]
        public JsonResult GetAreasViaTypeLand(int? typeLandId)
        {
            var lstAreas = _subSectionCache.GetAll(typeLandId);
            var dicAreas = new Dictionary<string, List<CateSubSectionModel>>();

            lstAreas.GroupBy(d => d.MainSectionName).ToList().ForEach(g =>
            {
                dicAreas.Add(g.Key, g.OrderBy(a => a.SubSectionId).ToList());
            });

            return Json(new { Areas = dicAreas }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.View)]
        public JsonResult GetDetailAreaViaId(int? areaId)
        {
            var areaModel = _subSectionCache.GetById(areaId);
            return Json(new { Area = areaModel }, JsonRequestBehavior.AllowGet);
        }
    }
}