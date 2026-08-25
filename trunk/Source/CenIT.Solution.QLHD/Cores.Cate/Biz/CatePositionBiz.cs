using System.Collections.Generic;
using System.Linq;
using Cores.Cate.Models;
using TSFramework.App.Models;
using TSFramework.App.Processors;

namespace Cores.Cate.Biz
{
    public class CatePositionBiz
    {
        private const string DATA_PROVIDER_NAME = "MCSProvider";
        private readonly string _catePositionDelete = "Cate_Position_Delete";
        private readonly string _catePositionGet = "Cate_Position_Get";
        private readonly string _catePositionGetById = "Cate_Position_GetById";
        private readonly string _catePositionSave = "Cate_Position_Save";

        /// <summary>
        ///     Get danh sách chức vụ
        /// </summary>
        /// <param name="total"></param>
        /// <param name="key"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public List<CatePositionModel> Get(out int total, string key, BaseSearchModel search)
        {
            search = search ?? new BaseSearchModel
            {
                Search = null,
                Order = "0",
                OrderDir = "ASC",
                StartIndex = 0,
                PageSize = -1
            };

            var listPositions = AppProcessor.ProcedureProvider.ExecuteTypedList<CatePositionModel>(_catePositionGet,
                DATA_PROVIDER_NAME,
                key,
                search.Search,
                search.Order,
                search.OrderDir,
                search.StartIndex,
                search.PageSize);

            total = 0;
            if (listPositions != null && listPositions.Count > 0)
                total = int.Parse(listPositions.First()?.TotalRow.ToString() ?? "0");
            return listPositions;
        }

        /// <summary>
        ///     Get chức vụ chi tiết bằng Id
        /// </summary>
        /// <param name="positionId"></param>
        /// <returns></returns>
        private CatePositionModel LoadDetail(int? positionId)
        {
            var lstPositionModels =
                AppProcessor.ProcedureProvider.ExecuteScalarObject<CatePositionModel>(_catePositionGetById,
                    DATA_PROVIDER_NAME, positionId);
            return lstPositionModels;
        }

        /// <summary>
        ///     Lưu thông tin chức vụ vào DB
        /// </summary>
        /// <param name="model"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public int? Save(CatePositionModel model, string username)
        {
            var id = AppProcessor.ProcedureProvider.Execute(_catePositionSave, DATA_PROVIDER_NAME,
                model.PositionID,
                model.PositionCode.Trim(),
                model.PositionName,
                username
            );

            return id;
        }

        /// <summary>
        ///     Xóa chức vụ
        /// </summary>
        /// <param name="model"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool Delete(CatePositionModel model, string username)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_catePositionDelete, DATA_PROVIDER_NAME,
                model.PositionID,
                username);
            return result == model.PositionID;
        }

        /// <summary>
        ///     Lấy thông tin chi tiết chức vụ
        /// </summary>
        /// <param name="positionId"></param>
        /// <returns></returns>
        public CatePositionModel GetById(int? positionId)
        {
            var position = LoadDetail(positionId);
            return position;
        }
    }
}