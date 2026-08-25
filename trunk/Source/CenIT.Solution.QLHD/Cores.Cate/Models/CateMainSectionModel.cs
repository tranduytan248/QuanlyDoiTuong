using System.Collections.Generic;
using System.Web.UI.WebControls;
using TSFramework.App.Attributes;

namespace Cores.Cate.Models
{
    public class CateMainSectionModel
    {
        public List<ListItem> ListTypeContracts = new List<ListItem>();
        public int MainSectionId { get; set; }

        [CustomRequired]
        [CustomDisplayName("MainSection_Label_Name")]
        public string MainSectionName { get; set; }

        [CustomRequired]
        [CustomDisplayName("ContractType")]
        public int Cate_ContractTypeId { get; set; }

        [CustomDisplayName("ContractType")] public int ContractTypeId { get; set; }

        public string ContractTypeName { get; set; }

        public int TotalRecord { get; set; }
    }
}