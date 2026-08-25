using System;
using System.Globalization;
using System.Web.Mvc;
using Cores.Base.Apps;
using Cores.Base.Helpers;
using Cores.Major.Caches;
using Cores.Major.Enums;
using Cores.Major.Models;
using Cores.Sys.Caches.Sys;
using Modules.Major.Areas.Major.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TSFramework.App.Attributes;
using TSFramework.App.Enums;
using TSFramework.App.Processors;
using TSFramework.Core.Enums;
using TSFramework.Core.Helpers;

namespace Modules.Major.Areas.Major.Controllers
{
    public class ContractPaymentController : AppController
    {
        private readonly FEContractCache _fEContractCache = new FEContractCache();
        private readonly MajorCustomerCache _cusCache = new MajorCustomerCache();
        private readonly SysConfigCache _configCache = new SysConfigCache();
        private readonly MajorContractPaymentCache _contractPaymentCache = new MajorContractPaymentCache();
        private readonly MajorContractCache _contractCache = new MajorContractCache();

        private readonly string _contractTitle = AppProcessor.Messagor.GetMessage("Contract_Title");
        private readonly string _contractPaymentTitle = AppProcessor.Messagor.GetMessage("ContractPayment_Title");

        private const string CONFIG_KEY_RECEIPT_INCOME_TEMPLATE_PATH = "CONFIG_KEY_RECEIPT_INCOME_TEMPLATE_PATH";
        private const string CONFIG_KEY_RECEIPT_OUTCOME_TEMPLATE_PATH = "CONFIG_KEY_RECEIPT_OUTCOME_TEMPLATE_PATH";

        private readonly string _receiptIncomeTemplateFolderPath = "/Contents/File/Template/MauBienNhanThuTien.docx";
        private readonly string _receiptOutcomeTemplateFolderPath = "/Contents/File/Template/MauBienNhanChiTien.docx";

        public ContractPaymentController()
        {
            var configModel = _configCache.GetViaKey(CONFIG_KEY_RECEIPT_INCOME_TEMPLATE_PATH);
            if (configModel != null && !string.IsNullOrEmpty(configModel.ConfigValue))
            {
                _receiptIncomeTemplateFolderPath = configModel.ConfigValue;
            }
            configModel = _configCache.GetViaKey(CONFIG_KEY_RECEIPT_OUTCOME_TEMPLATE_PATH);
            if (configModel != null && !string.IsNullOrEmpty(configModel.ConfigValue))
            {
                _receiptOutcomeTemplateFolderPath = configModel.ConfigValue;
            }
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult Payments(Guid? contractId)
        {
            var contractModel = _contractCache.GetById(contractId);
            if (contractModel == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_contractTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            return PartialView("Index", contractModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult GetPayments(Guid? contractId)
        {
            var draw = Request.Form.GetValues("draw")?[0];

            var dataPayments = _contractPaymentCache.GetPayments(contractId);
            var total = dataPayments.Count;

            var result = Json(
                new { draw = Convert.ToInt32(draw), recordsTotal = total, recordsFiltered = total, data = dataPayments },
                JsonRequestBehavior.AllowGet);
            return result;
        }

        [AjaxOnly]
        [ActionType(Type = EnumActionType.Add)]
        [HttpGet]
        public ActionResult AddPayment(Guid? contractId, int status)
        {
            var contractModel = _contractCache.GetById(contractId);
            if (contractModel == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_contractTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            MajorContractPaymentModel paymentModel = new MajorContractPaymentModel
            {
                PaymentId = Guid.NewGuid(),
                ContractId = contractId,
                ContractNo = contractModel.ContractNo,
                Total = contractModel.Total,
                RemainingAmount = contractModel.RemainingAmount,
                PaidAmount = contractModel.RemainingAmount,
                FormatterPaidAmount = $"{contractModel.RemainingAmount}",
                Status = status,
            };

            return PartialView("_Add", paymentModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Add)]
        [ValidateInput(false)]
        public ActionResult AddPayment(MajorContractPaymentModel model)
        {
            if (!ModelState.IsValid)
            {
                if (model.Status == (int)EnumPaymentStatus.Received)
                {
                    return PartialView("_PaymentReceived", model);
                }

                return PartialView("_PaymentRefunded", model);
            }
            model.TypePaymentName = model.TypePayment == 1 ? AppProcessor.Messagor.GetMessage("TypePayment_PayOff") : AppProcessor.Messagor.GetMessage("TypePayment_Advance");

            var paymentId = _contractPaymentCache.SavePayment(new MajorContractPaymentModel
            {
                PaymentId = model.PaymentId,
                ContractId = model.ContractId,
                PaidAmount = model.PaidAmount,
                RefDocNo = model.RefDocNo,
                PaymentInfo = $"{model.TypePaymentName} {model.PaidAmount.ToString("#,###", CultureInfo.GetCultureInfo("vi-VN"))} {_contractTitle} [{model.ContractNo}]",
                TypePayment = model.TypePayment,
                TypePaymentName = model.TypePaymentName,
                PercentAdvance = model.PercentAdvance,
                PaidOn = model.PaidOn,
                PaymentMethod = model.PaymentMethod,
                PaymentMethodName = model.PaymentMethodName,
                Reason = "Thêm mới",
                Status = model.Status,
                StatusName = model.Status == (int)EnumPaymentStatus.Received ? AppProcessor.Messagor.GetMessage("PaymentStatus_Received") : AppProcessor.Messagor.GetMessage("PaymentStatus_Refunded"),
                Note = model.Note,
                UpdatedBy = User.UserName
            });

            var response = CreateMessage($"{_contractPaymentTitle} [{_contractTitle} {model.ContractNo}]", EnumProcessType.Add, paymentId == 0 ? EnumMsgIcon.Error : EnumMsgIcon.Success);
            return Json(new { status = true, message = response }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult EditPayment(Guid? paymentId, Guid? contractId)
        {
            var contractModel = _contractCache.GetById(contractId);
            if (contractModel == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_contractTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var paymentModel = _contractPaymentCache.GetPaymentById(paymentId);
            if (paymentModel == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_contractPaymentTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            paymentModel.ContractNo = contractModel.ContractNo;
            paymentModel.IsEdit = true;

            return PartialView("_Edit", paymentModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionType(Type = EnumActionType.Edit)]
        [ValidateInput(false)]
        public ActionResult EditPayment(MajorContractPaymentModel model)
        {
            if (!ModelState.IsValid)
            {
                if (model.Status == (int)EnumPaymentStatus.Received)
                {
                    return PartialView("_PaymentReceived", model);
                }

                return PartialView("_PaymentRefunded", model);
            }
            string response;
            model.TypePaymentName = model.TypePayment == 1 ? AppProcessor.Messagor.GetMessage("TypePayment_PayOff") : AppProcessor.Messagor.GetMessage("TypePayment_Advance");
            var retSave = _contractPaymentCache.SavePayment(new MajorContractPaymentModel
            {
                PaymentId = model.PaymentId,
                ContractId = model.ContractId,
                PaidAmount = model.PaidAmount,
                RefDocNo = model.RefDocNo,
                PaymentInfo = $"{model.TypePaymentName} {model.PaidAmount.ToString("#,###", CultureInfo.GetCultureInfo("vi-VN"))} {_contractTitle} [{model.ContractNo}]",
                TypePayment = model.TypePayment,
                TypePaymentName = model.TypePaymentName,
                PercentAdvance = model.PercentAdvance,
                PaidOn = model.PaidOn,
                PaymentMethod = model.PaymentMethod,
                PaymentMethodName = model.PaymentMethodName,
                Note = model.Note,
                Status = model.Status,
                StatusName = model.Status == (int)EnumPaymentStatus.Received ? AppProcessor.Messagor.GetMessage("PaymentStatus_Received") : AppProcessor.Messagor.GetMessage("PaymentStatus_Refunded"),
                Reason = model.Reason,
                UpdatedBy = User.UserName
            });

            if (retSave == 0)
                response = CreateMessage($"{_contractPaymentTitle} [{_contractTitle} {model.ContractNo}]",
                    EnumProcessType.Edit, EnumMsgIcon.Error);
            else if (retSave == -9)
                response = CreateMessage($"{_contractPaymentTitle} [{_contractTitle} {model.ContractNo}]",
                    EnumProcessType.DataExisted, EnumMsgIcon.Error);
            else
                response = CreateMessage($"{_contractPaymentTitle} [{_contractTitle} {model.ContractNo}]",
                    EnumProcessType.Edit, EnumMsgIcon.Success);
            return Json(new { status = true, message = response }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult DeletePayment(Guid? paymentId)
        {
            var paymentModel = _contractPaymentCache.GetPaymentById(paymentId);
            if (paymentModel == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_contractPaymentTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Delete"),
                $"<b>{_contractPaymentTitle} [{_contractTitle} {paymentModel.ContractNo}]</b>");
            return PartialView("_Delete", paymentModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult DeletePayment(MajorContractPaymentModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Delete"),
                    $"<b>{_contractPaymentTitle} [{_contractTitle} {model.ContractNo}]</b>");
                return PartialView("_DeletePaymentBody", model);
            }

            model.UpdatedBy = User.UserName;
            var retDelete = _contractPaymentCache.DeletePayment(model);
            var response = CreateMessage($"<b>{_contractPaymentTitle} [{_contractTitle} {model.ContractNo}]</b>",
                EnumProcessType.Delete, retDelete > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response });
        }


        #region in biên nhận

        /// <summary>
        /// Hiển thị thông tin biên nhận
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult ShowRender(Guid? id)
        {
            var model = new MajorContractPaymentModel
            {
                PaymentId = id
            };

            return PartialView("_PreviewFormPayment", model);
        }

        /// <summary>
        /// Render biên nhận
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult RenderReceiptOfPayment(Guid? id)
        {
            var paymentModel = _contractPaymentCache.GetPaymentById(id);

            if (paymentModel == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_contractPaymentTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var contractModel = _contractCache.GetById(paymentModel.ContractId);
            //var custommer = _cusCache.GetById(contractModel.CusId);
            var keyQHNS = _configCache.GetViaKey("CONFIG_KEY_QHNS");
            var keyUnionName = _configCache.GetViaKey("CONFIG_KEY_UNIONNAME");
            var paytext = NumberHelper.NumberToString((paymentModel.PaidAmount < 0 ? Math.Abs(paymentModel.PaidAmount) : paymentModel.PaidAmount).ToString());
            var model = new PayRecieptModel
            {
                CusName = contractModel.CusName,
                Address = contractModel.Address,
                UnionName = keyUnionName.ConfigValue,
                ContractNo = contractModel.ContractNo,
                ContractAsign = contractModel.ContractSignal,
                Day = paymentModel.CreatedOn.Day.ToString(),
                Month = paymentModel.CreatedOn.Month.ToString(),
                Year = paymentModel.CreatedOn.Year.ToString(),
                MaQHNS = keyQHNS.ConfigValue,
                BillNo = paymentModel.RefDocNo,
                PayText = paytext,
                PayNumber = $"{(paymentModel.PaidAmount < 0 ? Math.Abs(paymentModel.PaidAmount) : paymentModel.PaidAmount):N0} VND".Replace(",", "."),
                TypePay = paymentModel.TypePayment == 1 ? AppProcessor.Messagor.GetMessage("PaymentStatus_Advance") : AppProcessor.Messagor.GetMessage("PaymentStatus_PayAll"),
                QRCode = Url.Action("QRContract", "Home", new { area = "", enContractId = SecurityHelper.EncryptId(paymentModel.ContractId ?? Guid.Empty) }, Request.Url.Scheme),
                PaymentMethod = paymentModel.PaymentMethodName,
            };
            var templatePath = paymentModel.Status == 1 ? Server.MapPath(_receiptIncomeTemplateFolderPath) : Server.MapPath(_receiptOutcomeTemplateFolderPath); // Đường dẫn của tệp mẫu Word

            // Gọi hàm RenderModelToWordAndSave để tạo và lưu tệp Word vào một mảng byte
            byte[] fileBytes = RenderWordHelper.RenderModelToPdfAndSave(model, templatePath);

            // Trả về file byte như là một file để tải xuống
            return File(fileBytes, ConstMIMEType.OfficeMIMETypes[".pdf"], $"{contractModel.ContractNoInfo}.pdf");
        }

        #endregion

        #region In biên bản nghiệm thu

        /// <summary>
        /// Hiển thị thông tin biên bản nghiệm thu
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult ShowAcceptantRecord(Guid? contractId)
        {
            var model = new MajorContractPaymentModel
            {
                ContractId = contractId
            };
            return PartialView("_PreviewAcceptantRecord", model);
        }

        /// <summary>
        /// Render biên bản nghiêm thu
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult RenderAcceptantRecord(Guid? contractId)
        {
            var datapaymentAdvance = _contractPaymentCache.CheckTypePayment(contractId, 1);
            var contractModel = _contractCache.GetById(contractId);
            var keyQHNS = _configCache.GetViaKey("CONFIG_KEY_QHNS");
            var keyUnionName = _configCache.GetViaKey("CONFIG_KEY_UNIONNAME");
            //var paytext = NumberHelper.NumberToString(paymentModel.PaidAmount.ToString());
            var dataInfoA = _fEContractCache.GetDataRenderContract(contractId);
            JObject obj = JObject.Parse(dataInfoA.JsonContractInfo);

            // Convert JObject thành đối tượng PayAcceptantRecordModel
            PayAcceptantRecordModel model = JsonConvert.DeserializeObject<PayAcceptantRecordModel>(obj.ToString());
            model.UnionName = keyUnionName.ConfigValue;
            model.ContractNo = contractModel.ContractNo + "/BBNT";
            model.Day = DateTime.Now.Day.ToString();
            model.Month = DateTime.Now.Month.ToString();
            model.Year = DateTime.Now.Year.ToString();
            model.MaQHNS = keyQHNS.ConfigValue;
            model.ContractNoInfo = contractModel.ContractNoInfo;
            model.TotalPayment = model.TotalPayment.Replace(",", ".") + " đồng";
            model.TotalPaymentDone = model.TotalPayment.Replace(",", ".");
            model.PaymentAdvance = $"{datapaymentAdvance.PaidAmount:N0} đồng".Replace(",", ".");
            model.PayNumber = $"{datapaymentAdvance.PaidAmount:N0} đồng".Replace(",", ".");
            model.PayText = NumberHelper.NumberToString(datapaymentAdvance.PaidAmount.ToString());

            var templatePath = Server.MapPath("~/Contents/File/Template/MauNghiemThu.docx"); // Đường dẫn của tệp mẫu Word

            // Gọi hàm RenderModelToWordAndSave để tạo và lưu tệp Word vào một mảng byte
            byte[] fileBytes = RenderWordHelper.RenderModelToPdfAndSave(model, templatePath);

            // Trả về file byte như là một file để tải xuống
            return File(fileBytes, ConstMIMEType.OfficeMIMETypes[".pdf"], "result.pdf");
        }
        #endregion
    }
}