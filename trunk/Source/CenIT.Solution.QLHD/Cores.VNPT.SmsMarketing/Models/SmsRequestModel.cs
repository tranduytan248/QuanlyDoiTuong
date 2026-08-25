using System.Collections.Generic;
using Newtonsoft.Json;

namespace Cores.VNPT.SmsMarketing.Models
{
    public class SmsRequestModel
    {
        [JsonProperty("RQST")] public Rqst Rqst { get; set; }
    }

    public class Rqst : SmsMarketingConfigModel
    {
        public Rqst(SmsMarketingConfigModel config)
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

        [JsonProperty("PARAMS")] public List<Param> Params { get; set; }

        [JsonProperty("MOBILELIST")] public string MobileList { get; set; }

        [JsonProperty("SCHEDULETIME")] public string ScheduleTime { get; set; } = "";

        [JsonProperty("ISTELCOSUB")] public string IstelCosub { get; set; } = "0";

        [JsonProperty("DATACODING")] public string DataCoding { get; set; } = "0";
    }

    public class Param
    {
        [JsonProperty("NUM")] public string Num { get; set; }

        [JsonProperty("CONTENT")] public string Content { get; set; }
    }
}