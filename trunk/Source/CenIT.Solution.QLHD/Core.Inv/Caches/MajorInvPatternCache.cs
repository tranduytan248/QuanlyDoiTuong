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
    public class MajorInvPatternCache : CacheLayer
    {
        private MajorInvPatternBiz _invPatternApi;

        private MajorInvPatternBiz Api => _invPatternApi ?? (_invPatternApi = new MajorInvPatternBiz());

        protected override string[] MasterCacheKeyArray =>
            new[] { "InvPatternsCache", "CENIT.APP.Cache" };

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<MajorInvPatternModel> GetAll()
        {
            var rawKey = "AllInvPatterns";
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<MajorInvPatternModel> invPatterns) return invPatterns;
            // Item not found in cache - retrieve it and insert it into the cache
            invPatterns = Api.GetAll();
            AddCacheItem(rawKey, invPatterns);

            return invPatterns;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<MajorInvPatternModel> GetByPattern(string pattern)
        {
            var rawKey = "GetByPattern";
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<MajorInvPatternModel> invPatterns) return invPatterns;
            // Item not found in cache - retrieve it and insert it into the cache
            invPatterns = Api.GetByPattern(pattern);
            AddCacheItem(rawKey, invPatterns);

            return invPatterns;
        }


        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<MajorInvPatternModel> Get(out int total, BaseSearchModel search = null)
        {
            var objectKey = EHashMD5.FromObject(search);
            var rawKey = $"ListInvPatterns-{objectKey}";
            var rawKeyTotal = string.Concat(rawKey, "-Total");
            var cacheTotal = (int?)GetCacheItem(rawKeyTotal);
            total = cacheTotal ?? 0;
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<MajorInvPatternModel> invPatterns) return invPatterns;
            // Item not found in cache - retrieve it and insert it into the cache
            invPatterns = Api.Get(out total, search);
            AddCacheItem(rawKey, invPatterns);
            AddCacheItem(rawKeyTotal, total);

            return invPatterns;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MajorInvPatternModel GetById(Guid? patternId)
        {
            if (patternId == null || patternId == Guid.Empty) return null;

            var rawKey = string.Concat("InvPatternByID-", patternId);

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is MajorInvPatternModel invPattern) return invPattern;
            // Item not found in cache - retrieve it and insert it into the cache
            invPattern = Api.GetById(patternId);
            if (invPattern != null) AddCacheItem(rawKey, invPattern);

            return invPattern;
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int? Save(MajorInvPatternModel model)
        {
            var patternId = Api.Save(model);
            if (patternId > 0)
                // Invalidate the cache
                InvalidateCache();
            return patternId;
        }

        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Delete(MajorInvPatternModel model)
        {
            var isDeleted = Api.Delete(model);
            if (isDeleted)
                // Invalidate the cache
                InvalidateCache();
            return isDeleted;
        }
    }
}