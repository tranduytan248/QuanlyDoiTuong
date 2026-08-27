namespace Cores.Major.Models
{
    /// <summary>
    /// So lieu tong hop hien tren dau man hinh Quan ly Doi tuong.
    /// Moi con so deu da ap dung phan quyen theo don vi va linh vuc cua nguoi dang dang nhap.
    /// </summary>
    public class MajorSubjectDashboardModel
    {
        /// <summary>Tong so doi tuong trong pham vi duoc phep xem.</summary>
        public int TotalSubjects { get; set; }

        /// <summary>Tong so luot vi pham da ghi nhan.</summary>
        public int TotalViolations { get; set; }

        /// <summary>So doi tuong vi pham tu 2 linh vuc tro len - can chu y.</summary>
        public int MultiFieldSubjects { get; set; }

        /// <summary>So luot vi pham trong 30 ngay gan nhat.</summary>
        public int RecentViolations { get; set; }
    }
}
