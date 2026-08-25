using System;
using System.Collections.Generic;
using System.Linq;
using Cores.Cate.Models;
using TSFramework.App.Models;
using TSFramework.App.Processors;

namespace Cores.Cate.Biz
{
    public class CateLandCalculationBiz
    {
        private const string DATA_PROVIDER_NAME = "MCSProvider";
        private readonly string _landCalculationDelete = "Cate_LandCalculation_Delete";
        private readonly string _landCalculationGet = "Cate_LandCalculation_Get";
        private readonly string _landCalculationGetByID = "Cate_LandCalculation_GetByID";
        private readonly string _landCalculationSave = "Cate_LandCalculation_Save";

        /// <summary>
        ///     Lấy toàn bộ danh sách theo giá trị lọc
        /// </summary>
        /// <returns>Danh sách Cate_landCalculation</returns>
        public List<CateLandCalculationModel> LoadList(out int total, string contentLands, BaseSearchModel search)
        {
            search = search ?? new BaseSearchModel
            {
                Search = null,
                Order = "1",
                OrderDir = "ASC",
                StartIndex = 0,
                PageSize = -1
            };
            var data = AppProcessor.ProcedureProvider.ExecuteTypedList<CateLandCalculationModel>(_landCalculationGet,
                DATA_PROVIDER_NAME,
                contentLands,
                search.Search,
                search.Order,
                search.OrderDir,
                search.StartIndex,
                search.PageSize);
            total = 0;
            if (data != null && data.Count > 0)
                total = int.Parse(data.First()?.TotalRow.ToString() ?? "0");
            return data;
        }

        /// <summary>
        ///     Lấy toàn bộ danh sách diện tích đất
        /// </summary>
        /// <returns>Danh sách diện tích đất</returns>
        public List<CateLandCalculationModel> GetAll(string contentLands = null)
        {
            var list = LoadList(out _, contentLands, null);
            return list;
        }

        /// <summary>
        ///     Lấy thông tin diện tích đất theo ID
        /// </summary>
        /// <returns>thông tin diện tích đất</returns>
        public CateLandCalculationModel LoadDetail(Guid? id)
        {
            var data = AppProcessor.ProcedureProvider.ExecuteScalarObject<CateLandCalculationModel>(
                _landCalculationGetByID, DATA_PROVIDER_NAME, id);
            return data;
        }

        /// <summary>
        ///     Xóa diện tích đất theo ID
        /// </summary>
        /// <returns> Kết quả thực hiện</returns>
        public int Delete(Guid? id)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_landCalculationDelete, DATA_PROVIDER_NAME, id);
            return result.GetValueOrDefault(0);
        }

        /// <summary>
        ///     Cập nhật danh sách Cate_landCalculation theo dữ liệu đầu vào
        /// </summary>
        /// <returns> Kết quả thực hiện</returns>
        public int Save(CateLandCalculationModel model, string savedBy)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_landCalculationSave, DATA_PROVIDER_NAME
                , model.LandCalculationId
                , model.ContentLandId
                , model.Condition
                , model.Recipe
                , model.Percentage
                , savedBy);
            return result.GetValueOrDefault(0);
        }
    }
}