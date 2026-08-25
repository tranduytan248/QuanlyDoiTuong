using System;
using TSFramework.App.Attributes;
using TSFramework.App.Models;

namespace Core.Inv.Models
{
    public class MajorInvProductModel : BaseModel
    {
        public Guid? ProductId { get; set; }

        [CustomDisplayName("Inv_Title")]
        [CustomRequired]
        public Guid? InvId { get; set; }

        [CustomDisplayName("InvProduct_Code")]
        [CustomRequired]
        public string ProductCode { get; set; }

        [CustomDisplayName("InvProduct_Name")]
        [CustomRequired]
        public string ProductName { get; set; }

        [CustomDisplayName("InvProduct_Unit")]
        [CustomRequired]
        public string ProductUnit { get; set; }

        [CustomDisplayName("InvProduct_Quantity")]
        [CustomRequired]
        public int ProductQuantity { get; set; } = 0;

        [CustomDisplayName("InvProduct_Price")]
        [CustomRequired]
        public long? ProductPrice { get; set; } = 0;

        [CustomDisplayName("InvProduct_DiscountRate")]
        public double? DiscountRate { get; set; }

        [CustomDisplayName("InvProduct_DiscountAmount")]
        public long? DiscountAmount { get; set; }

        [CustomDisplayName("InvProduct_TaxRate")]
        public double? TaxRate { get; set; } = 10;

        [CustomDisplayName("InvProduct_Amount")]
        [CustomRequired]
        public long? Amount { get; set; } = 0;

        [CustomDisplayName("InvProduct_Issum")]
        [CustomRequired]
        public int? Issum { get; set; } = 0;

        [CustomDisplayName("InvProduct_Issum")]
        public string IssumName { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string CreatedBy { get; set; }
        [RequiredIfNot("PatternId", null)] public new string Reason { get; set; }
    }
}