using TSFramework.App.Attributes;
using TSFramework.App.Models;

namespace Cores.Cate.Models
{
    public class CatePriceModel : BaseModel
    {
        public int PriceId { get; set; }

        [CustomRequired]
        [CustomDisplayName("SubSection_Label_Name")]
        public int Cate_SubSectionId { get; set; }

        [CustomRequired]
        [CustomDisplayName("Unit_Label_Unit")]
        public string Unit { get; set; }

        [CustomRequired]
        [CustomDisplayName("Price_Label_UnitPrice")]
        public double Price { get; set; }

        [CustomDisplayName("SubSection_Label_Name")]
        public string SubSectionName { get; set; }

        //public List<CateSubSectionModel> lst_SubSection = new List<CateSubSectionModel>();
    }

    public class CatePriceSearchModel
    {
        [CustomRequired]
        [CustomDisplayName("SubSection_Label_Name")]
        public int SubSectionId { get; set; }

        [CustomRequired]
        [CustomDisplayName("Unit_Label_Unit")]
        public string Unit { get; set; }
    }
}