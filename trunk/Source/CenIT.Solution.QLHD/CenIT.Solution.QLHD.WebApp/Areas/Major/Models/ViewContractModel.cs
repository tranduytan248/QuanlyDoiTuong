using System;

namespace Modules.Major.Areas.Major.Models
{
    public class ViewContractModel
    {
        public Guid? ContractId { get; set; }
        public string ContractNoInfo { get; set; }
        public string Address { get; set; }
        public string ContractTypeName { get; set; }
        public string CusName { get; set; }
        public string CusPhone { get; set; }
        public string CusAddress { get; set; }
        public string TypeCusName { get; set; }
        public string PurposeName { get; set; }
        public DateTime? ConfirmOn { get; set; }
        public DateTime? GiveResultOn { get; set; }
        public int? Status { get; set; }
        public string StatusColor { get; set; }
        public string StatusName { get; set; }
        public int? RemainingTime { get; set; }
    }
}