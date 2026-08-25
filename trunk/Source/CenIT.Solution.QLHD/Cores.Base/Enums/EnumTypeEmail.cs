using System.ComponentModel;

namespace Cores.Base.Enums
{
    public enum EnumTypeEmail
    {
        /// <summary>
        ///     Thông báo xác nhận thực hiện hợp đồng
        /// </summary>
        [Description("Situation_ContractConfirmation")]
        ContractConfirmation = 1, // Thông báo xác nhận thực hiện hợp đồng

        /// <summary>
        ///     Thông báo hợp đồng bị từ chối
        /// </summary>
        [Description("Situation_ContractRejection")]
        ContractRejection = 2, // Thông báo hợp đồng bị từ chối 

        /// <summary>
        ///     Thông báo hợp đồng có kết quả
        /// </summary>
        [Description("Situation_ContractResult")]
        ContractResult = 3, // Thông báo hợp đồng có kết quả 

        /// <summary>
        ///     Thông báo quên mật khẩu
        /// </summary>
        [Description("Situation_ForgotPassword")]
        ForgotPassword = 4, // Thông báo quên mật khẩu

        /// <summary>
        ///     Thông báo hợp đồng chờ xử lý
        /// </summary>
        [Description("Situation_ContractPending")]
        ContractPending = 5, // Thông báo hợp đồng chờ xử lý

        /// <summary>
        ///     Thông báo hồ sơ trễ hạn
        /// </summary>
        [Description("Situation_ProfileOverdue")]
        ProfileOverdue = 6, // Thông báo hồ sơ trễ hạn

        /// <summary>
        ///     Thông báo hồ sơ sắp đến hạn
        /// </summary>
        [Description("Situation_ProfileApproachingDeadline")]
        ProfileApproachingDeadline = 7 // Thông báo hồ sơ sắp đến hạn
    }
}