using System;

namespace Modules.Major.Areas.Invoice.Data
{
    public class ConfirmInvModel
    {
        public Guid? InvId { get; set; }
        public string InvKey { get; set; }
        public string InvNo { get; set; }
        public string Pattern { get; set; }
        public string Serial { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Reason { get; set; }

        public int TotalInvs { get; set; }
        public bool IsOldVersion { get; set; } = false;
    }
}