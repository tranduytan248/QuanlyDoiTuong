using Cores.eContract.Consts;
using Newtonsoft.Json;

namespace Cores.eContract.Models
{
    public class SignInfoModel
    {
        [JsonProperty("signForm")] public string SignForm { get; set; } = ConstsSignForms.USB_TOKEN;
    }
}