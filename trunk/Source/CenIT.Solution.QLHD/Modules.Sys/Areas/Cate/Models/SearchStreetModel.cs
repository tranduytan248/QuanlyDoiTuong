using System.Collections.Generic;
using System.Web.UI.WebControls;
using TSFramework.App.Attributes;

namespace Modules.Sys.Areas.Cate.Models
{
    public class SearchStreetModel
    {
        public SearchStreetModel()
        {
            ListProvinces = ListDistricts = ListWards = new List<ListItem>();
        }

        [CustomDisplayName("Province_Title")] public List<int> ListProvinceId { get; set; }

        public string ProvincesIds { get; set; }

        [CustomDisplayName("Province_Title")] public List<ListItem> ListProvinces { get; set; }

        public string DistrictIds { get; set; }

        [CustomDisplayName("District_Title")] public List<int> ListDistrictId { get; set; }

        [CustomDisplayName("District_Title")] public List<ListItem> ListDistricts { get; set; }

        public string WardIds { get; set; }

        [CustomDisplayName("Ward_Title")] public List<int> ListWardId { get; set; }

        [CustomDisplayName("Ward_Title")] public List<ListItem> ListWards { get; set; }
    }
}