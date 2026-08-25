using System;
using TSFramework.App.Attributes;

namespace Cores.Cate.Models
{
    public class CatePositionModel
    {
        public int PositionID { get; set; } = 0;

        [CustomRequired]
        [CustomDisplayName("Position_Label_Code")]
        public string PositionCode { get; set; }

        [CustomRequired]
        [CustomDisplayName("Position_Label_Name")]
        public string PositionName { get; set; }

        public bool IsDelete { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string LastModifiedOn { get; set; }
        public DateTime? LastModifiedBy { get; set; }
        public int? TotalRow { get; set; } = 0;
    }
}