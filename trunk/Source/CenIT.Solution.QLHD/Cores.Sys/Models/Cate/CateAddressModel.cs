using System.Collections.Generic;
using System.Web.UI.WebControls;
using TSFramework.App.Attributes;

namespace Cores.Sys.Models.Cate
{
    public class CateAddressModel
    {
        [CustomDisplayName("Province_Title")] public int? ProvinceId { get; set; }

        [CustomDisplayName("Province_Title")] public string ProvinceName { get; set; }

        [CustomDisplayName("Ward_Title")] public int? WardId { get; set; }

        [CustomDisplayName("Ward_Title")] public string WardName { get; set; }

        [CustomDisplayName("Street_Title")] public string StreetName { get; set; }

        [CustomDisplayName("Customer_AddressNo")]
        public string AddressNo { get; set; }

        public string ParentId { get; set; }

        public int EleViews { get; set; } = 32;

        public List<ListItem> ListProvinces { get; set; } = new List<ListItem>();
        public List<ListItem> ListWards { get; set; } = new List<ListItem>();
    }
}