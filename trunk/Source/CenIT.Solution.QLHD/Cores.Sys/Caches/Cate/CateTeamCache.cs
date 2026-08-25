using System.Collections.Generic;
using System.ComponentModel;
using Cores.Sys.Biz.Cate;
using Cores.Sys.Models.Cate;
using TSFramework.App.Models;
using TSFramework.Core.Members.Caching;
using TSFramework.Core.Utils;

namespace Cores.Sys.Caches.Cate
{
    [DataObject]
    public class CateTeamCache : CacheLayer
    {
        private CateTeamBiz _teamApi;

        private CateTeamBiz Api => _teamApi ?? (_teamApi = new CateTeamBiz());

        protected override string[] MasterCacheKeyArray => new[]
            { "TeamsCache", "WardsCache", "CENIT.APP.Cache" };

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CateTeamModel> Get(out int total, string provinceIds, string wardIds, BaseSearchModel search = null)
        {
            var objectKey = EHashMD5.FromObject(search);
            var rawKey = string.Concat("ListTeams-", objectKey, "-ProvinceIds-", provinceIds, "-WardIds-", wardIds);
            var rawKeyTotal = string.Concat(rawKey, "-Total");
            total = 0;
            var cacheTotal = (int?)GetCacheItem(rawKeyTotal);
            total = cacheTotal ?? 0;
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<CateTeamModel> teams) return teams;
            // Item not found in cache - retrieve it and insert it into the cache
            teams = Api.LoadList(out total, provinceIds, wardIds, search);
            AddCacheItem(rawKey, teams);
            AddCacheItem(rawKeyTotal, total);

            return teams;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public CateTeamModel GetById(int? teamId)
        {
            if (teamId < 0) return null;

            var rawKey = string.Concat("TeamByID-", teamId);

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is CateTeamModel team) return team;
            // Item not found in cache - retrieve it and insert it into the cache
            team = Api.GetById(teamId);
            if (team != null) AddCacheItem(rawKey, team);

            return team;
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int? Save(CateTeamModel model)
        {
            var teamId = Api.Save(model);
            if (teamId > 0)
                // Invalidate the cache
                InvalidateCache();
            return teamId;
        }

        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Delete(CateTeamModel model)
        {
            var isDeleted = Api.Delete(model);
            if (isDeleted)
                // Invalidate the cache
                InvalidateCache();
            return isDeleted;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CateTeamModel> GetViaWardId(string wardIds)
        {
            var rawKey = $"GetViaWardId-{wardIds}";
            if (GetCacheItem(rawKey) is List<CateTeamModel> listTeams) return listTeams;
            // Item not found in cache - retrieve it and insert it into the cache
            listTeams = Api.GetViaWardId(wardIds);
            AddCacheItem(rawKey, listTeams);

            return listTeams;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CateTeamModel> GetAll(string provinceIds = null, string wardIds = null)
        {
            var rawKey = $"AllTeams-{provinceIds}-{wardIds}";
            if (GetCacheItem(rawKey) is List<CateTeamModel> listTeams) return listTeams;
            // Item not found in cache - retrieve it and insert it into the cache
            listTeams = Api.GetAll(provinceIds, wardIds);
            AddCacheItem(rawKey, listTeams);

            return listTeams;
        }
    }
}