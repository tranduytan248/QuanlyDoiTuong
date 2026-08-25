using System.Collections.Generic;
using System.Web.UI.WebControls;
using TSFramework.App.Attributes;
using TSFramework.App.Models;

namespace Cores.Cate.Models
{
    public class CatePurPoseModel : BaseModel
    {
        public int PurPoseId { get; set; }

        [CustomRequired]
        [CustomDisplayName("PurPoseName")]
        public string PurPoseName { get; set; }

        [CustomRequired]
        [CustomDisplayName("ContractType")]
        public int ContractTypeId { get; set; }

        [CustomDisplayName("ContractTypeName")]
        public string ContractTypeName { get; set; }

        [CustomDisplayName("ContractType")]
        public List<ListItem> ListTypeContracts { get; set; } = new List<ListItem>();
    }
}