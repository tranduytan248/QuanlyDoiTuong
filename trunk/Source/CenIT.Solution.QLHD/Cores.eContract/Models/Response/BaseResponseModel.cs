using Newtonsoft.Json;
using System.Collections.Generic;

namespace Cores.eContract.Models.Response
{
    public class BaseResponseModel<T>
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("statusCode")]
        public int StatusCode { get; set; }

        [JsonProperty("object")]
        public T ResData { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("errors")]
        public List<ErrorModel> Errors { get; set; }

        [JsonProperty("error")]
        public List<string> Error { get; set; }
    }

    public class ErrorModel
    {
        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("info")]
        public ErrorInfoModel Info { get; set; }
    }

    public class ErrorInfoModel
    {
        [JsonProperty("missingProperty")]
        public string MissingProperty { get; set; }
    }
}