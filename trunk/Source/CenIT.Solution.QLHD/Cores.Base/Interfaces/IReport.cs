using System.Collections.Specialized;
using System.Data;
using System.Web;
using Microsoft.Reporting.WebForms;

namespace Cores.Base.Interfaces
{
    public interface IReport
    {
        string ReportKey { get; }
        string ReportName { get; }
        string Description { get; }
        string ProcedureName { get; }
        string ViewName { get; }

        void Export(HttpResponseBase response, DataTable data, string urlPathReport);
        ReportViewer CreateReport(DataTable data, string urlPathReport);
        object[] CreateParams(NameValueCollection param);
    }
}