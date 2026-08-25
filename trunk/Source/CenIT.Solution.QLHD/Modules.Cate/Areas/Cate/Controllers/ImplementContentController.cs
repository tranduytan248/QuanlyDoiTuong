using Cores.Cate.Caches;
using Cores.Cate.Enum;
using Cores.Cate.Models;

using Cores.Sys.Caches.Sys;
using Modules.Cate.Areas.Cate.Models;
using Newtonsoft.Json;
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
    public class ImplementContentController : AppController
    {
        private readonly CateImplementContentCache _cateImplementContentCache = new CateImplementContentCache();
        private readonly SysConfigCache _sysConfigCache = new SysConfigCache();

        private const string CONFIG_KEY_MAPPING_TYPECONTRACT_TEMPLATE_FILE =
            "CONFIG_KEY_MAPPING_TYPECONTRACT_TEMPLATE_FILE";

        private readonly string _cateImplementContentTitle = AppProcessor.Messagor.GetMessage("cateImplementContent_Title");
        private readonly Dictionary<int, string> _dictMappingTypeContractTemplates = new Dictionary<int, string>();

        public ImplementContentController()
        {
            var configMappingTemplateValue =
                _sysConfigCache.GetViaKey(CONFIG_KEY_MAPPING_TYPECONTRACT_TEMPLATE_FILE)?.ConfigValue;

            if (!string.IsNullOrEmpty(configMappingTemplateValue))
            {
                _dictMappingTypeContractTemplates =
                    JsonConvert.DeserializeObject<Dictionary<int, string>>(configMappingTemplateValue);
            }
        }

        // GET: Cate/ImplementContent
        [ActionType(Type = EnumActionType.View)]
        public ActionResult Index()
        {
            var searchModel = new SearchImplementContentModel();
            return View(searchModel);
        }

        /// <summary>
        /// Tìm kiếm 
        /// </summary>
        /// <returns></returns>
        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult Get(string tuKhoa)
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
            var data = _cateImplementContentCache.Get(out var total, tuKhoa, dataSearch);

            // Lấy thông tin id, url từ descriptionEnum
            //var enumValues = System.Enum.GetValues(typeof(EnumTypeContract))
            //             .Cast<EnumTypeContract>()
            //             .Where(t => (int)t >= 0)
            //             .Select(t =>
            //             {
            //                 string description = EnumHelper.GetDescription(t);
            //                 string[] parts = description.Split('-');
            //                 string id = parts[0].Trim();
            //                 string url = parts[1].Trim();

            //                 return new { Id = id, Url = url };
            //             }).ToList();

            //// Map fileid để lấy thông tin url
            //data.ForEach(item =>
            //{
            //    var matchedEnumValue = enumValues.FirstOrDefault(ev => ev.Id == item.FileId);
            //    if (matchedEnumValue != null)
            //    {
            //        item.FilePath = matchedEnumValue.Url;
            //    }
            //});

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
            var model = new CateImplementContentModel
            {
                ListFileSavDoc = _dictMappingTypeContractTemplates == null ? new List<ListItem>() :
                    _dictMappingTypeContractTemplates.Select(d => new ListItem { Text = AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription((EnumContractType)d.Key)), Value = d.Value }).ToList()
            };

            return PartialView("_Add", model);
        }

        /// <summary>
        /// Lưu mẫu hợp đồng
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionType(Type = EnumActionType.Add)]
        public ActionResult Add(CateImplementContentModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ListFileSavDoc = _dictMappingTypeContractTemplates == null
                    ? new List<ListItem>()
                    : _dictMappingTypeContractTemplates.Select(d => new ListItem
                    {
                        Text = AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription((EnumContractType)d.Key)),
                        Value = d.Value
                    }).ToList();
                return PartialView("_ImplementContent", model);
            }
            var implementContentID = _cateImplementContentCache.Save(model, User.UserName);

            var response = CreateMessage($"[{model.WorkContent}]",
                implementContentID == -9 ? EnumProcessType.DataExisted : EnumProcessType.Add,
                implementContentID > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);

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
            var model = _cateImplementContentCache.GetById(id);

            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_cateImplementContentTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            model.ListFileSavDoc = _dictMappingTypeContractTemplates == null
                ? new List<ListItem>()
                : _dictMappingTypeContractTemplates.Select(d => new ListItem
                {
                    Text = AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription((EnumContractType)d.Key)),
                    Value = d.Value
                }).ToList();

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
        public ActionResult Edit(CateImplementContentModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ListFileSavDoc = _dictMappingTypeContractTemplates == null
                    ? new List<ListItem>()
                    : _dictMappingTypeContractTemplates.Select(d => new ListItem
                    {
                        Text = AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription((EnumContractType)d.Key)),
                        Value = d.Value
                    }).ToList();
                return PartialView("_ImplementContent", model);
            }

            var implementContentID = _cateImplementContentCache.Save(model, User.UserName);

            var response = CreateMessage($"{_cateImplementContentTitle} [{model.WorkContent}]",
                implementContentID == -9 ? EnumProcessType.DataExisted : EnumProcessType.Edit,
                implementContentID > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);

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
            var model = _cateImplementContentCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_cateImplementContentTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Delete"),
                $"<b>{_cateImplementContentTitle} [{model.WorkContent}]</b>");
            return PartialView("_Delete", model);
        }

        /// <summary>
        /// Xóa 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(CateImplementContentModel model)
        {
            var deleted = _cateImplementContentCache.Delete(model);

            var response = CreateMessage($"{_cateImplementContentTitle} [{model.WorkContent}]", EnumProcessType.Delete,
                deleted > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response });
        }
    }
}