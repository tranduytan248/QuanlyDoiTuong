using System.Web.Mvc;

namespace Modules.Major.Areas.Major
{
    public class MajorAreaRegistration : AreaRegistration
    {
        public override string AreaName => "Major";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Major_default",
                "Major/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[]
                {
                    "Modules.Major.Areas.Major.Controllers"
                }
            );
        }
    }
}