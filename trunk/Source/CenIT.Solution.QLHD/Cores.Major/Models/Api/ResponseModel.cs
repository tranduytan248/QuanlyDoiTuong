using Newtonsoft.Json;

namespace Cores.Major.Models.Api
{
    public class ResponseModel
    {
        [JsonProperty("success")] public string Success { get; set; }

        [JsonProperty("message")] public string Message { get; set; }
    }
}