using System.Collections.Generic;
using System.Linq;
using Cores.Cate.Models;
using TSFramework.App.Models;
using TSFramework.App.Processors;

namespace Cores.Cate.Biz
{
    public class CateContractTemplateBiz
    {
        private const string DATA_PROVIDER_NAME = "MCSProvider";
        private readonly string _cateContractTemplateDelete = "Cate_Contract_Template_Delete";
        private readonly string _cateContractTemplateGet = "Cate_Contract_Template_Get";
        private readonly string _cateContractTemplateGetByID = "Cate_Contract_Template_GetByID";
        private readonly string _cateContractTemplateSave = "Cate_Contract_Template_Save";

        /// <summary>
        ///     Lấy toàn bộ danh sách theo giá trị lọc
        /// </summary>
        /// <returns>Danh sách Cate_Contract_Template</returns>
        public List<CateContractTemplateModel> LoadList(out int total, BaseSearchModel search)
        {
            search = search ?? new BaseSearchModel
                { Search = null, Order = "1", OrderDir = "ASC", StartIndex = 0, PageSize = -1 };
            var data = AppProcessor.ProcedureProvider.ExecuteTypedList<CateContractTemplateModel>(
                _cateContractTemplateGet,
                DATA_PROVIDER_NAME, search.Search, search.Order, search.OrderDir, search.StartIndex, search.PageSize);
            total = 0;
            if (data != null)
                total = int.Parse(data.First()?.TotalRow.ToString() ?? "0");
            return data;
        }

        /// <summary>
        ///     Lấy toàn bộ danh sách Cate_Contract_Template
        /// </summary>
        /// <returns>Danh sách Cate_Contract_Template</returns>
        public List<CateContractTemplateModel> GetAll()
        {
            var list = LoadList(out _, null);
            return list;
        }

        /// <summary>
        ///     Lấy danh sách Cate_Contract_Template theo ID
        /// </summary>
        /// <returns>Danh sách Cate_Contract_Template</returns>
        public CateContractTemplateModel LoadDetail(string id)
        {
            var data = AppProcessor.ProcedureProvider.ExecuteScalarObject<CateContractTemplateModel>(
                _cateContractTemplateGetByID, DATA_PROVIDER_NAME, id);
            return data;
        }

        /// <summary>
        ///     Xóa danh sách Cate_Contract_Template theo ID
        /// </summary>
        /// <returns> Kết quả thực hiện</returns>
        public int Delete(string id, string userName)
        {
            var result =
                AppProcessor.ProcedureProvider.Execute(_cateContractTemplateDelete, DATA_PROVIDER_NAME, id, userName);
            return result.GetValueOrDefault(0);
        }

        /// <summary>
        ///     Cập nhật danh sách Cate_Contract_Template theo dữ liệu đầu vào
        /// </summary>
        /// <returns> Kết quả thực hiện</returns>
        public int Save(CateContractTemplateModel model)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_cateContractTemplateSave, DATA_PROVIDER_NAME,
                model.Id,
                model.ContractTypeId,
                model.FileName,
                model.FullName,
                model.IsUsed,
                model.Status,
                model.TemplateFields,
                model.TemplateName,
                model.TemplatePath,
                model.TemplatePathCosumer,
                model.TemplateType,
                model.Username,
                model.IndexTabel,
                model.IndexRowInTable,
                model.Version,
                model.UpdatedBy);
            return result.GetValueOrDefault(0);
        }
    }
}