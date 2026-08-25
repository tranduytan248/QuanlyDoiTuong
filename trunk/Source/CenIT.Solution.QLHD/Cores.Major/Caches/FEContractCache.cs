using System;
using System.Collections.Generic;
using System.ComponentModel;
using Cores.Major.Biz;
using Cores.Major.Models;
using TSFramework.Core.Members.Caching;

namespace Cores.Major.Caches
{
    public class FEContractCache : CacheLayer
    {
        private FEContractBiz _contractApi;

        private FEContractBiz Api => _contractApi ?? (_contractApi = new FEContractBiz());

        protected override string[] MasterCacheKeyArray =>
            new[] { "ContractFECache", "CENIT.APP.Cache" };

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<FEContractModel> GetAll()
        {
            var rawKey = "AllContracts-";
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<FEContractModel> contracts) return contracts;
            // Item not found in cache - retrieve it and insert it into the cache
            contracts = Api.GetAll();
            AddCacheItem(rawKey, contracts);

            return contracts;
        }

        public List<FETaskModel> TasksGetByContractId(Guid? id)
        {
            var listTask = Api.TasksGetByContractId(id);
            return listTask;
        }

        public List<FEPaymentModel> PaymentGetByContractId(Guid? id)
        {
            var listPayment = Api.PaymentGetByContractId(id);
            return listPayment;
        }

        //public byte[] RenderContract(string templateId, string dataFields, out string errMsg)
        //{
        //    var objectKey = EHashMD5.FromObject(templateId);
        //    var objectKey2 = EHashMD5.FromObject(dataFields);
        //    var cacheKey = string.Concat("RenderContract_", objectKey, objectKey2);

        //    if (GetCacheItem(cacheKey) is byte[] cachedData)
        //    {
        //        errMsg = null;
        //        return cachedData;
        //    }

        //    // If data not found in cache, retrieve from the provider
        //    var response = EContractProvider.RenderContract(templateId, dataFields, out errMsg);

        //    if (response != null)
        //    {
        //        // Cache the retrieved data
        //        AddCacheItem(cacheKey, response);
        //        return response;
        //    }

        //    // Handle error when failed to retrieve data
        //    errMsg = "Failed";
        //    return null;
        //}

        public FERenderContractModel GetDataRenderContract(Guid? id)
        {
            if (id == null) return null;

            var renderContract = Api.GetDataRenderContract(id);
            return renderContract;
        }

        //lấy chi tiết hợp đồng by contractNo, cusName, identifierNo
        public FEContractModel DetailContract(string contractNo, string phone)
        {
            var contract = Api.DetailContract(contractNo, phone);
            return contract;
        }

        //Lấy chi tiết hợp đồng ByQRCode
        public FEContractModel DetailByQRCode(Guid? contractId)
        {
            var contract = Api.DetailByQRCode(contractId);
            return contract;
        }
    }
}