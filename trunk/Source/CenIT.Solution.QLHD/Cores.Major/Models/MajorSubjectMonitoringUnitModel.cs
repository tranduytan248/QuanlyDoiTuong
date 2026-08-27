using System;

namespace Cores.Major.Models
{
    /// <summary>
    /// Thông tin một đơn vị đã phát sinh hồ sơ hoặc ghi nhận vi phạm cho đối tượng.
    /// Dùng cho màn hình / popup xem chi tiết các đơn vị cùng theo dõi / giám sát đối tượng.
    /// </summary>
    public class MajorSubjectMonitoringUnitModel
    {
        public Guid? RecordId { get; set; }
        public string RecordType { get; set; } // "KHAI_BAO" hoặc "VI_PHAM"
        public string RecordTypeName { get; set; } // "Khai báo hồ sơ đối tượng" hoặc "Ghi nhận vi phạm"
        public DateTime? RecordDate { get; set; }
        public string RecordDateStr => RecordDate.HasValue ? RecordDate.Value.ToString("dd/MM/yyyy") : string.Empty;

        public Guid? UnionId { get; set; }
        public string UnitName { get; set; }
        public string ReporterName { get; set; }
        public string ReporterPosition { get; set; }
        public string ReporterPhone { get; set; }
        public string CreatedBy { get; set; }

        // Thông tin chi tiết nếu là vi phạm
        public string FieldNames { get; set; }
        public string BehaviorNames { get; set; }
        public string TreatmentMeasures { get; set; }
        public string Notes { get; set; }
        public string RelatedDocuments { get; set; }
        public string Images { get; set; }

        public bool? IsOwner { get; set; }
    }
}
