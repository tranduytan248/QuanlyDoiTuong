using System.ComponentModel;

namespace Cores.Major.Enums
{
    public enum EnumPaymentArising
    {
        /// <summary>
        ///     Phát sinh tăng
        /// </summary>
        [Description("TypeArising_Increase")] Increase = 0,

        /// <summary>
        ///     Phát sinh giảm
        /// </summary>
        [Description("TypeArising_Decrease")] Decrease = 1
    }
}