using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Cores.Base.Apps;
using Cores.eContract.Consts;
using Cores.Major.Caches;
using Cores.Major.Models;
using Cores.Sys.Caches.Cate;
using Cores.Sys.Models.Cate;
using Modules.Major.Areas.Major.Models;
using TSFramework.App.Attributes;
using TSFramework.App.Models;
using TSFramework.App.Processors;
using TSFramework.Core.Enums;
using TSFramework.Core.Utils;

namespace Modules.Major.Areas.Major.Controllers
{
    public class CustomerController : AppController
    {
        private readonly MajorCustomerCache _customerCache = new MajorCustomerCache();

        private readonly CateProvinceCache _provinceCache = new CateProvinceCache();

        private readonly string _customerTitle = AppProcessor.Messagor.GetMessage("Customer_Title");

        // GET: Major/Customer
        public ActionResult Index()
        {
            var searchModel = new SearchCustomerModel();
            return View(searchModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult Get(SearchCustomerModel searchModel)
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
            var data = _customerCache.Get(searchModel.Keyword, searchModel.TypeCus, out var total, dataSearch);
            var result = Json(
                new { draw = Convert.ToInt32(draw), recordsTotal = total, recordsFiltered = total, data },
                JsonRequestBehavior.AllowGet);
            return result;
        }

        /// <summary>
        /// Giao diện thêm mới khách hàng 
        /// </summary>
        /// <returns></returns>
        [AjaxOnly]
        [ActionType(Type = EnumActionType.Add)]
        [HttpGet]
        public ActionResult Add(string typeCus)
        {
            MajorCustomerModel model = new MajorCustomerModel
            {
                TypeCus = typeCus ?? ConstsCusType.CONSUMER,
                TypeCusName = AppProcessor.Messagor.GetMessage($"CusType_{(typeCus ?? ConstsCusType.CONSUMER).ToLower().ToUpperFirstChar()}")
            };

            return PartialView("_Add", model);
        }

        /// <summary>
        /// Thêm mới khách hàng 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionType(Type = EnumActionType.Add)]
        public ActionResult Add(MajorCustomerModel model)
        {
            if (!ModelState.IsValid)
            {
                if (model.TypeCus == ConstsCusType.CONSUMER)
                {
                    return PartialView("_Consumer", model);
                }

                return PartialView("_Business", model);
            }

            model.CusId = Guid.Empty;

            if (model.TypeCus == ConstsCusType.BUSINESS)
            {
                model.CusName = model.EnterpriseName;
            }

            model.UpdatedBy = User.UserName;

            var data = _customerCache.Save(model);

            string response = CreateMessage($"[{model.CusName}]",
              data == (int)EnumStatus.Existed ? EnumProcessType.DataExisted : EnumProcessType.Add,
              data > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);

            return Json(new { status = true, message = response });
        }


        /// <summary>
        ///  Giao diện cập nhật thông tin khách hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult Edit(Guid? id)
        {
            var model = _customerCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_customerTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            model.Address = model.AddressNo + " " + model.StreetName + ", " + model.WardName + ", " + model.ProvinceName;
            model.ListProvinces = _provinceCache.GetAll()
                    .OrderBy(d => d.ProvinceName)
                    .Select(d => new ListItem(d.ProvinceName, d.ProvinceId.ToString())).ToList();

            return PartialView("_Edit", model);
        }

        /// <summary>
        /// Cập nhật thông tin khách hàng
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult Edit(MajorCustomerModel model)
        {
            if (!ModelState.IsValid)
            {
                if (model.TypeCus == ConstsCusType.CONSUMER)
                {
                    return PartialView("_Consumer", model);
                }

                return PartialView("_Business", model);
            }

            string response;

            var customerId = _customerCache.Save(new MajorCustomerModel
            {
                CusId = model.CusId,
                TypeCus = model.TypeCus,
                TypeCusName = model.TypeCusName,
                CusName = model.TypeCus == ConstsCusType.BUSINESS ? model.EnterpriseName : model.CusName,
                TaxCode = model.TaxCode,
                Gender = model.Gender,
                TypeIdentifier = model.TypeIdentifier,
                TypeIdentifierName = model.TypeIdentifierName,
                IdentifierNo = model.IdentifierNo,
                Phone = model.Phone,
                Email = model.Email,
                ProvinceId = model.ProvinceId,
                WardId = model.WardId,
                StreetName = model.StreetName,
                AddressNo = model.AddressNo,
                RefCus = model.RefCus,
                Address = model.Address,

                RepresenterName = model.RepresenterName,
                RepresenterGender = model.RepresenterGender,
                RepresenterTitle = model.RepresenterTitle,
                UpdatedBy = User.UserName
            });

            if (customerId == 0)
                response = CreateMessage($"{_customerTitle} [{model.CusName}]",
                    EnumProcessType.Edit,
                    EnumMsgIcon.Error);
            else if (customerId == -9)
                response = CreateMessage($"{_customerTitle} [{model.CusName}]",
                    EnumProcessType.DataExisted,
                    EnumMsgIcon.Error
                );
            else
                response = CreateMessage($"{_customerTitle} [{model.CusName}]",
                    EnumProcessType.Edit,
                    EnumMsgIcon.Success
                );
            return Json(new { status = true, message = response }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Giao diện xóa thông tin khách hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(Guid? id)
        {
            var model = _customerCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_customerTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Delete"),
                $"<b>{_customerTitle} [{model.CusName}]</b>");
            return PartialView("_Delete", model);
        }

        /// <summary>
        /// Xóa thông tin khách hàng
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(MajorCustomerModel model)
        {
            model.UpdatedBy = User.UserName;
            var deleted = _customerCache.Delete(model, User.UserName);

            var response = CreateMessage($"{_customerTitle} [{model.CusName}]",
                EnumProcessType.Delete, deleted == 1 ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response });
        }


        [AjaxOnly]
        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        public ActionResult Address(string parentId, int? provinceId, int? wardId, string streetName, string addressNo)
        {
            var model = new CateAddressModel
            {
                ParentId = parentId,
                ProvinceId = provinceId,
                WardId = wardId,
                StreetName = streetName,
                AddressNo = addressNo,
                ListProvinces = _provinceCache.GetAll()
                    .OrderBy(d => d.ProvinceName)
                    .Select(d => new ListItem(d.ProvinceName, d.ProvinceId.ToString())).ToList()
            };
            return PartialView("_Address", model);
        }


        [AjaxOnly]
        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        public ActionResult Consumer()
        {
            var model = new MajorCustomerModel
            {
                ListProvinces = _provinceCache.GetAll()
                    .OrderBy(d => d.ProvinceName)
                    .Select(d => new ListItem(d.ProvinceName, d.ProvinceId.ToString())).ToList()
            };
            return PartialView("_Consumer", model);
        }

        [AjaxOnly]
        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        public ActionResult Business()
        {
            var model = new MajorCustomerModel
            {
                ListProvinces = _provinceCache.GetAll()
                    .OrderBy(d => d.ProvinceName)
                    .Select(d => new ListItem(d.ProvinceName, d.ProvinceId.ToString())).ToList()
            };
            return PartialView("_Business", model);
        }
    }
}