using System.Web.Mvc;
using Cores.Base.Apps;

namespace Modules.Sys.Areas.Sys.Controllers
{
    public class HomeController : AppController
    {
        // GET: Sys/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}