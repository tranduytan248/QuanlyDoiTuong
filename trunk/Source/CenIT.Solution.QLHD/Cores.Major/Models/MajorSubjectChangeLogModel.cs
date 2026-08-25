using System;

namespace Cores.Major.Models
{
    /// <summary>
    /// Một dòng log ghi lại thay đổi của Đối tượng hoặc Lịch sử vi phạm.
    /// Bảng chỉ ghi thêm, không sửa - phục vụ tra soát và báo cáo về sau.
    /// </summary>
    public class MajorSubjectChangeLogModel
    {
        public Guid? LogId { get; set; }

        public Guid SubjectId { get; set; }

        /// <summary>Chỉ có giá trị khi dòng log liên quan tới một lần vi phạm.</summary>
        public Guid? ViolationId { get; set; }

        /// <summary>SUBJECT hoặc VIOLATION - xem ConstsChangeLog.</summary>
        public string EntityType { get; set; }

        /// <summary>ADD, UPDATE hoặc DELETE - xem ConstsChangeLog.</summary>
        public string ActionType { get; set; }

        /// <summary>Chi tiết các trường bị thay đổi, lưu dạng JSON.</summary>
        public string ChangedFields { get; set; }

        /// <summary>Danh sách nhãn các trường bị thay đổi, để hiển thị nhanh trên lưới.</summary>
        public string ChangedFieldNames { get; set; }

        public string Description { get; set; }

        public string ActorUserName { get; set; }

        public string ActorName { get; set; }

        /// <summary>Chức vụ của người thực hiện tại thời điểm thao tác.</summary>
        public string ActorPosition { get; set; }

        /// <summary>Đơn vị công tác của người thực hiện tại thời điểm thao tác.</summary>
        public string ActorUnit { get; set; }

        public Guid? ActorUnionId { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string CreatedDateStr => CreatedDate?.ToString("dd/MM/yyyy HH:mm") ?? string.Empty;

        public string SubjectName { get; set; }

        public string IdentityCardNumber { get; set; }

        public int? TotalRow { get; set; } = 0;
    }
}
