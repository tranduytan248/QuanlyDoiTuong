using Newtonsoft.Json;
using System.Collections.Generic;

namespace Cores.eContract.Models
{
    public class FlowContractModel
    {
        [JsonProperty("contractFlowTemplateId")]
        public string ContractFlowTemplateId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("disable")]
        public string Disable { get; set; }
        [JsonProperty("partyId")]
        public string PartyId { get; set; }
        [JsonProperty("fileName")]
        public string FileName { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("discuss")]
        public bool Discuss { get; set; }
        [JsonProperty("signFlowType")]
        public string SignFlowType { get; set; }
        [JsonProperty("internalDiscuss")]
        public List<InternalDiscuss> listInternalDiscuss { get; set; }
        [JsonProperty("signForm")]
        public List<string> SignForm { get; set; }
        [JsonProperty("signFlow")]
        public List<SignFlow> listSignFlow { get; set; }
    }

    public class InternalDiscuss
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }
        [JsonProperty("fullName")]
        public string FullName { get; set; }
        [JsonProperty("permissions")]
        public List<string> Permissions { get; set; } //xem lai
        [JsonProperty("departmentId")]
        public string DepartmentId { get; set; }
        [JsonProperty("departmentName")]
        public string DepartmentName { get; set; }
    }

    public class SignFlow
    {
        [JsonProperty("signType")]
        public string SignType { get; set; }
        [JsonProperty("departmentId")]
        public string DepartmentId { get; set; }
        [JsonProperty("departmentName")]
        public string DepartmentName { get; set; }
        [JsonProperty("userId")]
        public string UserId { get; set; }
        [JsonProperty("fullName")]
        public string FullName { get; set; }
        [JsonProperty("sequence")]
        public string Sequence { get; set; }
        [JsonProperty("limitDate")]
        public string LimitDate { get; set; }
        [JsonProperty("signForm")]
        public List<string> SignForm { get; set; }

    }
}
