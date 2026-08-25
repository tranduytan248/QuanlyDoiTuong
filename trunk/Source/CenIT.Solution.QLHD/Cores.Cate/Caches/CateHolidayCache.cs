using System;
using System.Collections.Generic;
using System.ComponentModel;
using Cores.Cate.Biz;
using Cores.Cate.Models;
using TSFramework.App.Models;
using TSFramework.Core.Members.Caching;
using TSFramework.Core.Utils;

namespace Cores.Cate.Caches
{
    public class CateHolidayCache : CacheLayer
    {
        private CateHolidayBiz _cateHolidaysApi;
        protected override string[] MasterCacheKeyArray => new[] { "Cate_HolidaysCache", "CENIT.APP.Cache" };

        private CateHolidayBiz Api => _cateHolidaysApi ?? (_cateHolidaysApi = new CateHolidayBiz());

        /// <summary>
        ///     Xóa theo ID
        /// </summary>
        /// <returns>Kết quả thực hiện</returns>
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public int Delete(CateHolidayModel model, string deletedBy)
        {
            var isDeleted = Api.Delete(model.HolidayId, deletedBy);
            if (isDeleted > 0) InvalidateCache();
            return isDeleted;
        }

        /// <summary>
        ///     Lưu thông tin Cate_Holidays
        /// </summary>
        /// <returns>Kết quả thực hiện</returns>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public int Save(CateHolidayModel model, string savedBy)
        {
            var isSaved = Api.Save(model, savedBy);
            if (isSaved > 0) InvalidateCache();
            return isSaved;
        }

        /// <summary>
        ///     Lấy toàn bộ danh sách Cate_Holidays
        /// </summary>
        /// <returns>Kết quả thực hiện</returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CateHolidayModel> GetAll(bool? lunarCalendar = null, DateTime? fromDate = null,
            DateTime? toDate = null)
        {
            var rawKey = $"AllHolidays-{lunarCalendar}-{fromDate}-{toDate}";
            if (GetCacheItem(rawKey) is List<CateHolidayModel> data) return data;
            data = Api.GetAll(lunarCalendar);
            AddCacheItem(rawKey, data);
            return data;
        }

        /// <summary>
        ///     Lấy thông tinCate_Holidays theo ID
        /// </summary>
        /// <returns>Kết quả thực hiện</returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public CateHolidayModel GetById(int id)
        {
            if (id < 0) return null;
            var rawKey = string.Concat("GetCate_HolidaysByID_", id);
            if (GetCacheItem(rawKey) is CateHolidayModel data) return data;
            data = Api.LoadDetail(id);
            AddCacheItem(rawKey, data);
            return data;
        }

        /// <summary>
        ///     Lấy toàn bộ danh sách Cate_Holidays
        /// </summary>
        /// <returns>Kết quả thực hiện</returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CateHolidayModel> Get(out int total, bool? lunarCalendar = null, DateTime? fromDate = null,
            DateTime? toDate = null, BaseSearchModel search = null)
        {
            var rawKey = $"ListHolidays-{lunarCalendar}-{fromDate}-{toDate}-{EHashMD5.FromObject(search)}";
            var rawKeyTotal = string.Concat(rawKey, "-Total");
            var cacheTotal = (int?)GetCacheItem(rawKeyTotal);
            total = cacheTotal ?? 0;
            if (GetCacheItem(rawKey) is List<CateHolidayModel> data) return data;
            data = Api.LoadList(out total, lunarCalendar, fromDate, toDate, search);
            AddCacheItem(rawKey, data);
            AddCacheItem(rawKeyTotal, total);
            return data;
        }
    }
}