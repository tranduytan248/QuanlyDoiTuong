using System.Collections.Generic;
using System.Web.UI.WebControls;
using TSFramework.App.Attributes;
using TSFramework.App.Models;

namespace Cores.Cate.Models
{
    public class CateContractTypeModel : BaseModel
    {
        [CustomRequired]
        [CustomDisplayName("ContractType_Title")]
        public int ContractTypeId { get; set; }

        //[CustomRequired]
        [CustomDisplayName("ContractTypeCode")]
        public string ContractTypeCode { get; set; }

        [CustomRequired]
        [CustomDisplayName("ContractTypeName")]
        public string ContractTypeName { get; set; }

        [CustomDisplayName("ContractForm")]
        [CustomRequired]
        public string FileId { get; set; }

        public string FileName { get; set; }

        [CustomDisplayName("Contract_PercentAdvance")]
        [CustomRequired]
        public double PercentAdvance { get; set; } = 0;

        public string FormattedPercentAdvance => $"{PercentAdvance}%";

        [CustomDisplayName("ContractType_ContractSignal")]
        [CustomRequired]
        public string ContractSignal { get; set; }

        //public BaseResponseModel<ResTemplateContractModel> ListTemplate { get; set; }

        public List<ListItem> ListContractTemplates { get; set; } = new List<ListItem>();

        public List<ListItem> ListTypeContracts { get; set; }

        //{
        //    get
        //    {

        //        return System.Enum.GetValues(typeof(EnumContractType))
        //            .Cast<EnumContractType>()
        //            .OrderBy(t => AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(t)))
        //            .Select(t => new ListItem
        //            {
        //                Value = ((int)t).ToString(),
        //                Text = AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(t)),
        //            }).ToList();
        //    }
        //}

        public bool IsEdit { get; set; } = false;
    }
}