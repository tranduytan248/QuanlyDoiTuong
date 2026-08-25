using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Cores.Cate.Models;
using TSFramework.App.Attributes;

namespace Cores.Major.Models
{
    public class MajorDossierTaskSwitchHandlerModel
    {
        public Guid? HandlerId { get; set; }

        [CustomDisplayName("Task_Title")] public Guid? TaskId { get; set; }

        [CustomDisplayName("Task_Title")] public string TaskName { get; set; }

        /// <summary>
        ///     Đơn vị xử lý
        /// </summary>
        [CustomDisplayName("Union_Title")]
        public Guid? UnionId { get; set; }

        [CustomDisplayName("Union_Title")] public string UnionName { get; set; }

        public List<SelectListItem> ListUnions { get; set; } = new List<SelectListItem>();

        /// <summary>
        ///     Phòng ban xử lý
        /// </summary>
        [CustomDisplayName("Department_Title")]
        public Guid? DeptId { get; set; }

        [CustomDisplayName("Department_Title")]
        public string DeptName { get; set; }

        public List<SelectListItem> ListDepts { get; set; } = new List<SelectListItem>();

        [CustomDisplayName("Position_Title")] public int? PositionId { get; set; }

        [CustomDisplayName("Position_Title")] public string PositionName { get; set; }

        public List<ListItem> ListPositions { get; set; } = new List<ListItem>();

        [CustomDisplayName("Task_HandledBy")] public string StaffId { get; set; }

        public string Handler => StaffId;

        [CustomDisplayName("Task_HandledBy")] public string StaffName { get; set; }

        public List<SelectListItem> ListStaffs { get; set; } = new List<SelectListItem>();

        public List<CateUnionMemberModel> StaffsViaDept { get; set; } = new List<CateUnionMemberModel>();

        public List<string> TaskHandlers { get; set; } = new List<string>();

        public string PrimaryHandler { get; set; }

        public bool HasPrimary { get; set; } = false;

        [CustomDisplayName("Task_HandledBy_Primary")]
        public bool IsPrimary { get; set; } = false;

        public bool IsEdit { get; set; } = false;
    }
}