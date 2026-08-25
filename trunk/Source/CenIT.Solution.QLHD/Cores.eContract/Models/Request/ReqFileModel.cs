using Newtonsoft.Json;
using System.Web;

namespace Cores.eContract.Models.Request
{
    public class ReqFileModel
    {
        [JsonProperty("attachFile")]
        public HttpPostedFileBase AttachFile { get; set; }
    }
}