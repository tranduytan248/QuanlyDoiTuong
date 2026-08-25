using System.Collections.Generic;
using System.ComponentModel;
using Cores.Cate.Biz;
using Cores.Cate.Models;
using TSFramework.App.Models;
using TSFramework.Core.Members.Caching;
using TSFramework.Core.Utils;

namespace Cores.Cate.Caches
{
    public class CatePositionCache : CacheLayer
    {
        private CatePositionBiz _positionApi;

        private CatePositionBiz Api => _positionApi ?? (_positionApi = new CatePositionBiz());

        protected override string[] MasterCacheKeyArray =>
            new[] { "PositionsCache", "CENIT.APP.Cache" };

        /// <summary>
        ///     Get danh sách tất cả chức vụ
        /// </summary>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CatePositionModel> GetAll()
        {
            var rawKey = "AllPositions-";
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<CatePositionModel> positions) return positions;
            // Item not found in cache - retrieve it and insert it into the cache
            positions = Api.Get(out _, null, null);
            AddCacheItem(rawKey, positions);

            return positions;
        }

        /// <summary>
        ///     Get danh sách chức vụ
        /// </summary>
        /// <param name="total"></param>
        /// <param name="key"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CatePositionModel> Get(out int total, string key, BaseSearchModel search = null)
        {
            var objectKey = EHashMD5.FromObject(search);
            var objectKey2 = EHashMD5.FromObject(key);
            var rawKey = string.Concat("ListPositions-", objectKey, objectKey2);
            var rawKeyTotal = string.Concat(rawKey, "-Total");
            var cacheTotal = (int?)GetCacheItem(rawKeyTotal);
            total = cacheTotal ?? 0;
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<CatePositionModel> positions) return positions;
            // Item not found in cache - retrieve it and insert it into the cache
            positions = Api.Get(out total, key, search);
            AddCacheItem(rawKey, positions);
            AddCacheItem(rawKeyTotal, total);
            return positions;
        }

        /// <summary>
        ///     Get chức vụ chi tiết bằng Id
        /// </summary>
        /// <param name="positionID"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public CatePositionModel GetById(int? positionID)
        {
            if (positionID < 0) return null;

            var rawKey = string.Concat("PositionByID-", positionID);

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is CatePositionModel position) return position;
            // Item not found in cache - retrieve it and insert it into the cache
            position = Api.GetById(positionID);
            if (position != null) AddCacheItem(rawKey, position);

            return position;
        }

        /// <summary>
        ///     Lưu thông tin chức vụ vào DB
        /// </summary>
        /// <param name="model"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int? Save(CatePositionModel model, string username)
        {
            var positionId = Api.Save(model, username);
            if (positionId > 0)
                // Invalidate the cache
                InvalidateCache();
            return positionId;
        }

        /// <summary>
        ///     Xóa chức vụ
        /// </summary>
        /// <param name="model"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Delete(CatePositionModel model, string username)
        {
            var isDeleted = Api.Delete(model, username);
            if (isDeleted)
                // Invalidate the cache
                InvalidateCache();
            return isDeleted;
        }
    }
}