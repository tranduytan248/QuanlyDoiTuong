using System.Web.Mvc;
using Cores.Base.Apps;
using TSFramework.App.Processors;

namespace Modules.Sys.Areas.Cate.Controllers
{
    public class ImportController : AppController
    {
        private readonly string _importTitle = AppProcessor.Messagor.GetMessage("Import_Title");

        // GET: Cate/Import
        public ActionResult Index()
        {
            return View();
        }
    }
}