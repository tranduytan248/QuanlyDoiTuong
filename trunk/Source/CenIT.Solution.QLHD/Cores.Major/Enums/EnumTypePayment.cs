using System.ComponentModel;

namespace Cores.Major.Enums
{
    public enum EnumTypePayment
    {
        //None = 0,

        /// <summary>
        ///     Tạm ứng
        /// </summary>
        [Description("TypePayment_Advance")] Advance = 1,

        /// <summary>
        ///     Thanh lý
        /// </summary>
        [Description("TypePayment_PayOff")] PayOff = 2,

        /// <summary>
        ///     Hoàn trả
        /// </summary>
        [Description("TypePayment_Refunded")] Refunded = 3
    }
}