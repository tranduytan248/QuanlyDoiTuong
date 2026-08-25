using System;
using System.Collections.Generic;
using System.Linq;
using Cores.Cate.Models;
using TSFramework.App.Models;
using TSFramework.App.Processors;

namespace Cores.Cate.Biz
{
    public class CateHolidayBiz
    {
        private const string DATA_PROVIDER_NAME = "MCSProvider";
        private readonly string _cateHolidayDelete = "Cate_Holiday_Delete";
        private readonly string _cateHolidayGet = "Cate_Holiday_Get";
        private readonly string _cateHolidayGetByID = "Cate_Holiday_GetByID";
        private readonly string _cateHolidaySave = "Cate_Holiday_Save";

        /// <summary>
        ///     Lấy toàn bộ danh sách theo giá trị lọc
        /// </summary>
        /// <returns>Danh sách Cate_Holiday</returns>
        public List<CateHolidayModel> LoadList(out int total, bool? lunarCalendar = null, DateTime? fromDate = null,
            DateTime? toDate = null, BaseSearchModel search = null)
        {
            search = search ?? new BaseSearchModel
            {
                Search = null,
                Order = "1",
                OrderDir = "ASC",
                StartIndex = 0,
                PageSize = -1
            };

            var data = AppProcessor.ProcedureProvider.ExecuteTypedList<CateHolidayModel>(_cateHolidayGet,
                DATA_PROVIDER_NAME, lunarCalendar, fromDate, toDate, search.Search, search.Order, search.OrderDir,
                search.StartIndex, search.PageSize);
            total = 0;
            if (data != null && data.Count > 0)
                total = int.Parse(data.First()?.TotalRow.ToString() ?? "0");
            return data;
        }

        /// <summary>
        ///     Lấy toàn bộ danh sách Cate_Holiday
        /// </summary>
        /// <returns>Danh sách Cate_Holiday</returns>
        public List<CateHolidayModel> GetAll(bool? lunarCalendar = null, DateTime? fromDate = null,
            DateTime? toDate = null)
        {
            var allHolidays = LoadList(out _, lunarCalendar, fromDate, toDate);
            return allHolidays;
        }

        /// <summary>
        ///     Lấy danh sách Cate_Holiday theo ID
        /// </summary>
        /// <returns>Danh sách Cate_Holiday</returns>
        public CateHolidayModel LoadDetail(int id)
        {
            var data = AppProcessor.ProcedureProvider.ExecuteScalarObject<CateHolidayModel>(_cateHolidayGetByID,
                DATA_PROVIDER_NAME, id);
            return data;
        }

        /// <summary>
        ///     Xóa danh sách Cate_Holiday theo ID
        /// </summary>
        /// <returns> Kết quả thực hiện</returns>
        public int Delete(int id, string userName)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_cateHolidayDelete, DATA_PROVIDER_NAME, id, userName);
            return result.GetValueOrDefault(0);
        }

        /// <summary>
        ///     Cập nhật danh sách Cate_Holiday theo dữ liệu đầu vào
        /// </summary>
        /// <returns> Kết quả thực hiện</returns>
        public int Save(CateHolidayModel model, string savedBy)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_cateHolidaySave, DATA_PROVIDER_NAME,
                model.HolidayId,
                model.Date,
                model.HolidayName,
                model.IsPermanent,
                model.IsLunarCalendar,
                savedBy);
            return result.GetValueOrDefault(0);
        }
    }
}