using System;
using System.Collections.Generic;
using System.Web.Mvc;
using TSFramework.App.Attributes;
using TSFramework.App.Models;

namespace Cores.Major.Models
{
    public class MajorProcedureStepHandlingTimeModel : BaseModel
    {
        public Guid? HandlingTimeId { get; set; }

        public Guid? ProcedureId { get; set; }

        public Guid? StepId { get; set; }

        [CustomDisplayName("Step_HandlingTime")]
        [CustomRequired]
        public double HandlingTime { get; set; } = 1;

        [CustomDisplayName("Purpose_Title")]
        [CustomRequired]
        public List<int> ListPurposeIds { get; set; }

        public string PurposeIds { get; set; }

        public string PurposeNames { get; set; }

        public string ViewPurposeNames { get; set; }

        public List<SelectListItem> ListPurposes { get; set; } = new List<SelectListItem>();

        public bool IsEdit { get; set; } = false;
    }
}