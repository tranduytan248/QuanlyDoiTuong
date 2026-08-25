using System;
using System.Data;
using TSFramework.App.Attributes;
using TSFramework.App.Models;

namespace Core.Inv.Models
{
    public class MajorInvModel : BaseModel
    {
        public Guid? ContractId { get; set; }

        public Guid InvId { get; set; }

        [CustomDisplayName("Inv_InvKey")] public string InvKey { get; set; }

        [CustomDisplayName("Inv_InvNo")] public string InvNo { get; set; }

        public string Pattern { get; set; }

        public string Serial { get; set; }

        public int InvType { get; set; }

        public string InvTypeName { get; set; }

        public int InvStatus { get; set; }

        public string InvStatusName { get; set; }
        public string CusName { get; set; }
        public string CusCode { get; set; }
        public string CusTaxCode { get; set; }
        public string CusAdress { get; set; }

        public string Note { get; set; }

        public double TaxRate { get; set; }

        public long TaxAmount { get; set; }

        public long? DiscountAmount { get; set; }

        public long? DiscountNonTax { get; set; }

        public long? DiscountOther { get; set; }

        public long Total { get; set; }

        public long Amount { get; set; }

        public string AmountInWord { get; set; }

        public string CurrencyUnit { get; set; }

        public string PaymentMethod { get; set; }

        public string PublishBy { get; set; }

        public string PublishByName { get; set; }

        public DateTime? PublishOn { get; set; }

        public string ConfirmPaidBy { get; set; }

        public DateTime? PaidOn { get; set; }

        public string ErrCode { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public string CreatedByName { get; set; }

        public MajorInvCusModel InvCusInfo { get; set; }

        public DataTable DataInvCus { get; set; }

        public MajorInvProductModel InvProductInfo { get; set; }

        public DataTable DataInvProduct { get; set; }

        public string KindOfService { get; set; }

        public bool CanRepublish { get; set; } = false;
        public bool IsOldVersion { get; set; } = false;
    }

    public class MarjorViewInvModel
    {
        public Guid? InvId { get; set; }
        public string InvView { get; set; }
    }
}