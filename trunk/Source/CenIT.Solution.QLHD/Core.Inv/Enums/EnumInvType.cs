using System.ComponentModel;

namespace Core.Inv.Enums
{
    /// <summary>
    ///     Loại hóa đơn
    /// </summary>
    public enum EnumInvType
    {
        /// <summary>
        ///     Hoá đơn bình thường
        /// </summary>
        [Description("Hoá đơn bình thường")] InvoiceTypeNormal,

        /// <summary>
        ///     Hoá đơn thay thế
        /// </summary>
        [Description("Hoá đơn thay thế")] InvoiceTypeReplace,

        /// <summary>
        ///     Hoá đơn điều chỉnh tăng
        /// </summary>
        [Description("Hoá đơn điều chỉnh tăng")]
        InvoiceAdjustIncrease,

        /// <summary>
        ///     Hoá đơn điều chỉnh giảm
        /// </summary>
        [Description("Hoá đơn điều chỉnh giảm")]
        InvoiceAdjustDecrease,

        /// <summary>
        ///     Hoá đơn điều chỉnh thông tin
        /// </summary>
        [Description("Hoá đơn điều chỉnh thông tin")]
        InvoiceAdjustInfo
    }
}