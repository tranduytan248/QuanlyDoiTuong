using System.Web.Mvc;
using System.Web.Routing;

namespace CenIT.Solution.QLHD.WebApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "LoginRoute",
                "Login",
                new { controller = "Account", action = "Login", id = UrlParameter.Optional },
                new[] { "CenIT.Solution.QLHD.WebApp.Controllers" }
            );

            // Route rieng cho /App/... - cac action dung chung cua khung
            // (vi du ActionIsAllow ma TSFramework.js goi de kiem tra quyen).
            // BAT BUOC chi dinh namespace: co hai lop cung ten AppController
            // (Cores.Base.Apps va Cores.Sys.Apps), khong chi dinh thi MVC bao
            // "Multiple types were found that match the controller named 'App'".
            routes.MapRoute(
                "AppFrameworkRoute",
                "App/{action}/{id}",
                new { controller = "App", action = "Index", id = UrlParameter.Optional },
                new[] { "Cores.Base.Apps" }
            );

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "CenIT.Solution.QLHD.WebApp.Controllers", "Modules.Major.Controllers" }
            );
        }
    }
}