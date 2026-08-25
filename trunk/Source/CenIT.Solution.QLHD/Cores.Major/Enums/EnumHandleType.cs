using System.ComponentModel;

namespace Cores.Major.Enums
{
    public enum EnumHandleType
    {
        /// <summary>
        ///     Hồ sơ đã xử lý
        /// </summary>
        [Description("HandleType_PreviouslyProcessed")]
        PreviouslyProcessed = 1,

        /// <summary>
        ///     Hồ sơ cần xử lý - Xử lý chính
        /// </summary>
        [Description("HandleType_MainProcessing")]
        MainProcessing = 2,

        /// <summary>
        ///     Hồ sơ theo dõi - Xem
        /// </summary>
        [Description("HandleType_SupportProcessingView")]
        SupportProcessingView = 3
    }
}