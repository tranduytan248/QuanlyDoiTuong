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
    public class CateStreetCache : CacheLayer
    {
        private CateStreetBiz _streetApi;

        private CateStreetBiz Api => _streetApi ?? (_streetApi = new CateStreetBiz());

        protected override string[] MasterCacheKeyArray => new[] { "StreetsCache", "WardsCache", "CENIT.APP.Cache" };

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CateStreetModel> Get(out int total, string provinceIds, string districtIds, string wardIds,
            BaseSearchModel search = null)
        {
            var objectKey = EHashMD5.FromObject(search);
            var rawKey = $"ListStreets-{provinceIds}-{districtIds}-{wardIds}-ViaSearch-{objectKey}";
            var rawKeyTotal = string.Concat(rawKey, "-Total");
            total = 0;
            var cacheTotal = (int?)GetCacheItem(rawKeyTotal);
            total = cacheTotal ?? 0;
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<CateStreetModel> streets) return streets;
            // Item not found in cache - retrieve it and insert it into the cache
            streets = Api.LoadList(out total, provinceIds, districtIds, wardIds, search);
            AddCacheItem(rawKey, streets);
            AddCacheItem(rawKeyTotal, total);

            return streets;
        }

        public List<CateStreetModel> GetByWard(int idWard, out int total, BaseSearchModel search = null)
        {
            var objectKey = EHashMD5.FromObject(search);
            var rawKey = $"ListStreetsByWard-{idWard}-ViaSearch-{objectKey}";
            var rawKeyTotal = string.Concat(rawKey, "-Total");
            total = 0;
            var cacheTotal = (int?)GetCacheItem(rawKeyTotal);
            total = cacheTotal ?? 0;
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<CateStreetModel> streets) return streets;
            // Item not found in cache - retrieve it and insert it into the cache
            streets = Api.GetByWard(idWard, out total, search);
            AddCacheItem(rawKey, streets ?? new List<CateStreetModel>());
            AddCacheItem(rawKeyTotal, total);

            return streets;
        }

        public List<CateStreetModel> GetByWard(int? idWard)
        {
            var rawKey = $"ListStreetsByWard-{idWard}";
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<CateStreetModel> streets) return streets;
            // Item not found in cache - retrieve it and insert it into the cache
            streets = Api.GetByWard(idWard);
            AddCacheItem(rawKey, streets ?? new List<CateStreetModel>());

            return streets;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public CateStreetModel GetById(int? streetId)
        {
            if (streetId < 0) return null;

            var rawKey = $"StreetByID-{streetId}";

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is CateStreetModel street) return street;
            // Item not found in cache - retrieve it and insert it into the cache
            street = Api.GetById(streetId);
            if (street != null) AddCacheItem(rawKey, street);

            return street;
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int? Save(CateStreetModel model)
        {
            var streetId = Api.Save(model);
            if (streetId > 0)
                // Invalidate the cache
                InvalidateCache();
            return streetId;
        }

        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Delete(CateStreetModel model)
        {
            var isDeleted = Api.Delete(model);
            if (isDeleted)
                // Invalidate the cache
                InvalidateCache();
            return isDeleted;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CateStreetModel> GetAll(string provinceIds, string districtIds, string wardIds)
        {
            var rawKey = $"AllStreets-{provinceIds}-{districtIds}-{wardIds}";
            if (GetCacheItem(rawKey) is List<CateStreetModel> streets) return streets;
            // Item not found in cache - retrieve it and insert it into the cache
            streets = Api.GetAll(provinceIds, districtIds, wardIds);
            AddCacheItem(rawKey, streets);

            return streets;
        }
    }
}