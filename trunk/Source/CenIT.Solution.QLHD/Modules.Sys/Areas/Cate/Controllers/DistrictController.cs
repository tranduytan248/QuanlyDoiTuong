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
    public class DistrictController : AppController
    {
        private readonly CateDistrictCache _districtCache = new CateDistrictCache();

        private readonly string _districtTitle = AppProcessor.Messagor.GetMessage("District_Title");
        private readonly CateProvinceCache _provinceCache = new CateProvinceCache();

        // GET: Cate/District
        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        public ActionResult Index()
        {
            var searchModel = new SearchDistrictModel
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
        public ActionResult Get(SearchDistrictModel searchModel)
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
            var data = _districtCache.Get(searchModel.ProvinceIds, out int total, dataSearch);
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
            var provinceModel = _provinceCache.GetById(provinceId.GetValueOrDefault(0));
            var model = new CateDistrictModel
            {
                ProvinceCode = provinceModel?.ProvinceCode,
                ProvinceId = provinceModel?.ProvinceId ?? 0,
                Provinces = _provinceCache.GetAll()
                    .OrderBy(d => d.ProvinceName)
                    .Select(d => new ListItem(d.ProvinceName, d.ProvinceId.ToString())).ToList()
            };
            return PartialView("_Add", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Add)]
        [ValidateInput(false)]
        public ActionResult Add(CateDistrictModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Provinces = _provinceCache.GetAll()
                    .Select(p => new ListItem(p.ProvinceName, p.ProvinceCode))
                    .ToList();
                return PartialView("_District", model);
            }

            string response;
            var districtId = _districtCache.Save(new CateDistrictModel
            {
                DistrictId = model.DistrictId ?? 0,
                ProvinceId = model.ProvinceId,
                DistrictCode = EString.RemoveSign4VietnameseString(model.DistrictName),
                DistrictName = model.DistrictName,
                UserCreated = User.UserName,
                DateCreated = DateTime.Now
            });

            if (districtId == 0)
                response = CreateMessage($"{_districtTitle} [{model.DistrictName}]", EnumProcessType.Add,
                    EnumMsgIcon.Error);
            else if (districtId == -9)
                response = CreateMessage($"{_districtTitle} [ {model.DistrictName}]", EnumProcessType.DataExisted,
                    EnumMsgIcon.Error
                );
            else
                response = CreateMessage($"{_districtTitle} [ {model.DistrictName}]", EnumProcessType.Add,
                    EnumMsgIcon.Success
                );
            return Json(new { status = true, message = response }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult Edit(int id = 0)
        {
            var model = _districtCache.GetById(id);

            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_districtTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            model.Provinces = _provinceCache.GetAll()
                .OrderBy(d => d.ProvinceName)
                .Select(d => new ListItem(d.ProvinceName, d.ProvinceId.ToString()))
                .ToList();
            return PartialView("_Edit", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionType(Type = EnumActionType.Edit)]
        [ValidateInput(false)]
        public ActionResult Edit(CateDistrictModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Provinces = _provinceCache.GetAll()
                    .Select(p => new ListItem(p.ProvinceName, p.ProvinceCode))
                    .ToList();
                return PartialView("_District", model);
            }

            string response;
            var districtId = _districtCache.Save(new CateDistrictModel
            {
                DistrictId = model.DistrictId,
                ProvinceId = model.ProvinceId,
                DistrictCode = model.DistrictCode,
                DistrictName = model.DistrictName,
                UserCreated = User.UserName,
                DateCreated = DateTime.Now
            });

            if (districtId == 0)
                response = CreateMessage($"{_districtTitle} [{model.DistrictName}]", EnumProcessType.Edit,
                    EnumMsgIcon.Error);
            else if (districtId == -9)
                response = CreateMessage($"{_districtTitle} [ {model.DistrictName}]", EnumProcessType.DataExisted,
                    EnumMsgIcon.Error
                );
            else
                response = CreateMessage($"{_districtTitle} [ {model.DistrictName}]", EnumProcessType.Edit,
                    EnumMsgIcon.Success
                );
            return Json(new { status = true, message = response }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(int id = 0)
        {
            var model = _districtCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_districtTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Delete"),
                $"<b>{_districtTitle} [{model.DistrictName}]</b>");
            return PartialView("_Delete", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(CateDistrictModel model)
        {
            model.UserCreated = User.UserName;
            var deleted = _districtCache.Delete(model);
            var response = CreateMessage($"{model.DistrictName}", EnumProcessType.Delete,
                deleted ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response });
        }

        [AjaxOnly]
        [ActionType(Type = EnumActionType.Add)]
        [HttpGet]
        public ActionResult DistrictsViaProvince(int id)
        {
            var provinceModel = _provinceCache.GetById(id);
            if (provinceModel != null)
                return PartialView("_DistrictsViaProvince",
                    new CateProvinceModel
                    { ProvinceId = provinceModel.ProvinceId, ProvinceName = provinceModel.ProvinceName });
            var provinceName = AppProcessor.Messagor.GetMessage("Province_Title");
            return Json(new
            {
                status = true,
                message = CreateMessage($"{provinceName}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
            }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [ActionType(Type = EnumActionType.Add)]
        [HttpPost]
        public ActionResult GetDistrictsViaProvince(SearchDistrictModel searchModel)
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

            var data = _districtCache.Get(searchModel.ProvinceIds, out int total, dataSearch);
            var result = Json(
                new { draw = Convert.ToInt32(draw), recordsTotal = total, recordsFiltered = total, data },
                JsonRequestBehavior.AllowGet);
            return result;
        }
    }
}