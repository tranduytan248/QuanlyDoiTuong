using System.Collections.Generic;
using Cores.Sys.Biz.Sys;
using Cores.Sys.Models.Sys;
using TSFramework.Core.Members.Caching;

namespace Cores.Sys.Caches.Sys
{
    public class SysElnvAccountCache : CacheLayer
    {
        private SysElnvAccountBiz _elnvAccountApi;
        private SysElnvAccountBiz Api => _elnvAccountApi ?? (_elnvAccountApi = new SysElnvAccountBiz());

        protected override string[] MasterCacheKeyArray =>
            new[] { "SysElnvAccountCache", "CENIT.APP.Cache" };


        public List<SysElnvAccountModel> GetAll()
        {
            var rawKey = string.Concat("ListElnvAccount-");
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<SysElnvAccountModel> elnvAccounts) return elnvAccounts;
            // Item not found in cache - retrieve it and insert it into the cache
            elnvAccounts = Api.GetAll();
            if (elnvAccounts == null) return null;
            AddCacheItem(rawKey, elnvAccounts);

            return elnvAccounts;
        }

        public SysElnvAccountModel GetById(int userId)
        {
            if (userId < 0) return null;

            var rawKey = string.Concat("InvAccountByID-", userId);

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is SysElnvAccountModel invAccount) return invAccount;
            // Item not found in cache - retrieve it and insert it into the cache
            invAccount = Api.LoadDetail(userId);
            AddCacheItem(rawKey, invAccount);

            return invAccount;
        }

        public SysElnvAccountModel GetByUserName(string userName)
        {
            if (string.IsNullOrEmpty(userName)) return null;

            var rawKey = string.Concat("InvAccountByUserName-", userName);

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is SysElnvAccountModel invAccount) return invAccount;
            // Item not found in cache - retrieve it and insert it into the cache
            invAccount = Api.GetByUserName(userName);
            AddCacheItem(rawKey, invAccount);

            return invAccount;
        }

        public int? Save(SysElnvAccountModel model, string savedBy)
        {
            var userId = Api.Save(model, savedBy);
            // Invalidate the cache
            if (userId > 0) InvalidateCache();
            return userId;
        }
    }
}