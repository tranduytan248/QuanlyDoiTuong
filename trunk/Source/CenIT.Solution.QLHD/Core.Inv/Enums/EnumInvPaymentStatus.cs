using System.ComponentModel;

namespace Core.Inv.Enums
{
    public enum EnumInvPaymentStatus
    {
        /// <summary>
        ///     Chưa thanh toán
        /// </summary>
        [Description("PaymentStatus_NotYet")] NotYet = 0,

        /// <summary>
        ///     Đã thanh toán
        /// </summary>
        [Description("PaymentStatus_Paid")] Paid = 1
    }
}