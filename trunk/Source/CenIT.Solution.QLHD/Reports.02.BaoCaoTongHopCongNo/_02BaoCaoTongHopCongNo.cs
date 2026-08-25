using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;
using Cores.Base.Interfaces;
using Microsoft.Reporting.WebForms;
using TSFramework.Core.Utils;

namespace Reports._02.BaoCaoTongHopCongNo
{
    public class _02BaoCaoTongHopCongNo : IReport
    {
        private string TemplateReport => "_02BaoCaoTongHopCongNo.rdlc";
        private object[] ReportParams { get; set; }
        private string DataSetName => "BaoCaoTongHopCongNo02";
        public string ReportKey { get; } = "_02BaoCaoTongHopCongNo";
        public string ReportName { get; } = "02 - Báo cáo tổng hợp - Công nợ";
        public string Description { get; } = "02 - Báo cáotổng hợp - Công nợ";
        public string ProcedureName { get; } = "p_Reports_02BaoCaoTongHopCongNo";
        public string ViewName { get; } = "_02BaoCaoTongHopCongNo";

        public ReportViewer CreateReport(DataTable data, string urlPathReport)
        {
            var fullPathRdlc = Path.Combine(urlPathReport, TemplateReport);

            var fromDate = DateTime.ParseExact(ReportParams[0] as string, "dd/MM/yyyy", CultureInfo.CurrentCulture);
            var toDate = DateTime.ParseExact(ReportParams[1] as string, "dd/MM/yyyy", CultureInfo.CurrentCulture);

            var reportFilename = $"{ReportName}_{fromDate:yyyyMMdd}_{toDate:yyyyMMdd}";

            #region Repair Report Parameter

            var listParams = new List<ReportParameter>
            {
                new ReportParameter("p_FromDate", fromDate.ToString()),
                new ReportParameter("p_ToDate", toDate.ToString()),
                new ReportParameter("p_Unions", "")
            };

            #endregion

            #region Create Report

            // Setup the report viewer object and get the array of bytes
            var reportViewer = new ReportViewer
            {
                ProcessingMode = ProcessingMode.Local,
                SizeToReportContent = true,
                ZoomMode = ZoomMode.PageWidth,
                Width = Unit.Percentage(99),
                Height = Unit.Pixel(1000),
                AsyncRendering = false,
                PageCountMode = PageCountMode.Estimate
            };

            //reportViewer.ServerReport
            reportViewer.LocalReport.ReportPath = fullPathRdlc;
            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource(DataSetName, data));
            reportViewer.LocalReport.SetParameters(listParams);
            reportViewer.LocalReport.DisplayName = reportFilename;

            #endregion

            return reportViewer;
        }

        public object[] CreateParams(NameValueCollection param)
        {
            ReportParams = new object[]
            {
                param["FromDate"],
                param["ToDate"]
            };
            return new object[]
            {
                EFormatDate.YYYYMMDD(param["FromDate"], EFormatDate.DateType.ddMMyyyy),
                EFormatDate.YYYYMMDD(param["ToDate"], EFormatDate.DateType.ddMMyyyy),
                param["Unions"]
            };
        }

        public void Export(HttpResponseBase response, DataTable data, string urlPathReport)
        {
            var fullPathRdlc = Path.Combine(urlPathReport, TemplateReport);

            var fromDate = DateTime.ParseExact(ReportParams[0] as string, "dd/MM/yyyy", CultureInfo.CurrentCulture);
            var toDate = DateTime.ParseExact(ReportParams[1] as string, "dd/MM/yyyy", CultureInfo.CurrentCulture);

            var reportFilename = $"{ReportName}_{fromDate:yyyyMMdd}_{toDate:yyyyMMdd}";

            #region Repair Report Parameter

            var listParams = new List<ReportParameter>
            {
                new ReportParameter("p_FromDate", fromDate.ToString()),
                new ReportParameter("p_ToDate", toDate.ToString()),
                new ReportParameter("p_Unions", "")
            };

            #endregion

            #region Create Report

            // Variables
            Warning[] warnings;
            string[] streamIds;
            string mimeType;
            string encoding;
            string extension;

            // Setup the report viewer object and get the array of bytes
            var reportExcel = new ReportViewer { ProcessingMode = ProcessingMode.Local };

            reportExcel.LocalReport.ReportPath = fullPathRdlc;
            reportExcel.LocalReport.DataSources.Clear();
            reportExcel.LocalReport.DataSources.Add(
                new ReportDataSource(DataSetName, data));
            reportExcel.LocalReport.SetParameters(listParams);
            reportExcel.LocalReport.DisplayName = reportFilename;

            //Chuyển sang Excel
            var bytes = reportExcel.LocalReport.Render("EXCELOPENXML", null, out mimeType, out encoding, out extension,
                out streamIds, out warnings);

            #endregion

            response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            response.AddHeader("Content-Disposition", "attachment; filename=" + reportFilename + "." + extension);
            response.BinaryWrite(bytes);
            response.Flush();
            response.End();
        }
    }
}