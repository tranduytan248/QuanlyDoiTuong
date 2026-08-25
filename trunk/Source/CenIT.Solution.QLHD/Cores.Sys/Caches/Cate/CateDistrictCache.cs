using Cores.Sys.Biz.Cate;
using Cores.Sys.Models.Cate;
using System.Collections.Generic;
using System.ComponentModel;
using TSFramework.App.Models;
using TSFramework.Core.Members.Caching;
using TSFramework.Core.Utils;

namespace Cores.Sys.Caches.Cate
{
    [DataObject]
    public class CateDistrictCache : CacheLayer
    {
        private CateDistrictBiz _districtApi;

        private CateDistrictBiz Api => _districtApi ?? (_districtApi = new CateDistrictBiz());

        protected override string[] MasterCacheKeyArray =>
            new[] { "DistrictsCache", "ProvincesCache", "CENIT.APP.Cache" };

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CateDistrictModel> GetAll(string provinceIds = null)
        {
            var rawKey = $"AllDistricts-{provinceIds}-";
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<CateDistrictModel> districts) return districts;
            // Item not found in cache - retrieve it and insert it into the cache
            districts = Api.GetAll(provinceIds);
            AddCacheItem(rawKey, districts);

            return districts;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CateDistrictModel> Get(string provincesIds, out int total,
            BaseSearchModel search = null)
        {
            var objectKey = EHashMD5.FromObject(search);
            var rawKey = string.Concat($"ListDistricts-{provincesIds}-", objectKey);
            var rawKeyTotal = string.Concat(rawKey, "-Total");
            total = 0;
            var cacheTotal = (int?)GetCacheItem(rawKeyTotal);
            total = cacheTotal ?? 0;
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<CateDistrictModel> districts) return districts;
            // Item not found in cache - retrieve it and insert it into the cache
            districts = Api.Get(provincesIds, out total, search);
            AddCacheItem(rawKey, districts);
            AddCacheItem(rawKeyTotal, total);
            return districts;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public CateDistrictModel GetById(int? districtId)
        {
            if (districtId < 0) return null;

            var rawKey = $"DistrictByID-{districtId}";

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is CateDistrictModel district) return district;
            // Item not found in cache - retrieve it and insert it into the cache
            district = Api.GetById(districtId);
            if (district != null) AddCacheItem(rawKey, district);

            return district;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public CateDistrictModel GetByCode(string districtCode)
        {
            if (string.IsNullOrEmpty(districtCode)) return null;

            var rawKey = $"DistrictByCode-{districtCode}";

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is CateDistrictModel district) return district;
            // Item not found in cache - retrieve it and insert it into the cache
            district = Api.GetByCode(districtCode);
            if (district != null) AddCacheItem(rawKey, district);

            return district;
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int? Save(CateDistrictModel model)
        {
            var districtId = Api.Save(model);
            if (districtId > 0)
                // Invalidate the cache
                InvalidateCache();
            return districtId;
        }

        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Delete(CateDistrictModel model)
        {
            var isDeleted = Api.Delete(model);
            if (isDeleted)
                // Invalidate the cache
                InvalidateCache();
            return isDeleted;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CateDistrictModel> GetByProvinceCode(string provinceCode, BaseSearchModel search = null)
        {
            var objectKey = EHashMD5.FromObject(search);
            if (string.IsNullOrEmpty(provinceCode)) return null;
            var rawKey = $"LisDistrictsByProvinceCode-{provinceCode}-{objectKey}";

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<CateDistrictModel> districts) return districts;
            // Item not found in cache - retrieve it and insert it into the cache
            districts = Api.GetByProvinceCode(provinceCode, search);
            AddCacheItem(rawKey, districts);

            return districts;
        }

        public List<CateDistrictModel> GetViaProvinceId(int? provinceId, out int total, BaseSearchModel search = null)
        {
            var objectKey = EHashMD5.FromObject(search);
            var rawKey = $"ListByDistrictID-{provinceId}-ViaSearch-{objectKey}";
            var rawKeyTotal = string.Concat(rawKey, "-Total");
            total = 0;
            var cacheTotal = (int?)GetCacheItem(rawKeyTotal);
            total = cacheTotal ?? 0;
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<CateDistrictModel> districts) return districts;
            // Item not found in cache - retrieve it and insert it into the cache
            districts = Api.GetByProvinceId(provinceId, out total, search);
            AddCacheItem(rawKey, districts ?? new List<CateDistrictModel>());
            AddCacheItem(rawKeyTotal, total);

            return districts;
        }
    }
}