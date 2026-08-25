using System.Collections.Generic;
using System.ComponentModel;
using Cores.Cate.Biz;
using Cores.Cate.Models;
using TSFramework.App.Models;
using TSFramework.Core.Members.Caching;
using TSFramework.Core.Utils;

namespace Cores.Cate.Caches
{
    public class CatePurPoseCache : CacheLayer
    {
        private CatePurPoseBiz _catePurPoseBiz;
        protected override string[] MasterCacheKeyArray => new[] { "CatePurPoseCache", "CENIT.APP.Cache" };

        private CatePurPoseBiz Api => _catePurPoseBiz ?? (_catePurPoseBiz = new CatePurPoseBiz());

        /// <summary>
        ///     Lấy toàn bộ thông tin loại hợp đồng theo bộ lọc
        /// </summary>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CatePurPoseModel> Get(out int total, string searchValue, string contractTypeIds,
            BaseSearchModel search = null)
        {
            var objectKey = EHashMD5.FromObject(search);
            var rawKey = string.Concat("ListPurposes-", objectKey, contractTypeIds, searchValue);
            var rawKeyTotal = string.Concat(rawKey, "-Total");
            var cacheTotal = (int?)GetCacheItem(rawKeyTotal);
            total = cacheTotal ?? 0;
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<CatePurPoseModel> lstPurposes) return lstPurposes;
            // Item not found in cache - retrieve it and insert it into the cache
            lstPurposes = Api.GetList(out total, contractTypeIds: contractTypeIds, searchValue: searchValue,
                search: search);
            if (lstPurposes == null) return null;
            AddCacheItem(rawKey, lstPurposes);
            AddCacheItem(rawKeyTotal, total);

            return lstPurposes;
        }

        /// <summary>
        ///     Get tất cả khách hàng
        /// </summary>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CatePurPoseModel> GetAll(string searchValue = null, string contractTypeIds = null)
        {
            var rawKey = $"AllPurPoses-{searchValue}-{contractTypeIds}";
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<CatePurPoseModel> lstPurposes) return lstPurposes;
            // Item not found in cache - retrieve it and insert it into the cache
            lstPurposes = Api.GetAll(searchValue, contractTypeIds);
            AddCacheItem(rawKey, lstPurposes);

            return lstPurposes;
        }

        /// <summary>
        ///     Lấy chi tiết theo ID
        /// </summary>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public CatePurPoseModel GetById(int purposeId)
        {
            if (purposeId < 0) return null;

            var rawKey = string.Concat("PurposeViaId-", purposeId);

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is CatePurPoseModel data) return data;
            // Item not found in cache - retrieve it and insert it into the cache
            data = Api.GetById(purposeId);
            AddCacheItem(rawKey, data);

            return data;
        }

        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public int? Save(CatePurPoseModel model, string savedBy)
        {
            var purpose = Api.Save(model, savedBy);
            // Invalidate the cache
            if (purpose > 0) InvalidateCache();
            return purpose;
        }

        /// <summary>
        ///     Xóa theo ID
        /// </summary>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public int Delete(CatePurPoseModel model)
        {
            var isDeleted = Api.Delete(model);
            if (isDeleted > 0)
                // Invalidate the cache
                InvalidateCache();
            return isDeleted;
        }
    }
}