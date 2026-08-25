using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using TSFramework.App.Attributes;

namespace Modules.Major.Areas.Major.Models
{
    public class ProcedurePermitModel
    {
        public Guid? ProcedureId { get; set; }

        [CustomDisplayName("Procedure_Title")]
        public string ProcedureCode { get; set; }

        [CustomDisplayName("Procedure_Title")]
        public string ProcedureName { get; set; }

        [CustomDisplayName("Union_Title")]
        public string SelectedUnions { get; set; }

        public List<ListItem> ListUnions { get; set; } = new List<ListItem>();
    }
}