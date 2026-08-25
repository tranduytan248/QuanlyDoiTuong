using System;
using System.Collections.Generic;
using System.Linq;
using Cores.eContract.Consts;
using Cores.Sys.Caches.Sys;
using Cores.VNPT.SmsMarketing.Consts;
using Cores.VNPT.SmsMarketing.Helpers;
using Cores.VNPT.SmsMarketing.Models;
using Newtonsoft.Json;
using TSFramework.Core.Providers;
using TSFramework.Core.Utils;

namespace Cores.VNPT.SmsMarketing.Providers
{
    public class SmsProvider
    {
        private static readonly Dictionary<EnumContractStatusHandle, string> mappingContractStatusTemplateSms =
            new Dictionary<EnumContractStatusHandle, string>
            {
                { EnumContractStatusHandle.Confirm, "SMS_BRANDNAME_TEMPLATEID_OTP_CONFIRM" },
                { EnumContractStatusHandle.Refuse, "SMS_BRANDNAME_TEMPLATEID_OTP_REFUSE" },
                { EnumContractStatusHandle.Resolved, "SMS_BRANDNAME_TEMPLATEID_OTP_RESOLVED" }
            };

        private static string SmsApiAddress { get; set; }
        private static string ActionUrl { get; set; }

        private static SmsMarketingConfigModel Config(string templateType)
        {
            var configApi = new SysConfigCache();

            SmsApiAddress = configApi.GetViaKey("SMS_BRANDNAME_API")?.ConfigValue;
            ActionUrl = configApi.GetViaKey("SMS_BRANDNAME_SEND")?.ConfigValue;

            return new SmsMarketingConfigModel
            {
                AgentId = configApi.GetViaKey("SMS_BRANDNAME_AGENTID")?.ConfigValue,
                ApiPass = configApi.GetViaKey("SMS_BRANDNAME_APIPASS")?.ConfigValue,
                ApiUser = configApi.GetViaKey("SMS_BRANDNAME_APIUSER")?.ConfigValue,
                ContractId = configApi.GetViaKey("SMS_BRANDNAME_CONTRACTID")?.ConfigValue,
                ContractTypeId = configApi.GetViaKey("SMS_BRANDNAME_CONTRACTTYPEID")?.ConfigValue,
                LabelId = configApi.GetViaKey("SMS_BRANDNAME_LABELID")?.ConfigValue,
                UserName = configApi.GetViaKey("SMS_BRANDNAME_USERNAME")?.ConfigValue,
                TemplateId = configApi.GetViaKey(templateType)?.ConfigValue
            };
        }

        public static bool Send(out string msgErr, string phoneNum, EnumContractStatusHandle contractStatusHandle,
            params string[] orderParrams)
        {
            msgErr = null;
            var templateType = mappingContractStatusTemplateSms[contractStatusHandle];
            var smsRequestModel = new SmsRequestModel
            {
                Rqst = new Rqst(Config(templateType))
                {
                    MobileList = PhoneNumberHelper.ConvertPhone(phoneNum),
                    PackageId = DateTime.Now.ToString("ddMMyyyyHHmmss"),
                    ReqId = DateTime.Now.ToString("ddMMyyyyHHmmss"),
                    SaleOrderId = DateTime.Now.ToString("ddMMyyyyHHmmss"),

                    Params = orderParrams == null
                        ? new List<Param>()
                        : orderParrams.ToList()
                            .Select((p, idx) => new Param
                            {
                                Num = $"{idx}",
                                Content = EString.RemoveSign4VietnameseString(p)
                            }).ToList()
                }
            };
            var bodyData = JsonConvert.SerializeObject(smsRequestModel);

            var restResponse = RestServiceProvider.Post($"{SmsApiAddress}{ActionUrl}", new Dictionary<string, string>(),
                ConstsContentTypes.JSON_UTF8, bodyData);

            if (!string.IsNullOrEmpty(restResponse?.Content))
            {
                var smsRespone = JsonConvert.DeserializeObject<SmsResponseModel>(restResponse.Content);
                if (!smsRespone?.Rply.IsSuccess ?? false) msgErr = smsRespone.Rply.ErrorDesc;
                return smsRespone?.Rply.IsSuccess ?? false;
            }

            msgErr = "Response null";

            return false;
        }
    }
}