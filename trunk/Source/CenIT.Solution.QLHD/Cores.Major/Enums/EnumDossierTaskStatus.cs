using System.ComponentModel;

namespace Cores.Major.Enums
{
    public enum EnumDossierTaskStatus
    {
        /// <summary>
        ///     Tạm dừng xử lý
        /// </summary>
        [Description("HandleDossier_Paused")] Paused = -1,

        ///// <summary>
        ///// Chờ xử lý
        ///// </summary>
        //[Description("HandleDossier_WaitingForHandle")]
        //WaitingForHandle = 1,

        /// <summary>
        ///     Đang xử lý
        /// </summary>
        [Description("HandleDossier_Handling")]
        Handling = 2,

        /// <summary>
        ///     Hoàn thành
        /// </summary>
        [Description("HandleDossier_Completed")]
        Completed = 3
    }
}