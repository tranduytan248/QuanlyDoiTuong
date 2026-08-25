using System.Collections.Generic;
using System.IO;

namespace Modules.Sys.Areas.Sys.Models
{
    public class SysLogModel
    {
        public SysLogModel()
        {
            ListErrFiles = ListInvLogFiles = ListJobLogFiles = new List<FileInfo>();
        }

        public List<FileInfo> ListErrFiles { get; set; }

        public List<FileInfo> ListInvLogFiles { get; set; }

        public List<FileInfo> ListJobLogFiles { get; set; }
    }

    public class DeleteOldFileModel
    {
        public string TypeLog { get; set; }
        public int MonthAgo { get; set; }
    }
}