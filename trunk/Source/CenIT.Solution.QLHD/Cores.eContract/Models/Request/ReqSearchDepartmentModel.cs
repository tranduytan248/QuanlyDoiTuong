using Newtonsoft.Json;

namespace Cores.eContract.Models.Request
{
    public class ReqSearchDepartmentModel : ReqSearchModel
    {
        /// <summary>
        /// 0/1: Trạng thái Đang hoạt động/Ngưng hoạt động 
        /// </summary>
        [JsonProperty("status")]
        public int Status { get; set; } = 1;
    }
}