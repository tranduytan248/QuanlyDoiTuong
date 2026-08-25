using System.Collections.Generic;
using System.Data;
using Cores.Major.Models;
using TSFramework.App.Processors;

namespace Cores.Major.Biz
{
    public class MajorReportBiz
    {
        private const string DATA_PROVIDER_NAME = "MCSProvider";
        private readonly string _majorReportGetReportViaUser = "Major_Report_GetReportViaUser";
        private readonly string _majorReportSavePermit = "Major_Report_SavePermit";

        public List<MajorReportModel> GetForUser(string forUser)
        {
            var lstReports = AppProcessor.ProcedureProvider.ExecuteTypedList<MajorReportModel>(
                _majorReportGetReportViaUser,
                DATA_PROVIDER_NAME, forUser);
            return lstReports;
        }

        public DataTable GetDataReport(string procedureName, params object[] p)
        {
            var dataReport =
                AppProcessor.ProcedureProvider.ExecuteProcedure(procedureName, true, DATA_PROVIDER_NAME, p);
            return dataReport ?? new DataTable();
        }

        public int? SavePermit(MajorReportModel model)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_majorReportSavePermit, DATA_PROVIDER_NAME,
                model.ForUser,
                model.SelectedReports);

            return result;
        }
    }
}