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
    public class SearchCategoryModel
    {
        [CustomDisplayName("Category_Type")] public string CateTypes { get; set; }

        [CustomDisplayName("Category_Type")] public List<string> ListCates { get; set; }

        [CustomDisplayName("Category_Type")]
        public List<ListItem> ListCateTypes
        {
            get
            {
                return Enum.GetValues(typeof(EnumCateType))
                    .Cast<EnumCateType>()
                    .OrderBy(t => AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(t)))
                    .Select(t => new ListItem
                    {
                        Value = ((int)t).ToString(),
                        Text = AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(t))
                    }).ToList();
            }
        }
    }
}