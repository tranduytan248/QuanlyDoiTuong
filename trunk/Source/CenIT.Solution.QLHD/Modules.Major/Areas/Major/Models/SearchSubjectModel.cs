namespace Modules.Major.Areas.Major.Models
{
    public class SearchSubjectModel
    {
        /// <summary>Từ khoá chung (giữ lại để tương thích với ô tìm nhanh của DataTables).</summary>
        public string Key { get; set; }

        public string Gender { get; set; }

        /// <summary>Tra cứu theo số CCCD / CMND.</summary>
        public string IdentityCardNumber { get; set; }

        /// <summary>Tra cứu theo họ tên khai sinh hoặc tên gọi khác.</summary>
        public string FullName { get; set; }

        /// <summary>Tra cứu theo hành vi vi phạm - danh sách id phân tách bởi dấu phẩy.</summary>
        public string BehaviorIds { get; set; }

        /// <summary>Tra cứu theo loại đối tượng - danh sách id phân tách bởi dấu phẩy.</summary>
        public string SubjectTypeIds { get; set; }
    }
}
