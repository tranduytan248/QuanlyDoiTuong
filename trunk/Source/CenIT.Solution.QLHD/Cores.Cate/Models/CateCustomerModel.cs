using Cores.Sys.Models.Cate;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TSFramework.App.Attributes;
using TSFramework.App.Models;

namespace Cores.Cate.Models
{
    public class CateCustomerModel : BaseModel
    {
        public Guid? CustomerId { get; set; }

        [CustomDisplayName("Customer_Label_FullName")]
        [CustomRequired]
        public string FullName { get; set; }

        public string UserType { get; set; } = "CONSUMER";

        [CustomDisplayName("Customer_Label_Gender")]
        public bool Gender { get; set; }

        [CustomDisplayName("Customer_Label_PhoneNumber")]
        [CustomRequired]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-.●]?([0-9]{3})[-.●]?([0-9]{4})$", ErrorMessage = "Số điện thoại không đúng định dạng")]
        public string PhoneNumber { get; set; }

        [CustomDisplayName("Customer_Label_TaxCode")]
        [CustomRequired]
        public string TaxCode { get; set; }

        [CustomDisplayName("Customer_Label_Email")]
        [CustomRequired]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; }

        [CustomDisplayName("Customer_Label_Zalo")]
        public string Zalo { get; set; }

        [CustomDisplayName("Customer_Label_CitizenIdentification")]
        public string CitizenIdentification { get; set; }

        [CustomDisplayName("Position_Label_Name")]
        public string PositionName { get; set; }

        [CustomDisplayName("Union_Label_Name")]
        public string UnionName { get; set; }

        public bool IsDeleted { get; set; }

        [CustomDisplayName("Customer_Label_ApartmentNumber")]
        public string ApartmentNumber { get; set; }

        [CustomDisplayName("Customer_Label_Alley")]
        public string Alley { get; set; }

        public List<CateProvinceModel> Provinces { get; set; }
        public List<CateDistrictModel> Districts { get; set; }
        public List<CateWardModel> Wards { get; set; }
        public List<CateStreetModel> Streets { get; set; }

        [CustomDisplayName("Ward_Title")]
        [CustomRequired]
        public int? WardId { get; set; }

        [CustomDisplayName("Province_Title")]
        [CustomRequired]
        public int? ProvinceId { get; set; }

        [CustomDisplayName("Province_Title")]
        public string ProvinceName { get; set; }

        [CustomDisplayName("District_Title")]
        [CustomRequired]
        public int? DistrictId { get; set; }

        [CustomDisplayName("District_Title")] public string DistrictName { get; set; }

        [CustomDisplayName("Ward_Title")] public string WardName { get; set; }

        [CustomDisplayName("Street_Label_Name")]
        public string StreetName { get; set; }

        [CustomDisplayName("Street_Label_Name")]
        public int? StreetId { get; set; }



        [CustomDisplayName("Customer_Label_BusinessCode")]
        public string BusinessCode { get; set; }

        [CustomDisplayName("Customer_Label_PlaceGetCitizenIdentification")]
        public string PlaceGetCitizenIdentification { get; set; }

        [CustomDisplayName("Customer_Label_DateGetCitizenIdentification")]
        public DateTime? DateGetCitizenIdentification { get; set; }

        [CustomDisplayName("Customer_Label_PlaceGetBusinessCode")]
        public string PlaceGetBusinessCode { get; set; }

        [CustomDisplayName("Customer_Label_DateGetBusinessCode")]
        public DateTime? DateGetBusinessCode { get; set; }

        [CustomDisplayName("Customer_Label_PageType")]
        public int PageType { get; set; }

        [CustomRequired]
        [CustomDisplayName("Customer_Label_Address")]
        public string Address { get; set; }
    }
}
