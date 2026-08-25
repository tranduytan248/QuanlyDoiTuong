using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using TSFramework.App.Attributes;
using TSFramework.App.Models;

namespace Cores.Sys.Models.Cate
{
    public class CateDistrictModel : BaseModel
    {
        public CateDistrictModel()
        {
            Provinces = new List<ListItem>();
        }

        [CustomRequired]
        [CustomDisplayName("Province_Title")]

        public int? ProvinceId { get; set; }

        [CustomDisplayName("Province_Title")] public string ProvinceCode { get; set; }

        [CustomDisplayName("Province_Title")] public string ProvinceName { get; set; }

        [CustomDisplayName("District_Title")] public int? DistrictId { get; set; }

        [CustomDisplayName("District_Label_Code")]
        public string DistrictCode { get; set; }

        [CustomRequired]
        [CustomDisplayName("District_Label_Name")]
        public string DistrictName { get; set; }

        public bool IsDeleted { get; set; } = false;
        public new int? TotalRow { get; set; } = 0;

        public string UserCreated { get; set; }
        public DateTime DateCreated { get; set; }


        [CustomDisplayName("Province_Title")] public List<ListItem> Provinces { get; set; }
    }
}