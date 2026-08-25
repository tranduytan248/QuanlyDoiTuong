using System;

namespace Core.Inv.Models
{
    public class MajorInvActionLogModel
    {
        public string SysAccount { get; set; }

        public string InvAccount { get; set; }

        public string ActionType { get; set; }

        public string Contents { get; set; }

        public DateTime OnDate { get; set; }

        public string Reason { get; set; }
    }
}