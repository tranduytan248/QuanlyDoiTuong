using System.ComponentModel;

namespace Core.Inv.Enums
{
    /// <summary>
    ///     Loại thao tác với hoá đơn điện tử
    /// </summary>
    public enum EnumInvActionType
    {
        /// <summary>
        ///     Thêm mới và phát hành
        /// </summary>
        [Description("Thêm mới và phát hành")] ImportAndPublishInvoice,

        /// <summary>
        ///     Điều chỉnh hoá đơn
        /// </summary>
        [Description("Điều chỉnh hoá đơn")] AdjustInvoice,

        /// <summary>
        ///     Thay thế hoá đơn
        /// </summary>
        [Description("Thay thế hoá đơn")] ReplaceInvoice,

        /// <summary>
        ///     Huỷ hoá đơn
        /// </summary>
        [Description("Huỷ hoá đơn")] CancelInvoice,

        /// <summary>
        ///     Xác nhận thanh toán hoá đơn
        /// </summary>
        [Description("Xác nhận thanh toán hoá đơn")]
        ConfirmPaymentInvoice,

        /// <summary>
        ///     Huỷ xác nhận thanh toán hoá đơn
        /// </summary>
        [Description("Huỷ xác nhận thanh toán hoá đơn")]
        UnConfirmPaymentInvoice,

        /// <summary>
        ///     Cập nhật thông tin khách hàng
        /// </summary>
        [Description("Cập nhật thông tin khách hàng")]
        UpdateCustomerInfo
    }
}