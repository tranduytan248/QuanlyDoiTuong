using System;
using System.Collections.Generic;
using System.Linq;
using Core.Inv.Models;
using Core.Inv.Models.Invs;
using TSFramework.App.Models;
using TSFramework.App.Processors;

namespace Core.Inv.Biz
{
    public class MajorInvBiz
    {
        private const string DATA_PROVIDER_NAME = "MCSProvider";
        private readonly string _majorInvAdjust = "Major_Inv_Adjust";
        private readonly string _majorInvCancel = "Major_Inv_Cancel";
        private readonly string _majorInvGet = "Major_Inv_Get";
        private readonly string _majorInvGetByContractId = "Major_Inv_GetByContractId";
        private readonly string _majorInvGetById = "Major_Inv_GetById";
        private readonly string _majorInvGetByKey = "Major_Inv_GetByKey";

        private readonly string _majorInvGetView = "Major_Inv_GetView";

        private readonly string _majorInvPendingGet = "Major_Inv_Pending_Get";

        private readonly string _majorInvRollback = "Major_Inv_Rollback";
        private readonly string _majorInvSave = "Major_Inv_Save";
        private readonly string _majorInvSaveView = "Major_Inv_SaveView";
        private readonly string _majorInvSync = "Major_Inv_Sync";

        private readonly string _majorInvUpdate = "Major_Inv_Update";

        public List<MajorInvModel> Get(out int total, string userName, string managerUnions, string invNo = null,
            string pattern = null, string serials = null, string invStatus = null, string invTypes = null,
            DateTime? createdFrom = null, DateTime? createdTo = null, string creators = null, string cusName = null,
            string cusCode = null, string cusTaxCode = null, BaseSearchModel search = null)
        {
            search = search ?? new BaseSearchModel
            {
                Search = null,
                Order = "0",
                OrderDir = "ASC",
                StartIndex = 0,
                PageSize = -1
            };

            var lstInvoices = AppProcessor.ProcedureProvider.ExecuteTypedList<MajorInvModel>(_majorInvGet,
                DATA_PROVIDER_NAME, userName, managerUnions, invNo, pattern, serials, invStatus, invTypes,
                createdFrom, createdTo, creators, cusName, cusCode, cusTaxCode,
                search.Search,
                search.Order,
                search.OrderDir,
                search.StartIndex,
                search.PageSize);

            total = 0;
            if (lstInvoices != null && lstInvoices.Count > 0)
                total = int.Parse(lstInvoices.First()?.TotalRow.ToString() ?? "0");
            return lstInvoices;
        }

        public List<MajorInvModel> GetPendingInvs(out int total, string userName, string managerUnions,
            string pattern = null, string serials = null, string invTypes = null, DateTime? createdFrom = null,
            DateTime? createdTo = null, string creators = null, string cusName = null, string cusCode = null,
            string cusTaxCode = null, BaseSearchModel search = null)
        {
            search = search ?? new BaseSearchModel
            {
                Search = null,
                Order = "0",
                OrderDir = "ASC",
                StartIndex = 0,
                PageSize = -1
            };

            var lstPendingInvs = AppProcessor.ProcedureProvider.ExecuteTypedList<MajorInvModel>(_majorInvPendingGet,
                DATA_PROVIDER_NAME, userName, managerUnions, pattern, serials, invTypes, createdFrom, createdTo,
                creators,
                cusName, cusCode, cusTaxCode,
                search.Search,
                search.Order,
                search.OrderDir,
                search.StartIndex,
                search.PageSize);

            total = 0;
            if (lstPendingInvs != null && lstPendingInvs.Count > 0)
                total = int.Parse(lstPendingInvs.First()?.TotalRow.ToString() ?? "0");
            return lstPendingInvs;
        }

        public int? Update(InvStatusModel model)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_majorInvUpdate, DATA_PROVIDER_NAME,
                model.InvKey,
                model.InvNo,
                model.InvStatus,
                model.InvStatusName,
                model.PublishBy,
                model.PublishOn,
                model.ConfirmPaidBy,
                model.PaidOn,
                model.ErrCode,
                model.Reason,
                model.SavedBy
            );
            return result;
        }

        public int? Save(MajorInvModel model)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_majorInvSave, DATA_PROVIDER_NAME,
                model.ContractId,
                model.InvKey,
                model.Pattern,
                model.Serial,
                model.InvType,
                model.InvTypeName,
                model.InvStatus,
                model.InvStatusName,
                model.Note,
                model.TaxRate,
                model.TaxAmount,
                model.Amount,
                model.AmountInWord,
                model.CurrencyUnit,
                model.PaymentMethod,
                model.DataInvCus,
                model.DataInvProduct,
                model.PublishBy,
                model.PaidOn,
                model.ConfirmPaidBy,
                model.UpdatedBy
            );
            return result;
        }

        public int? Rollback(InvStatusModel model)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_majorInvRollback, DATA_PROVIDER_NAME,
                model.InvKey,
                model.Reason,
                model.SavedBy
            );
            return result;
        }

        public int? Sync(InvStatusModel model)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_majorInvSync, DATA_PROVIDER_NAME,
                model.InvKey,
                model.InvNo,
                model.InvStatus,
                model.InvStatusName,
                model.PublishOn,
                model.Reason,
                model.SavedBy
            );
            return result;
        }

        public MajorInvModel GetById(Guid? invId)
        {
            var invModel =
                AppProcessor.ProcedureProvider.ExecuteScalarObject<MajorInvModel>(_majorInvGetById,
                    DATA_PROVIDER_NAME, invId);

            return invModel;
        }

        public MajorInvModel GetByKey(string invKey)
        {
            var invModel =
                AppProcessor.ProcedureProvider.ExecuteScalarObject<MajorInvModel>(_majorInvGetByKey,
                    DATA_PROVIDER_NAME, invKey);

            return invModel;
        }

        public int? Cancel(MajorInvModel model)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_majorInvCancel, DATA_PROVIDER_NAME,
                model.InvKey,
                model.InvStatus,
                model.InvStatusName,
                model.Reason,
                model.UpdatedBy
            );
            return result;
        }

        public MarjorViewInvModel GetView(Guid? invId)
        {
            var invViewModel =
                AppProcessor.ProcedureProvider.ExecuteScalarObject<MarjorViewInvModel>(_majorInvGetView,
                    DATA_PROVIDER_NAME, invId);

            return invViewModel;
        }

        public int? SaveView(MarjorViewInvModel model)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_majorInvSaveView, DATA_PROVIDER_NAME,
                model.InvId,
                model.InvView
            );
            return result;
        }

        public MajorInvModel GetByContractId(Guid? contractId)
        {
            var invModel =
                AppProcessor.ProcedureProvider.ExecuteScalarObject<MajorInvModel>(_majorInvGetByContractId,
                    DATA_PROVIDER_NAME, contractId);

            return invModel;
        }

        public int? AdjustInvoice(MajorInvModel model, string adjustedFkey, int statusNew, string statusNameNew,
            string invNo)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_majorInvAdjust, DATA_PROVIDER_NAME,
                model.InvId,
                adjustedFkey,
                model.InvKey,
                model.InvStatus,
                model.InvStatusName,
                model.InvType,
                model.InvTypeName,
                statusNew,
                statusNameNew,
                model.DataInvCus,
                invNo,
                model.UpdatedBy
            );
            return result;
        }
    }
}