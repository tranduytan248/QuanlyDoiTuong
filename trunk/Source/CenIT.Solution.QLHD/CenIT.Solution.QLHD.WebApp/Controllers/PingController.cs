using Cores.Base.Apps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CenIT.Solution.QLHD.WebApp.Controllers
{
    public class PingController : AppController
    {
        // GET: Ping
        public ActionResult Index(string id)
        {
            return View();
        }
    }
}