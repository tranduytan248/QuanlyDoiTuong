using Cores.Sys.Models.Sys;
using TSFramework.App.Processors;

namespace Cores.Sys.Biz.Sys
{
    public class SysBlockIPBiz
    {
        private const string DATA_PROVIDER_NAME = "MCSProvider";
        private readonly string _sysBlockIPAdd = "Sys_BlockIP_Add";
        private readonly string _sysBlockIPGetByIP = "Sys_BlockIP_GetByIP";
        private readonly string _sysBlockIPRequest = "Sys_BlockIP_Request";
        private readonly string _sysBlockIPUnlock = "Sys_BlockIP_Unlock";

        public SysBlockIPModel GetByIP(string sIp)
        {
            var modelIP =
                AppProcessor.ProcedureProvider.ExecuteScalarObject<SysBlockIPModel>(_sysBlockIPGetByIP,
                    DATA_PROVIDER_NAME, sIp);
            return modelIP;
        }

        public int? Add(SysBlockIPModel model)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_sysBlockIPAdd, DATA_PROVIDER_NAME,
                model.IP,
                model.UrlRequest);

            return result;
        }

        public int? Request(SysBlockIPModel model)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_sysBlockIPRequest, DATA_PROVIDER_NAME,
                model.IP,
                model.UrlRequest);

            return result;
        }

        public int? Unlock(string userName, string sIp)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_sysBlockIPUnlock, DATA_PROVIDER_NAME,
                userName, sIp);

            return result;
        }
    }
}