using Cores.Cate.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using TSFramework.App.Attributes;
using TSFramework.App.Processors;
using TSFramework.Core.Helpers;

namespace Modules.Major.Areas.Major.Models
{
    public class SearchProcedureModel
    {
        //[CustomDisplayName("Cate_ProcedureType")]
        //public List<Guid> ListTypeProcedureIds { get; set; }

        //public string TypeProcedures { get; set; }

        //[CustomDisplayName("Cate_ProcedureType")]
        //public List<SelectListItem> ListProcedureTypes { get; set; } = new List<SelectListItem>();

        [CustomDisplayName("Union_Using")]
        public List<Guid> ListUnionIds { get; set; }

        public string UnionIds { get; set; }

        [CustomDisplayName("Union_Using")]
        public List<SelectListItem> ListUnions { get; set; } = new List<SelectListItem>();

        [CustomDisplayName("ContractType_Title")]
        public List<int> ListTypeContractIds { get; set; }

        public string TypeContractIds { get; set; }

        public List<ListItem> ListTypeContracts
        {
            get
            {
                return Enum.GetValues(typeof(EnumContractType))
                    .Cast<EnumContractType>()
                    .OrderBy(t => (int)t)
                    .Select(t => new ListItem
                    {
                        Value = ((int)t).ToString(),
                        Text = AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(t)),
                        //Group = new SelectListGroup { Name = AppProcessor.Messagor.GetMessage("ContractType_Title") }
                    }).ToList();
            }
        } 
    }
}