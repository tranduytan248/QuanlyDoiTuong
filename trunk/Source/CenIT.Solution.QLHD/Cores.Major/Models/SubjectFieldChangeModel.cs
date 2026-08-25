namespace Cores.Major.Models
{
    /// <summary>
    /// Mô tả một trường dữ liệu bị thay đổi. Danh sách các đối tượng này được
    /// tuần tự hoá thành JSON và lưu vào cột ChangedFields của bảng log.
    /// </summary>
    public class SubjectFieldChangeModel
    {
        /// <summary>Tên thuộc tính trong model, ví dụ FullName.</summary>
        public string Field { get; set; }

        /// <summary>Nhãn tiếng Việt để hiển thị, ví dụ "Họ và tên khai sinh".</summary>
        public string Label { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }
    }
}
