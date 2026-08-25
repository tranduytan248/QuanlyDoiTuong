using System.Collections.Generic;
using System.Linq;
using Cores.Cate.Models;
using TSFramework.App.Models;
using TSFramework.App.Processors;

namespace Cores.Cate.Biz
{
    public class CatePriceBiz
    {
        private const string DATA_PROVIDER_NAME = "MCSProvider";
        private readonly string _priceDelete = "Cate_Price_Delete";
        private readonly string _priceGet = "Cate_Price_Get";
        private readonly string _priceGetByID = "Cate_Price_GetByID";
        private readonly string _priceSave = "Cate_Price_Save";

        /// <summary>
        ///     Lấy toàn bộ danh sách theo giá trị lọc
        /// </summary>
        /// <returns>Danh sách Cate_Price</returns>
        public List<CatePriceModel> LoadList(out int total, CatePriceSearchModel searchModel, BaseSearchModel search)
        {
            search = search ?? new BaseSearchModel
            {
                Search = null,
                Order = "1",
                OrderDir = "ASC",
                StartIndex = 0,
                PageSize = -1
            };
            var data = AppProcessor.ProcedureProvider.ExecuteTypedList<CatePriceModel>(_priceGet,
                DATA_PROVIDER_NAME,
                searchModel.SubSectionId,
                //searchModel.Unit,
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
        public List<CatePriceModel> GetAll()
        {
            var list = LoadList(out _, null, null);
            return list;
        }

        /// <summary>
        ///     Lấy thông tin diện tích đất theo ID
        /// </summary>
        /// <returns>thông tin diện tích đất</returns>
        public CatePriceModel LoadDetail(int id)
        {
            var data = AppProcessor.ProcedureProvider.ExecuteScalarObject<CatePriceModel>(_priceGetByID,
                DATA_PROVIDER_NAME, id);
            return data;
        }

        /// <summary>
        ///     Xóa diện tích đất theo ID
        /// </summary>
        /// <returns> Kết quả thực hiện</returns>
        public int Delete(int id)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_priceDelete, DATA_PROVIDER_NAME, id);
            return result.GetValueOrDefault(0);
        }

        /// <summary>
        ///     Cập nhật danh sách Cate_Price theo dữ liệu đầu vào
        /// </summary>
        /// <returns> Kết quả thực hiện</returns>
        public int Save(CatePriceModel model, string savedBy)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_priceSave, DATA_PROVIDER_NAME
                , model.PriceId
                , model.Cate_SubSectionId
                , model.Unit
                , model.Price
                , savedBy);
            return result.GetValueOrDefault(0);
        }
    }
}