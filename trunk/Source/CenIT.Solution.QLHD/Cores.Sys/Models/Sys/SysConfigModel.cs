using System;
using System.Data;
using System.Web;
using TSFramework.App.Attributes;
using TSFramework.App.Models;

namespace Cores.Sys.Models.Sys
{
    public class SysConfigModel : BaseModel
    {
        public int ConfigId { get; set; }

        [CustomRequired]
        [CustomDisplayName("SysConfig_ConfigValue")]
        public string ConfigValue { get; set; }

        [CustomRequired]
        [CustomDisplayName("SysConfig_ConfigKey")]
        public string ConfigKey { get; set; }

        [CustomDisplayName("SysConfig_ConfigDesc")]
        public string ConfigDesc { get; set; }

        [CustomDisplayName("SysConfig_IsFile")]
        public bool IsFile { get; set; } = false;

        public string DataRefFile { get; set; }

        public DataTable TableRefFile { get; set; }

        [CustomDisplayName("Import_File")] public HttpPostedFileBase RefFile { get; set; }

        public new int? TotalRow { get; set; } = 0;
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string LastModifiedBy { get; set; }

        public string SaveBy { get; set; }

        public string DeletedBy { get; set; }
    }
}