using System.Collections.Generic;
using System.Linq;
using Cores.Cate.Models;
using TSFramework.App.Models;
using TSFramework.App.Processors;

namespace Cores.Cate.Biz
{
    public class CateContractStatusBiz
    {
        private const string DATA_PROVIDER_NAME = "MCSProvider";
        private readonly string _cateContractStatusDelete = "Cate_ContractStatus_Delete";
        private readonly string _cateContractStatusGet = "Cate_ContractStatus_Get";
        private readonly string _cateContractStatusGetById = "Cate_ContractStatus_GetById";
        private readonly string _cateContractStatusSave = "Cate_ContractStatus_Save";

        /// <summary>
        ///     Get danh sách trạng thái hợp đồng
        /// </summary>
        /// <param name="total"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public List<CateContractStatusModel> Get(out int total, BaseSearchModel search)
        {
            search = search ?? new BaseSearchModel
            {
                Search = null,
                Order = "0",
                OrderDir = "ASC",
                StartIndex = 0,
                PageSize = -1
            };

            var listContractStatus = AppProcessor.ProcedureProvider.ExecuteTypedList<CateContractStatusModel>(
                _cateContractStatusGet,
                DATA_PROVIDER_NAME,
                search.Search,
                search.Order,
                search.OrderDir,
                search.StartIndex,
                search.PageSize);

            total = 0;
            if (listContractStatus != null && listContractStatus.Count > 0)
                total = int.Parse(listContractStatus.First()?.TotalRow.ToString() ?? "0");
            return listContractStatus;
        }

        /// <summary>
        ///     Get trạng thái hợp đồng chi tiết bằng Id
        /// </summary>
        /// <param name="contractStatusId"></param>
        /// <returns></returns>
        private CateContractStatusModel LoadDetail(int? contractStatusId)
        {
            var lstContractStatusModels =
                AppProcessor.ProcedureProvider.ExecuteScalarObject<CateContractStatusModel>(_cateContractStatusGetById,
                    DATA_PROVIDER_NAME, contractStatusId);
            return lstContractStatusModels;
        }

        /// <summary>
        ///     Lưu thông tin trạng thái hợp đồng vào DB
        /// </summary>
        /// <param name="model"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public int? Save(CateContractStatusModel model, string username)
        {
            var id = AppProcessor.ProcedureProvider.Execute(_cateContractStatusSave, DATA_PROVIDER_NAME,
                model.ContractStatusId,
                model.ContractStatusCode.Trim(),
                model.ContractStatusName,
                model.EnumId,
                model.IsEContract,
                username
            );

            return id;
        }

        /// <summary>
        ///     Xóa trạng thái hợp đồng
        /// </summary>
        /// <param name="model"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool Delete(CateContractStatusModel model, string username)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_cateContractStatusDelete, DATA_PROVIDER_NAME,
                model.ContractStatusId,
                username);
            return result == model.ContractStatusId;
        }

        /// <summary>
        ///     Lấy thông tin chi tiết trạng thái hợp đồng
        /// </summary>
        /// <param name="contractStatusId"></param>
        /// <returns></returns>
        public CateContractStatusModel GetById(int? contractStatusId)
        {
            var contratStatus = LoadDetail(contractStatusId);
            return contratStatus;
        }
    }
}