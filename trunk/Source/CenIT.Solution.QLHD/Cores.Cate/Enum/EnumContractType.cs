using System.ComponentModel;

namespace Cores.Cate.Enum
{
    public enum EnumContractType
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
        ///     Thẩm định
        /// </summary>
        [Description("TypeContract_Expertise")]
        Expertise = 3,

        /// <summary>
        ///     Không xác định thời hạn
        /// </summary>
        [Description("TypeContract_Indefinite")]
        Indefinite = 4,

        /// <summary>
        ///     Cập nhật quy hoạch => Giống thẩm định
        /// </summary>
        [Description("TypeContract_UpdateZoning")]
        UpdateZoning = 5
    }
}