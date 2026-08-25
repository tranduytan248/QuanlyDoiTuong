using System;
using System.Collections.Generic;
using System.Linq;

namespace Cores.Major.Models
{
    public class ViewProcedureStructureModel
    {
        public Guid? ProcedureId { get; set; }

        public string ProcedureName { get; set; }

        public string ProcedureDesc { get; set; }

        public Guid? TypeProcedure { get; set; }

        public string TypeProcedureName { get; set; }

        public Guid? ProcUnionId { get; set; }

        public string ProcUnionName { get; set; }

        public DateTime? ApplyFrom { get; set; }

        public DateTime? ExpiredOn { get; set; }

        public int? Version { get; set; }

        public List<ViewStepStructureModel> Steps { get; set; }
    }

    public class ViewStepStructureModel
    {
        public Guid? StepId { get; set; }

        public string StepName { get; set; }

        public string StepDesc { get; set; }

        public string StepType { get; set; }

        //public double HandlingTime { get; set; } = 1;

        public Guid? UnionHandle { get; set; }

        public string UnionHandleName { get; set; }

        public Guid? DeptHandle { get; set; }

        public string DeptHandleName { get; set; }

        public string HandledBy { get; set; }

        //public string Handler { get; set; }

        public int? PositionId { get; set; }

        public string PositionName { get; set; }

        public int Ordinal { get; set; }

        public Guid? NextStep { get; set; }

        public string NextStepName { get; set; }

        public Guid? NextSituation { get; set; }

        public string NextSituationName { get; set; }

        public double? NextSituationProcessedTime { get; set; } = 1;

        public Guid? PrevStep { get; set; }

        public string PrevStepName { get; set; }

        public List<ViewSituationStructureModel> Situations { get; set; }

        public List<ViewFormStructureModel> ListRefForms { get; set; }

        public Guid? DossierId { get; set; }

        public Guid? InStep { get; set; }

        public Guid? SelectedSituationId { get; set; }

        public string SelectedSituationName { get; set; }

        public double SelectedSituationProcessedTime { get; set; } = 1;

        public string StaffNotificationConfigs { get; set; }

        public string CusNotificationConfigs { get; set; }

        public Guid? ProcType { get; set; }

        public string ProcTypeName { get; set; }

        public bool ViewInputForm { get; set; } = true;

        public bool RequiredUploadDoc { get; set; } = true;

        public bool RollbackReceptionStep { get; set; } = true;

        public bool? AllowChangeHandler { get; set; } = false;

        public string StepsChangeHandler { get; set; }

        public bool? AllowSwitchHandler { get; set; } = false;

        public bool? AttachResultFile { get; set; } = false;

        public List<ViewHandlerStepStructureModel> Handlers { get; set; }

        public List<ViewHandlingTimeStepStructureModel> HandlingTimes { get; set; }

        public double TotalHandlingTimes(string purposeId = null)
        {
            if (HandlingTimes == null || HandlingTimes.Count <= 0) return 1;
            if (string.IsNullOrEmpty(purposeId) || !HandlingTimes.Exists(ht =>
                    !string.IsNullOrEmpty(ht.PurposeIds) &&
                    ht.PurposeIds.Split(',').ToList().Exists(p => p == purposeId)))
                return 1;

            return HandlingTimes.Where(ht =>
                    !string.IsNullOrEmpty(ht.PurposeIds) &&
                    ht.PurposeIds.Split(',').ToList().Exists(p => p == purposeId))
                .Sum(ht => ht.HandlingTime);
        }
    }

    public class ViewSituationStructureModel
    {
        public Guid? SituationId { get; set; }

        public string SituationDesc { get; set; }

        public double ProcessedTime { get; set; } = 1;

        public Guid? NextStep { get; set; }

        public string NextStepName { get; set; }

        public Guid? NextSituation { get; set; }

        public string NextSituationName { get; set; }

        public double? NextSituationProcessedTime { get; set; } = 1;

        public Guid? HandledBy { get; set; }

        public string Handler { get; set; }

        public bool IsCondition { get; set; }

        public List<ViewFormStructureModel> ListRefForms { get; set; }

        public string RefLegislationDoc { get; set; }

        public Guid? NextStepProcType { get; set; }

        public string NextStepProcTypeName { get; set; }

        public bool ViewInputForm { get; set; } = true;

        public bool RequiredUploadDoc { get; set; } = true;

        public string ViolatedConstructions { get; set; }

        public List<int> ListViolatedConstructions { get; set; }
    }

    public class ViewFormStructureModel
    {
        public Guid? DossierId { get; set; }
        public Guid? FormId { get; set; }
        public string FormCode { get; set; }
        public string FormName { get; set; }
        public string FormDesc { get; set; }
        public string TemplateName { get; set; }
        public string ViewName { get; set; }
        public int? Version { get; set; }
        public string FormData { get; set; }

        public string MappingFormKeys { get; set; }
        //public Dictionary<string, object> FormData { get; set; }

        public string RequiredInfo { get; set; }
        public bool RequireDocNo { get; set; } = false;
        public bool RequireViewForm { get; set; } = false;
        public bool RequireUpload { get; set; } = false;

        public int? PrevIdx { get; set; }
        public Guid? PrevForm { get; set; }
        public int? NextIdx { get; set; }
        public Guid? NextForm { get; set; }
        public Guid? StepId { get; set; }
        public Guid? SituationId { get; set; }
    }

    public class StepRelatedLegalDocModel
    {
        public Guid? StepId { get; set; }
        public Guid? DocId { get; set; }
        public string DocName { get; set; }
    }

    public class ViewHandlerStepStructureModel
    {
        public Guid? UnionId { get; set; }

        public string UnionName { get; set; }

        public Guid? DeptId { get; set; }

        public string DeptName { get; set; }

        public int? PositionId { get; set; }

        public string PositionName { get; set; }

        public string StaffId { get; set; }

        public string StaffName { get; set; }

        public bool AllowChangeHandler { get; set; } = false;

        public string StepsChangeHandler { get; set; }

        public bool AllowSwitchHandler { get; set; } = false;
    }

    public class ViewHandlingTimeStepStructureModel
    {
        public double HandlingTime { get; set; } = 1;

        public string PurposeIds { get; set; }

        public string PurposeNames { get; set; }
    }
}