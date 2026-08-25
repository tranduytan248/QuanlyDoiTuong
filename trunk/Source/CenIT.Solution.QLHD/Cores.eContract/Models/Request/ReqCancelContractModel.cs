using Newtonsoft.Json;

namespace Cores.eContract.Models.Request
{
    public class ReqCancelContractModel
    {
        [JsonProperty("cancelReason")]
        public string CancelReason { get; set; }
    }
}