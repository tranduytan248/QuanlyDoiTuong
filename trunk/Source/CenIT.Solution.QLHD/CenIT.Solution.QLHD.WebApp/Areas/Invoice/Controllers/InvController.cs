using Core.Inv.Caches;
using Core.Inv.Providers;
using Cores.Sys.Caches.Sys;
using System;
using System.Web.Mvc;
using Core.Inv.Enums;
using TSFramework.App.Attributes;
using TSFramework.App.Processors;
using TSFramework.Core.Enums;
using Core.Inv.Models;
using EnumHelper = TSFramework.Core.Helpers.EnumHelper;
using Modules.Major.Areas.Major.Models;
using TSFramework.App.Models;
using System.Linq;
using System.Web.UI.WebControls;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Core.Inv.Models.Invs;
using Core.Inv.Helpers;
using Cores.eContract.Consts;
using FastMember;
using System.Data;
using System.Threading.Tasks;
using Cores.Base.Apps;
using Cores.Cate.Caches;
using TSFramework.Core.Utils;

namespace Modules.Major.Areas.Invoice.Controllers
{
    public class InvController : AppController
    {
        #region Inits

        private readonly SysConfigCache _sysConfigCache = new SysConfigCache();
        private readonly SysElnvAccountCache _invAccCache = new SysElnvAccountCache();

        private readonly CateUnionCache _unionCache = new CateUnionCache();

        private readonly MajorInvPatternCache _invPatternCache = new MajorInvPatternCache();
        private readonly MajorInvCache _invCache = new MajorInvCache();
        private readonly MajorInvCusCache _invCusCache = new MajorInvCusCache();
        private readonly MajorInvProductCache _invProductCache = new MajorInvProductCache();

        private readonly string _invTitle = AppProcessor.Messagor.GetMessage("Invoice_Title");

        private const string CONFIG_INV_HOST_INV_SERVICE = "CONFIG_INV_HOST_INV_SERVICE";
        private const string CONFIG_INV_SERVICE_ACCOUNT_NAME = "CONFIG_INV_SERVICE_ACCOUNT_NAME";
        private const string CONFIG_INV_SERVICE_ACCOUNT_PASS = "CONFIG_INV_SERVICE_ACCOUNT_PASS";

        private const string CONFIG_INV_OLD_HOST_INV_SERVICE = "CONFIG_INV_OLD_HOST_INV_SERVICE";
        private const string CONFIG_INV_OLD_SERVICE_ACCOUNT_NAME = "CONFIG_INV_OLD_SERVICE_ACCOUNT_NAME";
        private const string CONFIG_INV_OLD_SERVICE_ACCOUNT_PASS = "CONFIG_INV_OLD_SERVICE_ACCOUNT_PASS";

        private const string CONFIG_INV_DEFAULT_TAX_RATE = "CONFIG_INV_DEFAULT_TAX_RATE";

        private readonly InvProvider _invProvider;
        private readonly string _invServiceAccName = "";
        private readonly string _invServiceAccPass = "";

        private readonly int _defaultInvTaxRate;

        private static string[] _arrPermissionViaUser;

        #endregion

        public InvController()
        {
            #region Config Inv

            var configModel = _sysConfigCache.GetViaKey(CONFIG_INV_DEFAULT_TAX_RATE);
            if (configModel != null && !string.IsNullOrEmpty(configModel.ConfigValue))
            {
                _defaultInvTaxRate = int.Parse(configModel.ConfigValue ?? "10");
            }

            string hostInvService = "";
            configModel = _sysConfigCache.GetViaKey(CONFIG_INV_HOST_INV_SERVICE);
            if (configModel != null && !string.IsNullOrEmpty(configModel.ConfigValue))
            {
                hostInvService = configModel.ConfigValue;
            }

            configModel = _sysConfigCache.GetViaKey(CONFIG_INV_SERVICE_ACCOUNT_NAME);
            if (configModel != null && !string.IsNullOrEmpty(configModel.ConfigValue))
            {
                _invServiceAccName = configModel.ConfigValue;
            }

            configModel = _sysConfigCache.GetViaKey(CONFIG_INV_SERVICE_ACCOUNT_PASS);
            if (configModel != null && !string.IsNullOrEmpty(configModel.ConfigValue))
            {
                _invServiceAccPass = configModel.ConfigValue;
            }

            _invProvider = new InvProvider($"{hostInvService}PortalService.asmx", $"{hostInvService}BusinessService.asmx", $"{hostInvService}PublishService.asmx");

            #endregion
        }

        #region Main Actions

        // GET: Invoice/Inv
        public ActionResult Index()
        {
            _arrPermissionViaUser = GetPermissionViaUser(User.UserName);

            var lstUnionsManagerByUser = _unionCache.GetUnionsViaManager(User.UserName);

            var eInvAcc = _invAccCache.GetByUserName(User.UserName);

            var model = new SearchInvoiceModel
            {
                ListUnions = lstUnionsManagerByUser.Select(u => new ListItem(text: u.UnionName, value: $"{u.UnionId}")).ToList(),
                Permissions = _arrPermissionViaUser,

                ListInvPatterns = _invPatternCache.GetAll().OrderBy(d => d.Pattern)
                    .Where(d => d.IsActive)
                    .Select(d => new ListItem(d.Pattern, d.Pattern.ToString())).Distinct().ToList(),
                ListUsers = _invAccCache.GetAll().Select(d =>
                {
                    var liUser = new ListItem
                    {
                        Text = $"{d.FullName} - {d.EmpAccount}",
                        Value = d.EmpAccount.ToString()
                    };
                    liUser.Attributes.Add("data-content", $"{d.FullName} - <span class='badge badge-success'>{d.EmpAccount}</span>");

                    return liUser;
                }).ToList(),
                HasNotSysInvAccount = eInvAcc == null,
                IsInvServiceAccountIncorrect = !_invProvider.IsCorrectUser(_invServiceAccName, _invServiceAccPass)
            };

            return View(model);
        }

        #region Get

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult Get(SearchInvoiceModel searchModel)
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

            var data = _invCache.Get(total: out int total, userName: User.UserName, managerUnions: searchModel.UnionIds, invNo: searchModel.InvNo, pattern: searchModel.Pattern, serials: searchModel.Serials, invStatus: searchModel.InvStatus, invTypes: searchModel.InvTypes, createdFrom: searchModel.CreatedFrom, createdTo: searchModel.CreatedTo, creators: searchModel.Creators, cusName: searchModel.CusName, cusCode: searchModel.CusCode, cusTaxCode: searchModel.CusTaxCode, search: dataSearch);

            var result = Json(
                new { draw = Convert.ToInt32(draw), recordsTotal = total, recordsFiltered = total, data },
                JsonRequestBehavior.AllowGet);
            return result;
        }

        #endregion

        #region Detail

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Detail(Guid? id)
        {
            var invModel = _invCache.GetById(id);
            if (invModel == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_invTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var invViewModel = _invCache.GetView(invModel.InvId);
            Regex rRemScript = new Regex(@"<script[^>]*>[\s\S]*?</script>");
            if (invViewModel == null || string.IsNullOrEmpty(invViewModel.InvView) || invViewModel.InvView.Contains("ERR:"))
            {
                var htmlInv = _invProvider.GetInvViewNoPay(invModel.InvKey, _invServiceAccName, _invServiceAccPass);
                if (!string.IsNullOrEmpty(htmlInv))
                {
                    htmlInv = rRemScript.Replace(htmlInv, "");
                }
                invViewModel = new MarjorViewInvModel
                {
                    InvId = invModel.InvId,
                    InvView = htmlInv
                };
                _invCache.SaveView(invViewModel);
            }
            else
            {
                invViewModel.InvView = rRemScript.Replace(invViewModel.InvView, "");
            }

            if (!invViewModel.InvView.IsHTML())
            {
                return Json(new { status = true, message = CreateMessage("Dữ liệu Hoá đơn điện tử không tồn tại hoặc chưa được thuế chấp nhận", EnumProcessType.NonFormat, EnumMsgIcon.Error) }, JsonRequestBehavior.AllowGet);
            }

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(invViewModel.InvView);
            invViewModel.InvView = htmlDoc.DocumentNode.OuterHtml;

            var viewInv = new ViewInvModel
            {
                InvNo = invModel.InvNo,
                Pattern = invModel.Pattern,
                Serial = invModel.Serial,
                InvKey = invModel.InvKey,
                HtmlInv = invViewModel.InvView
            };

            return PartialView("_Detail", viewInv);
        }

        #endregion

        #region Cancel

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult Cancel(Guid? id)
        {
            var invModel = _invCache.GetById(id);
            if (invModel == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_invTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Cancel"),
                $"<b>{_invTitle} [{invModel.InvNo}-{invModel.Pattern}/{invModel.Serial}]</b>");

            return PartialView("_Cancel", invModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult Cancel(MajorInvModel invModel)
        {
            if (string.IsNullOrEmpty(invModel.Reason))
            {
                ModelState.AddModelError("Reason", "Dữ liệu lý do bắt buộc nhập");
                return PartialView("_CancelBody", invModel);
            }

            invModel.UpdatedBy = User.UserName;

            string response;

            #region Call Service Cancel Inv

            var userInfo = _invAccCache.GetByUserName(User.UserName);
            string sInvAccName = userInfo.ElnvAccount;
            string sInvAccPass = userInfo.ElnvACPassword;

            var retServiceCancel = _invProvider.CancelInv(invModel.InvKey, sInvAccName, sInvAccPass, _invServiceAccName,
                _invServiceAccPass, User.UserName, invModel.Reason);

            if (!retServiceCancel)
            {
                response = CreateMessage($"{AppProcessor.Messagor.GetMessage("CancelInv_Title")} [{invModel.InvNo}-{invModel.Pattern}/{invModel.Serial}]", EnumProcessType.NonFormat, EnumMsgIcon.Error);
                return Json(new { status = false, message = response });
            }

            #endregion

            invModel.InvStatus = (int)EnumInvStatus.InvoiceAreCancled;
            invModel.InvStatusName = EnumHelper.GetDescription(EnumInvStatus.InvoiceAreCancled);

            var ret = _invCache.Cancel(invModel);
            if (ret == -19)
            {
                response = CreateMessage($"{AppProcessor.Messagor.GetMessage("CancelInv_Title")} {_invTitle} [{invModel.InvNo} - {invModel.Pattern}-{invModel.Serial}]", EnumProcessType.NonFormat, EnumMsgIcon.Error);
                return Json(new { status = true, message = response });
            }

            response = CreateMessage($"{AppProcessor.Messagor.GetMessage("CancelInv_Title")} {_invTitle} [{invModel.InvNo} - {invModel.Pattern}-{invModel.Serial}]", EnumProcessType.NonFormat, ret > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response });
        }

        #endregion

        #region Adjust

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult Adjust(Guid? id)
        {
            var invModel = _invCache.GetById(id);
            if (invModel == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_invTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            invModel.InvCusInfo = _invCusCache.GetById(id);
            invModel.InvProductInfo = _invProductCache.GetById(id);
            return PartialView("_Adjust", invModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult Adjust(MajorInvModel invModel)
        {
            if (!ModelState.IsValid)
            {
                invModel.InvCusInfo = _invCusCache.GetById(invModel.InvId);
                invModel.InvProductInfo = _invProductCache.GetById(invModel.InvId);
                return PartialView("_AdjustBody", invModel);
            }
            var invKey = invModel.InvKey;
            int type = 4;
            invModel.Total = (long)invModel.InvProductInfo.Amount;
            invModel.TaxAmount = (long)(invModel.Total * 0.2 * 0.05);
            invModel.KindOfService = DateTime.Now.ToString("dd/MM/YYYY");
            invModel.InvKey = InvHelper.GenFKey();
            invModel.ContractId = invModel.ContractId;
            invModel.InvStatus = (int)EnumInvStatus.InvoiceAreAdjusted;
            invModel.InvStatusName = EnumHelper.GetDescription(EnumInvStatus.InvoiceAreAdjusted);
            var newInvStatus = (int)EnumInvStatus.InvoiceAdjustment;
            var newInvStatusName = EnumHelper.GetDescription(EnumInvStatus.InvoiceAdjustment);
            invModel.InvType = (int)EnumInvType.InvoiceAdjustInfo;
            invModel.InvTypeName = EnumHelper.GetDescription(EnumInvType.InvoiceAdjustInfo);
            var lstInvPatterns = _invPatternCache.GetAll();
            var usingInvPattern = lstInvPatterns.First(p => p.IsActive);
            var userInfo = _invAccCache.GetByUserName(User.UserName);
            string sInvAccName = userInfo.ElnvAccount;
            string sInvAccPass = userInfo.ElnvACPassword;
            var lstProducts = new InvProducts
            {
                ListProducts = new List<InvProduct>
                        {
                            new InvProduct
                            {
                                ProdId = Guid.NewGuid(),
                                ProdName = invModel.InvProductInfo.ProductName,
                                ProdPrice = string.Empty,
                                ProdUnit = " ",
                                Price = invModel.Total,
                                Amount =  $"{invModel.InvProductInfo.Amount}",
                                Total = $"{invModel.InvProductInfo.Amount}",
                                ProdQuantity = string.Empty,
                                TaxAmount = $"{invModel.TaxAmount}",
                                IsSum = $"{(int)EnumInvProductType.Product}",
                                TaxRate = $"{_defaultInvTaxRate}"
                            }
                        }
            };

            //Tạo sXmlInvData
            var eInvAdjustc = new InvAdjustInv
            {
                FKey = invModel.InvKey,
                CusCode = invModel.InvCusInfo.CusCode,
                CusName = invModel.InvCusInfo.TypeCus == ConstsCusType.BUSINESS ? invModel.InvCusInfo.CusName : "",
                CusAddress = invModel.InvCusInfo.CusAddress,
                CusPhone = invModel.InvCusInfo.CusPhone,
                CusTaxCode = invModel.InvCusInfo.CusTaxCode,
                CusBankNo = invModel.InvCusInfo.CusBankNo,
                PaymentMethod = invModel.PaymentMethod,
                KindOfService = invModel.KindOfService,
                Type = type,

                Products = lstProducts,

                Total = invModel.Total.ToString(),
                TaxRate = invModel.TaxRate.ToString(),
                TaxAmount = invModel.TaxAmount.ToString(),
                Amount = invModel.Amount.ToString(),
                AmountInWords = invModel.AmountInWord,
                Buyer = invModel.InvCusInfo.TypeCus == ConstsCusType.BUSINESS ? "" : invModel.InvCusInfo.CusName,
            };

            var resuilt = _invProvider.AdjustInvoice(eInvAdjustc, type, sInvAccName, sInvAccPass, _invServiceAccName,
                 _invServiceAccPass, invKey, usingInvPattern.Pattern, usingInvPattern.Serial, User.UserName, invModel.Reason);

            string response;
            if (resuilt.Contains("ERR"))
            {
                response = CreateMessage($"{AppProcessor.Messagor.GetMessage("AdjustcInvoice_Title")} {_invTitle} [{invModel.InvNo} - {invModel.Pattern}-{invModel.Serial}]-[{resuilt}]", EnumProcessType.NonFormat, EnumMsgIcon.Error);
                return Json(new { status = true, message = response });
            }

            var arrParrams = resuilt.Split('_');
            var invNo = int.Parse(arrParrams[1]);

            MajorInvCusModel modelInvCus = new MajorInvCusModel
            {
                InvId = invModel.InvId,
                CusCode = invModel.InvCusInfo.CusCode,
                CusName = invModel.InvCusInfo.CusName,
                Buyer = invModel.InvCusInfo.TypeCus == ConstsCusType.BUSINESS ? "" : invModel.InvCusInfo.CusName,
                TypeCus = invModel.InvCusInfo.TypeCus,
                TypeCusName = invModel.InvCusInfo.TypeCus == ConstsCusType.BUSINESS ? AppProcessor.Messagor.GetMessage("CusType_Business") : AppProcessor.Messagor.GetMessage("CusType_Consumer"),
                CusTaxCode = invModel.InvCusInfo.CusTaxCode,
                CusPhone = invModel.InvCusInfo.CusPhone,
                CusAddress = invModel.InvCusInfo.CusAddress,
                CusBankNo = invModel.InvCusInfo.CusBankNo,
                CusBankName = invModel.InvCusInfo.CusBankName,
            };

            var dataCusInfo = new DataTable();
            using (var reader = ObjectReader.Create(new List<MajorInvCusModel> { modelInvCus }, "CusCode", "CusName", "Buyer", "TypeCus", "TypeCusName", "CusTaxCode", "CusPhone", "CusAddress", "CusBankNo", "CusBankName"))
            {
                dataCusInfo.Load(reader);
            }
            invModel.DataInvCus = dataCusInfo;
            invModel.UpdatedBy = User.UserName;
            _invCache.AdjustInvoice(invModel, invKey, newInvStatus, newInvStatusName, invNo.ToString());

            response = CreateMessage($"{AppProcessor.Messagor.GetMessage("AdjustcInvoice_Title")} {_invTitle} [{invModel.InvNo} - {invModel.Pattern}-{invModel.Serial}]", EnumProcessType.NonFormat, EnumMsgIcon.Success);
            return Json(new { status = true, message = response });
        }

        #endregion

        #endregion

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.View)]
        public JsonResult GetSerialByPattern(string pattern)
        {
            var lstSerial = _invPatternCache.GetByPattern(pattern);
            var serials = new Dictionary<string, List<MajorInvPatternModel>>();

            lstSerial.GroupBy(d => d.Serial).ToList().ForEach(g => { serials.Add(g.Key, g.ToList()); });

            return Json(new { Serials = serials }, JsonRequestBehavior.AllowGet);
        }

        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        public Task<FileResult> DownloadInv(string invKey, string invNo)
        {
            var invInfo = _invCache.GetByKey(invKey);
            if (invInfo.IsOldVersion)
            {
                var oldInvProvider = new InvProvider(
                    $"{_sysConfigCache.GetViaKey(CONFIG_INV_OLD_HOST_INV_SERVICE)?.ConfigValue}PortalService.asmx",
                    $"{_sysConfigCache.GetViaKey(CONFIG_INV_OLD_HOST_INV_SERVICE)?.ConfigValue}BusinessService.asmx",
                    $"{_sysConfigCache.GetViaKey(CONFIG_INV_OLD_HOST_INV_SERVICE)?.ConfigValue}PublishService.asmx");
                string oldInvServiceAccName = _sysConfigCache.GetViaKey(CONFIG_INV_OLD_SERVICE_ACCOUNT_NAME)?.ConfigValue;
                string oldInvServiceAccPass = _sysConfigCache.GetViaKey(CONFIG_INV_OLD_SERVICE_ACCOUNT_PASS)?.ConfigValue;
                string oldHtmlInv = oldInvProvider.DownloadPDF(invKey, oldInvServiceAccName, oldInvServiceAccPass);
                // Convert base64 string back to bytes
                var oldPdfBytes = Convert.FromBase64String(oldHtmlInv);
                // Return the PDF file
                return Task.FromResult<FileResult>(File(oldPdfBytes, "application/pdf", invNo + ".pdf"));
            }
            string htmlInv = _invProvider.DownloadPDF(invKey, _invServiceAccName, _invServiceAccPass);
            // Convert base64 string back to bytes
            var pdfBytes = Convert.FromBase64String(htmlInv);

            // Return the PDF file
            return Task.FromResult<FileResult>(File(pdfBytes, "application/pdf", invNo + ".pdf"));
        }

    }
}