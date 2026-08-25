using Cores.Cate.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using TSFramework.App.Attributes;
using TSFramework.App.Processors;
using TSFramework.Core.Helpers;

namespace Modules.Cate.Areas.Cate.Models
{
    public class SearchPurPoseModel
    {
        public string SearchValue { get; set; }

        //public int ContractTypeId { get; set; }

        //public List<ListItem> ListTypeContracts { get; set; }

        #region Type Contract

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
                    }).ToList();
            }
        }

        #endregion

    }
}