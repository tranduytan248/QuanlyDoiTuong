using System;
using System.Collections.Generic;
using System.ComponentModel;
using Cores.Cate.Models;
using Cores.Major.Biz;
using Cores.Major.Models;
using TSFramework.App.Models;
using TSFramework.Core.Members.Caching;
using TSFramework.Core.Utils;

namespace Cores.Major.Caches
{
    public class MajorSubjectViolationCache : CacheLayer
    {
        private MajorSubjectViolationBiz _violationApi;
        private MajorSubjectViolationBiz Api => _violationApi ?? (_violationApi = new MajorSubjectViolationBiz());

        protected override string[] MasterCacheKeyArray =>
            new[] { "SubjectViolationsCache", "CENIT.APP.Cache" };

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        /// <summary>
        /// Danh sách lịch sử vi phạm, đã áp dụng phân quyền dữ liệu.
        /// Khoá cache phải chứa userName - xem ghi chú tại MajorSubjectCache.Get.
        /// </summary>
        public List<MajorSubjectViolationModel> Get(out int total, string key, Guid? subjectId, int? fieldId,
            string userName, BaseSearchModel search = null)
        {
            var objectKey = EHashMD5.FromObject(search);
            var objectKey2 = EHashMD5.FromObject(key);
            var objectKey3 = EHashMD5.FromObject(subjectId);
            var objectKey4 = EHashMD5.FromObject(fieldId);
            var objectKey5 = EHashMD5.FromObject(userName);
            var rawKey = string.Concat("ListSubjectViolations-", objectKey, objectKey2, objectKey3, objectKey4, objectKey5);
            var rawKeyTotal = string.Concat(rawKey, "-Total");
            var cacheTotal = (int?)GetCacheItem(rawKeyTotal);
            total = cacheTotal ?? 0;
            if (GetCacheItem(rawKey) is List<MajorSubjectViolationModel> violations) return violations;
            violations = Api.Get(out total, key, subjectId, fieldId, userName, search);
            AddCacheItem(rawKey, violations);
            AddCacheItem(rawKeyTotal, total);
            return violations;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public MajorSubjectViolationModel GetById(Guid? violationId)
        {
            var rawKey = string.Concat("SubjectViolationDetail-", violationId);
            if (GetCacheItem(rawKey) is MajorSubjectViolationModel violation) return violation;
            violation = Api.GetById(violationId);
            AddCacheItem(rawKey, violation);
            return violation;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        /// <summary>
        /// Lịch sử vi phạm của một đối tượng, đã áp dụng phân quyền dữ liệu.
        /// <paramref name="userName"/> = null nghĩa là không giới hạn phạm vi.
        /// </summary>
        public List<MajorSubjectViolationModel> GetBySubjectId(Guid? subjectId, string userName = null)
        {
            var rawKey = string.Concat("SubjectViolationsBySubject-", subjectId, "-", EHashMD5.FromObject(userName));
            if (GetCacheItem(rawKey) is List<MajorSubjectViolationModel> violations) return violations;
            violations = Api.GetBySubjectId(subjectId, userName);
            AddCacheItem(rawKey, violations);
            return violations;
        }

        public List<CateViolationBehaviorModel> GetBehaviors(Guid? violationId)
        {
            return Api.GetBehaviors(violationId);
        }

        public string Save(MajorSubjectViolationModel model, string username)
        {
            var result = Api.Save(model, username);
            InvalidateCache();
            return result;
        }

        public bool Delete(Guid violationId, string username)
        {
            var result = Api.Delete(violationId, username);
            InvalidateCache();
            return result;
        }
    }
}
