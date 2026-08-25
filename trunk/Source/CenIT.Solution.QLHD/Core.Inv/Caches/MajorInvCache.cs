using System;
using System.Collections.Generic;
using System.ComponentModel;
using Core.Inv.Biz;
using Core.Inv.Models;
using Core.Inv.Models.Invs;
using TSFramework.App.Models;
using TSFramework.Core.Members.Caching;
using TSFramework.Core.Utils;

namespace Core.Inv.Caches
{
    [DataObject]
    public class MajorInvCache : CacheLayer
    {
        private MajorInvBiz _invApi;

        private MajorInvBiz Api => _invApi ?? (_invApi = new MajorInvBiz());

        protected override string[] MasterCacheKeyArray => new[] { "InvsCache", "ContractsCache", "CENIT.APP.Cache" };

        /// <summary>
        ///     Get danh sách trạng thái hợp đồng
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<MajorInvModel> Get(out int total, string userName, string managerUnions, string invNo = null,
            string pattern = null, string serials = null, string invStatus = null, string invTypes = null,
            DateTime? createdFrom = null, DateTime? createdTo = null, string creators = null, string cusName = null,
            string cusCode = null, string cusTaxCode = null, BaseSearchModel search = null)
        {
            var objectKey = EHashMD5.FromObject(search);
            var rawKey =
                $"ListInvoices-{objectKey}-{userName}-{managerUnions}-{invNo}-{pattern}-{serials}-{invStatus}-{invTypes}-{createdFrom}-{createdTo}-{creators}-{cusName}-{cusCode}-{cusTaxCode}";
            var rawKeyTotal = $"{rawKey}-Total";
            total = 0;
            var cacheTotal = (int?)GetCacheItem(rawKeyTotal);
            total = cacheTotal ?? 0;
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<MajorInvModel> invoices) return invoices;
            // Item not found in cache - retrieve it and insert it into the cache
            invoices = Api.Get(out total, userName, managerUnions, search: search, invNo: invNo, pattern: pattern,
                serials: serials, invStatus: invStatus, invTypes: invTypes, createdFrom: createdFrom,
                createdTo: createdTo, creators: creators, cusName: cusName, cusCode: cusCode, cusTaxCode: cusTaxCode);
            AddCacheItem(rawKey, invoices);
            AddCacheItem(rawKeyTotal, total);
            return invoices;
        }

        /// <summary>
        ///     Lấy danh sách hoá đơn chờ
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<MajorInvModel> GetPendingInvs(out int total, string userName, string managerUnions = null,
            string pattern = null, string serials = null, string invTypes = null, DateTime? createdFrom = null,
            DateTime? createdTo = null, string creators = null, string cusName = null, string cusCode = null,
            string cusTaxCode = null, BaseSearchModel search = null)
        {
            var objectKey = EHashMD5.FromObject(search);
            var rawKey =
                $"ListPendingInvoices-{objectKey}-{userName}-{managerUnions}-{pattern}-{serials}-{invTypes}-{createdFrom}-{createdTo}-{creators}-{cusName}-{cusCode}-{cusTaxCode}";
            var rawKeyTotal = $"{rawKey}-Total";
            total = 0;
            var cacheTotal = (int?)GetCacheItem(rawKeyTotal);
            total = cacheTotal ?? 0;
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<MajorInvModel> lstPendingInvs) return lstPendingInvs;
            // Item not found in cache - retrieve it and insert it into the cache
            lstPendingInvs = Api.GetPendingInvs(out total, userName, managerUnions, pattern, serials,
                createdFrom: createdFrom, createdTo: createdTo, creators: creators, cusName: cusName, cusCode: cusCode,
                cusTaxCode: cusTaxCode, search: search);
            AddCacheItem(rawKey, lstPendingInvs);
            AddCacheItem(rawKeyTotal, total);
            return lstPendingInvs;
        }

        [DataObjectMethod(DataObjectMethodType.Insert, false)]
        public int? Save(MajorInvModel model)
        {
            var invId = Api.Save(model);
            if (invId > 0)
                // Invalidate the cache
                InvalidateCache();
            return invId;
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int? Update(InvStatusModel model)
        {
            var invId = Api.Update(model);
            if (invId > 0)
                // Invalidate the cache
                InvalidateCache();
            return invId;
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int? Rollback(InvStatusModel model)
        {
            var invId = Api.Rollback(model);
            if (invId > 0)
                // Invalidate the cache
                InvalidateCache();
            return invId;
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int? Sync(InvStatusModel model)
        {
            var invId = Api.Sync(model);
            if (invId > 0)
                // Invalidate the cache
                InvalidateCache();
            return invId;
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int? Cancel(MajorInvModel model)
        {
            var invId = Api.Cancel(model);
            if (invId > 0)
                // Invalidate the cache
                InvalidateCache();
            return invId;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MajorInvModel GetById(Guid? invId)
        {
            if (invId == null) return null;

            var rawKey = string.Concat("InvByID-", invId);

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is MajorInvModel inv) return inv;
            // Item not found in cache - retrieve it and insert it into the cache
            inv = Api.GetById(invId);
            if (inv != null) AddCacheItem(rawKey, inv);

            return inv;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MajorInvModel GetByKey(string invKey)
        {
            if (string.IsNullOrEmpty(invKey)) return null;

            var rawKey = string.Concat("InvByKey-", invKey);

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is MajorInvModel inv) return inv;
            // Item not found in cache - retrieve it and insert it into the cache
            inv = Api.GetByKey(invKey);
            if (inv != null) AddCacheItem(rawKey, inv);

            return inv;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MarjorViewInvModel GetView(Guid? invId)
        {
            if (invId == null) return null;

            var rawKey = string.Concat("InvByID-", invId);

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is MarjorViewInvModel invView) return invView;
            // Item not found in cache - retrieve it and insert it into the cache
            invView = Api.GetView(invId);
            if (invView != null) AddCacheItem(rawKey, invView);

            return invView;
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int? SaveView(MarjorViewInvModel model)
        {
            var invId = Api.SaveView(model);
            if (invId > 0)
                // Invalidate the cache
                InvalidateCache();
            return invId;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MajorInvModel GetByContractId(Guid? contractId)
        {
            if (contractId == null) return null;

            var rawKey = string.Concat("InvByContractId-", contractId);

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is MajorInvModel inv) return inv;
            // Item not found in cache - retrieve it and insert it into the cache
            inv = Api.GetByContractId(contractId);
            if (inv != null) AddCacheItem(rawKey, inv);

            return inv;
        }

        [DataObjectMethod(DataObjectMethodType.Insert, false)]
        public int? AdjustInvoice(MajorInvModel model, string adjustedFkey, int statusNew, string statusNameNew,
            string invNo)
        {
            var invId = Api.AdjustInvoice(model, adjustedFkey, statusNew, statusNameNew, invNo);
            if (invId > 0)
                // Invalidate the cache
                InvalidateCache();
            return invId;
        }
    }
}