using Newtonsoft.Json;
using System.Web;

namespace Cores.eContract.Models.Request
{
    public class ReqContractWithIdentificationModel
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

        /// <summary>
        /// File pdf hợp đồng
        /// </summary>
        [JsonProperty("file")]
        [JsonIgnore]
        public HttpPostedFileBase File { get; set; }

        /// <summary>
        /// File ả nh EKYC chân dung (KHCN)  
        /// Định dạng: png / jpg / jpeg 
        /// </summary>
        [JsonProperty("EKYC_CHANDUNG")]
        [JsonIgnore]
        public HttpPostedFileBase KYCChandung { get; set; }

        /// <summary>
        /// File ả nh EKYC mặt trước giấy tờ (KHCN) 
        /// Định dạng: png / jpg / jpeg 
        /// </summary>
        [JsonProperty("EKYC_MATTRUOC")]
        [JsonIgnore]
        public HttpPostedFileBase KYCMattruoc { get; set; }

        /// <summary>
        /// File ả nh EKYC mặt sau giấy tờ (KHCN) 
        /// Định dạng: png / jpg / jpeg 
        /// </summary>
        [JsonProperty("EKYC_MATSAU")]
        [JsonIgnore]
        public HttpPostedFileBase KYCMatsau { get; set; }

        /// <summary>
        /// File giấy phép kinh doanh (KHDN) 
        /// Định dạng: pdf 
        /// </summary>
        [JsonProperty("GPKD")]
        [JsonIgnore]
        public HttpPostedFileBase GPKD { get; set; }
    }
}