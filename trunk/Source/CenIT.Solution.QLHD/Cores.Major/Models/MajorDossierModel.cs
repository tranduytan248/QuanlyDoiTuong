using System;
using System.Collections.Generic;
using System.Data;
using TSFramework.App.Attributes;
using TSFramework.App.Models;

namespace Cores.Major.Models
{
    public class MajorDossierModel : BaseModel
    {
        [CustomDisplayName("Dossier_Title")] public Guid? DossierId { get; set; }

        [CustomDisplayName("Dossier_Code")] public string DossierCode { get; set; }

        [CustomDisplayName("Dossier_Name")] public string DossierName { get; set; }

        [CustomDisplayName("Procedure_Using")] public Guid? ProcedureId { get; set; }

        [CustomDisplayName("Procedure_Using")] public string ProcedureName { get; set; }

        public string ProcConfigs { get; set; }

        [CustomDisplayName("Dossier_HandlingTime")]
        public double TotalHandlingTime { get; set; } = 0;

        public Guid? PrevStep { get; set; }

        public string PrevStepName { get; set; }

        public Guid? TaskId { get; set; }

        public Guid? PrevTask { get; set; }

        public Guid? InStep { get; set; }

        public string InStepName { get; set; }

        public Guid? UnionHandled { get; set; }

        public string UnionHandledName { get; set; }

        public string UnionName { get; set; }

        public string HandledBy { get; set; }

        public double? HandlingTime { get; set; } = 0;

        public int? PositionId { get; set; }

        public int Status { get; set; }

        public string StatusName { get; set; }

        public int TaskStatus { get; set; }

        public string TaskStatusName { get; set; }

        public new bool CanEdit { get; set; }

        public bool AllowSwitchHandler { get; set; } = false;

        public bool SwitchedHandler { get; set; } = false;

        public string ContractNoInfo { get; set; }

        [CustomDisplayName("Contract_ContractType")]
        public int? ContractTypeId { get; set; }

        [CustomDisplayName("Contract_ContractType")]
        public string ContractTypeName { get; set; }

        [CustomDisplayName("Customer_Title")] public Guid? CusId { get; set; }

        [CustomDisplayName("Customer_Title")] public string CusName { get; set; }

        //[CustomDisplayName("Customer_TypeCus")]
        public string TypeCus { get; set; }

        //[CustomDisplayName("Customer_TypeCus")]
        public string TypeCusName { get; set; }

        public string Address { get; set; }

        [CustomDisplayName("Contract_LandParcelNo")]
        public string LandParcelNo { get; set; }

        [CustomDisplayName("Contract_MapNo")] public string MapNo { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ActionType { get; set; }

        [RequiredIf("ActionType", "DELETE")] public new string Reason { get; set; }

        public List<MajorDossierTaskModel> ListTasks { get; set; }

        public DataTable TableRefFiles { get; set; }

        public DateTime? ReceivedOn { get; set; }

        public DateTime? ConfirmOn { get; set; }

        public DateTime? ApprovedOn { get; set; }

        public DateTime? GiveResultOn { get; set; }

        //Kiểm tra trễ hạn 1:Chưa trễ, -1:Đã trễ, 0:Sắp đến hạn
        public int CheckContractLate { get; set; }

        public int DelayDay { get; set; }

        #region Handle Permit

        public int HandlePermit { get; set; } = 1;

        public bool Handle => 2 == HandlePermit;

        public bool View => 1 == HandlePermit;

        public bool Supervisor => 3 == HandlePermit;

        #endregion
    }
}