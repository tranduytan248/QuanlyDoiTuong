using System.Collections.Generic;
using System.Linq;
using Cores.Cate.Models;
using TSFramework.App.Models;
using TSFramework.App.Processors;

namespace Cores.Cate.Biz
{
    public class CateMainSectionBiz
    {
        private const string DATA_PROVIDER_NAME = "MCSProvider";
        private readonly string _mainSectionDelete = "Cate_MainSection_Delete";
        private readonly string _mainSectionGet = "Cate_MainSection_Get";
        private readonly string _mainSectionGetByID = "Cate_MainSection_GetByID";
        private readonly string _mainSectionSave = "Cate_MainSection_Save";

        /// <summary>
        ///     Lấy toàn bộ danh sách theo giá trị lọc
        /// </summary>
        /// <returns>Danh sách Cate_MainSection</returns>
        public List<CateMainSectionModel> LoadList(out int total, string typeContractIdsIds, BaseSearchModel search)
        {
            search = search ?? new BaseSearchModel
            {
                Search = null,
                Order = "1",
                OrderDir = "ASC",
                StartIndex = 0,
                PageSize = -1
            };
            var data = AppProcessor.ProcedureProvider.ExecuteTypedList<CateMainSectionModel>(_mainSectionGet,
                DATA_PROVIDER_NAME,
                typeContractIdsIds,
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
        ///     Lấy toàn bộ danh sách điều kiện lập đơn giá
        /// </summary>
        /// <returns>Danh sách điều kiện lập đơn giá</returns>
        public List<CateMainSectionModel> GetAll(string typeContractIds)
        {
            var lstMainSections = LoadList(out _, typeContractIds, null);
            return lstMainSections;
        }

        /// <summary>
        ///     Lấy thông tin điều kiện lập đơn giá theo ID
        /// </summary>
        /// <returns>thông tin điều kiện lập đơn giá</returns>
        public CateMainSectionModel LoadDetail(int id)
        {
            var data = AppProcessor.ProcedureProvider.ExecuteScalarObject<CateMainSectionModel>(_mainSectionGetByID,
                DATA_PROVIDER_NAME, id);
            return data;
        }

        /// <summary>
        ///     Xóa điều kiện lập đơn giá theo ID
        /// </summary>
        /// <returns> Kết quả thực hiện</returns>
        public int Delete(int id)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_mainSectionDelete, DATA_PROVIDER_NAME, id);
            return result.GetValueOrDefault(0);
        }

        /// <summary>
        ///     Cập nhật danh sách Cate_MainSection theo dữ liệu đầu vào
        /// </summary>
        /// <returns> Kết quả thực hiện</returns>
        public int Save(CateMainSectionModel model, string savedBy)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_mainSectionSave, DATA_PROVIDER_NAME,
                model.MainSectionId,
                model.Cate_ContractTypeId,
                model.MainSectionName, savedBy);
            return result.GetValueOrDefault(0);
        }
    }
}