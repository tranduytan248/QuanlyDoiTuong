namespace Modules.Major.Areas.Invoice.Data
{
    public class InvPatternSerialInfoModel
    {
        public string Serial { get; set; }
        public int TotalInv { get; set; }
        public string InvNoFrom { get; set; }
        public string InvNoTo { get; set; }
        public string CurrentInvNo { get; set; }
        public int TotalRemainingInv { get; set; }
        public string BeginUsedFrom { get; set; }
        //public int Status { get; set; }
    }
}