using Newtonsoft.Json;

namespace Cores.eContract.Models.Request
{
    public class ReqSearchModel
    {
        /// <summary>
        /// Trang
        /// </summary>
        [JsonProperty("page")]
        public int Page { get; set; } = 1;

        /// <summary>
        /// Số kết quả tối đa một trang 
        /// </summary>
        [JsonProperty("maxSize")]
        public int MaxSize { get; set; } = 100;

        /// <summary>
        /// DESC hoặc ASC
        /// </summary>
        [JsonProperty("sort")]
        public string Sort { get; set; } = "ASC";

        /// <summary>
        /// Trường thông tin sắp xếp 
        /// </summary>
        [JsonProperty("propertiesSort")]
        public string PropertiesSort { get; set; }

        /// <summary>
        /// Giá trị tìm kiếm, để trố ng để list tất cả
        /// </summary>
        [JsonProperty("keySearch")]
        public string KeySearch { get; set; }
    }
}