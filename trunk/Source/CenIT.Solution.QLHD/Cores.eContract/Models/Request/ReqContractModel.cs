using Newtonsoft.Json;
using System.Web;

namespace Cores.eContract.Models.Request
{
    public class ReqContractModel : ReqFileModel
    {
        /// <summary>
        /// String dạng object các giá trị key và  value biến render Hợp đồng  
        /// </summary>
        [JsonProperty("fields")]
        public string Fields { get; set; }

        /// <summary>
        /// String dạng object các thông khách hàng
        /// </summary>
        [JsonProperty("customer")]
        public string Customer { get; set; }

        /// <summary>
        /// String dạng object các thông hợp đồng
        /// </summary>
        [JsonProperty("contract")]
        public string Contract { get; set; }

        [JsonProperty("file")]
        [JsonIgnore]
        public new HttpPostedFileBase AttachFile { get; set; }
    }
}