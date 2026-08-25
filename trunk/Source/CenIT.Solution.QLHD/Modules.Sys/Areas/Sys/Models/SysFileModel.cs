using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Modules.Sys.Areas.Sys.Models
{
    public class SysFileModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }

        //[JsonIgnore]
        [JsonProperty("absolutePath")]
        public string AbsolutePath { get; set; }

        [JsonIgnore]
        //[JsonProperty("isFolder")]
        public bool IsFolder { get; set; } = false;

        [JsonProperty("isFile")]
        public bool IsFile { get; set; } = false;

        [JsonProperty("icons")]
        public Dictionary<string, string[]> Icons { get; set; }
        [JsonProperty("children")]
        public List<SysFileModel> Childrens { get; set; } = new List<SysFileModel>();
    }
}