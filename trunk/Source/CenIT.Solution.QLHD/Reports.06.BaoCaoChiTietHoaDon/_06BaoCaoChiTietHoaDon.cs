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

namespace Reports._06.BaoCaoChiTietHoaDon
{
    public class _06BaoCaoChiTietHoaDon : IReport
    {
        private string TemplateReport => "_06BaoCaoChiTietHoaDon.rdlc";
        private object[] ReportParams { get; set; }
        private string DataSetName => "BaoCaoChiTietHoaDon";
        public string ReportKey => "_06BaoCaoChiTietHoaDon";
        public string ReportName => "06 - Báo cáo chi tiết hoá đơn";
        public string Description => "06 - Báo cáo chi tiết hoá đơn";
        public string ProcedureName => "p_Reports_06BaoCaoChiTietHoaDon";
        public string ViewName => "_06BaoCaoChiTietHoaDon";

        public object[] CreateParams(NameValueCollection param)
        {
            ReportParams = new object[]
            {
                param["UnionName"],
                param["FromDate"],
                param["ToDate"]
            };
            return new object[]
            {
                param["UnionId"],
                EFormatDate.YYYYMMDD(param["FromDate"], EFormatDate.DateType.ddMMyyyy),
                EFormatDate.YYYYMMDD(param["ToDate"], EFormatDate.DateType.ddMMyyyy)
            };
        }

        public ReportViewer CreateReport(DataTable data, string urlPathReport)
        {
            var fullPathRdlc = Path.Combine(urlPathReport, TemplateReport);

            var unionName = ReportParams[0] as string;
            var fromDate = DateTime.ParseExact(ReportParams[1] as string, "dd/MM/yyyy", CultureInfo.CurrentCulture);
            var toDate = DateTime.ParseExact(ReportParams[2] as string, "dd/MM/yyyy", CultureInfo.CurrentCulture);

            var reportFilename = $"{ReportName}-{unionName}_{fromDate:yyyyMMdd}_{toDate:yyyyMMdd}";

            #region Repair Report Parameter

            var listParams = new List<ReportParameter>
            {
                new ReportParameter("p_UnionName", unionName),
                new ReportParameter("p_FromDate", fromDate.ToString()),
                new ReportParameter("p_ToDate", toDate.ToString())
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

        public void Export(HttpResponseBase response, DataTable data, string urlPathReport)
        {
            var fullPathRdlc = Path.Combine(urlPathReport, TemplateReport);

            var unionName = ReportParams[0] as string;
            var fromDate = DateTime.ParseExact(ReportParams[1] as string, "dd/MM/yyyy", CultureInfo.CurrentCulture);
            var toDate = DateTime.ParseExact(ReportParams[2] as string, "dd/MM/yyyy", CultureInfo.CurrentCulture);

            var reportFilename = $"{ReportName}-{unionName}_{fromDate:yyyyMMdd}_{toDate:yyyyMMdd}";

            #region Repair Report Parameter

            var listParams = new List<ReportParameter>
            {
                new ReportParameter("p_UnionName", unionName),
                new ReportParameter("p_FromDate", fromDate.ToString()),
                new ReportParameter("p_ToDate", toDate.ToString())
            };

            #endregion

            #region Create Report

            // Setup the report viewer object and get the array of bytes
            var reportExcel = new ReportViewer { ProcessingMode = ProcessingMode.Local };

            reportExcel.LocalReport.ReportPath = fullPathRdlc;
            reportExcel.LocalReport.DataSources.Clear();
            reportExcel.LocalReport.DataSources.Add(new ReportDataSource(DataSetName, data));
            reportExcel.LocalReport.SetParameters(listParams);
            reportExcel.LocalReport.DisplayName = reportFilename;

            //Chuyển sang Excel
            var bytes = reportExcel.LocalReport.Render("EXCELOPENXML", null, out _, out _, out var extension,
                out _, out _);

            #endregion

            response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            response.AddHeader("Content-Disposition", "attachment; filename=" + reportFilename + "." + extension);
            response.BinaryWrite(bytes);
            response.Flush();
            response.End();
        }
    }
}