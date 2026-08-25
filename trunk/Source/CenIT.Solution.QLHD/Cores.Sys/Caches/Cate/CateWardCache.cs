using System.Collections.Generic;
using System.ComponentModel;
using Cores.Sys.Biz.Cate;
using Cores.Sys.Models.Cate;
using TSFramework.App.Models;
using TSFramework.Core.Members.Caching;
using TSFramework.Core.Utils;

namespace Cores.Sys.Caches.Cate
{
    [DataObject]
    public class CateWardCache : CacheLayer
    {
        private CateWardBiz _wardApi;

        private CateWardBiz Api => _wardApi ?? (_wardApi = new CateWardBiz());

        protected override string[] MasterCacheKeyArray => new[] { "WardsCache", "ProvincesCache", "CENIT.APP.Cache" };

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CateWardModel> Get(string provinceIds, out int total, BaseSearchModel search = null)
        {
            var objectKey = EHashMD5.FromObject(search);
            var rawKey =
                $"ListWards-{provinceIds}-ViaSearch-{objectKey}";
            var rawKeyTotal = string.Concat(rawKey, "-Total");
            total = 0;
            var cacheTotal = (int?)GetCacheItem(rawKeyTotal);
            total = cacheTotal ?? 0;
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<CateWardModel> wards) return wards;
            // Item not found in cache - retrieve it and insert it into the cache
            wards = Api.LoadList(provinceIds, out total, search);
            AddCacheItem(rawKey, wards);
            AddCacheItem(rawKeyTotal, total);

            return wards;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public CateWardModel GetById(int? wardId)
        {
            if (wardId < 0) return null;

            var rawKey = string.Concat("WardByID-", wardId);

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is CateWardModel ward) return ward;
            // Item not found in cache - retrieve it and insert it into the cache
            ward = Api.GetById(wardId);
            if (ward != null) AddCacheItem(rawKey, ward);

            return ward;
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int? Save(CateWardModel model)
        {
            var wardId = Api.Save(model);
            if (wardId > 0)
                // Invalidate the cache
                InvalidateCache();
            return wardId;
        }

        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Delete(CateWardModel model)
        {
            var isDeleted = Api.Delete(model);
            if (isDeleted)
                // Invalidate the cache
                InvalidateCache();
            return isDeleted;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CateWardModel> GetAll(int? provinceId = null)
        {
            var rawKey = $"AllWard-{provinceId}";
            if (GetCacheItem(rawKey) is List<CateWardModel> listWards) return listWards;
            // Item not found in cache - retrieve it and insert it into the cache
            listWards = Api.GetAll(provinceId?.ToString());
            AddCacheItem(rawKey, listWards);

            return listWards;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CateWardModel> GetAll(int? provinceId, int? districtId)
        {
            var rawKey = $"AllWard-{provinceId}-{districtId}";
            if (GetCacheItem(rawKey) is List<CateWardModel> listWards) return listWards;
            listWards = Api.GetAll(provinceId?.ToString());
            AddCacheItem(rawKey, listWards);

            return listWards;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CateWardModel> GetByStreetId(int? streetId, out int total, BaseSearchModel search = null)
        {
            total = 0;
            var listWards = TSFramework.App.Processors.AppProcessor.ProcedureProvider.ExecuteTypedList<CateWardModel>("Cate_Ward_GetByStreetId", "MCSProvider", streetId);
            if (listWards != null) total = listWards.Count;
            return listWards ?? new List<CateWardModel>();
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CateWardModel> GetByProvinceId(int? provinceId, out int total, BaseSearchModel search = null)
        {
            var objectKey = EHashMD5.FromObject(search);

            var rawKey = string.Concat("AllWardsByProvince-", provinceId, objectKey);
            var rawKeyTotal = string.Concat(rawKey, "-Total");
            total = 0;
            var cacheTotal = (int?)GetCacheItem(rawKeyTotal);
            total = cacheTotal ?? 0;

            if (GetCacheItem(rawKey) is List<CateWardModel> listWards) return listWards;
            // Item not found in cache - retrieve it and insert it into the cache
            listWards = Api.GetByProvinceId(provinceId, out total, search);
            if (listWards == null) return null;
            AddCacheItem(rawKey, listWards);
            AddCacheItem(rawKeyTotal, total);

            return listWards;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CateWardModel> GetByProvinceCode(string provinceCode, out int total, BaseSearchModel search = null)
        {
            var objectKey = EHashMD5.FromObject(search);

            var rawKey = string.Concat("AllWardsByProvince-", provinceCode, objectKey);
            var rawKeyTotal = string.Concat(rawKey, "-Total");
            total = 0;
            var cacheTotal = (int?)GetCacheItem(rawKeyTotal);
            total = cacheTotal ?? 0;

            if (GetCacheItem(rawKey) is List<CateWardModel> listWards) return listWards;
            // Item not found in cache - retrieve it and insert it into the cache
            listWards = Api.GetByProvinceCode(provinceCode, out total, search);
            if (listWards == null) return null;
            AddCacheItem(rawKey, listWards);
            AddCacheItem(rawKeyTotal, total);

            return listWards;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CateWardModel> GetByProvinceId(int? provinceId)
        {
            var rawKey = string.Concat("AllWardsByProvince-", provinceId);

            if (GetCacheItem(rawKey) is List<CateWardModel> listWards) return listWards;
            // Item not found in cache - retrieve it and insert it into the cache
            listWards = Api.GetByProvinceId(provinceId);
            if (listWards == null) return null;
            AddCacheItem(rawKey, listWards);

            return listWards;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CateWardModel> GetByProvinceCode(string provinceCode)
        {
            var rawKey = string.Concat("AllWardsByProvince-", provinceCode);

            if (GetCacheItem(rawKey) is List<CateWardModel> listWards) return listWards;
            // Item not found in cache - retrieve it and insert it into the cache
            listWards = Api.GetByProvinceCode(provinceCode);
            if (listWards == null) return null;
            AddCacheItem(rawKey, listWards);

            return listWards;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CateWardModel> GetByDistrict(int? districtId)
        {
            if (districtId == null) return new List<CateWardModel>();
            var rawKey = string.Concat("WardsByDistrict-", districtId);
            if (GetCacheItem(rawKey) is List<CateWardModel> listWards) return listWards;
            listWards = Api.GetByDistrict(districtId);
            if (listWards == null) return new List<CateWardModel>();
            AddCacheItem(rawKey, listWards);
            return listWards;
        }
    }
}