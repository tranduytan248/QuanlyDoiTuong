using System.Web.Mvc;

namespace Modules.Major.Areas.Invoice
{
    public class InvoiceAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "Invoice";

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Invoice_default",
                "Invoice/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[]
                {
                    "Modules.Major.Areas.Invoice.Controllers"
                }
            );
        }
    }
}