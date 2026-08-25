using Modules.Major.Areas.Major.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Cores.Major.Caches;
using TSFramework.App.Attributes;
using TSFramework.App.Models;
using TSFramework.Core.Enums;
using Microsoft.Reporting.WebForms;
using System.IO;
using System.Linq;
using Cores.Sys.Caches.Sys;
using Syncfusion.EJ2.Base;
using Cores.Cate.Caches;
using System.Web.UI.WebControls;
using Cores.Base.Apps;

namespace Modules.Major.Controllers
{
    [AllowAnyPermission]
    public class DashboardController : AppController
    {
        private readonly SysConfigCache _sysConfigCache = new SysConfigCache();
        private readonly MajorContractCache _contractCache = new MajorContractCache();
        private readonly CateUnionCache _unionCache = new CateUnionCache();

        private const string CONFIG_KEY_REPORTS_TEMPLATE_PATH = "CONFIG_KEY_REPORTS_TEMPLATE_PATH";

        private readonly string _reportTemplatePath = "/Contents/Modules/Major/TemplateReports/";

        public DashboardController()
        {
            var configModel = _sysConfigCache.GetViaKey(CONFIG_KEY_REPORTS_TEMPLATE_PATH);
            if (configModel != null && !string.IsNullOrEmpty(configModel.ConfigValue))
            {
                _reportTemplatePath = configModel.ConfigValue;
            }
        }

        // GET: Dashboard
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Subject", new { area = "Major" });
        }

        //[AllowAnonymous]
        [HttpPost]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult GetAll(SearchContractModel dm)
        {
            var arrStatusContracts = new Dictionary<int, string> {
                {-1, "info"},
                {0, "warning"},
                {1, "primary"},
                {2, "secondary"},
                {3, "success"},
                {4, "purple"},
                {99, "danger"}
            };

            var dataContracts = _contractCache.Dashboard(out _, User.UserName, dm.UnionIds, dm.SearchValue, dm.FromDate, dm.ToDate, dm.GiveResultFromDate, dm.GiveResultToDate, dm.ContractStatus, dm.TypeContractIds, dm.TypeCusIds, dm.TypeTermIds);

            var dataSource = dataContracts.Select(d => new ViewContractModel
            {
                ContractNoInfo = d.ContractNoInfo,
                CusName = d.CusName,
                PurposeName = d.PurposeName,
                Address = d.Address,
                //ConfirmOn = d.ConfirmOn?.ToString("yyyy-MM-dd HH:mm:ss"),
                ConfirmOn = d.ConfirmOn,
                //GiveResultOn = d.GiveResultOn?.ToString("yyyy-MM-dd HH:mm:ss"),
                GiveResultOn = d.GiveResultOn,
                Status = d.Status,
                StatusColor = arrStatusContracts[d.Status ?? -1],
                StatusName = d.StatusName,
                RemainingTime = d.RemainingTime,
                ContractId = d.ContractId,
            }).ToList();

            DataOperations operation = new DataOperations();
            if (dm.IsLazyLoad == false && dm.Sorted != null && dm.Sorted.Count > 0) //Sorting for grouping
            {
                dataSource = operation.PerformSorting(dataSource, dm.Sorted).ToList();
            }

            int iCount = dataSource.Count;
            if (dm.IsLazyLoad == false && dm.Skip != 0)
            {
                dataSource = operation.PerformSkip(dataSource, dm.Skip).ToList(); // Paging
            }

            if (dm.IsLazyLoad == false && dm.Take != 0)
            {
                dataSource = operation.PerformTake(dataSource, dm.Take).ToList();
            }

            //IEnumerable groupedData = null;

            //if (dm.IsLazyLoad)
            //{
            //    groupedData = operation.PerformGrouping<ViewContractModel>(dataSource, dm); // Lazy load grouping
            //    groupedData = operation.PerformSorting(groupedData, dm); // Sorting with Lazy load grouping
            //    if (dm.OnDemandGroupInfo != null && dm.Group.Count == dm.OnDemandGroupInfo.Level)
            //    {
            //        iCount = groupedData.Cast<ViewContractModel>().Count();
            //    }
            //    else
            //    {
            //        iCount = groupedData.Cast<Group>().Count();
            //    }
            //    groupedData = operation.PerformSkip(groupedData, dm.OnDemandGroupInfo?.Skip ?? dm.Skip);
            //    groupedData = operation.PerformTake(groupedData, dm.OnDemandGroupInfo?.Take ?? dm.Take);
            //}
            //if(!dm.RequiresCounts) Json(dataSource, JsonRequestBehavior.AllowGet);

            //if (groupedData == null)
            return Json(new { result = dataSource, count = iCount }, JsonRequestBehavior.AllowGet);

            //return Json(new { result = groupedData.Cast<Group>(), count = iCount }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult Get(SearchContractModel searchModel)
        {
            var search = Request.Form.GetValues("search[value]")?[0];
            var draw = Request.Form.GetValues("draw")?[0];
            var order = Request.Form.GetValues("order[0][column]")?[0];
            var orderDir = Request.Form.GetValues("order[0][dir]")?[0];
            var startRec = Convert.ToInt32(Request.Form.GetValues("start")?[0]);
            var pageSize = Convert.ToInt32(Request.Form.GetValues("length")?[0]);
            var dataSearch = new BaseSearchModel
            {
                Search = string.IsNullOrEmpty(search) ? null : search,
                Order = order,
                OrderDir = orderDir,
                StartIndex = startRec,
                PageSize = pageSize
            };

            Session[$"Dashboard-SearchQuery-{User.UserName}"] = dataSearch;

            var data = _contractCache.Dashboard(out var total, User.UserName, searchModel.UnionIds, searchModel.SearchValue, searchModel.FromDate, searchModel.ToDate, searchModel.GiveResultFromDate, searchModel.GiveResultToDate, searchModel.ContractStatus, searchModel.TypeContractIds, searchModel.TypeCusIds, searchModel.TypeTermIds, dataSearch);

            var result = Json(new { draw = Convert.ToInt32(draw), recordsTotal = total, recordsFiltered = total, data }, JsonRequestBehavior.AllowGet);
            return result;
        }

        [ActionType(Type = EnumActionType.View)]
        [HttpPost]
        public ActionResult ExportContract(SearchContractModel searchModel)
        {
            BaseSearchModel dataSearchModel = (BaseSearchModel)Session[$"Dashboard-SearchQuery-{User.UserName}"];
            if (dataSearchModel != null)
                dataSearchModel.PageSize = -1;

            searchModel.UnionIds = searchModel.ListUnionIds?.Count > 0 ? string.Join(",", searchModel.ListUnionIds) : null;
            searchModel.ContractStatus = searchModel.ListContractStatusIds?.Count > 0 ? string.Join(",", searchModel.ListContractStatusIds) : null;
            searchModel.TypeContractIds = searchModel.ListTypeContractIds?.Count > 0 ? string.Join(",", searchModel.ListTypeContractIds) : null;
            searchModel.TypeCusIds = searchModel.ListTypeCusIds?.Count > 0 ? string.Join(",", searchModel.ListTypeCusIds) : null;
            searchModel.TypeTermIds = searchModel.ListTypeTermIds?.Count > 0 ? string.Join(",", searchModel.ListTypeTermIds) : null;

            var dataContracts = _contractCache.Dashboard(out _, User.UserName, searchModel.UnionIds, searchModel.SearchValue, searchModel.FromDate, searchModel.ToDate, searchModel.GiveResultFromDate, searchModel.GiveResultToDate, searchModel.ContractStatus, searchModel.TypeContractIds, searchModel.TypeCusIds, searchModel.TypeTermIds, dataSearchModel);

            #region Process Export File

            var rptTemplateName = "rptDashboardContract.rdlc";
            var fullPathRdlc = Path.Combine(Server.MapPath(_reportTemplatePath), rptTemplateName);

            var reportFilename = $"Danh sách hợp đồng_{DateTime.Now:dd/MM/yyyy}";

            // Setup the report viewer object and get the array of bytes
            var reportExcel = new ReportViewer { ProcessingMode = ProcessingMode.Local };

            reportExcel.LocalReport.ReportPath = fullPathRdlc;
            reportExcel.LocalReport.DataSources.Clear();
            reportExcel.LocalReport.DataSources.Add(new ReportDataSource("DashboardContracts", dataContracts));

            //Chuyển sang Excel
            var bytes = reportExcel.LocalReport.Render("EXCELOPENXML", null, out _, out _, out var extension,
                out _, out _);

            #endregion

            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                reportFilename + "." + extension);
        }
    }
}