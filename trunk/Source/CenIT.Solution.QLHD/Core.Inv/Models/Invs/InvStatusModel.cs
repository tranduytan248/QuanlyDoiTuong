using System;

namespace Core.Inv.Models.Invs
{
    public class InvStatusModel
    {
        public string InvKey { get; set; }

        public string InvNo { get; set; }

        public int InvStatus { get; set; }

        public string InvStatusName { get; set; }

        public string PublishBy { get; set; }

        public DateTime? PublishOn { get; set; }

        public string ConfirmPaidBy { get; set; }

        public DateTime? PaidOn { get; set; }

        public string ErrCode { get; set; }

        public string Reason { get; set; }

        public string SavedBy { get; set; }
    }
}