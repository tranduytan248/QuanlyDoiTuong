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
            if (ConfigurationManager.AppSettings["MaintenanceMode"] != "true")
            {
                //if (!Request.IsLocal)
                {
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
                    if (ipRequestModel == null || !ipRequestModel.IsLock) return;

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
                }
            }

            if (!Request.IsLocal) HttpContext.Current.RewritePath("AppOffline.htm");
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