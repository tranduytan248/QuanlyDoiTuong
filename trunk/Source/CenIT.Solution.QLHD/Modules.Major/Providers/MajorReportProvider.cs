using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web;
using Cores.Base.Interfaces;
using Cores.Base.Providers;
using Cores.Major.Caches;
using Microsoft.Reporting.WebForms;


namespace Modules.Major.Providers
{
    public class MajorReportProvider
    {
        private static readonly MajorReportCache reportApi = new MajorReportCache();

        public static List<IReport> LoadReports()
        {
            return GenericLibProvider<IReport>
                .LoadLib(ConfigurationManager.AppSettings["Report_FolderPath"] ?? "Libraries/Reports")
                .ToList();
        }

        public static void Export(IReport pReport, string currentUser, NameValueCollection formData, HttpResponseBase response, string urlPath)
        {
            var ps = pReport.CreateParams(formData);

            if (!string.IsNullOrEmpty(pReport.ProcedureName))
            {
                var lstParram = ps.ToList();
                lstParram.Insert(0, currentUser);
                var data = reportApi.GetDataReport(pReport.ProcedureName, lstParram.ToArray());
                pReport.Export(response, data, urlPath);
            }
        }

        public static ReportViewer CreateViewExport(IReport pReport, string currentUser, NameValueCollection formData, string urlPath)
        {
            var ps = pReport.CreateParams(formData);
            var lstParram = ps.ToList();
            lstParram.Insert(0, currentUser);
            var dataReport = reportApi.GetDataReport(pReport.ProcedureName, lstParram.ToArray());
            return pReport.CreateReport(dataReport, urlPath);
        }
    }
}