using System.Web.Mvc;
using Cores.Base.Apps;

namespace Modules.Major.Areas.Major.Controllers
{
    public class DashboardController : AppController
    {
        // GET: Major/Dashboard
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Subject", new { area = "Major" });
        }
    }
}
