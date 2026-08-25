using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Cores.Major.Biz;
using Cores.Major.Models;
using TSFramework.Core.Members.Caching;

namespace Cores.Major.Caches
{
    public class MajorReportCache : CacheLayer
    {
        private MajorReportBiz _formApi;

        private MajorReportBiz Api => _formApi ?? (_formApi = new MajorReportBiz());

        protected override string[] MasterCacheKeyArray => new[] { "ReportsCache", "CENIT.APP.Cache" };

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<MajorReportModel> GetForUser(string forUser)
        {
            var rawKey = string.Concat($"ListReportsForUser-{forUser}");
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<MajorReportModel> lstReports) return lstReports;
            // Item not found in cache - retrieve it and insert it into the cache
            lstReports = Api.GetForUser(forUser);
            AddCacheItem(rawKey, lstReports);

            return lstReports;
        }

        public DataTable GetDataReport(string procedureName, params object[] p)
        {
            return Api.GetDataReport(procedureName, p);
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int? SavePermit(MajorReportModel model)
        {
            var ret = Api.SavePermit(model);
            if (ret > 0)
                // Invalidate the cache
                InvalidateCache();
            return ret;
        }
    }
}