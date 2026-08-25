using System;
using TSFramework.App.Attributes;
using TSFramework.App.Models;

namespace Core.Inv.Models
{
    public class MajorInvPatternModel : BaseModel
    {
        public Guid? PatternId { get; set; }

        [CustomDisplayName("InvPattern_Pattern")]
        [CustomRequired]
        public string Pattern { get; set; }

        [CustomDisplayName("InvPattern_Serial")]
        [CustomRequired]
        public string Serial { get; set; }

        public int TotalRemainingInv { get; set; } = 0;

        [CustomDisplayName("InvPattern_Status")]
        public int Status { get; set; } = 0;

        [CustomDisplayName("InvPattern_Status")]
        public string StatusName { get; set; }

        [CustomDisplayName("InvPattern_Status_Using")]
        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string CreatedBy { get; set; }
    }
}