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
    public class MajorSubjectCache : CacheLayer
    {
        private MajorSubjectBiz _subjectApi;
        private MajorSubjectBiz Api => _subjectApi ?? (_subjectApi = new MajorSubjectBiz());

        protected override string[] MasterCacheKeyArray =>
            new[] { "SubjectsCache", "CENIT.APP.Cache" };

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<MajorSubjectModel> GetAll()
        {
            var rawKey = "AllSubjects-";
            if (GetCacheItem(rawKey) is List<MajorSubjectModel> subjects) return subjects;
            subjects = Api.GetAll();
            AddCacheItem(rawKey, subjects);
            return subjects;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<MajorSubjectModel> Get(out int total, string key, string gender, BaseSearchModel search = null)
        {
            var objectKey = EHashMD5.FromObject(search);
            var objectKey2 = EHashMD5.FromObject(key);
            var objectKey3 = EHashMD5.FromObject(gender);
            var rawKey = string.Concat("ListSubjects-", objectKey, objectKey2, objectKey3);
            var rawKeyTotal = string.Concat(rawKey, "-Total");
            var cacheTotal = (int?)GetCacheItem(rawKeyTotal);
            total = cacheTotal ?? 0;
            if (GetCacheItem(rawKey) is List<MajorSubjectModel> subjects) return subjects;
            subjects = Api.Get(out total, key, gender, search);
            AddCacheItem(rawKey, subjects);
            AddCacheItem(rawKeyTotal, total);
            return subjects;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public MajorSubjectModel GetById(Guid? subjectId)
        {
            var rawKey = string.Concat("SubjectDetail-", subjectId);
            if (GetCacheItem(rawKey) is MajorSubjectModel subject) return subject;
            subject = Api.GetById(subjectId);
            AddCacheItem(rawKey, subject);
            return subject;
        }

        public string Save(MajorSubjectModel model, string username)
        {
            var result = Api.Save(model, username);
            InvalidateCache();
            return result;
        }

        public bool Delete(Guid subjectId, string username)
        {
            var result = Api.Delete(subjectId, username);
            InvalidateCache();
            return result;
        }
    }
}
