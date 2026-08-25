using Cores.Sys.Caches.Cate;
using Cores.Sys.Models.Cate;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Cores.Base.Apps;
using TSFramework.App.Attributes;
using TSFramework.Core.Enums;

namespace Modules.Sys.Areas.Cate.Controllers
{
    public class AjaxController : AppController
    {
        private readonly CateProvinceCache _provinceCache = new CateProvinceCache();
        private readonly CateTeamCache _teamCache = new CateTeamCache();
        private readonly CateWardCache _wardCache = new CateWardCache();

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.View)]
        public JsonResult GetWardsBelongProvinceCode(string provinceCode)
        {
            var lstWards = _wardCache.GetByProvinceCode(provinceCode);

            var dicWards = new Dictionary<string, List<CateWardModel>>();
            lstWards.GroupBy(d => d.ProvinceName).ToList().ForEach(g => { dicWards.Add(g.Key, g.ToList()); });

            return Json(new { Wards = dicWards }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.View)]
        public JsonResult GetWardsBelongProvinceId(int? provinceId)
        {
            var lstWards = _wardCache.GetByProvinceId(provinceId);

            var dicWards = new Dictionary<string, List<CateWardModel>>();
            lstWards.GroupBy(d => d.ProvinceName).ToList().ForEach(g => { dicWards.Add(g.Key, g.ToList()); });

            return Json(new { Wards = dicWards }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.View)]
        public JsonResult GetTeamsBelongWard(string wardIds)
        {
            var lstTeams = _teamCache.GetViaWardId(wardIds);
            var dicTeamsViaWard = new Dictionary<string, List<CateTeamModel>>();
            lstTeams.GroupBy(d => new { d.WardId, d.WardName }).ToList()
                .ForEach(g => { dicTeamsViaWard.Add($"{g.Key.WardId}_{g.Key.WardName}", g.ToList()); });

            return Json(new { Teams = dicTeamsViaWard }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.View)]
        public JsonResult GetListProvinces()
        {
            var lstProvinces = _provinceCache.GetAll();
            return Json(new { Provinces = lstProvinces }, JsonRequestBehavior.AllowGet);
        }
    }
}