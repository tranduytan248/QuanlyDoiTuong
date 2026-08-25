using System.Collections.Generic;
using System;
using System.Web.Mvc;
using TSFramework.App.Attributes;
using System.Web.UI.WebControls;
using Cores.Cate.Models;

namespace Modules.Cate.Areas.Cate.Models
{
    public class SearchLandCalculationModel
    {
        [CustomDisplayName("ContentLand_Label_Name")]
        public string ContentLandIds { get; set; }

        [CustomDisplayName("ContentLand_Label_Name")]
        public List<Guid> ListContentLandIds { get; set; }

        [CustomDisplayName("ContentLand_Label_Name")]
        public string ContentLandName { get; set; }

        public List<CateContentLandModel> ListContentLands { get; set; }

        //public List<SelectListItem> ListContentLands { get; set; }


        //[CustomDisplayName("ContractType_Label_Name")]
        //public List<int> ListTypeContractIds { get; set; }

        //public string TypeContractIds { get; set; }

        //public List<ListItem> ListTypeContracts { get; set; } = new List<ListItem>();
    }
}