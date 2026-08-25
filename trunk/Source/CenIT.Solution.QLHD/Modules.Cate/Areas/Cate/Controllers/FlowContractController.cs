using Cores.Cate.Caches;
using Cores.eContract.Models;
using Cores.eContract.Models.Request;
using Cores.Sys.Apps;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using TSFramework.App.Attributes;
using TSFramework.Core.Enums;

namespace Modules.Cate.Areas.Cate.Controllers
{
    public class FlowContractController : AppController
    {
        private readonly CateFlowContractCache _flowContractCache = new CateFlowContractCache();

        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        public ActionResult Index()
        {
            return View();

        }

        // Action method to display the list of contract templates
        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult Get()
        {
            var search = Request.Form.GetValues("search[value]")?[0];
            var draw = Request.Form.GetValues("draw")?[0];
            var orderDir = Request.Form.GetValues("order[0][dir]")?[0];
            var startRec = Convert.ToInt32(Request.Form.GetValues("start")?[0]);
            var pageSize = Convert.ToInt32(Request.Form.GetValues("length")?[0]);

            var reqModel = new ReqListFlowContractModel
            {
                KeySearch = string.IsNullOrEmpty(search) ? null : search,
                PropertiesSort = "created",
                Sort = orderDir,
                Page = startRec > 0 ? startRec : 1,
                MaxSize = pageSize,
            };

            var response = _flowContractCache.GetListFlowContract(out int total, reqModel, out _);
            var data = new List<FlowContractModel>();
            if (response != null)
            {
                data = response.ResData.ListFlowContract;
            }
            var result = Json(
                new { draw = Convert.ToInt32(draw), recordsTotal = total, recordsFiltered = total, data },
                JsonRequestBehavior.AllowGet);

            return result;
        }

        // Action method to view details of a contract template
        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult Details(string id)
        {
            var response = _flowContractCache.GetDetailFlowContract(id, out _);

            if (response != null)
            {
                // Pass the template details to the view
                return PartialView("Details", response.ResData);
            }

            // Handle the case where there's an error or no data is returned
            return Content("Error occurred while fetching contract flow details.");
        }

        //public ActionResult GetListPosition()
        //{
        //    return View();
        //}
        //public ActionResult GetListPosition(HttpPostedFileBase attachFile)
        //{
        //    string errMsg;
        //    var response = _flowContractCache.GetListPosition(attachFile, out errMsg);

        //    if (response != null && response.StatusCode == (int)HttpStatusCode.OK)
        //    {
        //        // Pass the template details to the view
        //        return PartialView("_Position", response.ResData);
        //    }
        //    else
        //    {
        //        // Handle the case where there's an error or no data is returned
        //        return Content("Error occurred while fetching list position details.");
        //    }
        //}
    }
}