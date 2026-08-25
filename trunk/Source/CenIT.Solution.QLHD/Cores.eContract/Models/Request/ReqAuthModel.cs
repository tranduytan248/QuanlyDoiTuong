using Cores.eContract.Consts;
using Newtonsoft.Json;

namespace Cores.eContract.Models.Request
{
    public class ReqAuthModel
    {
        /// <summary>
        /// Định danh App chủ thể do eContract cung cấp. (Trường hợp 02: Dùng cứng giá trị “clientapp”)
        /// </summary>
        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        /// <summary>
        /// Mã  bí mật do eContract cung cấp. (Trường hợp 02: Dùng cứng giá trị  “password”)
        /// </summary>
        [JsonProperty("client_secret")]
        public string ClientSecret { get; set; }

        /// <summary>
        /// Dùng cứng giá trị  “client_credentials” (Trường hợp 02: Dùng cứng giá trị  “password”)
        /// </summary>
        [JsonProperty("grant_type")] public string GrantType { get; set; } = ConstsAuthGrantTypes.CLIENT_CREDENTIALS;
    }
}