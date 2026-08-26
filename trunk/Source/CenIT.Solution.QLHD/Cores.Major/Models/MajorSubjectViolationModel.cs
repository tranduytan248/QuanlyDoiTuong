using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TSFramework.App.Attributes;

namespace Cores.Major.Models
{
    public class MajorSubjectViolationModel
    {
        public Guid? ViolationId { get; set; } = Guid.Empty;

        [CustomRequired]
        [CustomDisplayName("SubjectViolation_Label_Subject")]
        public Guid SubjectId { get; set; }

        public string SubjectName { get; set; }
        public string IdentityCardNumber { get; set; }
        public string PhoneNumber { get; set; }

        [CustomRequired]
        [CustomDisplayName("SubjectViolation_Label_ViolationDate")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime ViolationDate { get; set; } = DateTime.Now;

        public string ViolationDateStr => ViolationDate.ToString("dd/MM/yyyy HH:mm");

        [CustomDisplayName("SubjectViolation_Label_TreatmentMeasures")]
        public string TreatmentMeasures { get; set; }

        [CustomDisplayName("SubjectViolation_Label_RelatedDocuments")]
        public string RelatedDocuments { get; set; }

        [CustomDisplayName("SubjectViolation_Label_Images")]
        public string Images { get; set; }

        [CustomDisplayName("SubjectViolation_Label_Notes")]
        public string Notes { get; set; }

        [CustomDisplayName("SubjectViolation_Label_Behaviors")]
        public string BehaviorIds { get; set; }

        public string BehaviorNames { get; set; }

        public List<int> ListBehaviorIds { get; set; } = new List<int>();

        /// <summary>
        /// Danh sach van ban dinh kem, duoc phan tich san tu chuoi JSON trong
        /// RelatedDocuments. Nho vay giao dien chi can lap qua danh sach, khong
        /// phai phan tich JSON bang JavaScript o tung man hinh.
        /// </summary>
        public List<ViolationDocumentModel> ListRelatedDocuments
        {
            get
            {
                var documents = new List<ViolationDocumentModel>();
                if (string.IsNullOrWhiteSpace(RelatedDocuments)) return documents;

                var raw = RelatedDocuments.Trim();

                // Truong hop du lieu cu: chi luu mot duong dan don le
                if (!raw.StartsWith("["))
                {
                    documents.Add(new ViolationDocumentModel
                    {
                        Name = System.IO.Path.GetFileName(raw),
                        Url = raw
                    });
                    return documents;
                }

                try
                {
                    documents = Newtonsoft.Json.JsonConvert
                        .DeserializeObject<List<ViolationDocumentModel>>(raw)
                        ?? new List<ViolationDocumentModel>();
                }
                catch (Exception)
                {
                    // Chuoi khong dung dinh dang JSON - bo qua de khong lam hong giao dien
                    documents.Clear();
                }

                return documents;
            }
        }

        /* --- Thông tin người khai báo: không hiển thị trên form, chỉ lưu xuống CSDL --- */
        public string ReporterName { get; set; }

        public string ReporterUnit { get; set; }

        public string ReporterPosition { get; set; }

        public string ReporterPhone { get; set; }

        /// <summary>
        /// Khoá đơn vị khai báo, dùng để phân quyền xem dữ liệu theo tổ / phòng ban.
        /// </summary>
        public Guid? ReporterUnionId { get; set; }

        /// <summary>
        /// True khi người đang đăng nhập chính là người đã khai báo lần vi phạm này
        /// (hoặc là super admin). Chỉ dùng để hiển thị nút Sửa / Xoá;
        /// quyền thật vẫn được kiểm tra lại ở tầng controller.
        /// </summary>
        public bool IsOwner { get; set; }

        /// <summary>Danh sách lĩnh vực của các hành vi trong lần vi phạm này.</summary>
        public string FieldNames { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public int? TotalRow { get; set; } = 0;
    }
}
