using System;
using System.Collections.Generic;
using Cores.Major.Models;
using TSFramework.App.Processors;

namespace Cores.Major.Biz
{
    public class MajorContractPaymentBiz
    {
        private const string DATA_PROVIDER_NAME = "MCSProvider";
        private readonly string _majorContractPaymentCheckTypePayment = "Major_Contract_Payment_CheckTypePayment";

        private readonly string _majorContractPaymentDelete = "Major_Contract_Payment_Delete";
        private readonly string _majorContractPaymentGetById = "Major_Contract_Payment_GetById";

        private readonly string _majorContractPaymentGetViaContract = "Major_Contract_Payment_GetViaContract";
        private readonly string _majorContractPaymentSave = "Major_Contract_Payment_Save";

        public List<MajorContractPaymentModel> GetPayments(Guid? contractId)
        {
            var lstPayments = AppProcessor.ProcedureProvider.ExecuteTypedList<MajorContractPaymentModel>(
                _majorContractPaymentGetViaContract,
                DATA_PROVIDER_NAME, contractId);
            return lstPayments;
        }


        #region Payments

        public MajorContractPaymentModel GetPaymentById(Guid? paymentId)
        {
            var paymentModel =
                AppProcessor.ProcedureProvider.ExecuteScalarObject<MajorContractPaymentModel>(
                    _majorContractPaymentGetById,
                    DATA_PROVIDER_NAME, paymentId);

            return paymentModel;
        }

        public MajorContractPaymentModel CheckTypePayment(Guid? contractId, int typePayment)
        {
            var paymentModel =
                AppProcessor.ProcedureProvider.ExecuteScalarObject<MajorContractPaymentModel>(
                    _majorContractPaymentCheckTypePayment,
                    DATA_PROVIDER_NAME, contractId, typePayment);

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
                model.Note,
                model.Reason,
                model.UpdatedBy);

            return result;
        }

        #endregion
    }
}