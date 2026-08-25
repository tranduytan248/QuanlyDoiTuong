using Cores.Cate.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using TSFramework.App.Attributes;
using TSFramework.App.Models;
using TSFramework.App.Processors;
using TSFramework.Core.Helpers;

namespace Modules.Cate.Areas.Cate.Models
{
    public class SearchUnionModel : BaseSearchModel
    {
        [CustomDisplayName("Union_TypeUnion")] public string TypeUnions { get; set; }

        [CustomDisplayName("Union_TypeUnion")]
        public List<ListItem> ListTypeUnions
        {
            get
            {
                return Enum.GetValues(typeof(EnumTypeUnion))
                    .Cast<EnumTypeUnion>()
                    .OrderBy(t => AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(t)))
                    .Select(t => new ListItem
                    {
                        Value = ((int)t).ToString(),
                        Text = AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(t))
                    }).ToList();
            }
        }

        [CustomDisplayName("Union_BelongUnion")]
        public string BelongUnions { get; set; }

        [CustomDisplayName("Union_BelongUnion")]
        public List<SelectListItem> ListUnions { get; set; }
    }
}