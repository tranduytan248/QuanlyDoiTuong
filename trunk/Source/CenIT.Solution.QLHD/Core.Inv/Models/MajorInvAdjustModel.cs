using System;

namespace Core.Inv.Models
{
    public class MajorInvAdjustModel
    {
        public Guid AdjustedInvId { get; set; }
        public Guid AdjustInvId { get; set; }
        public string AdjustedInvKey { get; set; }
        public string AdjustInvKey { get; set; }
        public string AdjustedInvSerial { get; set; }
        public string AdjustInvSerial { get; set; }
        public string AdjustedInvNo { get; set; }
        public string AdjustInvNo { get; set; }
        public string AdjustedInvPattern { get; set; }
        public string AdjustInvPattern { get; set; }
        public int AdjustedInvStatus { get; set; }
        public int AdjustInvStatus { get; set; }
        public int? TotalRow { get; set; } = 0;
    }
}