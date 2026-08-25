using Cores.Sys.Models.Sys;
using TSFramework.App.Processors;

namespace Cores.Sys.Biz.Sys
{
    public class SysIpRequestBiz
    {
        private const string DATA_PROVIDER_NAME = "MCSProvider";
        private readonly string _sysIpRequestGetByIp = "Sys_IPRequest_GetByIp";

        public SysIPRequestModel GetByIP(string sIp)
        {
            var modelIP =
                AppProcessor.ProcedureProvider.ExecuteScalarObject<SysIPRequestModel>(_sysIpRequestGetByIp,
                    DATA_PROVIDER_NAME, sIp);
            return modelIP;
        }
    }
}