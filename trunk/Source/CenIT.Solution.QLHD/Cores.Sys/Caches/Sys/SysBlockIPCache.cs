using System.ComponentModel;
using Cores.Sys.Biz.Sys;
using Cores.Sys.Models.Sys;
using TSFramework.Core.Members.Caching;

namespace Cores.Sys.Caches.Sys
{
    [DataObject]
    public class SysBlockIPCache : CacheLayer
    {
        private SysBlockIPBiz _blockIPApi;

        protected override string[] MasterCacheKeyArray => new[] { "SysBlockIPCache", "CENIT.Application.Cache" };

        private SysBlockIPBiz Api => _blockIPApi ?? (_blockIPApi = new SysBlockIPBiz());


        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public SysBlockIPModel GetByIp(string sBlockIP)
        {
            if (string.IsNullOrEmpty(sBlockIP)) return null;

            // See if the item is in the cache
            // Item not found in cache - retrieve it and insert it into the cache
            var ipRequest = Api.GetByIP(sBlockIP);

            return ipRequest;
        }

        [DataObjectMethod(DataObjectMethodType.Insert, false)]
        public int? Add(SysBlockIPModel model)
        {
            var ret = Api.Add(model);
            if (ret > 0)
                // Invalidate the cache
                InvalidateCache();
            return ret;
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int? Request(SysBlockIPModel model)
        {
            var ret = Api.Request(model);
            if (ret > 0)
                // Invalidate the cache
                InvalidateCache();
            return ret;
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int? Unlock(string userName, string sIp)
        {
            var ret = Api.Unlock(userName, sIp);
            if (ret > 0)
                // Invalidate the cache
                InvalidateCache();
            return ret;
        }
    }
}