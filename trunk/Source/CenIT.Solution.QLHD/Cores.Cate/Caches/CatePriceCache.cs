using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Cores.Cate.Biz;
using Cores.Cate.Models;
using TSFramework.App.Models;
using TSFramework.Core.Members.Caching;
using TSFramework.Core.Utils;

namespace Cores.Cate.Caches
{
    public class CatePriceCache : CacheLayer
    {
        private CatePriceBiz _priceApi;
        protected override string[] MasterCacheKeyArray => new[] { "CatePriceCache", "CENIT.APP.Cache" };
        private CatePriceBiz Api => _priceApi ?? (_priceApi = new CatePriceBiz());

        /// <summary>
        ///     Lấy toàn bộ danh sách Cate_Price
        /// </summary>
        /// <returns>Kết quả thực hiện</returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CatePriceModel> Get(out int total, CatePriceSearchModel searchModel, BaseSearchModel search = null)
        {
            var objectKey = EHashMD5.FromObject(search);
            var objectKey2 = EHashMD5.FromObject(searchModel);
            var rawKey = string.Concat("GetSearch_Cate_Price", objectKey, objectKey2);
            var rawKeyTotal = string.Concat(rawKey, "-Total");
            var cacheTotal = (int?)GetCacheItem(rawKeyTotal);
            total = cacheTotal ?? 0;
            if (GetCacheItem(rawKey) is List<CatePriceModel> data) return data;
            data = Api.LoadList(out total, searchModel, search);
            AddCacheItem(rawKey, data);
            AddCacheItem(rawKeyTotal, total);
            return data;
        }

        /// <summary>
        ///     Lấy thông tin Cate_Price theo ID
        /// </summary>
        /// <returns>Kết quả thực hiện</returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public CatePriceModel GetById(int id)
        {
            if (id < 0) return null;
            var rawKey = string.Concat("GetCate_PriceByID_", id);
            if (GetCacheItem(rawKey) is CatePriceModel data) return data;
            data = Api.LoadDetail(id);
            AddCacheItem(rawKey, data);
            return data;
        }

        /// <summary>
        ///     Lấy toàn bộ danh sách Cate_Price
        /// </summary>
        /// <returns>Kết quả thực hiện</returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CatePriceModel> GetAll()
        {
            var rawKey = string.Concat("GetAllCate_Price");
            var data = GetCacheItem(rawKey) as List<CatePriceModel>;
            if (data != null || data.Any()) return data;
            data = Api.GetAll();
            AddCacheItem(rawKey, data);
            return data;
        }

        /// <summary>
        ///     Xóa theo ID
        /// </summary>
        /// <returns>Kết quả thực hiện</returns>
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public int Delete(CatePriceModel model)
        {
            var isDeleted = Api.Delete(model.PriceId);
            if (isDeleted > 0) InvalidateCache();
            return isDeleted;
        }

        /// <summary>
        ///     Lưu thông tin Cate_Price
        /// </summary>
        /// <returns>Kết quả thực hiện</returns>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public int Save(CatePriceModel model, string savedBy)
        {
            var isSaved = Api.Save(model, savedBy);
            if (isSaved > 0) InvalidateCache();
            return isSaved;
        }
    }
}