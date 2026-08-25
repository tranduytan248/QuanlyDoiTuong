using System;
using Core.Inv.Models;
using TSFramework.App.Processors;

namespace Core.Inv.Biz
{
    public class MajorInvCusBiz
    {
        private const string DATA_PROVIDER_NAME = "MCSProvider";

        private readonly string _majorInvCusGetById = "Major_Inv_Cus_GetById";
        private readonly string _majorInvGetCusViaKey = "Major_Inv_GetCusViaKey";

        public MajorInvCusModel GetById(Guid? invId)
        {
            var invCusModel =
                AppProcessor.ProcedureProvider.ExecuteScalarObject<MajorInvCusModel>(_majorInvCusGetById,
                    DATA_PROVIDER_NAME, invId);

            return invCusModel;
        }

        public MajorInvCusModel GetByInvKey(string invKey)
        {
            var invCusModel =
                AppProcessor.ProcedureProvider.ExecuteScalarObject<MajorInvCusModel>(_majorInvGetCusViaKey,
                    DATA_PROVIDER_NAME, invKey);

            return invCusModel;
        }
    }
}