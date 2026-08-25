using System;
using System.Collections.Generic;
using System.ComponentModel;
using Cores.Major.Biz;
using Cores.Major.Models;
using TSFramework.Core.Members.Caching;

namespace Cores.Major.Caches
{
    public class MajorContractPaymentCache : CacheLayer
    {
        private MajorContractPaymentBiz _contractApi;
        private MajorContractPaymentBiz Api => _contractApi ?? (_contractApi = new MajorContractPaymentBiz());

        protected override string[] MasterCacheKeyArray => new[] { "ContractsCache", "CENIT.APP.Cache" };

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<MajorContractPaymentModel> GetPayments(Guid? contractId)
        {
            if (contractId == null) return null;

            var rawKey = string.Concat("ContractPaymentsByID-", contractId);
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<MajorContractPaymentModel> contractPayments) return contractPayments;
            // Item not found in cache - retrieve it and insert it into the cache
            contractPayments = Api.GetPayments(contractId);
            AddCacheItem(rawKey, contractPayments);

            return contractPayments;
        }

        #region Payments

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MajorContractPaymentModel GetPaymentById(Guid? paymentId)
        {
            if (paymentId == null) return null;

            var rawKey = string.Concat("ContractPaymentByID-", paymentId);

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is MajorContractPaymentModel payment) return payment;
            // Item not found in cache - retrieve it and insert it into the cache
            payment = Api.GetPaymentById(paymentId);
            if (payment != null) AddCacheItem(rawKey, payment);

            return payment;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MajorContractPaymentModel CheckTypePayment(Guid? contractId, int typePayment)
        {
            if (contractId == null) return null;

            var rawKey = string.Concat("CheckTypePayment-", contractId, typePayment);

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is MajorContractPaymentModel payment) return payment;
            // Item not found in cache - retrieve it and insert it into the cache
            payment = Api.CheckTypePayment(contractId, typePayment);
            if (payment != null) AddCacheItem(rawKey, payment);

            return payment;
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int? SavePayment(MajorContractPaymentModel model)
        {
            var paymentId = Api.SavePayment(model);
            if (paymentId > 0)
                // Invalidate the cache
                InvalidateCache();
            return paymentId;
        }

        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public int? DeletePayment(MajorContractPaymentModel model)
        {
            var ret = Api.DeletePayment(model);
            if (ret > 0)
                // Invalidate the cache
                InvalidateCache();
            return ret;
        }

        #endregion
    }
}