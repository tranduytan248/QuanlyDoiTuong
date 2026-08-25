using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using TSFramework.App.Attributes;
using TSFramework.App.Models;

namespace Cores.Major.Models
{
    public class MajorProcedureStepHandlerModel : BaseModel
    {
        public Guid? StepId { get; set; }

        public Guid? ProcedureId { get; set; }

        [CustomDisplayName("Union_Title")]
        [CustomRequired]
        public Guid? UnionId { get; set; }

        [CustomDisplayName("Union_Title")] public string UnionName { get; set; }

        public List<SelectListItem> ListUnions { get; set; } = new List<SelectListItem>();


        [CustomDisplayName("Department_Title")]
        [CustomRequired]
        public Guid? DeptId { get; set; }

        [CustomDisplayName("Department_Title")]
        public string DeptName { get; set; }

        public List<SelectListItem> ListDepts { get; set; } = new List<SelectListItem>();

        [CustomDisplayName("Position_Title")] public int? PositionID { get; set; }

        [CustomDisplayName("Position_Title")] public string PositionName { get; set; }

        public List<ListItem> ListPositions { get; set; } = new List<ListItem>();

        [CustomDisplayName("Staff_Title")] public string StaffId { get; set; }

        [CustomDisplayName("Staff_Title")] public string StaffName { get; set; }

        public List<SelectListItem> ListStaffs { get; set; } = new List<SelectListItem>();

        public bool IsEdit { get; set; } = false;

        #region Extend Change Handlers

        [CustomDisplayName("Step_AllowChangeHandler")]
        public bool AllowChangeHandler { get; set; } = false;

        [CustomDisplayName("Step_StepsChangeHandler")]
        public string StepsChangeHandler { get; set; }

        public List<string> ListStepNameChanges { get; set; }

        [CustomDisplayName("Step_StepsChangeHandler")]
        public List<Guid> ListStepsChangeHandler { get; set; } = new List<Guid>();

        [CustomDisplayName("Step_AllowSwitchHandler")]
        public bool AllowSwitchHandler { get; set; } = false;

        public List<SelectListItem> ListStepsChangeHandlers { get; set; } = new List<SelectListItem>();

        #endregion
    }
}