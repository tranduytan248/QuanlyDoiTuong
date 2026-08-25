using Newtonsoft.Json;

namespace Cores.eContract.Models.Request
{
    public class ReqDigitalSignModel : ReqFileModel
    {
        [JsonProperty("data")]
        public SignInfoModel SignInfo { get; set; }
    }
}