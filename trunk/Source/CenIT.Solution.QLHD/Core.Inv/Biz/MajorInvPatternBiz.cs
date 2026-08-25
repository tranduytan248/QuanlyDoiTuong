using System;
using System.Collections.Generic;
using System.Linq;
using Core.Inv.Models;
using TSFramework.App.Models;
using TSFramework.App.Processors;

namespace Core.Inv.Biz
{
    public class MajorInvPatternBiz
    {
        private const string DATA_PROVIDER_NAME = "MCSProvider";

        private readonly string _majorInvPatternDelete = "Major_Inv_Pattern_Delete";
        private readonly string _majorInvPatternGet = "Major_Inv_Pattern_Get";
        private readonly string _majorInvPatternGetById = "Major_Inv_Pattern_GetById";
        private readonly string _majorInvPatternGetByPattern = "Major_Inv_Pattern_GetByPattern";
        private readonly string _majorInvPatternSave = "Major_Inv_Pattern_Save";

        public List<MajorInvPatternModel> Get(out int total, BaseSearchModel search)
        {
            search = search ?? new BaseSearchModel
            {
                Search = null,
                Order = "0",
                OrderDir = "ASC",
                StartIndex = 0,
                PageSize = -1
            };

            var lstPatterns = AppProcessor.ProcedureProvider.ExecuteTypedList<MajorInvPatternModel>(_majorInvPatternGet,
                DATA_PROVIDER_NAME,
                search.Search,
                search.Order,
                search.OrderDir,
                search.StartIndex,
                search.PageSize);

            total = 0;
            if (lstPatterns != null && lstPatterns.Count > 0)
                total = int.Parse(lstPatterns.First()?.TotalRow.ToString() ?? "0");
            return lstPatterns;
        }

        public MajorInvPatternModel GetById(Guid? cateId)
        {
            var lstPatterns =
                AppProcessor.ProcedureProvider.ExecuteScalarObject<MajorInvPatternModel>(_majorInvPatternGetById,
                    DATA_PROVIDER_NAME, cateId);

            return lstPatterns;
        }

        public List<MajorInvPatternModel> GetByPattern(string pattern)
        {
            var lstPatterns =
                AppProcessor.ProcedureProvider.ExecuteTypedList<MajorInvPatternModel>(_majorInvPatternGetByPattern,
                    DATA_PROVIDER_NAME, pattern);

            return lstPatterns;
        }

        public bool Delete(MajorInvPatternModel model)
        {
            var result =
                AppProcessor.ProcedureProvider.Execute(_majorInvPatternDelete, DATA_PROVIDER_NAME, model.PatternId,
                    model.UpdatedBy);
            return result == 1;
        }

        public List<MajorInvPatternModel> GetAll()
        {
            var lstPatterns = Get(out _, null);
            return lstPatterns;
        }

        public int? Save(MajorInvPatternModel model)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_majorInvPatternSave, DATA_PROVIDER_NAME,
                model.PatternId,
                model.Pattern,
                model.Serial,
                model.IsActive,
                model.UpdatedBy);
            return result;
        }
    }
}