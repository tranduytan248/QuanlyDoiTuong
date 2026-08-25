using System.Collections.Generic;
using Cores.Sys.Models.Sys;
using TSFramework.App.Processors;

namespace Cores.Sys.Biz.Sys
{
    public class SysElnvAccountBiz
    {
        private const string DATA_PROVIDER_NAME = "MCSProvider";
        private readonly string _sysElnvAccountGet = "Sys_ElnvAccount_Get";

        private readonly string _sysElnvAccountGetById = "Sys_ElnvAccount_GetById";
        private readonly string _sysElnvAccountGetByUserName = "Sys_ElnvAccount_GetByUserName";
        private readonly string _sysElnvAccountSave = "Sys_ElnvAccount_Save";

        public List<SysElnvAccountModel> GetAll()
        {
            var dataElnvAcounts =
                AppProcessor.ProcedureProvider.ExecuteTypedList<SysElnvAccountModel>(_sysElnvAccountGet,
                    DATA_PROVIDER_NAME);
            return dataElnvAcounts;
        }

        public SysElnvAccountModel LoadDetail(int userId)
        {
            var dataElnvAcount =
                AppProcessor.ProcedureProvider.ExecuteScalarObject<SysElnvAccountModel>(_sysElnvAccountGetById,
                    DATA_PROVIDER_NAME,
                    userId);
            return dataElnvAcount;
        }

        public SysElnvAccountModel GetByUserName(string userName)
        {
            var dataElnvAcount =
                AppProcessor.ProcedureProvider.ExecuteScalarObject<SysElnvAccountModel>(_sysElnvAccountGetByUserName,
                    DATA_PROVIDER_NAME,
                    userName);
            return dataElnvAcount;
        }

        public int Save(SysElnvAccountModel model, string savedBy)
        {
            var userId = AppProcessor.ProcedureProvider.Execute(_sysElnvAccountSave,
                DATA_PROVIDER_NAME,
                model.UserId,
                model.EmpAccount,
                model.ElnvAccount,
                model.ElnvACPassword,
                model.Reason,
                savedBy
            );

            return userId.GetValueOrDefault(0);
        }
    }
}