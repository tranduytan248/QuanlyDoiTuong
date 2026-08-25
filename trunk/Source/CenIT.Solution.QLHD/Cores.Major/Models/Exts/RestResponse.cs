using Newtonsoft.Json;

namespace Cores.Major.Models.Exts
{
    public class RestResponse
    {
        [JsonProperty("message")] public object Message;

        [JsonProperty("result")] public object Result;

        [JsonProperty("success")] public string Success;
    }
}