using Newtonsoft.Json;
using System.Collections.Generic;

namespace Modules.Major.Areas.Major.Models.Exts
{
    public class ResUnitFieldModel : RestResponse
    {
        [JsonProperty("result")]
        public new ResultUnitField Result { get; set; }
    }

    public class ResultUnitField
    {
        [JsonProperty("code")]
        public object Code { get; set; }

        [JsonProperty("lstUnit")]
        public List<LstUnit> LstUnit { get; set; }

        [JsonProperty("lstField")]
        public List<LstField> LstField { get; set; }
    }

    public class LstField
    {
        [JsonProperty("value")]
        public int Value { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("isMain")]
        public bool IsMain { get; set; }
    }

    public class LstUnit
    {
        [JsonProperty("value")]
        public int UnitId { get; set; }

        [JsonProperty("text")]
        public string UnitName { get; set; }

        [JsonProperty("parentId")]
        public int ParentId { get; set; }

        [JsonProperty("unitLevel")]
        public int UnitLevel { get; set; }

        [JsonProperty("isMain")]
        public bool IsMain { get; set; }
    }
}