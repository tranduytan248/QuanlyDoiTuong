using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Cores.Cate.Enum;
using TSFramework.App.Attributes;
using TSFramework.App.Models;

namespace Cores.Cate.Models
{
    public class CateUnionMemberModel : BaseModel
    {
        [CustomDisplayName("Union_Title")]
        [CustomRequired]
        public Guid? UnionId { get; set; }

        [CustomDisplayName("Union_Title")] public string UnionName { get; set; }

        [CustomDisplayName("Position_Title")]
        [CustomRequired]
        public int? PositionId { get; set; }

        [CustomDisplayName("Position_Title")] public string PositionName { get; set; }

        [CustomDisplayName("User_Title")]
        //[CustomRequired]
        public string UserName { get; set; }

        [CustomDisplayName("User_Title")] public string FullName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        [CustomDisplayName("Union_Member_Permit")]
        public int Permit { get; set; } = (int)EnumUnionMemberPermit.Handler;

        public string PermitName { get; set; }

        [CustomDisplayName("User_Title")] public List<ListItem> ListUsers { get; set; } = new List<ListItem>();

        [CustomDisplayName("Union_Title")]
        public List<SelectListItem> ListUnions { get; set; } = new List<SelectListItem>();

        [CustomDisplayName("Position_Title")] public List<ListItem> ListPositions { get; set; } = new List<ListItem>();
    }
}