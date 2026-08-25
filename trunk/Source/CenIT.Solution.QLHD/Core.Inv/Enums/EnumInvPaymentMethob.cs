using System.ComponentModel;

namespace Core.Inv.Enums
{
    /// <summary>
    ///     Loại phương thức thanh toán
    /// </summary>
    public enum EnumInvPaymentMethob
    {
        /// <summary>
        ///     Chuyển khoản
        /// </summary>
        [Description("CK")] Transfer,

        /// <summary>
        ///     Tiền mặt
        /// </summary>
        [Description("TM")] Cash,

        /// <summary>
        ///     Chuyển khoản hoặc tiền mặt
        /// </summary>
        [Description("TM, CK")] TransferOrCash,

        /// <summary>
        ///     Thanh toán thẻ tín dụng
        /// </summary>
        [Description("TTD")] Credit,

        /// <summary>
        ///     Thanh toán bù trừ
        /// </summary>
        [Description("Bù trừ")] Clearing
    }
}