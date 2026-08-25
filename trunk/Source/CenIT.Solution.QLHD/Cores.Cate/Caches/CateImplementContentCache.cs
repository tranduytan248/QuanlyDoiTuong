using System.Collections.Generic;
using System.ComponentModel;
using Cores.Cate.Biz;
using Cores.Cate.Models;
using TSFramework.App.Models;
using TSFramework.Core.Members.Caching;
using TSFramework.Core.Utils;

namespace Cores.Cate.Caches
{
    public class CateImplementContentCache : CacheLayer
    {
        private CateImplementContentBiz _cateImplementContentBiz;
        protected override string[] MasterCacheKeyArray => new[] { "CateImplementContentCache", "CENIT.APP.Cache" };

        private CateImplementContentBiz Api =>
            _cateImplementContentBiz ?? (_cateImplementContentBiz = new CateImplementContentBiz());

        /// <summary>
        ///     Lấy toàn bộ thông tin nội dung thực hiên
        /// </summary>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CateImplementContentModel> Get(out int total, string tuKhoa, BaseSearchModel search = null)
        {
            var objectKey = EHashMD5.FromObject(search + tuKhoa);
            var rawKey = string.Concat("ListImplementContent-", objectKey);
            var rawKeyTotal = string.Concat(rawKey, "-Total");
            var cacheTotal = (int?)GetCacheItem(rawKeyTotal);
            total = cacheTotal ?? 0;
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<CateImplementContentModel> datas) return datas;
            // Item not found in cache - retrieve it and insert it into the cache
            datas = Api.GetList(out total, tuKhoa, search);
            if (datas == null) return null;
            AddCacheItem(rawKey, datas);
            AddCacheItem(rawKeyTotal, total);

            return datas;
        }

        /// <summary>
        ///     Lấy chi tiết theo ID
        /// </summary>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public CateImplementContentModel GetById(int implementContentID)
        {
            if (implementContentID < 0) return null;

            var rawKey = string.Concat("ImplementContent_GetByID-", implementContentID);

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is CateImplementContentModel data) return data;
            // Item not found in cache - retrieve it and insert it into the cache
            data = Api.GetById(implementContentID);
            AddCacheItem(rawKey, data);

            return data;
        }

        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public int? Save(CateImplementContentModel model, string savedBy)
        {
            var implementContent = Api.Save(model, savedBy);
            // Invalidate the cache
            if (implementContent > 0) InvalidateCache();
            return implementContent;
        }

        /// <summary>
        ///     Xóa theo ID
        /// </summary>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public int Delete(CateImplementContentModel model)
        {
            var isDeleted = Api.Delete(model);
            if (isDeleted > 0)
                // Invalidate the cache
                InvalidateCache();
            return isDeleted;
        }
    }
}