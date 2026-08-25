using System.ComponentModel;

namespace Cores.Major.Enums
{
    public enum EnumPaymentMethod
    {
        /// <summary>
        ///     Tiền mặt
        /// </summary>
        [Description("PaymentMethod_Cash")] Cash = 1,

        /// <summary>
        ///     Online/Chuyển khoản/Ví điện tử, v.v.v
        /// </summary>
        [Description("PaymentMethod_Online")] Online = 2
    }
}