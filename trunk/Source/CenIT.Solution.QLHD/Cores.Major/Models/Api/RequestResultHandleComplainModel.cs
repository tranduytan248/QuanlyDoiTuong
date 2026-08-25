using System;
using System.Collections.Generic;

namespace Cores.Major.Models.Api
{
    public class RequestResultHandleComplainModel
    {
        /// <summary>
        ///     Id của PAKN
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Trạng thái của PAKN mặc định là 10 – Đã xử lý
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        ///     Danh sách file đính kèm câu trả lời
        /// </summary>
        public List<string> QD { get; set; }

        /// <summary>
        ///     Quá trình xử lý PAKN
        /// </summary>
        public List<HisProcessingModel> ListHisprocessing { get; set; }

        /// <summary>
        ///     True là giữ bí mật câu trả lời; False là công bố
        /// </summary>
        public bool IsPrivate { get; set; }

        /// <summary>
        ///     True là giữ bí mật file đính kèm của người dân gửi; False là công bố
        /// </summary>
        public bool IsPrivateVBNguoiDan { get; set; }

        /// <summary>
        ///     True là giữ bí mật file đính kèm của cán bộ trả lời; False là công bố
        /// </summary>
        public bool IsPrivateVBCanBo { get; set; }

        /// <summary>
        ///     Mã đơn vị xử  lý
        /// </summary>
        public bool UnitId { get; set; }
    }

    public class HisProcessingModel
    {
        /*
         *Trạng thái hành động:
           8: gửi phê duyệt;
           9: từ chối phê duyệt;
           10: phê duyệt;
         */
        /// <summary>
        ///     Trạng thái hành động
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        ///     Thời gian thực hiện thao tác
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        ///     Id người gửi
        /// </summary>
        public int UserIdSend { get; set; }

        /// <summary>
        ///     Id người nhận
        /// </summary>
        public int UserIdReceive { get; set; }

        /// <summary>
        ///     Câu trả lời hoặc lý do từ chối câu trả lời để cán bộ xử lý lại
        /// </summary>
        public string Content { get; set; }
    }
}