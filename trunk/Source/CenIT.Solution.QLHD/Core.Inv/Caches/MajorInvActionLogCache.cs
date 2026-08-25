using System.ComponentModel;
using System.Data;
using Core.Inv.Biz;
using TSFramework.Core.Members.Caching;

namespace Core.Inv.Caches
{
    [DataObject]
    public class MajorInvActionLogCache : CacheLayer
    {
        private MajorInvActionLogBiz _invActionLogApi;

        private MajorInvActionLogBiz Api => _invActionLogApi ?? (_invActionLogApi = new MajorInvActionLogBiz());

        protected override string[] MasterCacheKeyArray =>
            new[] { "InvActionLogsCache", "CENIT.APP.Cache" };

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int? Save(DataTable dataActionLogs)
        {
            var invActionLogId = Api.Save(dataActionLogs);
            if (invActionLogId > 0)
                // Invalidate the cache
                InvalidateCache();
            return invActionLogId;
        }
    }
}