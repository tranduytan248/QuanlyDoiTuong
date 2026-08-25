using Cores.Cate.Caches;
using Cores.Cate.Models;

using Modules.Cate.Areas.Cate.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Cores.Base.Apps;
using TSFramework.App.Attributes;
using TSFramework.App.Models;
using TSFramework.App.Processors;
using TSFramework.Core.Enums;

namespace Modules.Cate.Areas.Cate.Controllers
{
    public class ContentLandController : AppController
    {
        private readonly CateContentLandCache _contentLandCache = new CateContentLandCache();
        private readonly CateContractTypeCache _contractTypeCache = new CateContractTypeCache();

        private readonly string _contentLandTitle = AppProcessor.Messagor.GetMessage("ContentLand_Label");

        // GET: Cate/ContentLand
        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        public ActionResult Index()
        {
            var model = new SearchContentLandModel
            {
                ListTypeContracts = _contractTypeCache.GetAll()
                    .OrderBy(d => d.ContractTypeId)
                    .Select(d => new ListItem(d.ContractTypeName, d.ContractTypeId.ToString())).Distinct().ToList()
            };

            return View(model);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult Get(SearchContentLandModel searchModel)
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
            var data = _contentLandCache.Get(out var total, searchModel.TypeContractIds, dataSearch);
            var result = Json(
                new { draw = Convert.ToInt32(draw), recordsTotal = total, recordsFiltered = total, data },
                JsonRequestBehavior.AllowGet);
            return result;
        }

        /// <summary>
        /// Thêm mới đơn giá
        /// </summary>
        /// <returns></returns>
        [AjaxOnly]
        [ActionType(Type = EnumActionType.Add)]
        [HttpGet]
        public ActionResult Add()
        {
            var model = new CateContentLandModel
            {
                ListTypeContracts = _contractTypeCache.GetAll()
                    .OrderBy(d => d.ContractTypeId)
                    .Select(d => new ListItem(d.ContractTypeName, d.ContractTypeId.ToString())).Distinct().ToList()
            };
            return PartialView("_Add", model);
        }

        /// <summary>
        /// Thêm mới đơn giá
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionType(Type = EnumActionType.Add)]
        public ActionResult Add(CateContentLandModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ListTypeContracts = _contractTypeCache.GetAll().OrderBy(d => d.ContractTypeId)
                   .Select(d => new ListItem(d.ContractTypeName, d.ContractTypeId.ToString())).Distinct().ToList();
                return PartialView("_ContentLand", model);
            }

            var data = _contentLandCache.Save(model, User.UserName);

            string response = CreateMessage($"[{model.ContentLandName}]",
              data == (int)EnumStatus.Existed ? EnumProcessType.DataExisted : EnumProcessType.Add,
              data > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);

            return Json(new { status = true, message = response });
        }

        /// <summary>
        /// Cập nhật đơn giá
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult Edit(Guid id)
        {
            var model = _contentLandCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_contentLandTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            model.ListTypeContracts = _contractTypeCache.GetAll().OrderBy(d => d.ContractTypeId)
                .Select(d => new ListItem(d.ContractTypeName, d.ContractTypeId.ToString())).Distinct().ToList();
            return PartialView("_Edit", model);
        }

        /// <summary>
        /// Cập nhật đơn giá
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult Edit(CateContentLandModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ListTypeContracts = _contractTypeCache.GetAll().OrderBy(d => d.ContractTypeId)
                   .Select(d => new ListItem(d.ContractTypeName, d.ContractTypeId.ToString())).Distinct().ToList();
                return PartialView("_ContentLand", model);
            }
            var data = _contentLandCache.Save(model, User.UserName);

            string response = CreateMessage($"[ {model.ContentLandName} ]",
              data == (int)EnumStatus.Existed ? EnumProcessType.DataExisted : EnumProcessType.Edit,
              data > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);

            return Json(new { status = true, message = response });
        }

        /// <summary>
        /// Xóa đơn giá
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(Guid id)
        {
            var model = _contentLandCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_contentLandTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Delete"),
                $"<b>[{model.ContentLandName}] </b>");
            return PartialView("_Delete", model);
        }

        /// <summary>
        /// Xóa đơn giá
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(CateContentLandModel model)
        {
            var deleted = _contentLandCache.Delete(model);

            var response = CreateMessage($"[ {model.ContentLandName} ]", EnumProcessType.Delete,
                deleted > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response });
        }

        #region Extend Functions

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.View)]
        public JsonResult GetContentLandsViaTypeContract(int? typeContract)
        {
            var lstContentLands = _contentLandCache.GetAll($"{typeContract}").Select(cl => new ListItem
            {
                Text = cl.ContentLandName,
                Value = $"{cl.ContentLandId}"
            }).ToList();

            return Json(new { ContentLands = lstContentLands }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}