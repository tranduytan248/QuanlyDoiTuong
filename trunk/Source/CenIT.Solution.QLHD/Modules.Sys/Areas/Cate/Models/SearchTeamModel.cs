using System.Collections.Generic;
using System.Web.UI.WebControls;
using TSFramework.App.Attributes;

namespace Modules.Sys.Areas.Cate.Models
{
    public class SearchTeamModel
    {
        public SearchTeamModel()
        {
            ListProvinces = ListWards = new List<ListItem>();
        }

        [CustomDisplayName("Province_Title")] public List<int> ListProvinceId { get; set; }

        public string ProvinceIds { get; set; }

        [CustomDisplayName("Province_Title")] public List<ListItem> ListProvinces { get; set; }

        public string WardIds { get; set; }

        [CustomDisplayName("Ward_Title")] public List<int> ListWardId { get; set; }

        [CustomDisplayName("Ward_Title")] public List<ListItem> ListWards { get; set; }

        public int? WardId { get; set; }
    }
}