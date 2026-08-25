using System;
using System.Collections.Generic;
using System.Web.Mvc;
using TSFramework.App.Attributes;
using TSFramework.App.Models;

namespace Cores.Major.Models
{
    public class MajorProcedureStepSituationModel : BaseModel
    {
        public Guid? SituationId { get; set; }

        [CustomDisplayName("Situation_Title")]
        [CustomRequired]
        public string SituationName { get; set; }

        public Guid? ProcedureId { get; set; }

        [CustomDisplayName("Step_Title")]
        [CustomRequired]
        public Guid? StepId { get; set; }

        [CustomDisplayName("Step_Title")] public string StepName { get; set; }

        [CustomDisplayName("Step_NextStep")]
        [CustomRequired]
        public Guid? NextStep { get; set; }

        [CustomDisplayName("Step_NextStep")] public string NextStepName { get; set; }

        public List<SelectListItem> ListNextSteps { get; set; } = new List<SelectListItem>();

        public bool IsEdit { get; set; } = false;
    }
}