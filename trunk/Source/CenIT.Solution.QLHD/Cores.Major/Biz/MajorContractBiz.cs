using System;
using System.Collections.Generic;
using System.Linq;
using Cores.Major.Models;
using TSFramework.App.Models;
using TSFramework.App.Processors;

namespace Cores.Major.Biz
{
    public class MajorContractBiz
    {
        private const string DATA_PROVIDER_NAME = "MCSProvider";
        private readonly string _majorContractAcceptant = "Major_Contract_Acceptant";
        private readonly string _majorContractApprove = "Major_Contract_Approve";

        private readonly string _majorContractCheckDuplicateMapAndLandParcelNo =
            "Major_Contract_CheckDuplicateMapAndLandParcelNo";

        private readonly string _majorContractCheckLandMap = "Major_Contract_CheckLandMap";
        private readonly string _majorContractCusGetViaContract = "Major_Contract_Cus_GetViaContract";
        private readonly string _majorContractDashboard = "Major_Contract_Dashboard";
        private readonly string _majorContractDelete = "Major_Contract_Delete";
        private readonly string _majorContractGet = "Major_Contract_Get";
        private readonly string _majorContractGetById = "Major_Contract_GetById";

        private readonly string _majorContractPaymentDelete = "Major_Contract_Payment_Delete";
        private readonly string _majorContractPaymentGetById = "Major_Contract_Payment_GetById";
        private readonly string _majorContractPaymentGetViaContract = "Major_Contract_Payment_GetViaContract";
        private readonly string _majorContractPaymentSave = "Major_Contract_Payment_Save";
        private readonly string _majorContractReject = "Major_Contract_Reject";
        private readonly string _majorContractSave = "Major_Contract_Save";
        private readonly string _majorContractSaveRefFiles = "Major_Contract_SaveRefFiles";

        private readonly string _majorContractTaskGetViaContract = "Major_Contract_Task_GetViaContract";
        private readonly string _majorContractUpdateInfo = "Major_Contract_UpdateInfo";
        private readonly string _majorContractValid = "Major_Contract_Valid";

        public List<MajorContractPaymentModel> GetPayments(Guid? contractId)
        {
            var lstPayments = AppProcessor.ProcedureProvider.ExecuteTypedList<MajorContractPaymentModel>(
                _majorContractPaymentGetViaContract,
                DATA_PROVIDER_NAME, contractId);
            return lstPayments;
        }

        public List<MajorContractTaskModel> GetTasks(Guid? contractId)
        {
            var lstContracts = AppProcessor.ProcedureProvider.ExecuteTypedList<MajorContractTaskModel>(
                _majorContractTaskGetViaContract,
                DATA_PROVIDER_NAME, contractId);
            return lstContracts;
        }

        public MajorContractCustomerModel GetCus(Guid? contractId)
        {
            var customer = AppProcessor.ProcedureProvider.ExecuteScalarObject<MajorContractCustomerModel>(
                _majorContractCusGetViaContract,
                DATA_PROVIDER_NAME, contractId);
            return customer;
        }

        #region Contract Actions

        public List<MajorContractModel> Get(out int total, string managerUnions, string searchValue,
            DateTime? receivedFromDate, DateTime? receivedToDate, DateTime? giveResultFromDate,
            DateTime? giveResultToDate, string contractStatus, string typeContractIds, string typeCusIds,
            string username, BaseSearchModel search)
        {
            search = search ?? new BaseSearchModel
            {
                Search = null,
                Order = "1",
                OrderDir = "ASC",
                StartIndex = 0,
                PageSize = -1
            };

            var lstContracts = AppProcessor.ProcedureProvider.ExecuteTypedList<MajorContractModel>(_majorContractGet,
                DATA_PROVIDER_NAME, managerUnions, searchValue, receivedFromDate, receivedToDate, giveResultFromDate,
                giveResultToDate, contractStatus, typeContractIds, typeCusIds, username,
                search.Search,
                search.Order,
                search.OrderDir,
                search.StartIndex,
                search.PageSize);

            total = 0;
            if (lstContracts != null && lstContracts.Count > 0)
                total = int.Parse(lstContracts.First()?.TotalRow.ToString() ?? "0");
            return lstContracts;
        }

        public MajorContractModel GetById(Guid? contractId)
        {
            var contractModel =
                AppProcessor.ProcedureProvider.ExecuteScalarObject<MajorContractModel>(_majorContractGetById,
                    DATA_PROVIDER_NAME, contractId);

            return contractModel;
        }

        public int? Delete(MajorContractModel model)
        {
            var result =
                AppProcessor.ProcedureProvider.Execute(_majorContractDelete, DATA_PROVIDER_NAME, model.ContractId,
                    model.Reason, model.UpdatedBy);
            return result;
        }

        public List<MajorContractModel> GetAll(string managerUnions = null, string searchValue = null,
            DateTime? receivedFromDate = null, DateTime? receivedToDate = null, DateTime? giveResultFromDate = null,
            DateTime? giveResultToDate = null, string contractStatus = null, string typeContractIds = null,
            string typeCusIds = null, string username = null)
        {
            var lstContracts = Get(out _, managerUnions, searchValue, receivedFromDate, receivedToDate,
                giveResultFromDate, giveResultToDate, contractStatus, typeContractIds, typeCusIds, username, null);
            return lstContracts;
        }

        public int? Save(MajorContractModel model)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_majorContractSave, DATA_PROVIDER_NAME,
                model.ContractId,
                model.UnionId,

                //model.ContractNo,
                model.ContractSignal,
                model.ContractTypeId,
                model.ContractTypeName,
                model.PurposeId,
                model.PurposeName,
                model.LandParcelNo,
                model.MapNo,
                model.Status,
                model.StatusName,
                model.SubTotal,
                model.Discount,
                model.TaxRate,
                model.TaxAmount,
                model.Total,
                model.TotalInWords,
                model.PaymentMethod,
                model.PaymentMethodName,
                model.PercentAdvance,
                model.AdvanceAmount,
                model.PeriodAdvance,
                model.InfoDiscountContract,
                model.FuncDiscountContract,
                model.JsonExtendContracts,
                model.ExtendInfos,
                model.HandlingTime,
                model.ProvinceId,
                model.ProvinceName,
                model.WardId,
                model.WardName,
                model.Address,
                model.DataTasks,
                model.DataCus,
                //model.DataDossier,
                model.Reason,
                model.UpdatedBy);

            return result;
        }

        public int SaveRefFiles(MajorContractModel model)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_majorContractSaveRefFiles, DATA_PROVIDER_NAME,
                model.ContractId,
                model.TableRefFiles,
                model.UpdatedBy);

            return result.GetValueOrDefault(0);
        }

        public List<MajorContractModel> CheckLandMap(string mapNo, string landParcelNo, int? provinceId, int? wardId)
        {
            var lstContracts = AppProcessor.ProcedureProvider.ExecuteTypedList<MajorContractModel>(
                _majorContractCheckLandMap,
                DATA_PROVIDER_NAME, mapNo, landParcelNo, provinceId, wardId);

            return lstContracts;
        }

        public int? Valid(MajorContractModel model)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_majorContractValid, DATA_PROVIDER_NAME,
                model.ContractId,
                model.UsingUnionCode,
                model.Status,
                model.StatusName,
                //model.DataDossier,
                model.UpdatedBy);

            return result;
        }

        public int? Approve(MajorApproveDossierModel model)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_majorContractApprove, DATA_PROVIDER_NAME,
                model.DossierId,
                model.ApprovedOn,
                model.GiveResultOn,
                model.ContractStatus,
                model.ContractStatusName,
                model.DataDossier,
                model.Status,
                model.StatusName,
                model.NextStepId,
                model.NextStepName,
                model.UnionHandled,
                model.HandledBy,
                model.PositionId,
                model.HandlingTime,
                model.HandlingDossierTime,
                model.CurrentTaskStatus,
                model.CurrentTaskStatusName,
                model.TaskStatus,
                model.TaskStatusName,
                model.AllowSwitchHandler,
                model.UpdatedBy);

            return result;
        }

        public int UpdateInfo(Guid? contractId, string jsonContractInfo, string fileId, string updatedBy)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_majorContractUpdateInfo, DATA_PROVIDER_NAME,
                contractId,
                jsonContractInfo,
                fileId,
                updatedBy);

            return result.GetValueOrDefault(0);
        }

        #region Reject - từ chối hợp đồng

        public int? Reject(MajorContractRejectModel rejectModel)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_majorContractReject, DATA_PROVIDER_NAME,
                rejectModel.ContractId,
                rejectModel.RejectOn,
                rejectModel.ContractStatus,
                rejectModel.ContractStatusName,
                null,
                rejectModel.UpdatedBy);

            return result;
        }

        #endregion

        public int? Acceptant(MajorContractModel model)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_majorContractAcceptant, DATA_PROVIDER_NAME,
                model.ContractId,
                model.Status,
                model.StatusName,
                model.UpdatedBy);

            return result;
        }

        public List<MajorContractModel> Dashboard(out int total, string username, string managerUnions = null,
            string searchValue = null, DateTime? receivedFromDate = null, DateTime? receivedToDate = null,
            DateTime? giveResultFromDate = null, DateTime? giveResultToDate = null, string contractStatus = null,
            string typeContractIds = null, string typeCusIds = null, string typeTermIds = null,
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

            var lstContracts = AppProcessor.ProcedureProvider.ExecuteTypedList<MajorContractModel>(
                _majorContractDashboard, DATA_PROVIDER_NAME, managerUnions, searchValue, receivedFromDate,
                receivedToDate, giveResultFromDate, giveResultToDate, contractStatus, typeContractIds, typeCusIds,
                typeTermIds, username,
                search.Search,
                search.Order,
                search.OrderDir,
                search.StartIndex,
                search.PageSize);

            total = 0;
            if (lstContracts != null && lstContracts.Count > 0)
                total = int.Parse(lstContracts.First()?.TotalRow.ToString() ?? "0");
            return lstContracts;
        }

        #endregion

        #region Payments

        public MajorContractPaymentModel GetPaymentById(Guid? paymentId)
        {
            var paymentModel =
                AppProcessor.ProcedureProvider.ExecuteScalarObject<MajorContractPaymentModel>(
                    _majorContractPaymentGetById,
                    DATA_PROVIDER_NAME, paymentId);

            return paymentModel;
        }

        public int? DeletePayment(MajorContractPaymentModel model)
        {
            var result =
                AppProcessor.ProcedureProvider.Execute(_majorContractPaymentDelete, DATA_PROVIDER_NAME, model.PaymentId,
                    model.Reason, model.UpdatedBy);
            return result;
        }

        public int? SavePayment(MajorContractPaymentModel model)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_majorContractPaymentSave, DATA_PROVIDER_NAME,
                model.PaymentId,
                model.ContractId,
                model.PaidAmount,
                //model.RefDocNo,
                model.PaymentInfo,
                model.TypePayment,
                model.TypePaymentName,
                model.PercentAdvance,
                model.PaidOn,
                model.PaymentMethod,
                model.PaymentMethodName,
                model.Status,
                model.StatusName,
                model.TypeArising,
                model.TypeArisingName,
                model.ArisingAmount,
                model.DiscountRate,
                model.DiscountAmount,
                model.Note,
                model.Reason,
                model.UpdatedBy);

            return result;
        }

        public int? CheckCheckDuplicateMapAndLandParcelNo(string landParcelNo, string mapNo)
        {
            var result =
                AppProcessor.ProcedureProvider.Execute(_majorContractCheckDuplicateMapAndLandParcelNo,
                    DATA_PROVIDER_NAME, landParcelNo, mapNo);
            return result;
        }

        #endregion
    }
}