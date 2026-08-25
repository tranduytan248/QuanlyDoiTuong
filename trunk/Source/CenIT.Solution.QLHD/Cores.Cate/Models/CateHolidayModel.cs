using System;
using System.Collections.Generic;
using System.Linq;
using TSFramework.App.Attributes;
using TSFramework.App.Models;

namespace Cores.Cate.Models
{
    public class CateHolidayModel : BaseSearchModel
    {
        public int HolidayId { get; set; }

        [CustomDisplayName("Holiday_Label_Date")]
        public string Date { get; set; }

        public DateTime RealDate { get; set; }

        [CustomRequired]
        [CustomDisplayName("Holiday_Label_HolidayName")]
        public string HolidayName { get; set; }

        [CustomDisplayName("Holiday_Label_IsPermanent")]
        public bool IsPermanent { get; set; }

        [CustomDisplayName("Holiday_Label_LunarCalendar")]
        public bool IsLunarCalendar { get; set; }

        public int? TotalRow { get; set; } = 0;

        public List<DateTime> SelectedDates { get; set; }

        [CustomRequired]
        [CustomDisplayName("Holiday_Label_Day")]
        public string Day { get; set; }

        [CustomRequired]
        [CustomDisplayName("Holiday_Label_Month")]
        public string Month { get; set; }

        public string Year { get; set; }

        // Mảng các ngày trong tháng
        public List<string> Days { get; set; } = Enumerable.Range(1, 31).Select(day => day.ToString("00")).ToList();

        // Mảng các tháng
        public List<string> Months { get; set; } =
            Enumerable.Range(1, 12).Select(month => month.ToString("00")).ToList();
    }
}