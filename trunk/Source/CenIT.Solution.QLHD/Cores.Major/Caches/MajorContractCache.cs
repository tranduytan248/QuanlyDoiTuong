using System;
using System.Collections.Generic;
using System.ComponentModel;
using Cores.Major.Biz;
using Cores.Major.Models;
using TSFramework.App.Models;
using TSFramework.Core.Members.Caching;

namespace Cores.Major.Caches
{
    [DataObject]
    public class MajorContractCache : CacheLayer
    {
        private MajorContractBiz _contractApi;

        private MajorContractBiz Api => _contractApi ?? (_contractApi = new MajorContractBiz());

        protected override string[] MasterCacheKeyArray => new[] { "ContractsCache", "InvsCache", "CustomersCache", "CENIT.APP.Cache" };

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

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<MajorContractTaskModel> GetTask(Guid? contractId)
        {
            if (contractId == null) return null;

            var rawKey = string.Concat("ContractTasksByID-", contractId);
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<MajorContractTaskModel> contractTasks) return contractTasks;
            // Item not found in cache - retrieve it and insert it into the cache
            contractTasks = Api.GetTasks(contractId);
            AddCacheItem(rawKey, contractTasks);

            return contractTasks;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MajorContractCustomerModel GetCus(Guid? contractId)
        {
            if (contractId == null) return null;

            var rawKey = string.Concat("ContractCustomerByID-", contractId);

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is MajorContractCustomerModel contractCus) return contractCus;
            // Item not found in cache - retrieve it and insert it into the cache
            contractCus = Api.GetCus(contractId);
            if (contractCus != null) AddCacheItem(rawKey, contractCus);

            return contractCus;
        }

        #region Reject - từ chối hợp đồng

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int? Reject(MajorContractRejectModel rejectModel)
        {
            var retUpdate = Api.Reject(rejectModel);
            if (retUpdate > 0)
                // Invalidate the cache
                InvalidateCache();
            return retUpdate;
        }

        #endregion

        #region Contract Actions

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<MajorContractModel> GetAll(string managerUnions = null, string searchValue = null,
            DateTime? receivedFromDate = null, DateTime? receivedToDate = null, DateTime? giveResultFromDate = null,
            DateTime? giveResultToDate = null, string contractStatus = null, string typeContractIds = null,
            string typeCusIds = null)
        {
            var rawKey =
                $"AllContracts-{managerUnions}-{searchValue}-{receivedFromDate}-{receivedToDate}-{giveResultFromDate}-{giveResultToDate}-{contractStatus}-{typeContractIds}-{typeCusIds}";
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<MajorContractModel> contracts) return contracts;
            // Item not found in cache - retrieve it and insert it into the cache
            contracts = Api.GetAll(managerUnions, searchValue, receivedFromDate, receivedToDate, giveResultFromDate,
                giveResultToDate, contractStatus, typeContractIds, typeCusIds);
            AddCacheItem(rawKey, contracts);

            return contracts;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<MajorContractModel> Get(out int total, string managerUnions = null, string searchValue = null,
            DateTime? receivedFromDate = null, DateTime? receivedToDate = null, DateTime? giveResultFromDate = null,
            DateTime? giveResultToDate = null, string contractStatus = null, string typeContractIds = null,
            string typeCusIds = null, string username = null, BaseSearchModel search = null)
        {
            #region Cache

            //var objectKey = EHashMD5.FromObject(search);
            //var rawKey = $"ListContracts-{objectKey}-{managerUnions}-{searchValue}-{fromDate}-{toDate}-{contractStatus}-{typeContractIds}-{typeCusIds}-{username}";
            //var rawKeyTotal = string.Concat(rawKey, "-Total");

            //total = 0;
            //var cacheTotal = (int?)GetCacheItem(rawKeyTotal);
            //total = cacheTotal ?? 0;
            //// See if the item is in the cache
            //if (GetCacheItem(rawKey) is List<MajorContractModel> contracts) return contracts;
            //// Item not found in cache - retrieve it and insert it into the cache
            //contracts = Api.Get(out total, managerUnions, searchValue, fromDate, toDate, contractStatus, typeContractIds, typeCusIds, username, search);
            //AddCacheItem(rawKey, contracts);
            //AddCacheItem(rawKeyTotal, total);

            #endregion

            #region Not Cache

            var contracts = Api.Get(out total, managerUnions, searchValue, receivedFromDate, receivedToDate,
                giveResultFromDate, giveResultToDate, contractStatus, typeContractIds, typeCusIds, username, search);

            #endregion

            return contracts;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MajorContractModel GetById(Guid? contractId)
        {
            if (contractId == null) return null;

            var rawKey = string.Concat("ContractByID-", contractId);

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is MajorContractModel contract) return contract;
            // Item not found in cache - retrieve it and insert it into the cache
            contract = Api.GetById(contractId);
            if (contract != null) AddCacheItem(rawKey, contract);

            return contract;
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int? Save(MajorContractModel model)
        {
            var contractId = Api.Save(model);
            if (contractId > 0)
                // Invalidate the cache
                InvalidateCache();
            return contractId;
        }

        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public int? Delete(MajorContractModel model)
        {
            var ret = Api.Delete(model);
            if (ret > 0)
                // Invalidate the cache
                InvalidateCache();
            return ret;
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int? SaveRefFiles(MajorContractModel model)
        {
            var contractId = Api.SaveRefFiles(model);
            if (contractId > 0)
                // Invalidate the cache
                InvalidateCache();
            return contractId;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<MajorContractModel> CheckLandMap(string mapNo, string landParcelNo, int? provinceId, int? wardId)
        {
            var rawKey = $"CheckLandMap-{mapNo}-{landParcelNo}-{provinceId}-{wardId}";

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<MajorContractModel> lstContracts) return lstContracts;
            // Item not found in cache - retrieve it and insert it into the cache
            lstContracts = Api.CheckLandMap(mapNo, landParcelNo, provinceId, wardId);
            AddCacheItem(rawKey, lstContracts);

            return lstContracts;
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int? Valid(MajorContractModel model)
        {
            var contractId = Api.Valid(model);
            if (contractId > 0)
                // Invalidate the cache
                InvalidateCache();
            return contractId;
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int? Approve(MajorApproveDossierModel model)
        {
            var contractId = Api.Approve(model);
            if (contractId > 0)
                // Invalidate the cache
                InvalidateCache();
            return contractId;
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int? UpdateInfo(Guid? contractId, string jsonContractInfo, string fileId, string updatedBy)
        {
            var retUpdate = Api.UpdateInfo(contractId, jsonContractInfo, fileId, updatedBy);
            if (retUpdate > 0)
                // Invalidate the cache
                InvalidateCache();
            return retUpdate;
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int? Acceptant(MajorContractModel model)
        {
            var contractId = Api.Acceptant(model);
            if (contractId > 0)
                // Invalidate the cache
                InvalidateCache();
            return contractId;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<MajorContractModel> Dashboard(out int total, string userName, string managerUnions = null,
            string searchValue = null, DateTime? receivedFromDate = null, DateTime? receivedToDate = null,
            DateTime? giveResultFromDate = null, DateTime? giveResultToDate = null, string contractStatus = null,
            string typeContractIds = null, string typeCusIds = null, string typeTermIds = null,
            BaseSearchModel search = null)
        {
            //var objectKey = EHashMD5.FromObject(search);
            //var rawKey = $"DashboardContracts-{objectKey}-{searchValue}-{fromDate}-{toDate}-{contractStatus}-{typeContractIds}-{typeCusIds}-{username}";
            //var rawKeyTotal = string.Concat(rawKey, "-Total");

            //total = 0;
            //var cacheTotal = (int?)GetCacheItem(rawKeyTotal);
            //total = cacheTotal ?? 0;
            //// See if the item is in the cache
            //if (GetCacheItem(rawKey) is List<MajorContractModel> contracts) return contracts;
            // Item not found in cache - retrieve it and insert it into the cache
            var contracts = Api.Dashboard(out total, userName, managerUnions, searchValue, receivedFromDate,
                receivedToDate, giveResultFromDate, giveResultToDate, contractStatus, typeContractIds, typeCusIds,
                typeTermIds, search);
            //AddCacheItem(rawKey, contracts);
            //AddCacheItem(rawKeyTotal, total);

            return contracts;
        }

        #endregion

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