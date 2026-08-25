using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.UI.WebControls;
using Cores.Cate.Enum;
using TSFramework.App.Attributes;
using TSFramework.App.Models;

namespace Cores.Major.Models
{
    public class MajorContractTaskModel : BaseModel
    {
        public Guid TaskId { get; set; }

        [CustomDisplayName("Contract_Title")]
        [CustomRequired]
        public Guid ContractId { get; set; }

        //public int? TypeContractEnum { get; set; }

        public int? TypeContractId { get; set; }

        public string TypeContractName { get; set; }

        public int? Ordinal { get; set; } = 1;

        [RequiredIf("IsNotBoundaryMarkers", false)]
        [CustomDisplayName("ContractTask_Contents")]
        public int? ContentId { get; set; }

        [CustomDisplayName("ContractTask_Contents")]
        [CustomRequired]
        public string Contents { get; set; }

        public List<ListItem> ListContents { get; set; } = new List<ListItem>();

        public bool IsNotBoundaryMarkers => TypeContractId == (int)EnumContractType.MeasureDraw ||
                                            TypeContractId == (int)EnumContractType.Expertise ||
                                            TypeContractId == (int)EnumContractType.Indefinite ||
                                            TypeContractId == (int)EnumContractType.UpdateZoning;

        [CustomDisplayName("ContractTask_TypeLand")]
        [RequiredIf("IsNotBoundaryMarkers", true)]
        public int SectionId { get; set; }

        [CustomDisplayName("ContractTask_TypeLand")]
        public string TypeLandName { get; set; }

        [CustomDisplayName("ContractTask_Detail")]
        [RequiredIf("IsNotBoundaryMarkers", false)]
        public string SubSectionName { get; set; }

        [CustomDisplayName("ContractTask_Detail")]
        [CustomRequired]
        public int? SubSectionId { get; set; }

        /// <summary>
        ///     Diện tích
        /// </summary>
        [CustomDisplayName("ContractTask_Area")]
        [RequiredIf("IsNotBoundaryMarkers", true)]
        public string Area { get; set; }

        [CustomDisplayName("ContractTask_Unit")]
        //[CustomRequired]
        public string Unit { get; set; }

        /// <summary>
        ///     Đơn giá
        /// </summary>
        [CustomDisplayName("ContractTask_Price")]
        [CustomRequired]
        public long Price { get; set; }

        public string FormattedPrice { get; set; } = "0";

        /// <summary>
        ///     Khối lượng
        /// </summary>
        [CustomDisplayName("ContractTask_Amount")]
        [CustomRequired]
        [Range(1, int.MaxValue, ErrorMessage = "Giá trị phải lớn hơn 0")]
        public int Amount { get; set; } = 1;

        public string FormattedAmount { get; set; } = "0";

        [CustomDisplayName("ContractTask_Condition")]
        [CustomRequired]
        public Guid? LandCalculationId { get; set; }

        /// <summary>
        ///     Mức tính
        /// </summary>
        [CustomDisplayName("ContractTask_Rate")]
        public double Rate { get; set; }

        public double FormattedRate { get; set; } = 0;


        /// <summary>
        ///     Công thức mức tính
        /// </summary>
        //[CustomDisplayName("ContractTask_RateFormula")]
        public string RateFormula { get; set; }

        /// <summary>
        ///     Thành tiền
        /// </summary>
        [CustomDisplayName("ContractTask_Total")]
        [CustomRequired]
        public double? Total { get; set; }

        public string FormattedTotal { get; set; } = "0";

        public bool IsEdit { get; set; } = false;

        public bool IsDeleted { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        [CustomDisplayName("ContractTask_ContentLand")]
        public Guid? ContentLandId { get; set; }

        public List<ListItem> ListTypeLands { get; set; } = new List<ListItem>();
        public List<ListItem> ListAreas { get; set; } = new List<ListItem>();
        public List<ListItem> ListSubSections { get; set; } = new List<ListItem>();

        public List<ListItem> ListContentLands { get; set; } = new List<ListItem>();
        public List<ListItem> ListConditions { get; set; } = new List<ListItem>();
    }
}