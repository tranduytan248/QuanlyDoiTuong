using System;
using TSFramework.App.Attributes;
using TSFramework.App.Models;

namespace Core.Inv.Models
{
    public class MajorInvConfigModel : BaseModel
    {
        public Guid? ConfigId { get; set; }

        [CustomDisplayName("InvConfig_Key")]
        [CustomRequired]
        public string ConfigKey { get; set; }

        [CustomDisplayName("InvConfig_Value")]
        [CustomRequired]
        public string ConfigValue { get; set; }

        [CustomDisplayName("InvConfig_Desc")] public string ConfigDesc { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string CreatedBy { get; set; }
    }
}