using TSFramework.App.Attributes;
using TSFramework.App.Models;

namespace Cores.Cate.Models
{
    public class CateLandTypeModel : BaseSearchModel
    {
        public int LandType_ID { get; set; }

        [CustomRequired]
        [CustomDisplayName("LandType_Label_Code")]
        public string LandTypeCode { get; set; }

        [CustomRequired]
        [CustomDisplayName("LandType_Label_Name")]
        public string LandTypeName { get; set; }
    }
}