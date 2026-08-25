using Newtonsoft.Json;

namespace Cores.eContract.Models.Request
{
    public class ReqSearchContractTypeModel : ReqSearchModel
    {
        /// <summary>
        /// Tìm kiếm theo trạng thái loại hợp đồng
        /// Y hoăc true : Trạng thái Đang hoạt động 
        /// N hoặc false: Ngưng hoạt động
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}