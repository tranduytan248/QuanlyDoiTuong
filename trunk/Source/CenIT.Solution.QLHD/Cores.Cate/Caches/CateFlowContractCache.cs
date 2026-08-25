using Cores.eContract.Models;
using Cores.eContract.Models.Request;
using Cores.eContract.Models.Response;
using Modules.eContract.Providers;
using TSFramework.Core.Members.Caching;
using TSFramework.Core.Utils;

namespace Cores.Cate.Caches
{
    public class CateFlowContractCache : CacheLayer
    {
        protected override string[] MasterCacheKeyArray => new[] { "CateFlowContractCache", "CENIT.APP.Cache" };

        public BaseResponseModel<ResListFlowContractModel> GetListFlowContract(out int total, ReqListFlowContractModel reqModel, out string errMsg)
        {
            var objectKey = EHashMD5.FromObject(reqModel);
            var cacheKey = string.Concat("GetListFlowContract_", objectKey);

            var rawKeyTotal = string.Concat(cacheKey, "-Total");
            total = 0;

            if (GetCacheItem(cacheKey) is BaseResponseModel<ResListFlowContractModel> cachedData)
            {
                errMsg = null;
                total = cachedData.ResData.TotalElement;
                return cachedData;
            }

            // If data not found in cache, retrieve from the provider
            var response = EContractProvider.GetListFlowContract(reqModel, out errMsg);

            if (response != null)
            {
                // Cache the retrieved data
                AddCacheItem(cacheKey, response);
                AddCacheItem(rawKeyTotal, response.ResData.TotalElement);
                return response;
            }
            else
            {
                // Handle error when failed to retrieve data
                errMsg = "Failed to retrieve data from the provider.";
                return null;
            }
        }

        public BaseResponseModel<FlowContractModel> GetDetailFlowContract(string contractFlowTemplateId, out string errMsg)
        {
            var objectKey = EHashMD5.FromObject(contractFlowTemplateId);
            var cacheKey = string.Concat("GetDetailFlowContact_", objectKey);

            if (GetCacheItem(cacheKey) is BaseResponseModel<FlowContractModel> cachedData)
            {
                errMsg = null;
                return cachedData;
            }

            // If data not found in cache, retrieve from the provider
            var response = EContractProvider.GetDetailFlowContact(contractFlowTemplateId, out errMsg);

            if (response != null)
            {
                // Cache the retrieved data
                AddCacheItem(cacheKey, response);
                return response;
            }
            else
            {
                // Handle error when failed to retrieve data
                return null;
            }
        }



    }
}
