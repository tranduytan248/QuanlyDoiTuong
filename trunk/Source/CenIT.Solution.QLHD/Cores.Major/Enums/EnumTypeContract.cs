using System.ComponentModel;

namespace Cores.Major.Enums
{
    public enum EnumTypeContract
    {
        /// <summary>
        ///     Đo vẽ đất
        /// </summary>
        [Description("TypeContract_MeasureDraw")]
        MeasureDraw = 1,

        /// <summary>
        ///     Cắm mốc ranh giới
        /// </summary>
        [Description("TypeContract_BoundaryMarkers")]
        BoundaryMarkers = 2,

        /// <summary>
        ///     Thẩm định đo đạc
        /// </summary>
        [Description("TypeContract_Expertise")]
        Expertise = 3,

        /// <summary>
        ///     Cập nhật quy hoạch
        /// </summary>
        [Description("TypeContract_UpdateZoning")]
        UpdateZoning = 4
    }
}