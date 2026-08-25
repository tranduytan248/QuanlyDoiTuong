using System.Collections.Generic;
using TSFramework.App.Attributes;

namespace Cores.Cate.Models
{
    /// <summary>
    /// Phân quyền lĩnh vực cho người dùng.
    /// Quyết định người dùng được xem dữ liệu Đối tượng / Lịch sử vi phạm
    /// thuộc những lĩnh vực nào.
    /// </summary>
    public class CateUserFieldModel
    {
        [CustomRequired]
        [CustomDisplayName("User_Title")]
        public string UserName { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        /// <summary>Danh sách id lĩnh vực được phân, phân tách bởi dấu phẩy.</summary>
        public string FieldIds { get; set; }

        /// <summary>Danh sách tên lĩnh vực được phân, dùng để hiển thị trên lưới.</summary>
        public string FieldNames { get; set; }

        public int TotalField { get; set; }

        public int? TotalRow { get; set; } = 0;

        /// <summary>Danh mục toàn bộ lĩnh vực, dùng để dựng danh sách chọn trên form.</summary>
        public List<CateFieldModel> ListFields { get; set; } = new List<CateFieldModel>();
    }
}
