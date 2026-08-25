using System;
using System.Collections.Generic;
using System.Web.Mvc;
using TSFramework.App.Attributes;

namespace Modules.Major.Areas.Major.Models
{
    public class ReportModel
    {
        public string ReportKey { get; set; }
        public string ReportName { get; set; }
        public string ViewName { get; set; }


        [CustomDisplayName("Union_Title")]
        public Guid? UnionId { get; set; }

        [CustomDisplayName("Union_Title")]
        public string UnionName { get; set; }

        [CustomDisplayName("Union_Title")]
        public List<Guid> Unions { get; set; }

        public List<SelectListItem> ListUnions { get; set; }
    }
}