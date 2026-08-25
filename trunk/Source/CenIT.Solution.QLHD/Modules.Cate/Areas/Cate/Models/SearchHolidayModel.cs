using TSFramework.App.Attributes;

namespace Modules.Cate.Areas.Cate.Models
{
    public class SearchHolidayModel
    {
        /// <summary>
        /// Loại lịch:
        /// 1: Dương lịch
        /// 2: Âm lịch
        /// 3: Cả 2
        /// </summary>
        [CustomDisplayName("Holiday_Label_LunarCalendar")]
        public int? TypeCalendar { get; set; }

        public bool? LunarCalendar => TypeCalendar == 3 ? (bool?)null:  TypeCalendar == 2;
    }
}