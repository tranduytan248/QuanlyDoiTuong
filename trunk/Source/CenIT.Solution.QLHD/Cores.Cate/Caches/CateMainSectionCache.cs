using System.Collections.Generic;
using System.ComponentModel;
using Cores.Cate.Biz;
using Cores.Cate.Models;
using TSFramework.App.Models;
using TSFramework.Core.Members.Caching;
using TSFramework.Core.Utils;

namespace Cores.Cate.Caches
{
    public class CateMainSectionCache : CacheLayer
    {
        private CateMainSectionBiz _mainSectionApi;
        protected override string[] MasterCacheKeyArray => new[] { "CateMainSectionCache", "CENIT.APP.Cache" };
        private CateMainSectionBiz Api => _mainSectionApi ?? (_mainSectionApi = new CateMainSectionBiz());

        /// <summary>
        ///     Lấy toàn bộ danh sách Cate_MainSection
        /// </summary>
        /// <returns>Kết quả thực hiện</returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CateMainSectionModel> Get(out int total, string typeContractIdsIds, BaseSearchModel search = null)
        {
            var objectKey = EHashMD5.FromObject(search);
            var rawKey = string.Concat("GetSearch_Cate_MainSection", objectKey, typeContractIdsIds);
            var rawKeyTotal = string.Concat(rawKey, "-Total");
            var cacheTotal = (int?)GetCacheItem(rawKeyTotal);
            total = cacheTotal ?? 0;
            if (GetCacheItem(rawKey) is List<CateMainSectionModel> lstMainSections) return lstMainSections;
            lstMainSections = Api.LoadList(out total, typeContractIdsIds, search);
            AddCacheItem(rawKey, lstMainSections);
            AddCacheItem(rawKeyTotal, total);
            return lstMainSections;
        }

        /// <summary>
        ///     Lấy thông tin Cate_MainSection theo ID
        /// </summary>
        /// <returns>Kết quả thực hiện</returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public CateMainSectionModel GetById(int id)
        {
            if (id < 0) return null;
            var rawKey = string.Concat("GetCate_MainSectionByID_", id);
            if (GetCacheItem(rawKey) is CateMainSectionModel mainSectionModel) return mainSectionModel;
            mainSectionModel = Api.LoadDetail(id);
            AddCacheItem(rawKey, mainSectionModel);
            return mainSectionModel;
        }

        /// <summary>
        ///     Lấy toàn bộ danh sách Cate_MainSection
        /// </summary>
        /// <returns>Kết quả thực hiện</returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CateMainSectionModel> GetAll(string typeContractIds = null)
        {
            var rawKey = $"GetAllCate_MainSection-{typeContractIds}";
            if (GetCacheItem(rawKey) is List<CateMainSectionModel> lstMainSections) return lstMainSections;
            lstMainSections = Api.GetAll(typeContractIds);
            AddCacheItem(rawKey, lstMainSections);
            return lstMainSections;
        }

        /// <summary>
        ///     Xóa theo ID
        /// </summary>
        /// <returns>Kết quả thực hiện</returns>
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public int Delete(CateMainSectionModel model)
        {
            var isDeleted = Api.Delete(model.MainSectionId);
            if (isDeleted > 0) InvalidateCache();
            return isDeleted;
        }

        /// <summary>
        ///     Lưu thông tin Cate_MainSection
        /// </summary>
        /// <returns>Kết quả thực hiện</returns>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public int Save(CateMainSectionModel model, string savedBy)
        {
            var isSaved = Api.Save(model, savedBy);
            if (isSaved > 0) InvalidateCache();
            return isSaved;
        }
    }
}