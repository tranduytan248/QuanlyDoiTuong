using Newtonsoft.Json;
using System.Collections.Generic;

namespace Cores.eContract.Models.Response
{
    public class ResListPositionSignatureModel
    {
        [JsonProperty("result")]
        public List<PositionSignatureModel> listPositionSignature { get; set; }
        /// <summary>
        /// Trang 
        /// </summary>
        [JsonProperty("page")]
        public int Page { get; set; }

        /// <summary>
        /// Số lượng kết quả tối đa một trang  
        /// </summary>
        [JsonProperty("maxSize")]
        public int MaxSize { get; set; }

        /// <summary>
        /// Tổng số kết quả 
        /// </summary>
        [JsonProperty("totalElement")]
        public int TotalElement { get; set; }
    }
}
