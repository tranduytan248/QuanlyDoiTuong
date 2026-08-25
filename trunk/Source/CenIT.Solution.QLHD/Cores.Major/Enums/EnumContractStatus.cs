using System.ComponentModel;

namespace Cores.Major.Enums
{
    public enum EnumContractStatus
    {
        /// <summary>
        ///     Hợp đồng nháp
        /// </summary>
        [Description("ContractStatus_Draft")] Draft = -1,

        /// <summary>
        ///     Chờ xử lý
        /// </summary>
        [Description("ContractStatus_Waiting")]
        Waiting = 0,

        /// <summary>
        ///     Đang xử lý
        /// </summary>
        [Description("ContractStatus_Handling")]
        Handling = 1,

        /// <summary>
        ///     Tạm dừng
        /// </summary>
        [Description("ContractStatus_Paused")] Paused = 2,

        /// <summary>
        ///     Đã hoàn thành
        /// </summary>
        [Description("ContractStatus_Completed")]
        Completed = 3,

        /// <summary>
        ///     Đã thanh lý
        /// </summary>
        [Description("ContractStatus_Liquidated")]
        Liquidated = 4,

        /// <summary>
        ///     Đã huỷ
        /// </summary>
        [Description("ContractStatus_Cancel")] Cancel = 99
    }
}