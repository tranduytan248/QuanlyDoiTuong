using Core.Inv.Caches;
using Core.Inv.Enums;
using Cores.Sys.Apps;
using Cores.Sys.Caches.Sys;
using Modules.Major.Areas.Major.Models;
using System;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using TSFramework.App.Attributes;
using TSFramework.App.Models;
using TSFramework.Core.Enums;

namespace Modules.Major.Areas.Invoice.Controllers
{
    public class EInvoiceAdjustController : AppController
    {
        private readonly MajorInvPatternCache _invPatternCache = new MajorInvPatternCache();
        private readonly MajorInvAdjustCache _eInvoiceAdjustCache = new MajorInvAdjustCache();
        private readonly SysElnvAccountCache _invAccCache = new SysElnvAccountCache();

        // GET: Invoice/EInvoiceAdjust
        public ActionResult Index()
        {
            var model = new SearchInvoiceModel
            {
                ListTPattern = _invPatternCache.GetAll().OrderBy(d => d.Pattern)
                    .Where(d => d.IsActive)
                    .Select(d => new ListItem(d.Pattern, d.Pattern.ToString())).Distinct().ToList(),
                ListTType = Enum.GetValues(typeof(EnumInvType))
                    .Cast<EnumInvType>()
                    .Select(e => new ListItem(GetEnumDescription(e), ((int)e).ToString()))
                    .ToList(),
                ListUser = _invAccCache.GetAll().Select(d => new ListItem(d.FullName + "-" + d.EmpAccount, d.EmpAccount.ToString())).ToList()
            };
            return View(model);
        }

        private static string GetEnumDescription(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attribute != null ? attribute.Description : value.ToString();
        }

        #region Get

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
            var data = _eInvoiceAdjustCache.GetInvAdjust(total: out int total, userName: User.UserName, managerUnions: searchModel.UnionIds, invNo: searchModel.InvNo, pattern: searchModel.Pattern, serials: searchModel.Serials, invTypes: searchModel.InvTypes, createdFrom: searchModel.CreatedFrom, createdTo: searchModel.CreatedTo, creators: searchModel.Creators, cusName: searchModel.CusName, cusTaxCode: searchModel.CusTaxCode, search: dataSearch);
            var result = Json(
                new { draw = Convert.ToInt32(draw), recordsTotal = total, recordsFiltered = total, data },
                JsonRequestBehavior.AllowGet);
            return result;
        }
        #endregion

    }
}