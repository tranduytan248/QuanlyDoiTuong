using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TSFramework.App.Attributes;

namespace Cores.Major.Models
{
    public class MajorSubjectModel
    {
        public Guid? SubjectId { get; set; } = Guid.Empty;

        [CustomRequired]
        [CustomDisplayName("Subject_Label_IdentityCardNumber")]
        public string IdentityCardNumber { get; set; }

        [CustomRequired]
        [CustomDisplayName("Subject_Label_FullName")]
        public string FullName { get; set; }

        [CustomDisplayName("Subject_Label_OtherName")]
        public string OtherName { get; set; }

        [CustomDisplayName("Subject_Label_DateOfBirth")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfBirth { get; set; }

        public string DateOfBirthStr => DateOfBirth.HasValue ? DateOfBirth.Value.ToString("dd/MM/yyyy") : string.Empty;

        [CustomDisplayName("Subject_Label_Gender")]
        public string Gender { get; set; }

        [CustomDisplayName("Subject_Label_Ethnicity")]
        public string Ethnicity { get; set; }

        [CustomDisplayName("Subject_Label_Religion")]
        public string Religion { get; set; }

        [CustomDisplayName("Subject_Label_Nationality")]
        public string Nationality { get; set; } = "Việt Nam";

        [CustomDisplayName("Subject_Label_PlaceOfOrigin")]
        public string PlaceOfOrigin { get; set; }

        [CustomDisplayName("Subject_Label_IdentityCardFrontUrl")]
        public string IdentityCardFrontUrl { get; set; }

        [CustomDisplayName("Subject_Label_IdentityCardBackUrl")]
        public string IdentityCardBackUrl { get; set; }

        [CustomDisplayName("Subject_Label_AvatarUrl")]
        public string AvatarUrl { get; set; }

        [CustomDisplayName("Subject_Label_BirthRegistrationPlace")]
        public string BirthRegistrationPlace { get; set; }

        [CustomDisplayName("Subject_Label_CurrentResidence")]
        public string CurrentResidence { get; set; }

        [CustomDisplayName("Subject_Label_PhoneNumber")]
        public string PhoneNumber { get; set; }

        [CustomDisplayName("Subject_Label_ReporterName")]
        public string ReporterName { get; set; }

        [CustomDisplayName("Subject_Label_ReporterUnit")]
        public string ReporterUnit { get; set; }

        [CustomDisplayName("Subject_Label_ReporterPhone")]
        public string ReporterPhone { get; set; }

        [CustomDisplayName("Subject_Label_ReporterPosition")]
        public string ReporterPosition { get; set; }

        /// <summary>
        /// Khoá đơn vị khai báo, dùng để phân quyền xem dữ liệu theo tổ / phòng ban.
        /// </summary>
        public Guid? ReporterUnionId { get; set; }

        public int ViolationCount { get; set; } = 0;
        public int TrackingUnitCount { get; set; } = 1;
        public string TrackingUnits { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public int? TotalRow { get; set; } = 0;

        // Thông tin ghi nhận vi phạm
        public DateTime? InitialViolationDate { get; set; }
        public int? InitialFieldId { get; set; }
        public string InitialBehaviorIds { get; set; }
        public string InitialTreatmentMeasures { get; set; }
        public string InitialRelatedDocuments { get; set; }
        public string InitialNotes { get; set; }
        public string InitialImages { get; set; }

        public List<MajorSubjectViolationModel> ListViolations { get; set; } = new List<MajorSubjectViolationModel>();
        public List<MajorSubjectMonitoringUnitModel> ListMonitoringUnits { get; set; } = new List<MajorSubjectMonitoringUnitModel>();
    }
}
