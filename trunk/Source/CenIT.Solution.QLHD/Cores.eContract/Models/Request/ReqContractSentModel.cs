using Cores.eContract.Consts;
using Newtonsoft.Json;

namespace Cores.eContract.Models.Request
{
    public class ReqContractSentModel : ReqSearchModel
    {
        /// <summary>
        /// Từ ngày tạo  (YYYY-MM-DD) 
        /// </summary>
        [JsonProperty("fromDate")] public string FromDate { get; set; }
        /// <summary>
        /// Đến ngày tạo  (YYYY-MM-DD) 
        /// </summary>
        [JsonProperty("toDate")] public string ToDate { get; set; }
        /// <summary>
        /// Trạng thái hợp đồng
        ///  - LC_DRAFT_CREATE: Bản nháp  
        ///  - LC_DRAFT_DISCUSS: Đàm phán  (chưa có thỏa thuận) 
        ///  - LC_DRAFT_DEAL: Đàm phán (đã  có bên thỏa thuận)  
        ///  - LC_DRAFT_SUBMIT: Chờ ký 
        ///  - LC_DRAFT_CANCEL: Đã từ chối 
        ///  - LC_CONTRACT_VALID: Có hiệu lực  
        /// </summary>
        [JsonProperty("status")] public string Status { get; set; } = ConstsContractStatus.CREATE;
        /// <summary>
        /// Loại đối tác tham gia hợp đồng  
        ///  - CONSUMER: Cá nhân 
        ///  - BUSINESS: Doanh nghiệp 
        /// </summary>
        [JsonProperty("partnerType")] public string PartnerType { get; set; } = ConstsPartnerTypes.CONSUMER;
        /// <summary>
        /// Lượt ký nội bộ (INTERNAL) 
        /// Lượt ký đối tác  (EXTERNAL) 
        /// </summary>
        [JsonProperty("signTurn")] public string SignTurn { get; set; } = ConstsSignTurns.INTERNAL;
        /// <summary>
        /// Id hợp đồng 
        /// </summary>
        [JsonProperty("contractId")] public string ContractId { get; set; }
    }
}