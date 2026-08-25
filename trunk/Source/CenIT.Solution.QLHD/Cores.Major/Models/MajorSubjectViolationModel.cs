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

        public bool IsDeleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public int? TotalRow { get; set; } = 0;
    }
}
