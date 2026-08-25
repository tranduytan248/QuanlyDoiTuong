using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Cores.Base.Apps;
using Cores.Base.Interfaces;
using Cores.Cate.Caches;
using Cores.Major.Caches;
using Cores.Major.Models;
using Cores.Sys.Caches.Sys;
using Modules.Major.Areas.Major.Models;
using Modules.Major.Providers;
using TSFramework.App.Attributes;
using TSFramework.App.Processors;
using TSFramework.Core.Enums;

namespace Modules.Major.Areas.Major.Controllers
{
    public class ReportController : AppController
    {
        private readonly MajorReportCache _reportApi = new MajorReportCache();
        private readonly CateUnionCache _unionCache = new CateUnionCache();
        private readonly SysUserCache _userCache = new SysUserCache();
        //private readonly SysConfigCache _configCache = new SysConfigCache();

        private readonly string _pageTitle = AppProcessor.Messagor.GetMessage("Major_Report_Title");
        private readonly List<IReport> _reports = MajorReportProvider.LoadReports();

        // GET: Report
        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var lstPermitReports = _reportApi.GetForUser(User.UserName);

            var lstReports = _reports
                .Where(rpt => lstPermitReports.Exists(rpP => rpP.ReportKey == rpt.ReportKey))
                .OrderBy(i => i.ReportName)
                .ToList();

            return await Task.Run(() => View(lstReports));
        }

        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        [AjaxOnly]
        public async Task<ActionResult> ViewReport(string report)
        {
            var pReport = _reports.FirstOrDefault(r => r.ReportKey == report);
            if (pReport == null)
            {
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_pageTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            }

            ViewBag.Title = pReport.ReportName;

            ReportModel model = new ReportModel
            {
                ReportKey = pReport.ReportKey,
                ReportName = pReport.ReportName,
                ViewName = pReport.ViewName,
                ListUnions = _unionCache.GetUnionsViaManager(User.UserName)
                    .OrderBy(u => u.Manager)
                    .ThenBy(u => u.UnionName)
                    .Select(u => new SelectListItem
                    {
                        Text = u.UnionName,
                        Value = u.UnionId.ToString(),
                        Group = new SelectListGroup { Name = u.Manager }
                    }).ToList()
            };

            return await Task.Run(() => PartialView("_Report", model));
        }

        [ActionType(Type = EnumActionType.View)]
        [AjaxOnly]
        [HttpPost]
        public async Task<ActionResult> ViewReport()
        {
            #region Test

            //_04BaoCaoTongHopDoDac report = new _04BaoCaoTongHopDoDac();

            //var ps = report.CreateParams(Request.Form);
            //var lstParram = ps.ToList();
            //lstParram.Insert(0, User.UserName);
            //var dataReport = new MajorReportCache().GetDataReport(report.ProcedureName, lstParram.ToArray());
            //report.CreateReport(dataReport, Server.MapPath("~/Contents/Modules/Major/TemplateReports/"));
            //report.Export(Response, dataReport, Server.MapPath("~/Contents/Modules/Major/TemplateReports/"));

            #endregion

            var sReportKey = Request.Form["ReportKey"];
            ViewBag.Title = Request.Form["ReportName"];
            var pReport = _reports.FirstOrDefault(r => r.ReportKey == sReportKey);

            ViewBag.ReportViewer = MajorReportProvider.CreateViewExport(pReport, User.UserName, Request.Form, Server.MapPath("~/Contents/Modules/Major/TemplateReports/"));
            return await Task.Run(() => PartialView("_Viewer"));
        }

        [ActionType(Type = EnumActionType.View)]
        [HttpPost]
        public async Task<ActionResult> ExportReport()
        {
            var sReportKey = Request.Form["ReportKey"];
            ViewBag.Title = Request.Form["ReportName"];
            var pReport = _reports.FirstOrDefault(r => r.ReportKey == sReportKey);

            MajorReportProvider.Export(pReport, User.UserName, Request.Form, Response, Server.MapPath("~/Contents/Modules/Major/TemplateReports/"));

            return await Task.Run(() => PartialView("_Viewer"));
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult PermitReport(string forUser)
        {
            var userModel = _userCache.GetByUserName(forUser);
            if (userModel == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{AppProcessor.Messagor.GetMessage("User_Title")}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var permitReports = _reportApi.GetForUser(forUser);

            var reportModel = new MajorReportModel
            {
                ForUser = forUser,
                FullName = userModel.FullName,
                Email = userModel.Email,
                SelectedReports = permitReports != null
                    ? string.Join(",", permitReports.Select(u => u.ReportKey).ToList())
                    : string.Empty,
                ListReports = _reports.Select(r => new ListItem { Text = r.ReportName, Value = r.ReportKey }).ToList()
            };

            return PartialView("_Permit", reportModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult PermitReport(MajorReportModel model)
        {
            if (ModelState.IsValid)
            {
                var retId = _reportApi.SavePermit(model);
                var response =
                    CreateMessage(
                        $"{AppProcessor.Messagor.GetMessage("PermitReport_Message")} [{model.FullName}]",
                        EnumProcessType.NonFormat, retId > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);
                return Json(new { status = true, message = response });
            }

            model.ListReports = _reports.Select(r => new ListItem { Text = r.ReportName, Value = r.ReportKey })
                .ToList();

            return PartialView("_PermitReport", model);
        }
    }
}