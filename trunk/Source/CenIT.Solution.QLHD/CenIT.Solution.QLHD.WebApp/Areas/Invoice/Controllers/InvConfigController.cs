using System;
using System.Web.Mvc;
using Core.Inv.Caches;
using Core.Inv.Models;
using Cores.Base.Apps;
using TSFramework.App.Attributes;
using TSFramework.App.Models;
using TSFramework.App.Processors;
using TSFramework.Core.Enums;

namespace Modules.Major.Areas.Invoice.Controllers
{
    public class InvConfigController : AppController
    {
        private readonly string _funcName = AppProcessor.Messagor.GetMessage("InvConfig_Title");
        private readonly MajorInvConfigCache _invConfigCache = new MajorInvConfigCache();

        // GET: 
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
            var data = _invConfigCache.Get(out int total, dataSearch);

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
            var model = new MajorInvConfigModel();
            return PartialView("_Add", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Add)]
        public ActionResult Add(MajorInvConfigModel model)
        {
            
            if (!ModelState.IsValid) return PartialView("_InvConfig", model);
            string response;
            var configId = _invConfigCache.Save(new MajorInvConfigModel
            {
                ConfigId = model.ConfigId,
                ConfigKey = model.ConfigKey,
                ConfigValue = model.ConfigValue,
                ConfigDesc = model.ConfigDesc,
                UpdatedBy = User.UserName
            });

            if (configId == 0)
                response = CreateMessage($"{_funcName} [{model.ConfigKey}]", EnumProcessType.Add, EnumMsgIcon.Success);
            else if (configId == -2)
                response = CreateMessage($"{_funcName} [ {model.ConfigKey}]", EnumProcessType.DataExisted, EnumMsgIcon.Error);
            else
            {
                response = CreateMessage($"{_funcName} [ {model.ConfigKey}]", EnumProcessType.Add, EnumMsgIcon.Success);
            }

            return Json(new
            {
                status = true,
                message = response
            }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult Edit(Guid? id)
        {
            var model = _invConfigCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_funcName}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            return PartialView("_Edit", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult Edit(MajorInvConfigModel model)
        {
            if (!ModelState.IsValid) return PartialView("_InvConfig", model);

            var configId = _invConfigCache.Save(new MajorInvConfigModel
            {
                ConfigId = model.ConfigId,
                ConfigKey = model.ConfigKey,
                ConfigValue = model.ConfigValue,
                ConfigDesc = model.ConfigDesc,
                UpdatedBy = User.UserName
            });

            string response;
            if (configId == 0)
                response = CreateMessage($"{_funcName} [{model.ConfigKey}]", EnumProcessType.Add, EnumMsgIcon.Success);
            else if (configId == -2)
                response = CreateMessage($"{_funcName} [ {model.ConfigKey}]", EnumProcessType.DataExisted, EnumMsgIcon.Error);
            else
            {
                response = CreateMessage($"{_funcName} [ {model.ConfigKey}]", EnumProcessType.Add, EnumMsgIcon.Success);
            }

            return Json(new
            {
                status = true,
                message = response
            }, JsonRequestBehavior.AllowGet);
        }


        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(Guid? id)
        {
            var model = _invConfigCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_funcName}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Delete"),
                $"<b>{_funcName} [{model.ConfigKey}]</b>");

            return PartialView("_Delete", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(MajorInvConfigModel model)
        {
            var invConfig = _invConfigCache.GetById(model.ConfigId);
            invConfig.UpdatedBy = User.UserName;
            var deleted = _invConfigCache.Delete(invConfig);

            var response = CreateMessage($"{_funcName} [{model.ConfigKey}]", EnumProcessType.Delete,
                deleted ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response });
        }
    }
}