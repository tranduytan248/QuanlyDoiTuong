using TSFramework.App.Attributes;

namespace Cores.Sys.Models.Sys
{
    public class SysElnvAccountModel
    {
        public int UserId { get; set; }

        [CustomDisplayName("ElnvAccount_FullName")]
        public string FullName { get; set; }

        [CustomDisplayName("EmpAccount_Label")]
        public string EmpAccount { get; set; }

        [CustomRequired]
        [CustomDisplayName("ElnvAccount_Label")]
        public string ElnvAccount { get; set; }

        [CustomRequired]
        [CustomDisplayName("ElnvACPassword_Label")]
        public string ElnvACPassword { get; set; }

        [CustomDisplayName("Reason_Label")] public string Reason { get; set; }
    }
}