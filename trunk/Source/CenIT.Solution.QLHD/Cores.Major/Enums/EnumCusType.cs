using System.ComponentModel;

namespace Cores.Major.Enums
{
    public enum EnumCusType
    {
        /// <summary>
        ///     Cá nhân
        /// </summary>
        [Description("CusType_Consumer")] Consumer = 1,

        /// <summary>
        ///     Doanh nghiệp
        /// </summary>
        [Description("CusType_Business")] Business = 2
    }
}