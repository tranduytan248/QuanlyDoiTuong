using Cores.Cate.Caches;
using Cores.Cate.Enum;
using Cores.Cate.Models;
using Cores.Sys.Apps;
using Cores.Sys.Caches.Cate;
using Cores.Sys.Models.Cate;
using Modules.Cate.Areas.Cate.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.WebPages;
using TSFramework.App.Attributes;
using TSFramework.App.Models;
using TSFramework.App.Processors;
using TSFramework.Core.Enums;

namespace Modules.Cate.Areas.Cate.Controllers
{
    public class CustomerController : AppController
    {

        private readonly CateCustomerCache _customerCache = new CateCustomerCache();
        private readonly CateProvinceCache _provinceCache = new CateProvinceCache();
        private readonly CateDistrictCache _districtCache = new CateDistrictCache();
        private readonly CateWardCache _wardCache = new CateWardCache();
        private readonly CateStreetCache _streetCache = new CateStreetCache();
        private readonly string _customerTitle = AppProcessor.Messagor.GetMessage("Customer_Title");

        // GET: Cate/Customer
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
            var data = _customerCache.Get(searchModel.UserType,searchModel.FullName, out var total, dataSearch);
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
        public ActionResult Add()
        {
            var model = new CateCustomerModel
            {
                Provinces = _provinceCache.GetAll(),
                Districts = new List<CateDistrictModel>(),
                Wards = new List<CateWardModel>(),
                Streets = new List<CateStreetModel>(),
            };
            return PartialView("_Add", model);
        }

        /// <summary>
        /// Giao diện thêm mới khách hàng 
        /// </summary>
        /// <returns></returns>
        [AjaxOnly]
        [ActionType(Type = EnumActionType.Add)]
        [HttpGet]
        public ActionResult EditAddress(string appN, string alley, int? pId, int? dId, int? wId, int? sId, string pName, string dName, string wName, string sName)
        {
            var model = new CateCustomerModel
            {
                ProvinceName = pName,
                DistrictName = dName,
                WardName = wName,
                StreetName = sName,
                ApartmentNumber = appN,
                Alley = alley,
                ProvinceId = pId,
                DistrictId = dId,
                WardId = wId,
                StreetId = sId,
                Provinces = _provinceCache.GetAll(),
                Districts = pId != null ? _districtCache.GetAll(pId.ToString()) : new List<CateDistrictModel>(),
                Wards = dId != null ? _wardCache.GetByDistrict(dId) : new List<CateWardModel>(),
                Streets = wId != null ? _streetCache.GetByWard(wId) : new List<CateStreetModel>(),
            };
            return PartialView("_EditAddress", model);
        }

        /// <summary>
        /// Thêm mới khách hàng 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionType(Type = EnumActionType.Add)]
        public ActionResult EditAddress(CateCustomerModel model)
        {

            if (model.ProvinceId == null || model.DistrictId == null || model.WardId == null)
            {
                model.Provinces = _provinceCache.GetAll();
                model.Districts = model.ProvinceId != null ? _districtCache.GetAll(model.ProvinceId.ToString()) : new List<CateDistrictModel>();
                model.Wards = model.DistrictId != null ? _wardCache.GetByDistrict(model.DistrictId) : new List<CateWardModel>();
                model.Streets = model.WardId != null ? _streetCache.GetByWard(model.WardId) : new List<CateStreetModel>();
                return PartialView("_Address", model);
            }


            return Json(new { status = true, data = model });
        }

        /// <summary>
        /// Thêm mới khách hàng 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionType(Type = EnumActionType.Add)]
        public ActionResult Add(CateCustomerModel model)
        {
            model.Provinces = _provinceCache.GetAll();
            model.Districts = model.ProvinceId != null ? _districtCache.GetAll(model.ProvinceId.ToString()) : new List<CateDistrictModel>();
            model.Wards = model.DistrictId != null ? _wardCache.GetByDistrict(model.DistrictId) : new List<CateWardModel>();
            model.Streets = model.WardId != null ? _streetCache.GetByWard(model.WardId) : new List<CateStreetModel>();

            if (!ModelState.IsValid)
            {
                // kiểm tra là tổ chức phải nhập đơn vị và chức vụ
                if (model.UserType == "BUSINESS")
                {
                    if (model.UnionName.IsEmpty())
                    {
                        ModelState.AddModelError("UnionName", AppProcessor.Messagor.GetMessage("UnionId_NotEmpty"));
                    }

                    if (model.PositionName.IsEmpty())
                    {
                        ModelState.AddModelError("PositionName", AppProcessor.Messagor.GetMessage("PositionId_NotEmpty"));
                    }
                }

                return PartialView("_Customer", model);
            }

            if (model.UserType == "BUSINESS")
            {
                if (model.UnionName.IsEmpty())
                {
                    ModelState.AddModelError("UnionName", AppProcessor.Messagor.GetMessage("UnionId_NotEmpty"));
                    return PartialView("_Customer", model);
                }

                if (model.PositionName.IsEmpty())
                {
                    ModelState.AddModelError("PositionName", AppProcessor.Messagor.GetMessage("PositionId_NotEmpty"));
                    return PartialView("_Customer", model);
                }
            }
            // Kiểm tra validate Passport và cccd
            if (model.PageType == ((int)EnumPageType.Passport))
            {
                if (model.CitizenIdentification != null && !CheckValidatePassport(model.CitizenIdentification))
                {
                    ModelState.AddModelError("CitizenIdentification", AppProcessor.Messagor.GetMessage("Passpord Malformed"));
                    return PartialView("_Customer", model);
                }
            }
            else if (model.PageType == ((int)EnumPageType.CitizenIdentification))
            {
                if (model.CitizenIdentification != null && !checkCitizentIdentity(model.CitizenIdentification))
                {
                    ModelState.AddModelError("CitizenIdentification", AppProcessor.Messagor.GetMessage("CitizenIdentification Malformed"));
                    return PartialView("_Customer", model);
                }
            }
            model.CustomerId = Guid.Empty;
            var data = _customerCache.Save(model, User.UserName);

            string response = CreateMessage($"[{model.FullName}]",
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

            model.Provinces = _provinceCache.GetAll();
            model.Districts = _districtCache.GetAll(model.ProvinceId.ToString());
            model.Wards = _wardCache.GetByDistrict(model.DistrictId);
            model.Streets = _streetCache.GetByWard(model.WardId);

            return PartialView("_Edit", model);
        }

        /// <summary>
        /// Cập nhật thông tin khách hàng
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionType(Type = EnumActionType.Edit)]
        [ValidateInput(false)]
        public ActionResult Edit(CateCustomerModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Provinces = _provinceCache.GetAll();
                model.Districts = _districtCache.GetAll(model.ProvinceId.ToString());
                model.Wards = _wardCache.GetByDistrict(model.DistrictId);
                model.Streets = _streetCache.GetByWard(model.WardId);
                return PartialView("_Customer", model);
            }

            string response;

            var customerId = _customerCache.Save(new CateCustomerModel
            {
                CustomerId = model.CustomerId,
                FullName = model.FullName,
                Gender = model.Gender,
                UserType = model.UserType,
                TaxCode = model.TaxCode,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                Zalo = model.Zalo,
                CitizenIdentification = model.CitizenIdentification,
                PositionName = model.PositionName,
                UnionName = model.UnionName,
                ProvinceId = model.ProvinceId,
                DistrictId = model.DistrictId,
                WardId = model.WardId,
                StreetId = model.StreetId,
                BusinessCode = model.BusinessCode,
                PlaceGetCitizenIdentification = model.PlaceGetCitizenIdentification,
                PlaceGetBusinessCode = model.PlaceGetBusinessCode,
                DateGetBusinessCode = model.DateGetBusinessCode,
                PageType = model.PageType,
                Alley = model.Alley,
                ApartmentNumber = model.ApartmentNumber,
            }, User.UserName);

            if (customerId == 0)
                response = CreateMessage($"{_customerTitle} [{model.FullName}]",
                    EnumProcessType.Edit,
                    EnumMsgIcon.Error);
            else if (customerId == -9)
                response = CreateMessage($"{_customerTitle} [{model.FullName}]",
                    EnumProcessType.DataExisted,
                    EnumMsgIcon.Error
                );
            else
                response = CreateMessage($"{_customerTitle} [{model.FullName}]",
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
                $"<b>{_customerTitle} [{model.FullName}]</b>");
            return PartialView("_Delete", model);
        }

        public ActionResult ExportCustomer(string fullName, string userType)
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
            var data = _customerCache.Get(userType, fullName, out var total, dataSearch);

            if (data.Count == 0)
            {
                return Json(new { status = false, message = AppProcessor.Messagor.GetMessage("No_Data") });
            }

            // tạo file Excel
            var package = CreateExeclFile(data);

            // luu file
            string fileName = "TTKhachHang" + DateTime.Now.ToString("_ddMMyyyy_HHmmss");
            return File(package.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName + ".xlsx");
        }

        /// <summary>
        /// Tạo file excel từ danh sách phiếu xuất 
        /// </summary>
        /// <returns></returns>
        private ExcelPackage CreateExeclFile(List<CateCustomerModel> list)
        {
            if (list.Count == 0) return null;
            CultureInfo cultureInfo = new CultureInfo("vi-VN");
            string fileName = "BanDoc" + DateTime.Now.ToString("_ddMMyyyy_HHmmss");

            var package = new ExcelPackage(new FileInfo(fileName));

            var worksheet = package.Workbook.Worksheets.Add(AppProcessor.Messagor.GetMessage("Customer_Title"));

            // Set default width cho tất cả column
            worksheet.DefaultColWidth = 15;
            worksheet.Column(1).Width = 5;
            worksheet.Column(2).Width = 25;
            worksheet.Column(3).Width = 30;
            worksheet.Column(4).Width = 25;
            worksheet.Column(6).Width = 25;

            // Điền thông tin phiếu nhập vào sheet
            worksheet.Cells["A1:L1"].Merge = true;
            worksheet.Cells["A1:L1"].Value =
                string.Concat(AppProcessor.Messagor.GetMessage("Customer_Title").ToUpper());

            worksheet.Cells["A1:L1"].Style.Font.Name = "Arial";
            worksheet.Cells["A1:L1"].Style.Font.Size = 30;
            worksheet.Cells["A1:L1"].Style.Font.Bold = true;
            worksheet.Cells["A1:L1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            // Tạo tiêu đề cho danh sách chi tiết phiếu xuất
            worksheet.Cells["A2"].Value = AppProcessor.Messagor.GetMessage("Column_No");
            worksheet.Cells["B2"].Value = AppProcessor.Messagor.GetMessage("Customer_Label_UserType");
            worksheet.Cells["C2"].Value = AppProcessor.Messagor.GetMessage("Union_Label_Name");
            worksheet.Cells["D2"].Value = AppProcessor.Messagor.GetMessage("Customer_Label_FullName");
            worksheet.Cells["E2"].Value = AppProcessor.Messagor.GetMessage("Customer_Label_Gender");
            worksheet.Cells["F2"].Value = AppProcessor.Messagor.GetMessage("Customer_Label_PhoneNumber");
            worksheet.Cells["G2"].Value = AppProcessor.Messagor.GetMessage("Customer_Label_Email");
            worksheet.Cells["H2"].Value = AppProcessor.Messagor.GetMessage("Customer_Label_TaxCode");
            worksheet.Cells["I2"].Value = AppProcessor.Messagor.GetMessage("Customer_Label_Address");
            worksheet.Cells["K2"].Value = AppProcessor.Messagor.GetMessage("Customer_Label_PageType");
            worksheet.Cells["L2"].Value = AppProcessor.Messagor.GetMessage("Customer_Label_CitizenIdentification");


            // Lấy range vào tạo format cho range đó ở đây là từ A6 den G6
            using (var range = worksheet.Cells["A2:L2"])
            {
                // Set PatternType
                range.Style.Fill.PatternType = ExcelFillStyle.DarkGray;
                // Set Màu cho Background
                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Aqua);
                // Canh giữa cho các text
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                // Set màu va style cho Border
                range.Style.Border.BorderAround(ExcelBorderStyle.Dashed, System.Drawing.Color.Black);
            }

            // tạo bảng bạn đọc
            int count = 0;
            int tableRow = 3;
            foreach (var customer in list)
            {
                count++;
                worksheet.Cells["A" + tableRow].Value = count;
                worksheet.Cells["B" + tableRow].Value = customer.UserType == "BUSINESS" ? "Doanh nghiệp" : "Cá nhân";
                worksheet.Cells["C" + tableRow].Value = customer.UnionName;
                worksheet.Cells["D" + tableRow].Value = customer.FullName;
                worksheet.Cells["E" + tableRow].Value = customer.Gender ? "Nam" : "Nữ";
                worksheet.Cells["F" + tableRow].Value = customer.PhoneNumber;
                worksheet.Cells["G" + tableRow].Value = customer.Email;
                worksheet.Cells["H" + tableRow].Value = customer.TaxCode;
                worksheet.Cells["I" + tableRow].Value = customer.Address;
                worksheet.Cells["K" + tableRow].Value = customer.PageType == (int)EnumPageType.Passport ? "Hộ chiếu" : customer.PageType == (int)EnumPageType.CitizenIdentification ? "CCCD" : "Khác";
                worksheet.Cells["L" + tableRow].Value = customer.CitizenIdentification;
                tableRow++;
            }
            return package;
        }

        /// <summary>
        /// Xóa thông tin khách hàng
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(CateCustomerModel model)
        {
            model.UpdatedBy = User.UserName;
            var deleted = _customerCache.Delete(model, User.UserName);

            var response = CreateMessage($"{_customerTitle} [{model.FullName}]",
                EnumProcessType.Delete, deleted ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response });
        }

        // Hàm check passport
        private bool CheckValidatePassport(string value)
        {

            string passportRegex = "^[a-z][0-9]{7}$";

            if (!Regex.IsMatch(value, passportRegex))
            {
                return false;
            }
            return true;
        }

        // Hàm check cccd
        private bool checkCitizentIdentity(string value)
        {
            // Kiểm tra xem input có đúng 12 ký tự không
            if (value.Length != 12)
            {
                return false;
            }

            // Kiểm tra xem input có chứa chỉ số không
            if (!Regex.IsMatch(value, @"^\d+$"))
            {
                return false;
            }
            return true;
        }
    }
}