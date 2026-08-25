using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Cores.Cate.Models;
using Cores.Major.Enums;
using TSFramework.App.Attributes;
using TSFramework.App.Models;
using TSFramework.App.Processors;
using TSFramework.Core.Helpers;

namespace Cores.Major.Models
{
    public class MajorContractModel : BaseModel
    {
        public List<MajorContractPaymentModel> ListPayments = new List<MajorContractPaymentModel>();

        public List<MajorContractTaskModel> ListTasks = new List<MajorContractTaskModel>();
        public Guid? ContractId { get; set; }

        public Guid? UnionId { get; set; }

        public string UnionName { get; set; }

        //[CustomDisplayName("Union_UnionInfo")]
        //public string UnionInfo { get; set; }

        public string ContractFile { get; set; }

        [CustomDisplayName("Contract_ContractNo")]
        //[CustomRequired]
        public string ContractNo { get; set; }

        [CustomDisplayName("Contract_ContractSignal")]
        [CustomRequired]
        public string ContractSignal { get; set; }

        public string ContractNoInfo { get; set; }

        [CustomDisplayName("Contract_ContractType")]
        public int? ContractTypeId { get; set; }

        [CustomDisplayName("Contract_ExtendInfos")]
        //[CustomRequired]
        public string ExtendInfos { get; set; }

        //public int? ContractTypeEnum { get; set; }

        [CustomDisplayName("Contract_ContractType")]
        public string ContractTypeName { get; set; }

        [CustomDisplayName("Customer_Title")] public Guid? CusId { get; set; }

        [CustomDisplayName("Customer_Title")] public string CusName { get; set; }

        public string CusPhone { get; set; }
        public string CusAddress { get; set; }

        //[CustomDisplayName("Customer_TypeCus")]
        public string TypeCus { get; set; }

        //[CustomDisplayName("Customer_TypeCus")]
        public string TypeCusName { get; set; }

        [CustomDisplayName("Purpose_Title")]
        [CustomRequired]
        public int PurposeId { get; set; }

        [CustomDisplayName("Purpose_Title")] public string PurposeName { get; set; }

        [CustomDisplayName("Contract_LandParcelNo")]
        public string LandParcelNo { get; set; }

        [CustomDisplayName("Contract_MapNo")] public string MapNo { get; set; }

        [CustomDisplayName("Contract_PercentAdvance")]
        public double PercentAdvance { get; set; } = 50;

        public string FormattedPercentAdvance => $"{PercentAdvance}%";

        [CustomDisplayName("Contract_AdvanceAmount")]
        public long AdvanceAmount { get; set; } = 0;

        public string FormattedAdvanceAmount { get; set; }

        [CustomDisplayName("Contract_PeriodAdvance")]
        public int PeriodAdvance { get; set; } = 1;

        [CustomDisplayName("ContractPayment_PaymentMethod")]
        public int? PaymentMethod { get; set; } = (int)EnumPaymentMethod.Cash;

        public string PaymentMethodName { get; set; }

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

        [CustomDisplayName("Province_Title")] public int ProvinceId { get; set; }

        [CustomDisplayName("Province_Title")] public string ProvinceName { get; set; }

        [CustomDisplayName("Ward_Title")] public int WardId { get; set; }

        [CustomDisplayName("Ward_Title")] public string WardName { get; set; }

        [CustomDisplayName("Contract_Address")]
        [CustomRequired]
        public string Address { get; set; }

        [CustomDisplayName("Contract_SubTotal")]
        public long SubTotal { get; set; } = 0;

        [CustomDisplayName("Contract_SubTotal")]
        public string SubTotalFormatter { get; set; } = "0";

        [CustomDisplayName("Contract_Discount")]
        public long Discount { get; set; } = 0;

        [CustomDisplayName("Contract_Discount")]
        public string DiscountFormatter { get; set; } = "0";

        public int ExpertiseCostPercent { get; set; } = 25;

        [CustomDisplayName("Contract_ExpertiseCost")]
        public long ExpertiseCost { get; set; }

        [CustomDisplayName("Contract_ExpertiseCost")]
        public string ExpertiseCostFormatter { get; set; }

        [CustomDisplayName("Contract_Total")] public long Total { get; set; } = 0;
        [CustomDisplayName("Contract_Total")] public string TotalInFormatter { get; set; }

        public string TotalInWords { get; set; }

        [CustomDisplayName("Contract_LiquidationAmount")]
        public long LiquidationAmount { get; set; }

        public long DiscountAmount { get; set; } = 0;

        public long RemainingAmount { get; set; } = 0;

        public long TotalPaidAmount { get; set; } = 0;

        public bool NotPaidYet { get; set; } = false;

        //[CustomDisplayName("Contract_ReceivedOn")]
        public DateTime? ReceivedOn { get; set; } = DateTime.Now;

        //[CustomDisplayName("Contract_ConfirmOn")]
        public DateTime? ConfirmOn { get; set; }

        //[CustomDisplayName("Contract_HandlingTime")]
        public double HandlingTime { get; set; }

        //[CustomDisplayName("Contract_GiveResultOn")]
        public DateTime? GiveResultOn { get; set; }

        //[CustomDisplayName("Contract_CompletedOn")]
        public DateTime? ApprovedOn { get; set; }

        public int? RemainingTime { get; set; }

        public DateTime? CompletedOn { get; set; }

        [CustomDisplayName("ContractPayment_Status")]
        public int? Status { get; set; }

        public string StatusName { get; set; }

        public bool IsApproved => ApprovedOn != null;

        public bool IsDraft { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public bool IsNew { get; set; } = false;

        public bool HasInv { get; set; } = false;

        public bool IsPaid { get; set; } = false;

        public bool IncludeQRCode { get; set; } = false;

        public bool UsingUnionCode { get; set; } = false;

        public string FileType { get; set; } = ".pdf";

        public string RenderContractId { get; set; }

        [RequiredIf("IsNew", false)] public new string Reason { get; set; }

        public MajorContractCustomerModel CusInfo { get; set; } = new MajorContractCustomerModel();

        public string JsonExtendContracts { get; set; }

        //[CustomDisplayName("Contract_Tasks")]
        public DataTable DataTasks { get; set; }

        //[CustomDisplayName("Contract_Customers")]
        public DataTable DataCus { get; set; }

        //[CustomDisplayName("Contract_Dossier")]
        public DataTable DataDossier { get; set; }

        public List<ListItem> ListPurposes { get; set; } = new List<ListItem>();

        public DataTable TableRefFiles { get; set; }

        [CustomDisplayName("Contract_RefFile")]
        public List<CateDocModel> ListRefFiles { get; set; }

        [CustomDisplayName("Contract_RefFile")]
        public List<HttpPostedFileBase> RefFiles { get; set; }

        [CustomDisplayName("Contract_RejectOn")]
        public DateTime? RejectOn { get; set; }

        //Kiểm tra trễ hạn 1:Chưa trễ, -1:Đã trễ, 0:Sắp đến hạn
        public int CheckContractLate { get; set; }

        public int DelayDay { get; set; }

        public List<ListItem> ListTypePayments
        {
            get
            {
                return Enum.GetValues(typeof(EnumTypePayment))
                    .Cast<EnumTypePayment>()
                    .OrderBy(t => AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(t)))
                    .Select(t => new ListItem
                    {
                        Value = ((int)t).ToString(),
                        Text = AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(t))
                    }).ToList();
            }
        }

        // Discount function and info

        [CustomDisplayName("Contract_Discount")]
        public string FuncDiscountContract { get; set; }

        [CustomDisplayName("Contract_Discount")]
        public string InfoDiscountContract { get; set; }

        // Tax function and info

        public bool HasTaxForContract { get; set; } = false;

        public string FuncTaxContract { get; set; } = "Thuế GTGT ({0} * {1}% = {2})";

        [CustomDisplayName("Contract_Tax")] public decimal TaxRate { get; set; }

        [CustomDisplayName("Contract_Tax")] public decimal TaxAmount { get; set; }

        public string TaxInfo { get; set; }
    }

    public class MajorContractConfirmModel : BaseModel
    {
        [CustomDisplayName("Contract_Title")]
        [CustomRequired]
        public Guid? ContractId { get; set; }

        [CustomDisplayName("Contract_ContractNo")]
        [CustomRequired]
        public string ContractNo { get; set; }

        [CustomDisplayName("Contract_ContractSignal")]
        [CustomRequired]
        public string ContractSignal { get; set; }

        public string ContractNoInfo { get; set; }

        [CustomDisplayName("Customer_Title")] public string CusName { get; set; }

        [CustomDisplayName("Contract_Total")] public long Total { get; set; } = 0;
        [CustomDisplayName("Contract_Total")] public string FormattedTotal => $"{Total}";

        [CustomDisplayName("Contract_ReceivedOn")]
        [CustomRequired]
        public DateTime? ReceivedOn { get; set; }

        [CustomDisplayName("Contract_ConfirmOn")]
        //[CustomRequired]
        public DateTime? ConfirmOn { get; set; }

        [CustomDisplayName("Contract_HandleTime")]
        [CustomRequired]
        public double? HandleTime { get; set; }

        [CustomDisplayName("Contract_GiveResultOn")]
        [CustomRequired]
        public DateTime? GiveResultOn { get; set; }

        [CustomDisplayName("Contract_ConfirmOn")]
        [CustomRequired]
        public DateTime? ApprovedOn { get; set; }

        [CustomDisplayName("Contract_CompletedOn")]
        public DateTime? CompletedOn { get; set; }

        public DataTable TableRefFile { get; set; }

        [CustomDisplayName("Contract_RefFile")]
        public List<CateDocModel> ListRefFiles { get; set; }

        [CustomDisplayName("Contract_RefFile")]
        //[CustomRequired]
        public List<HttpPostedFileBase> RefFiles { get; set; }
    }

    public class MajorContractRejectModel : BaseModel
    {
        [CustomDisplayName("Contract_Title")]
        [CustomRequired]
        public Guid? ContractId { get; set; }

        [CustomDisplayName("Contract_ContractNo")]
        [CustomRequired]
        public string ContractNo { get; set; }

        [CustomDisplayName("Contract_ContractSignal")]
        [CustomRequired]
        public string ContractSignal { get; set; }

        public string ContractNoInfo { get; set; }

        public int ContractStatus { get; set; }

        public string ContractStatusName { get; set; }

        [CustomDisplayName("Customer_Title")] public string CusName { get; set; }

        [CustomDisplayName("Contract_ReceivedOn")]
        [CustomRequired]
        public DateTime? ReceivedOn { get; set; }

        [CustomDisplayName("Contract_RejectOn")]
        [CustomRequired]
        public DateTime? RejectOn { get; set; }

        public DataTable TableRefFile { get; set; }

        [CustomDisplayName("Contract_RefFile")]
        public List<CateDocModel> ListRefFiles { get; set; }

        [CustomDisplayName("Contract_RefFile")]
        //[CustomRequired]
        public List<HttpPostedFileBase> RefFiles { get; set; }
    }
}