using System.Collections.Generic;
using System.ComponentModel;
using Cores.Cate.Biz;
using Cores.Cate.Models;
using TSFramework.App.Models;
using TSFramework.Core.Members.Caching;
using TSFramework.Core.Utils;

namespace Cores.Cate.Caches
{
    public class CateContractTypeCache : CacheLayer
    {
        private CateContractTypeBiz _cateContractTypeBiz;
        protected override string[] MasterCacheKeyArray => new[] { "CateContractTypeCache", "CENIT.APP.Cache" };

        private CateContractTypeBiz Api => _cateContractTypeBiz ?? (_cateContractTypeBiz = new CateContractTypeBiz());

        /// <summary>
        ///     Lấy toàn bộ thông tin loại hợp đồng theo bộ lọc
        /// </summary>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CateContractTypeModel> Get(out int total, BaseSearchModel search = null)
        {
            var objectKey = EHashMD5.FromObject(search);
            var rawKey = string.Concat("ListContractTypes-", objectKey);
            var rawKeyTotal = string.Concat(rawKey, "-Total");
            var cacheTotal = (int?)GetCacheItem(rawKeyTotal);
            total = cacheTotal ?? 0;
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<CateContractTypeModel> datas) return datas;
            // Item not found in cache - retrieve it and insert it into the cache
            datas = Api.GetList(out total, search);
            if (datas == null) return null;
            AddCacheItem(rawKey, datas);
            AddCacheItem(rawKeyTotal, total);

            return datas;
        }

        /// <summary>
        ///     Get tất cả khách hàng
        /// </summary>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CateContractTypeModel> GetAll()
        {
            var rawKey = "AllContractTypes";
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<CateContractTypeModel> contractTypes) return contractTypes;
            // Item not found in cache - retrieve it and insert it into the cache
            contractTypes = Api.GetAll();
            AddCacheItem(rawKey, contractTypes);

            return contractTypes;
        }

        /// <summary>
        ///     Lấy chi tiết theo ID
        /// </summary>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public CateContractTypeModel GetById(int? contractTypeID)
        {
            if (contractTypeID < 0) return null;

            var rawKey = string.Concat("ContractType_GetByID-", contractTypeID);

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is CateContractTypeModel data) return data;
            // Item not found in cache - retrieve it and insert it into the cache
            data = Api.GetById(contractTypeID);
            AddCacheItem(rawKey, data);

            return data;
        }

        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public int? Save(CateContractTypeModel model)
        {
            var contractType = Api.Save(model);
            // Invalidate the cache
            if (contractType > 0) InvalidateCache();
            return contractType;
        }

        /// <summary>
        ///     Xóa theo ID
        /// </summary>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public int Delete(CateContractTypeModel model)
        {
            var isDeleted = Api.Delete(model);
            if (isDeleted > 0)
                // Invalidate the cache
                InvalidateCache();
            return isDeleted;
        }
    }
}