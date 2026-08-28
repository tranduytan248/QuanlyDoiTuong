using Cores.Sys.Caches.Sys;
using Cores.Sys.Models.Sys;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TSFramework.App.BaseApps;
using TSFramework.App.Processors;
using TSFramework.Core.Members.Job;
using TSFramework.Core.Providers;
using TSFramework.Core.Utils;

namespace CenIT.Solution.QLHD.WebApp
{
    public class WebApiApplication : BaseHttpApplication
    {
        private void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => new SignalRUserProvider());
            SqlServerTypes.Utilities.LoadNativeAssemblies(Server.MapPath("~/bin"));

            #region Register Job

            var jobLibrariesPathFolder = ConfigurationManager.AppSettings["JobFolderPath"] ?? "/Libraries/Jobs";

            var jobCache = new SysJobCache();
            var lstActiveJobs = jobCache.GetAll().Where(j => j.IsActive).ToList();

            lstActiveJobs.ForEach(j =>
            {
                var sFileName = j.JobLibrary;
                var jobLibrariesAbsolutePathFolder = Server.MapPath(jobLibrariesPathFolder);
                var jobLibrariesAbsoluteFilePath = string.Concat(jobLibrariesAbsolutePathFolder, "/", sFileName);

                if (j.FileLibrary != null)
                {
                    if (!Directory.Exists(jobLibrariesAbsolutePathFolder))
                        Directory.CreateDirectory(jobLibrariesAbsolutePathFolder);
                    j.FileLibrary.SaveAs(jobLibrariesAbsoluteFilePath);
                }

                if (File.Exists(jobLibrariesAbsoluteFilePath))
                {
                    var jobPlugable = JobPlugableProvider.GetJobPlugable(jobLibrariesAbsoluteFilePath);
                    JobSchedulerProvider.UpdateCronExpression(j.JobId.ToString(), j.CronExpression);

                    if (jobPlugable != null)
                        ExecuteJob(jobPlugable, j);
                }
            });

            #endregion
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (ConfigurationManager.AppSettings["MaintenanceMode"] == "true")
            {
                if (!Request.IsLocal)
                {
                    HttpContext.Current.RewritePath("AppOffline.htm");
                }
                return;
            }

            var userNameResetLock = Request.QueryString["ping"];
            var blockIPCache = new SysBlockIPCache();
            var blockIPModel = blockIPCache.GetByIp(Request.UserHostAddress);
            if (blockIPModel?.IsLock ?? false)
            {
                if (!string.IsNullOrEmpty(userNameResetLock))
                {
                    var retUnlockIp = blockIPCache.Unlock(userNameResetLock, Request.UserHostAddress);
                    if (retUnlockIp > 0)
                    {
                        Response.Clear();
                        Response.Redirect($"/?g={Guid.NewGuid()}", true);
                        return;
                    }
                }
                Response.Clear();
                Response.Status = "301 Moved Permanently";
                return;
            }

            var ipRequestCache = new SysIpRequestCache();
            var ipRequestModel = ipRequestCache.GetByIp(Request.UserHostAddress);
            if (ipRequestModel != null && ipRequestModel.IsLock)
            {
                if (!string.IsNullOrEmpty(userNameResetLock))
                {
                    var retUnlockIp = blockIPCache.Unlock(userNameResetLock, Request.UserHostAddress);
                    if (retUnlockIp > 0)
                    {
                        Response.RedirectPermanent("/", true);
                        return;
                    }
                }

                Response.Clear();
                Response.Status = "301 Moved Permanently";
                return;
            }
        }

        /// <summary>
        /// Ghi nhan phien lam viec cua nguoi da dang nhap, phuc vu man hinh
        /// Giam sat truc tuyen.
        ///
        /// Chi ghi voi request giao dien that: bo qua file tinh (.js, .css, anh)
        /// va cac loi goi ngam nhu ActionIsAllow - neu khong man hinh giam sat
        /// se hien nhung duong dan vo nghia thay vi man hinh nguoi dung dang xem.
        ///
        /// Moi loi o day deu nuot: ghi nhan hoat dong khong duoc phep lam hong
        /// request cua nguoi dung.
        /// </summary>
        private void TrackUserActivity()
        {
            try
            {
                var ctx = HttpContext.Current;
                if (ctx?.User?.Identity == null || !ctx.User.Identity.IsAuthenticated) return;
                if (ctx.Session == null) return;

                var path = Request.Path ?? string.Empty;
                if (string.IsNullOrEmpty(path)) return;

                // Bo qua file tinh
                var ext = Path.GetExtension(path);
                if (!string.IsNullOrEmpty(ext)) return;

                // Bo qua cac loi goi ngam khong phai man hinh nguoi dung dang xem
                if (path.StartsWith("/App/", StringComparison.OrdinalIgnoreCase)) return;
                if (path.IndexOf("/Get", StringComparison.OrdinalIgnoreCase) >= 0) return;
                if (path.StartsWith("/signalr", StringComparison.OrdinalIgnoreCase)) return;

                // Bo qua dang xuat: hanh dong do vua xoa phien, ghi nhan lai o day
                // se tao lai ban ghi va nguoi vua thoat van hien dang truc tuyen.
                if (path.StartsWith("/Account/Logout", StringComparison.OrdinalIgnoreCase)) return;

                new SysUserActivityCache().Track(
                    ctx.Session.SessionID,
                    ctx.User.Identity.Name,
                    path,
                    ResolveScreenName(path),
                    Request.UserHostAddress,
                    Request.UserAgent);
            }
            catch (Exception ex)
            {
                // Theo doi hoat dong khong duoc lam hong request cua nguoi dung,
                // nhung van ghi log de con biet duong chan doan khi no khong chay.
                try { AppProcessor.Logger.Error(ex); } catch { }
            }
        }

        /// <summary>
        /// Ten man hinh de doc, chi dung cho nhung duong dan KHONG co trong menu
        /// (trang chu, dang nhap...). Voi cac man hinh co trong menu, proc
        /// p_Sys_UserActivity_Get tu tra ten tu bang Sys_Menus - nho vay ten hien
        /// thi luon trung voi ten nguoi dung thay va tu cap nhat khi menu doi.
        /// </summary>
        private static string ResolveScreenName(string path)
        {
            if (string.IsNullOrEmpty(path)) return null;

            if (path == "/" ||
                path.Equals("/Home", StringComparison.OrdinalIgnoreCase) ||
                path.Equals("/Home/Index", StringComparison.OrdinalIgnoreCase))
                return "Trang chủ";

            if (path.StartsWith("/Account/", StringComparison.OrdinalIgnoreCase))
                return "Tài khoản";

            return path;
        }

        /// <summary>
        /// Ghi nhan hoat dong nguoi dung ngay sau khi controller xu ly xong.
        ///
        /// Phai dung su kien nay chu khong phai BeginRequest hay EndRequest:
        ///   - BeginRequest: Session chua duoc khoi tao
        ///   - EndRequest  : Session da bi giai phong
        /// Chi o PostRequestHandlerExecute thi HttpContext.Session moi con song.
        /// </summary>
        protected void Application_PostRequestHandlerExecute(object sender, EventArgs e)
        {
            TrackUserActivity();
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            if (Response.StatusCode == 302 && Response.RedirectLocation == "/Error/NotFound" && !Request.IsLocal)
            {
                var blockIPCache = new SysBlockIPCache();

                blockIPCache.Request(new SysBlockIPModel
                {
                    IP = Request.UserHostAddress,
                    UrlRequest = Request.RawUrl
                });
            }
        }

        private void ExecuteJob(IJobPlugable jobPlugable, SysJobModel appJobModel)
        {
            NameValueCollection collectionParrams = null;
            if (!string.IsNullOrEmpty(appJobModel.JobParrams) && EString.IsValidJson(appJobModel.JobParrams))
            {
                var dictParrams = JsonConvert.DeserializeObject<Dictionary<string, string>>(appJobModel.JobParrams);
                if (dictParrams != null)
                {
                    collectionParrams = new NameValueCollection(dictParrams.Count);
                    foreach (var k in dictParrams) collectionParrams.Add(k.Key, k.Value);
                }
            }

            var instanceJob = jobPlugable?.BuildJob(appJobModel.JobId.ToString(), appJobModel.JobDescription,
                appJobModel.CronExpression, collectionParrams);
            if (instanceJob == null) return;

            instanceJob.MainJob.IsActive = appJobModel.IsActive;
            JobSchedulerProvider.RegisterJobScheduler(instanceJob.JobType, instanceJob.MainJob);
        }
    }
}