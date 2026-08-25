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
    public class PurPoseController : AppController
    {
        private readonly CatePurPoseCache _catePurposeCache = new CatePurPoseCache();
        private readonly CateContractTypeCache _cateContractTypeCache = new CateContractTypeCache();
        private readonly string _catePurPoseTitle = AppProcessor.Messagor.GetMessage("catePurPose_Title");

        // GET: Cate/PurPose
        [ActionType(Type = EnumActionType.View)]
        public ActionResult Index()
        {
            var model = new SearchPurPoseModel();

            return View(model);
        }

        /// <summary>
        /// Tìm kiếm 
        /// </summary>
        /// <returns></returns>
        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult Get(SearchPurPoseModel model)
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
            var data = _catePurposeCache.Get(total: out var total,searchValue: model.SearchValue, contractTypeIds: model.TypeContractIds,search: dataSearch);

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
            var model = new CatePurPoseModel
            {
                ListTypeContracts = _cateContractTypeCache.GetAll().OrderBy(d => d.ContractTypeId)
                    .Select(d => new ListItem(d.ContractTypeName, d.ContractTypeId.ToString())).ToList()
            };

            return PartialView("_Add", model);
        }

        /// <summary>
        /// Lưu mẫu hợp đồng
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionType(Type = EnumActionType.Add)]
        public ActionResult Add(CatePurPoseModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ListTypeContracts = _cateContractTypeCache.GetAll().OrderBy(d => d.ContractTypeId)
                   .Select(d => new ListItem(d.ContractTypeName, d.ContractTypeId.ToString())).ToList();
                return PartialView("_PurPose", model);
            }
            var purPoseID = _catePurposeCache.Save(model, User.UserName);

            var response = CreateMessage($"[{model.PurPoseName}]",
                purPoseID == -9 ? EnumProcessType.DataExisted : EnumProcessType.Add,
                purPoseID > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);

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
            var model = _catePurposeCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_catePurPoseTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            model.ListTypeContracts = _cateContractTypeCache.GetAll().OrderBy(d => d.ContractTypeId)
                  .Select(d => new ListItem(d.ContractTypeName, d.ContractTypeId.ToString())).ToList();
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
        public ActionResult Edit(CatePurPoseModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ListTypeContracts = _cateContractTypeCache.GetAll().OrderBy(d => d.ContractTypeId)
                  .Select(d => new ListItem(d.ContractTypeName, d.ContractTypeId.ToString())).ToList();
                return PartialView("_PurPose", model);
            }

            var purposeID = _catePurposeCache.Save(model, User.UserName);

            var response = CreateMessage($"{_catePurPoseTitle} [{model.PurPoseName}]",
                //   giayPhepID == (int)EnumStatus.Existed ? EnumProcessType.DataExisted : EnumProcessType.Edit,
                purposeID == -9 ? EnumProcessType.DataExisted : EnumProcessType.Edit,
                purposeID > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);

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
            var model = _catePurposeCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_catePurPoseTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Delete"),
                $"<b>{_catePurPoseTitle} [{model.PurPoseName}]</b>");
            return PartialView("_Delete", model);
        }

        /// <summary>
        /// Xóa 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(CatePurPoseModel model)
        {
            var deleted = _catePurposeCache.Delete(model);

            var response = CreateMessage($"{_catePurPoseTitle} [{model.PurPoseName}]", EnumProcessType.Delete,
                deleted > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response });
        }
    }
}