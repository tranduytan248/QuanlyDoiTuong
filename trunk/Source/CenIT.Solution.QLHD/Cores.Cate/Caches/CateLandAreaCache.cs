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
    public class CateLandAreaCache : CacheLayer
    {
        private CateLandAreaBiz _landAreaApi;
        protected override string[] MasterCacheKeyArray => new[] { "CateLandAreaCache", "CENIT.APP.Cache" };
        private CateLandAreaBiz Api => _landAreaApi ?? (_landAreaApi = new CateLandAreaBiz());

        /// <summary>
        ///     Lấy toàn bộ danh sách Cate_LandArea
        /// </summary>
        /// <returns>Kết quả thực hiện</returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CateLandAreaModel> Get(out int total, CateLandAreaSearchModel searchModel,
            BaseSearchModel search = null)
        {
            var objectKey = EHashMD5.FromObject(search);
            var objectKey2 = EHashMD5.FromObject(searchModel);
            var rawKey = string.Concat("GetSearch_Cate_LandArea", objectKey, objectKey2);
            var rawKeyTotal = string.Concat(rawKey, "-Total");
            var cacheTotal = (int?)GetCacheItem(rawKeyTotal);
            total = cacheTotal ?? 0;
            if (GetCacheItem(rawKey) is List<CateLandAreaModel> data) return data;
            data = Api.LoadList(out total, searchModel, search);
            AddCacheItem(rawKey, data);
            AddCacheItem(rawKeyTotal, total);
            return data;
        }

        /// <summary>
        ///     Lấy thông tin Cate_LandArea theo ID
        /// </summary>
        /// <returns>Kết quả thực hiện</returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public CateLandAreaModel GetById(int id)
        {
            if (id < 0) return null;
            var rawKey = string.Concat("GetCate_LandAreaByID_", id);
            if (GetCacheItem(rawKey) is CateLandAreaModel data) return data;
            data = Api.LoadDetail(id);
            AddCacheItem(rawKey, data);
            return data;
        }

        /// <summary>
        ///     Lấy toàn bộ danh sách Cate_LandArea
        /// </summary>
        /// <returns>Kết quả thực hiện</returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CateLandAreaModel> GetAll()
        {
            var rawKey = string.Concat("GetAllCate_LandArea");
            var data = GetCacheItem(rawKey) as List<CateLandAreaModel>;
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
        public int Delete(CateLandAreaModel model)
        {
            var isDeleted = Api.Delete(model.LandArea_ID);
            if (isDeleted > 0) InvalidateCache();
            return isDeleted;
        }

        /// <summary>
        ///     Lưu thông tin Cate_LandArea
        /// </summary>
        /// <returns>Kết quả thực hiện</returns>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public int Save(CateLandAreaModel model, string savedBy)
        {
            var isSaved = Api.Save(model, savedBy);
            if (isSaved > 0) InvalidateCache();
            return isSaved;
        }
    }
}