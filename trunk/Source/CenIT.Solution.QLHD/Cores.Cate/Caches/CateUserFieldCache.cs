using System.Collections.Generic;
using System.ComponentModel;
using Cores.Cate.Biz;
using Cores.Cate.Models;
using TSFramework.App.Models;
using TSFramework.Core.Members.Caching;
using TSFramework.Core.Utils;

namespace Cores.Cate.Caches
{
    /// <summary>
    /// Tầng cache cho phân quyền lĩnh vực của người dùng.
    /// </summary>
    public class CateUserFieldCache : CacheLayer
    {
        private CateUserFieldBiz _userFieldApi;
        private CateUserFieldBiz Api => _userFieldApi ?? (_userFieldApi = new CateUserFieldBiz());

        protected override string[] MasterCacheKeyArray =>
            new[] { "UserFieldsCache", "CENIT.APP.Cache" };

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CateUserFieldModel> Get(out int total, string key, BaseSearchModel search = null)
        {
            var objectKey = EHashMD5.FromObject(search);
            var objectKey2 = EHashMD5.FromObject(key);
            var rawKey = string.Concat("ListUserFields-", objectKey, objectKey2);
            var rawKeyTotal = string.Concat(rawKey, "-Total");
            var cacheTotal = (int?)GetCacheItem(rawKeyTotal);
            total = cacheTotal ?? 0;
            if (GetCacheItem(rawKey) is List<CateUserFieldModel> userFields) return userFields;
            userFields = Api.Get(out total, key, search);
            AddCacheItem(rawKey, userFields);
            AddCacheItem(rawKeyTotal, total);
            return userFields;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CateFieldModel> GetByUser(string userName)
        {
            var rawKey = string.Concat("UserFieldsByUser-", userName);
            if (GetCacheItem(rawKey) is List<CateFieldModel> fields) return fields;
            fields = Api.GetByUser(userName);
            AddCacheItem(rawKey, fields);
            return fields;
        }

        public bool Save(string userName, string fieldIds, string createdBy)
        {
            var result = Api.Save(userName, fieldIds, createdBy);
            InvalidateCache();
            return result;
        }

        /// <summary>
        /// Xoá toàn bộ cache phân quyền lĩnh vực.
        /// Gọi sau khi thay đổi phân quyền để dữ liệu hiển thị được cập nhật ngay.
        /// </summary>
        public void InvalidateAll()
        {
            InvalidateCache();
        }
    }
}
