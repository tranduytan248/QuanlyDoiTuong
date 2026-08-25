using Newtonsoft.Json;

namespace Cores.eContract.Models
{
    public class SignFrameModel
    {
        [JsonProperty("x")]
        public int X { get; set; }

        [JsonProperty("y")]
        public int Y { get; set; }

        [JsonProperty("w")]
        public int W { get; set; }

        [JsonProperty("h")]
        public int H { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }
    }
}