using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using TSFramework.App.Attributes;
using TSFramework.App.Models;

namespace Cores.Major.Models
{
    public class MajorProcedureModel : BaseModel
    {
        public Guid? ProcedureId { get; set; }

        [CustomDisplayName("Procedure_Code")]
        [CustomRequired]
        public string ProcedureCode { get; set; }

        [CustomDisplayName("Procedure_Name")]
        [CustomRequired]
        public string ProcedureName { get; set; }

        [CustomDisplayName("Procedure_Desc")] public string ProcedureDesc { get; set; }

        [CustomDisplayName("Procedure_ApplyFrom")]
        //[CustomRequired]
        public DateTime? ApplyFrom { get; set; } = DateTime.Now;

        [CustomDisplayName("Procedure_ExpiredOn")]
        //[CustomRequired]
        public DateTime? ExpiredOn { get; set; } = DateTime.Now;

        [CustomDisplayName("Procedure_Version")]
        public int Version { get; set; } = 1;

        [CustomDisplayName("Procedure_Status")]
        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; }

        public bool IsClone { get; set; }


        [RequiredIfNot("ProcedureId", null)] public new string Reason { get; set; }

        public string StepsTreeView { get; set; }

        [CustomDisplayName("Procedure_ContractType")]
        [CustomRequired]
        public int? ContractTypeId { get; set; }

        [CustomDisplayName("Procedure_ContractType")]
        public string ContractTypeName { get; set; }

        public string Unions { get; set; }

        [CustomDisplayName("Union_Using")] public Guid? UnionUsing { get; set; }

        [CustomDisplayName("Union_Using")] public string UnionUsingName { get; set; }

        [CustomRequired]
        [CustomDisplayName("Union_Using")]
        public List<Guid> UnionIds { get; set; }

        [CustomDisplayName("Union_Using")] public string SelectedUnions { get; set; }

        public List<SelectListItem> ListUnions { get; set; } = new List<SelectListItem>();

        public List<ListItem> ListContractTypes { get; set; } = new List<ListItem>();

        //public List<ListItem> ListContractTypes
        //{
        //    get
        //    {
        //        return Enum.GetValues(typeof(EnumContractType))
        //            .Cast<EnumContractType>()
        //            .OrderBy(t => AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(t)))
        //            .Select(t => new ListItem
        //            {
        //                Value = ((int)t).ToString(),
        //                Text = AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(t))
        //            }).ToList();
        //    }
        //}
    }
}