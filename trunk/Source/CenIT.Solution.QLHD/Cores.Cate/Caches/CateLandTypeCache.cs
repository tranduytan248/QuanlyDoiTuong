using System.Collections.Generic;
using System.ComponentModel;
using Cores.Cate.Biz;
using Cores.Cate.Models;
using TSFramework.App.Models;
using TSFramework.Core.Members.Caching;
using TSFramework.Core.Utils;

namespace Cores.Cate.Caches
{
    public class CateLandTypeCache : CacheLayer
    {
        private CateLandTypeBiz _landTypeApi;
        protected override string[] MasterCacheKeyArray => new[] { "CateLandTypeCache", "CENIT.APP.Cache" };
        private CateLandTypeBiz Api => _landTypeApi ?? (_landTypeApi = new CateLandTypeBiz());

        /// <summary>
        ///     Lấy toàn bộ danh sách Cate_LandType
        /// </summary>
        /// <returns>Kết quả thực hiện</returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CateLandTypeModel> Get(out int total, BaseSearchModel search = null)
        {
            var rawKey = string.Concat("GetSearch_Cate_LandType", EHashMD5.FromObject(search));
            var rawKeyTotal = string.Concat(rawKey, "-Total");
            var cacheTotal = (int?)GetCacheItem(rawKeyTotal);
            total = cacheTotal ?? 0;
            if (GetCacheItem(rawKey) is List<CateLandTypeModel> data) return data;
            data = Api.LoadList(out total, search);
            AddCacheItem(rawKey, data);
            AddCacheItem(rawKeyTotal, total);
            return data;
        }

        /// <summary>
        ///     Lấy thông tinCate_LandType theo ID
        /// </summary>
        /// <returns>Kết quả thực hiện</returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public CateLandTypeModel GetById(int id)
        {
            if (id < 0) return null;
            var rawKey = string.Concat("GetCate_LandTypeByID_", id);
            if (GetCacheItem(rawKey) is CateLandTypeModel data) return data;
            data = Api.LoadDetail(id);
            AddCacheItem(rawKey, data);
            return data;
        }

        /// <summary>
        ///     Lấy toàn bộ danh sách Cate_LandType
        /// </summary>
        /// <returns>Kết quả thực hiện</returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CateLandTypeModel> GetAll()
        {
            var rawKey = string.Concat("GetAllCate_LandType");
            if (GetCacheItem(rawKey) is List<CateLandTypeModel> data) return data;
            data = Api.GetAll();
            AddCacheItem(rawKey, data);
            return data;
        }

        /// <summary>
        ///     Xóa theo ID
        /// </summary>
        /// <returns>Kết quả thực hiện</returns>
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public int Delete(CateLandTypeModel model)
        {
            var isDeleted = Api.Delete(model.LandType_ID);
            if (isDeleted > 0) InvalidateCache();
            return isDeleted;
        }

        /// <summary>
        ///     Lưu thông tin Cate_LandType
        /// </summary>
        /// <returns>Kết quả thực hiện</returns>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public int Save(CateLandTypeModel model, string savedBy)
        {
            var isSaved = Api.Save(model, savedBy);
            if (isSaved > 0) InvalidateCache();
            return isSaved;
        }
    }
}