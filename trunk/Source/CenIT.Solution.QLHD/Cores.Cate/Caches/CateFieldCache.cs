using System.Collections.Generic;
using System.ComponentModel;
using Cores.Cate.Biz;
using Cores.Cate.Models;
using TSFramework.App.Models;
using TSFramework.Core.Members.Caching;
using TSFramework.Core.Utils;

namespace Cores.Cate.Caches
{
    public class CateFieldCache : CacheLayer
    {
        private CateFieldBiz _fieldApi;
        private CateFieldBiz Api => _fieldApi ?? (_fieldApi = new CateFieldBiz());

        protected override string[] MasterCacheKeyArray =>
            new[] { "FieldsCache", "CENIT.APP.Cache" };

        public void InvalidateAll()
        {
            InvalidateCache();
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CateFieldModel> GetAll()
        {
            var rawKey = "AllFields-";
            if (GetCacheItem(rawKey) is List<CateFieldModel> fields) return fields;
            fields = Api.GetAll();
            AddCacheItem(rawKey, fields);
            return fields;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CateFieldModel> Get(out int total, string key, BaseSearchModel search = null)
        {
            var objectKey = EHashMD5.FromObject(search);
            var objectKey2 = EHashMD5.FromObject(key);
            var rawKey = string.Concat("ListFields-", objectKey, objectKey2);
            var rawKeyTotal = string.Concat(rawKey, "-Total");
            var cacheTotal = (int?)GetCacheItem(rawKeyTotal);
            total = cacheTotal ?? 0;
            if (GetCacheItem(rawKey) is List<CateFieldModel> fields) return fields;
            fields = Api.Get(out total, key, search);
            AddCacheItem(rawKey, fields);
            AddCacheItem(rawKeyTotal, total);
            return fields;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public CateFieldModel GetById(int? fieldId)
        {
            var rawKey = string.Concat("FieldDetail-", fieldId);
            if (GetCacheItem(rawKey) is CateFieldModel field) return field;
            field = Api.GetById(fieldId);
            AddCacheItem(rawKey, field);
            return field;
        }

        public int? Save(CateFieldModel model, string username)
        {
            var result = Api.Save(model, username);
            InvalidateCache();
            return result;
        }

        public bool Delete(CateFieldModel model, string username)
        {
            var result = Api.Delete(model, username);
            InvalidateCache();
            return result;
        }
    }
}
