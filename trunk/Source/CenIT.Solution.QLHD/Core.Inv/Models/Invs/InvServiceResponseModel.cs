namespace Core.Inv.Models.Invs
{
    public class InvServiceResponseModel
    {
        public bool IsErr { get; set; } = false;
        public object ResponseContents { get; set; }
    }
}