using System;
using System.Collections.Generic;
using System.Linq;
using Cores.Major.Models;
using TSFramework.App.Models;
using TSFramework.App.Processors;

namespace Cores.Major.Biz
{
    public class MajorProcedureStepBiz
    {
        private const string DATA_PROVIDER_NAME = "MCSProvider";
        private readonly string _majorProcedureStepDelete = "Major_Procedure_Step_Delete";
        private readonly string _majorProcedureStepGet = "Major_Procedure_Step_Get";
        private readonly string _majorProcedureStepGetById = "Major_Procedure_Step_GetById";
        private readonly string _majorProcedureStepGetByKey = "Major_Procedure_Step_GetByKey";
        private readonly string _majorProcedureStepSave = "Major_Procedure_Step_Save";

        #region Proc Step

        public List<MajorProcedureStepModel> Get(string procedureIds, out int total, BaseSearchModel search)
        {
            search = search ?? new BaseSearchModel
            {
                Search = null,
                Order = "1",
                OrderDir = "ASC",
                StartIndex = 0,
                PageSize = -1
            };

            var lstSteps = AppProcessor.ProcedureProvider.ExecuteTypedList<MajorProcedureStepModel>(
                _majorProcedureStepGet,
                DATA_PROVIDER_NAME, procedureIds,
                search.Search,
                search.Order,
                search.OrderDir,
                search.StartIndex,
                search.PageSize);

            total = 0;
            if (lstSteps != null && lstSteps.Count > 0)
                total = int.Parse(lstSteps.First()?.TotalRow.ToString() ?? "0");
            return lstSteps;
        }

        private MajorProcedureStepModel LoadDetail(Guid? stepId)
        {
            var stepInfo =
                AppProcessor.ProcedureProvider.ExecuteScalarObject<MajorProcedureStepModel>(_majorProcedureStepGetById,
                    DATA_PROVIDER_NAME, stepId);

            return stepInfo;
        }

        public bool Delete(MajorProcedureStepModel model)
        {
            var result =
                AppProcessor.ProcedureProvider.Execute(_majorProcedureStepDelete, DATA_PROVIDER_NAME, model.StepId,
                    model.Reason, model.UpdatedBy);
            return result == 1;
        }

        public List<MajorProcedureStepModel> GetAll(string procedureIds = null)
        {
            var lstSteps = Get(procedureIds, out _, null);
            return lstSteps;
        }

        public MajorProcedureStepModel GetById(Guid? stepId)
        {
            var stepInfo = LoadDetail(stepId);
            return stepInfo;
        }

        public int? Save(MajorProcedureStepModel model)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_majorProcedureStepSave, DATA_PROVIDER_NAME,
                model.StepId,
                model.ProcedureId,
                model.StepName,
                model.StepDesc,
                model.StepType,
                model.NextStep,
                model.PrevStep,
                model.ContractStatus,
                model.ContractStatusName,
                model.CusNotificationConfigs,
                model.StaffNotificationConfigs,
                model.AttachResultFile,
                model.TableHandlingTimes,
                model.TableHandlers,
                model.TableSituations,
                model.Reason,
                model.UpdatedBy);

            return result;
        }

        public MajorProcedureStepModel GetByKey(Guid? procedureId, string stepName)
        {
            var stepInfo =
                AppProcessor.ProcedureProvider.ExecuteScalarObject<MajorProcedureStepModel>(_majorProcedureStepGetByKey,
                    DATA_PROVIDER_NAME, procedureId, stepName);

            return stepInfo;
        }

        #endregion

        #region Step Handler

        private readonly string _majorProcedureStepHandlerGet = "Major_Procedure_Step_Handler_Get";
        private readonly string _majorProcedureStepHandlerGetById = "Major_Procedure_Step_Handler_GetById";
        private readonly string _majorProcedureStepHandlerSave = "Major_Procedure_Step_Handler_Save";

        public List<MajorProcedureStepHandlerModel> GetHandlers(Guid? stepId)
        {
            var lstHandlers = AppProcessor.ProcedureProvider.ExecuteTypedList<MajorProcedureStepHandlerModel>(
                _majorProcedureStepHandlerGet,
                DATA_PROVIDER_NAME, stepId);
            return lstHandlers;
        }

        public MajorProcedureStepHandlerModel GetHandlerById(Guid? stepId, Guid? unionId)
        {
            var handlerInfo =
                AppProcessor.ProcedureProvider.ExecuteScalarObject<MajorProcedureStepHandlerModel>(
                    _majorProcedureStepHandlerGetById,
                    DATA_PROVIDER_NAME, stepId, unionId);

            return handlerInfo;
        }

        public int? SaveHandler(MajorProcedureStepHandlerModel model)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_majorProcedureStepHandlerSave, DATA_PROVIDER_NAME,
                model.StepId,
                model.UnionId,
                model.DeptId,
                model.PositionID,
                model.StaffId,
                model.UpdatedBy);

            return result;
        }

        #endregion

        #region Step HandlingTime

        private readonly string _majorProcedureStepHandlingTimeGet = "Major_Procedure_Step_HandlingTime_Get";
        private readonly string _majorProcedureStepHandlingTimeGetById = "Major_Procedure_Step_HandlingTime_GetById";
        private readonly string _majorProcedureStepHandlingTimeSave = "Major_Procedure_Step_HandlingTime_Save";

        public List<MajorProcedureStepHandlingTimeModel> GetHandlingTimes(Guid? stepId)
        {
            var lstHanlers = AppProcessor.ProcedureProvider.ExecuteTypedList<MajorProcedureStepHandlingTimeModel>(
                _majorProcedureStepHandlingTimeGet,
                DATA_PROVIDER_NAME, stepId);
            return lstHanlers;
        }

        public MajorProcedureStepHandlingTimeModel GetHandlingTimeById(Guid? handlingTimeById)
        {
            var handlerInfo =
                AppProcessor.ProcedureProvider.ExecuteScalarObject<MajorProcedureStepHandlingTimeModel>(
                    _majorProcedureStepHandlingTimeGetById,
                    DATA_PROVIDER_NAME, handlingTimeById);

            return handlerInfo;
        }

        public int? SaveHandlingTime(MajorProcedureStepHandlingTimeModel model)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_majorProcedureStepHandlingTimeSave, DATA_PROVIDER_NAME,
                model.HandlingTimeId,
                model.StepId,
                model.HandlingTime,
                model.PurposeIds,
                model.PurposeNames,
                model.UpdatedBy);

            return result;
        }

        #endregion

        #region Step Situation

        private readonly string _majorProcedureStepSituationGet = "Major_Procedure_Step_Situation_Get";
        private readonly string _majorProcedureStepSituationGetById = "Major_Procedure_Step_Situation_GetById";
        private readonly string _majorProcedureStepSituationSave = "Major_Procedure_Step_Situation_Save";

        public List<MajorProcedureStepSituationModel> GetSituations(Guid? stepId)
        {
            var lstSituations = AppProcessor.ProcedureProvider.ExecuteTypedList<MajorProcedureStepSituationModel>(
                _majorProcedureStepSituationGet,
                DATA_PROVIDER_NAME, stepId);
            return lstSituations;
        }

        public MajorProcedureStepSituationModel GetSituationById(Guid? stepId, Guid? unionId)
        {
            var situationInfo =
                AppProcessor.ProcedureProvider.ExecuteScalarObject<MajorProcedureStepSituationModel>(
                    _majorProcedureStepSituationGetById,
                    DATA_PROVIDER_NAME, stepId, unionId);

            return situationInfo;
        }

        public int? SaveSituation(MajorProcedureStepSituationModel model)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_majorProcedureStepSituationSave, DATA_PROVIDER_NAME,
                model.SituationId,
                model.StepId,
                model.StepName,
                model.SituationName,
                model.NextStep,
                model.NextStepName,
                model.UpdatedBy);

            return result;
        }

        #endregion
    }
}