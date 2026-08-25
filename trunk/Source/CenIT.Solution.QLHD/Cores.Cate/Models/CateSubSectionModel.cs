using System.Collections.Generic;
using TSFramework.App.Attributes;

namespace Cores.Cate.Models
{
    public class CateSubSectionModel
    {
        public List<CateMainSectionModel> ListMianSections = new List<CateMainSectionModel>();
        public int SubSectionId { get; set; }

        [CustomRequired]
        [CustomDisplayName("SubSection_Label_Name")]
        public string SubSectionName { get; set; }

        [CustomRequired]
        [CustomDisplayName("Price_Label_Unit")]
        public string Unit { get; set; }

        [CustomRequired]
        [CustomDisplayName("Price_Label_UnitPrice")]
        public double Price { get; set; }

        public string MainSectionName { get; set; }
        public int Cate_MainSectionId { get; set; }
        public int TotalRecord { get; set; }
    }
}