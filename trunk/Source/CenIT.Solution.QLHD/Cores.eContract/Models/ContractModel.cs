using Newtonsoft.Json;
using System.Collections.Generic;

namespace Cores.eContract.Models
{
    public class ContractModel
    {
        [JsonProperty("contractValue")]
        public string ContractValue { get; set; }

        [JsonProperty("creationNote")]
        public string CreationNote { get; set; }

        [JsonProperty("flowTemplateId")]
        public string FlowTemplateId { get; set; }

        [JsonProperty("orgTemplateId")]
        public string OrgTemplateId { get; set; }

        [JsonProperty("productId")]
        public string ProductId { get; set; }

        [JsonProperty("sequence")]
        public int Sequence { get; set; }

        [JsonProperty("signFlow")]
        public List<SignFlowModel> SignFlow { get; set; }

        [JsonProperty("signForm")]
        public List<string> SignForm { get; set; }

        [JsonProperty("templateId")]
        public string TemplateId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("validDate")]
        public string ValidDate { get; set; }

        [JsonProperty("verificationType")]
        public string VerificationType { get; set; }
    }
}