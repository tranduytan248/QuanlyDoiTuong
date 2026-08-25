using Newtonsoft.Json;

namespace Modules.Major.Areas.Major.Models
{
    public class PayAcceptantRecordModel
    {
        public string UnionName { get; set; }
        [JsonProperty("${tenDoanhNghiepB}")]
        public string UnionNameBrand { get; set; }
        [JsonProperty("${tong_KP}")]
        public string TotalPayment { get; set; }
        public string CodeUnion { get; set; }
        public string TotalPaymentDone { get; set; }
        public string PaymentAdvance { get; set; }
        [JsonProperty("${thongTinBenA}")]
        public string InfoA { get; set; }
        [JsonProperty("${danhXungB}")]
        public string TitleB { get; set; }
        [JsonProperty("${tenKH}")]
        public string NameA { get; set; }
        [JsonProperty("${tenB}")]
        public string NameB { get; set; }
        [JsonProperty("${chucVuB}")]
        public string PositionB { get; set; }
        [JsonProperty("${soDienThoaiB}")]
        public string PhoneB { get; set; }
        public string PayNumber { get; set; }
        public string PayText { get; set; }
        public string ContractNo { get; set; }
        public string ContractAsign { get; set; }
        public string ContractNoInfo { get; set; }
        [JsonProperty("${ngayLap}")]
        public string ContractDay { get; set; }
        public string Day { get; set; }
        [JsonProperty("${thangLap}")]
        public string ContractMonth { get; set; }
        public string Month { get; set; }
        [JsonProperty("${namLap}")]
        public string ContractYear { get; set; }
        public string Year { get; set; }
        public string MaQHNS { get; set; }
    }
}