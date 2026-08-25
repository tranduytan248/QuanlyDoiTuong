using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Cores.Major.Enums;
using TSFramework.App.Attributes;
using TSFramework.App.Models;
using TSFramework.App.Processors;
using TSFramework.Core.Helpers;

namespace Cores.Major.Models
{
    public class MajorContractPaymentModel : BaseModel
    {
        public Guid? PaymentId { get; set; }

        [CustomRequired]
        [CustomDisplayName("Contract_Title")]
        public Guid? ContractId { get; set; }

        public long Ordinal { get; set; }

        [CustomDisplayName("Contract_ContractNo")]
        public string ContractNo { get; set; }

        [CustomDisplayName("Contract_Total")] public long Total { get; set; } = 0;
        public string FormatterTotal => $"{Total}";

        [CustomDisplayName("Contract_RemainingAmount")]
        public long RemainingAmount { get; set; } = 0;

        public string FormatterRemainingAmount => $"{RemainingAmount}";

        [CustomDisplayName("ContractPayment_PaidAmount")]
        [CustomRequired]
        public long PaidAmount { get; set; }

        public string FormatterPaidAmount { get; set; }

        [CustomDisplayName("ContractPayment_TotalPaidAmount")]
        public long TotalPaidAmount { get; set; }

        [CustomDisplayName("ContractPayment_TotalPaidAmount")]
        public string FormatterTotalPaidAmount => $"{TotalPaidAmount}";

        [CustomDisplayName("ContractPayment_RefDocNo")]
        //[CustomRequired]
        public string RefDocNo { get; set; }

        /// <summary>
        ///     Nội dung thanh toán: 1 - Tạm ứng; 2 - Thanh toán hết
        /// </summary>
        [CustomDisplayName("ContractPayment_TypePayment")]
        public int TypePayment { get; set; } = 1;

        public string TypePaymentName { get; set; } = AppProcessor.Messagor.GetMessage("TypePayment_Advance");

        //[CustomDisplayName("ContractPayment_PercentAdvance")]
        [CustomRequired] public double PercentAdvance { get; set; } = 50;

        public long AmountViaPercent { get; set; } = 0;

        public int RateCalc { get; set; } = 1;

        [CustomDisplayName("ContractPayment_PaymentInfo")]
        public string PaymentInfo { get; set; }

        [CustomDisplayName("ContractPayment_PaidOn")]
        [CustomRequired]
        public DateTime? PaidOn { get; set; } = DateTime.Now;

        [CustomRequired]
        [CustomDisplayName("ContractPayment_PaymentMethod")]
        public int? PaymentMethod { get; set; } = (int)EnumPaymentMethod.Cash;

        public string PaymentMethodName { get; set; } =
            AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(EnumPaymentMethod.Cash));

        public List<ListItem> ListPaymentMethods
        {
            get
            {
                return Enum.GetValues(typeof(EnumPaymentMethod))
                    .Cast<EnumPaymentMethod>()
                    .OrderBy(t => AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(t)))
                    .Select(t => new ListItem
                    {
                        Value = ((int)t).ToString(),
                        Text = AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(t))
                    }).ToList();
            }
        }

        [CustomDisplayName("ContractPayment_Status")]
        public int? Status { get; set; } = (int)EnumPaymentStatus.Received;

        [CustomDisplayName("ContractPayment_StatusName")]
        public string StatusName { get; set; } =
            AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(EnumPaymentStatus.Received));

        public string StatusCode { get; set; } = "BN";

        public List<ListItem> ListPaymentStatus
        {
            get
            {
                return Enum.GetValues(typeof(EnumPaymentStatus))
                    .Cast<EnumPaymentStatus>()
                    .OrderBy(t => AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(t)))
                    .Select(t => new ListItem
                    {
                        Value = ((int)t).ToString(),
                        Text = AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(t))
                    }).ToList();
            }
        }

        public MajorCustomerModel Customer { get; set; } = new MajorCustomerModel();

        [CustomDisplayName("ContractPayment_Note")]
        public string Note { get; set; }

        public int AutomaticallyNo { get; set; }

        [CustomDisplayName("Contract_Discount")]
        public string InfoDiscountContract { get; set; }

        //[RequiredIf("IsEdit", true)]
        [CustomRequired] public new string Reason { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsEdit { get; set; } = false;

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        #region Payment Arising

        [CustomDisplayName("ContractPayment_HasArising")]
        public bool HasArising { get; set; }

        /// <summary>
        ///     Phát sinh thanh toán: 0 - Phát sinh thêm; 1 - Miễn giảm
        /// </summary>
        [CustomDisplayName("ContractPayment_TypeArising")]
        public int? TypeArising { get; set; }

        [CustomDisplayName("ContractPayment_TypeArising")]
        public string TypeArisingName { get; set; } = AppProcessor.Messagor.GetMessage("TypeArising_Increase");

        [CustomDisplayName("ContractPayment_LiquidationAmount")]
        public long? LiquidationAmount { get; set; }

        [CustomDisplayName("ContractPayment_LiquidationAmount")]
        public string FormatterLiquidationAmount { get; set; }

        [CustomDisplayName("ContractPayment_ArisingAmount")]
        public long? ArisingAmount { get; set; } = 0;

        [CustomDisplayName("ContractPayment_ArisingAmount")]
        public string FormatterArisingAmount { get; set; } = "0";

        public bool HasTaxForContract { get; set; } = false;

        #region Discount

        [CustomDisplayName("ContractPayment_DiscountRate")]
        public double? DiscountRate { get; set; }

        [CustomDisplayName("ContractPayment_DiscountAmount")]
        public long? DiscountAmount { get; set; }

        [CustomDisplayName("ContractPayment_DiscountAmount")]
        public string FormatterDiscountAmount { get; set; }

        public string DiscountFormula { get; set; }

        #endregion

        #region Tax

        [CustomDisplayName("Contract_Tax")] public decimal? TaxRate { get; set; }

        [CustomDisplayName("Contract_Tax")] public decimal? TaxAmount { get; set; }

        public string FormatterTaxAmount { get; set; }

        [CustomDisplayName("Contract_Tax")] public string TaxInfo { get; set; }

        #endregion

        public List<ListItem> ListTypeArisings
        {
            get
            {
                return Enum.GetValues(typeof(EnumPaymentArising))
                    .Cast<EnumPaymentArising>()
                    .OrderBy(t => AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(t)))
                    .Select(t => new ListItem
                    {
                        Value = ((int)t).ToString(),
                        Text = AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(t))
                    }).ToList();
            }
        }

        public Dictionary<string, int> RealArisingValue => new Dictionary<string, int>
        {
            { $"{(int)EnumPaymentArising.Increase}", 1 },
            { $"{(int)EnumPaymentArising.Decrease}", -1 }
        };

        public long? RealArisingAmount => RealArisingValue[$"{TypeArising ?? 0}"] * ArisingAmount;

        #endregion
    }
}