using System;
using System.Collections.Generic;
using System.Linq;
using Cores.Major.Models;
using TSFramework.App.Models;
using TSFramework.App.Processors;

namespace Cores.Major.Biz
{
    public class FEContractBiz
    {
        private const string DATA_PROVIDER_NAME = "MCSProvider";
        private readonly string _majorContractGetByContractNo = "Major_Contract_GetByContractNo";
        private readonly string _majorContractGetFilterContract = "Major_Contract_GetFilterContract";
        private readonly string _majorContractGetForQR = "Major_Contract_GetForQR";
        private readonly string _majorContractRenderFormGetByContractId = "Major_ContractRenderForm_GetByContractId";
        private readonly string _majorPaymentGetByContractId = "Major_Payment_GetByContractId";
        private readonly string _majorTaskGetByContractId = "Major_Task_GetByContractId";

        public List<FEContractModel> Get(out int total, string contractNo, string cusName, string identifierNo,
            string mapNo, string landParcelNo, int? streetId, int? provinceId, int? wardId, BaseSearchModel search)
        {
            search = search ?? new BaseSearchModel
            {
                Search = null,
                Order = "1",
                OrderDir = "ASC",
                StartIndex = 0,
                PageSize = -1
            };

            var listContracts = AppProcessor.ProcedureProvider.ExecuteTypedList<FEContractModel>(
                _majorContractGetFilterContract,
                DATA_PROVIDER_NAME,
                contractNo,
                cusName,
                identifierNo,
                mapNo,
                landParcelNo,
                streetId,
                provinceId,
                wardId,
                search.Search,
                search.Order,
                search.OrderDir,
                search.StartIndex,
                search.PageSize);

            total = 0;
            if (listContracts != null && listContracts.Count > 0)
                total = int.Parse(listContracts.First()?.TotalRow.ToString() ?? "0");
            return listContracts;
        }

        public List<FEContractModel> GetAll()
        {
            var listContracts = Get(out _, null, null, null, null, null, null, null, null, null);
            return listContracts;
        }

        public List<FETaskModel> TasksGetByContractId(Guid? id)
        {
            var listTask = AppProcessor.ProcedureProvider.ExecuteTypedList<FETaskModel>(_majorTaskGetByContractId,
                DATA_PROVIDER_NAME, id);
            return listTask;
        }

        // get detail contrcat by contractno, cusname,indentiferno
        public FEContractModel DetailContract(string contractNo, string phone)
        {
            var contract = AppProcessor.ProcedureProvider.ExecuteScalarObject<FEContractModel>(
                _majorContractGetByContractNo,
                DATA_PROVIDER_NAME, contractNo, phone);
            return contract;
        }

        // get detail contrcat by contractno, cusname,indentiferno
        public FEContractModel DetailByQRCode(Guid? contractId)
        {
            var contract = AppProcessor.ProcedureProvider.ExecuteScalarObject<FEContractModel>(_majorContractGetForQR,
                DATA_PROVIDER_NAME, contractId);
            return contract;
        }

        public List<FEPaymentModel> PaymentGetByContractId(Guid? id)
        {
            var listPayment = AppProcessor.ProcedureProvider.ExecuteTypedList<FEPaymentModel>(
                _majorPaymentGetByContractId,
                DATA_PROVIDER_NAME, id);
            return listPayment;
        }

        public FERenderContractModel GetDataRenderContract(Guid? id)
        {
            var renderContract = AppProcessor.ProcedureProvider.ExecuteScalarObject<FERenderContractModel>(
                _majorContractRenderFormGetByContractId,
                DATA_PROVIDER_NAME, id);
            return renderContract;
        }
    }
}