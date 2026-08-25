using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Cores.Major.Biz;
using Cores.Major.Models;
using TSFramework.App.Models;
using TSFramework.Core.Members.Caching;
using TSFramework.Core.Utils;

namespace Cores.Major.Caches
{
    public class MajorCustomerCache : CacheLayer
    {
        private MajorCustomerBiz _majorCustomersApi;
        protected override string[] MasterCacheKeyArray => new[] { "CustomersCache", "ContractsCache", "CENIT.APP.Cache" };

        private MajorCustomerBiz Api => _majorCustomersApi ?? (_majorCustomersApi = new MajorCustomerBiz());

        /// <summary>
        ///     Xóa theo ID
        /// </summary>
        /// <returns>Kết quả thực hiện</returns>
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public int Delete(MajorCustomerModel model, string deletedBy)
        {
            var isDeleted = Api.Delete(model.CusId, deletedBy);
            if (isDeleted > 0) InvalidateCache();
            return isDeleted;
        }

        /// <summary>
        ///     Lưu thông tin Major_Customers
        /// </summary>
        /// <returns>Kết quả thực hiện</returns>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public int Save(MajorCustomerModel model)
        {
            var isSaved = Api.Save(model);
            if (isSaved > 0) InvalidateCache();
            return isSaved;
        }

        /// <summary>
        ///     Lấy toàn bộ danh sách Major_Customers
        /// </summary>
        /// <returns>Kết quả thực hiện</returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<MajorCustomerModel> GetAll()
        {
            var rawKey = string.Concat("GetAllMajor_Customers");
            var data = GetCacheItem(rawKey) as List<MajorCustomerModel>;
            if (data != null || data.Any()) return data;
            data = Api.GetAll();
            AddCacheItem(rawKey, data);
            return data;
        }

        /// <summary>
        ///     Lấy thông tinMajor_Customers theo ID
        /// </summary>
        /// <returns>Kết quả thực hiện</returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public MajorCustomerModel GetById(Guid? id)
        {
            if (id == null) return null;
            var rawKey = string.Concat("GetMajor_CustomersByID_", id);
            if (GetCacheItem(rawKey) is MajorCustomerModel data) return data;
            data = Api.LoadDetail(id);
            AddCacheItem(rawKey, data);
            return data;
        }

        /// <summary>
        ///     Lấy toàn bộ danh sách Major_Customers
        /// </summary>
        /// <returns>Kết quả thực hiện</returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<MajorCustomerModel> Get(string keyword, string cusType, out int total,
            BaseSearchModel search = null)
        {
            var objectKey = EHashMD5.FromObject(search);
            var rawKey = string.Concat("GetSearch_Major_Customers", EHashMD5.FromObject(keyword + cusType + objectKey));
            var rawKeyTotal = string.Concat(rawKey, "-Total");
            total = 0;
            var cacheTotal = (int?)GetCacheItem(rawKeyTotal);
            total = cacheTotal ?? 0;
            if (GetCacheItem(rawKey) is List<MajorCustomerModel> data) return data;
            data = Api.LoadList(keyword, cusType, out total, search);
            AddCacheItem(rawKey, data);
            AddCacheItem(rawKeyTotal, total);
            return data;
        }
    }
}