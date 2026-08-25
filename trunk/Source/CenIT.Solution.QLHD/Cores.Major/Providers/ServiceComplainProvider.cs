using System.Collections.Generic;
using Cores.Major.Models.Api;
using Newtonsoft.Json;
using TSFramework.Core.Providers;

namespace Cores.Major.Providers
{
    public class ServiceComplainProvider
    {
        public static bool SendResultHandleComplain(out string resMsg, string urlReq, RequestResultHandleComplainModel result)
        {
            resMsg = string.Empty;
            var jsonSetting = new JsonSerializerSettings
            {
                StringEscapeHandling = StringEscapeHandling.EscapeHtml,
                NullValueHandling = NullValueHandling.Ignore
            };
            var dataReqJson = JsonConvert.SerializeObject(result, jsonSetting);

            var restResponse = RestServiceProvider.Post(urlReq, new Dictionary<string, string>(), "application/json", dataReqJson).Content;
            if (!string.IsNullOrEmpty(restResponse))
            {
                var resSendResult = JsonConvert.DeserializeObject<ResponseModel>(restResponse);
                resMsg = resSendResult.Message;
                return resSendResult.Success.Contains("OK");
            }

            return false;
        }
    }
}