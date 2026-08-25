using Cores.Sys.Caches.Cate;
using Cores.Sys.Models.Cate;
using Modules.Sys.Areas.Cate.Models;
using System;
using System.Collections.Generic;
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
    public class StreetController : AppController
    {
        private readonly CateDistrictCache _districtCache = new CateDistrictCache();
        private readonly CateProvinceCache _provinceCache = new CateProvinceCache();
        private readonly CateStreetCache _streetCache = new CateStreetCache();

        private readonly string _streetTitle = AppProcessor.Messagor.GetMessage("Street_Title");
        private readonly CateWardCache _wardCache = new CateWardCache();

        // GET: Cate/Wards

        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        public ActionResult Index()
        {
            var searchModel = new SearchStreetModel
            {
                ListProvinces = _provinceCache.GetAll()
                    .OrderBy(d => d.ProvinceName)
                    .Select(d => new ListItem(d.ProvinceName, d.ProvinceId.ToString())).ToList()
                //ListDistricts = _districtCache.GetAll()
                //    .OrderBy(d => d.DistrictName)
                //    .Select(d => new ListItem(d.DistrictName, d.DistrictId.ToString())).ToList(),
                //ListWards = _wardCache.GetAll()
                //    .OrderBy(d => d.WardName)
                //    .Select(d => new ListItem(d.WardName, d.WardId.ToString())).ToList()
            };
            return View(searchModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult Get(SearchStreetModel searchModel)
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
            searchModel.ProvincesIds = string.IsNullOrEmpty(searchModel.ProvincesIds) ? null : searchModel.ProvincesIds;
            searchModel.DistrictIds = string.IsNullOrEmpty(searchModel.DistrictIds) ? null : searchModel.DistrictIds;
            searchModel.WardIds = string.IsNullOrEmpty(searchModel.WardIds) ? null : searchModel.WardIds;

            var data = _streetCache.Get(out int total, searchModel.ProvincesIds, searchModel.DistrictIds,
                searchModel.WardIds,
                dataSearch);
            var result = Json(
                new { draw = Convert.ToInt32(draw), recordsTotal = total, recordsFiltered = total, data },
                JsonRequestBehavior.AllowGet);
            return result;
        }

        [AjaxOnly]
        [ActionType(Type = EnumActionType.Add)]
        [HttpGet]
        public ActionResult Add(int? wardId)
        {
            var wardModel = _wardCache.GetById(wardId.GetValueOrDefault(0));

            var model = new CateStreetModel
            {
                Provinces = _provinceCache.GetAll()
                    .OrderBy(d => d.ProvinceName)
                    .Select(d => new ListItem(d.ProvinceName, d.ProvinceId.ToString())).ToList(),
                ProvinceId = wardModel?.ProvinceId,
                DistrictId = wardModel?.DistrictId,
                Districts = _districtCache.GetAll(wardModel?.ProvinceId?.ToString())
                    .Select(d => new ListItem($"{d.DistrictName}", d.DistrictId.ToString()))
                    .ToList(),
                SelectedWardIds = wardId != null ? new List<int> { wardId.Value } : new List<int>(),
                Wards = _wardCache.GetAll(wardModel?.ProvinceId, wardModel?.DistrictId)
                    .Select(d => new ListItem(d.WardName, d.WardId.ToString()))
                    .ToList()
            };
            return PartialView("_Add", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Add)]
        [ValidateInput(false)]
        public ActionResult Add(CateStreetModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Provinces = _provinceCache.GetAll()
                    .OrderBy(d => d.ProvinceName)
                    .Select(d => new ListItem(d.ProvinceName, d.ProvinceId.ToString())).ToList();
                //model.ProvinceId = _provinceCache.GetViaDistrict(model.DistrictId)?.ProvinceId;
                model.Districts = _districtCache.GetAll(model.ProvinceId.ToString())
                    .Select(d => new ListItem($"{d.DistrictName}", d.DistrictId.ToString()))
                    .ToList();
                model.Wards = _wardCache.GetAll(model.ProvinceId, model.DistrictId)
                    .Select(d => new ListItem(d.WardName, d.WardId.ToString()))
                    .ToList();
                return PartialView("_Street", model);
            }

            string response;
            var streetId = _streetCache.Save(new CateStreetModel
            {
                WardIds = string.Join(",", model.SelectedWardIds),
                StreetId = model.StreetId,
                ParentId = model.ParentId,
                DistrictId = model.DistrictId,
                StreetCode = EString.RemoveSign4VietnameseString(model.StreetName),
                StreetName = model.StreetName,
                UserCreated = User.UserName,
                DateCreated = DateTime.Now
            });

            if (streetId == 0)
                response = CreateMessage($"{_streetTitle} [{model.StreetName}]", EnumProcessType.Add,
                    EnumMsgIcon.Error);
            else if (streetId == -9)
                response = CreateMessage($"{_streetTitle} [ {model.StreetName}]", EnumProcessType.DataExisted,
                    EnumMsgIcon.Error
                );
            else
                response = CreateMessage($"{_streetTitle} [ {model.StreetName}]", EnumProcessType.Add,
                    EnumMsgIcon.Success);
            return Json(new { status = true, message = response }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult Edit(int id = 0)
        {
            var model = _streetCache.GetById(id);

            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_streetTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var provinceModel = _provinceCache.GetViaDistrict(model.DistrictId);
            model.ProvinceId = provinceModel.ProvinceId;

            model.Provinces = _provinceCache.GetAll().OrderBy(d => d.ProvinceName)
                .Select(d => new ListItem
                {
                    Text = d.ProvinceName,
                    Value = d.ProvinceId.ToString(),
                    Selected = d.ProvinceId == model.ProvinceId
                }).ToList();

            model.Districts = _districtCache.GetAll(provinceModel.ProvinceId.ToString())
                .OrderBy(d => d.DistrictName)
                .Select(d => new ListItem(d.DistrictName, d.DistrictId.ToString())).ToList();

            model.Wards = _wardCache.GetAll(model.ProvinceId, model.DistrictId)
                .Select(d => new ListItem { Text = d.WardName, Value = d.WardId.ToString() })
                .ToList();

            model.SelectedWardIds = _wardCache.GetByStreetId(model.StreetId, out int _).Select(w => w.WardId).ToList();

            return PartialView("_Edit", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionType(Type = EnumActionType.Edit)]
        [ValidateInput(false)]
        public ActionResult Edit(CateStreetModel model)
        {
            if (!ModelState.IsValid)
            {
                var provinceModel = _provinceCache.GetViaDistrict(model.DistrictId);
                model.ProvinceId = provinceModel.ProvinceId;

                model.Provinces = _provinceCache.GetAll().OrderBy(d => d.ProvinceName)
                    .Select(d => new ListItem(d.ProvinceName, d.ProvinceId.ToString())).ToList();
                model.Districts = _districtCache.GetAll(provinceModel.ProvinceId.ToString())
                    .OrderBy(d => d.DistrictName)
                    .Select(d => new ListItem(d.DistrictName, d.DistrictId.ToString())).ToList();
                model.Wards = _wardCache.GetAll(model.ProvinceId, model.DistrictId)
                    .Select(d => new ListItem(d.WardName, d.WardId.ToString()))
                    .ToList();

                return PartialView("_Street", model);
            }

            string response;
            var streetId = _streetCache.Save(new CateStreetModel
            {
                WardIds = string.Join(",", model.SelectedWardIds),
                //WardId = model.WardId,
                StreetId = model.StreetId,
                ParentId = model.ParentId,
                DistrictId = model.DistrictId,
                StreetName = model.StreetName,
                StreetCode = model.StreetCode,
                UserCreated = User.UserName,
                DateCreated = DateTime.Now
            });

            if (streetId == 0)
                response = CreateMessage($"{_streetTitle} [{model.StreetName}]", EnumProcessType.Edit,
                    EnumMsgIcon.Error);
            else if (streetId == -9)
                response = CreateMessage($"{_streetTitle} [ {model.StreetName}]", EnumProcessType.DataExisted,
                    EnumMsgIcon.Error
                );
            else
                response = CreateMessage($"{_streetTitle} [ {model.StreetName}]", EnumProcessType.Edit,
                    EnumMsgIcon.Success
                );
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
            var model = _streetCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_streetTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Delete"),
                $"<b>{_streetTitle} [{model.StreetName}]</b>");
            return PartialView("_Delete", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(CateStreetModel model)
        {
            model.UserCreated = User.UserName;
            var deleted = _streetCache.Delete(model);
            var response = CreateMessage($"{model.StreetName}", EnumProcessType.Delete,
                deleted ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response });
        }

        [AjaxOnly]
        [ActionType(Type = EnumActionType.Add)]
        [HttpGet]
        public ActionResult StreetsViaWard(int id)
        {
            var wardModel = _wardCache.GetById(id);
            if (wardModel != null)
                return PartialView("_StreetsViaWard", new CateWardModel
                {
                    ProvinceId = wardModel.ProvinceId,
                    ProvinceName = wardModel.ProvinceName,
                    DistrictId = wardModel.DistrictId,
                    DistrictName = wardModel.DistrictName,

                    WardId = wardModel.WardId,
                    WardName = wardModel.WardName,
                    WardCode = wardModel.WardCode
                });
            var wardTitle = AppProcessor.Messagor.GetMessage("Ward_Title");
            return Json(new
            {
                status = true,
                message = CreateMessage($"{wardTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
            }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [ActionType(Type = EnumActionType.Add)]
        [HttpPost]
        public ActionResult GetStreetsViaWard(SearchStreetModel searchModel)
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

            var data = _streetCache.Get(out int total, searchModel.ProvincesIds, searchModel.DistrictIds,
                searchModel.WardIds, dataSearch);
            var result = Json(
                new { draw = Convert.ToInt32(draw), recordsTotal = total, recordsFiltered = total, data },
                JsonRequestBehavior.AllowGet);
            return result;
        }
    }
}