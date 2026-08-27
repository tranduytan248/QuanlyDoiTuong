using System.Collections.Generic;
using Cores.Sys.Models.Sys;
using TSFramework.App.Processors;

namespace Cores.Sys.Biz.Sys
{
    /// <summary>
    /// Truy xuat phien lam viec dang hoat dong phuc vu man hinh Giam sat truc tuyen.
    /// </summary>
    public class SysUserActivityBiz
    {
        private const string DATA_PROVIDER_NAME = "MCSProvider";
        private readonly string _track = "Sys_UserActivity_Track";
        private readonly string _end = "Sys_UserActivity_End";
        private readonly string _get = "Sys_UserActivity_Get";

        /// <summary>
        /// Ghi nhan hoat dong cua mot phien. Goi o moi request cua nguoi da dang nhap.
        /// </summary>
        public void Track(string sessionId, string userName, string currentUrl,
            string screenName, string ipAddress, string userAgent)
        {
            AppProcessor.ProcedureProvider.ExecuteScalar(_track, DATA_PROVIDER_NAME,
                sessionId, userName, currentUrl, screenName, ipAddress, userAgent);
        }

        /// <summary>Xoa phien khi nguoi dung dang xuat.</summary>
        public void End(string sessionId)
        {
            AppProcessor.ProcedureProvider.ExecuteScalar(_end, DATA_PROVIDER_NAME, sessionId);
        }

        /// <summary>
        /// Danh sach phien con hoat dong trong <paramref name="timeoutMinutes"/> phut gan nhat.
        /// </summary>
        public List<SysUserActivityModel> Get(int timeoutMinutes)
        {
            return AppProcessor.ProcedureProvider
                .ExecuteTypedList<SysUserActivityModel>(_get, DATA_PROVIDER_NAME, timeoutMinutes);
        }
    }
}
