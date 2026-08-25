using Cores.Sys.Caches.Cate;
using Cores.Sys.Models.Cate;
using System;
using System.Web.Mvc;
using Cores.Base.Apps;
using TSFramework.App.Attributes;
using TSFramework.App.Models;
using TSFramework.App.Processors;
using TSFramework.Core.Enums;
using TSFramework.Core.Utils;

namespace Modules.Sys.Areas.Cate.Controllers
{
    public class ProvinceController : AppController
    {
        private readonly CateProvinceCache _provinceCache = new CateProvinceCache();
        private readonly string _provinceTitle = AppProcessor.Messagor.GetMessage("Province_Title");

        // GET: Cate/Province
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
            var data = _provinceCache.Get(out int total, dataSearch);
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
            var model = new CateProvinceModel();

            return PartialView("_Add", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Add)]
        [ValidateInput(false)]
        public ActionResult Add(CateProvinceModel model)
        {
            if (!ModelState.IsValid) return PartialView("_Province", model);
            string response;
            var provinceId = _provinceCache.Save(new CateProvinceModel
            {
                ProvinceId = model.ProvinceId,
                ProvinceCode = string.IsNullOrEmpty(model.ProvinceCode)
                    ? EString.RemoveSign4VietnameseString(model.ProvinceName)
                    : model.ProvinceCode,
                ProvinceName = model.ProvinceName,
                UserCreated = User.UserName,
                DateCreated = DateTime.Now
            });
            if (provinceId == 0)
                response = CreateMessage($"{_provinceTitle} [{model.ProvinceName}]", EnumProcessType.Add,
                    EnumMsgIcon.Error);
            else if (provinceId == -9)
                response = CreateMessage($"{_provinceTitle} [ {model.ProvinceName}]", EnumProcessType.DataExisted,
                    EnumMsgIcon.Error
                );
            else
                response = CreateMessage($"{_provinceTitle} [ {model.ProvinceName}]", EnumProcessType.Add,
                    EnumMsgIcon.Success
                );
            return Json(new { status = true, message = response }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult Edit(int id = 0)
        {
            var model = _provinceCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_provinceTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            return PartialView("_Edit", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionType(Type = EnumActionType.Edit)]
        [ValidateInput(false)]
        public ActionResult Edit(CateProvinceModel model)
        {
            if (!ModelState.IsValid) return PartialView("_Province", model);
            string response;
            var provinceId = _provinceCache.Save(new CateProvinceModel
            {
                ProvinceId = model.ProvinceId,
                ProvinceCode = model.ProvinceCode,
                ProvinceName = model.ProvinceName,
                UserCreated = User.UserName,
                DateCreated = DateTime.Now
            });
            if (provinceId == 0)
                response = CreateMessage($"{_provinceTitle} [{model.ProvinceName}]", EnumProcessType.Edit,
                    EnumMsgIcon.Error);
            else if (provinceId == -9)
                response = CreateMessage($"{_provinceTitle} [ {model.ProvinceName}]", EnumProcessType.DataExisted,
                    EnumMsgIcon.Error
                );
            else
                response = CreateMessage($"{_provinceTitle} [ {model.ProvinceName}]", EnumProcessType.Edit,
                    EnumMsgIcon.Success
                );
            return Json(new { status = true, message = response }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(int id = 0)
        {
            var model = _provinceCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_provinceTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Delete"),
                $"<b>{_provinceTitle} [{model.ProvinceName}]</b>");
            return PartialView("_Delete", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(CateProvinceModel model)
        {
            model.UserCreated = User.UserName;
            var deleted = _provinceCache.Delete(model);

            var response = CreateMessage($"{_provinceTitle} [{model.ProvinceName}]",
                EnumProcessType.Delete, deleted ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response });
        }
    }
}