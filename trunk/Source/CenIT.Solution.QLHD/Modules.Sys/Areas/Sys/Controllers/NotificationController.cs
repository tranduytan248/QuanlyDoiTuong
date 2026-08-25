using Cores.Sys.Caches.Sys;
using Cores.Sys.Models.Sys;
using System;
using System.Linq;
using System.Web.Mvc;
using Cores.Base.Apps;
using TSFramework.App.Attributes;
using TSFramework.App.Models;
using TSFramework.App.Processors;
using TSFramework.Core.Enums;

namespace Modules.Sys.Areas.Sys.Controllers
{
    public class NotificationController : AppController
    {
        private readonly SysNotificationCache _notificationCache = new SysNotificationCache();

        private readonly string _notificationTitle = AppProcessor.Messagor.GetMessage("Notification_Label");

        // GET: Sys/Notification
        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        public ActionResult Index()
        {

            return View();
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
            var data = _notificationCache.Get(out var total, dataSearch);

            var result = Json(
                new { draw = Convert.ToInt32(draw), recordsTotal = total, recordsFiltered = total, data },
                JsonRequestBehavior.AllowGet);
            return result;
        }

        [AjaxOnly]
        [ActionType(Type = EnumActionType.Add)]
        [HttpGet]
        public ActionResult Add()
        {
            var channelTypes = Enum.GetValues(typeof(ChannelTypeEnum)).Cast<ChannelTypeEnum>().ToList();
            var model = new SysNotificationModel
            {
                ChannelTypeList = channelTypes
            };
            return PartialView("_Add", model);
        }

        [HttpPost]
        [ActionType(Type = EnumActionType.Add)]
        public ActionResult Add(SysNotificationModel model)
        {
            if (!ModelState.IsValid)
            {
                // Lấy danh sách lỗi từ ModelState
                return PartialView("_Notification", model);
            }

            var data = _notificationCache.Save(model, User.UserName);

            string response = CreateMessage($"[{model.NotificationCode}]",
              data == (int)EnumStatus.Existed ? EnumProcessType.DataExisted : EnumProcessType.Add,
              data > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);

            return Json(new { status = true, message = response });
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult Edit(Guid id)
        {
            var channelTypes = Enum.GetValues(typeof(ChannelTypeEnum)).Cast<ChannelTypeEnum>().ToList();
            var model = _notificationCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_notificationTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            model.ChannelTypeList = channelTypes;

            return PartialView("_Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult Edit(SysNotificationModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_Notification", model);
            }
            var data = _notificationCache.Save(model, User.UserName);

            string response = CreateMessage($"[ {model.NotificationCode} ]",
              data == (int)EnumStatus.Existed ? EnumProcessType.DataExisted : EnumProcessType.Edit,
              data > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);

            return Json(new { status = true, message = response });
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(Guid id)
        {
            var model = _notificationCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_notificationTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Delete"),
                $"<b>[{model.NotificationCode}] </b>");
            return PartialView("_Delete", model);
        }

        [HttpPost]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(SysNotificationModel model)
        {
            var deleted = _notificationCache.Delete(model);

            var response = CreateMessage($"[ {model.NotificationCode} ]", EnumProcessType.Delete,
                deleted > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response });
        }
    }
}