using Cores.Cate.Caches;
using Cores.Cate.Enum;
using Cores.Cate.Models;

using Modules.Cate.Areas.Cate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Cores.Base.Apps;
using TSFramework.App.Attributes;
using TSFramework.App.Models;
using TSFramework.App.Processors;
using TSFramework.Core.Enums;
using TSFramework.Core.Helpers;

namespace Modules.Cate.Areas.Cate.Controllers
{
    public class ContractTypeController : AppController
    {
        private readonly CateContractTypeCache _contractTypeCache = new CateContractTypeCache();
        private readonly CateContractTemplateCache _contractTemplateCache = new CateContractTemplateCache();

        private readonly string _cateContractTypeTitle = AppProcessor.Messagor.GetMessage("ContractType_Title");

        private readonly List<ListItem> _lstTypeContracts;

        public ContractTypeController()
        {
            _lstTypeContracts = Enum.GetValues(typeof(EnumContractType))
                .Cast<EnumContractType>()
                .OrderBy(t => AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(t)))
                .Select(t => new ListItem
                {
                    Value = ((int)t).ToString(),
                    Text = AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(t)),
                }).ToList();
        }

        // GET: Cate/ContractType
        [ActionType(Type = EnumActionType.View)]
        public ActionResult Index()
        {
            var lstContractTypes = _contractTypeCache.GetAll();
            var searchModel = new SearchContractTypeModel
            {
                ListTypeContracts = _lstTypeContracts.Where(i => !lstContractTypes.Exists(t => $"{t.ContractTypeId}" == i.Value)).ToList()
            };

            return View(searchModel);
        }

        /// <summary>
        /// Tìm kiếm 
        /// </summary>
        /// <returns></returns>
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
            var data = _contractTypeCache.Get(out var total, dataSearch);

            var result = Json(
                new { draw = Convert.ToInt32(draw), recordsTotal = total, recordsFiltered = total, data },
                JsonRequestBehavior.AllowGet);
            return result;
        }

        /// <summary>
        /// Thêm mới 
        /// </summary>
        /// <returns></returns>
        [AjaxOnly]
        [ActionType(Type = EnumActionType.Add)]
        [HttpGet]
        public ActionResult Add()
        {
            var lstContractTypes = _contractTypeCache.GetAll();

            //var dataSearch = new ReqSearchModel();
            var model = new CateContractTypeModel
            {
                ListContractTemplates = _contractTemplateCache.GetAll()?
                    .Select(ct => new ListItem(text:ct.FullName,value:$"{ct.Id}")).ToList(),
                ListTypeContracts = _lstTypeContracts.Where(i => !lstContractTypes.Exists(t => $"{t.ContractTypeId}" == i.Value)).ToList()
            };

            return PartialView("_Add", model);
        }

        /// <summary>
        /// Lưu mẫu hợp đồng
        /// </summary>
        /// <returns></returns>
        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Add)]
        public ActionResult Add(CateContractTypeModel model)
        {
            if (!ModelState.IsValid)
            {
                var lstContractTypes = _contractTypeCache.GetAll();

                //var dataSearch = new ReqSearchModel();
                //model.ListTemplate = _contractCache.GetListTemplateContract(out _, dataSearch, out _);

                model.ListContractTemplates = _contractTemplateCache.GetAll()?
                    .Select(ct => new ListItem(text: ct.FullName, value: $"{ct.Id}")).ToList();
                model.ListTypeContracts = _lstTypeContracts
                    .Where(i => !lstContractTypes.Exists(t => $"{t.ContractTypeId}" == i.Value)).ToList();

                return PartialView("_ContractType", model);
            }

            model.ContractTypeCode = $"{(EnumContractType)model.ContractTypeId}";

            model.UpdatedBy = User.UserName;

            var contractTypeID = _contractTypeCache.Save(model);

            var response = CreateMessage($"[{model.ContractTypeName}]",
                contractTypeID == -9 ? EnumProcessType.DataExisted : EnumProcessType.Add,
                contractTypeID > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);

            return Json(new { status = true, message = response });
        }

        /// <summary>
        /// Cập nhật
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult Edit(int id)
        {
            var model = _contractTypeCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_cateContractTypeTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            //var dataSearch = new ReqSearchModel();
            //model.ListTemplate = _contractCache.GetListTemplateContract(out _, dataSearch, out _);
            model.ListContractTemplates = _contractTemplateCache.GetAll()?
                .Select(ct => new ListItem(text: ct.FullName, value: $"{ct.Id}")).ToList();

            model.ListTypeContracts = _lstTypeContracts;
            model.IsEdit = true;
            return PartialView("_Edit", model);
        }

        /// <summary>
        /// Cập nhật 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult Edit(CateContractTypeModel model)
        {
            if (!ModelState.IsValid)
            {
                model.IsEdit = true;
                //var dataSearch = new ReqSearchModel();
                //model.ListTemplate = _contractCache.GetListTemplateContract(out _, dataSearch, out _);
                model.ListContractTemplates = _contractTemplateCache.GetAll()?
                    .Select(ct => new ListItem(text: ct.FullName, value: $"{ct.Id}")).ToList();

                model.ListTypeContracts = _lstTypeContracts;

                return PartialView("_ContractType", model);
            }

            model.UpdatedBy = User.UserName;
            var contractTypeID = _contractTypeCache.Save(model);

            var response = CreateMessage($"{_cateContractTypeTitle} [{model.ContractTypeName}]",
                //   giayPhepID == (int)EnumStatus.Existed ? EnumProcessType.DataExisted : EnumProcessType.Edit,
                contractTypeID == -9 ? EnumProcessType.DataExisted : EnumProcessType.Edit,
                contractTypeID > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);

            return Json(new { status = true, message = response });
        }

        /// <summary>
        /// Xóa 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(int id = 0)
        {
            var model = _contractTypeCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_cateContractTypeTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Delete"),
                $"<b>{_cateContractTypeTitle} [{model.ContractTypeName}]</b>");
            return PartialView("_Delete", model);
        }

        /// <summary>
        /// Xóa 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(CateContractTypeModel model)
        {
            var deleted = _contractTypeCache.Delete(model);

            var response = CreateMessage($"{_cateContractTypeTitle} [{model.ContractTypeName}]", EnumProcessType.Delete,
                deleted > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response });
        }
    }
}