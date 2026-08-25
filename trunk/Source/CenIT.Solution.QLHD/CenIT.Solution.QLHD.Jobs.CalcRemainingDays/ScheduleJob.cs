using System;
using TSFramework.Core.Members.Job;
using TSFramework.Core.Providers;

namespace CenIT.Solution.QLHD.Jobs.CalcRemainingDays
{
    [JobPlugin("CenIT.Solution.QLHD.Jobs.CalcRemainingDays", "Job calculate remaining days for contract and update")]
    public class ScheduleJob : IJobPlugable
    {
        public void ExecuteJobNow(string sJobId, string sDescription, params object[] dataObjects)
        {
            var sPreTriggerId = JobProvider.GenerateTriggerId(5); // Can custom
            var sPreGroupId = JobProvider.GenerateGroupId(6); // Can custom
            var sTriggerId = $"{sPreTriggerId}-{sJobId}";
            var sGroupId = $"{sPreGroupId}-{sJobId}";

            var importDataScheduleJob = new ExecuteJob()
                .SetJobId(sJobId)
                .SetGroupId(sGroupId)
                .SetDescription(sDescription)
                .SetTriggerId(sTriggerId)
                .StartNow();

            var dataMailContent = dataObjects?[0];
            importDataScheduleJob.ExecuteNow(dataMailContent);
        }

        public string PluginName
        {
            get => "Job calculate remaining days for contract and update";
            set { }
        }

        public Type JobType { get; set; }

        public JobModel MainJob { get; set; }

        public IJobPlugable BuildJob(string sJobId, string sDescription, string sCronExpression,
            params object[] dataObjects)
        {
            // sCronExpression = "0 0/30 * * * ? *"; // 30 phút chạy 1 lần
            var sPreTriggerId = JobProvider.GenerateTriggerId(5); // Can custom
            var sPreGroupId = JobProvider.GenerateGroupId(6); // Can custom
            var sTriggerId = $"{sPreTriggerId}-{sJobId}";
            var sGroupId = $"{sPreGroupId}-{sJobId}";

            var importDataScheduleJob = new ExecuteJob()
                .SetJobId(sJobId)
                .SetGroupId(sGroupId)
                .SetDescription(sDescription)
                .SetTriggerId(sTriggerId)
                .WithCronSchedule(sCronExpression);

            return new ScheduleJob { MainJob = importDataScheduleJob, JobType = typeof(ExecuteJob) };
        }
    }
}