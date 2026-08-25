using System.ComponentModel;

namespace Core.Inv.Enums
{
    /// <summary>
    ///     Trạng thái hóa đơn
    /// </summary>
    public enum EnumInvStatus
    {
        /// <summary>
        ///     Hoá đơn vừa khởi tạo
        /// </summary>
        [Description("Hoá đơn vừa khởi tạo")] InvoiceJustCreated,

        /// <summary>
        ///     Hoá đơn đã phát hành
        /// </summary>
        [Description("Hoá đơn có đủ chữ ký")] InvoiceHasSignature,

        /// <summary>
        ///     Hoá đơn đã khai báo thuế
        /// </summary>
        [Description("Hoá đơn đã khai báo thuế")]
        InvoiceTaxDeclaration,

        /// <summary>
        ///     Hoá đơn bị thay thế
        /// </summary>
        [Description("Hoá đơn bị thay thế")] InvoiceAreReplaced,

        /// <summary>
        ///     Hoá đơn bị điều chỉnh
        /// </summary>
        [Description("Hoá đơn bị điều chỉnh")] InvoiceAreAdjusted,

        /// <summary>
        ///     Hoá đơn xoá bỏ
        /// </summary>
        [Description("Hoá đơn xoá bỏ")] InvoiceAreCancled,

        /// <summary>
        ///     Hoá đơn bị điều chỉnh
        /// </summary>
        [Description("Hoá đơn điều chỉnh")] InvoiceAdjustment
    }
}