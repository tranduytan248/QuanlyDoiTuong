using System.Web.Mvc;

namespace Modules.Sys.Areas.Cate
{
    public class CateAreaRegistration : AreaRegistration
    {
        public override string AreaName => "Cate";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Sys_Cate_default",
                "Cate/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[]
                {
                    "Modules.Sys.Areas.Cate.Controllers",
                    "Modules.Cate.Areas.Cate.Controllers"
                }
            );
        }
    }
}