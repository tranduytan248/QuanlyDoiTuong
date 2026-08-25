using System;

namespace Cores.Base.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ReportInfoAttribute : Attribute
    {
        public ReportInfoAttribute(string reportKey, string reportName, string description)
        {
            ReportKey = reportKey;
            ReportName = reportName;
            Description = description;
        }

        public string ReportKey { get; }

        public string ReportName { get; }

        public string Description { get; }
    }
}