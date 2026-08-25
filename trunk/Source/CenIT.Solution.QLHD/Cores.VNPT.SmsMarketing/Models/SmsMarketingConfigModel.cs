using System;
using Newtonsoft.Json;

namespace Cores.VNPT.SmsMarketing.Models
{
    public class SmsMarketingConfigModel
    {
        public SmsMarketingConfigModel(SmsMarketingConfigModel config)
        {
            UserName = config.UserName;
            AgentId = config.AgentId;
            ApiUser = config.ApiUser;
            ApiPass = config.ApiPass;
            ContractId = config.ContractId;
            ContractTypeId = config.ContractTypeId;
            LabelId = config.LabelId;
            Name = config.Name;
            TemplateId = config.TemplateId;
        }

        public SmsMarketingConfigModel()
        {
        }

        [JsonProperty("name")] public string Name { get; set; } = "send_sms_list";

        [JsonProperty("TEMPLATEID")] public string TemplateId { get; set; }

        [JsonProperty("REQID")] public string ReqId { get; set; } = DateTime.Now.ToString("ddMMyyyyHHmmss");

        [JsonProperty("LABELID")] public string LabelId { get; set; }

        [JsonProperty("CONTRACTID")] public string ContractId { get; set; }

        [JsonProperty("CONTRACTTYPEID")] public string ContractTypeId { get; set; }

        [JsonProperty("AGENTID")] public string AgentId { get; set; }

        [JsonProperty("APIUSER")] public string ApiUser { get; set; }

        [JsonProperty("APIPASS")] public string ApiPass { get; set; }

        [JsonProperty("USERNAME")] public string UserName { get; set; }

        [JsonProperty("SALEORDERID")] public string SaleOrderId { get; set; } = DateTime.Now.ToString("ddMMyyyyHHmmss");

        [JsonProperty("PACKAGEID")] public string PackageId { get; set; } = DateTime.Now.ToString("ddMMyyyyHHmmss");
    }
}