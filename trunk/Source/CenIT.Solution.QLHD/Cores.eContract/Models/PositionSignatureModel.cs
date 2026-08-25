using Newtonsoft.Json;
using System.Collections.Generic;

namespace Cores.eContract.Models
{
    public class PositionSignatureModel
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("position")]
        public List<Position> Position { get; set; }
    }

    public class Position
    {
        [JsonProperty("bboxSign")]
        public List<double> BboxSign { get; set; }

        [JsonProperty("pageSign")]
        public int PageSign { get; set; }
    }
}
