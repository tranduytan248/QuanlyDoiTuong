using System;
using System.Collections.Generic;
using System.ComponentModel;
using Cores.Cate.Biz;
using Cores.Cate.Models;
using TSFramework.App.Models;
using TSFramework.Core.Members.Caching;
using TSFramework.Core.Utils;

namespace Cores.Cate.Caches
{
    [DataObject]
    public class CateCategoryCache : CacheLayer
    {
        private CateCategoryBiz _categoryApi;

        private CateCategoryBiz Api => _categoryApi ?? (_categoryApi = new CateCategoryBiz());

        protected override string[] MasterCacheKeyArray =>
            new[] { "CategoriesCache", "CENIT.APP.Cache" };

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CateCategoryModel> GetAll(string cateTypes = null)
        {
            var rawKey = $"AllCategories-{cateTypes}";
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<CateCategoryModel> categories) return categories;
            // Item not found in cache - retrieve it and insert it into the cache
            categories = Api.GetAll(cateTypes);
            AddCacheItem(rawKey, categories);

            return categories;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CateCategoryModel> Get(string cateTypes, out int total, BaseSearchModel search = null)
        {
            var objectKey = EHashMD5.FromObject(search);
            var rawKey = string.Concat($"ListCategories-{cateTypes}", objectKey);
            var rawKeyTotal = string.Concat(rawKey, "-Total");
            var cacheTotal = (int?)GetCacheItem(rawKeyTotal);
            total = cacheTotal ?? 0;
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<CateCategoryModel> categories) return categories;
            // Item not found in cache - retrieve it and insert it into the cache
            categories = Api.Get(cateTypes, out total, search);
            AddCacheItem(rawKey, categories);
            AddCacheItem(rawKeyTotal, total);

            return categories;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public CateCategoryModel GetById(Guid? categoryId)
        {
            if (categoryId == null || categoryId == Guid.Empty) return null;

            var rawKey = string.Concat("CategoryByID-", categoryId);

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is CateCategoryModel category) return category;
            // Item not found in cache - retrieve it and insert it into the cache
            category = Api.GetById(categoryId);
            if (category != null) AddCacheItem(rawKey, category);

            return category;
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int? Save(CateCategoryModel model)
        {
            var categoryId = Api.Save(model);
            if (categoryId > 0)
                // Invalidate the cache
                InvalidateCache();
            return categoryId;
        }

        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Delete(CateCategoryModel model)
        {
            var isDeleted = Api.Delete(model);
            if (isDeleted)
                // Invalidate the cache
                InvalidateCache();
            return isDeleted;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public CateCategoryModel GetByCode(string cateCode)
        {
            if (string.IsNullOrEmpty(cateCode)) return null;

            var rawKey = string.Concat("CategoryByCode-", cateCode);

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is CateCategoryModel category) return category;
            // Item not found in cache - retrieve it and insert it into the cache
            category = Api.GetByCode(cateCode);
            if (category != null) AddCacheItem(rawKey, category);

            return category;
        }

        //get danh sách lĩnh vực vi phạm
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CateCategoryModel> GetViolationType()
        {
            var rawKey = string.Concat("CategoryViolationType");

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<CateCategoryModel> category) return category;
            // Item not found in cache - retrieve it and insert it into the cache
            category = Api.GetViolationType();
            if (category != null) AddCacheItem(rawKey, category);

            return category;
        }
    }
}