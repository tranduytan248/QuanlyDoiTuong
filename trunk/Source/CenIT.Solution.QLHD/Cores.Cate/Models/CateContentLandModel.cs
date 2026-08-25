using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using TSFramework.App.Attributes;
using TSFramework.App.Models;

namespace Cores.Cate.Models
{
    public class CateContentLandModel : BaseModel
    {
        public Guid ContentLandId { get; set; }

        [CustomRequired]
        [CustomDisplayName("ContractType_Label_Name")]
        public int ContractTypeId { get; set; }

        [CustomRequired]
        [CustomDisplayName("ContentLand_Label_Name")]
        public string ContentLandName { get; set; }

        [CustomDisplayName("ContractType_Label_Name")]
        public string ContractTypeName { get; set; }


        [CustomDisplayName("ContractType_Label_Name")]
        public string ContractTypeCode { get; set; }

        public string ContractTypeAndContentLand => ContentLandName + "-" + ContractTypeName;

        public List<ListItem> ListTypeContracts { get; set; } = new List<ListItem>();
    }
}