using System.Web.Mvc;
using Cores.Base.Apps;
using TSFramework.App.Attributes;

namespace CenIT.Solution.QLHD.WebApp.Controllers
{
    //[AllowAnyPermission]
    [AllowAnonymous]
    public class ErrorController : AppController
    {
        [AllowAnyPermission]
        public ActionResult AccessDenied()
        {
            Response.StatusCode = 403;
            return View();
        }

        [AllowAnyPermission]
        public ActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View();
        }

        [AllowAnyPermission]
        public ActionResult Error()
        {
            Response.StatusCode = 500;
            return View();
        }
    }
}