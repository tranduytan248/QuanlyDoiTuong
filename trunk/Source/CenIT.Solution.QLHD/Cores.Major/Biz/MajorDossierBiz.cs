using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Cores.Major.Models;
using TSFramework.App.Models;
using TSFramework.App.Processors;

namespace Cores.Major.Biz
{
    public class MajorDossierBiz
    {
        private const string DATA_PROVIDER_NAME = "MCSProvider";
        private readonly string _majorDossierApprove = "Major_Dossier_Approve";
        private readonly string _majorDossierChangeHandler = "Major_Dossier_ChangeHandler";
        private readonly string _majorDossierComplete = "Major_Dossier_Complete";
        private readonly string _majorDossierDelete = "Major_Dossier_Delete";
        private readonly string _majorDossierGet = "Major_Dossier_Get";
        private readonly string _majorDossierGetById = "Major_Dossier_GetById";
        private readonly string _majorDossierHandle = "Major_Dossier_Handle";
        private readonly string _majorDossierSave = "Major_Dossier_Save";
        private readonly string _majorDossierSaveRefFiles = "Major_Dossier_SaveRefFiles";
        private readonly string _majorDossierTaskContinue = "Major_Dossier_Task_Continue";
        private readonly string _majorDossierTaskGet = "Major_Dossier_Task_Get";


        private readonly string _majorDossierTaskGetById = "Major_Dossier_Task_GetById";
        private readonly string _majorDossierTaskPause = "Major_Dossier_Task_Pause";
        private readonly string _majorDossierTaskSaveRefFiles = "Major_Dossier_Task_SaveRefFiles";
        private readonly string _majorDossierTaskSwitchHandler = "Major_Dossier_Task_SwitchHandler";
        private readonly string _majorDossierUpdateProcConfig = "Major_Dossier_UpdateProcConfig";

        #region Dossiers

        public List<MajorDossierModel> Get(out int total, string userName, string managerUnions = null,
            string searchValue = null, DateTime? receivedFromDate = null, DateTime? receivedToDate = null,
            DateTime? giveResultFromDate = null, DateTime? giveResultToDate = null, string lstStatus = null,
            string handleTypes = null, string typeContractIds = null, string typeCusIds = null,
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

            var listDossiers = AppProcessor.ProcedureProvider.ExecuteTypedList<MajorDossierModel>(_majorDossierGet,
                DATA_PROVIDER_NAME, userName, managerUnions, searchValue, receivedFromDate, receivedToDate,
                giveResultFromDate, giveResultToDate, lstStatus, handleTypes, typeContractIds, typeCusIds,
                search.Search,
                search.Order,
                search.OrderDir,
                search.StartIndex,
                search.PageSize
            );

            total = 0;
            if (listDossiers != null && listDossiers.Count > 0)
                total = int.Parse(listDossiers.First()?.TotalRow.ToString() ?? "0");
            return listDossiers;
        }


        private MajorDossierModel LoadDetail(Guid? dossierId)
        {
            var dossierModel =
                AppProcessor.ProcedureProvider.ExecuteScalarObject<MajorDossierModel>(_majorDossierGetById,
                    DATA_PROVIDER_NAME, dossierId);

            return dossierModel;
        }

        public int? Delete(MajorDossierModel model)
        {
            var result =
                AppProcessor.ProcedureProvider.Execute(_majorDossierDelete, DATA_PROVIDER_NAME, model.DossierId,
                    model.Reason, model.UpdatedBy);
            return result;
        }

        public List<MajorDossierModel> GetAll(string userName, string managerUnions = null, string searchValue = null,
            DateTime? receivedFromDate = null, DateTime? receivedToDate = null, DateTime? giveResultFromDate = null,
            DateTime? giveResultToDate = null, string lstStatus = null, string handleTypes = null,
            string typeContractIds = null, string typeCusIds = null)
        {
            var listDossiers = Get(out _, userName, managerUnions, searchValue, receivedFromDate, receivedToDate,
                giveResultFromDate, giveResultToDate, lstStatus, handleTypes, typeContractIds, typeCusIds);
            return listDossiers;
        }

        public MajorDossierModel GetById(Guid? dossierId)
        {
            var procedure = LoadDetail(dossierId);
            return procedure;
        }

        public int? Save(MajorDossierModel model)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_majorDossierSave, DATA_PROVIDER_NAME,
                model.DossierId,
                model.DossierName,
                model.TotalHandlingTime,
                model.ProcedureId,
                model.ProcedureName,
                model.ProcConfigs,
                model.InStep,
                model.InStepName,
                model.UnionHandled,
                model.HandledBy,
                model.PositionId,
                model.HandlingTime,
                model.Status,
                model.StatusName,
                model.TaskStatus,
                model.TaskStatusName,
                model.UpdatedBy);

            return result;
        }

        public int? Approve(MajorApproveDossierModel model)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_majorDossierApprove, DATA_PROVIDER_NAME,
                model.DossierId,
                model.ApprovedOn,
                model.GiveResultOn,
                model.Status,
                model.StatusName,
                model.NextStepId,
                model.NextStepName,
                model.UnionHandled,
                model.HandledBy,
                model.PositionId,
                model.HandlingTime,
                model.CurrentTaskStatus,
                model.CurrentTaskStatusName,
                model.TaskStatus,
                model.TaskStatusName,
                model.AllowSwitchHandler,
                model.UpdatedBy);

            return result;
        }

        public int? UpdateProcConfig(MajorDossierModel model)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_majorDossierUpdateProcConfig, DATA_PROVIDER_NAME,
                model.DossierId,
                model.ProcConfigs);

            return result;
        }

        public int SaveRefFiles(MajorDossierModel model)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_majorDossierSaveRefFiles, DATA_PROVIDER_NAME,
                model.DossierId,
                model.TableRefFiles,
                model.UpdatedBy);

            return result.GetValueOrDefault(0);
        }

        #endregion


        #region Dossier Tasks

        public List<MajorDossierTaskModel> GetTasks(Guid? dossierId)
        {
            var dossierTasks =
                AppProcessor.ProcedureProvider.ExecuteTypedList<MajorDossierTaskModel>(_majorDossierTaskGet,
                    DATA_PROVIDER_NAME, dossierId);

            return dossierTasks;
        }

        public MajorDossierTaskModel GetTaskById(Guid? taskId)
        {
            var dossierTaskModel =
                AppProcessor.ProcedureProvider.ExecuteScalarObject<MajorDossierTaskModel>(_majorDossierTaskGetById,
                    DATA_PROVIDER_NAME, taskId);

            return dossierTaskModel;
        }

        public int? SwitchHandler(Guid? taskId, string handlingComments, DataTable dataHandlers, string saveBy)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_majorDossierTaskSwitchHandler, DATA_PROVIDER_NAME,
                taskId,
                handlingComments,
                dataHandlers,
                saveBy);

            return result;
        }

        public int? Handle(MajorDossierTaskModel model)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_majorDossierHandle, DATA_PROVIDER_NAME,
                model.TaskId,
                model.UnionHandle,
                model.HandledBy,
                model.PositionId,
                model.Status,
                model.StatusName,
                model.UpdatedBy);

            return result;
        }

        public int? Complete(MajorDossierTaskModel model)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_majorDossierComplete, DATA_PROVIDER_NAME,
                model.TaskId,
                model.NextStep,
                model.NextStepName,
                model.UnionHandle,
                model.HandledBy,
                model.PositionId,
                model.SelectedSituation,
                model.SelectedSituationName,
                model.Status,
                model.StatusName,
                model.HandlingResult,
                model.NextStatus,
                model.NextStatusName,
                model.AllowSwitchHandler,
                model.HandlingTime,
                model.IsFinish,
                model.IsRollbackPrev,
                model.UpdatedBy);

            return result;
        }

        public int? ChangeHandler(MajorDossierTaskModel model)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_majorDossierChangeHandler, DATA_PROVIDER_NAME,
                model.TaskId,
                model.HandledBy);

            return result;
        }

        public int? PauseTask(MajorDossierTaskModel model)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_majorDossierTaskPause, DATA_PROVIDER_NAME,
                model.TaskId,
                model.DossierId,
                model.Status,
                model.StatusName,
                model.ContractStatus,
                model.ContractStatusName,
                model.Reason,
                model.UpdatedBy);

            return result;
        }

        public int? ContinueTask(MajorDossierTaskModel model)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_majorDossierTaskContinue, DATA_PROVIDER_NAME,
                model.TaskId,
                model.DossierId,
                model.Status,
                model.StatusName,
                model.ContractStatus,
                model.ContractStatusName,
                model.UpdatedBy);

            return result;
        }

        public int SaveRefFiles(MajorDossierTaskModel model)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_majorDossierTaskSaveRefFiles, DATA_PROVIDER_NAME,
                model.TaskId,
                model.TableRefFiles,
                model.UpdatedBy);

            return result.GetValueOrDefault(0);
        }

        #endregion
    }
}