using System.Collections.Generic;
using System.Web.UI.WebControls;
using TSFramework.App.Attributes;

namespace Modules.Sys.Areas.Cate.Models
{
    public class SearchDistrictModel
    {
        public SearchDistrictModel()
        {
            ListProvinces = new List<ListItem>();
        }

        [CustomDisplayName("Province_Title")] public List<int> ListProvinceId { get; set; }

        public string ProvinceIds { get; set; }

        [CustomDisplayName("Province_Title")] public List<ListItem> ListProvinces { get; set; }
    }
}