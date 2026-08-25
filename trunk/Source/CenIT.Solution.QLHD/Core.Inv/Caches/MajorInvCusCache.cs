using System;
using System.ComponentModel;
using Core.Inv.Biz;
using Core.Inv.Models;
using TSFramework.Core.Members.Caching;

namespace Core.Inv.Caches
{
    public class MajorInvCusCache : CacheLayer
    {
        private MajorInvCusBiz _invCusApi;

        private MajorInvCusBiz Api => _invCusApi ?? (_invCusApi = new MajorInvCusBiz());

        protected override string[] MasterCacheKeyArray =>
            new[] { "InvsCusCache", "InvsCache", "CENIT.APP.Cache" };

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MajorInvCusModel GetById(Guid? invId)
        {
            if (invId == null) return null;

            var rawKey = string.Concat("InvByID-", invId);

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is MajorInvCusModel invCus) return invCus;
            // Item not found in cache - retrieve it and insert it into the cache
            invCus = Api.GetById(invId);
            if (invCus != null) AddCacheItem(rawKey, invCus);

            return invCus;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MajorInvCusModel GetByInvKey(string invKey)
        {
            if (string.IsNullOrEmpty(invKey)) return null;

            var rawKey = string.Concat("InvByInvKey-", invKey);

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is MajorInvCusModel invCus) return invCus;
            // Item not found in cache - retrieve it and insert it into the cache
            invCus = Api.GetByInvKey(invKey);
            if (invCus != null) AddCacheItem(rawKey, invCus);

            return invCus;
        }
    }
}