using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Cores.eContract.Consts;
using Cores.Major.Enums;
using TSFramework.App.Attributes;
using TSFramework.App.Models;
using TSFramework.App.Processors;
using TSFramework.Core.Helpers;

namespace Cores.Major.Models
{
    public class MajorContractCustomerModel : BaseModel
    {
        [CustomDisplayName("Contract_Title")] public Guid ContractId { get; set; }

        [CustomDisplayName("Customer_Title")] public Guid? CusId { get; set; }

        [CustomDisplayName("Customer_TypeCus")]
        [CustomRequired]
        public string TypeCus { get; set; } = ConstsCusType.CONSUMER;

        [CustomDisplayName("Customer_TypeCus")]
        [CustomRequired]
        public string TypeCusName { get; set; } = AppProcessor.Messagor.GetMessage("CusType_Consumer");

        [CustomDisplayName("Customer_Enterprise_Name")]
        [RequiredIf("TypeCus", ConstsCusType.BUSINESS)]
        [CustomMaxLength(100)]
        public string EnterpriseName { get; set; }

        [CustomDisplayName("Customer_Name")]
        [RequiredIf("TypeCus", ConstsCusType.CONSUMER)]
        [CustomMaxLength(100)]
        public string CusName { get; set; }

        public string CusCode { get; set; }

        [CustomDisplayName("Customer_Representer_Title")]
        public string Title { get; set; }

        [CustomDisplayName("Customer_TaxCode")]
        [RequiredIf("TypeCus", ConstsCusType.BUSINESS)]
        public string TaxCode { get; set; }

        [CustomDisplayName("Customer_Gender")] public int Gender { get; set; } = 1;

        [CustomDisplayName("Customer_Gender_Alias")]
        public string GenderAlias =>
            Gender == 1
                ? "Ông"
                : "Bà";

        [CustomDisplayName("Customer_TypeIdentifier")]
        public int TypeIdentifier { get; set; } = (int)EnumTypeIdentifier.IdCard;

        [CustomDisplayName("Customer_TypeIdentifier")]
        public string TypeIdentifierName { get; set; } =
            AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(EnumTypeIdentifier.IdCard));

        [CustomDisplayName("Customer_IdentifierNo")]
        public string IdentifierNo { get; set; }

        [CustomDisplayName("Customer_Phone")]
        [CustomRequired]
        public string Phone { get; set; }

        [CustomDisplayName("Customer_Email")]
        [RequiredIf("TypeCus", "BUSINESS")]
        public string Email { get; set; }

        [CustomDisplayName("Province_Title")] public int ProvinceId { get; set; }

        [CustomDisplayName("Province_Title")] public string ProvinceName { get; set; }

        [CustomDisplayName("Ward_Title")] public int WardId { get; set; }

        [CustomDisplayName("Ward_Title")] public string WardName { get; set; }

        [CustomDisplayName("Street_Title")] public string StreetName { get; set; }

        [CustomDisplayName("Customer_AddressNo")]
        public string AddressNo { get; set; }

        [CustomDisplayName("Customer_Address")]
        [CustomRequired]
        public string Address { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsPrimary { get; set; } = true;

        public bool IsRepresenter => TypeCus == ConstsCusType.BUSINESS;

        public List<ListItem> ListProvinces { get; set; } = new List<ListItem>();
        public List<ListItem> ListWards { get; set; } = new List<ListItem>();

        public Guid? RefCus { get; set; }

        #region Representer

        [CustomDisplayName("Customer_Representer_Name")]
        [RequiredIf("TypeCus", ConstsCusType.BUSINESS)]
        public string RepresenterName { get; set; }

        public string RepresenterIdentifierNo { get; set; }

        [CustomDisplayName("Customer_Representer_Title")]
        [RequiredIf("TypeCus", ConstsCusType.BUSINESS)]
        public string RepresenterTitle { get; set; }

        [CustomDisplayName("Customer_Representer_Gender")]
        public int RepresenterGender { get; set; } = 1;

        [CustomDisplayName("Customer_Representer_Alias")]
        public string RepresenterGenderAlias =>
            RepresenterGender == 1
                ? AppProcessor.Messagor.GetMessage("CusAliasMale")
                : AppProcessor.Messagor.GetMessage("CusAliasFemale");

        #endregion
    }
}