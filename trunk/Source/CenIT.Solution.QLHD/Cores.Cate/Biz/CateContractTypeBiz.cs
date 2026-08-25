using System.Collections.Generic;
using System.Linq;
using Cores.Cate.Models;
using TSFramework.App.Models;
using TSFramework.App.Processors;

namespace Cores.Cate.Biz
{
    public class CateContractTypeBiz
    {
        private const string DATA_PROVIDER_NAME = "MCSProvider";
        private readonly string _cateContractTypeDelete = "Cate_ContractType_Delete";
        private readonly string _cateContractTypeGet = "Cate_ContractType_Get";
        private readonly string _cateContractTypeGetByID = "Cate_ContractType_GetByID";

        private readonly string _cateContractTypeSave = "Cate_ContractType_Save";

        private List<CateContractTypeModel> LoadList(out int total, BaseSearchModel search = null)
        {
            search = search ?? new BaseSearchModel
            {
                Search = null,
                Order = "1",
                OrderDir = "ASC",
                StartIndex = 0,
                PageSize = -1
            };

            var lstContractTypes = AppProcessor.ProcedureProvider.ExecuteTypedList<CateContractTypeModel>(
                _cateContractTypeGet,
                DATA_PROVIDER_NAME,
                search.Search,
                search.Order,
                search.OrderDir,
                search.StartIndex,
                search.PageSize);

            total = 0;
            if (lstContractTypes != null && lstContractTypes.Count > 0)
                total = int.Parse(lstContractTypes.First()?.TotalRow.ToString() ?? "0");
            return lstContractTypes;
        }

        /// <summary>
        ///     Lấy toàn bộ danh sách nội dung thực hiên theo bộ lọc
        /// </summary>
        /// <returns></returns>
        public List<CateContractTypeModel> GetList(out int total, BaseSearchModel search = null)
        {
            var lstContractTypes = LoadList(out total, search);
            return lstContractTypes;
        }

        /// <summary>
        ///     Lấy chi tiết theo ID
        /// </summary>
        /// <returns></returns>
        private CateContractTypeModel LoadDetail(int? contractTypeID)
        {
            var contractTypeModel =
                AppProcessor.ProcedureProvider.ExecuteScalarObject<CateContractTypeModel>(_cateContractTypeGetByID,
                    DATA_PROVIDER_NAME,
                    contractTypeID);
            return contractTypeModel;
        }

        /// <summary>
        ///     Lấy chi tiết theo ID
        /// </summary>
        /// <returns></returns>
        public CateContractTypeModel GetById(int? contractTypeID)
        {
            var contractTypeModel = LoadDetail(contractTypeID);
            return contractTypeModel;
        }

        /// <summary>
        ///     Lấy tất cả loại hợp đồng
        /// </summary>
        /// <returns></returns>
        public List<CateContractTypeModel> GetAll()
        {
            var lstContractTypes = LoadList(out _);
            return lstContractTypes;
        }


        /// <summary>
        ///     Lưu thông tin
        /// </summary>
        /// <returns></returns>
        public int Save(CateContractTypeModel model)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_cateContractTypeSave,
                DATA_PROVIDER_NAME,
                model.ContractTypeId,
                model.ContractTypeCode,
                model.ContractTypeName,
                model.PercentAdvance,
                model.ContractSignal,
                model.FileId,
                model.FileName,
                model.UpdatedBy
            );
            return result.GetValueOrDefault(0);
        }

        /// <summary>
        ///     Xóa loại hợp đồng
        /// </summary>
        /// <returns></returns>
        public int Delete(CateContractTypeModel model)
        {
            var result =
                AppProcessor.ProcedureProvider.Execute(_cateContractTypeDelete, DATA_PROVIDER_NAME,
                    model.ContractTypeId);
            return result.GetValueOrDefault(0);
        }
    }
}