using System.Collections.Generic;
using System.ComponentModel;
using Cores.Cate.Biz;
using Cores.Cate.Models;
using TSFramework.App.Models;
using TSFramework.Core.Members.Caching;
using TSFramework.Core.Utils;

namespace Cores.Cate.Caches
{
    public class CateViolationBehaviorCache : CacheLayer
    {
        private CateViolationBehaviorBiz _behaviorApi;
        private CateViolationBehaviorBiz Api => _behaviorApi ?? (_behaviorApi = new CateViolationBehaviorBiz());

        protected override string[] MasterCacheKeyArray =>
            new[] { "ViolationBehaviorsCache", "CENIT.APP.Cache" };

        public void InvalidateAll()
        {
            InvalidateCache();
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CateViolationBehaviorModel> GetAll(int? fieldId = null)
        {
            var rawKey = string.Concat("AllBehaviors-", fieldId);
            if (GetCacheItem(rawKey) is List<CateViolationBehaviorModel> behaviors) return behaviors;
            behaviors = Api.GetAll(fieldId);
            AddCacheItem(rawKey, behaviors);
            return behaviors;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CateViolationBehaviorModel> Get(out int total, string key, int? fieldId, BaseSearchModel search = null)
        {
            var objectKey = EHashMD5.FromObject(search);
            var objectKey2 = EHashMD5.FromObject(key);
            var objectKey3 = EHashMD5.FromObject(fieldId);
            var rawKey = string.Concat("ListBehaviors-", objectKey, objectKey2, objectKey3);
            var rawKeyTotal = string.Concat(rawKey, "-Total");
            var cacheTotal = (int?)GetCacheItem(rawKeyTotal);
            total = cacheTotal ?? 0;
            if (GetCacheItem(rawKey) is List<CateViolationBehaviorModel> behaviors) return behaviors;
            behaviors = Api.Get(out total, key, fieldId, search);
            AddCacheItem(rawKey, behaviors);
            AddCacheItem(rawKeyTotal, total);
            return behaviors;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public CateViolationBehaviorModel GetById(int? behaviorId)
        {
            var rawKey = string.Concat("BehaviorDetail-", behaviorId);
            if (GetCacheItem(rawKey) is CateViolationBehaviorModel behavior) return behavior;
            behavior = Api.GetById(behaviorId);
            AddCacheItem(rawKey, behavior);
            return behavior;
        }

        public int? Save(CateViolationBehaviorModel model, string username)
        {
            var result = Api.Save(model, username);
            InvalidateCache();
            return result;
        }

        public bool Delete(CateViolationBehaviorModel model, string username)
        {
            var result = Api.Delete(model, username);
            InvalidateCache();
            return result;
        }
    }
}
