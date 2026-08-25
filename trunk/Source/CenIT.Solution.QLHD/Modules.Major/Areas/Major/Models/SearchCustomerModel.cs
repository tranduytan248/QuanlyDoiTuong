using System.Collections.Generic;
using System.Web.UI.WebControls;
using TSFramework.App.Attributes;
using TSFramework.App.Processors;
using Cores.eContract.Consts;
using TSFramework.Core.Utils;

namespace Modules.Major.Areas.Major.Models
{
    public class SearchCustomerModel
    {
        [CustomDisplayName("Customer_Search_Info")]
        public string Keyword { get; set; }

        [CustomDisplayName("Customer_Label_UserType")]
        public string TypeCus { get; set; }

        public string PrefixCus { get; set; } = string.Empty;

        public List<ListItem> ListTypeCuss =>
            new List<ListItem>
            {
                new ListItem
                {
                    Text = AppProcessor.Messagor.GetMessage($"CusType_{ConstsCusType.CONSUMER.ToLower().ToUpperFirstChar()}"),
                    Value = ConstsCusType.CONSUMER
                },
                new ListItem
                {
                    Text = AppProcessor.Messagor.GetMessage($"CusType_{ConstsCusType.BUSINESS.ToLower().ToUpperFirstChar()}"),
                    Value = ConstsCusType.BUSINESS
                }
            };
    }
}