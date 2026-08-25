using System;
using TSFramework.App.Attributes;

namespace Cores.Major.Models
{
    public class MajorStepChangeHandleModel
    {
        [CustomDisplayName("Dossier_Title")] public Guid? DossierId { get; set; }

        [CustomDisplayName("Task_Title")] public Guid? TaskId { get; set; }

        [CustomDisplayName("Step_Title")] public Guid? StepId { get; set; }

        [CustomDisplayName("Step_Name")] public string StepName { get; set; }

        [CustomDisplayName("Step_Desc")] public string StepDesc { get; set; }

        [CustomDisplayName("Department_Title")]
        [CustomRequired]
        public Guid? UnionId { get; set; }

        [CustomDisplayName("Department_Title")]
        public string UnionName { get; set; }

        [CustomDisplayName("Staff_Handle_Title")]
        [CustomRequired]
        public string StaffId { get; set; }

        [CustomDisplayName("Staff_Handle_Title")]
        public string StaffName { get; set; }

        [CustomDisplayName("Position_Title")] public int? PositionID { get; set; }

        [CustomDisplayName("Position_Title")] public string PositionName { get; set; }

        public bool AllowSwitchHandler { get; set; } = false;
    }
}