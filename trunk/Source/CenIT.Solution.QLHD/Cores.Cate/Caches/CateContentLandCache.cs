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
    public class CateContentLandCache : CacheLayer
    {
        private CateContentLandBiz _contentLandApi;
        protected override string[] MasterCacheKeyArray => new[] { "CateContentLandCache", "CENIT.APP.Cache" };
        private CateContentLandBiz Api => _contentLandApi ?? (_contentLandApi = new CateContentLandBiz());

        /// <summary>
        ///     Lấy toàn bộ danh sách Cate_ContentLand
        /// </summary>
        /// <returns>Kết quả thực hiện</returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CateContentLandModel> Get(out int total, string typeContractIds, BaseSearchModel search = null)
        {
            var objectKey = EHashMD5.FromObject(search);
            var rawKey = $"ListContentLands-{objectKey}-{typeContractIds}";
            var rawKeyTotal = string.Concat(rawKey, "-Total");
            var cacheTotal = (int?)GetCacheItem(rawKeyTotal);
            total = cacheTotal ?? 0;
            if (GetCacheItem(rawKey) is List<CateContentLandModel> data) return data;
            data = Api.LoadList(out total, typeContractIds, search);
            AddCacheItem(rawKey, data);
            AddCacheItem(rawKeyTotal, total);
            return data;
        }

        /// <summary>
        ///     Lấy thông tin Cate_ContentLand theo ID
        /// </summary>
        /// <returns>Kết quả thực hiện</returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public CateContentLandModel GetById(Guid id)
        {
            var rawKey = string.Concat("ContentLandById_", id);
            if (GetCacheItem(rawKey) is CateContentLandModel data) return data;
            data = Api.LoadDetail(id);
            AddCacheItem(rawKey, data);
            return data;
        }

        /// <summary>
        ///     Lấy toàn bộ danh sách Cate_ContentLand
        /// </summary>
        /// <returns>Kết quả thực hiện</returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CateContentLandModel> GetAll(string typeContractIds = null)
        {
            var rawKey = $"AllContentLands-{typeContractIds}";
            if (GetCacheItem(rawKey) is List<CateContentLandModel> data) return data;
            data = Api.GetAll(typeContractIds);
            AddCacheItem(rawKey, data);
            return data;
        }

        /// <summary>
        ///     Xóa theo ID
        /// </summary>
        /// <returns>Kết quả thực hiện</returns>
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public int Delete(CateContentLandModel model)
        {
            var isDeleted = Api.Delete(model.ContentLandId);
            if (isDeleted > 0) InvalidateCache();
            return isDeleted;
        }

        /// <summary>
        ///     Lưu thông tin Cate_ContentLand
        /// </summary>
        /// <returns>Kết quả thực hiện</returns>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public int Save(CateContentLandModel model, string savedBy)
        {
            var isSaved = Api.Save(model, savedBy);
            if (isSaved > 0) InvalidateCache();
            return isSaved;
        }
    }
}