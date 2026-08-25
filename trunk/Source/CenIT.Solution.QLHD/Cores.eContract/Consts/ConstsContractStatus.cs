namespace Cores.eContract.Consts
{
    /// <summary>
    /// Trạng thái hợp đồng 
    /// </summary>
    public class ConstsContractStatus
    {
        /// <summary>
        /// Mới khởi tạo
        /// </summary>
        public const string CREATE = "LC_DRAFT_CREATE";
        /// <summary>
        /// Chờ thẩm định
        /// </summary>
        public const string CONFIRM = "LC_DRAFT_WAIT_CONFIRM";
        /// <summary>
        /// Chờ ký 
        /// </summary>
        public const string SUBMIT = "LC_DRAFT_SUBMIT";
        /// <summary>
        /// Đàm phán 
        /// </summary>

        public const string DEAL = "LC_DRAFT_DEAL";
        /// <summary>
        ///  Chờ ký
        /// </summary>
        public const string SIGNED = "LC_DRAFT_SIGNED";
        /// <summary>
        /// HĐ huỷ 
        /// </summary>
        public const string CANCEL = "LC_DRAFT_CANCEL";
        /// <summary>
        /// HĐ có hiệu lực   
        /// </summary>
        public const string VALID = "LC_CONTRACT_VALID";
        /// <summary>
        /// HĐ đang cảnh báo  
        /// </summary>
        public const string WARNING = "LC_DRAFT_WARNING";
        /// <summary>
        /// HĐ ký lỗi
        /// </summary>
        public const string SIGN_FAIL = "LC_DRAFT_SIGN_FAIL";
        /// <summary>
        /// HĐ hết hiệu lực 
        /// </summary>
        public const string EXPIRED = "LC_DRAFT_EXPIRED";
    }
}