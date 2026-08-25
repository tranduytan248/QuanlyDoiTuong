using System;
using TSFramework.App.Attributes;

namespace Cores.Cate.Models
{
    public class CateContractStatusModel
    {
        public int ContractStatusId { get; set; } = 0;

        [CustomRequired]
        [CustomDisplayName("ContractStatus_Label_Code")]
        public string ContractStatusCode { get; set; }

        [CustomRequired]
        [CustomDisplayName("ContractStatus_Label_Name")]
        public string ContractStatusName { get; set; }

        [CustomDisplayName("ContractStatus_Label_Enum")]
        public int EnumId { get; set; }

        [CustomDisplayName("ContractStatus_Label_IsContract")]
        public bool IsEContract { get; set; }

        public bool IsDelete { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string LastModifiedOn { get; set; }
        public DateTime? LastModifiedBy { get; set; }
        public int? TotalRow { get; set; } = 0;
    }
}