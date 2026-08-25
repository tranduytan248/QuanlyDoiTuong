using System;
using System.Collections.Generic;
using System.ComponentModel;
using Cores.Major.Biz;
using Cores.Major.Models;
using TSFramework.App.Models;
using TSFramework.Core.Members.Caching;
using TSFramework.Core.Utils;

namespace Cores.Major.Caches
{
    /// <summary>
    /// Tầng cache cho log cập nhật.
    /// </summary>
    public class MajorSubjectChangeLogCache : CacheLayer
    {
        private MajorSubjectChangeLogBiz _changeLogApi;
        private MajorSubjectChangeLogBiz Api => _changeLogApi ?? (_changeLogApi = new MajorSubjectChangeLogBiz());

        protected override string[] MasterCacheKeyArray =>
            new[] { "SubjectChangeLogsCache", "CENIT.APP.Cache" };

        /// <summary>
        /// Danh sách log, đã áp dụng phân quyền dữ liệu.
        /// Khoá cache BẮT BUỘC chứa userName vì kết quả khác nhau theo từng người dùng.
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<MajorSubjectChangeLogModel> Get(out int total, Guid? subjectId, string entityType,
            string userName, BaseSearchModel search = null)
        {
            var objectKey = EHashMD5.FromObject(search);
            var objectKey2 = EHashMD5.FromObject(subjectId);
            var objectKey3 = EHashMD5.FromObject(entityType);
            var objectKey4 = EHashMD5.FromObject(userName);
            var rawKey = string.Concat("ListSubjectChangeLogs-", objectKey, objectKey2, objectKey3, objectKey4);
            var rawKeyTotal = string.Concat(rawKey, "-Total");
            var cacheTotal = (int?)GetCacheItem(rawKeyTotal);
            total = cacheTotal ?? 0;
            if (GetCacheItem(rawKey) is List<MajorSubjectChangeLogModel> logs) return logs;
            logs = Api.Get(out total, subjectId, entityType, userName, search);
            AddCacheItem(rawKey, logs);
            AddCacheItem(rawKeyTotal, total);
            return logs;
        }

        public string Save(MajorSubjectChangeLogModel model)
        {
            var result = Api.Save(model);
            InvalidateCache();
            return result;
        }
    }
}
