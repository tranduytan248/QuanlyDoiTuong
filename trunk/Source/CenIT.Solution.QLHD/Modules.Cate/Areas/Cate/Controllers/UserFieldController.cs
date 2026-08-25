using System;
using System.Web.Mvc;
using Cores.Base.Apps;
using Cores.Cate.Caches;
using Cores.Cate.Models;
using Modules.Cate.Areas.Cate.Models;
using TSFramework.App.Attributes;
using TSFramework.App.Models;
using TSFramework.App.Processors;
using TSFramework.Core.Enums;

namespace Modules.Cate.Areas.Cate.Controllers
{
    /// <summary>
    /// Màn hình phân quyền lĩnh vực cho người dùng.
    /// Người dùng chỉ được xem dữ liệu Đối tượng / Lịch sử vi phạm thuộc những
    /// lĩnh vực được phân công tại đây.
    /// </summary>
    public class UserFieldController : AppController
    {
        private readonly CateUserFieldCache _userFieldCache = new CateUserFieldCache();
        private readonly CateFieldCache _fieldCache = new CateFieldCache();
        private readonly string _userFieldTitle = AppProcessor.Messagor.GetMessage("UserField_Title") ?? "Phân quyền lĩnh vực";

        // GET: Cate/UserField
        public ActionResult Index()
        {
            var searchModel = new SearchUserFieldModel();
            return View(searchModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult Get(SearchUserFieldModel searchModel)
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
            var data = _userFieldCache.Get(out int total, searchModel?.Key, dataSearch);
            return Json(new { draw = Convert.ToInt32(draw), recordsTotal = total, recordsFiltered = total, data },
                JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return Json(new
                {
                    status = false,
                    message = CreateMessage(_userFieldTitle, EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                }, JsonRequestBehavior.AllowGet);
            }

            _fieldCache.InvalidateAll();

            var assignedFields = _userFieldCache.GetByUser(id) ?? new System.Collections.Generic.List<CateFieldModel>();
            var model = new CateUserFieldModel
            {
                UserName = id,
                ListFields = _fieldCache.GetAll(),
                FieldIds = string.Join(",", assignedFields.ConvertAll(item => item.FieldId.ToString()))
            };

            ViewBag.ListFields = model.ListFields;
            return PartialView("_Edit", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Edit)]
        [ValidateInput(false)]
        public ActionResult Edit(CateUserFieldModel model)
        {
            try
            {
                if (model == null || string.IsNullOrWhiteSpace(model.UserName))
                {
                    return Json(new
                    {
                        status = false,
                        message = CreateMessage(_userFieldTitle, EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                    }, JsonRequestBehavior.AllowGet);
                }

                var isSuccess = _userFieldCache.Save(model.UserName, model.FieldIds, User.UserName);

                string response = CreateMessage($"{_userFieldTitle} [{model.UserName}]",
                    EnumProcessType.Edit, isSuccess ? EnumMsgIcon.Success : EnumMsgIcon.Error);
                return Json(new { status = isSuccess, message = response }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "alert('ERROR: " + ex.Message.Replace("'", "\\'") + "');" },
                    JsonRequestBehavior.AllowGet);
            }
        }
    }
}
