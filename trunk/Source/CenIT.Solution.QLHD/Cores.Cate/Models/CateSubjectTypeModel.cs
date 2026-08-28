using System;
using TSFramework.App.Attributes;

namespace Cores.Cate.Models
{
    public class CateSubjectTypeModel
    {
        public int SubjectTypeId { get; set; } = 0;

        [CustomRequired]
        [CustomDisplayName("SubjectType_Label_Code")]
        public string SubjectTypeCode { get; set; }

        [CustomRequired]
        [CustomDisplayName("SubjectType_Label_Name")]
        public string SubjectTypeName { get; set; }

        [CustomDisplayName("SubjectType_Label_Description")]
        public string Description { get; set; }

        [CustomDisplayName("SubjectType_Label_SortOrder")]
        public int SortOrder { get; set; } = 0;

        [CustomDisplayName("SubjectType_Label_IsActive")]
        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public int? TotalRow { get; set; } = 0;
    }

    public class SearchSubjectTypeModel
    {
        public string Key { get; set; }
    }
}
