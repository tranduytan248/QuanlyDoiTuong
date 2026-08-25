using Newtonsoft.Json;

namespace Cores.eContract.Models.Request
{
    public class ReqContractDownloadModel
    {
        /// <summary>
        /// ID hợp đồng trên hệ  thố ng eContract  
        /// </summary>
        [JsonProperty("contractId")] public string ContractId { get; set; }
        /// <summary>
        /// DRAFT/CONTRACT
        /// </summary>
        [JsonProperty("documentType")] public string DocumentType { get; set; }
        /// <summary>
        /// Hash file hợp đồng. Lấy từ API chi tiết hợp đồng 
        /// </summary>
        [JsonProperty("documentHash")] public string DocumentHash { get; set; }

    }
}