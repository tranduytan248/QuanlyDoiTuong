using TSFramework.App.Attributes;

namespace TSFramework.App.Models
{
    public abstract class BaseModel
    {
        public string UpdatedBy { get; set; }

        [CustomDisplayName("Reason_Title")] public virtual string Reason { get; set; }

        public long RowIndex { get; set; } = 0;

        public int? TotalRow { get; set; } = 0;

        public bool CanEdit { get; set; } = true;

        public bool CanDelete { get; set; } = true;
    }
}