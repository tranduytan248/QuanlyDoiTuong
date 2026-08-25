using System.Web.Mvc;
using Cores.Base.Apps;
using TSFramework.App.Attributes;
using TSFramework.Core.Enums;

namespace Modules.Major.Areas.Invoice.Controllers
{
    [AllowAnonymous]
    public class InvCheckingController : AppController
    {
        // GET: Invoice/InvChecking
        [ActionType(Type = EnumActionType.View)]
        public ActionResult Index()
        {
            return View();
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult WarningInv(int type = 1)
        {
            return PartialView("_WarningInv", type);
        }
    }
}