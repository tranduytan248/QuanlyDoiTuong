using System.Net;
using System;
using System.Configuration;
using System.Web.Hosting;
using Quartz;
using TSFramework.App.Processors;
using TSFramework.Core.Members.Job;

namespace CenIT.Solution.QLHD.Jobs.RefreshJob
{
    public class ExecuteJob : JobModel
    {
        public override void Execute(IJobExecutionContext context)
        {
            var jobName = "Jobs.RefreshJob";
            try
            {
                var hostProtocol = ConfigurationManager.AppSettings["Host_Protocol"] ?? "http://";
                AppProcessor.JobLogger.Message(jobName, "========================================");
                AppProcessor.JobLogger.Message(jobName, "==========Gọi Request tới site========");
                AppProcessor.JobLogger.Message(jobName, "========================================");
                var urlHost = $"{hostProtocol}{HostingEnvironment.SiteName}/";
                using (var client = new WebClient())
                {
                    client.DownloadString(urlHost);
                }
            }
            catch (Exception ex)
            {
                AppProcessor.JobLogger.Error(jobName, ex);
            }
        }

        public override void ExecuteNow(object data)
        {
            var jobName = "Jobs.RefreshJob";
            try
            {
                var hostProtocol = ConfigurationManager.AppSettings["Host_Protocol"] ?? "http://";
                AppProcessor.JobLogger.Message(jobName, "========================================");
                AppProcessor.JobLogger.Message(jobName, "==========Gọi Request tới site========");
                AppProcessor.JobLogger.Message(jobName, "========================================");
                var urlHost = $"{hostProtocol}{HostingEnvironment.SiteName}/";
                using (var client = new WebClient())
                {
                    client.DownloadString(urlHost);
                }
            }
            catch (Exception ex)
            {
                AppProcessor.JobLogger.Error(jobName, ex);
            }
        }
    }
}