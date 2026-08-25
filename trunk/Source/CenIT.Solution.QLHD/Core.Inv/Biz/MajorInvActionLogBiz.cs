using System.Data;
using TSFramework.App.Processors;

namespace Core.Inv.Biz
{
    public class MajorInvActionLogBiz
    {
        private const string DATA_PROVIDER_NAME = "MCSProvider";

        private readonly string _majorInvActionLogSave = "Major_Inv_ActionLog_Save";

        public int? Save(DataTable dataActionLogs)
        {
            var result =
                AppProcessor.ProcedureProvider.Execute(_majorInvActionLogSave, DATA_PROVIDER_NAME, dataActionLogs);
            return result;
        }
    }
}