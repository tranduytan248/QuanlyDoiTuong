using System.Collections.Generic;
using System.Linq;
using Cores.Cate.Models;
using TSFramework.App.Models;
using TSFramework.App.Processors;

namespace Cores.Cate.Biz
{
    public class CateViolationBehaviorBiz
    {
        private const string DATA_PROVIDER_NAME = "MCSProvider";
        private readonly string _cateViolationBehaviorDelete = "Cate_ViolationBehavior_Delete";
        private readonly string _cateViolationBehaviorGet = "Cate_ViolationBehavior_Get";
        private readonly string _cateViolationBehaviorGetById = "Cate_ViolationBehavior_GetById";
        private readonly string _cateViolationBehaviorGetAll = "Cate_ViolationBehavior_GetAll";
        private readonly string _cateViolationBehaviorSave = "Cate_ViolationBehavior_Save";

        public List<CateViolationBehaviorModel> Get(out int total, string key, int? fieldId, BaseSearchModel search)
        {
            search = search ?? new BaseSearchModel
            {
                Search = null,
                Order = "0",
                OrderDir = "ASC",
                StartIndex = 0,
                PageSize = -1
            };

            var listBehaviors = AppProcessor.ProcedureProvider.ExecuteTypedList<CateViolationBehaviorModel>(_cateViolationBehaviorGet,
                DATA_PROVIDER_NAME,
                key,
                fieldId,
                search.Search,
                search.Order,
                search.OrderDir,
                search.StartIndex,
                search.PageSize);

            total = 0;
            if (listBehaviors != null && listBehaviors.Count > 0)
                total = int.Parse(listBehaviors.First()?.TotalRow.ToString() ?? "0");
            return listBehaviors;
        }

        public List<CateViolationBehaviorModel> GetAll(int? fieldId = null)
        {
            return AppProcessor.ProcedureProvider.ExecuteTypedList<CateViolationBehaviorModel>(_cateViolationBehaviorGetAll,
                DATA_PROVIDER_NAME, fieldId);
        }

        public CateViolationBehaviorModel GetById(int? behaviorId)
        {
            if (!behaviorId.HasValue || behaviorId.Value <= 0) return null;
            return AppProcessor.ProcedureProvider.ExecuteScalarObject<CateViolationBehaviorModel>(_cateViolationBehaviorGetById,
                DATA_PROVIDER_NAME, behaviorId.Value);
        }

        public int? Save(CateViolationBehaviorModel model, string username)
        {
            var id = AppProcessor.ProcedureProvider.Execute(_cateViolationBehaviorSave, DATA_PROVIDER_NAME,
                model.BehaviorId,
                model.FieldId,
                model.BehaviorCode != null ? model.BehaviorCode.Trim() : string.Empty,
                model.BehaviorName != null ? model.BehaviorName.Trim() : string.Empty,
                model.Description,
                model.IsActive,
                username
            );

            return id;
        }

        public bool Delete(CateViolationBehaviorModel model, string username)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_cateViolationBehaviorDelete, DATA_PROVIDER_NAME,
                model.BehaviorId,
                username);
            return result == model.BehaviorId;
        }
    }
}
