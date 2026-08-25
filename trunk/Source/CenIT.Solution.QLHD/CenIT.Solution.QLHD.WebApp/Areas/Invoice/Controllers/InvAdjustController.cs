using Core.Inv.Caches;
using Cores.Cate.Caches;
using Cores.Sys.Caches.Sys;
using Modules.Major.Areas.Major.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Cores.Base.Apps;
using TSFramework.App.Attributes;
using TSFramework.App.Models;
using TSFramework.Core.Enums;

namespace Modules.Major.Areas.Invoice.Controllers
{
    public class InvAdjustController : AppController
    {
        private readonly MajorInvPatternCache _invPatternCache = new MajorInvPatternCache();
        private readonly MajorInvAdjustCache _invAdjustCache = new MajorInvAdjustCache();
        private readonly SysElnvAccountCache _invAccCache = new SysElnvAccountCache();

        private readonly CateUnionCache _unionCache = new CateUnionCache();

        private static string[] _arrPermissionViaUser;

        // GET: Invoice/EInvoiceAdjust
        public ActionResult Index()
        {
            _arrPermissionViaUser = GetPermissionViaUser(User.UserName);

            var lstUnionsManagerByUser = _unionCache.GetUnionsViaManager(User.UserName);

            var model = new SearchInvoiceModel
            {
                ListUnions = lstUnionsManagerByUser.Select(u => new ListItem(text: u.UnionName, value: $"{u.UnionId}")).ToList(),
                Permissions = _arrPermissionViaUser,

                ListInvPatterns = _invPatternCache.GetAll().OrderBy(d => d.Pattern)
                    .Where(d => d.IsActive)
                    .Select(d => new ListItem(d.Pattern, d.Pattern.ToString())).Distinct().ToList(),
                ListUsers = _invAccCache.GetAll().Select(d => new ListItem($"{d.FullName} - {d.EmpAccount}", d.EmpAccount.ToString())).ToList()
            };
            return View(model);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult Get(SearchInvoiceModel searchModel)
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
            var data = _invAdjustCache.GetInvAdjust(total: out int total, userName: User.UserName, managerUnions: searchModel.UnionIds, invNo: searchModel.InvNo, pattern: searchModel.Pattern, serials: searchModel.Serials, invTypes: searchModel.InvTypes, createdFrom: searchModel.CreatedFrom, createdTo: searchModel.CreatedTo, creators: searchModel.Creators, cusName: searchModel.CusName, cusTaxCode: searchModel.CusTaxCode, search: dataSearch);
            var result = Json(
                new { draw = Convert.ToInt32(draw), recordsTotal = total, recordsFiltered = total, data },
                JsonRequestBehavior.AllowGet);
            return result;
        }

    }
}