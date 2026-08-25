using System.ComponentModel;

namespace Core.Inv.Enums
{
    public enum EnumInvTransferStatus
    {
        /// <summary>
        ///     Chưa chuyển khoản
        /// </summary>
        [Description("TransferStatus_NotYet")] NotYet = -1,

        /// <summary>
        ///     Đã chuyển đổi
        /// </summary>
        [Description("TransferStatus_Transferred")]
        Transferred = 1
    }
}