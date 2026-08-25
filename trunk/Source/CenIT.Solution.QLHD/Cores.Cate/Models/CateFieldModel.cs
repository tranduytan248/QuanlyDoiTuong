using System;
using TSFramework.App.Attributes;

namespace Cores.Cate.Models
{
    public class CateFieldModel
    {
        public int FieldId { get; set; } = 0;

        [CustomRequired]
        [CustomDisplayName("Field_Label_Code")]
        public string FieldCode { get; set; }

        [CustomRequired]
        [CustomDisplayName("Field_Label_Name")]
        public string FieldName { get; set; }

        [CustomDisplayName("Field_Label_Description")]
        public string Description { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public int? TotalRow { get; set; } = 0;
    }
}
