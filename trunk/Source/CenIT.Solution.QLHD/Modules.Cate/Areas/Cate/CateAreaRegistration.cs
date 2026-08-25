using System.Web.Mvc;

namespace Modules.Cate.Areas.Cate
{
    public class CateAreaRegistration : AreaRegistration
    {
        public override string AreaName => "Cate";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Cate_default",
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