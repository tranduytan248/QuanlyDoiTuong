using Cores.Sys.Caches.Sys;
using Cores.Sys.Models.Sys;
using Modules.Sys.Areas.Sys.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Hosting;
using System.Web.Mvc;
using Cores.Base.Apps;
using TSFramework.App.Attributes;
using TSFramework.App.Processors;
using TSFramework.Core.Enums;

namespace Modules.Sys.Areas.Sys.Controllers
{
    [AllowAnyPermission]
    public class SysLogController : AppController
    {
        private readonly SysProcedureLogCache _procApi = new SysProcedureLogCache();

        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        public ActionResult Index()
        {
            //var listErrFiles = new List<FileInfo>();
            //var dirErrLogs =
            //    Path.Combine(HostingEnvironment.MapPath("/" + ConfigurationManager.AppSettings["LogPath"]));
            //var errFiles = Directory.GetFiles(dirErrLogs);

            //foreach (var file in errFiles)
            //{
            //    var fi = new FileInfo(file);
            //    if (fi.LastAccessTime.Month == DateTime.Now.Month) listErrFiles.Add(fi);
            //}

            //listErrFiles = listErrFiles.OrderByDescending(fi => fi.LastAccessTime).Take(10).ToList();

            //return View(new SysLogModel { ListErrFiles = listErrFiles });
            return View();
        }

        #region DeleteFile

        [ActionType(Type = EnumActionType.View)]
        [AjaxOnly]
        [HttpGet]
        public ActionResult DeleteFile(string fileName, string type = "Err")
        {
            var fullPathFile = string.Empty;

            if (type == "Err")
                fullPathFile =
                    Path.Combine(HostingEnvironment.MapPath("/" + ConfigurationManager.AppSettings["LogPath"]),
                        fileName);

            if (type == "Inv")
                fullPathFile =
                    Path.Combine(HostingEnvironment.MapPath("/" + ConfigurationManager.AppSettings["InvLogPath"]),
                        fileName);

            if (type == "Job")
                fullPathFile =
                    Path.Combine(HostingEnvironment.MapPath("/" + ConfigurationManager.AppSettings["JobLogPath"]),
                        fileName);

            if (!System.IO.File.Exists(fullPathFile))
                return Json(new
                {
                    status = true,
                    message = CreateMessage("File Log", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Delete"),
                $" tệp: <b>{fileName}</b>");
            var fi = new FileInfoModel { Name = fileName, FullName = fullPathFile };

            return PartialView("_Delete", fi);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult DeleteFile(FileInfoModel fi)
        {
            var isDeleted = false;
            if (System.IO.File.Exists(fi.FullName))
            {
                System.IO.File.Delete(fi.FullName);
                isDeleted = true;
            }

            var response = CreateMessage($"xoá file: <b>{fi.Name}</b>", EnumProcessType.Delete,
                isDeleted ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new
            {
                status = true,
                message = response,
                fileName = Regex.Replace(fi.Name, "[^0-9a-zA-Z]+", "", RegexOptions.Compiled)
            });
        }

        #endregion

        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        public ActionResult DownloadFile(string fileName, string type = "Err")
        {
            var fullPathFile = string.Empty;

            if (type == "Err")
                fullPathFile =
                    Path.Combine(HostingEnvironment.MapPath("/" + ConfigurationManager.AppSettings["LogPath"]),
                        fileName);

            if (type == "Inv")
                fullPathFile =
                    Path.Combine(HostingEnvironment.MapPath("/" + ConfigurationManager.AppSettings["InvLogPath"]),
                        fileName);

            if (type == "Job")
                fullPathFile =
                    Path.Combine(HostingEnvironment.MapPath("/" + ConfigurationManager.AppSettings["JobLogPath"]),
                        fileName);

            if (!System.IO.File.Exists(fullPathFile))
                return Json(new
                {
                    status = true,
                    message = CreateMessage("File Log", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var bytes = System.IO.File.ReadAllBytes(fullPathFile);
            return File(bytes, "text/plain", fileName);
        }

        [ActionType(Type = EnumActionType.View)]
        [AjaxOnly]
        [HttpGet]
        public ActionResult ViewErrFile(string fileName, string type = "Err")
        {
            var fullPathFile = string.Empty;

            if (type == "Err")
                fullPathFile =
                    Path.Combine(HostingEnvironment.MapPath("/" + ConfigurationManager.AppSettings["LogPath"]),
                        fileName);

            if (type == "Inv")
                fullPathFile =
                    Path.Combine(HostingEnvironment.MapPath("/" + ConfigurationManager.AppSettings["InvLogPath"]),
                        fileName);

            if (type == "Job")
                fullPathFile =
                    Path.Combine(HostingEnvironment.MapPath("/" + ConfigurationManager.AppSettings["JobLogPath"]),
                        fileName);

            if (!System.IO.File.Exists(fullPathFile))
                return Json(new
                {
                    status = true,
                    message = CreateMessage("File Log", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            var fileContents = System.IO.File.ReadAllLines(fullPathFile);
            return PartialView("_ViewFile", string.Join(Environment.NewLine, fileContents));
        }

        #region DeleteOldFile

        [ActionType(Type = EnumActionType.View)]
        [AjaxOnly]
        [HttpGet]
        public ActionResult DeleteOldFile(string type = "Err")
        {
            return PartialView("_DeleteOldFile", new DeleteOldFileModel { TypeLog = type });
        }

        [ActionType(Type = EnumActionType.View)]
        [AjaxOnly]
        [HttpPost]
        public ActionResult DeleteOldFile(DeleteOldFileModel model)
        {
            var dirErrLogs = string.Empty;
            string response;

            if (model.TypeLog == "Procedure")
            {
                DateTime? toMonth = DateTime.Now.AddMonths(model.MonthAgo * -1);
                var isSuccess = _procApi.DeleteAll(null, toMonth);
                response = CreateMessage("Procedure Log", EnumProcessType.Delete,
                    isSuccess ? EnumMsgIcon.Success : EnumMsgIcon.Error);

                return Json(new { status = true, message = response, logId = -1 });
            }

            if (model.TypeLog == "Err")
                dirErrLogs =
                    Path.Combine(HostingEnvironment.MapPath("/" + ConfigurationManager.AppSettings["LogPath"]));

            if (model.TypeLog == "Inv")
                dirErrLogs =
                    Path.Combine(HostingEnvironment.MapPath("/" + ConfigurationManager.AppSettings["InvLogPath"]));

            if (model.TypeLog == "Job")
                dirErrLogs =
                    Path.Combine(HostingEnvironment.MapPath("/" + ConfigurationManager.AppSettings["JobLogPath"]));


            var errFiles = Directory.GetFiles(dirErrLogs);

            foreach (var file in errFiles)
            {
                var fi = new FileInfo(file);
                if (fi.LastAccessTime < DateTime.Now.AddMonths(-1 * model.MonthAgo)) fi.Delete();
            }

            response = CreateMessage($"Đã Xoá file log của <b>{model.MonthAgo}</b> tháng trước",
                EnumProcessType.Delete, EnumMsgIcon.Success);
            return Json(new { status = true, message = response });
        }

        #endregion

        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        public ActionResult ErrFile(DateTime? fromMonth, DateTime? toMonth)
        {
            fromMonth = fromMonth ?? DateTime.Now;
            toMonth = toMonth ?? DateTime.Now;

            var listErrFile = new List<FileInfo>();
            var dirErrLogs =
                Path.Combine(HostingEnvironment.MapPath("/" + ConfigurationManager.AppSettings["LogPath"]));
            var errFiles = Directory.GetFiles(dirErrLogs);

            foreach (var file in errFiles)
            {
                var fi = new FileInfo(file);
                if (fi.CreationTime.Month >= fromMonth.Value.Month && fi.CreationTime.Month <= toMonth.Value.Month)
                    listErrFile.Add(fi);
            }

            listErrFile = listErrFile.OrderByDescending(fi => fi.LastAccessTime).Take(30).ToList();

            return PartialView("_ErrFile", new SysLogModel { ListErrFiles = listErrFile });
        }

        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        public ActionResult InvLogFile(DateTime? fromMonth, DateTime? toMonth)
        {
            fromMonth = fromMonth ?? DateTime.Now;
            toMonth = toMonth ?? DateTime.Now;

            var lstInvLogFiles = new List<FileInfo>();
            var dirInvLog =
                Path.Combine(HostingEnvironment.MapPath("/" + ConfigurationManager.AppSettings["InvLogPath"]));
            var invLogFiles = Directory.GetFiles(dirInvLog);

            foreach (var file in invLogFiles)
            {
                var fi = new FileInfo(file);
                if (fi.CreationTime.Month >= fromMonth.Value.Month && fi.CreationTime.Month <= toMonth.Value.Month)
                    lstInvLogFiles.Add(fi);
            }

            lstInvLogFiles = lstInvLogFiles.OrderByDescending(fi => fi.LastAccessTime).Take(30).ToList();

            return PartialView("_InvLogFile", new SysLogModel { ListInvLogFiles = lstInvLogFiles });
        }

        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        public ActionResult JobLogFile(DateTime? fromMonth, DateTime? toMonth)
        {
            fromMonth = fromMonth ?? DateTime.Now;
            toMonth = toMonth ?? DateTime.Now;

            var lstJobLogFiles = new List<FileInfo>();
            var dirJobLogs =
                Path.Combine(HostingEnvironment.MapPath("/" + ConfigurationManager.AppSettings["JobLogPath"]));
            if (Directory.Exists(dirJobLogs))
            {
                var jobLogFiles = Directory.GetFiles(dirJobLogs);

                foreach (var file in jobLogFiles)
                {
                    var fi = new FileInfo(file);
                    if (fi.CreationTime.Month >= fromMonth.Value.Month && fi.CreationTime.Month <= toMonth.Value.Month)
                        lstJobLogFiles.Add(fi);
                }

                lstJobLogFiles = lstJobLogFiles.OrderByDescending(fi => fi.LastAccessTime).Take(30).ToList();
            }

            return PartialView("_JobLogFile", new SysLogModel { ListJobLogFiles = lstJobLogFiles });
        }

        #region ProcedureLogs

        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        public ActionResult ProcedureLogs(DateTime? fromMonth, DateTime? toMonth)
        {
            fromMonth = fromMonth ?? DateTime.Now;
            toMonth = toMonth ?? DateTime.Now;

            var lstProcLogs = _procApi.GetAll(fromMonth, toMonth);

            lstProcLogs = lstProcLogs.Take(30).ToList();

            return PartialView("_ProcedureLog", lstProcLogs);
        }

        [ActionType(Type = EnumActionType.View)]
        [AjaxOnly]
        [HttpGet]
        public ActionResult DeleteProcLog(Guid logId)
        {
            var procLogModel = _procApi.GetById(logId);
            if (procLogModel == null)
            {
                return Json(new
                {
                    status = true,
                    message = CreateMessage("Procedure Log", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            }
            ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Delete"),
                $"<b>Procedure Log [{procLogModel.ProcedureName}]</b>");
            return PartialView("_DeleteProcLog", procLogModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult DeleteProcLog(SysProcedureLogModel procLog)
        {
            var isDeleted = _procApi.Delete(procLog);

            var response = CreateMessage("Procedure Log", EnumProcessType.Delete,
                isDeleted ? EnumMsgIcon.Success : EnumMsgIcon.Error);

            return Json(new
            {
                status = true,
                message = response,
                logId = procLog.LogId
            });
        }

        #endregion
    }
}