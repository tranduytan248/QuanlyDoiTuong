using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.UI.WebControls;
using Cores.eContract.Consts;
using Cores.Major.Enums;
using TSFramework.App.Attributes;
using TSFramework.App.Models;
using TSFramework.App.Processors;
using TSFramework.Core.Helpers;

namespace Cores.Major.Models
{
    public class MajorCustomerModel : BaseModel
    {
        public Guid? CusId { get; set; }

        [RequiredIf("TypeCus", ConstsCusType.CONSUMER)]
        [CustomDisplayName("Customer_Label_FullName")]
        [CustomMaxLength(100)]
        public string CusName { get; set; }

        [CustomRequired]
        [CustomDisplayName("Customer_TypeCus")]
        public string TypeCus { get; set; } = "CONSUMER";

        [CustomDisplayName("Customer_TypeCus")]
        public string TypeCusName { get; set; }

        [CustomDisplayName("Customer_Label_Gender")]
        public int Gender { get; set; }

        [CustomRequired]
        [CustomDisplayName("Customer_Label_PhoneNumber")]
        public string Phone { get; set; }

        [CustomDisplayName("Customer_Label_TaxCode")]
        public string TaxCode { get; set; }

        [CustomDisplayName("Customer_Label_Email")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
            ErrorMessage = "Email không đúng định dạng")]
        [RequiredIf("TypeCus", ConstsCusType.BUSINESS)]
        public string Email { get; set; }

        [CustomDisplayName("Customer_TypeIdentifier")]
        public int TypeIdentifier { get; set; } = (int)EnumTypeIdentifier.IdCard;

        [CustomDisplayName("Customer_TypeIdentifier")]
        public string TypeIdentifierName { get; set; } =
            AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(EnumTypeIdentifier.IdCard));

        [CustomDisplayName("Customer_IdentifierNo")]
        public string IdentifierNo { get; set; }

        [CustomDisplayName("Customer_Label_RefCus")]
        public Guid? RefCus { get; set; }

        public bool IsDeleted { get; set; }

        public List<ListItem> ListProvinces { get; set; } = new List<ListItem>();
        public List<ListItem> ListWards { get; set; } = new List<ListItem>();

        [CustomDisplayName("Ward_Title")] public int? WardId { get; set; }

        [CustomDisplayName("Province_Title")] public int? ProvinceId { get; set; }

        [CustomDisplayName("Province_Title")] public string ProvinceName { get; set; }

        [CustomDisplayName("Ward_Title")] public string WardName { get; set; }

        [CustomDisplayName("Street_Label_Name")]
        public string StreetName { get; set; }

        [CustomDisplayName("AddressNo_Label_AddressNo")]
        public string AddressNo { get; set; }

        [CustomRequired]
        [CustomDisplayName("Customer_Address")]
        public string Address { get; set; }


        [CustomDisplayName("Customer_Enterprise_Name")]
        [RequiredIf("TypeCus", ConstsCusType.BUSINESS)]
        public string EnterpriseName { get; set; }


        [CustomDisplayName("Customer_Representer_Name")]
        [RequiredIf("TypeCus", ConstsCusType.BUSINESS)]
        public string RepresenterName { get; set; }

        [CustomDisplayName("Customer_Representer_Gender")]
        public int RepresenterGender { get; set; } = 1;

        [CustomDisplayName("Customer_Representer_Title")]
        [RequiredIf("TypeCus", ConstsCusType.BUSINESS)]
        public string RepresenterTitle { get; set; }
    }
}