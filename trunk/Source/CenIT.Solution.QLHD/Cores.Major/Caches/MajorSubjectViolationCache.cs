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
        public List<MajorSubjectViolationModel> Get(out int total, string key, Guid? subjectId, int? fieldId, BaseSearchModel search = null)
        {
            var objectKey = EHashMD5.FromObject(search);
            var objectKey2 = EHashMD5.FromObject(key);
            var objectKey3 = EHashMD5.FromObject(subjectId);
            var objectKey4 = EHashMD5.FromObject(fieldId);
            var rawKey = string.Concat("ListSubjectViolations-", objectKey, objectKey2, objectKey3, objectKey4);
            var rawKeyTotal = string.Concat(rawKey, "-Total");
            var cacheTotal = (int?)GetCacheItem(rawKeyTotal);
            total = cacheTotal ?? 0;
            if (GetCacheItem(rawKey) is List<MajorSubjectViolationModel> violations) return violations;
            violations = Api.Get(out total, key, subjectId, fieldId, search);
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
        public List<MajorSubjectViolationModel> GetBySubjectId(Guid? subjectId)
        {
            var rawKey = string.Concat("SubjectViolationsBySubject-", subjectId);
            if (GetCacheItem(rawKey) is List<MajorSubjectViolationModel> violations) return violations;
            violations = Api.GetBySubjectId(subjectId);
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
