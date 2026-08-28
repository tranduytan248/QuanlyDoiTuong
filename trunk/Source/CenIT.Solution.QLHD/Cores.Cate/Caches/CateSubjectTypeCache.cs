using System.Collections.Generic;
using System.ComponentModel;
using Cores.Cate.Biz;
using Cores.Cate.Models;
using TSFramework.App.Models;
using TSFramework.Core.Members.Caching;
using TSFramework.Core.Utils;

namespace Cores.Cate.Caches
{
    public class CateSubjectTypeCache : CacheLayer
    {
        private CateSubjectTypeBiz _subjectTypeApi;
        private CateSubjectTypeBiz Api => _subjectTypeApi ?? (_subjectTypeApi = new CateSubjectTypeBiz());

        protected override string[] MasterCacheKeyArray =>
            new[] { "SubjectTypesCache", "CENIT.APP.Cache" };

        public void InvalidateAll()
        {
            InvalidateCache();
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CateSubjectTypeModel> GetAll()
        {
            var rawKey = "AllSubjectTypes-";
            if (GetCacheItem(rawKey) is List<CateSubjectTypeModel> list) return list;
            list = Api.GetAll();
            AddCacheItem(rawKey, list);
            return list;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CateSubjectTypeModel> Get(out int total, string key, BaseSearchModel search = null)
        {
            var objectKey = EHashMD5.FromObject(search);
            var objectKey2 = EHashMD5.FromObject(key);
            var rawKey = string.Concat("ListSubjectTypes-", objectKey, objectKey2);
            var rawKeyTotal = string.Concat(rawKey, "-Total");
            var cacheTotal = (int?)GetCacheItem(rawKeyTotal);
            total = cacheTotal ?? 0;
            if (GetCacheItem(rawKey) is List<CateSubjectTypeModel> list) return list;
            list = Api.Get(out total, key, search);
            AddCacheItem(rawKey, list);
            AddCacheItem(rawKeyTotal, total);
            return list;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public CateSubjectTypeModel GetById(int? subjectTypeId)
        {
            var rawKey = string.Concat("SubjectTypeDetail-", subjectTypeId);
            if (GetCacheItem(rawKey) is CateSubjectTypeModel item) return item;
            item = Api.GetById(subjectTypeId);
            AddCacheItem(rawKey, item);
            return item;
        }

        public int? Save(CateSubjectTypeModel model, string username)
        {
            var ret = Api.Save(model, username);
            if (ret > 0) InvalidateCache();
            return ret;
        }

        public int? Delete(int subjectTypeId, string username)
        {
            var ret = Api.Delete(subjectTypeId, username);
            if (ret > 0) InvalidateCache();
            return ret;
        }

        public int? ToggleStatus(int subjectTypeId, string username)
        {
            var ret = Api.ToggleStatus(subjectTypeId, username);
            if (ret > 0) InvalidateCache();
            return ret;
        }
    }
}
