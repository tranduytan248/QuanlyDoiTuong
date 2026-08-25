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
    public class MajorInvAdjustCache : CacheLayer
    {
        private MajorInvAdjustBiz _invApi;

        private MajorInvAdjustBiz Api => _invApi ?? (_invApi = new MajorInvAdjustBiz());

        protected override string[] MasterCacheKeyArray =>
            new[] { "InvAdjustCache", "CENIT.APP.Cache" };

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<MajorInvAdjustModel> GetInvAdjust(out int total, string userName, string managerUnions,
            string invNo = null, string pattern = null, string serials = null, string invTypes = null,
            string cusTaxCode = null, DateTime? createdFrom = null, DateTime? createdTo = null, string creators = null,
            string cusName = null, BaseSearchModel search = null)
        {
            var objectKey = EHashMD5.FromObject(search);
            var rawKey = string.Concat("ListInvAdjust-", objectKey, invNo, pattern, serials, invTypes, cusTaxCode,
                createdFrom, createdTo, creators, cusName);
            var rawKeyTotal = string.Concat(rawKey, "-Total");
            total = 0;
            var cacheTotal = (int?)GetCacheItem(rawKeyTotal);
            total = cacheTotal ?? 0;
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<MajorInvAdjustModel> lstAdjustInv) return lstAdjustInv;
            // Item not found in cache - retrieve it and insert it into the cache
            lstAdjustInv = Api.GetInvAdjust(out total, userName, managerUnions, invNo, pattern, serials, invTypes,
                createdFrom, createdTo, creators, cusName, cusTaxCode, search);
            AddCacheItem(rawKey, lstAdjustInv);
            AddCacheItem(rawKeyTotal, total);
            return lstAdjustInv;
        }
    }
}