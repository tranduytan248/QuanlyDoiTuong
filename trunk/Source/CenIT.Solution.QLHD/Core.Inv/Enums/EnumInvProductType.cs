using System.ComponentModel;

namespace Core.Inv.Enums
{
    public enum EnumInvProductType
    {
        [Description("Hàng hóa, dịch vụ")] Product = 1,
        [Description("Khuyến mại")] Promotion = 2,
        [Description("Chiết khấu thương mại")] Discount = 3,
        [Description(" Ghi chú/diễn giải")] Note = 4
    }
}