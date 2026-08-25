using Newtonsoft.Json;

namespace Cores.VNPT.SmsMarketing.Models
{
    public class SmsResponseModel
    {
        [JsonProperty("RPLY")] public SmsResponseDataModel Rply { get; set; }
    }

    public class SmsResponseDataModel
    {
        [JsonProperty("name")] public string Name { get; set; }

        /// <summary>
        ///     -1 Exception
        ///     0 Success
        ///     1 Username, password, IP, status các API không hợp lệ
        /// </summary>
        [JsonProperty("ERROR")]
        public string Error { get; set; }

        [JsonProperty("ERROR_DESC")] public string ErrorDesc { get; set; }

        [JsonIgnore] public bool IsSuccess => Error == "0";
    }
}