using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Cores.Base.Interfaces;
using FastMember;
using Microsoft.Reporting.WebForms;
using TSFramework.Core.Providers;
using TSFramework.Core.Utils;

namespace Reports._04.BaoCaoTongHopDoDac
{
    public class _04BaoCaoTongHopDoDac : IReport
    {
        private string TemplateReport => "_04BaoCaoTongHopDoDac.rdlc";
        private object[] ReportParams { get; set; }
        private string DataSetName => "BaoCaoTongHopDoDac04";
        public string ReportKey => "_04BaoCaoTongHopDoDac";
        public string ReportName => "04 - Báo cáo tổng hợp - Đo đạc";
        public string Description => "04 - Báo cáotổng hợp - Đo đạc";
        public string ProcedureName => "p_Reports_04TongHopDoDac";
        public string ViewName => "_04BaoCaoTongHopDoDac";

        public ReportViewer CreateReport(DataTable data, string urlPathReport)
        {
            var dataReport = ProcessData(data);
            var fullPathRdlc = Path.Combine(urlPathReport, TemplateReport);

            var fromDate = DateTime.ParseExact(ReportParams[0] as string, "dd/MM/yyyy", CultureInfo.CurrentCulture);
            var toDate = DateTime.ParseExact(ReportParams[1] as string, "dd/MM/yyyy", CultureInfo.CurrentCulture);

            var reportFilename = $"{ReportName}_{fromDate:yyyyMMdd}_{toDate:yyyyMMdd}";

            #region Repair Report Parameter

            var listParams = new List<ReportParameter>
            {
                //new ReportParameter("p_ForYear", forYear.ToString()),
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
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource(DataSetName, dataReport));
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
            var dataReport = ProcessData(data);
            var fullPathRdlc = Path.Combine(urlPathReport, TemplateReport);

            var fromDate = DateTime.ParseExact(ReportParams[0] as string, "dd/MM/yyyy", CultureInfo.CurrentCulture);
            var toDate = DateTime.ParseExact(ReportParams[1] as string, "dd/MM/yyyy", CultureInfo.CurrentCulture);

            var reportFilename = $"{ReportName}_{fromDate:yyyyMMdd}_{toDate:yyyyMMdd}";

            #region Repair Report Parameter

            var listParams = new List<ReportParameter>
            {
                //new ReportParameter("p_ForYear", forYear.ToString()),
                new ReportParameter("p_FromDate", fromDate.ToString()),
                new ReportParameter("p_ToDate", toDate.ToString()),
                new ReportParameter("p_Unions", "")
            };

            #endregion

            #region Create Report

            // Variables

            // Setup the report viewer object and get the array of bytes
            var reportExcel = new ReportViewer { ProcessingMode = ProcessingMode.Local };

            reportExcel.LocalReport.ReportPath = fullPathRdlc;
            reportExcel.LocalReport.DataSources.Clear();
            reportExcel.LocalReport.DataSources.Add(new ReportDataSource(DataSetName, dataReport));
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

        private DataTable ProcessData(DataTable data)
        {
            var reportData = new DataTable();
            reportData.Columns.AddRange(new[]
            {
                new DataColumn("UnionName"),
                new DataColumn("ContractId"),
                new DataColumn("ContractNoInfo"),
                new DataColumn("ConfirmOn"),
                new DataColumn("CompletedOn"),
                new DataColumn("CusName"),
                new DataColumn("PurposeName"),
                new DataColumn("LandParcelNo"),
                new DataColumn("MapNo"),
                new DataColumn("Address"),
                new DataColumn("WardName"),
                new DataColumn("ProvinceName"),
                new DataColumn("MeasureHandler"),
                new DataColumn("MeasureDept"),
                new DataColumn("MeasureCompletedOn"),
                new DataColumn("ExpertiseHandler"),
                new DataColumn("ExpertiseCompletedOn"),
                new DataColumn("SignCreatedOn"),
                new DataColumn("SignCompletedOn"),
                new DataColumn("StatusName")
            });

            if (data == null || data.Rows.Count <= 0) return reportData;
            var lstTasks = ModelProvider.CreateListFromTable<MeasureReportModel>(data);
            if (lstTasks == null || lstTasks.Count <= 0) return reportData;

            reportData = new DataTable();

            var lstReportItems = lstTasks.GroupBy(t => new
            {
                t.UnionName,
                t.TypeContract,
                t.ContractId,
                t.ContractNo,
                t.ContractNoInfo,
                t.ConfirmOn,
                t.CompletedOn,
                t.CusName,
                t.PurposeName,
                t.LandParcelNo,
                t.MapNo,
                t.Address,
                t.WardName,
                t.ProvinceName,
                t.StatusName
            }).Select(g => new
            {
                g.Key.UnionName,
                g.Key.ContractId,
                g.Key.ContractNo,
                g.Key.ContractNoInfo,
                g.Key.ConfirmOn,
                g.Key.CompletedOn,
                g.Key.CusName,
                g.Key.PurposeName,
                g.Key.LandParcelNo,
                g.Key.MapNo,
                g.Key.Address,
                g.Key.WardName,
                g.Key.ProvinceName,
                g.Key.StatusName,
                MeasureHandler = g.FirstOrDefault(t => t.TaskIdx == 1)?.Handler,
                MeasureDept = g.FirstOrDefault(t => t.TaskIdx == 1)?.DeptHandle,
                MeasureCompletedOn = g.Key.TypeContract == null
                    ? g.FirstOrDefault(t => t.TaskIdx == 1)?.TaskCompletedOn
                    : g.FirstOrDefault(t => t.TaskIdx == 1)?.TaskCompletedOn, // Ngày chuyển KCS
                ExpertiseHandler = g.Key.TypeContract == null
                    ? g.FirstOrDefault(t => t.TaskIdx == 2)?.Handler
                    : g.FirstOrDefault(t => t.TaskIdx == 2)?.Handler, // KCS
                ExpertiseCompletedOn = g.Key.TypeContract == null
                    ? g.FirstOrDefault(t => t.TaskIdx == 3)?.TaskCreatedOn
                    : g.FirstOrDefault(t => t.TaskIdx == 3)?.TaskCreatedOn, // Ngày chuyển cập nhật CSDL
                SignCreatedOn = g.FirstOrDefault(t => t.TaskIdx == 3)?.TaskCompletedOn, // Ngày hoàn thành cập nhật CSDL
                SignCompletedOn = g.FirstOrDefault(t => t.TaskIdx == 4)?.TaskCompletedOn // Ngày ký
            }).OrderBy(g => int.Parse(g.ContractNo.Split(g.ContractNo.Contains("/") ? '/' : '-')[0])).ToList();


            using (var reader = ObjectReader.Create(lstReportItems, "UnionName", "ContractId", "ContractNoInfo",
                       "ConfirmOn", "CompletedOn", "CusName", "PurposeName", "LandParcelNo", "MapNo", "Address",
                       "WardName", "ProvinceName", "MeasureHandler", "MeasureDept", "MeasureCompletedOn",
                       "ExpertiseHandler", "ExpertiseCompletedOn", "SignCreatedOn", "SignCompletedOn", "StatusName"))
            {
                reportData.Load(reader);
            }

            return reportData;
        }
    }

    public class MeasureReportModel
    {
        public string UnionName { get; set; }
        public Guid ContractId { get; set; }
        public string ContractNo { get; set; }
        public string ContractNoInfo { get; set; }
        public DateTime ConfirmOn { get; set; }
        public DateTime? CompletedOn { get; set; }
        public string CusName { get; set; }
        public string PurposeName { get; set; }
        public string LandParcelNo { get; set; }
        public string MapNo { get; set; }
        public string Address { get; set; }
        public string WardName { get; set; }
        public string ProvinceName { get; set; }
        public int TaskIdx { get; set; }
        public string TaskName { get; set; }
        public int? TypeContract { get; set; }
        public string HandledBy { get; set; }
        public string Handler { get; set; }
        public string DeptHandle { get; set; }
        public DateTime? TaskCreatedOn { get; set; }
        public DateTime? TaskCompletedOn { get; set; }
        public string StatusName { get; set; }
    }
}