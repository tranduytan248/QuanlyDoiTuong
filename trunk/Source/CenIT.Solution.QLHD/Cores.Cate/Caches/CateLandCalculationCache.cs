using System;
using System.Collections.Generic;
using System.ComponentModel;
using Cores.Cate.Biz;
using Cores.Cate.Models;
using TSFramework.App.Models;
using TSFramework.Core.Members.Caching;
using TSFramework.Core.Utils;

namespace Cores.Cate.Caches
{
    public class CateLandCalculationCache : CacheLayer
    {
        private CateLandCalculationBiz _landCalculationApi;
        protected override string[] MasterCacheKeyArray => new[] { "CateLandCalculationCache", "CENIT.APP.Cache" };

        private CateLandCalculationBiz Api =>
            _landCalculationApi ?? (_landCalculationApi = new CateLandCalculationBiz());

        /// <summary>
        ///     Lấy toàn bộ danh sách Cate_landCalculation
        /// </summary>
        /// <returns>Kết quả thực hiện</returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CateLandCalculationModel> Get(out int total, string contentLands = null,
            BaseSearchModel search = null)
        {
            var objectKey = EHashMD5.FromObject(search);
            var rawKey = $"ListLandCalculation-{objectKey}-{contentLands}";
            var rawKeyTotal = string.Concat(rawKey, "-Total");
            var cacheTotal = (int?)GetCacheItem(rawKeyTotal);
            total = cacheTotal ?? 0;
            if (GetCacheItem(rawKey) is List<CateLandCalculationModel> data) return data;
            data = Api.LoadList(out total, contentLands, search);
            AddCacheItem(rawKey, data);
            AddCacheItem(rawKeyTotal, total);
            return data;
        }

        /// <summary>
        ///     Lấy thông tin Cate_landCalculation theo ID
        /// </summary>
        /// <returns>Kết quả thực hiện</returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public CateLandCalculationModel GetById(Guid? id)
        {
            var rawKey = string.Concat("GetCate_landCalculationByID_", id);
            if (GetCacheItem(rawKey) is CateLandCalculationModel data) return data;
            data = Api.LoadDetail(id);
            AddCacheItem(rawKey, data);
            return data;
        }

        /// <summary>
        ///     Lấy toàn bộ danh sách Cate_landCalculation
        /// </summary>
        /// <returns>Kết quả thực hiện</returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CateLandCalculationModel> GetAll(string contentLands = null)
        {
            var rawKey = $"AllLandCalculations-{contentLands}";
            if (GetCacheItem(rawKey) is List<CateLandCalculationModel> data) return data;
            data = Api.GetAll(contentLands);
            AddCacheItem(rawKey, data);
            return data;
        }

        /// <summary>
        ///     Xóa theo ID
        /// </summary>
        /// <returns>Kết quả thực hiện</returns>
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public int Delete(CateLandCalculationModel model)
        {
            var isDeleted = Api.Delete(model.LandCalculationId);
            if (isDeleted > 0) InvalidateCache();
            return isDeleted;
        }

        /// <summary>
        ///     Lưu thông tin Cate_landCalculation
        /// </summary>
        /// <returns>Kết quả thực hiện</returns>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public int Save(CateLandCalculationModel model, string savedBy)
        {
            var isSaved = Api.Save(model, savedBy);
            if (isSaved > 0) InvalidateCache();
            return isSaved;
        }
    }
}