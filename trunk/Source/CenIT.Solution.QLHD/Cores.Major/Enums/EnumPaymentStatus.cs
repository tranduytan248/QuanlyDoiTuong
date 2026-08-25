using System.ComponentModel;

namespace Cores.Major.Enums
{
    public enum EnumPaymentStatus
    {
        /// <summary>
        ///     Thu tiền
        /// </summary>
        [Description("PaymentStatus_Received")]
        Received = 1,

        /// <summary>
        ///     Chi tiền
        /// </summary>
        [Description("PaymentStatus_Refunded")]
        Refunded = 2
    }
}