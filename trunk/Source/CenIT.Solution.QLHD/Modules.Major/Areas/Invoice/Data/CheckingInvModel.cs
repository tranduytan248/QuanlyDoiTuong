namespace Modules.Major.Areas.Invoice.Data
{
    public class CheckingInvModel
    {
        public bool? HasNotSysInvAccount { get; set; } = false;
        public bool? IsInvServiceAccountIncorrect { get; set; } = false;
    }
}