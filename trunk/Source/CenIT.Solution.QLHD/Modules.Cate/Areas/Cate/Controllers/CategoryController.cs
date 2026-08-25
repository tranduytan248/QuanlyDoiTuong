using Cores.Cate.Caches;
using Cores.Cate.Enum;
using Cores.Cate.Models;
using Modules.Cate.Areas.Cate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Cores.Base.Apps;
using TSFramework.App.Attributes;
using TSFramework.App.Models;
using TSFramework.App.Processors;
using TSFramework.Core.Enums;
using TSFramework.Core.Helpers;
using TSFramework.Core.Utils;

namespace Modules.Cate.Areas.Cate.Controllers
{
    public class CategoryController : AppController
    {
        private readonly CateCategoryCache _cateCache = new CateCategoryCache();
        private readonly string _categoryTitle = AppProcessor.Messagor.GetMessage("Category_Title");

        // GET: Cate/Category
        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        public ActionResult Index()
        {
            var searchModel = new SearchCategoryModel();
            return View(searchModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult Get(SearchCategoryModel searchModel)
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
            var data = _cateCache.Get(searchModel.CateTypes, out var total, dataSearch);
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
            var model = new CateCategoryModel();

            return PartialView("_Add", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Add)]
        [ValidateInput(false)]
        public ActionResult Add(CateCategoryModel model)
        {
            if (!ModelState.IsValid) return PartialView("_Category", model);
            string response;
            model.CateTypeName = EnumHelper.GetDescription((EnumCateType)model.CateType);

            var categoryId = _cateCache.Save(new CateCategoryModel
            {
                CateId = model.CateId ?? Guid.NewGuid(),
                CateCode = string.IsNullOrEmpty(model.CateCode)
                    ? EString.RemoveSign4VietnameseString(model.CateName)
                    : model.CateCode,
                CateName = model.CateName,
                CateType = model.CateType,
                CateTypeName = model.CateTypeName,
                CateParentId = model.CateParentId,
                Priority = model.Priority,
                Note = model.Note,
                UpdatedBy = User.UserName
            });
            if (categoryId == 0)
                response = CreateMessage($"{_categoryTitle} [{model.CateName} - {model.CateTypeName}]",
                    EnumProcessType.Add,
                    EnumMsgIcon.Error);
            else if (categoryId == -9)
                response = CreateMessage($"{_categoryTitle} [{model.CateName} - {model.CateTypeName}]",
                    EnumProcessType.DataExisted,
                    EnumMsgIcon.Error
                );
            else
                response = CreateMessage($"{_categoryTitle} [{model.CateName} - {model.CateTypeName}]",
                    EnumProcessType.Add,
                    EnumMsgIcon.Success
                );
            return Json(new { status = true, message = response }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult Edit(Guid? id)
        {
            var model = _cateCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_categoryTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            model.ListParentCates = _cateCache.GetAll(model.CateType.ToString())
                .Where(c => c.CateId != id)
                .OrderBy(c => c.CateParentName).ThenBy(c => c.Priority).ThenBy(c => c.CateName)
                .Select(c => new SelectListItem
                {
                    Text = c.CateName,
                    Value = c.CateId.ToString(),
                    Group = new SelectListGroup { Name = c.CateParentName }
                })
                .ToList();
            return PartialView("_Edit", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionType(Type = EnumActionType.Edit)]
        [ValidateInput(false)]
        public ActionResult Edit(CateCategoryModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ListParentCates = _cateCache.GetAll(model.CateType.ToString())
                    .Where(c => c.CateId != model.CateId)
                    .OrderBy(c => c.CateParentName).ThenBy(c => c.Priority).ThenBy(c => c.CateName)
                    .Select(c => new SelectListItem
                    {
                        Text = c.CateName,
                        Value = c.CateId.ToString(),
                        Group = new SelectListGroup { Name = c.CateParentName }
                    })
                    .ToList();
                return PartialView("_Category", model);
            }
            string response;
            model.CateTypeName = EnumHelper.GetDescription((EnumCateType)model.CateType);

            var categoryId = _cateCache.Save(new CateCategoryModel
            {
                CateId = model.CateId,
                CateCode = model.CateCode,
                CateName = model.CateName,
                CateType = model.CateType,
                CateTypeName = model.CateTypeName,
                CateParentId = model.CateParentId,
                Priority = model.Priority,
                Note = model.Note,
                UpdatedBy = User.UserName
            });
            if (categoryId == 0)
                response = CreateMessage($"{_categoryTitle} [{model.CateName} - {model.CateTypeName}]",
                    EnumProcessType.Edit,
                    EnumMsgIcon.Error);
            else if (categoryId == -9)
                response = CreateMessage($"{_categoryTitle} [{model.CateName} - {model.CateTypeName}]",
                    EnumProcessType.DataExisted,
                    EnumMsgIcon.Error
                );
            else
                response = CreateMessage($"{_categoryTitle} [{model.CateName} - {model.CateTypeName}]",
                    EnumProcessType.Edit,
                    EnumMsgIcon.Success
                );
            return Json(new { status = true, message = response }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(Guid? id)
        {
            var model = _cateCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_categoryTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Delete"),
                $"<b>{_categoryTitle} [{model.CateName} - {model.CateTypeName}]</b>");
            return PartialView("_Delete", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(CateCategoryModel model)
        {
            model.UpdatedBy = User.UserName;
            var deleted = _cateCache.Delete(model);

            var response = CreateMessage($"{_categoryTitle} [{model.CateName} - {model.CateTypeName}]",
                EnumProcessType.Delete, deleted ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response });
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult CatesViaType(int cateType)
        {
            var catesViaType = _cateCache.GetAll(cateType.ToString())
                .OrderBy(c => c.Priority).ThenBy(c => c.CateName)
                .ToList();
            var dicCates = new Dictionary<string, List<CateCategoryModel>>();
            if (catesViaType.Count > 0)
            {
                catesViaType.OrderBy(c => c.CateParentName).GroupBy(d => d.CateParentName).ToList()
                    .ForEach(g => { dicCates.Add(g.Key ?? string.Empty, g.ToList()); });
            }

            return Json(new { Cates = dicCates }, JsonRequestBehavior.AllowGet);
        }
    }
}