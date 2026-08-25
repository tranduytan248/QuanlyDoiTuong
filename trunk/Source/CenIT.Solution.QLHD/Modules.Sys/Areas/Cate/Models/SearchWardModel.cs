using System.Collections.Generic;
using System.Web.UI.WebControls;
using TSFramework.App.Attributes;

namespace Modules.Sys.Areas.Cate.Models
{
    public class SearchWardModel
    {
        [CustomDisplayName("Province_Title")] public List<int> ListProvinceId { get; set; }

        public string ProvinceIds { get; set; }

        [CustomDisplayName("Province_Title")] public List<ListItem> ListProvinces { get; set; } = new List<ListItem>();

        [CustomDisplayName("Province_Title")] public int? ProvinceId { get; set; }

        public string ProvinceName { get; set; }

        public List<ListItem> Provinces { get; set; }
    }
}