using System.Collections.Generic;
using System.Linq;
using Cores.Cate.Models;
using TSFramework.App.Models;
using TSFramework.App.Processors;

namespace Cores.Cate.Biz
{
    public class CateFieldBiz
    {
        private const string DATA_PROVIDER_NAME = "MCSProvider";
        private readonly string _cateFieldDelete = "Cate_Field_Delete";
        private readonly string _cateFieldGet = "Cate_Field_Get";
        private readonly string _cateFieldGetById = "Cate_Field_GetById";
        private readonly string _cateFieldGetAll = "Cate_Field_GetAll";
        private readonly string _cateFieldSave = "Cate_Field_Save";

        public List<CateFieldModel> Get(out int total, string key, BaseSearchModel search)
        {
            search = search ?? new BaseSearchModel
            {
                Search = null,
                Order = "0",
                OrderDir = "ASC",
                StartIndex = 0,
                PageSize = -1
            };

            var listFields = AppProcessor.ProcedureProvider.ExecuteTypedList<CateFieldModel>(_cateFieldGet,
                DATA_PROVIDER_NAME,
                key,
                search.Search,
                search.Order,
                search.OrderDir,
                search.StartIndex,
                search.PageSize);

            total = 0;
            if (listFields != null && listFields.Count > 0)
                total = int.Parse(listFields.First()?.TotalRow.ToString() ?? "0");
            return listFields;
        }

        public List<CateFieldModel> GetAll()
        {
            return AppProcessor.ProcedureProvider.ExecuteTypedList<CateFieldModel>(_cateFieldGetAll, DATA_PROVIDER_NAME);
        }

        public CateFieldModel GetById(int? fieldId)
        {
            if (!fieldId.HasValue || fieldId.Value <= 0) return null;
            return AppProcessor.ProcedureProvider.ExecuteScalarObject<CateFieldModel>(_cateFieldGetById, DATA_PROVIDER_NAME, fieldId.Value);
        }

        public int? Save(CateFieldModel model, string username)
        {
            var id = AppProcessor.ProcedureProvider.Execute(_cateFieldSave, DATA_PROVIDER_NAME,
                model.FieldId,
                model.FieldCode != null ? model.FieldCode.Trim() : string.Empty,
                model.FieldName != null ? model.FieldName.Trim() : string.Empty,
                model.Description,
                model.IsActive,
                username
            );

            return id;
        }

        public bool Delete(CateFieldModel model, string username)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_cateFieldDelete, DATA_PROVIDER_NAME,
                model.FieldId,
                username);
            return result == model.FieldId;
        }
    }
}
