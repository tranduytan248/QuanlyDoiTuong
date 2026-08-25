using System;
using System.ComponentModel;
using Core.Inv.Biz;
using Core.Inv.Models;
using TSFramework.Core.Members.Caching;

namespace Core.Inv.Caches
{
    public class MajorInvProductCache : CacheLayer
    {
        private MajorInvProductBiz _invProductApi;

        private MajorInvProductBiz Api => _invProductApi ?? (_invProductApi = new MajorInvProductBiz());

        protected override string[] MasterCacheKeyArray =>
            new[] { "InvsProductCache", "InvsCache", "CENIT.APP.Cache" };

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MajorInvProductModel GetById(Guid? invId)
        {
            if (invId == null) return null;

            var rawKey = string.Concat("InvByID-", invId);

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is MajorInvProductModel invProduct) return invProduct;
            // Item not found in cache - retrieve it and insert it into the cache
            invProduct = Api.GetById(invId);
            if (invProduct != null) AddCacheItem(rawKey, invProduct);

            return invProduct;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MajorInvProductModel GetProductsViaKey(string invKey)
        {
            if (string.IsNullOrEmpty(invKey)) return null;

            var rawKey = string.Concat("InvByInvKey-", invKey);

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is MajorInvProductModel invProductModel) return invProductModel;
            // Item not found in cache - retrieve it and insert it into the cache
            invProductModel = Api.GetProductsViaKey(invKey);
            if (invProductModel != null) AddCacheItem(rawKey, invProductModel);

            return invProductModel;
        }
    }
}