using System.Collections.Generic;
using System.ComponentModel;
using Cores.Cate.Biz;
using Cores.Cate.Models;
using TSFramework.App.Models;
using TSFramework.Core.Members.Caching;
using TSFramework.Core.Utils;

namespace Cores.Cate.Caches
{
    public class CateContractStatusCache : CacheLayer
    {
        private CateContractStatusBiz _contractStatusApi;

        private CateContractStatusBiz Api => _contractStatusApi ?? (_contractStatusApi = new CateContractStatusBiz());

        protected override string[] MasterCacheKeyArray =>
            new[] { "ContractStatusCache", "CENIT.APP.Cache" };

        /// <summary>
        ///     Get danh sách tất cả trạng thái hợp đồng
        /// </summary>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CateContractStatusModel> GetAll()
        {
            var rawKey = "AllContractStatus-";
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<CateContractStatusModel> contractStatus) return contractStatus;
            // Item not found in cache - retrieve it and insert it into the cache
            contractStatus = Api.Get(out _, null);
            AddCacheItem(rawKey, contractStatus);

            return contractStatus;
        }

        /// <summary>
        ///     Get danh sách trạng thái hợp đồng
        /// </summary>
        /// <param name="total"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CateContractStatusModel> Get(out int total, BaseSearchModel search = null)
        {
            var objectKey = EHashMD5.FromObject(search);
            var rawKey = string.Concat("ListContractStatus-", objectKey);
            var rawKeyTotal = string.Concat(rawKey, "-Total");
            var cacheTotal = (int?)GetCacheItem(rawKeyTotal);
            total = cacheTotal ?? 0;
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<CateContractStatusModel> contractStatus) return contractStatus;
            // Item not found in cache - retrieve it and insert it into the cache
            contractStatus = Api.Get(out total, search);
            AddCacheItem(rawKey, contractStatus);
            AddCacheItem(rawKeyTotal, total);
            return contractStatus;
        }

        /// <summary>
        ///     Get trạng thái hợp đồng chi tiết bằng Id
        /// </summary>
        /// <param name="contractStatusID"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public CateContractStatusModel GetById(int? contractStatusID)
        {
            if (contractStatusID < 0) return null;

            var rawKey = string.Concat("ContractStatusByID-", contractStatusID);

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is CateContractStatusModel contractStatus) return contractStatus;
            // Item not found in cache - retrieve it and insert it into the cache
            contractStatus = Api.GetById(contractStatusID);
            if (contractStatus != null) AddCacheItem(rawKey, contractStatus);

            return contractStatus;
        }

        /// <summary>
        ///     Lưu thông tin trạng thái hợp đồng vào DB
        /// </summary>
        /// <param name="model"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int? Save(CateContractStatusModel model, string username)
        {
            var contractStatusId = Api.Save(model, username);
            if (contractStatusId > 0)
                // Invalidate the cache
                InvalidateCache();
            return contractStatusId;
        }

        /// <summary>
        ///     Xóa trạng thái hợp đồng
        /// </summary>
        /// <param name="model"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Delete(CateContractStatusModel model, string username)
        {
            var isDeleted = Api.Delete(model, username);
            if (isDeleted)
                // Invalidate the cache
                InvalidateCache();
            return isDeleted;
        }
    }
}