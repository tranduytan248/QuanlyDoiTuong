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
    public class CateCategoryModel : BaseModel
    {
        public Guid? CateId { get; set; }

        [CustomDisplayName("Category_Code")] public string CateCode { get; set; }

        [CustomDisplayName("Category_Name")]
        [CustomRequired]
        public string CateName { get; set; }

        [CustomDisplayName("Category_Type")]
        [CustomRequired]
        public int CateType { get; set; }

        [CustomDisplayName("Category_Type")] public string CateTypeName { get; set; }

        [CustomDisplayName("Category_Parent")] public Guid? CateParentId { get; set; }

        [CustomDisplayName("Category_Parent")] public string CateParentName { get; set; }

        [CustomDisplayName("Category_Priority")]
        public int Priority { get; set; } = 0;

        [CustomDisplayName("Category_Note")] public string Note { get; set; }

        public List<ListItem> ListCateTypes
        {
            get
            {
                return System.Enum.GetValues(typeof(EnumCateType))
                    .Cast<EnumCateType>()
                    .OrderBy(t => AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(t)))
                    .Select(t => new ListItem
                    {
                        Value = ((int)t).ToString(),
                        Text = AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(t))
                    }).ToList();
            }
        }

        public List<SelectListItem> ListParentCates { get; set; } = new List<SelectListItem>();

        public bool IsDeleted { get; set; }
    }
}