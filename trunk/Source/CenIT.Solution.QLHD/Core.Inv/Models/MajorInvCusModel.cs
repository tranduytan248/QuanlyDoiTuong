using System;
using TSFramework.App.Attributes;

namespace Core.Inv.Models
{
    public class MajorInvCusModel
    {
        public Guid InvId { get; set; }

        [CustomDisplayName("Customer_Code")] public string CusCode { get; set; }

        [CustomDisplayName("Customer_Name")] public string CusName { get; set; }

        [CustomDisplayName("Buyer")] public string Buyer { get; set; }

        [CustomDisplayName("Customer_TypeCus")]
        public string TypeCus { get; set; }

        public string TypeCusName { get; set; }

        [CustomDisplayName("Customer_TaxCode")]
        public string CusTaxCode { get; set; }

        [CustomDisplayName("Customer_IdentifierNo")]
        public string CusIdentifierNo { get; set; }

        [CustomDisplayName("CustommerPhone_Search")]
        public string CusPhone { get; set; }

        [CustomDisplayName("Contract_Address")]
        public string CusAddress { get; set; }

        [CustomDisplayName("Inv_BankNo")] public string CusBankNo { get; set; }

        [CustomDisplayName("Inv_BankName")] public string CusBankName { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }
        public string InvKey { get; set; }
        public int InvStatus { get; set; }
        public string InvStatusName { get; set; }
    }
}