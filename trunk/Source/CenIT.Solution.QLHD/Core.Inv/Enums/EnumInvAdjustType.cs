using System.ComponentModel;

namespace Core.Inv.Enums
{
    public enum EnumInvAdjustType
    {
        /// <summary>
        ///     Hoá đơn điều chỉnh tăng
        /// </summary>
        [Description("Hoá đơn điều chỉnh tăng")]
        InvoiceAdjustIncrease = 2,

        /// <summary>
        ///     Hoá đơn điều chỉnh giảm
        /// </summary>
        [Description("Hoá đơn điều chỉnh giảm")]
        InvoiceAdjustDecrease = 3,

        /// <summary>
        ///     Hoá đơn điều chỉnh thông tin
        /// </summary>
        [Description("Hoá đơn điều chỉnh thông tin")]
        InvoiceAdjustInfo = 4
    }
}