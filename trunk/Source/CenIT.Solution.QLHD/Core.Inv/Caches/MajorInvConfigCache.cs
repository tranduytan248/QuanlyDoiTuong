using System;
using System.Collections.Generic;
using System.ComponentModel;
using Core.Inv.Biz;
using Core.Inv.Models;
using TSFramework.App.Models;
using TSFramework.Core.Members.Caching;
using TSFramework.Core.Utils;

namespace Core.Inv.Caches
{
    [DataObject]
    public class MajorInvConfigCache : CacheLayer
    {
        private MajorInvConfigBiz _invConfigApi;

        private MajorInvConfigBiz Api => _invConfigApi ?? (_invConfigApi = new MajorInvConfigBiz());

        protected override string[] MasterCacheKeyArray =>
            new[] { "InvConfigsCache", "CENIT.APP.Cache" };

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<MajorInvConfigModel> GetAll()
        {
            var rawKey = "AllInvConfigs";
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<MajorInvConfigModel> invConfigs) return invConfigs;
            // Item not found in cache - retrieve it and insert it into the cache
            invConfigs = Api.GetAll();
            AddCacheItem(rawKey, invConfigs);

            return invConfigs;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<MajorInvConfigModel> Get(out int total, BaseSearchModel search = null)
        {
            var objectKey = EHashMD5.FromObject(search);
            var rawKey = $"ListInvConfigs-{objectKey}";
            var rawKeyTotal = string.Concat(rawKey, "-Total");
            var cacheTotal = (int?)GetCacheItem(rawKeyTotal);
            total = cacheTotal ?? 0;
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<MajorInvConfigModel> invConfigs) return invConfigs;
            // Item not found in cache - retrieve it and insert it into the cache
            invConfigs = Api.Get(out total, search);
            AddCacheItem(rawKey, invConfigs);
            AddCacheItem(rawKeyTotal, total);

            return invConfigs;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MajorInvConfigModel GetById(Guid? invConfigId)
        {
            if (invConfigId == null || invConfigId == Guid.Empty) return null;

            var rawKey = string.Concat("InvConfigByID-", invConfigId);

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is MajorInvConfigModel invConfig) return invConfig;
            // Item not found in cache - retrieve it and insert it into the cache
            invConfig = Api.GetById(invConfigId);
            if (invConfig != null) AddCacheItem(rawKey, invConfig);

            return invConfig;
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int? Save(MajorInvConfigModel model)
        {
            var invConfigId = Api.Save(model);
            if (invConfigId > 0)
                // Invalidate the cache
                InvalidateCache();
            return invConfigId;
        }

        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Delete(MajorInvConfigModel model)
        {
            var isDeleted = Api.Delete(model);
            if (isDeleted)
                // Invalidate the cache
                InvalidateCache();
            return isDeleted;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MajorInvConfigModel GetByKey(string configKey)
        {
            if (string.IsNullOrEmpty(configKey)) return null;

            var rawKey = string.Concat("InvConfigByKey-", configKey);

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is MajorInvConfigModel invConfig) return invConfig;
            // Item not found in cache - retrieve it and insert it into the cache
            invConfig = Api.GetByKey(configKey);
            if (invConfig != null) AddCacheItem(rawKey, invConfig);

            return invConfig;
        }
    }
}