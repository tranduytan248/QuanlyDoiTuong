using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using TSFramework.App.Attributes;

namespace Cores.Major.Models
{
    public class MajorReportModel
    {
        public Guid? ReportId { get; set; }
        public string ReportKey { get; set; }
        public string ForUser { get; set; }

        [CustomDisplayName("User_Title")] public string FullName { get; set; }

        [CustomDisplayName("User_Label_Email")]
        public string Email { get; set; }

        [CustomDisplayName("Major_Report_Title")]
        //public List<string> SelectedReports { get; set; } = new List<string>();
        public string SelectedReports { get; set; }

        public List<ListItem> ListReports { get; set; } = new List<ListItem>();

        public DataTable Reports { get; set; }
    }
}