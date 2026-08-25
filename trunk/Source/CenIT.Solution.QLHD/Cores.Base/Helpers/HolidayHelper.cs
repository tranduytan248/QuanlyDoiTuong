using System;
using System.Collections.Generic;
using Cores.Cate.Caches;
using TSFramework.Core.Helpers;

namespace Cores.Base.Helpers
{
    public class HolidayHelper
    {
        public static List<DateTime> GetListHolidays(int year)
        {
            var cateHolidayCache = new CateHolidayCache();
            var holidays = cateHolidayCache.GetAll();

            var listHolidays = new List<DateTime>();

            foreach (var holiday in holidays)
                // Kiểm tra nếu ngày không cố định
                if (!holiday.IsPermanent)
                {
                    // kiểm tra năm có đúng năm cần lấy không
                    if (DateTime.ParseExact(holiday.Date, "dd/MM/yyyy", null).Year == year)
                    {
                        // Nếu là ngày âm lịch, chuyển đổi sang ngày dương lịch
                        if (holiday.IsLunarCalendar)
                        {
                            var lunarDay = int.Parse(holiday.Date.Split('/')[0]);
                            var lunarMonth = int.Parse(holiday.Date.Split('/')[1]);
                            var lunarYear = year; // Sử dụng năm hiện tại
                            var lunarLeap =
                                ConvertDateHelper.IsLeapLunarYear(lunarYear)
                                    ? 1
                                    : 0; // Xác định xem năm âm lịch có phải là năm nhuận hay không
                            var timeZone = 7.0; // Múi giờ là 7 (cho Việt Nam)

                            var solarDate = ConvertDateHelper.convertLunar2Solar(lunarDay, lunarMonth, lunarYear,
                                lunarLeap, timeZone);

                            // Thêm ngày dương lịch đã chuyển đổi vào danh sách
                            listHolidays.Add(new DateTime(solarDate[2], solarDate[1], solarDate[0]));
                        }
                        else // Nếu là ngày dương lịch, không cần chuyển đổi, chỉ cần thêm vào danh sách
                        {
                            listHolidays.Add(DateTime.ParseExact(holiday.Date, "dd/MM/yyyy", null));
                        }
                    }
                }
                // Kiểm tra xem ngày trong danh sách là ngày âm lịch hay dương lịch
                else if (holiday.IsLunarCalendar)
                {
                    // Nếu là ngày âm lịch, chuyển đổi sang ngày dương lịch
                    var lunarDay = int.Parse(holiday.Date.Split('/')[0]);
                    var lunarMonth = int.Parse(holiday.Date.Split('/')[1]);
                    var lunarYear = year; // Sử dụng năm hiện tại
                    var lunarLeap =
                        ConvertDateHelper.IsLeapLunarYear(lunarYear)
                            ? 1
                            : 0; // Xác định xem năm âm lịch có phải là năm nhuận hay không
                    var timeZone = 7.0; // Múi giờ là 7 (cho Việt Nam)

                    var solarDate =
                        ConvertDateHelper.convertLunar2Solar(lunarDay, lunarMonth, lunarYear, lunarLeap, timeZone);

                    // Thêm ngày dương lịch đã chuyển đổi vào danh sách
                    listHolidays.Add(new DateTime(solarDate[2], solarDate[1], solarDate[0]));
                }
                else
                {
                    // Nếu là ngày dương lịch, không cần chuyển đổi, chỉ cần thêm vào danh sách
                    listHolidays.Add(DateTime.ParseExact(holiday.Date + "/" + year, "dd/MM/yyyy", null));
                }

            // Xác định ngày đầu tiên của năm
            var startDate = new DateTime(year, 1, 1);
            // Duyệt qua từng ngày trong năm
            for (var date = startDate; date.Year == year; date = date.AddDays(1))
                // Kiểm tra nếu ngày đó là thứ Bảy hoặc Chủ nhật
                if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                    listHolidays.Add(date);

            return listHolidays;
        }
    }
}