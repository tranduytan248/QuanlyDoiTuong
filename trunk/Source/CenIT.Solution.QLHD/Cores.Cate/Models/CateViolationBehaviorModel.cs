using System;
using TSFramework.App.Attributes;

namespace Cores.Cate.Models
{
    public class CateViolationBehaviorModel
    {
        public int BehaviorId { get; set; } = 0;

        [CustomRequired]
        [CustomDisplayName("ViolationBehavior_Label_Field")]
        public int FieldId { get; set; }

        public string FieldName { get; set; }
        public string FieldCode { get; set; }

        [CustomRequired]
        [CustomDisplayName("ViolationBehavior_Label_Code")]
        public string BehaviorCode { get; set; }

        [CustomRequired]
        [CustomDisplayName("ViolationBehavior_Label_Name")]
        public string BehaviorName { get; set; }

        [CustomDisplayName("ViolationBehavior_Label_Description")]
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
