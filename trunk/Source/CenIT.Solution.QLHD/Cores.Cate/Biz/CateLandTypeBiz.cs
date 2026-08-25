using System.Collections.Generic;
using System.Linq;
using Cores.Cate.Models;
using TSFramework.App.Models;
using TSFramework.App.Processors;

namespace Cores.Cate.Biz
{
    public class CateLandTypeBiz
    {
        private const string DATA_PROVIDER_NAME = "MCSProvider";
        private readonly string _landtypeDelete = "Cate_LandType_Delete";
        private readonly string _landtypeGet = "Cate_LandType_Get";
        private readonly string _landtypeGetByID = "Cate_LandType_GetByID";
        private readonly string _landtypeSave = "Cate_LandType_Save";

        /// <summary>
        ///     Lấy toàn bộ danh sách theo giá trị lọc
        /// </summary>
        /// <returns>Danh sách Cate_LandType</returns>
        public List<CateLandTypeModel> LoadList(out int total, BaseSearchModel search)
        {
            search = search ?? new BaseSearchModel
            {
                Search = null,
                Order = "1",
                OrderDir = "ASC",
                StartIndex = 0,
                PageSize = -1
            };
            var data = AppProcessor.ProcedureProvider.ExecuteTypedList<CateLandTypeModel>(_landtypeGet,
                DATA_PROVIDER_NAME,
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
        ///     Lấy toàn bộ danh sách loại đất
        /// </summary>
        /// <returns>Danh sách loại đất</returns>
        public List<CateLandTypeModel> GetAll()
        {
            var list = LoadList(out _, null);
            return list;
        }

        /// <summary>
        ///     Lấy thông tin loại đất theo ID
        /// </summary>
        /// <returns>thông tin loại đất</returns>
        public CateLandTypeModel LoadDetail(int id)
        {
            var data = AppProcessor.ProcedureProvider.ExecuteScalarObject<CateLandTypeModel>(_landtypeGetByID,
                DATA_PROVIDER_NAME, id);
            return data;
        }

        /// <summary>
        ///     Xóa loại đất theo ID
        /// </summary>
        /// <returns> Kết quả thực hiện</returns>
        public int Delete(int id)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_landtypeDelete, DATA_PROVIDER_NAME, id);
            return result.GetValueOrDefault(0);
        }

        /// <summary>
        ///     Cập nhật danh sách Cate_LandType theo dữ liệu đầu vào
        /// </summary>
        /// <returns> Kết quả thực hiện</returns>
        public int Save(CateLandTypeModel model, string savedBy)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_landtypeSave, DATA_PROVIDER_NAME
                , model.LandType_ID
                , model.LandTypeCode
                , model.LandTypeName
                , savedBy);
            return result.GetValueOrDefault(0);
        }
    }
}