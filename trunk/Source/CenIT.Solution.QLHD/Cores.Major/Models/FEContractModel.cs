using System;
using System.Collections.Generic;
using Cores.Cate.Models;

namespace Cores.Major.Models
{
    public class FEContractModel
    {
        //Major_Contracts
        public Guid? ContractId { get; set; }
        public string ContractNo { get; set; }
        public int ContractTypeId { get; set; }
        public string ContractTypeName { get; set; }
        public string ContractStatusName { get; set; }
        public string ContractSignal { get; set; }
        public Guid? CusId { get; set; }
        public int PurposeId { get; set; }
        public string PurposeName { get; set; }
        public string LandParcelNo { get; set; }
        public string MapNo { get; set; }
        public int ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public int WardId { get; set; }
        public string WardName { get; set; }

        public DateTime? ReceivedOn { get; set; }
        public DateTime? ConfirmOn { get; set; }
        public double HandlingTime { get; set; }
        public DateTime? GiveResultOn { get; set; }
        public int? Status { get; set; }
        public string StatusName { get; set; }
        public int TotalRow { get; set; }

        //Major_Contracts_Customers
        public string TypeCus { get; set; }
        public string TypeCusName { get; set; }
        public string TaxCode { get; set; }
        public int Gender { get; set; }
        public int TypeIdentifier { get; set; }
        public string TypeIdentifierName { get; set; }
        public string CusName { get; set; }
        public string IdentifierNo { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string InStepName { get; set; }
    }

    public class DetailContract
    {
        public Guid? ContractId { get; set; }
        public FEContractModel ContractModel { get; set; }
        public List<FETaskModel> ListTask { get; set; }
        public List<MajorContractTaskModel> ListContractTasks { get; set; }
        public List<FEPaymentModel> ListPayment { get; set; }
        public List<CateDocModel> ListRefFiles { get; set; }
    }

    public class FETaskModel
    {
        //Major_ContractDetails
        public Guid? TaskId { get; set; }
        public Guid? ContractId { get; set; }
        public string Contents { get; set; }
        public int Amount { get; set; }
        public double Total { get; set; }
        public string Area { get; set; }
        public string Unit { get; set; }
        public double Price { get; set; }
        public double Rate { get; set; }
        public string RateFormula { get; set; }

        //Cate_LandCalculation
        public Guid? LandCalculationId { get; set; }
        public Guid? ContentLandId { get; set; }
        public string Condition { get; set; }
        public string Recipe { get; set; }
        public double Percentage { get; set; }

        //SubSection
        public int SectionId { get; set; }
        public int Cate_MainSectionId { get; set; }

        public string SubSectionName { get; set; }
        //
    }

    public class FEPaymentModel
    {
        //Major_Payment
        public Guid? PaymentId { get; set; }
        public Guid? ContractId { get; set; }
        public long PaidAmount { get; set; }
        public string RefDocNo { get; set; }
        public string PaymentInfo { get; set; }
        public DateTime? PaidOn { get; set; }
        public string Note { get; set; }
    }

    public class FERenderContractModel
    {
        public int ContractRenderFormId { get; set; }
        public Guid? ContractId { get; set; }
        public string JsonContractInfo { get; set; }
        public string FileId { get; set; }
        public string TemplatePath { get; set; }

        //Thong tin duoc lay tu mau hop dong
        public int IndexTabel { get; set; } = 2;
        public int IndexRowInTable { get; set; } = 3;

        public string ContractTypeCode { get; set; }
        public string ContractTypeName { get; set; }
        public string ContractTypeCus { get; set; }
    }
}