using Cores.eContract.Models.Request;
using Cores.eContract.Models.Response;
using Modules.eContract.Providers;
using System.Net;
using TSFramework.Core.Members.Caching;
using TSFramework.Core.Utils;

namespace Cores.Cate.Caches
{
    public class CateContractFormCache : CacheLayer
    {
        protected override string[] MasterCacheKeyArray => new[] { "CateContractFormCache", "CENIT.APP.Cache" };

        public BaseResponseModel<ResTemplateContractModel> GetListTemplateContract(out int total, ReqSearchModel reqModel, out string errMsg)
        {
            var objectKey = EHashMD5.FromObject(reqModel);
            var cacheKey = string.Concat("GetListTemplateContract_", objectKey);

            var rawKeyTotal = string.Concat(cacheKey, "-Total");
            total = 0;

            if (GetCacheItem(cacheKey) is BaseResponseModel<ResTemplateContractModel> cachedData)
            {
                errMsg = null;
                total = cachedData.ResData.TotalElement;// tong so dong
                return cachedData;
            }

            // If data not found in cache, retrieve from the provider
            var response = EContractProvider.GetContractTemplates(reqModel, out errMsg);

            if (response != null && response.StatusCode == (int)HttpStatusCode.OK)
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

        public BaseResponseModel<ResDetailTemplateContractModel> GetDetailTemplateContract(string templateId, out string errMsg)
        {
            var objectKey = EHashMD5.FromObject(templateId);
            var cacheKey = string.Concat("GetDetailTemplateContract_", objectKey);

            if (GetCacheItem(cacheKey) is BaseResponseModel<ResDetailTemplateContractModel> cachedData)
            {
                errMsg = null;
                return cachedData;
            }

            // If data not found in cache, retrieve from the provider
            var response = EContractProvider.GetDetailTemplateContract(templateId, out errMsg);

            if (response.StatusCode == (int)HttpStatusCode.OK)
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
