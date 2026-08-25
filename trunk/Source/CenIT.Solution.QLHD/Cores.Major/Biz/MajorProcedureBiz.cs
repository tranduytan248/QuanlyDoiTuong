using System;
using System.Collections.Generic;
using System.Linq;
using Cores.Major.Models;
using TSFramework.App.Models;
using TSFramework.App.Processors;

namespace Cores.Major.Biz
{
    public class MajorProcedureBiz
    {
        private const string DATA_PROVIDER_NAME = "MCSProvider";
        private readonly string _majorProcedureClone = "Major_Procedure_Clone";
        private readonly string _majorProcedureDelete = "Major_Procedure_Delete";
        private readonly string _majorProcedureGet = "Major_Procedure_Get";
        private readonly string _majorProcedureGetById = "Major_Procedure_GetById";
        private readonly string _majorProcedureGetByUnionId = "Major_Procedure_GetByUnionId";
        private readonly string _majorProcedureSave = "Major_Procedure_Save";
        private readonly string _majorProcedureToggleStatus = "Major_Procedure_ToggleStatus";

        public List<MajorProcedureModel> Get(out int total, string unionIds = null, string typeContracts = null,
            BaseSearchModel search = null)
        {
            search = search ?? new BaseSearchModel
            {
                Search = null,
                Order = "1",
                OrderDir = "ASC",
                StartIndex = 0,
                PageSize = -1
            };

            var listProcedures = AppProcessor.ProcedureProvider.ExecuteTypedList<MajorProcedureModel>(
                _majorProcedureGet,
                DATA_PROVIDER_NAME,
                unionIds, typeContracts,
                search.Search,
                search.Order,
                search.OrderDir,
                search.StartIndex,
                search.PageSize);

            total = 0;
            if (listProcedures != null && listProcedures.Count > 0)
                total = int.Parse(listProcedures.First()?.TotalRow.ToString() ?? "0");
            return listProcedures;
        }

        private MajorProcedureModel LoadDetail(Guid? procedureId)
        {
            var lstProcedures =
                AppProcessor.ProcedureProvider.ExecuteScalarObject<MajorProcedureModel>(_majorProcedureGetById,
                    DATA_PROVIDER_NAME, procedureId);

            return lstProcedures;
        }

        public int? Delete(MajorProcedureModel model)
        {
            var result =
                AppProcessor.ProcedureProvider.Execute(_majorProcedureDelete, DATA_PROVIDER_NAME, model.ProcedureId,
                    model.Reason, model.UpdatedBy);
            return result;
        }

        public List<MajorProcedureModel> GetAll(string unionIds = null, string typeContracts = null)
        {
            var listProcedures = Get(out _, unionIds, typeContracts);
            return listProcedures;
        }

        public MajorProcedureModel GetById(Guid? procedureId)
        {
            var procedure = LoadDetail(procedureId);
            return procedure;
        }

        public int? Save(MajorProcedureModel model)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_majorProcedureSave, DATA_PROVIDER_NAME,
                model.ProcedureId,
                model.ProcedureCode,
                model.ProcedureName,
                model.ProcedureDesc,
                model.ApplyFrom,
                model.ExpiredOn,
                model.Version,
                model.ContractTypeId,
                model.ContractTypeName,
                model.SelectedUnions,
                model.Reason,
                model.UpdatedBy);

            return result;
        }

        public int? Clone(MajorProcedureModel model)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_majorProcedureClone, DATA_PROVIDER_NAME,
                model.ProcedureId,
                model.ProcedureCode,
                model.ProcedureName,
                model.ProcedureDesc,
                model.ApplyFrom,
                model.ExpiredOn,
                model.Version,
                model.ContractTypeId,
                model.ContractTypeName,
                model.UpdatedBy);

            return result;
        }

        public List<MajorProcedureModel> GetViaUnion(Guid? unionId)
        {
            var lstProcs =
                AppProcessor.ProcedureProvider.ExecuteTypedList<MajorProcedureModel>(_majorProcedureGetByUnionId,
                    DATA_PROVIDER_NAME, unionId);

            return lstProcs;
        }

        public bool ToggleStatus(MajorProcedureModel model)
        {
            var result =
                AppProcessor.ProcedureProvider.Execute(_majorProcedureToggleStatus, DATA_PROVIDER_NAME,
                    model.ProcedureId, model.Reason, model.UpdatedBy);
            return result == 1;
        }
    }
}