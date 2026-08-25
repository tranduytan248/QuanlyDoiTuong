using Cores.Cate.Caches;
using Cores.Cate.Models;

using System;
using System.Web.Mvc;
using Cores.Base.Apps;
using TSFramework.App.Attributes;
using TSFramework.App.Models;
using TSFramework.App.Processors;
using TSFramework.Core.Enums;

namespace Modules.Cate.Areas.Cate.Controllers
{
    public class ContractStatusController : AppController
    {
        private readonly CateContractStatusCache _contractStatusCache = new CateContractStatusCache();
        private readonly string _contractStatusTitle = AppProcessor.Messagor.GetMessage("ContractStatus_Title");
        // GET: Cate/ContractStatus
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Get danh sách trạng thái hợp đồng
        /// </summary>
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
            var data = _contractStatusCache.Get(out int total, dataSearch);
            var result = Json(
                new { draw = Convert.ToInt32(draw), recordsTotal = total, recordsFiltered = total, data },
                JsonRequestBehavior.AllowGet);
            return result;
        }

        /// <summary>
        /// Thêm mới trạng thái hợp đồng
        /// </summary>
        /// <returns></returns>
        [AjaxOnly]
        [ActionType(Type = EnumActionType.Add)]
        [HttpGet]
        public ActionResult Add()
        {
            return PartialView("_Add");
        }

        /// <summary>
        /// Thêm mới quy định
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Add)]
        [ValidateInput(false)]
        public ActionResult Add(CateContractStatusModel model)
        {
            ModelState.Remove("ContractStatusId");
            if (!ModelState.IsValid)
            {
                return PartialView("_ContractStatus", model);
            }

            var contractStatusId = _contractStatusCache.Save(model, User.UserName);

            string response = CreateMessage($"{_contractStatusTitle} [{model.ContractStatusName}]",
             contractStatusId == (int)EnumStatus.Existed ? EnumProcessType.DataExisted : EnumProcessType.Add,
             contractStatusId > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Giao diện cập nhật trạng thái hợp đồng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult Edit(int id = 0)
        {
            var model = _contractStatusCache.GetById(id);
            return PartialView("_Edit", model);
        }

        /// <summary>
        /// Cập nhật trạng thái hợp đồng
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionType(Type = EnumActionType.Edit)]
        [ValidateInput(false)]
        public ActionResult Edit(CateContractStatusModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_ContractStatus", model);
            }

            var contractStatusId = _contractStatusCache.Save(model, User.UserName);

            string response = CreateMessage($"{_contractStatusTitle} [{model.ContractStatusName}]",
              contractStatusId == (int)EnumStatus.Existed ? EnumProcessType.DataExisted : EnumProcessType.Edit,
              contractStatusId > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Giao diện xóa trạng thái hợp đồng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(int id = 0)
        {
            var model = _contractStatusCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_contractStatusTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Delete"),
                $"<b>{_contractStatusTitle} [{model.ContractStatusName}]</b>");
            return PartialView("_Delete", model);
        }

        /// <summary>
        /// Xóa trạng thái hợp đồng
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(CateContractStatusModel model)
        {
            var deleted = _contractStatusCache.Delete(model, User.UserName);

            var response = CreateMessage($"{_contractStatusTitle} [{model.ContractStatusName}]",
                EnumProcessType.Delete, deleted ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response });
        }
    }
}