using System;
using System.Linq;
using System.Web.Mvc;
using Cores.Base.Apps;
using Cores.Major.Caches;
using TSFramework.App.Attributes;
using TSFramework.App.Models;
using TSFramework.App.Processors;
using TSFramework.Core.Enums;

namespace Modules.Major.Controllers
{
    [AllowAnyPermission]
    public class NotificationController : AppController
    {
        private readonly MajorMessageCache _messageCache = new MajorMessageCache();

        private readonly string _messageTitle = AppProcessor.Messagor.GetMessage("Major_Message_Title");


        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        public ActionResult Messages()
        {
            var lstMessages = _messageCache.Get(User.UserName, out _);
            lstMessages = lstMessages.Count(m => !m.IsReaded) < 10 ? lstMessages.OrderBy(m => m.IsReaded).Take(10).ToList() : lstMessages.Where(m => !m.IsReaded).ToList();

            return PartialView("_Messages", lstMessages);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult Get()
        {
            var search = Request.Form.GetValues("search[value]")?[0];
            var draw = Request.Form.GetValues("draw")?[0];
            var order = Request.Form.GetValues("order[0][column]")?[0];
            var orderDir = Request.Form.GetValues("order[0][dir]")?[0];
            var startRec = Convert.ToInt32(Request.Form.GetValues("start")?[0]);
            var pageSize = Convert.ToInt32(Request.Form.GetValues("length")?[0]);

            var dataSearch = new BaseSearchModel
            {
                Search = string.IsNullOrEmpty(search) ? null : search,
                Order = order,
                OrderDir = orderDir,
                StartIndex = startRec,
                PageSize = pageSize
            };

            var data = _messageCache.Get(User.UserName, out var total, dataSearch);
            var result = Json(
                new { draw = Convert.ToInt32(draw), recordsTotal = total, recordsFiltered = total, data },
                JsonRequestBehavior.AllowGet);
            return result;
        }

        [AjaxOnly]
        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        public ActionResult All()
        {
            return PartialView("_All");
        }

        [AjaxOnly]
        [ActionType(Type = EnumActionType.Edit)]
        [HttpGet]
        public ActionResult MaskAsRead(Guid? id)
        {
            var isSuccess = _messageCache.MaskAsRead(id);
            var response = CreateMessage(_messageTitle, EnumProcessType.Edit, isSuccess ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response });
        }

        [AjaxOnly]
        [ActionType(Type = EnumActionType.Edit)]
        [HttpGet]
        public ActionResult MaskAllAsRead()
        {
            var isSuccess = _messageCache.MaskAllAsRead(User.UserName);
            var response = CreateMessage(_messageTitle, EnumProcessType.Edit, isSuccess ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response });
        }

        [ActionType(Type = EnumActionType.Delete)]
        [AjaxOnly]
        [HttpGet]
        public ActionResult Delete(Guid? id)
        {
            var isSuccess = _messageCache.Delete(id);
            var response = CreateMessage(_messageTitle, EnumProcessType.Delete, isSuccess ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response });
        }

        [AjaxOnly]
        [ActionType(Type = EnumActionType.Delete)]
        [HttpGet]
        public ActionResult DeleteAll()
        {
            var isSuccess = _messageCache.DeleteAll(User.UserName);
            var response = CreateMessage(_messageTitle, EnumProcessType.Delete, isSuccess ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response });
        }
    }
}