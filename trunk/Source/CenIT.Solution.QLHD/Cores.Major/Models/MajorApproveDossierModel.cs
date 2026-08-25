using System;
using System.Data;
using TSFramework.App.Attributes;
using TSFramework.App.Models;

namespace Cores.Major.Models
{
    public class MajorApproveDossierModel : BaseModel
    {
        [CustomDisplayName("Dossier_Title")] public Guid? DossierId { get; set; }

        [CustomDisplayName("Dossier_Code")] public string DossierCode { get; set; }

        [CustomDisplayName("Dossier_Name")] public string DossierName { get; set; }

        [CustomDisplayName("Contract_ReceivedOn")]
        public DateTime? ReceivedOn { get; set; }

        [CustomDisplayName("Contract_ConfirmOn")]
        public DateTime? ApprovedOn { get; set; }

        [CustomDisplayName("Contract_HandleTime")]
        public double? HandleTime { get; set; }

        [CustomDisplayName("Contract_GiveResultOn")]
        public DateTime? GiveResultOn { get; set; }

        public int ContractStatus { get; set; }

        public string ContractStatusName { get; set; }

        public int Status { get; set; }

        public string StatusName { get; set; }

        public Guid? NextStepId { get; set; }

        public string NextStepName { get; set; }

        public Guid? UnionHandled { get; set; }

        public string UnionHandledName { get; set; }

        public string HandledBy { get; set; }

        public int? PositionId { get; set; }

        public double? HandlingTime { get; set; } = 0;

        public double? HandlingDossierTime { get; set; } = 0;

        public int CurrentTaskStatus { get; set; }

        public string CurrentTaskStatusName { get; set; }

        public int TaskStatus { get; set; }

        public string TaskStatusName { get; set; }

        public bool AllowSwitchHandler { get; set; } = false;

        public DataTable DataDossier { get; set; }
    }
}