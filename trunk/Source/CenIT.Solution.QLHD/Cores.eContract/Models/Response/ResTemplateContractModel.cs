using Newtonsoft.Json;
using System.Collections.Generic;

namespace Cores.eContract.Models.Response
{
    public class ResTemplateContractModel
    {
        [JsonProperty("data")]
        public List<TemplateContractModel> TemplateContracts { get; set; }

        [JsonProperty("maxSize")]
        public int MaxSize { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("propertiesSort")]
        public string PropertiesSort { get; set; }

        [JsonProperty("sort")]
        public string Sort { get; set; }

        [JsonProperty("totalElement")]
        public int TotalElement { get; set; }
    }
}