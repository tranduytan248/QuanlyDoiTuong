using System.Collections.Generic;
using System.ComponentModel;
using Cores.Cate.Biz;
using Cores.Cate.Models;
using TSFramework.App.Models;
using TSFramework.Core.Members.Caching;
using TSFramework.Core.Utils;

namespace Cores.Cate.Caches
{
    public class CateSubSectionCache : CacheLayer
    {
        private CateSubSectionBiz _subSectionApi;
        protected override string[] MasterCacheKeyArray => new[] { "CateSubSectionCache", "CENIT.APP.Cache" };
        private CateSubSectionBiz Api => _subSectionApi ?? (_subSectionApi = new CateSubSectionBiz());

        /// <summary>
        ///     Lấy toàn bộ danh sách Cate_SubSection
        /// </summary>
        /// <returns>Kết quả thực hiện</returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CateSubSectionModel> Get(out int total, int mainSection, BaseSearchModel search = null)
        {
            var objectKey = EHashMD5.FromObject(search);
            var rawKey = string.Concat("GetSearch_Cate_SubSection", objectKey, mainSection);
            var rawKeyTotal = string.Concat(rawKey, "-Total");
            var cacheTotal = (int?)GetCacheItem(rawKeyTotal);
            total = cacheTotal ?? 0;
            if (GetCacheItem(rawKey) is List<CateSubSectionModel> data) return data;
            data = Api.LoadList(out total, mainSection, search);
            AddCacheItem(rawKey, data);
            AddCacheItem(rawKeyTotal, total);
            return data;
        }

        /// <summary>
        ///     Lấy thông tin Cate_SubSection theo ID
        /// </summary>
        /// <returns>Kết quả thực hiện</returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public CateSubSectionModel GetById(int? id)
        {
            if (id < 0) return null;
            var rawKey = string.Concat("GetCate_SubSectionByID_", id);
            if (GetCacheItem(rawKey) is CateSubSectionModel data) return data;
            data = Api.LoadDetail(id);
            AddCacheItem(rawKey, data);
            return data;
        }

        /// <summary>
        ///     Lấy toàn bộ danh sách Cate_SubSection
        /// </summary>
        /// <returns>Kết quả thực hiện</returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CateSubSectionModel> GetAll(int? mainSection)
        {
            var rawKey = $"GetAllCate_SubSection-{mainSection}";
            if (GetCacheItem(rawKey) is List<CateSubSectionModel> data) return data;
            data = Api.GetAll(mainSection);
            AddCacheItem(rawKey, data);
            return data;
        }

        /// <summary>
        ///     Xóa theo ID
        /// </summary>
        /// <returns>Kết quả thực hiện</returns>
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public int Delete(int id)
        {
            var isDeleted = Api.Delete(id);
            if (isDeleted > 0) InvalidateCache();
            return isDeleted;
        }

        /// <summary>
        ///     Lưu thông tin Cate_SubSection
        /// </summary>
        /// <returns>Kết quả thực hiện</returns>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public int Save(CateSubSectionModel model, string savedBy)
        {
            var isSaved = Api.Save(model, savedBy);
            if (isSaved > 0) InvalidateCache();
            return isSaved;
        }
    }
}