using System.Collections.Generic;
using System.Linq;
using Cores.Cate.Models;
using TSFramework.App.Models;
using TSFramework.App.Processors;

namespace Cores.Cate.Biz
{
    public class CateSubjectTypeBiz
    {
        private const string DATA_PROVIDER_NAME = "MCSProvider";
        private readonly string _cateSubjectTypeDelete = "Cate_SubjectType_Delete";
        private readonly string _cateSubjectTypeGet = "Cate_SubjectType_Get";
        private readonly string _cateSubjectTypeGetById = "Cate_SubjectType_GetById";
        private readonly string _cateSubjectTypeGetAll = "Cate_SubjectType_GetAll";
        private readonly string _cateSubjectTypeSave = "Cate_SubjectType_Save";
        private readonly string _cateSubjectTypeToggleStatus = "Cate_SubjectType_ToggleStatus";

        public List<CateSubjectTypeModel> Get(out int total, string key, BaseSearchModel search)
        {
            search = search ?? new BaseSearchModel
            {
                Search = null,
                Order = "0",
                OrderDir = "ASC",
                StartIndex = 0,
                PageSize = -1
            };

            var list = AppProcessor.ProcedureProvider.ExecuteTypedList<CateSubjectTypeModel>(_cateSubjectTypeGet,
                DATA_PROVIDER_NAME,
                key,
                search.Search,
                search.Order,
                search.OrderDir,
                search.StartIndex,
                search.PageSize);

            total = 0;
            if (list != null && list.Count > 0)
                total = int.Parse(list.First()?.TotalRow.ToString() ?? "0");
            return list;
        }

        public List<CateSubjectTypeModel> GetAll()
        {
            return AppProcessor.ProcedureProvider.ExecuteTypedList<CateSubjectTypeModel>(_cateSubjectTypeGetAll, DATA_PROVIDER_NAME);
        }

        public CateSubjectTypeModel GetById(int? subjectTypeId)
        {
            if (!subjectTypeId.HasValue || subjectTypeId.Value <= 0) return null;
            return AppProcessor.ProcedureProvider.ExecuteScalarObject<CateSubjectTypeModel>(_cateSubjectTypeGetById, DATA_PROVIDER_NAME, subjectTypeId.Value);
        }

        public int? Save(CateSubjectTypeModel model, string username)
        {
            var id = AppProcessor.ProcedureProvider.Execute(_cateSubjectTypeSave, DATA_PROVIDER_NAME,
                model.SubjectTypeId,
                model.SubjectTypeCode != null ? model.SubjectTypeCode.Trim() : string.Empty,
                model.SubjectTypeName != null ? model.SubjectTypeName.Trim() : string.Empty,
                model.Description != null ? model.Description.Trim() : string.Empty,
                model.SortOrder,
                model.IsActive,
                username);
            return id;
        }

        public int? Delete(int subjectTypeId, string username)
        {
            if (subjectTypeId <= 0) return null;
            return AppProcessor.ProcedureProvider.Execute(_cateSubjectTypeDelete, DATA_PROVIDER_NAME, subjectTypeId, username);
        }

        public int? ToggleStatus(int subjectTypeId, string username)
        {
            if (subjectTypeId <= 0) return null;
            return AppProcessor.ProcedureProvider.Execute(_cateSubjectTypeToggleStatus, DATA_PROVIDER_NAME, subjectTypeId, username);
        }
    }
}
