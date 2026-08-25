using System.Collections.Generic;
using System.Web.UI.WebControls;
using TSFramework.App.Attributes;

namespace Modules.Cate.Areas.Cate.Models
{
    public class SearchContentLandModel
    {
        [CustomDisplayName("ContractType_Label_Name")]
        public List<int> ListTypeContractIds { get; set; }

        //[CustomDisplayName("ContractType_Label_Name")]
        public string TypeContractIds { get; set; }

        public List<ListItem> ListTypeContracts { get; set; } = new List<ListItem>();
    }
}