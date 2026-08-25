using Cores.Major.Models;
using System;
using TSFramework.App.Attributes;
using TSFramework.App.Models;

namespace Modules.Major.Areas.Major.Models
{
    public class AcceptantContractModel : BaseModel
    {
        public Guid? ContractId { get; set; }

        public string ContractNoInfo { get; set; }

        public string LandParcelNo { get; set; }

        public string MapNo { get; set; }

        public string Address { get; set; }

        public string PurposeName { get; set; }

        public string TypeCusName { get; set; }

        public string CusName { get; set; }

        public MajorContractCustomerModel CusInfo { get; set; } = new MajorContractCustomerModel();

        public long? LiquidationAmount { get; set; } = 0;
                   
        public long? DiscountAmount { get; set; } = 0;

        public decimal? TaxAmount { get; set; } = 0;

        public long? TotalPaidAmount { get; set;} = 0;

        public bool HasTaxForContract { get; set; } = false;

        public string FuncTaxContract { get; set; } = "Thuế GTGT ({0} * {1}% = {2})";

        [CustomDisplayName("Contract_Tax")]
        public decimal TaxRate { get; set; }

        public string TaxInfo { get; set; }
    }
}