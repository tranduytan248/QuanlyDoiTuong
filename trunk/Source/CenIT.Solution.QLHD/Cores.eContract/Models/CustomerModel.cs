using Newtonsoft.Json;

namespace Cores.eContract.Models
{
    public class CustomerModel
    {
        /// <summary>
        /// Tên khách hàng 
        /// </summary>
        [JsonProperty("ten")]
        public string Name { get; set; }

        /// <summary>
        /// Số điện thoại (bắt buộc) 
        /// </summary>
        [JsonProperty("sdt")]
        public string MobilePhone { get; set; }

        /// <summary>
        /// Email khách hàng (bắt buộc) 
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Mã số thuế khách hàng 
        /// </summary>
        [JsonProperty("mst")]
        public string TaxCode { get; set; }

        /// <summary>
        /// Số CMT/CCCD/HC 
        /// </summary>
        [JsonProperty("cmnd")]
        public string IdNo { get; set; }

        /// <summary>
        /// Id loại giấy tờ 
        /// - Passport: 1 
        /// - Các loại khác: 0 
        /// </summary>
        [JsonProperty("loaiGtId")]
        public string TypeIdDocument { get; set; }

        /// <summary>
        /// Tên tổ chức
        /// </summary>
        [JsonProperty("tenToChuc")]
        public string OrganName { get; set; }

        /// <summary>
        /// Loại khách hà ng  
        /// - CONSUMER: Cá nhân 
        /// - BUSINESS: Doanh nghiệp  
        /// </summary>
        [JsonProperty("userType")]
        public string UserType { get; set; }

        /// <summary>
        /// Tên tài khoản khách hàng
        /// </summary>
        [JsonProperty("username")]
        public string Username { get; set; }

        /// <summary>
        /// Nơi cấp CCCD khách hàng 
        /// </summary>
        [JsonProperty("noiCap")]
        public string IssuedBy { get; set; }

        /// <summary>
        /// Số  đăng ký doanh nghiệp 
        /// </summary>
        [JsonProperty("soDkdn")]
        public string BusinessRegNum { get; set; }

        /// <summary>
        /// Ngày cấp số đăng ký doanh nghiệp
        /// </summary>
        [JsonProperty("ngayCapSoDkdn")]
        public string BusinessRegNumIssuedOn { get; set; }

        /// <summary>
        /// Nơi cấp đăng ký kinh doanh
        /// </summary>
        [JsonProperty("noiCapDkkd")]
        public string BusinessRegNumIssuedBy { get; set; }

        /// <summary>
        /// ID mẫu email để gửi cho KH. (Tổ chức có nhu cầu gửi email cho KH theo template của công ty mình có thể gửi yêu cầu tạo mẫu email từ eContract) 
        /// </summary>
        [JsonProperty("emailTemplateId")]
        public string EmailTemplateId { get; set; }

        /// <summary>
        /// Vị trí ký hợp đồng cho khách hà ng 
        /// </summary>
        [JsonProperty("signFrame")]
        public string SignFrame { get; set; }
    }
}