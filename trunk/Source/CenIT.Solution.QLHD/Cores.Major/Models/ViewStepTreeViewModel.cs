using System.Collections.Generic;
using Newtonsoft.Json;

namespace Cores.Major.Models
{
    public class ViewStepTreeViewModel
    {
        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("typeElement")] public string TypeElement { get; set; }

        [JsonProperty("icons")] public Dictionary<string, List<string>> Icons { get; set; }

        [JsonProperty("children")] public List<ViewStepTreeViewModel> Children { get; set; }
    }
}