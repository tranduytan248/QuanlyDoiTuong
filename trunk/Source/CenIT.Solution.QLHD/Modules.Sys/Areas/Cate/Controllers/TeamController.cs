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
    public class TeamController : AppController
    {
        private readonly CateProvinceCache _provinceCache = new CateProvinceCache();
        private readonly CateWardCache _wardCache = new CateWardCache();
        private readonly CateTeamCache _teamCache = new CateTeamCache();

        private readonly string _teamTitle = AppProcessor.Messagor.GetMessage("Team_Title");

        // GET: Cate/Wards

        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        public ActionResult Index()
        {
            var searchModel = new SearchTeamModel
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
        public ActionResult Get(SearchTeamModel searchModel)
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
            searchModel.WardIds = string.IsNullOrEmpty(searchModel.WardIds) ? null : searchModel.WardIds;

            var data = _teamCache.Get(out int total, searchModel.ProvinceIds, searchModel.WardIds, dataSearch);
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

            var model = new CateTeamModel
            {
                Provinces = _provinceCache.GetAll()
                    .OrderBy(d => d.ProvinceName)
                    .Select(d => new ListItem(d.ProvinceName, d.ProvinceId.ToString())).ToList(),
                ProvinceId = wardModel?.ProvinceId,
                WardId = wardId,
                Wards = _wardCache.GetAll(wardModel?.ProvinceId)
                    .Select(d => new ListItem($"{d.WardName}", d.WardId.ToString()))
                    .ToList()
            };
            return PartialView("_Add", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Add)]
        [ValidateInput(false)]
        public ActionResult Add(CateTeamModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Provinces = _provinceCache.GetAll()
                    .OrderBy(d => d.ProvinceName)
                    .Select(d => new ListItem(d.ProvinceName, d.ProvinceId.ToString())).ToList();
                model.Wards = _wardCache.GetAll(model.ProvinceId)
                    .Select(d => new ListItem($"{d.WardName}", d.WardId.ToString()))
                    .ToList();
                return PartialView("_Team", model);
            }

            string response;
            var teamId = _teamCache.Save(new CateTeamModel
            {
                WardId = model.WardId,
                TeamId = model.TeamId,
                TeamCode = EString.RemoveSign4VietnameseString(model.TeamName),
                TeamName = model.TeamName,
                UserCreated = User.UserName,
                DateCreated = DateTime.Now
            });

            if (teamId == 0)
                response = CreateMessage($"{_teamTitle} [{model.WardName}]", EnumProcessType.Add,
                    EnumMsgIcon.Error);
            else if (teamId == -9)
                response = CreateMessage($"{_teamTitle} [ {model.WardName}]", EnumProcessType.DataExisted,
                    EnumMsgIcon.Error
                );
            else
                response = CreateMessage($"{_teamTitle} [ {model.WardName}]", EnumProcessType.Add, EnumMsgIcon.Success
                );
            return Json(new { status = true, message = response }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult Edit(int id = 0)
        {
            var model = _teamCache.GetById(id);

            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_teamTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            model.Provinces = _provinceCache.GetAll().OrderBy(d => d.ProvinceName)
                .Select(d => new ListItem(d.ProvinceName, d.ProvinceId.ToString())).ToList();
           
            model.Wards = _wardCache.GetAll(model.ProvinceId)
                .Select(d => new ListItem($"{d.WardName}", d.WardId.ToString()))
                .ToList();

            return PartialView("_Edit", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionType(Type = EnumActionType.Edit)]
        [ValidateInput(false)]
        public ActionResult Edit(CateTeamModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Provinces = _provinceCache.GetAll().OrderBy(d => d.ProvinceName)
                    .Select(d => new ListItem(d.ProvinceName, d.ProvinceId.ToString())).ToList();
                model.Wards = _wardCache.GetAll(model.ProvinceId)
                    .Select(d => new ListItem($"{d.WardName}", d.WardId.ToString()))
                    .ToList();

                return PartialView("_Team", model);
            }

            string response;
            var teamId = _teamCache.Save(new CateTeamModel
            {
                WardId = model.WardId,
                WardCode = model.WardCode,
                TeamId = model.TeamId,
                TeamName = model.TeamName,
                TeamCode = model.TeamCode,
                UserCreated = User.UserName,
                DateCreated = DateTime.Now
            });

            if (teamId == 0)
                response = CreateMessage($"{_teamTitle} [{model.WardName}]", EnumProcessType.Edit,
                    EnumMsgIcon.Error);
            else if (teamId == -9)
                response = CreateMessage($"{_teamTitle} [ {model.WardName}]", EnumProcessType.DataExisted,
                    EnumMsgIcon.Error
                );
            else
                response = CreateMessage($"{_teamTitle} [ {model.WardName}]", EnumProcessType.Edit,
                    EnumMsgIcon.Success);
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
            var model = _teamCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_teamTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Delete"),
                $"<b>{_teamTitle} [{model.WardName}]</b>");
            return PartialView("_Delete", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(CateTeamModel model)
        {
            model.UserCreated = User.UserName;
            var deleted = _teamCache.Delete(model);
            var response = CreateMessage($"{model.WardName}", EnumProcessType.Delete,
                deleted ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response });
        }

        [AjaxOnly]
        [ActionType(Type = EnumActionType.Add)]
        [HttpGet]
        public ActionResult TeamsViaWard(int id)
        {
            var wardModel = _wardCache.GetById(id);
            if (wardModel != null)
                return PartialView("_TeamsViaWard", new CateWardModel
                {
                    ProvinceId = wardModel.ProvinceId,
                    ProvinceName = wardModel.ProvinceName,

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
        public ActionResult GetTeamsViaWard(SearchTeamModel searchModel)
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

            var data = _teamCache.Get(out int total, searchModel.ProvinceIds, searchModel.WardIds, dataSearch);
            var result = Json(
                new { draw = Convert.ToInt32(draw), recordsTotal = total, recordsFiltered = total, data },
                JsonRequestBehavior.AllowGet);
            return result;
        }
    }
}