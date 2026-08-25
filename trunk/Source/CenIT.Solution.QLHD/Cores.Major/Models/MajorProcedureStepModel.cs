using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Cores.Cate.Models;
using Cores.Major.Enums;
using Newtonsoft.Json;
using TSFramework.App.Attributes;
using TSFramework.App.Models;
using TSFramework.App.Processors;
using TSFramework.Core.Helpers;

namespace Cores.Major.Models
{
    public class MajorProcedureStepModel : BaseModel
    {
        public List<ListItem> ListProcedures { get; set; } = new List<ListItem>();

        public List<SelectListItem> ListPrevSteps { get; set; } = new List<SelectListItem>();

        public List<SelectListItem> ListNextSteps { get; set; } = new List<SelectListItem>();

        public List<CateUnionModel> ListUnionsUsingProc { get; set; } = new List<CateUnionModel>();

        #region Base

        [JsonProperty("StepId")] public Guid? StepId { get; set; }

        [JsonProperty("ProcedureId")]
        [CustomDisplayName("Procedure_Title")]
        [CustomRequired]
        public Guid? ProcedureId { get; set; }

        [JsonProperty("ProcedureName")] public string ProcedureName { get; set; }

        [JsonProperty("StepName")]
        [CustomDisplayName("Step_Name")]
        [CustomRequired]
        public string StepName { get; set; }

        [JsonProperty("StepDesc")]
        [CustomDisplayName("Step_Desc")]
        public string StepDesc { get; set; }

        [JsonProperty("StepType")]
        [CustomDisplayName("Step_TypeName")]
        public string StepType { get; set; }

        [JsonProperty("HandlingTime")]
        [CustomDisplayName("Step_HandlingTime")]
        //[CustomRequired]
        public double HandlingTime { get; set; } = 1;

        [JsonProperty("HandledBy")]
        [CustomDisplayName("Step_Handler")]
        //[CustomRequired]
        public int? HandledBy { get; set; }

        [JsonProperty("Handler")]
        [CustomDisplayName("Step_Handler")]
        public string Handler { get; set; }

        //[CustomDisplayName("Step_Handler")]
        //public List<ListItem> ListHandlers { get; set; } = new List<ListItem>();

        [CustomDisplayName("Step_Ordinal")] public int Ordinal { get; set; }

        [CustomDisplayName("Procedure_Title")] public Guid? NextProcId { get; set; }

        public string NextProcName { get; set; }

        [CustomDisplayName("Step_NextStep")] public Guid? NextStep { get; set; }

        [CustomDisplayName("Step_NextStep")] public string NextStepName { get; set; }

        [CustomDisplayName("Procedure_Title")] public Guid? PrevProcId { get; set; }

        [JsonProperty("Procedure_Title")] public string PrevProcName { get; set; }

        [CustomDisplayName("Step_PrevStep")]
        [CustomRequired]
        public Guid? PrevStep { get; set; }

        [CustomDisplayName("Step_PrevStep")] public string PrevStepName { get; set; }

        //[CustomDisplayName("Union_Using")]
        //public Guid? ProcUnionId { get; set; }

        //[CustomDisplayName("Union_Using")]
        //public string ProcUnionName { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        [RequiredIfNot("StepId", null)] public override string Reason { get; set; }

        public List<MajorProcedureStepHandlerModel> DataHandlers = new List<MajorProcedureStepHandlerModel>();
        public DataTable TableHandlers { get; set; }

        public List<MajorProcedureStepHandlingTimeModel> DataHandlingTimes =
            new List<MajorProcedureStepHandlingTimeModel>();

        public DataTable TableHandlingTimes { get; set; }

        public List<MajorProcedureStepSituationModel> DataSituations = new List<MajorProcedureStepSituationModel>();
        public DataTable TableSituations { get; set; }

        [CustomDisplayName("ContractStatus_Title")]
        [CustomRequired]
        public int? ContractStatus { get; set; } = (int)EnumContractStatus.Handling;

        public string ContractStatusName { get; set; } =
            AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(EnumContractStatus.Handling));

        public List<SelectListItem> ListContractStatus
        {
            get
            {
                return Enum.GetValues(typeof(EnumContractStatus))
                    .Cast<EnumContractStatus>()
                    .OrderBy(t => AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(t)))
                    .Select(t => new SelectListItem
                    {
                        Value = ((int)t).ToString(),
                        Text = AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(t)),
                        Group = new SelectListGroup { Name = AppProcessor.Messagor.GetMessage("ContractType_Title") }
                    }).ToList();
            }
        }

        [CustomDisplayName("Step_AttachResultFile")]
        public bool AttachResultFile { get; set; }

        public bool WarningSave { get; set; } = false;

        #endregion

        #region Notification Configs

        [CustomDisplayName("NotificationConfigs_Title")]
        public List<ListItem> NotificationConfigs { get; set; } = new List<ListItem>();

        [CustomDisplayName("Cus_NotificationConfigs_Title")]
        public string CusNotificationConfigs { get; set; }

        [CustomDisplayName("Cus_NotificationConfigs_Title")]
        public List<string> ListCusNotificationConfigs { get; set; } = new List<string>();

        public List<string> CusActiveNotifications { get; set; } = new List<string>();

        [CustomDisplayName("Staff_NotificationConfigs_Title")]
        public string StaffNotificationConfigs { get; set; }

        [CustomDisplayName("Staff_NotificationConfigs_Title")]
        public List<string> ListStaffNotificationConfigs { get; set; } = new List<string>();

        public List<string> StaffActiveNotifications { get; set; } = new List<string>();

        #endregion
    }
}