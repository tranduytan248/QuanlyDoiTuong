using System;
using System.Collections.Generic;
using System.Linq;
using Cores.Cate.Models;
using TSFramework.App.Models;
using TSFramework.App.Processors;

namespace Cores.Cate.Biz
{
    public class CateContentLandBiz
    {
        private const string DATA_PROVIDER_NAME = "MCSProvider";
        private readonly string _contentLandDelete = "Cate_ContentLand_Delete";
        private readonly string _contentLandGet = "Cate_ContentLand_Get";
        private readonly string _contentLandGetByID = "Cate_ContentLand_GetByID";
        private readonly string _contentLandSave = "Cate_ContentLand_Save";

        /// <summary>
        ///     Lấy toàn bộ danh sách theo giá trị lọc
        /// </summary>
        /// <returns>Danh sách Cate_contentLand</returns>
        public List<CateContentLandModel> LoadList(out int total, string typeContractIds, BaseSearchModel search)
        {
            search = search ?? new BaseSearchModel
            {
                Search = null,
                Order = "1",
                OrderDir = "ASC",
                StartIndex = 0,
                PageSize = -1
            };
            var data = AppProcessor.ProcedureProvider.ExecuteTypedList<CateContentLandModel>(_contentLandGet,
                DATA_PROVIDER_NAME,
                typeContractIds,
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
        public List<CateContentLandModel> GetAll(string typeContractIds = null)
        {
            var lstContentLands = LoadList(out _, typeContractIds, null);
            return lstContentLands;
        }

        /// <summary>
        ///     Lấy thông tin diện tích đất theo ID
        /// </summary>
        /// <returns>thông tin diện tích đất</returns>
        public CateContentLandModel LoadDetail(Guid? id)
        {
            var contentLand =
                AppProcessor.ProcedureProvider.ExecuteScalarObject<CateContentLandModel>(_contentLandGetByID,
                    DATA_PROVIDER_NAME, id);
            return contentLand;
        }

        /// <summary>
        ///     Xóa Cate_contentLand theo ID
        /// </summary>
        /// <returns> Kết quả thực hiện</returns>
        public int Delete(Guid? id)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_contentLandDelete, DATA_PROVIDER_NAME, id);
            return result.GetValueOrDefault(0);
        }

        /// <summary>
        ///     Cập nhật danh sách Cate_contentLand theo dữ liệu đầu vào
        /// </summary>
        /// <returns> Kết quả thực hiện</returns>
        public int Save(CateContentLandModel model, string savedBy)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_contentLandSave, DATA_PROVIDER_NAME
                , model.ContentLandId
                , model.ContractTypeId
                , model.ContentLandName
                , savedBy);
            return result.GetValueOrDefault(0);
        }
    }
}