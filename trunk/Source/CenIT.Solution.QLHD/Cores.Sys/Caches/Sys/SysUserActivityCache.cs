using System.Collections.Generic;
using Cores.Sys.Biz.Sys;
using Cores.Sys.Models.Sys;
using TSFramework.Core.Members.Caching;

namespace Cores.Sys.Caches.Sys
{
    /// <summary>
    /// Tang truy xuat phien dang hoat dong.
    ///
    /// CO Y KHONG CACHE: day la du lieu thoi gian thuc - cache lai thi man hinh
    /// giam sat se hien thong tin cu, dung nghia vo hieu hoa muc dich cua no.
    /// </summary>
    public class SysUserActivityCache : CacheLayer
    {
        private SysUserActivityBiz _api;
        private SysUserActivityBiz Api => _api ?? (_api = new SysUserActivityBiz());

        protected override string[] MasterCacheKeyArray =>
            new[] { "UserActivitiesCache", "CENIT.APP.Cache" };

        public void Track(string sessionId, string userName, string currentUrl,
            string screenName, string ipAddress, string userAgent)
        {
            Api.Track(sessionId, userName, currentUrl, screenName, ipAddress, userAgent);
        }

        public void End(string sessionId)
        {
            Api.End(sessionId);
        }

        public List<SysUserActivityModel> Get(int timeoutMinutes = 5)
        {
            return Api.Get(timeoutMinutes) ?? new List<SysUserActivityModel>();
        }
    }
}
