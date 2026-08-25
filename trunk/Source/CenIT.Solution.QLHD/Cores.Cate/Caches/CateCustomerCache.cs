using Cores.Cate.Biz;
using Cores.Cate.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using TSFramework.App.Models;
using TSFramework.Core.Members.Caching;
using TSFramework.Core.Utils;

namespace Cores.Cate.Caches
{
    public class CateCustomerCache : CacheLayer
    {
        private CateCustomerBiz _customerApi;

        private CateCustomerBiz Api => _customerApi ?? (_customerApi = new CateCustomerBiz());

        protected override string[] MasterCacheKeyArray =>
            new[] { "CustomersCache", "CENIT.APP.Cache" };


        /// <summary>
        /// Get tất cả khách hàng
        /// </summary>
        /// <param name="cateTypes"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CateCustomerModel> GetAll(string cateTypes = null)
        {
            var rawKey = $"AllCustomers-{cateTypes}";
            // See if the item is in the cache
            var customers = GetCacheItem(rawKey) as List<CateCustomerModel>;
            if (customers != null) return customers;
            // Item not found in cache - retrieve it and insert it into the cache
            customers = Api.GetAll(cateTypes);
            AddCacheItem(rawKey, customers);

            return customers;
        }

        /// <summary>
        /// Get danh sách khách hàng theo search
        /// </summary>
        /// <param name="userType"></param>
        /// <param name="fullName">Tên</param>
        /// <param name="total"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CateCustomerModel> Get(string userType, string fullName,  out int total, BaseSearchModel search = null)
        {
            var objectKey = EHashMD5.FromObject(search);
            var rawKey = string.Concat($"ListCustomers-{fullName}-{userType}", objectKey);
            var rawKeyTotal = string.Concat(rawKey, "-Total");

            total = 0;
            var cacheTotal = (int?)GetCacheItem(rawKeyTotal);
            total = cacheTotal ?? 0;
            // See if the item is in the cache
            var customers = GetCacheItem(rawKey) as List<CateCustomerModel>;
            if (customers != null) return customers;
            // Item not found in cache - retrieve it and insert it into the cache
            customers = Api.Get(fullName, userType, out total, search);
            AddCacheItem(rawKey, customers);
            AddCacheItem(rawKeyTotal, total);

            return customers;
        }

        /// <summary>
        /// Lưu thông tin khách hàng
        /// </summary>
        /// <param name="model"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int? Save(CateCustomerModel model, string username)
        {
            var customerId = Api.Save(model, username);
            if (customerId > 0)
                // Invalidate the cache
                InvalidateCache();
            return customerId;
        }

        /// <summary>
        /// Get danh sách khách hàng theo id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public CateCustomerModel GetById(Guid? customerId)
        {
            if (customerId == null || customerId == Guid.Empty) return null;

            var rawKey = string.Concat("CustomerByID-", customerId);

            // See if the item is in the cache
            var customer = GetCacheItem(rawKey) as CateCustomerModel;
            if (customer != null) return customer;
            // Item not found in cache - retrieve it and insert it into the cache
            customer = Api.GetById(customerId);
            if (customer != null) AddCacheItem(rawKey, customer);

            return customer;
        }

        /// <summary>
        /// Xóa thông tin khách hàng 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Delete(CateCustomerModel model, string username)
        {
            var isDeleted = Api.Delete(model, username);
            if (isDeleted)
                // Invalidate the cache
                InvalidateCache();
            return isDeleted;
        }
    }
}
