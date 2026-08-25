using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Cores.Cate.Enum;
using TSFramework.App.Attributes;
using TSFramework.App.Models;
using TSFramework.App.Processors;
using TSFramework.Core.Helpers;

namespace Cores.Cate.Models
{
    public class CateLandCalculationModel : BaseModel
    {
        public List<CateContentLandModel> ListContentLands = new List<CateContentLandModel>();
        public Guid LandCalculationId { get; set; }

        [CustomRequired]
        [CustomDisplayName("ContentLand_Label_Name")]
        public Guid? ContentLandId { get; set; }

        [CustomRequired]
        [CustomDisplayName("Condition_Label_Name")]
        public string Condition { get; set; }

        [CustomDisplayName("Recipe_Label_Name")]
        public string Recipe { get; set; }

        [CustomRequired]
        [CustomDisplayName("Percentage_Label_Name")]
        public double Percentage { get; set; }

        [CustomDisplayName("ContentLand_Label_Name")]
        public string ContentLandName { get; set; }

        [CustomDisplayName("ContractType_Title")]
        public int TypeContract { get; set; }

        [CustomDisplayName("ContractType_Title")]
        public string TypeContractName { get; set; }

        public List<ListItem> ListTypeContracts
        {
            get
            {
                return System.Enum.GetValues(typeof(EnumContractType))
                    .Cast<EnumContractType>()
                    .OrderBy(t => (int)t)
                    .Select(t => new ListItem
                    {
                        Value = ((int)t).ToString(),
                        Text = AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(t))
                    }).ToList();
            }
        }
    }
}