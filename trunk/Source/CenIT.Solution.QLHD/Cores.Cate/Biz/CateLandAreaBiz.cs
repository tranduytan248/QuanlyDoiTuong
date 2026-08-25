using System.Collections.Generic;
using System.Linq;
using Cores.Cate.Models;
using TSFramework.App.Models;
using TSFramework.App.Processors;

namespace Cores.Cate.Biz
{
    public class CateLandAreaBiz
    {
        private const string DATA_PROVIDER_NAME = "MCSProvider";
        private readonly string _landAreaDelete = "Cate_LandArea_Delete";
        private readonly string _landAreaGet = "Cate_LandArea_Get";
        private readonly string _landAreaGetByID = "Cate_LandArea_GetByID";
        private readonly string _landAreaSave = "Cate_LandArea_Save";

        /// <summary>
        ///     Lấy toàn bộ danh sách theo giá trị lọc
        /// </summary>
        /// <returns>Danh sách Cate_LandArea</returns>
        public List<CateLandAreaModel> LoadList(out int total, CateLandAreaSearchModel searchModel,
            BaseSearchModel search)
        {
            search = search ?? new BaseSearchModel
            {
                Search = null,
                Order = "1",
                OrderDir = "ASC",
                StartIndex = 0,
                PageSize = -1
            };
            var data = AppProcessor.ProcedureProvider.ExecuteTypedList<CateLandAreaModel>(_landAreaGet,
                DATA_PROVIDER_NAME,
                searchModel.LandType_ID,
                searchModel.TuKhoa,
                search.Search,
                search.Order,
                search.OrderDir,
                search.StartIndex,
                search.PageSize);
            total = 0;
            if (data != null && data.Count > 0)
                total = int.Parse(data.First()?.TotalRecord.ToString() ?? "0");
            return data;
        }

        /// <summary>
        ///     Lấy toàn bộ danh sách diện tích đất
        /// </summary>
        /// <returns>Danh sách diện tích đất</returns>
        public List<CateLandAreaModel> GetAll()
        {
            var list = LoadList(out _, null, null);
            return list;
        }

        /// <summary>
        ///     Lấy thông tin diện tích đất theo ID
        /// </summary>
        /// <returns>thông tin diện tích đất</returns>
        public CateLandAreaModel LoadDetail(int id)
        {
            var data = AppProcessor.ProcedureProvider.ExecuteScalarObject<CateLandAreaModel>(_landAreaGetByID,
                DATA_PROVIDER_NAME, id);
            return data;
        }

        /// <summary>
        ///     Xóa diện tích đất theo ID
        /// </summary>
        /// <returns> Kết quả thực hiện</returns>
        public int Delete(int id)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_landAreaDelete, DATA_PROVIDER_NAME, id);
            return result.GetValueOrDefault(0);
        }

        /// <summary>
        ///     Cập nhật danh sách Cate_LandArea theo dữ liệu đầu vào
        /// </summary>
        /// <returns> Kết quả thực hiện</returns>
        public int Save(CateLandAreaModel model, string savedBy)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_landAreaSave, DATA_PROVIDER_NAME
                , model.LandArea_ID
                , model.LandType_ID
                , model.LandSize
                , model.Unit
                , model.UnitPrice
                , savedBy);
            return result.GetValueOrDefault(0);
        }
    }
}