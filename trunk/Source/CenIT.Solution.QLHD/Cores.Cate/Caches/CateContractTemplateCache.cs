using System.Collections.Generic;
using System.ComponentModel;
using Cores.Cate.Biz;
using Cores.Cate.Models;
using TSFramework.App.Models;
using TSFramework.Core.Members.Caching;
using TSFramework.Core.Utils;

namespace Cores.Cate.Caches
{
    public class CateContractTemplateCache : CacheLayer
    {
        private CateContractTemplateBiz _cateContractTemplateApi;
        protected override string[] MasterCacheKeyArray => new[] { "CateContractTemplateCache", "CENIT.APP.Cache" };

        private CateContractTemplateBiz Api =>
            _cateContractTemplateApi ?? (_cateContractTemplateApi = new CateContractTemplateBiz());

        /// <summary>
        ///     Lấy toàn bộ danh sách Cate_Contract_Template
        /// </summary>
        /// <returns>Kết quả thực hiện</returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CateContractTemplateModel> GetAll()
        {
            var rawKey = string.Concat("GetAllCate_Contract_Template");
            if (GetCacheItem(rawKey) is List<CateContractTemplateModel> data) return data;
            data = Api.GetAll();
            AddCacheItem(rawKey, data);
            return data;
        }

        /// <summary>
        ///     Lấy thông tinCate_Contract_Template theo ID
        /// </summary>
        /// <returns>Kết quả thực hiện</returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public CateContractTemplateModel GetById(string id)
        {
            if (id == null) return null;
            var rawKey = string.Concat("GetCate_Contract_TemplateByID_", id);
            if (GetCacheItem(rawKey) is CateContractTemplateModel data) return data;
            data = Api.LoadDetail(id);
            AddCacheItem(rawKey, data);
            return data;
        }

        /// <summary>
        ///     Lấy toàn bộ danh sách Cate_Contract_Template
        /// </summary>
        /// <returns>Kết quả thực hiện</returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CateContractTemplateModel> Get(out int total, BaseSearchModel search = null)
        {
            var rawKey = string.Concat("GetSearch_Cate_Contract_Template", EHashMD5.FromObject(search));
            var rawKeyTotal = string.Concat(rawKey, "-Total");
            var cacheTotal = (int?)GetCacheItem(rawKeyTotal);
            total = cacheTotal ?? 0;
            if (GetCacheItem(rawKey) is List<CateContractTemplateModel> data) return data;
            data = Api.LoadList(out total, search);
            AddCacheItem(rawKey, data);
            AddCacheItem(rawKeyTotal, total);
            return data;
        }

        /// <summary>
        ///     Xóa theo ID
        /// </summary>
        /// <returns>Kết quả thực hiện</returns>
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public int Delete(string id, string deletedBy)
        {
            var isDeleted = Api.Delete(id, deletedBy);
            if (isDeleted > 0) InvalidateCache();
            return isDeleted;
        }

        /// <summary>
        ///     Lưu thông tin Cate_Contract_Template
        /// </summary>
        /// <returns>Kết quả thực hiện</returns>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public int Save(CateContractTemplateModel model)
        {
            var isSaved = Api.Save(model);
            if (isSaved > 0) InvalidateCache();
            return isSaved;
        }
    }
}