using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using TSFramework.App.Attributes;
using TSFramework.App.Models;

namespace Cores.Sys.Models.Cate
{
    public class CateTeamModel : BaseModel
    {
        public CateTeamModel()
        {
            Provinces = Wards = new List<ListItem>();
        }

        public int TeamId { get; set; }

        [CustomDisplayName("Province_Title")] public int? ProvinceId { get; set; }

        public string ProvinceName { get; set; }

        [CustomDisplayName("Ward_Title")]
        [CustomRequired]
        public int? WardId { get; set; }

        [CustomDisplayName("Ward_Title")] public string WardName { get; set; }

        [CustomDisplayName("Ward_Title")] public string WardCode { get; set; }

        [CustomDisplayName("Ward_Title")] public string WardTeam { get; set; }

        [CustomDisplayName("Team_Label_Code")]
        //[CustomRequired]
        public string TeamCode { get; set; }

        [CustomRequired]
        [CustomDisplayName("Team_Label_Name")]
        public string TeamName { get; set; }

        public bool IsDeleted { get; set; }
        public new int? TotalRow { get; set; } = 0;
        public string UserCreated { get; set; }
        public DateTime DateCreated { get; set; }

        public List<ListItem> Provinces { get; set; }
        public List<ListItem> Wards { get; set; }

        #region Property For Area Partition

        public bool IsDisabled { get; set; } = false;

        #endregion
    }
}