using System.Collections.Generic;
using TSFramework.App.Attributes;
using TSFramework.App.Models;

namespace Cores.Cate.Models
{
    public class CateLandAreaModel : BaseSearchModel
    {
        public List<CateLandTypeModel> ListLandTypes = new List<CateLandTypeModel>();
        public int LandArea_ID { get; set; }

        [CustomRequired]
        [CustomDisplayName("LandArea_Label_LandSize")]
        public string LandSize { get; set; }

        [CustomRequired]
        [CustomDisplayName("LandType_Label_Name")]
        public int LandType_ID { get; set; }

        [CustomRequired]
        [CustomDisplayName("LandArea_Label_Unit")]
        public string Unit { get; set; }

        [CustomRequired]
        [CustomDisplayName("LandArea_Label_UnitPrice")]
        public decimal UnitPrice { get; set; }

        [CustomDisplayName("LandType_Label_Name")]
        public string LandTypeName { get; set; }
    }

    public class CateLandAreaSearchModel : CateLandAreaModel
    {
        [CustomDisplayName("Label_Search")] public string TuKhoa { get; set; }
    }
}