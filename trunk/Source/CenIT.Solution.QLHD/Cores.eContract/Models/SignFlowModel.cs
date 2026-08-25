using Newtonsoft.Json;
using System.Collections.Generic;

namespace Cores.eContract.Models
{
    public class SignFlowModel
    {
        [JsonProperty("signType")]
        public string SignType { get; set; }

        [JsonProperty("departmentId")]
        public string DepartmentId { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("sequence")]
        public int Sequence { get; set; }

        [JsonProperty("limitDate")]
        public int LimitDate { get; set; }

        [JsonProperty("signForm")]
        public List<string> SignForm { get; set; }

        [JsonProperty("signFrame")]
        public List<SignFrameModel> SignFrame { get; set; }
    }
}