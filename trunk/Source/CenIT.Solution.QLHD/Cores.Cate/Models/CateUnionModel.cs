using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Cores.Cate.Enum;
using TSFramework.App.Attributes;
using TSFramework.App.Models;
using TSFramework.App.Processors;
using TSFramework.Core.Helpers;

namespace Cores.Cate.Models
{
    public class CateUnionModel : BaseModel
    {
        public Guid? UnionId { get; set; }

        [CustomDisplayName("Union_Code")]
        [CustomRequired]
        public string UnionCode { get; set; }

        [CustomDisplayName("Union_Name")]
        [CustomRequired]
        public string UnionName { get; set; }

        [CustomDisplayName("Union_TypeUnion")]
        [CustomRequired]
        public int TypeUnion { get; set; }

        [CustomDisplayName("Union_TypeUnion")] public string TypeUnionName { get; set; }

        [CustomDisplayName("Union_BelongUnion")]
        public Guid? BelongUnion { get; set; }

        [CustomDisplayName("Union_BelongUnion")]
        public string BelongUnionName { get; set; } = string.Empty;

        [CustomDisplayName("Union_UnionInfo")] public string UnionInfo { get; set; }

        [CustomDisplayName("Union_Note")] public string Note { get; set; }

        public bool HasChildren { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        [RequiredIfNot("UnionId", null)] public new string Reason { get; set; }

        [CustomDisplayName("Union_BelongUnion")]
        public List<SelectListItem> ListUnions { get; set; } = new List<SelectListItem>();

        public List<ListItem> ListTypeUnions
        {
            get
            {
                return System.Enum.GetValues(typeof(EnumTypeUnion))
                    .Cast<EnumTypeUnion>()
                    .OrderBy(t => AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(t)))
                    .Select(t => new ListItem
                    {
                        Value = ((int)t).ToString(),
                        Text = AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(t))
                    }).ToList();
            }
        }

        public List<CateUnionModel> ListChildrens { get; set; } = new List<CateUnionModel>();
    }
}