using Newtonsoft.Json;

namespace Modules.Major.Areas.Major.Models.Exts
{
    public class RestResponse
    {
        [JsonProperty("message")]
        public object Message;

        [JsonProperty("success")]
        public string Success;

        [JsonProperty("result")]
        public object Result;
    }
}