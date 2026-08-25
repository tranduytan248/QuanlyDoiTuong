using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;
using Cores.Major.Caches;
using Cores.Major.Enums;
using Cores.Sys.Caches.Sys;
using Newtonsoft.Json;
using Quartz;
using TSFramework.App.Models;
using TSFramework.App.Processors;
using TSFramework.Core.Members.Job;

namespace CenIT.Solution.QLHD.Jobs.CalcRemainingDays
{
    public class ExecuteJob : JobModel
    {
        private const string JobName = "Jobs.CalcRemainingDays";
        private readonly MajorContractCache _contractCache = new MajorContractCache();
        private readonly SysJobCache _jobCache = new SysJobCache();

        public override void Execute(IJobExecutionContext context)
        {
            var logBuilder = new StringBuilder();

            try
            {
                #region Get Job Info And Parrams

                logBuilder.AppendLine("");
                logBuilder.AppendLine("============================================================");
                logBuilder.AppendLine($"=========== {JobName} =======");
                logBuilder.AppendLine("=================Lấy thông tin Job và tham số===============");
                var jobId = context.JobDetail.Key.Name;

                var jobModel = _jobCache.GetById(jobId);
                if (jobModel == null)
                {
                    logBuilder.AppendLine($"    => Dữ liệu Job [{jobId} - {JobName}] không tồn tại");
                    AppProcessor.JobLogger.Message(JobName, logBuilder.ToString());
                    return;
                }

                var jobParams = jobModel.JobParrams;
                if (string.IsNullOrEmpty(jobParams))
                {
                    logBuilder.AppendLine($"    => Tham số Job [{jobId} - {JobName}] không tồn tại");
                    AppProcessor.JobLogger.Message(JobName, logBuilder.ToString());
                    return;
                }

                var dictJobParrams = JsonConvert.DeserializeObject<Dictionary<string, string>>(jobParams);

                var procCalcRemainingDays = dictJobParrams["Procedure_Calc_Remaining_Days"];
                if (string.IsNullOrEmpty(procCalcRemainingDays))
                {
                    logBuilder.AppendLine("    => Tham số [Procedure_Calc_Remaining_Days] không tồn tại");
                    AppProcessor.JobLogger.Message(JobName, logBuilder.ToString());
                    return;
                }

                logBuilder.AppendLine($"    - Tham số Job [Procedure_Calc_Remaining_Days]: {procCalcRemainingDays}");

                var supervisorUser = dictJobParrams["Supervisor_UserName"];
                if (string.IsNullOrEmpty(supervisorUser))
                {
                    logBuilder.AppendLine("    => Tham số [Supervisor_UserName] không tồn tại");
                    AppProcessor.JobLogger.Message(JobName, logBuilder.ToString());
                    return;
                }

                logBuilder.AppendLine($"    - Tham số Job [Supervisor_UserName]: {supervisorUser}");

                var dataProviderName = dictJobParrams["Data_Provider_Name"];
                if (string.IsNullOrEmpty(dataProviderName))
                {
                    logBuilder.AppendLine("    => Tham số [Data_Provider_Name] không tồn tại");
                    AppProcessor.JobLogger.Message(JobName, logBuilder.ToString());
                    return;
                }

                logBuilder.AppendLine($"    - Tham số Job [Data_Provider_Name]: {dataProviderName}");

                #endregion

                _contractCache.Get(out var iTotalContracts, username: supervisorUser,
                    contractStatus: $"{(int)EnumContractStatus.Handling}",
                    search: new BaseSearchModel { PageSize = 10, StartIndex = 0 });

                logBuilder.AppendLine(
                    $"    - Thực thi procedure tính ngày thực hiện còn lại: [{iTotalContracts} dòng]");

                #region Create Queue Tasks And Run

                var iTotalFetchRows = 100;

                var queueTasks = new Queue<Task>();
                for (var iTotalSkipRows = 0; iTotalContracts > 0; iTotalSkipRows += iTotalFetchRows)
                {
                    var rows = iTotalSkipRows;
                    var total = iTotalContracts;
                    iTotalContracts -= iTotalContracts > iTotalFetchRows ? iTotalFetchRows : iTotalContracts;

                    queueTasks.Enqueue(new Task(() =>
                    {
                        var resultExeceProc = AppProcessor.ProcedureProvider.Execute(procCalcRemainingDays, true,
                            dataProviderName, rows, iTotalFetchRows);

                        logBuilder.AppendLine(
                            $"        + Thực thi procedure [Skip: {rows}] - [còn lại: {total}] => {resultExeceProc}");
                    }));
                }

                Task.Factory.StartNew(() =>
                {
                    while (queueTasks.Count > 0)
                    {
                        var queueTask = queueTasks.Dequeue();
                        queueTask.Start();
                        while (!queueTask.IsCompleted)
                        {
                        }
                    }

                    AppProcessor.JobLogger.Message(JobName, logBuilder.ToString());
                });

                #endregion
            }
            catch (Exception ex)
            {
                logBuilder.AppendLine("         => Lỗi");
                AppProcessor.JobLogger.Message(JobName, logBuilder.ToString());
                AppProcessor.JobLogger.Error(JobName, ex);
            }
        }

        public override void ExecuteNow(object data)
        {
            #region Get Job Info And Parrams

            var logBuilder = new StringBuilder();
            logBuilder.AppendLine("");
            logBuilder.AppendLine("============================================================");
            logBuilder.AppendLine($"=========== {JobName} =======");
            logBuilder.AppendLine("=================Lấy thông tin Job và tham số===============");

            var jobParams = data as NameValueCollection;
            if (jobParams == null || jobParams.Count <= 0)
            {
                logBuilder.AppendLine($"    => Tham số Job [{JobName}] không tồn tại");
                AppProcessor.JobLogger.Message(JobName, logBuilder.ToString());
                return;
            }

            var dictJobParrams = jobParams; //JsonConvert.DeserializeObject<Dictionary<string, string>>(jobParams);

            var procCalcRemainingDays = dictJobParrams["Procedure_Calc_Remaining_Days"];
            if (string.IsNullOrEmpty(procCalcRemainingDays))
            {
                logBuilder.AppendLine("    => Tham số [Procedure_Calc_Remaining_Days] không tồn tại");
                AppProcessor.JobLogger.Message(JobName, logBuilder.ToString());
                return;
            }

            logBuilder.AppendLine($"    - Tham số Job [Procedure_Calc_Remaining_Days]: {procCalcRemainingDays}");

            var supervisorUser = dictJobParrams["Supervisor_UserName"];
            if (string.IsNullOrEmpty(supervisorUser))
            {
                logBuilder.AppendLine("    => Tham số [Supervisor_UserName] không tồn tại");
                AppProcessor.JobLogger.Message(JobName, logBuilder.ToString());
                return;
            }

            logBuilder.AppendLine($"    - Tham số Job [Supervisor_UserName]: {supervisorUser}");

            var dataProviderName = dictJobParrams["Data_Provider_Name"];
            if (string.IsNullOrEmpty(dataProviderName))
            {
                logBuilder.AppendLine("    => Tham số [Data_Provider_Name] không tồn tại");
                AppProcessor.JobLogger.Message(JobName, logBuilder.ToString());
                return;
            }

            logBuilder.AppendLine($"    - Tham số Job [Data_Provider_Name]: {dataProviderName}");

            #endregion

            try
            {
                _contractCache.Get(out var iTotalContracts, username: supervisorUser,
                    contractStatus: $"{(int)EnumContractStatus.Handling}",
                    search: new BaseSearchModel { PageSize = 10, StartIndex = 0 });

                logBuilder.AppendLine(
                    $"    - Thực thi procedure tính ngày thực hiện còn lại: [{iTotalContracts} dòng]");

                #region Create Queue Tasks And Run

                var iTotalFetchRows = 100;

                var queueTasks = new Queue<Task>();
                for (var iTotalSkipRows = 0; iTotalContracts > 0; iTotalSkipRows += iTotalFetchRows)
                {
                    var rows = iTotalSkipRows;
                    var total = iTotalContracts;

                    iTotalContracts -= iTotalContracts > iTotalFetchRows ? iTotalFetchRows : iTotalContracts;

                    queueTasks.Enqueue(new Task(() =>
                    {
                        var resultExeceProc = AppProcessor.ProcedureProvider.Execute(procCalcRemainingDays, true,
                            dataProviderName, rows, iTotalFetchRows);

                        logBuilder.AppendLine(
                            $"        + Thực thi procedure [Skip: {rows}] - [còn lại: {total}] => {resultExeceProc}");
                    }));
                }

                Task.Factory.StartNew(() =>
                {
                    while (queueTasks.Count > 0)
                    {
                        var queueTask = queueTasks.Dequeue();
                        queueTask.Start();
                        while (!queueTask.IsCompleted)
                        {
                        }
                    }

                    AppProcessor.JobLogger.Message(JobName, logBuilder.ToString());
                });

                #endregion
            }
            catch (Exception e)
            {
                logBuilder.AppendLine("         => Lỗi");
                AppProcessor.JobLogger.Message(JobName, logBuilder.ToString());
                AppProcessor.JobLogger.Error(JobName, e);
            }
        }
    }
}