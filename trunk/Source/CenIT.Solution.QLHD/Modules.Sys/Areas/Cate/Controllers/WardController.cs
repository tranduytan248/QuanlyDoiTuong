using Cores.Sys.Caches.Cate;
using Cores.Sys.Models.Cate;
using Modules.Sys.Areas.Cate.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Cores.Base.Apps;
using TSFramework.App.Attributes;
using TSFramework.App.Models;
using TSFramework.App.Processors;
using TSFramework.Core.Enums;
using TSFramework.Core.Utils;

namespace Modules.Sys.Areas.Cate.Controllers
{
    public class WardController : AppController
    {
        private readonly CateProvinceCache _provinceCache = new CateProvinceCache();
        private readonly CateWardCache _wardCache = new CateWardCache();

        private readonly string _wardTitle = AppProcessor.Messagor.GetMessage("Ward_Title");

        // GET: Cate/Wards

        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        public ActionResult Index()
        {
            var searchModel = new SearchWardModel
            {
                ListProvinces = _provinceCache.GetAll()
                    .OrderBy(d => d.ProvinceName)
                    .Select(d => new ListItem(d.ProvinceName, d.ProvinceId.ToString())).ToList()
            };
            return View(searchModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult Get(SearchWardModel searchModel)
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
            searchModel.ProvinceIds = string.IsNullOrEmpty(searchModel.ProvinceIds) ? null : searchModel.ProvinceIds;
            searchModel.ProvinceIds = string.IsNullOrEmpty(searchModel.ProvinceIds) ? null : searchModel.ProvinceIds;

            var data = _wardCache.Get(searchModel.ProvinceIds, out int total, dataSearch);
            var result = Json(
                new { draw = Convert.ToInt32(draw), recordsTotal = total, recordsFiltered = total, data },
                JsonRequestBehavior.AllowGet);
            return result;
        }

        [AjaxOnly]
        [ActionType(Type = EnumActionType.Add)]
        [HttpGet]
        public ActionResult Add(int? provinceId)
        {
            var provinceModel = _provinceCache.GetById(provinceId);

            var model = new CateWardModel
            {
                Provinces = _provinceCache.GetAll()
                    .Select(d => new ListItem(d.ProvinceName, d.ProvinceId.ToString())).ToList(),

                ProvinceId = provinceModel?.ProvinceId ?? 0,
                ProvinceCode = provinceModel?.ProvinceCode
            };
            return PartialView("_Add", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Add)]
        [ValidateInput(false)]
        public ActionResult Add(CateWardModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Provinces = _provinceCache.GetAll().OrderBy(d => d.ProvinceName)
                    .Select(d => new ListItem(d.ProvinceName, d.ProvinceId.ToString())).ToList();

                return PartialView("_Ward", model);
            }

            string response;
            var wardId = _wardCache.Save(new CateWardModel
            {
                ProvinceId = model.ProvinceId,
                WardId = model.WardId,
                WardCode = EString.RemoveSign4VietnameseString(model.WardName),
                WardName = model.WardName,
                UserCreated = User.UserName,
                DateCreated = DateTime.Now
            });

            if (wardId == 0)
                response = CreateMessage($"{_wardTitle} [{model.WardName}]", EnumProcessType.Add, EnumMsgIcon.Error);
            else if (wardId == -9)
                response = CreateMessage($"{_wardTitle} [ {model.WardName}]", EnumProcessType.DataExisted, EnumMsgIcon.Error);
            else
                response = CreateMessage($"{_wardTitle} [ {model.WardName}]", EnumProcessType.Add, EnumMsgIcon.Success);
            return Json(new { status = true, message = response }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult Edit(int id = 0)
        {
            var model = _wardCache.GetById(id);

            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_wardTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            model.Provinces = _provinceCache.GetAll().OrderBy(d => d.ProvinceName)
                .Select(d => new ListItem(d.ProvinceName, d.ProvinceId.ToString())).ToList();

            return PartialView("_Edit", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionType(Type = EnumActionType.Edit)]
        [ValidateInput(false)]
        public ActionResult Edit(CateWardModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Provinces = _provinceCache.GetAll().OrderBy(d => d.ProvinceName)
                    .Select(d => new ListItem(d.ProvinceName, d.ProvinceId.ToString())).ToList();

                return PartialView("_Ward", model);
            }

            string response;
            var wardId = _wardCache.Save(new CateWardModel
            {
                ProvinceId = model.ProvinceId,
                WardId = model.WardId,
                WardCode = model.WardCode,
                WardName = model.WardName,
                UserCreated = User.UserName,
                DateCreated = DateTime.Now
            });

            if (wardId == 0)
                response = CreateMessage($"{_wardTitle} [{model.WardName}]", EnumProcessType.Edit, EnumMsgIcon.Error);
            else if (wardId == -9)
                response = CreateMessage($"{_wardTitle} [ {model.WardName}]", EnumProcessType.DataExisted, EnumMsgIcon.Error);
            else
                response = CreateMessage($"{_wardTitle} [ {model.WardName}]", EnumProcessType.Edit, EnumMsgIcon.Success);
            return Json(new
            {
                status = true,
                message = response
            }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(int id = 0)
        {
            var model = _wardCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_wardTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Delete"),
                $"<b>{_wardTitle} [{model.WardName}]</b>");
            return PartialView("_Delete", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(CateWardModel model)
        {
            model.UserCreated = User.UserName;
            var deleted = _wardCache.Delete(model);
            var response = CreateMessage($"{model.WardName}", EnumProcessType.Delete,
                deleted ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response });
        }

        [AjaxOnly]
        [ActionType(Type = EnumActionType.Add)]
        [HttpGet]
        public ActionResult WardsViaProvince(int id)
        {
            var provinceModel = _provinceCache.GetById(id);
            if (provinceModel != null)
                return PartialView("_WardsViaProvince", new CateProvinceModel
                {
                    ProvinceId = provinceModel.ProvinceId,
                    ProvinceName = provinceModel.ProvinceName,
                });
            var provinceTitle = AppProcessor.Messagor.GetMessage("Province_Title");
            return Json(new
            {
                status = true,
                message = CreateMessage($"{provinceTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
            }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [ActionType(Type = EnumActionType.Add)]
        [HttpPost]
        public ActionResult GetWardsViaProvince(SearchWardModel searchModel)
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

            var data = _wardCache.Get(searchModel.ProvinceIds, out int total, dataSearch);
            var result = Json(
                new { draw = Convert.ToInt32(draw), recordsTotal = total, recordsFiltered = total, data },
                JsonRequestBehavior.AllowGet);
            return result;
        }
    }
}