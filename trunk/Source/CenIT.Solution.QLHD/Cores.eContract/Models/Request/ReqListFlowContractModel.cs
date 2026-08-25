using Newtonsoft.Json;

namespace Cores.eContract.Models.Request
{
    public class ReqListFlowContractModel : ReqSearchModel
    {
        /// <summary>
        /// Trạng thái 0: đang hoạt động, 1: ngưng hoạt động
        /// </summary>
        [JsonProperty("disable")]
        public string Disable { get; set; }

        /// <summary>
        /// Đàm phán 1: đàm phá, 0: không đàm phán 
        /// </summary>
        [JsonProperty("discuss")]
        public string Discuss { get; set; }
    }
}