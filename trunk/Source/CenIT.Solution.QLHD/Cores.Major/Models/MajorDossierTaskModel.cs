using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Cores.Cate.Models;
using TSFramework.App.Attributes;
using TSFramework.App.Models;

namespace Cores.Major.Models
{
    public class MajorDossierTaskModel : BaseModel
    {
        public List<MajorProcedureStepModel> ListProcSteps = new List<MajorProcedureStepModel>();


        public List<ViewStepStructureModel> ListSteps = new List<ViewStepStructureModel>();

        [CustomDisplayName("Task_Title")] public Guid TaskId { get; set; }

        public int Ordinal { get; set; }

        [CustomDisplayName("Dossier_Title")]
        [CustomRequired]
        public Guid DossierId { get; set; }

        [CustomDisplayName("Dossier_Title")] public string DossierName { get; set; }

        [CustomDisplayName("Dossier_Title")] public string DossierCode { get; set; }

        [CustomDisplayName("Procedure_Title")] public Guid? ProcedureId { get; set; }

        [CustomDisplayName("Procedure_Title")] public string ProcedureName { get; set; }

        public Guid? ProcUnionId { get; set; }

        [CustomDisplayName("Step_PrevStep")] public Guid? PrevStep { get; set; }

        [CustomDisplayName("Step_PrevStep")] public string PrevStepName { get; set; }

        [CustomDisplayName("Step_PrevStep")] public ViewStepStructureModel PrevStepView { get; set; }

        [CustomDisplayName("Step_NextStep")] public Guid? NextStep { get; set; }

        [CustomDisplayName("Step_NextStep")] public string NextStepName { get; set; }

        [CustomDisplayName("Step_NextStep_Handler")]
        public string NextStepHandler { get; set; }

        [CustomDisplayName("Step_NextStep_Handler")]
        public string NextStepHandlerName { get; set; }

        public List<CateUnionMemberModel> ListNextStepHandlers { get; set; } = new List<CateUnionMemberModel>();

        public Guid? PrevTask { get; set; }

        [CustomDisplayName("Step_Title")]
        [CustomRequired]
        public Guid InStep { get; set; }

        [CustomDisplayName("Task_Title")] public string InStepName { get; set; }

        public Guid? UnionHandle { get; set; }

        public string UnionHandleName { get; set; }

        [CustomDisplayName("Task_HandledBy")] public string HandledBy { get; set; }

        public string HandledByName { get; set; }

        public int? PositionId { get; set; }

        [CustomDisplayName("Step_HandlingTime")]
        [CustomRequired]
        public double HandlingTime { get; set; } = 0;

        [CustomDisplayName("Task_Status")] public int Status { get; set; }

        [CustomDisplayName("Task_Status")] public string StatusName { get; set; }

        [CustomDisplayName("Contract_Status")] public int ContractStatus { get; set; }

        [CustomDisplayName("Contract_Status")] public string ContractStatusName { get; set; }

        public int NextStatus { get; set; }

        [CustomDisplayName("Task_Status")] public string NextStatusName { get; set; }

        [CustomDisplayName("Task_StartHandleOn")]
        public DateTime? StartHandleOn { get; set; }

        [CustomDisplayName("Task_CompletedOn")]
        public DateTime? CompletedOn { get; set; }

        public bool AllowChangeHandler { get; set; } = false;

        public string StepsChangeHandler { get; set; }

        [CustomDisplayName("Task_HandleResult")]
        public string HandlingResult { get; set; }

        public string Note { get; set; }

        public DataTable TableRefFiles { get; set; }

        [CustomDisplayName("Contract_RefFile")]
        public List<CateDocModel> ListRefFiles { get; set; }

        [CustomDisplayName("Task_ResultFile")] public List<HttpPostedFileBase> ResultFiles { get; set; } = null;

        public bool AttachResultFile { get; set; } = true;

        public ViewStepStructureModel InStepView { get; set; }

        [CustomDisplayName("Step_Situation_Title")]
        public Guid? SelectedSituation { get; set; }

        public string SelectedSituationName { get; set; }

        public bool IsRollbackPrev { get; set; } = false;

        public bool IsFinish { get; set; } = false;

        public bool AllowSwitchHandler { get; set; } = false;

        public bool SwitchedHandler { get; set; } = false;

        public Dictionary<string, StepHandlerModel> StepHandlers { get; set; }

        public List<MajorStepChangeHandleModel> ListStepsChangeHandler { get; set; } =
            new List<MajorStepChangeHandleModel>();

        public List<SelectListItem> ListUnions { get; set; } = new List<SelectListItem>();

        public List<CateUnionMemberModel> ListStaffs { get; set; } = new List<CateUnionMemberModel>();

        [CustomDisplayName("Position_Title")] public List<ListItem> ListPositions { get; set; } = new List<ListItem>();

        public Dictionary<int, List<CateDocModel>> ListRefImgs { get; set; }

        public string RefDocs { get; set; }

        public string Supporters { get; set; }

        public string PausedLogs { get; set; }

        public string ReasonPaused { get; set; }

        public bool? IsPause { get; set; }

        [RequiredIfNot("IsPause", null)] public new string Reason { get; set; }
    }

    public class StepHandlerModel
    {
        public Guid UnionHandle { get; set; }
        public string UnionHandleName { get; set; }

        [CustomDisplayName("Task_UnionHandle")]
        [CustomRequired]
        public Guid DeptHandle { get; set; }

        public string DeptHandleName { get; set; }

        [CustomDisplayName("Task_HandledBy")]
        [CustomRequired]
        public string HandledBy { get; set; }

        public string HandledByName { get; set; }
        public bool AllowSwitchHandler { get; set; } = false;
    }

    public class MajorDossierSwitchHandlerTaskModel
    {
        public List<(string StaffName, string StaffId, bool IsPrimary)> DataHandlers =
            new List<(string StaffName, string StaffId, bool IsPrimary)>();

        public List<MajorDossierTaskSwitchHandlerModel> DataTaskHandlers =
            new List<MajorDossierTaskSwitchHandlerModel>();

        [CustomDisplayName("Task_Title")] public Guid TaskId { get; set; }

        [CustomDisplayName("Task_Title")] public string TaskName { get; set; }

        [CustomDisplayName("Dossier_Title")] public string DossierName { get; set; }

        public string DossierCode { get; set; }

        public string InStepName { get; set; }

        public string HandelBy { get; set; }

        public string HandelByName { get; set; }


        [CustomDisplayName("Task_HandlingComments")]
        public string HandlingComments { get; set; }
    }
}