using System;
using Core.Inv.Models;
using TSFramework.App.Processors;

namespace Core.Inv.Biz
{
    public class MajorInvProductBiz
    {
        private const string DATA_PROVIDER_NAME = "MCSProvider";
        private readonly string _majorInvGetProductsViaKey = "Major_Inv_GetProductsViaKey";

        private readonly string _majorInvProductsGetById = "Major_Inv_Products_GetById";

        public MajorInvProductModel GetById(Guid? invId)
        {
            var invProductModel =
                AppProcessor.ProcedureProvider.ExecuteScalarObject<MajorInvProductModel>(_majorInvProductsGetById,
                    DATA_PROVIDER_NAME, invId);

            return invProductModel;
        }

        public MajorInvProductModel GetProductsViaKey(string invKey)
        {
            var invProductModel =
                AppProcessor.ProcedureProvider.ExecuteScalarObject<MajorInvProductModel>(_majorInvGetProductsViaKey,
                    DATA_PROVIDER_NAME, invKey);

            return invProductModel;
        }
    }
}