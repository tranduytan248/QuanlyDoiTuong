using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using TSFramework.App.Attributes;
using TSFramework.App.Models;

namespace Cores.Sys.Models.Cate
{
    public class CateStreetModel : BaseModel
    {
        public int? StreetId { get; set; } = 0;

        [CustomDisplayName("Street_Label_Code")]
        //[CustomRequired]
        public string StreetCode { get; set; }

        [CustomRequired]
        [CustomDisplayName("Street_Label_Name")]
        public string StreetName { get; set; }

        [CustomDisplayName("Street_Label_Parent")]
        public int? ParentId { get; set; } = 0;

        [CustomDisplayName("Street_Label_Parent")]
        public string ParentName { get; set; }

        [CustomDisplayName("Street_Label_Parent")]
        public string ParentCode { get; set; }

        public string UserCreated { get; set; }
        public DateTime DateCreated { get; set; }

        public new int? TotalRow { get; set; } = 0;

        public List<ListItem> Provinces { get; set; }
        public List<ListItem> Districts { get; set; }
        public List<ListItem> Wards { get; set; }
        public List<ListItem> Streets { get; set; }

        [CustomDisplayName("Ward_Title")] public string WardIds { get; set; }

        [CustomDisplayName("Province_Title")] public int? ProvinceId { get; set; }

        [CustomDisplayName("Province_Title")] public string ProvinceName { get; set; }

        [CustomDisplayName("District_Title")] public int? DistrictId { get; set; }

        [CustomDisplayName("District_Title")] public string DistrictName { get; set; }

        [CustomDisplayName("Ward_Title")]
        [CustomRequired]
        public List<int> SelectedWardIds { get; set; }

        [CustomDisplayName("Ward_Title")] public string WardName { get; set; }
    }
}