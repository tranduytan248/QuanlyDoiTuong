using System;
using System.Collections.Generic;
using System.Linq;
using Core.Inv.Models;
using TSFramework.App.Models;
using TSFramework.App.Processors;

namespace Core.Inv.Biz
{
    public class MajorInvConfigBiz
    {
        private const string DATA_PROVIDER_NAME = "MCSProvider";

        private readonly string _majorInvConfigDelete = "Major_Inv_Config_Delete";
        private readonly string _majorInvConfigGet = "Major_Inv_Config_Get";
        private readonly string _majorInvConfigGetById = "Major_Inv_Config_GetById";
        private readonly string _majorInvConfigGetByKey = "Major_Inv_Config_GetByKey";
        private readonly string _majorInvConfigSave = "Major_Inv_Config_Save";

        public List<MajorInvConfigModel> Get(out int total, BaseSearchModel search)
        {
            search = search ?? new BaseSearchModel
            {
                Search = null,
                Order = "0",
                OrderDir = "ASC",
                StartIndex = 0,
                PageSize = -1
            };

            var lstConfigs = AppProcessor.ProcedureProvider.ExecuteTypedList<MajorInvConfigModel>(_majorInvConfigGet,
                DATA_PROVIDER_NAME,
                search.Search,
                search.Order,
                search.OrderDir,
                search.StartIndex,
                search.PageSize);

            total = 0;
            if (lstConfigs != null && lstConfigs.Count > 0)
                total = int.Parse(lstConfigs.First()?.TotalRow.ToString() ?? "0");
            return lstConfigs;
        }

        public MajorInvConfigModel GetById(Guid? cateId)
        {
            var lstConfigs =
                AppProcessor.ProcedureProvider.ExecuteScalarObject<MajorInvConfigModel>(_majorInvConfigGetById,
                    DATA_PROVIDER_NAME, cateId);

            return lstConfigs;
        }

        public bool Delete(MajorInvConfigModel model)
        {
            var result =
                AppProcessor.ProcedureProvider.Execute(_majorInvConfigDelete, DATA_PROVIDER_NAME, model.ConfigId,
                    model.UpdatedBy);
            return result == 1;
        }

        public List<MajorInvConfigModel> GetAll()
        {
            var lstConfigs = Get(out _, null);
            return lstConfigs;
        }

        public int? Save(MajorInvConfigModel model)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_majorInvConfigSave, DATA_PROVIDER_NAME,
                model.ConfigId,
                model.ConfigKey,
                model.ConfigValue,
                model.ConfigDesc,
                model.UpdatedBy);
            return result;
        }

        public MajorInvConfigModel GetByKey(string configKey)
        {
            var categoryInfo =
                AppProcessor.ProcedureProvider.ExecuteScalarObject<MajorInvConfigModel>(_majorInvConfigGetByKey,
                    DATA_PROVIDER_NAME, configKey);

            return categoryInfo;
        }
    }
}