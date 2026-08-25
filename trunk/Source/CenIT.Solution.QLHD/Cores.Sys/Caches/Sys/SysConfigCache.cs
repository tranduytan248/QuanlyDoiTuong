using System.Collections.Generic;
using System.ComponentModel;
using Cores.Sys.Biz.Sys;
using Cores.Sys.Models.Sys;
using TSFramework.App.Models;
using TSFramework.Core.Members.Caching;
using TSFramework.Core.Utils;

namespace Cores.Sys.Caches.Sys
{
    [DataObject]
    public class SysConfigCache : CacheLayer
    {
        private SysConfigBiz _configsApi;

        private SysConfigBiz Api => _configsApi ?? (_configsApi = new SysConfigBiz());

        protected override string[] MasterCacheKeyArray =>
            new[] { "SysConfigsCache", "CENIT.APP.Cache" };

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<SysConfigModel> GetAll()
        {
            const string rawKey = "AllConfigs";
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<SysConfigModel> configs) return configs;
            // Item not found in cache - retrieve it and insert it into the cache
            configs = Api.GetAll();
            AddCacheItem(rawKey, configs);

            return configs;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<SysConfigModel> Get(out int total, BaseSearchModel search = null)
        {
            var objectKey = EHashMD5.FromObject(search);
            var rawKey = string.Concat("ListMessages-", objectKey);
            var rawKeyTotal = string.Concat(rawKey, "-Total");
            total = 0;
            // See if the item is in the cache
            var configs = GetCacheItem(rawKey) as List<SysConfigModel>;
            var cacheTotal = (int?)GetCacheItem(rawKeyTotal);
            total = cacheTotal ?? 0;

            if (configs != null) return configs;
            // Item not found in cache - retrieve it and insert it into the cache
            configs = Api.GetList(out total, search);
            AddCacheItem(rawKey, configs);
            AddCacheItem(rawKeyTotal, total);

            return configs;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public SysConfigModel GetById(int configId)
        {
            if (configId < 0) return null;

            var rawKey = string.Concat("SysConfigByID-", configId);

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is SysConfigModel config) return config;
            // Item not found in cache - retrieve it and insert it into the cache
            config = Api.GetById(configId);
            if (config != null) AddCacheItem(rawKey, config);

            return config;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public SysConfigModel GetViaKey(string configKey)
        {
            if (string.IsNullOrEmpty(configKey)) return null;

            var rawKey = string.Concat("SysConfigByID-", configKey);

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is SysConfigModel config) return config;
            // Item not found in cache - retrieve it and insert it into the cache
            config = Api.GetViaKey(configKey);
            if (config != null) AddCacheItem(rawKey, config);

            return config;
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int? Save(SysConfigModel model)
        {
            var configId = Api.Save(model);
            if (configId > 0)
                // Invalidate the cache
                InvalidateCache();
            return configId;
        }

        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Delete(SysConfigModel model)
        {
            var isDeleted = Api.Delete(model);
            if (isDeleted)
                // Invalidate the cache
                InvalidateCache();
            return isDeleted;
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int? SaveRefFiles(SysConfigModel model)
        {
            var configId = Api.SaveRefFiles(model);
            if (configId > 0)
                // Invalidate the cache
                InvalidateCache();
            return configId;
        }
    }
}