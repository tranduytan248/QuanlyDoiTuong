using Newtonsoft.Json;

namespace Cores.eContract.Models.Request
{
    public class ReqAuthUserModel : ReqAuthModel
    {
        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("domain")]
        public string Domain { get; set; }
        [JsonProperty("grant_type")]
        public new string GrantType { get; set; } = "password";
        [JsonProperty("client_id")]
        public string ClientID { get; set; } = "clientapp";
        [JsonProperty("client_secret")]
        public new string ClientSecret { get; set; } = "password";
    }
}