using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using Core.Inv.Caches;
using Core.Inv.Enums;
using Core.Inv.Helpers;
using Core.Inv.Models;
using Core.Inv.Models.Invs;
using Core.Inv.Providers;
using Cores.Base.Apps;
using Cores.Base.Enums;
using Cores.Base.Helpers;
using Cores.Base.Interfaces;
using Cores.Base.Models;
using Cores.Cate.Caches;
using Cores.Cate.Enum;
using Cores.Cate.Models;
using Cores.eContract.Consts;
using Cores.Major.Caches;
using Cores.Major.Enums;
using Cores.Major.Models;
using Cores.Major.Providers;
using Cores.Sys.Caches.Cate;
using Cores.Sys.Caches.Sys;
using Cores.Sys.Models.Cate;
using Cores.Sys.Models.Sys;
using Cores.VNPT.SmsMarketing.Consts;
using Cores.VNPT.SmsMarketing.Providers;
using FastMember;
using HtmlAgilityPack;
using Modules.Major.Areas.Major.Models;
using Modules.Major.Providers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TSFramework.App.Attributes;
using TSFramework.App.Enums;
using TSFramework.App.Models;
using TSFramework.App.Processors;
using TSFramework.Core.Enums;
using TSFramework.Core.Helpers;
using TSFramework.Core.Utils;
using Image = System.Drawing.Image;
using XmlHelper = TSFramework.Core.Helpers.XmlHelper;

namespace Modules.Major.Areas.Major.Controllers
{
    public class ContractController : AppController
    {
        #region Inits

        #region Caches

        private readonly SysConfigCache _sysConfigCache = new SysConfigCache();
        private readonly SysElnvAccountCache _invAccCache = new SysElnvAccountCache();
        private readonly SysUserCache _sysUserCache = new SysUserCache();

        private readonly MajorContractCache _contractCache = new MajorContractCache();
        private readonly MajorProcedureCache _procedureCache = new MajorProcedureCache();
        private readonly MajorProcedureStepCache _stepCache = new MajorProcedureStepCache();
        private readonly MajorCustomerCache _customerCache = new MajorCustomerCache();
        private readonly MajorInvPatternCache _invPatternCache = new MajorInvPatternCache();
        private readonly MajorInvCache _invCache = new MajorInvCache();

        private readonly CateDocCache _docCache = new CateDocCache();
        private readonly CateUnionCache _unionCache = new CateUnionCache();
        private readonly CateProvinceCache _provinceCache = new CateProvinceCache();
        private readonly CatePurPoseCache _purposeCache = new CatePurPoseCache();
        private readonly CateMainSectionCache _mainSectionCache = new CateMainSectionCache();
        private readonly CateContentLandCache _contentLandCache = new CateContentLandCache();
        private readonly CateSubSectionCache _subSectionCache = new CateSubSectionCache();
        private readonly CateContractTypeCache _contractTypeCache = new CateContractTypeCache();
        private readonly CateHolidayCache _holidayCache = new CateHolidayCache();

        private readonly FEContractCache _fEContractCache = new FEContractCache();

        #endregion

        private readonly string _contractTitle = AppProcessor.Messagor.GetMessage("Contract_Title");
        private readonly string _contractTaskTitle = AppProcessor.Messagor.GetMessage("ContractTask_Title");
        private readonly string _contractCusTitle = AppProcessor.Messagor.GetMessage("ContractCus_Title");
        private readonly string _contractPaymentTitle = AppProcessor.Messagor.GetMessage("ContractPayment_Title");
        private readonly string _contractTypeTitle = AppProcessor.Messagor.GetMessage("ContractType_Title");
        private readonly string _procedureTitle = AppProcessor.Messagor.GetMessage("Procedure_Title");
        private readonly string _stepTitle = AppProcessor.Messagor.GetMessage("Step_Title");

        #region Config Keys

        //private const string CONFIG_KEY_CONTRACT_TYPE_MAPPING_TEMPLATE = "CONFIG_KEY_CONTRACT_TYPE_MAPPING_TEMPLATE";
        private const string CONFIG_KEY_MAPPING_TEMPLATE_CONTRACT_TASK = "CONFIG_KEY_MAPPING_TEMPLATE_CONTRACT_TASK";
        private const string CONFIG_KEY_EXTEND_CONTRACT_INFO_FOR_RENDER = "CONFIG_KEY_EXTEND_CONTRACT_INFO_FOR_RENDER";
        private const string CONFIG_KEY_FUNCTION_DISCOUNT_CONTRACT = "CONFIG_KEY_FUNCTION_DISCOUNT_CONTRACT";
        private const string CONFIG_CONTRACT_DISCOUNT_TAX_INFO = "CONFIG_CONTRACT_DISCOUNT_TAX_INFO";

        private const string CONFIG_KEY_UNION_NAME_USER = "CONFIG_KEY_UNION_NAME_USER";
        private const string CONFIG_KEY_OFFICE_APPVIEWER_URL = "CONFIG_KEY_OFFICE_APPVIEWER_URL";
        private const string CONFIG_KEY_REF_FOLDER_CONTRACT_PATH = "CONFIG_KEY_REF_FOLDER_CONTRACT_PATH";
        //private const string CONFIG_KEY_TYPE_CONTRACT_MAPPING = "CONFIG_KEY_TYPE_CONTRACT_MAPPING";
        private const string CONFIG_KEY_DEFAULT_PROVINCE_CODE = "CONFIG_KEY_DEFAULT_PROVINCE_CODE";

        //private const string KEYS_TEMPLATE = "TEMPLATE";
        //private const string KEYS_PERCENT_PAYMENT = "PERCENT_PAYMENT";
        //private const string KEYS_SIGNAL = "SIGNAL";

        private const string CONFIG_INV_HOST_INV_SERVICE = "CONFIG_INV_HOST_INV_SERVICE";
        private const string CONFIG_INV_SERVICE_ACCOUNT_NAME = "CONFIG_INV_SERVICE_ACCOUNT_NAME";
        private const string CONFIG_INV_SERVICE_ACCOUNT_PASS = "CONFIG_INV_SERVICE_ACCOUNT_PASS";
        private const string CONFIG_INV_DEFAULT_TAX_RATE = "CONFIG_INV_DEFAULT_TAX_RATE";
        private const string CONFIG_INV_DEFAULT_PRODUCT_NAME = "CONFIG_INV_DEFAULT_PRODUCT_NAME";
        private const string CONFIG_INV_DISCOUNT_FORMULA = "CONFIG_INV_DISCOUNT_FORMULA";
        private const string CONFIG_INV_RATE_FOR_CALC_TAX = "CONFIG_INV_RATE_FOR_CALC_TAX";
        private const string CONFIG_INV_PATTERN_FOR_DISCOUNT_CONTRACT = "CONFIG_INV_PATTERN_FOR_DISCOUNT_CONTRACT";

        private const string CONFIG_KEY_ACCEPTANT_TEMPLATE_PATH = "CONFIG_KEY_ACCEPTANT_TEMPLATE_PATH";
        private const string CONFIG_KEY_CONTRACT_EXPERTISE_COST_PERCENT = "CONFIG_KEY_CONTRACT_EXPERTISE_COST_PERCENT";
        private const string CONFIG_KEY_NOTIFICATION_LIBS_FOLDER_PATH = "CONFIG_KEY_NOTIFICATION_LIBS_FOLDER_PATH";
        private const string CONFIG_UNION_NOT_USING_CODE_FOR_CONTRACT = "CONFIG_UNION_NOT_USING_CODE_FOR_CONTRACT";
        private const string CONFIG_KEY_TEMPLATE_BY_PERCENT_VALUE = "CONFIG_KEY_TEMPLATE_BY_PERCENT_VALUE";
        private const string CONFIG_CUS_INFO_IN_CONTRACT = "CONFIG_CUS_INFO_IN_CONTRACT";

        private const string CONFIG_KEY_HAS_CALC_TAX_CONTRACT = "CONFIG_KEY_HAS_CALC_TAX_CONTRACT";
        private const string CONFIG_TAX_INFO_IN_CONTRACT = "CONFIG_TAX_INFO_IN_CONTRACT";

        #endregion

        #region Private Values

        private readonly InvProvider _invProvider;

        private readonly string _invServiceAccName = "";
        private readonly string _invServiceAccPass = "";

        //private readonly Dictionary<string, Dictionary<string, string>> _dictConfigsViaTypeContract;

        //private readonly Dictionary<string, string> _dictMappingContractTemplates = new Dictionary<string, string>();
        private readonly Dictionary<string, Dictionary<string, string>> _dictMappingTemplateContractTasks = new Dictionary<string, Dictionary<string, string>>();
        private readonly string _functionDiscountContract = string.Empty;
        private readonly string _infoDiscountContract = string.Empty;
        private readonly Dictionary<string, string> _dictExtendContractInfos = new Dictionary<string, string>();
        private readonly Dictionary<Guid, string> _dictMappingUnionViaUnitNames = new Dictionary<Guid, string>();
        private readonly string _refContractDocsFolderPath = "/Contents/Modules/Major/RefDocs/";
        private readonly string _contractFolderName = "Contracts";
        private readonly string _defaultProvinceCode = "";

        private readonly int _expertiseCostPercent = 25;

        private readonly int _defaultInvTaxRate = 8;
        private readonly int _defaultInvRateForCalcTax = 5;
        private readonly string _defaultInvProdName = "Thanh toán Hợp đồng dịch vụ đo vẽ số {0} ngày {1}";
        private readonly string _defaultInvDiscountFormula = "{0}*0.05*0.2";
        private readonly string _invTemplateFolderPath = "/Contents/Modules/Major/Templates/Invoice/";
        private readonly string _invPatternForDiscountContract;

        private readonly string _acceptantTemplateFolderPath = "/Contents/File/Template/MauNghiemThu.docx";

        private readonly string _notificationLibrariesPathFolder = "/Libraries/Notifications";
        private readonly List<INotify> _listNotificationProviders;

        private readonly List<Guid> _unionsNotUsingCode = new List<Guid>();

        private readonly bool _hasCalcTaxForContract;
        //private readonly string _funcTaxContract = "";
        private readonly string _taxInfoContract = "";

        #endregion

        #endregion

        public ContractController()
        {
            SysConfigModel configModel;

            //var configModel = _sysConfigCache.GetViaKey(CONFIG_KEY_CONTRACT_TYPE_MAPPING_TEMPLATE);
            //if (configModel != null && !string.IsNullOrEmpty(configModel.ConfigValue))
            //{
            //    _dictMappingContractTemplates =
            //        JsonConvert.DeserializeObject<Dictionary<string, string>>(configModel.ConfigValue);
            //}

            #region Configs

            configModel = _sysConfigCache.GetViaKey(CONFIG_KEY_MAPPING_TEMPLATE_CONTRACT_TASK);
            if (configModel != null && !string.IsNullOrEmpty(configModel.ConfigValue))
            {
                _dictMappingTemplateContractTasks =
                    JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(configModel.ConfigValue);
            }

            configModel = _sysConfigCache.GetViaKey(CONFIG_KEY_EXTEND_CONTRACT_INFO_FOR_RENDER);
            if (configModel != null && !string.IsNullOrEmpty(configModel.ConfigValue))
            {
                _dictExtendContractInfos =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(configModel.ConfigValue);
            }

            configModel = _sysConfigCache.GetViaKey(CONFIG_KEY_FUNCTION_DISCOUNT_CONTRACT);
            if (configModel != null && !string.IsNullOrEmpty(configModel.ConfigValue))
            {
                _functionDiscountContract = configModel.ConfigValue;
            }

            configModel = _sysConfigCache.GetViaKey(CONFIG_CONTRACT_DISCOUNT_TAX_INFO);
            if (configModel != null && !string.IsNullOrEmpty(configModel.ConfigValue))
            {
                _infoDiscountContract = configModel.ConfigValue;
            }

            configModel = _sysConfigCache.GetViaKey(CONFIG_KEY_UNION_NAME_USER);
            if (configModel != null && !string.IsNullOrEmpty(configModel.ConfigValue))
            {
                _dictMappingUnionViaUnitNames =
                    JsonConvert.DeserializeObject<Dictionary<Guid, string>>(configModel.ConfigValue);
            }

            configModel = _sysConfigCache.GetViaKey(CONFIG_KEY_REF_FOLDER_CONTRACT_PATH);
            if (configModel != null && !string.IsNullOrEmpty(configModel.ConfigValue))
            {
                _refContractDocsFolderPath = configModel.ConfigValue;
            }

            //configModel = _sysConfigCache.GetViaKey(CONFIG_KEY_TYPE_CONTRACT_MAPPING);
            //if (configModel != null && !string.IsNullOrEmpty(configModel.ConfigValue))
            //{
            //    _dictConfigsViaTypeContract =
            //        JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(configModel.ConfigValue);
            //}

            configModel = _sysConfigCache.GetViaKey(CONFIG_KEY_DEFAULT_PROVINCE_CODE);
            if (configModel != null && !string.IsNullOrEmpty(configModel.ConfigValue))
            {
                _defaultProvinceCode = configModel.ConfigValue;
            }

            configModel = _sysConfigCache.GetViaKey(CONFIG_KEY_ACCEPTANT_TEMPLATE_PATH);
            if (configModel != null && !string.IsNullOrEmpty(configModel.ConfigValue))
            {
                _acceptantTemplateFolderPath = configModel.ConfigValue;
            }

            configModel = _sysConfigCache.GetViaKey(CONFIG_KEY_CONTRACT_EXPERTISE_COST_PERCENT);
            if (configModel != null && !string.IsNullOrEmpty(configModel.ConfigValue))
            {
                _expertiseCostPercent = int.Parse(configModel.ConfigValue ?? "25");
            }

            configModel = _sysConfigCache.GetViaKey(CONFIG_KEY_NOTIFICATION_LIBS_FOLDER_PATH);
            if (configModel != null && !string.IsNullOrEmpty(configModel.ConfigValue))
            {
                _notificationLibrariesPathFolder = configModel.ConfigValue;
            }
            _listNotificationProviders = MajorProvider.LoadNotifications(_notificationLibrariesPathFolder);

            configModel = _sysConfigCache.GetViaKey(CONFIG_UNION_NOT_USING_CODE_FOR_CONTRACT);
            if (configModel != null && !string.IsNullOrEmpty(configModel.ConfigValue))
            {
                _unionsNotUsingCode =
                    JsonConvert.DeserializeObject<List<Guid>>(configModel.ConfigValue);
            }

            configModel = _sysConfigCache.GetViaKey(CONFIG_KEY_HAS_CALC_TAX_CONTRACT);
            if (configModel != null && !string.IsNullOrEmpty(configModel.ConfigValue))
            {
                _hasCalcTaxForContract = bool.Parse(configModel.ConfigValue ?? "false");
            }

            configModel = _sysConfigCache.GetViaKey(CONFIG_TAX_INFO_IN_CONTRACT);
            if (configModel != null && !string.IsNullOrEmpty(configModel.ConfigValue))
            {
                _taxInfoContract = configModel.ConfigValue;
            }

            #endregion

            #region Config Inv

            configModel = _sysConfigCache.GetViaKey(CONFIG_INV_DEFAULT_TAX_RATE);
            if (configModel != null && !string.IsNullOrEmpty(configModel.ConfigValue))
            {
                _defaultInvTaxRate = int.Parse(configModel.ConfigValue ?? "10");
            }

            configModel = _sysConfigCache.GetViaKey(CONFIG_INV_DEFAULT_PRODUCT_NAME);
            if (configModel != null && !string.IsNullOrEmpty(configModel.ConfigValue))
            {
                _defaultInvProdName = configModel.ConfigValue;
            }

            configModel = _sysConfigCache.GetViaKey(CONFIG_INV_DISCOUNT_FORMULA);
            if (configModel != null && !string.IsNullOrEmpty(configModel.ConfigValue))
            {
                _defaultInvDiscountFormula = configModel.ConfigValue;
            }

            configModel = _sysConfigCache.GetViaKey(CONFIG_INV_RATE_FOR_CALC_TAX);
            if (configModel != null && !string.IsNullOrEmpty(configModel.ConfigValue))
            {
                _defaultInvRateForCalcTax = int.Parse(configModel.ConfigValue ?? "5");
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

            configModel = _sysConfigCache.GetViaKey(CONFIG_INV_PATTERN_FOR_DISCOUNT_CONTRACT);
            if (configModel != null && !string.IsNullOrEmpty(configModel.ConfigValue))
            {
                _invPatternForDiscountContract = configModel.ConfigValue;
            }

            _invProvider = new InvProvider($"{hostInvService}PortalService.asmx", $"{hostInvService}BusinessService.asmx", $"{hostInvService}PublishService.asmx");

            #endregion
        }

        private static string[] _arrPermissionViaUser;

        #region Main Actions

        // GET: Major/Contract
        [ActionType(Type = EnumActionType.View)]
        public ActionResult Index()
        {
            _arrPermissionViaUser = GetPermissionViaUser(User.UserName);

            var lstUnionsManagerByUser = _unionCache.GetUnionsViaManager(User.UserName);

            var searchModel = new SearchContractModel
            {
                ListUnions = lstUnionsManagerByUser.Select(u => new ListItem(text: u.UnionName, value: $"{u.UnionId}")).ToList(),
                Permissions = _arrPermissionViaUser
            };

            return View(searchModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.View)]
        public Task<ActionResult> Get(SearchContractModel searchModel)
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

            var data = _contractCache.Get(out var total, searchModel.UnionIds, searchModel.SearchValue, searchModel.FromDate, searchModel.ToDate, searchModel.GiveResultFromDate, searchModel.GiveResultToDate, searchModel.ContractStatus, searchModel.TypeContractIds, searchModel.TypeCusIds, User.UserName, dataSearch);

            #region Check late or not

            var contractLate = _sysConfigCache.GetViaKey("CONFIG_KEY_CONTRACT_LATE");
            int dateLateConfig = int.Parse(contractLate.ConfigValue);

            data.ForEach(c =>
            {
                if (c.DelayDay > dateLateConfig)
                {
                    c.CheckContractLate = 1;
                }
                else if (c.DelayDay < 0)
                {
                    c.CheckContractLate = -1;
                }
                else
                {
                    c.CheckContractLate = 0;
                }
            });

            //foreach (var contract in data)
            //{
            //    if (contract.GiveResultOn != null)
            //    {
            //        TimeSpan delayTimeSpan = contract.GiveResultOn.Value.Subtract(DateTime.Today);

            //        // Lấy số ngày làm việc còn lại, không tính ngày nghỉ
            //        for (DateTime date = DateTime.Today; date <= contract.GiveResultOn.Value; date = date.AddDays(1))
            //        {
            //            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday || lstHolidays.Exists(h => h.RealDate.Subtract(date).Days == 0))
            //            {
            //                delayTimeSpan = delayTimeSpan.Subtract(TimeSpan.FromDays(1));
            //            }
            //        }
            //        contract.DelayDay = delayTimeSpan.Days;
            //        if (contract.DelayDay > dateLateConfig)
            //        {
            //            contract.CheckContractLate = 1;
            //        }
            //        else if (contract.DelayDay < 0)
            //        {
            //            contract.CheckContractLate = -1;
            //        }
            //        else
            //        {
            //            contract.CheckContractLate = 0;
            //        }
            //    }
            //    else
            //    {
            //        contract.DelayDay = 0;
            //        contract.CheckContractLate = 1;
            //    }
            //}

            #endregion

            var result = Json(new { draw = Convert.ToInt32(draw), recordsTotal = total, recordsFiltered = total, data, permission = _arrPermissionViaUser }, JsonRequestBehavior.AllowGet);

            return Task.FromResult<ActionResult>(result);
        }

        #region Add

        [AjaxOnly]
        [ActionType(Type = EnumActionType.Add)]
        [HttpGet]
        public ActionResult Add(int typeContract)
        {
            var unionMemberInfo = _unionCache.GetMemberInfo(User.UserName);
            if (unionMemberInfo == null)
            {
                return Json(new
                {
                    status = true,
                    message = CreateMessage(AppProcessor.Messagor.GetMessage("Err_Account_Not_Belong_Union"), EnumProcessType.NonFormat, EnumMsgIcon.Error)
                });
            }

            var unionViaStaff = _unionCache.GetUnionByMember(User.UserName);
            if (unionViaStaff == null)
            {
                return Json(new
                {
                    status = true,
                    message = CreateMessage(AppProcessor.Messagor.GetMessage("Err_Account_Not_Belong_Union"), EnumProcessType.NonFormat, EnumMsgIcon.Error)
                });
            }

            #region Check Procedure Via User

            var procsViaUnion = _procedureCache.GetViaUnion(unionViaStaff.UnionId);

            var usingProc =
                procsViaUnion.FirstOrDefault(p => p.ContractTypeId == typeContract);

            if (usingProc == null)
            {
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_procedureTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            }

            #endregion

            //var contractTypeEnum = _mappingTypeContractToEnum[typeContract];
            //var configViaTypeContract = _dictConfigsViaTypeContract.ContainsKey($"{typeContract}") ? _dictConfigsViaTypeContract[$"{typeContract}"] : new Dictionary<string, string>();

            var typeContractModel = _contractTypeCache.GetById(typeContract);
            if (typeContractModel == null)
            {
                return Json(new
                {
                    status = true,
                    message = CreateMessage(_contractTypeTitle, EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            }

            var signalContractViaType = typeContractModel.ContractSignal;

            var usingUnionCode = !_unionsNotUsingCode?.Exists(uc => uc == unionViaStaff.UnionId) ?? false;
            var unionCode = usingUnionCode ? "-" + unionViaStaff.UnionCode : "";
            var model = new MajorContractModel
            {
                ContractId = Guid.NewGuid(),
                //ContractSignal = configViaTypeContract.TryGetValue(KEYS_SIGNAL, out var value) ? value + unionCode : string.Empty,
                ContractSignal = string.IsNullOrEmpty(signalContractViaType) ? string.Empty : $"{signalContractViaType}{unionCode}",
                ContractTypeId = typeContract,
                ContractTypeName = AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription((EnumContractType)typeContract)),
                //ContractTypeEnum = contractTypeEnum,
                //PercentAdvance = double.Parse(configViaTypeContract.TryGetValue(KEYS_PERCENT_PAYMENT, out string percentValue) ? percentValue : "50"),
                PercentAdvance = typeContractModel.PercentAdvance,
                IsNew = true,
                TypeCus = ConstsCusType.CONSUMER,
                CusInfo = new MajorContractCustomerModel
                {
                    CusId = Guid.NewGuid()
                },
                ListPurposes = _purposeCache.GetAll(contractTypeIds: $"{typeContract}")
                    .OrderBy(p => p.PurPoseName)
                    .Select(d => new ListItem(d.PurPoseName, d.PurPoseId.ToString())).ToList(),
                UnionId = unionViaStaff.UnionId,
                UnionName = unionViaStaff.UnionName,
                ExtendInfos = unionViaStaff.UnionInfo,
                // Discount
                FuncDiscountContract = _hasCalcTaxForContract ? string.Empty : _functionDiscountContract,
                InfoDiscountContract = _hasCalcTaxForContract ? string.Empty : _infoDiscountContract,
                // Tax
                HasTaxForContract = _hasCalcTaxForContract,
                TaxRate = _defaultInvTaxRate,
                //FuncTaxContract = _funcTaxContract,
                TaxInfo = _taxInfoContract,

                ExpertiseCostPercent = _expertiseCostPercent
            };
            return PartialView("_Add", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Add)]
        [ValidateInput(false)]
        public Task<ActionResult> Add(MajorContractModel model)
        {
            #region Valid

            var lstContractTasks = Session[$"ContractTasks-{User.UserName}-{model.ContractId}"] as List<MajorContractTaskModel>;
            lstContractTasks = lstContractTasks ?? new List<MajorContractTaskModel>();
            //var contractTypeEnum = _mappingTypeContractToEnum[model.ContractTypeId ?? 1];

            if (lstContractTasks.Count <= 0 && model.ContractTypeId != (int)EnumContractType.Indefinite)
            {
                return Task.FromResult<ActionResult>(Json(new
                {
                    status = false,
                    message = CreateMessage(AppProcessor.Messagor.GetMessage("Err_Empty_ListTasks"), EnumProcessType.NonFormat, EnumMsgIcon.Error)
                }));
            }

            var unionMemberInfo = _unionCache.GetMemberInfo(User.UserName);
            if (unionMemberInfo == null)
            {
                return Task.FromResult<ActionResult>(Json(new
                {
                    status = false,
                    message = CreateMessage(AppProcessor.Messagor.GetMessage("Err_Account_Not_Belong_Union"), EnumProcessType.NonFormat, EnumMsgIcon.Error)
                }));
            }

            var unionViaStaff = _unionCache.GetUnionByMember(User.UserName);
            if (unionViaStaff == null)
            {
                return Task.FromResult<ActionResult>(Json(new
                {
                    status = false,
                    message = CreateMessage(AppProcessor.Messagor.GetMessage("Err_Account_Not_Belong_Union"), EnumProcessType.NonFormat, EnumMsgIcon.Error)
                }));
            }

            if (!ModelState.IsValid)
            {
                //var dbTypeContract = _mappingTypeContract[model.ContractTypeId ?? 1];

                model.ListPurposes = _purposeCache.GetAll(contractTypeIds: $"{model.ContractTypeId}")
                    .OrderBy(p => p.PurPoseName)
                    .Select(d => new ListItem(d.PurPoseName, d.PurPoseId.ToString())).ToList();
                model.CusInfo = model.CusInfo ?? new MajorContractCustomerModel
                {
                    CusId = Guid.NewGuid()
                };

                model.UnionName = unionViaStaff.UnionName;
                model.ExtendInfos = unionViaStaff.UnionInfo;

                // Discount
                model.FuncDiscountContract = _hasCalcTaxForContract ? string.Empty : _functionDiscountContract;
                model.InfoDiscountContract = _hasCalcTaxForContract ? string.Empty : _infoDiscountContract;
                // Tax
                model.HasTaxForContract = _hasCalcTaxForContract;
                model.TaxRate = _defaultInvTaxRate;
                //model.FuncTaxContract = _funcTaxContract;
                model.TaxInfo = _taxInfoContract;

                model.ExpertiseCostPercent = _expertiseCostPercent;

                model.ListTasks = lstContractTasks;

                return Task.FromResult<ActionResult>(PartialView("_Contract", model));
            }

            #endregion

            #region Process Task

            var dataContractTasks = new DataTable();

            using (var reader = ObjectReader.Create(lstContractTasks, "TaskId", "ContractId", "Ordinal", "Contents", "ContentId", "SectionId", "TypeLandName", "SubSectionId", "SubSectionName", "Area", "Unit", "Price", "Amount", "ContentLandId", "LandCalculationId", "Rate", "RateFormula"))
            {
                dataContractTasks.Load(reader);
            }

            #region Render Table Task Contract

            var dictMappingTemplateTasks = _dictMappingTemplateContractTasks[$"{model.ContractTypeId}"];

            var dtContractTasks = new DataTable();
            var arrCols = dictMappingTemplateTasks.Keys.ToArray();

            using (var reader = ObjectReader.Create(lstContractTasks, arrCols))
            {
                dtContractTasks.Load(reader);
            }

            DataTable dtClonedContractTasks = dtContractTasks.Clone();

            for (var colIdx = 0; colIdx < dtClonedContractTasks.Columns.Count; colIdx++)
            {
                dtClonedContractTasks.Columns[colIdx].DataType = typeof(string);
            }

            foreach (DataRow row in dtContractTasks.Rows)
            {
                dtClonedContractTasks.ImportRow(row);
            }

            foreach (var key in dictMappingTemplateTasks.Keys)
            {
                dtClonedContractTasks.Columns[key].ColumnName = dictMappingTemplateTasks[key];
            }

            #endregion

            #endregion

            #region Process Customer

            if (model.CusInfo == null)
            {
                return Task.FromResult<ActionResult>(Json(new
                {
                    status = false,
                    message = CreateMessage($"{_contractCusTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                }));
            }

            var dataContractCus = new DataTable();
            model.CusInfo.CusName = model.CusInfo.TypeCus == ConstsCusType.BUSINESS
                ? model.CusInfo.EnterpriseName
                : model.CusInfo.CusName;
            using (var reader = ObjectReader.Create(new List<MajorContractCustomerModel> { model.CusInfo }, "CusId", "TypeCus", "TypeCusName", "CusName", "TaxCode", "Gender", "TypeIdentifier", "TypeIdentifierName", "IdentifierNo", "Phone", "Email", "ProvinceId", "WardId", "StreetName", "AddressNo", "Address", "IsRepresenter", "RepresenterName", "RepresenterIdentifierNo", "RepresenterTitle", "RepresenterGender", "IsPrimary"))
            {
                dataContractCus.Load(reader);
            }

            #endregion

            #region Process Dossier

            #region Check Procedure Via User

            var procsViaUnion = _procedureCache.GetViaUnion(unionViaStaff.UnionId);

            var usingProc =
                procsViaUnion.FirstOrDefault(p => p.ContractTypeId == model.ContractTypeId);

            if (usingProc == null)
            {
                return Task.FromResult<ActionResult>(Json(new
                {
                    status = false,
                    message = CreateMessage($"{_procedureTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                }));
            }

            #endregion

            #region Get Proc Configs

            var lstStepsInProc = _stepCache.GetAll(usingProc.ProcedureId.ToString());
            var startStep = lstStepsInProc.FirstOrDefault(s => s.PrevStep == null && s.StepType == "Start");
            var firstStep = lstStepsInProc.FirstOrDefault(s => s.StepId == startStep?.NextStep);
            double totalHandlingTime = 0;

            if (firstStep == null)
            {
                return Task.FromResult<ActionResult>(Json(new
                {
                    status = false,
                    message = CreateMessage($"{_stepTitle} - {_procedureTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                }));
            }

            #region Views Steps Structure

            List<ViewStepStructureModel> viewStepStructures = new List<ViewStepStructureModel>();
            lstStepsInProc.ForEach(s =>
            {
                var handlers = _stepCache.GetHandlers(s.StepId);
                var handlingTimes = _stepCache.GetHandlingTimes(s.StepId);
                totalHandlingTime += handlingTimes
                    .Where(ht => !string.IsNullOrEmpty(ht.PurposeIds) && ht.PurposeIds.Split(',').Select(int.Parse).ToList().Exists(p => p == model.PurposeId))
                    .Sum(ht => ht.HandlingTime);
                var handlerStep = handlers.FirstOrDefault(h => h.UnionId == unionViaStaff.UnionId);
                var situations = _stepCache.GetSituations(s.StepId);

                viewStepStructures.Add(new ViewStepStructureModel
                {
                    StepId = s.StepId,
                    StepName = s.StepName,
                    StepDesc = s.StepDesc,
                    PrevStepName = s.PrevStepName,
                    StepType = s.StepType,
                    NextStep = s.NextStep,
                    NextStepName = s.NextStepName,
                    PrevStep = s.PrevStep,
                    Ordinal = s.Ordinal,

                    UnionHandle = handlerStep?.UnionId ?? unionViaStaff.UnionId,
                    UnionHandleName = handlerStep?.UnionName ?? unionViaStaff.UnionName,
                    DeptHandle = handlerStep?.DeptId ?? unionMemberInfo.UnionId,
                    DeptHandleName = handlerStep?.DeptName ?? unionMemberInfo.UnionName,
                    PositionId = handlerStep?.PositionID ?? unionMemberInfo.PositionId,
                    PositionName = handlerStep?.PositionName ?? unionMemberInfo.PositionName,
                    HandledBy = handlerStep?.StaffId ?? unionMemberInfo.UserName,

                    AllowChangeHandler = handlerStep?.AllowChangeHandler,
                    StepsChangeHandler = handlerStep?.StepsChangeHandler,
                    AllowSwitchHandler = handlerStep?.AllowSwitchHandler,

                    AttachResultFile = s.AttachResultFile,
                    StaffNotificationConfigs = s.StaffNotificationConfigs,
                    CusNotificationConfigs = s.CusNotificationConfigs,

                    Handlers = handlers.Select(h => new ViewHandlerStepStructureModel
                    {
                        UnionId = h.UnionId,
                        UnionName = h.UnionName,
                        DeptId = h.DeptId,
                        DeptName = h.DeptName,
                        PositionId = h.PositionID,
                        PositionName = h.PositionName,
                        StaffId = h.StaffId,
                        StaffName = h.StaffName,
                        AllowChangeHandler = h.AllowChangeHandler,
                        StepsChangeHandler = h.StepsChangeHandler,
                        AllowSwitchHandler = h.AllowSwitchHandler
                    }).ToList(),
                    HandlingTimes = handlingTimes.Select(ht => new ViewHandlingTimeStepStructureModel
                    {
                        HandlingTime = ht.HandlingTime,
                        PurposeIds = ht.PurposeIds,
                        PurposeNames = ht.PurposeNames
                    }).ToList(),
                    Situations = situations.Select(si => new ViewSituationStructureModel
                    {
                        SituationId = si.SituationId,
                        SituationDesc = si.SituationName,
                        NextStep = si.NextStep,
                        NextStepName = si.NextStepName,
                    }).ToList()
                });
            });

            var procStructureModel = new ViewProcedureStructureModel
            {
                ProcedureId = usingProc.ProcedureId,
                ApplyFrom = usingProc.ApplyFrom,
                ExpiredOn = usingProc.ExpiredOn,
                ProcedureDesc = usingProc.ProcedureDesc,
                ProcedureName = usingProc.ProcedureName,
                Version = usingProc.Version,
                Steps = viewStepStructures,
                ProcUnionId = unionViaStaff.UnionId,
                ProcUnionName = unionViaStaff.UnionName
            };
            var firstStepView = procStructureModel.Steps.FirstOrDefault(s => s.StepId == firstStep.StepId);
            if (firstStepView == null)
            {
                return Task.FromResult<ActionResult>(Json(new
                {
                    status = false,
                    message = CreateMessage($"{_stepTitle} - {_procedureTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                }));
            }

            firstStepView.HandledBy = User.UserName;

            #endregion

            #endregion

            #endregion

            #region Render Contract Info

            #region Extend Info

            var sExtendFields = Request.Form["ExtendFields"];
            if (!string.IsNullOrEmpty(sExtendFields))
            {
                var lstExtendFields = sExtendFields.Split(';').ToList();
                NameValueCollection extendInfos = new NameValueCollection();

                foreach (string key in Request.Form.AllKeys)
                {
                    if (lstExtendFields.Contains(key))
                    {
                        extendInfos.Add(key, Request.Form[key]);
                    }
                }
                var jsonExtendInfos = JsonConvert.SerializeObject(extendInfos.AllKeys.ToDictionary(k => k, k => extendInfos[k]));
                model.ExtendInfos = jsonExtendInfos;
            }

            #endregion

            #region Owner Info

            Dictionary<string, string> dictUnionInfo = null;
            var unionInfo = unionViaStaff.UnionInfo;
            if (!string.IsNullOrEmpty(unionInfo))
            {
                dictUnionInfo = JsonConvert.DeserializeObject<Dictionary<string, string>>(unionInfo);
            }

            #endregion

            string cusInfo = "";
            var configCusInfoModel = _sysConfigCache.GetViaKey(CONFIG_CUS_INFO_IN_CONTRACT);
            if (configCusInfoModel != null && !string.IsNullOrEmpty(configCusInfoModel.ConfigValue))
            {
                var dictConfigCusInfo = JsonConvert.DeserializeObject<Dictionary<string, string>>(configCusInfoModel.ConfigValue);
                if (dictConfigCusInfo?.ContainsKey(model.CusInfo.TypeCus) ?? false)
                {
                    var templateCusInfo = dictConfigCusInfo[model.CusInfo.TypeCus];
                    if (!string.IsNullOrEmpty(templateCusInfo))
                    {
                        cusInfo = model.CusInfo.TypeCus == ConstsCusType.BUSINESS
                            ? string.Format(templateCusInfo, model.CusInfo.RepresenterGenderAlias, model.CusInfo.RepresenterName, model.CusInfo.RepresenterTitle, model.CusInfo.Address, model.CusInfo.TaxCode, model.CusInfo.Phone)
                            : string.Format(templateCusInfo, model.CusInfo.GenderAlias, model.CusInfo.CusName, model.CusInfo.Address, model.CusInfo.Phone, model.CusInfo.TypeIdentifierName, model.CusInfo.IdentifierNo);
                    }
                }
            }

            //var cusInfo = model.CusInfo.TypeCus == ConstsCusType.BUSINESS ? $"Đại diện {model.CusInfo.RepresenterGenderAlias}: {model.CusInfo.RepresenterName}\nChức vụ: {model.CusInfo.RepresenterTitle}.\nĐịa chỉ: {model.CusInfo.Address}\nMã số thuế: {model.CusInfo.TaxCode}\nĐiện thoại: {model.CusInfo.Phone}" : $"Đại diện {model.CusInfo.GenderAlias}: {model.CusInfo.CusName}\nĐịa chỉ: {model.CusInfo.Address}\nĐiện thoại: {model.CusInfo.Phone}";

            var addressContract = String.IsNullOrEmpty(model.MapNo) || String.IsNullOrEmpty(model.MapNo) ? model.Address : string.Format(AppProcessor.Messagor.GetMessage("AddressContract"), model.MapNo, model.LandParcelNo, model.Address);
            var dictJsonDataContracts = new Dictionary<string, object>();
            var keyUnionName = _sysConfigCache.GetViaKey("CONFIG_KEY_UNIONNAME");
            var typeContract = _contractTypeCache.GetById(model.ContractTypeId ?? 0);

            #region Calc Value Of Contract

            // Chi phí sản phẩm
            var rawSubTotal = lstContractTasks.Sum(a => a.Total ?? 0);
            double expertiseCost = 0;

            //double subTotal = Math.Round(rawSubTotal / 1000) * 1000;
            double subTotal = Math.Round(rawSubTotal);

            if (typeContract.ContractTypeId == (int)EnumContractType.Expertise)
            {
                // Chi phí thẩm định / cập nhật quy hoạch
                expertiseCost = Math.Round(rawSubTotal * _expertiseCostPercent / 100);
                subTotal = Math.Round(expertiseCost);
                //rawSubTotal = expertiseCost;
            }

            // Chi phí sản phẩm làm tròn
            else if (typeContract.ContractTypeId == (int)EnumContractType.Indefinite)
            {
                subTotal = model.SubTotal;
            }

            double discountAmount = 0, taxAmount = 0, total;
            string totalInWords, functionDiscountContract = "";

            #region Tính toán thuế hoặc thành tiền giảm giá cho hợp đồng

            if (model.HasTaxForContract)
            {
                total = subTotal;
                double totalBeforeTax = Math.Round(total * 100 / (double)(model.TaxRate + 100));
                taxAmount = Math.Round(totalBeforeTax * (double)model.TaxRate / 100);

                totalInWords = NumberHelper.NumberToString(total.ToString());
            }
            else
            {
                functionDiscountContract = string.Format(_functionDiscountContract, subTotal);
                discountAmount =
                    Math.Round(double.Parse(dataContractTasks.Compute(functionDiscountContract, "").ToString()));
                total = subTotal - discountAmount;
                totalInWords = NumberHelper.NumberToString(total.ToString());
            }

            #endregion

            #endregion

            if (_dictExtendContractInfos.Count > 0)
            {
                var clone = (CultureInfo)CultureInfo.InvariantCulture.Clone();
                clone.NumberFormat.CurrencySymbol = "";

                Dictionary<string, object> dictTotalContractInfos = new Dictionary<string, object>
                {
                    //{ "DayContract", $"{(model.ConfirmOn ?? DateTime.Now).Day}"},
                    //{ "MonthContract", $"{(model.ConfirmOn ?? DateTime.Now).Month}"},
                    //{ "YearContract", $"{(model.ConfirmOn ?? DateTime.Now).Year}"},

                    { "DayContract", ""},
                    { "MonthContract", ""},
                    { "YearContract", ""},

                    { "FormattedPercentAdvance", model.FormattedPercentAdvance},
                    { "CoordinatesSignature1", ""},
                    { "CoordinatesSignature2", ""},

                    { $"TotalCosts_{model.ContractTypeId}", $"{Math.Round(rawSubTotal).ToString("C0", clone)}"},
                    { "AppraisalCosts", $"{expertiseCost.ToString("C0", clone)}"},
                    { "RoundTotalCosts", $"{Math.Round(subTotal).ToString("C0", clone)}"},
                    { "DiscountAmount", $"{Math.Round(discountAmount).ToString("C0", clone)}"},
                    { "TaxRate", $"{model.TaxRate}"},
                    { "TaxAmount", $"{Math.Round(taxAmount).ToString("C0", clone)}"},
                    { "TotalAmount", $"{total.ToString("C0", clone)}"},
                    { "TotalAmountInWord", totalInWords},
                    { "Tasks", JArray.Parse(JsonConvert.SerializeObject(dtClonedContractTasks)) }
                };

                if (model.CusInfo.TypeCus == ConstsCusType.BUSINESS)
                {
                    dictTotalContractInfos.Add("CusInfo.CusName", $"\n{model.CusInfo.CusName}");
                }
                NameValueCollection formData = new NameValueCollection(Request.Form)
                {
                    { "CusInfo", cusInfo },
                    { "UnionName", _dictMappingUnionViaUnitNames[unionViaStaff.UnionId.Value] },
                    { "HandlingTime", $"{totalHandlingTime}" }
                };

                foreach (var key in _dictExtendContractInfos.Keys)
                {
                    if (formData.AllKeys.Contains(key))
                    {
                        if (key == "Address")
                        {
                            dictJsonDataContracts.Add(_dictExtendContractInfos[key], addressContract);
                        }
                        else if (key == "CusInfo.CusName")
                        {
                            if (model.CusInfo.TypeCus == ConstsCusType.CONSUMER)
                            {
                                dictJsonDataContracts.Add(_dictExtendContractInfos[key], "");
                            }
                        }
                        else
                        {
                            dictJsonDataContracts.Add(_dictExtendContractInfos[key], formData[key]);
                        }
                    }
                    else if (dictUnionInfo.TryGetValue(key, out var value))
                    {
                        if (key == "EnterpriseName")
                        {
                            value = keyUnionName.ConfigValue;
                        }
                        if (key == "EnterpriseNameUpperCase")
                        {
                            value = keyUnionName.ConfigValue.ToUpper();
                        }

                        dictJsonDataContracts.Add(_dictExtendContractInfos[key], value);
                    }
                    else if (dictUnionInfo.ContainsKey($"{model.CusInfo.TypeCus}.{key}"))
                    {
                        var valueInForm = Request.Form[$"{model.CusInfo.TypeCus}.{key}"];
                        dictJsonDataContracts.Add(_dictExtendContractInfos[key],
                            !string.IsNullOrEmpty(valueInForm)
                                ? valueInForm
                                : dictUnionInfo[$"{model.CusInfo.TypeCus}.{key}"]);
                    }
                    else
                    {
                        if (_dictExtendContractInfos.TryGetValue(key, out var info))
                        {
                            if (dictTotalContractInfos.TryGetValue(key, out var contractInfo))
                            {
                                dictJsonDataContracts.Add(info, contractInfo);
                            }
                        }
                    }
                }

                //foreach (var key in dictTotalContractInfos.Keys)
                //{
                //    if (_dictExtendContractInfos.TryGetValue(key, out var info))
                //    {
                //        dictJsonDataContracts.Add(info, dictTotalContractInfos[key]);
                //    }
                //}
            }

            var jsonDataContracts = JsonConvert.SerializeObject(dictJsonDataContracts);

            #endregion

            var contractId = _contractCache.Save(new MajorContractModel
            {
                ContractId = model.ContractId,
                UnionId = unionViaStaff.UnionId,

                ContractNo = model.ContractNo,
                ContractSignal = model.ContractSignal,
                ContractTypeId = model.ContractTypeId,
                ContractTypeName = model.ContractTypeName,
                PurposeId = model.PurposeId,
                PurposeName = model.PurposeName,
                LandParcelNo = model.LandParcelNo,
                MapNo = model.MapNo,

                Status = (int)EnumContractStatus.Draft,
                StatusName = AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(EnumContractStatus.Draft)),

                SubTotal = (long)subTotal,
                Discount = (long)discountAmount,
                // Tax
                TaxRate = model.TaxRate,
                TaxAmount = (long)taxAmount,

                Total = (long)total,
                TotalInWords = totalInWords,

                PaymentMethod = model.PaymentMethod,
                PaymentMethodName = model.PaymentMethodName,
                PercentAdvance = model.PercentAdvance,
                AdvanceAmount = model.AdvanceAmount,
                PeriodAdvance = model.PeriodAdvance,

                InfoDiscountContract = string.Format(_infoDiscountContract, $"{subTotal:N0}", $"{discountAmount:N0}"),
                FuncDiscountContract = functionDiscountContract,

                JsonExtendContracts = jsonDataContracts,
                ExtendInfos = model.ExtendInfos,
                HandlingTime = totalHandlingTime,

                ProvinceId = model.ProvinceId,
                ProvinceName = model.ProvinceName,
                WardId = model.WardId,
                WardName = model.WardName,
                Address = model.Address,

                DataTasks = dataContractTasks,
                DataCus = dataContractCus,
                //DataDossier = dataDossier,

                Reason = "Thêm mới",
                UpdatedBy = User.UserName
            });

            var response = CreateMessage($"{_contractTitle} [{model.ContractTypeName} - {(model.CusInfo?.IsRepresenter ?? false ? model.CusInfo?.EnterpriseName : model.CusInfo?.CusName)}]", EnumProcessType.Add, contractId == 0 ? EnumMsgIcon.Error : EnumMsgIcon.Success);

            return Task.FromResult<ActionResult>(Json(new { status = contractId == 0, message = response, contractId = model.ContractId }, JsonRequestBehavior.AllowGet));
        }

        #endregion

        #region Edit

        [AjaxOnly]
        [ActionType(Type = EnumActionType.Edit)]
        [HttpGet]
        public ActionResult Edit(Guid? contractId)
        {
            var contractModel = _contractCache.GetById(contractId);
            if (contractModel == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_contractTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            //contractModel.ContractTypeEnum = _mappingTypeContractToEnum[contractModel.ContractTypeId ?? 1];

            contractModel.ContractTypeName =
                AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription((EnumContractType)contractModel.ContractTypeId));

            contractModel.CusInfo = _contractCache.GetCus(contractId);
            contractModel.ListPurposes = _purposeCache.GetAll(contractTypeIds: $"{contractModel.ContractTypeId}")
                .OrderBy(p => p.PurPoseName)
                .Select(d => new ListItem(d.PurPoseName, d.PurPoseId.ToString())).ToList();

            var lstContractTasks = _contractCache.GetTask(contractId);
            lstContractTasks.ForEach(t =>
            {
                t.FormattedPrice = t.Price.ToString("N0");
                t.FormattedRate = t.Rate * 100;
                t.FormattedTotal = (t.Total ?? 0).ToString("N0");
            });
            Session[$"ContractTasks-{User.UserName}-{contractId}"] = contractModel.ListTasks = lstContractTasks.Select(t => t).ToList();
            contractModel.IsNew = false;

            // Tax
            contractModel.HasTaxForContract = contractModel.TaxRate > 0;

            // Discount
            contractModel.FuncDiscountContract = contractModel.HasTaxForContract ? string.Empty : _functionDiscountContract;
            contractModel.InfoDiscountContract = contractModel.HasTaxForContract ? string.Empty : _infoDiscountContract;

            //contractModel.FuncTaxContract = _funcTaxContract;
            contractModel.TaxInfo = _taxInfoContract;

            return PartialView("_Edit", contractModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionType(Type = EnumActionType.Edit)]
        [ValidateInput(false)]
        public ActionResult Edit(MajorContractModel model)
        {
            var lstContractTasks = Session[$"ContractTasks-{User.UserName}-{model.ContractId}"] as List<MajorContractTaskModel>;

            if (lstContractTasks?.Count <= 0 && model.ContractTypeId != (int)EnumContractType.Indefinite)
            //if (lstContractTasks?.Count <= 0)
            {
                return Json(new
                {
                    status = true,
                    message = CreateMessage(AppProcessor.Messagor.GetMessage("Err_Empty_ListTasks"), EnumProcessType.NonFormat, EnumMsgIcon.Error)
                });
            }

            if (!ModelState.IsValid)
            {
                model.HasTaxForContract = model.TaxRate > 0;

                model.ListPurposes = _purposeCache.GetAll(contractTypeIds: $"{model.ContractTypeId}")
                    .OrderBy(p => p.PurPoseName)
                    .Select(d => new ListItem(d.PurPoseName, d.PurPoseId.ToString())).ToList();
                model.CusInfo = _contractCache.GetCus(model.ContractId);
                Session[$"ContractTasks-{User.UserName}-{model.ContractId}"] = model.ListTasks = lstContractTasks.Select(t => t).ToList();

                model.ContractTypeName =
                    AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription((EnumContractType)model.ContractTypeId));

                return PartialView("_Contract", model);
            }

            #region Process Task

            var dataContractTasks = new DataTable();

            using (var reader = ObjectReader.Create(lstContractTasks, "TaskId", "ContractId", "Ordinal", "Contents", "ContentId", "SectionId", "TypeLandName", "SubSectionId", "SubSectionName", "Area", "Unit", "Price", "Amount", "ContentLandId", "LandCalculationId", "Rate", "RateFormula"))
            {
                dataContractTasks.Load(reader);
            }

            #region Render Table Task Contract

            var dictMappingTemplateTasks = _dictMappingTemplateContractTasks[$"{model.ContractTypeId ?? 1}"];

            var dtContractTasks = new DataTable();
            var arrCols = dictMappingTemplateTasks.Keys.ToArray();

            using (var reader = ObjectReader.Create(lstContractTasks, arrCols))
            {
                dtContractTasks.Load(reader);
            }

            DataTable dtClonedContractTasks = dtContractTasks.Clone();

            for (var colIdx = 0; colIdx < dtClonedContractTasks.Columns.Count; colIdx++)
            {
                dtClonedContractTasks.Columns[colIdx].DataType = typeof(string);
            }

            foreach (DataRow row in dtContractTasks.Rows)
            {
                dtClonedContractTasks.ImportRow(row);
            }

            foreach (var key in dictMappingTemplateTasks.Keys)
            {
                dtClonedContractTasks.Columns[key].ColumnName = dictMappingTemplateTasks[key];
            }

            #endregion

            #endregion

            #region Process Customer

            if (model.CusInfo == null)
            {
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_contractCusTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            }

            model.CusInfo.CusName = model.CusInfo.TypeCus == ConstsCusType.BUSINESS
                ? model.CusInfo.EnterpriseName
                : model.CusInfo.CusName;

            var dataContractCus = new DataTable();

            using (var reader = ObjectReader.Create(new List<MajorContractCustomerModel> { model.CusInfo }, "CusId", "TypeCus", "TypeCusName", "CusName", "TaxCode", "Gender", "TypeIdentifier", "TypeIdentifierName", "IdentifierNo", "Phone", "Email", "ProvinceId", "WardId", "StreetName", "AddressNo", "Address", "IsRepresenter", "RepresenterName", "RepresenterIdentifierNo", "RepresenterTitle", "RepresenterGender", "IsPrimary"))
            {
                dataContractCus.Load(reader);
            }

            #endregion

            #region Process Dossier

            var dataDossier = new DataTable();
            using (var reader = ObjectReader.Create(new List<MajorDossierModel>(), "TotalHandlingTime", "ProcedureId", "ProcedureName", "ProcConfigs", "InStep", "InStepName", "UnionHandled", "HandledBy", "PositionId", "HandlingTime", "Status", "StatusName", "TaskStatus", "TaskStatusName"))
            {
                dataDossier.Load(reader);
            }

            #endregion

            #region Render Contract Info

            #region Incase change purpose

            var procsViaUnion = _procedureCache.GetViaUnion(model.UnionId);
            var usingProc = procsViaUnion.FirstOrDefault(p => p.ContractTypeId == model.ContractTypeId);

            var lstStepsInProc = _stepCache.GetAll(usingProc.ProcedureId.ToString());
            double totalHandlingTime = 0;

            lstStepsInProc.ForEach(s =>
            {
                var handlingTimes = _stepCache.GetHandlingTimes(s.StepId);
                totalHandlingTime += handlingTimes
                    .Where(ht => !string.IsNullOrEmpty(ht.PurposeIds) && ht.PurposeIds.Split(',').Select(int.Parse)
                        .ToList().Exists(p => p == model.PurposeId))
                    .Sum(ht => ht.HandlingTime);
            });

            #endregion

            #region Extend Info

            var sExtendFields = Request.Form["ExtendFields"];
            if (!string.IsNullOrEmpty(sExtendFields))
            {
                var lstExtendFields = sExtendFields.Split(';').ToList();
                NameValueCollection extendInfos = new NameValueCollection();

                foreach (string key in Request.Form.AllKeys)
                {
                    if (lstExtendFields.Contains(key))
                    {
                        extendInfos.Add(key, Request.Form[key]);
                    }
                }
                var jsonExtendInfos = JsonConvert.SerializeObject(extendInfos.AllKeys.ToDictionary(k => k, k => extendInfos[k]));
                model.ExtendInfos = jsonExtendInfos;
            }


            #endregion

            #region Owner Info

            var unionViaMember = _unionCache.GetUnionByMember(User.UserName);
            if (unionViaMember == null)
            {
                return Json(new
                {
                    status = true,
                    message = CreateMessage(AppProcessor.Messagor.GetMessage("Err_Account_Not_Belong_Union"), EnumProcessType.NonFormat, EnumMsgIcon.Error)
                });
            }

            Dictionary<string, string> dictUnionInfo = null;

            var unionInfo = unionViaMember.UnionInfo;
            if (!string.IsNullOrEmpty(unionInfo))
            {
                dictUnionInfo = JsonConvert.DeserializeObject<Dictionary<string, string>>(unionInfo);
            }

            #endregion

            var contractModel = _contractCache.GetById(model.ContractId);

            string cusInfo = "";
            var configCusInfoModel = _sysConfigCache.GetViaKey(CONFIG_CUS_INFO_IN_CONTRACT);
            if (configCusInfoModel != null && !string.IsNullOrEmpty(configCusInfoModel.ConfigValue))
            {
                var dictConfigCusInfo = JsonConvert.DeserializeObject<Dictionary<string, string>>(configCusInfoModel.ConfigValue);
                if (dictConfigCusInfo?.ContainsKey(model.CusInfo.TypeCus) ?? false)
                {
                    var templateCusInfo = dictConfigCusInfo[model.CusInfo.TypeCus];
                    if (!string.IsNullOrEmpty(templateCusInfo))
                    {
                        cusInfo = model.CusInfo.TypeCus == ConstsCusType.BUSINESS
                            ? string.Format(templateCusInfo, model.CusInfo.RepresenterGenderAlias, model.CusInfo.RepresenterName, model.CusInfo.RepresenterTitle, model.CusInfo.Address, model.CusInfo.TaxCode, model.CusInfo.Phone)
                            : string.Format(templateCusInfo, model.CusInfo.GenderAlias, model.CusInfo.CusName, model.CusInfo.Address, model.CusInfo.Phone, model.CusInfo.TypeIdentifierName, model.CusInfo.IdentifierNo);
                    }
                }
            }

            var keyUnionName = _sysConfigCache.GetViaKey("CONFIG_KEY_UNIONNAME");

            var dictJsonDataContracts = string.IsNullOrEmpty(contractModel.JsonExtendContracts) ? new Dictionary<string, object>() :
                JsonConvert.DeserializeObject<Dictionary<string, object>>(contractModel.JsonExtendContracts);

            #region Calc Value Of Contract

            var typeContract = _contractTypeCache.GetById(model.ContractTypeId ?? 0);

            var rawSubTotal = lstContractTasks.Sum(a => a.Total ?? 0);
            double expertiseCost = 0;

            //var subTotal = Math.Round(rawSubTotal / 1000) * 1000;
            var subTotal = Math.Round(rawSubTotal);

            // Chi phí thẩm định
            if (typeContract.ContractTypeId == (int)EnumContractType.Expertise)
            {
                expertiseCost = Math.Round(rawSubTotal * _expertiseCostPercent / 100);
                //subTotal = Math.Round(expertiseCost / 1000) * 1000;
                subTotal = Math.Round(expertiseCost);
                //rawSubTotal = expertiseCost;
            }
            // Chi phí sản phẩm làm tròn
            else if (typeContract.ContractTypeId == (int)EnumContractType.Indefinite)
            {
                subTotal = model.SubTotal;
            }

            double discountAmount = 0, taxAmount = 0, total;
            string totalInWords, functionDiscountContract = "";

            #region Tính toán thuế hoặc thành tiền giảm giá cho hợp đồng

            if (model.HasTaxForContract)
            {
                total = subTotal;
                double totalBeforeTax = Math.Round(total * 100 / (double)(model.TaxRate + 100));
                taxAmount = Math.Round(totalBeforeTax * (double)model.TaxRate / 100);

                totalInWords = NumberHelper.NumberToString(total.ToString());
            }
            else
            {
                functionDiscountContract = string.Format(_functionDiscountContract, subTotal);
                discountAmount =
                    Math.Round(double.Parse(dataContractTasks.Compute(functionDiscountContract, "").ToString()));
                total = subTotal - discountAmount;
                totalInWords = NumberHelper.NumberToString(total.ToString());
            }

            #endregion

            //var functionDiscountContract = string.Format(_functionDiscountContract, subTotal);
            //var discountAmount = Math.Round(double.Parse(dataContractTasks.Compute(functionDiscountContract, "").ToString()));

            //var total = subTotal - discountAmount;
            //var totalInWords = NumberHelper.NumberToString($"{total}");

            #endregion

            if (_dictExtendContractInfos.Count > 0)
            {
                var addressContract = String.IsNullOrEmpty(model.MapNo) || String.IsNullOrEmpty(model.MapNo) ? model.Address : string.Format(AppProcessor.Messagor.GetMessage("AddressContract"), model.MapNo, model.LandParcelNo, model.Address);

                var clone = (CultureInfo)CultureInfo.InvariantCulture.Clone();
                clone.NumberFormat.CurrencySymbol = "";

                Dictionary<string, object> dictTotalContractInfos = new Dictionary<string, object>
                {
                    { "FormattedPercentAdvance", model.FormattedPercentAdvance},
                    { "PercentAdvance", model.PercentAdvance},
                    { $"TotalCosts_{model.ContractTypeId}", $"{Math.Round(rawSubTotal).ToString("C0", clone)}"},
                    { "RoundTotalCosts", $"{Math.Round(subTotal).ToString("C0", clone)}"},
                    { "DiscountAmount", $"{Math.Round(discountAmount).ToString("C0", clone)}"},
                    { "TaxRate", $"{model.TaxRate}"},
                    { "TaxAmount", $"{Math.Round(taxAmount).ToString("C0", clone)}"},
                    { "TotalAmount", $"{total.ToString("C0", clone)}"},
                    { "TotalAmountInWord", totalInWords},
                    { "Tasks", JArray.Parse(JsonConvert.SerializeObject(dtClonedContractTasks)) },
                    { "CoordinatesSignature1", ""},
                    { "CoordinatesSignature2", ""},
                    { "AppraisalCosts", $"{expertiseCost.ToString("C0", clone)}"},
                };

                NameValueCollection formData = new NameValueCollection(Request.Form)
                {
                    { "CusInfo", cusInfo },
                    { "UnionName", _dictMappingUnionViaUnitNames[unionViaMember.UnionId.Value] },
                    //{ "HandlingTime", $"{totalHandlingTime}" }
                };

                formData["HandlingTime"] = $"{totalHandlingTime}";

                foreach (var key in _dictExtendContractInfos.Keys)
                {
                    if (formData.AllKeys.Contains(key))
                    {
                        if (key == "Address")
                        {
                            if (dictJsonDataContracts.ContainsKey(_dictExtendContractInfos[key]))
                                dictJsonDataContracts[_dictExtendContractInfos[key]] = addressContract;
                            else
                                dictJsonDataContracts.Add(_dictExtendContractInfos[key], addressContract);
                        }
                        else if (key == "CusInfo.CusName")
                        {
                            if (model.CusInfo.TypeCus == ConstsCusType.CONSUMER)
                            {
                                if (dictJsonDataContracts.ContainsKey(_dictExtendContractInfos[key]))
                                    dictJsonDataContracts[_dictExtendContractInfos[key]] = "";
                                else
                                    dictJsonDataContracts.Add(_dictExtendContractInfos[key], "");
                            }
                        }
                        else
                        {
                            if (dictJsonDataContracts.ContainsKey(_dictExtendContractInfos[key]))
                                dictJsonDataContracts[_dictExtendContractInfos[key]] = formData[key];
                            else
                                dictJsonDataContracts.Add(_dictExtendContractInfos[key], formData[key]);
                        }
                    }
                    else if (dictUnionInfo.TryGetValue(key, out var value))
                    {
                        if (key == "EnterpriseName")
                        {
                            value = keyUnionName.ConfigValue;
                        }
                        if (key == "EnterpriseNameUpperCase")
                        {
                            value = keyUnionName.ConfigValue.ToUpper();
                        }
                        if (dictJsonDataContracts.ContainsKey(_dictExtendContractInfos[key]))
                            dictJsonDataContracts[_dictExtendContractInfos[key]] = value;
                        else
                            dictJsonDataContracts.Add(_dictExtendContractInfos[key], value);
                    }
                    else if (dictUnionInfo.ContainsKey($"{model.CusInfo.TypeCus}.{key}"))
                    {
                        var valueInForm = Request.Form[$"{model.CusInfo.TypeCus}.{key}"];
                        if (dictJsonDataContracts.ContainsKey(_dictExtendContractInfos[key]))
                            dictJsonDataContracts[_dictExtendContractInfos[key]] = !string.IsNullOrEmpty(valueInForm)
                                ? valueInForm
                                : dictUnionInfo[$"{model.CusInfo.TypeCus}.{key}"];
                        else
                            dictJsonDataContracts.Add(_dictExtendContractInfos[key],
                            !string.IsNullOrEmpty(valueInForm)
                                ? valueInForm
                                : dictUnionInfo[$"{model.CusInfo.TypeCus}.{key}"]);
                    }
                    else
                    {
                        if (_dictExtendContractInfos.TryGetValue(key, out var info))
                        {
                            if (dictTotalContractInfos.TryGetValue(key, out var contractInfo))
                            {
                                if (dictJsonDataContracts.ContainsKey(_dictExtendContractInfos[key]))
                                    dictJsonDataContracts[_dictExtendContractInfos[key]] = contractInfo;
                                else
                                    dictJsonDataContracts.Add(info, contractInfo);
                            }
                        }
                    }
                }
            }

            var jsonDataContracts = JsonConvert.SerializeObject(dictJsonDataContracts);

            #endregion

            var contractId = _contractCache.Save(new MajorContractModel
            {
                ContractId = model.ContractId,

                ContractNo = model.ContractNo,
                ContractSignal = model.ContractSignal,
                ContractTypeId = model.ContractTypeId,
                ContractTypeName = model.ContractTypeName,
                PurposeId = model.PurposeId,
                PurposeName = model.PurposeName,
                LandParcelNo = model.LandParcelNo,
                MapNo = model.MapNo,

                SubTotal = (long)subTotal,
                Discount = (long)discountAmount,
                // Tax
                TaxRate = model.TaxRate,
                TaxAmount = (long)taxAmount,
                Total = (long)total,
                TotalInWords = totalInWords,

                PaymentMethod = model.PaymentMethod,
                PaymentMethodName = model.PaymentMethodName,
                PercentAdvance = model.PercentAdvance,
                AdvanceAmount = model.AdvanceAmount,
                PeriodAdvance = model.PeriodAdvance,

                InfoDiscountContract = string.Format(_infoDiscountContract, $"{subTotal:N0}", $"{discountAmount:N0}"),
                FuncDiscountContract = functionDiscountContract,

                JsonExtendContracts = jsonDataContracts,
                ExtendInfos = model.ExtendInfos,
                HandlingTime = totalHandlingTime,

                ProvinceId = model.ProvinceId,
                ProvinceName = model.ProvinceName,
                WardId = model.WardId,
                WardName = model.WardName,
                Address = model.Address,

                DataTasks = dataContractTasks,
                DataCus = dataContractCus,
                DataDossier = dataDossier,

                Reason = model.Reason,
                UpdatedBy = User.UserName
            });

            var response = CreateMessage($"{_contractTitle} [{model.ContractNo} - {(model.CusInfo?.IsRepresenter ?? false ? model.CusInfo?.EnterpriseName : model.CusInfo?.CusName)}]", EnumProcessType.Edit, contractId == 0 ? EnumMsgIcon.Error : EnumMsgIcon.Success);
            return Json(new { status = true, message = response }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Delete

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(Guid? contractId)
        {
            var contractModel = _contractCache.GetById(contractId);
            if (contractModel == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_contractTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Delete"),
                $"<b>{_contractTitle} [{contractModel.ContractTypeName} - {contractModel.CusName}]</b>");
            return PartialView("_Delete", contractModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(MajorContractModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Delete"),
                $"<b>{_contractTitle} [{model.ContractTypeName} - {model.CusName}]</b>");
                return PartialView("_DeleteBody", model);
            }

            model.UpdatedBy = User.UserName;
            var ret = _contractCache.Delete(model);
            string response;
            if (ret == -19)
            {
                response = CreateMessage(string.Format(AppProcessor.Messagor.GetMessage("Data_Was_Used"), $"{_contractTitle} [{model.ContractTypeName} - {model.CusName}]"), EnumProcessType.NonFormat, EnumMsgIcon.Error);
                return Json(new { status = true, message = response });
            }
            response = CreateMessage($"{_contractTitle} [{model.ContractTypeName} - {model.CusName}]",
                EnumProcessType.Delete, ret > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response });
        }

        #endregion

        #endregion

        #region Valid Contract

        [AjaxOnly]
        [ActionType(Type = EnumActionType.Edit)]
        [HttpGet]
        public ActionResult Valid(Guid? contractId)
        {
            var contractModel = _contractCache.GetById(contractId);
            if (contractModel == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_contractTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Valid"),
                $"<b class='text-danger-d1'>[{contractModel.ContractTypeName} - {contractModel.CusName}]</b>");
            contractModel.IsNew = true;

            return PartialView("_Valid", contractModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult Valid(MajorContractModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Valid"),
                    $"<b>[{model.ContractTypeName} - {model.CusName}]</b>");
                return PartialView("_ValidBody", model);
            }

            model.UpdatedBy = User.UserName;
            //model.DataDossier = dataDossier;

            model.Status = (int)EnumContractStatus.Waiting;
            model.StatusName = AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(EnumContractStatus.Waiting));
            model.UsingUnionCode = !_unionsNotUsingCode?.Exists(uc => uc == model.UnionId) ?? false;

            var ret = _contractCache.Valid(model);

            if (ret > 0)
            {
                var contractInfo = _contractCache.GetById(model.ContractId);
                var contractFormInfo = _fEContractCache.GetDataRenderContract(model.ContractId);
                if (contractFormInfo != null && !string.IsNullOrEmpty(contractFormInfo.JsonContractInfo))
                {
                    var dictContractInfo =
                        JsonConvert.DeserializeObject<Dictionary<string, object>>(contractFormInfo.JsonContractInfo);

                    bool needUpdateContractInfo = false;

                    if (dictContractInfo.ContainsKey(_dictExtendContractInfos["ContractNo"]))
                    {
                        needUpdateContractInfo = true;
                        dictContractInfo[_dictExtendContractInfos["ContractNo"]] = contractInfo.ContractNo;
                    }
                    if (dictContractInfo.ContainsKey(_dictExtendContractInfos["DayContract"]))
                    {
                        needUpdateContractInfo = true;
                        dictContractInfo[_dictExtendContractInfos["DayContract"]] = $"{DateTime.Now.Day}";
                    }
                    if (dictContractInfo.ContainsKey(_dictExtendContractInfos["MonthContract"]))
                    {
                        needUpdateContractInfo = true;
                        dictContractInfo[_dictExtendContractInfos["MonthContract"]] = $"{DateTime.Now.Month}";
                    }
                    if (dictContractInfo.ContainsKey(_dictExtendContractInfos["YearContract"]))
                    {
                        needUpdateContractInfo = true;
                        dictContractInfo[_dictExtendContractInfos["YearContract"]] = $"{DateTime.Now.Year}";
                    }

                    if (needUpdateContractInfo)
                    {
                        var jsonContractInfo = JsonConvert.SerializeObject(dictContractInfo);
                        ret = _contractCache.UpdateInfo(model.ContractId, jsonContractInfo, contractFormInfo.FileId, User.UserName);
                    }

                }
            }

            var response = CreateMessage($"{_contractTitle} [{model.ContractTypeName} - {model.CusName}]",
                EnumProcessType.Edit, ret > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response, contractId = model.ContractId });
        }

        #endregion

        #region Approve

        [AjaxOnly]
        [ActionType(Type = EnumActionType.Edit)]
        [HttpGet]
        public async Task<ActionResult> Approve(Guid? contractId)
        {
            var contractModel = _contractCache.GetById(contractId);
            if (contractModel == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_contractTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var contractConfirmModel = new MajorContractConfirmModel
            {
                ContractId = contractId,
                ContractNo = contractModel.ContractNo,
                ContractSignal = contractModel.ContractSignal,
                ContractNoInfo = contractModel.ContractNoInfo,
                Total = contractModel.Total,
                CusName = contractModel.CusName,
                ReceivedOn = contractModel.ConfirmOn,
                ApprovedOn = DateTime.Now,
                HandleTime = contractModel.HandlingTime,
                GiveResultOn = CalculateReturnDate(DateTime.Now, contractModel.HandlingTime),
                ConfirmOn = DateTime.Now,
                UpdatedBy = User.UserName
            };

            return PartialView("_Approve", contractConfirmModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Edit)]
        [ValidateInput(false)]
        public async Task<ActionResult> Approve(MajorContractConfirmModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_ApproveBody", model);
            }

            var contractModel = _contractCache.GetById(model.ContractId);
            if (contractModel == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_contractTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            #region Get Proc Configs

            var deptMemberInfo = _unionCache.GetMemberInfo(User.UserName);
            if (deptMemberInfo == null)
            {
                return Json(new
                {
                    status = true,
                    message = CreateMessage(AppProcessor.Messagor.GetMessage("Err_Account_Not_Belong_Union"), EnumProcessType.NonFormat, EnumMsgIcon.Error)
                });
            }

            var unionViaStaff = _unionCache.GetUnionByMember(User.UserName);
            if (unionViaStaff == null)
            {
                return Json(new
                {
                    status = true,
                    message = CreateMessage(AppProcessor.Messagor.GetMessage("Err_Account_Not_Belong_Union"), EnumProcessType.NonFormat, EnumMsgIcon.Error)
                });
            }

            #region Check Procedure Via User

            var procsViaUnion = _procedureCache.GetViaUnion(unionViaStaff.UnionId);

            var usingProc =
                procsViaUnion.FirstOrDefault(p => p.ContractTypeId == contractModel.ContractTypeId);

            if (usingProc == null)
            {
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_procedureTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            }

            var lstStepsInProc = _stepCache.GetAll(usingProc.ProcedureId.ToString());
            var startStep = lstStepsInProc.FirstOrDefault(s => s.PrevStep == null && s.StepType == "Start");
            var firstStep = lstStepsInProc.FirstOrDefault(s => s.StepId == startStep?.NextStep);
            var totalHandlingTime = lstStepsInProc.Sum(s => s.HandlingTime);

            if (firstStep == null)
            {
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_stepTitle} - {_procedureTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            }

            #endregion

            #region Views Steps Structure

            List<ViewStepStructureModel> viewStepStructures = new List<ViewStepStructureModel>();

            lstStepsInProc.ForEach(s =>
            {
                var handlers = _stepCache.GetHandlers(s.StepId);
                var handlingTimes = _stepCache.GetHandlingTimes(s.StepId);
                var handlerStep = handlers.FirstOrDefault(h => h.UnionId == unionViaStaff.UnionId);
                var situations = _stepCache.GetSituations(s.StepId);

                viewStepStructures.Add(new ViewStepStructureModel
                {
                    StepId = s.StepId,
                    StepName = s.StepName,
                    StepDesc = s.StepDesc,
                    PrevStepName = s.PrevStepName,
                    StepType = s.StepType,
                    NextStep = s.NextStep,
                    NextStepName = s.NextStepName,
                    PrevStep = s.PrevStep,
                    Ordinal = s.Ordinal,

                    UnionHandle = handlerStep?.UnionId ?? unionViaStaff.UnionId,
                    UnionHandleName = handlerStep?.UnionName ?? unionViaStaff.UnionName,
                    DeptHandle = handlerStep?.DeptId ?? deptMemberInfo.UnionId,
                    DeptHandleName = handlerStep?.DeptName ?? deptMemberInfo.UnionName,
                    PositionId = handlerStep?.PositionID ?? deptMemberInfo.PositionId,
                    PositionName = handlerStep?.PositionName ?? deptMemberInfo.PositionName,
                    HandledBy = handlerStep?.StaffId ?? deptMemberInfo.UserName,

                    AllowChangeHandler = handlerStep?.AllowChangeHandler,
                    StepsChangeHandler = handlerStep?.StepsChangeHandler,
                    AllowSwitchHandler = handlerStep?.AllowSwitchHandler,

                    AttachResultFile = s.AttachResultFile,

                    StaffNotificationConfigs = s.StaffNotificationConfigs,
                    CusNotificationConfigs = s.CusNotificationConfigs,

                    Handlers = handlers.Select(h => new ViewHandlerStepStructureModel
                    {
                        UnionId = h.UnionId,
                        UnionName = h.UnionName,
                        DeptId = h.DeptId,
                        DeptName = h.DeptName,
                        PositionId = h.PositionID,
                        PositionName = h.PositionName,
                        StaffId = h.StaffId,
                        StaffName = h.StaffName,
                        AllowChangeHandler = h.AllowChangeHandler,
                        StepsChangeHandler = h.StepsChangeHandler,
                        AllowSwitchHandler = h.AllowSwitchHandler
                    }).ToList(),
                    HandlingTimes = handlingTimes.Select(ht => new ViewHandlingTimeStepStructureModel
                    {
                        HandlingTime = ht.HandlingTime,
                        PurposeIds = ht.PurposeIds,
                        PurposeNames = ht.PurposeNames
                    }).ToList(),
                    Situations = situations.Select(si => new ViewSituationStructureModel
                    {
                        SituationId = si.SituationId,
                        SituationDesc = si.SituationName,
                        NextStep = si.NextStep,
                        NextStepName = si.NextStepName,
                    }).ToList()
                });
            });

            var procStructureModel = new ViewProcedureStructureModel
            {
                ProcedureId = usingProc.ProcedureId,
                ApplyFrom = usingProc.ApplyFrom,
                ExpiredOn = usingProc.ExpiredOn,
                ProcedureDesc = usingProc.ProcedureDesc,
                ProcedureName = usingProc.ProcedureName,
                Version = usingProc.Version,
                Steps = viewStepStructures,
                ProcUnionId = unionViaStaff.UnionId,
                ProcUnionName = unionViaStaff.UnionName
            };
            var firstStepView = procStructureModel.Steps.FirstOrDefault(s => s.StepId == firstStep.StepId);
            if (firstStepView == null)
            {
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_stepTitle} - {_procedureTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            }

            firstStepView.HandledBy = firstStepView.HandledBy ?? User.UserName;

            #endregion

            #endregion

            #region Dossier Info

            var procConfigs = JsonConvert.SerializeObject(procStructureModel);

            var dossierModel = new MajorDossierModel
            {
                TotalHandlingTime = totalHandlingTime,
                ProcedureId = usingProc.ProcedureId,
                ProcedureName = usingProc.ProcedureName,
                InStep = firstStep.StepId,
                InStepName = firstStep.StepName,
                ProcConfigs = procConfigs,
                UnionHandled = unionViaStaff.UnionId,
                PositionId = deptMemberInfo.PositionId,
                HandledBy = User.UserName != firstStepView.HandledBy ? User.UserName : firstStepView.HandledBy,

                HandlingTime = firstStepView.HandlingTimes
                    .Where(ht => !string.IsNullOrEmpty(ht.PurposeIds) && ht.PurposeIds.Split(',').Select(int.Parse).ToList().Exists(p => p == contractModel.PurposeId))
                    .Sum(ht => ht.HandlingTime),

                Status = (int)EnumDossierStatus.Handling,
                StatusName = AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(EnumDossierStatus.Handling)),

                TaskStatus = (int)EnumDossierTaskStatus.Handling,
                TaskStatusName = AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(EnumDossierTaskStatus.Handling)),
            };

            var dataDossier = new DataTable();
            using (var reader = ObjectReader.Create(new List<MajorDossierModel> { dossierModel }, "TotalHandlingTime", "ProcedureId", "ProcedureName", "ProcConfigs", "InStep", "InStepName", "UnionHandled", "HandledBy", "PositionId", "HandlingTime", "Status", "StatusName", "TaskStatus", "TaskStatusName"))
            {
                dataDossier.Load(reader);
            }

            #endregion

            var nextStepView = procStructureModel.Steps.FirstOrDefault(s => s.StepId == firstStepView.NextStep);
            //var curentStepView = procStructureModel.Steps.FirstOrDefault(s => s.StepId == firstStepView.StepId);
            if (nextStepView == null)
            {
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_stepTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            }

            var handlerNextStep = nextStepView.Handlers.First(h => h.UnionId == firstStepView.UnionHandle);
            MajorApproveDossierModel approveDossierModel = new MajorApproveDossierModel
            {
                DossierId = model.ContractId,
                ApprovedOn = model.ApprovedOn,
                GiveResultOn = model.GiveResultOn,

                ContractStatus = (int)EnumContractStatus.Handling,
                ContractStatusName = AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(EnumContractStatus.Handling)),

                Status = (int)EnumDossierStatus.Handling,
                StatusName = AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(EnumDossierStatus.Handling)),
                NextStepId = nextStepView.StepId,
                NextStepName = nextStepView.StepName,

                DataDossier = dataDossier,

                UnionHandled = handlerNextStep.DeptId,
                UnionHandledName = firstStepView.DeptHandleName,

                HandledBy = handlerNextStep.StaffId,//User.UserName != handlerNextStep.StaffId ? User.UserName : handlerNextStep.StaffId,
                PositionId = handlerNextStep.PositionId,
                HandlingTime = model.HandleTime,
                HandlingDossierTime = nextStepView.TotalHandlingTimes($"{contractModel.PurposeId}"),

                CurrentTaskStatus = (int)EnumDossierTaskStatus.Completed,
                CurrentTaskStatusName = AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(EnumDossierTaskStatus.Completed)),

                TaskStatus = (int)EnumDossierTaskStatus.Handling,
                TaskStatusName = AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(EnumDossierTaskStatus.Handling)),

                UpdatedBy = User.UserName
            };

            var saveResult = _contractCache.Approve(approveDossierModel);
            if (saveResult > 0)
            {
                var sender = User.UserName;

                try
                {
                    var saveFile = SaveRefFiles(model.RefFiles, model.ContractId, sender, out var errMsg);
                    if (!saveFile)
                    {
                        AppProcessor.Logger.Message(errMsg);
                    }
                }
                catch (Exception ex)
                {
                    AppProcessor.Logger.Error(ex);
                }

                Queue<Task> queueTasks = new Queue<Task>();

                #region Gửi thông báo tới nhân viên xử lý tiếp theo

                if (!string.IsNullOrEmpty(nextStepView.StaffNotificationConfigs))
                {
                    var handledBy = approveDossierModel.HandledBy;
                    var urlSearchContract = Request.Url.GetLeftPart(UriPartial.Authority);
                    var urlSearchDossier = Url.Action("Index", "Dossier", new { q = contractModel.ContractNoInfo });

                    queueTasks.Enqueue(new Task(() =>
                    {
                        try
                        {
                            var reciever = _sysUserCache.GetByUserName(handledBy);

                            nextStepView.StaffNotificationConfigs.Split(';').ToList().ForEach(t =>
                            {
                                var libNotify = _listNotificationProviders.FirstOrDefault(n => n.Name == t);
                                if (libNotify != null)
                                {
                                    libNotify.Push(new ContentNotifyModel
                                    {
                                        TypeEmail = EnumTypeEmail.ContractPending,
                                        ContractInfo = new Contract
                                        {
                                            ContractNo = contractModel.ContractNo,
                                            ContractSignal = contractModel.ContractSignal,
                                            SearchContractDetailUrl = urlSearchContract,
                                            SearchContractUrl = urlSearchContract
                                        },
                                        CusInfo = new Customer
                                        {
                                            CusName = reciever.FullName,
                                            Email = reciever.Email,
                                            Phone = reciever.Phone
                                        },
                                        InsiteNotification = new InsiteNotificationModel
                                        {
                                            Icon = EnumMsgIcon.Info,
                                            Title = AppProcessor.Messagor.GetMessage("Dossier_Notify_Title_Handing"),
                                            Message = string.Format(AppProcessor.Messagor.GetMessage("Notify_Title_Handing_Contract"), contractModel.ContractNoInfo),
                                            Placement = "tr",
                                            Url = urlSearchDossier,
                                            Sender = sender,
                                            Receiver = approveDossierModel.HandledBy
                                        }

                                    });
                                }
                            });

                            AppProcessor.Logger.Message($"[{contractModel.ContractNoInfo}] - Gửi thông báo tới nhân viên xử lý tiếp theo");

                        }
                        catch (Exception ex)
                        {
                            AppProcessor.Logger.Error(ex);
                        }
                    }));
                }

                #endregion

                #region Gửi tin nhắn SMS tới khách hàng

                var requestUrl = Request.Url;
                var indxHome = Url.Action("Index", "Home", new { area = "" }, Request.Url.Scheme);

                queueTasks.Enqueue(new Task(() =>
                {
                    try
                    {
                        var cusInfo = _contractCache.GetCus(contractModel.ContractId);

                        // kiểm tra để gửi email
                        if (!string.IsNullOrEmpty(cusInfo.Email))
                        {
                            Dictionary<string, string> dictUnionInfo = null;
                            var unionInfo = unionViaStaff.UnionInfo;
                            var nameUnion = "";
                            var addressUnion = "";
                            var emailUnion = "";
                            var phoneUnion = "";

                            // Deserialize thông tin từ đối tượng JSON thành một từ điển
                            if (!string.IsNullOrEmpty(unionInfo))
                            {
                                dictUnionInfo = JsonConvert.DeserializeObject<Dictionary<string, string>>(unionInfo);
                            }

                            if (dictUnionInfo != null)
                                foreach (var kvp in dictUnionInfo)
                                {
                                    var key = kvp.Key;
                                    var value = kvp.Value;

                                    switch (key)
                                    {
                                        case "EnterpriseName":
                                            nameUnion = value;
                                            break;
                                        case "Email":
                                            emailUnion = value;
                                            break;
                                        case "Phone":
                                            phoneUnion = value;
                                            break;
                                        case "EnterpriseAddress":
                                            addressUnion = value;
                                            break;
                                    }
                                }

                            var modelNotification = new ContentNotificationModel
                            {
                                TypeEmail = EnumTypeEmail.ContractConfirmation,
                                ContractInfo = new Contract
                                {
                                    ContractNo = contractModel.ContractNo,
                                    ContractSignal = contractModel.ContractSignal,
                                    ContractNoInfo = contractModel.ContractNoInfo,
                                    SearchContractDetailUrl = requestUrl.GetLeftPart(UriPartial.Authority),
                                    SearchContractUrl = requestUrl.GetLeftPart(UriPartial.Authority)
                                },
                                CusInfo = new Customer
                                {
                                    CusName = contractModel.CusName,
                                    Email = cusInfo.Email,
                                    Phone = cusInfo.Phone,
                                    TypeCus = cusInfo.TypeCus
                                },
                                UnionInfo = new Union
                                {

                                    UnionName = nameUnion,
                                    Address = addressUnion,
                                    Email = emailUnion,
                                    Phone = phoneUnion
                                }
                            };
                            SendNotificationHelper.Send(modelNotification);
                        }

                        // gửi tin nhắn
                        var isSuccess = SmsProvider.Send(out string msgErr, cusInfo.Phone, EnumContractStatusHandle.Confirm, contractModel.ContractNoInfo, indxHome);

                        //SMSProvider.Send_SMS_Contract_Confirm(cusInfo.Phone, contractModel.ContractNoInfo, indxHome);

                        AppProcessor.Logger.Message($"[{contractModel.ContractNoInfo}] - Gửi tin nhắn xác nhận hợp đồng tới khách hàng: {(isSuccess ? "Thành công" : $"Thất bại ({msgErr})")}");
                    }
                    catch (Exception ex)
                    {
                        AppProcessor.Logger.Error(ex);
                    }
                }));

                #endregion

                await Task.Factory.StartNew(() =>
                {
                    while (queueTasks.Count > 0)
                    {
                        var queueTask = queueTasks.Dequeue();
                        queueTask.Start();
                    }
                });
            }
            var response = CreateMessage($"{_contractTitle} [{contractModel.ContractNo} - {contractModel.CusName}]", EnumProcessType.Edit, saveResult == 0 ? EnumMsgIcon.Error : EnumMsgIcon.Success);
            return Json(new { status = true, message = response }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Upload File

        [AjaxOnly]
        [ActionType(Type = EnumActionType.Edit)]
        [HttpGet]
        public async Task<ActionResult> UploadFile(Guid? contractId)
        {
            var contractModel = _contractCache.GetById(contractId);
            if (contractModel == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_contractTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var uploadFile = new UploadFileModel
            {
                ContractId = contractId,
                ContractNo = contractModel.ContractNo,
                ContractSignal = contractModel.ContractSignal,
                ContractNoInfo = contractModel.ContractNoInfo,
                CusName = contractModel.CusName
            };

            return PartialView("UploadFile/_UploadFile", uploadFile);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Edit)]
        [ValidateInput(false)]
        public Task<ActionResult> UploadFile(UploadFileModel model)
        {
            if (model.RefFiles == null || model.RefFiles.Count <= 0 || model.RefFiles[0] == null)
            {
                ModelState.AddModelError("RefFiles", "Vui lòng chọn tệp tải lên");
            }
            if (!ModelState.IsValid)
            {
                return Task.FromResult<ActionResult>(PartialView("UploadFile/_UploadFileBody", model));
            }

            var contractModel = _contractCache.GetById(model.ContractId);
            if (contractModel == null)
                return Task.FromResult<ActionResult>(Json(new
                {
                    status = true,
                    message = CreateMessage($"{_contractTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                }));

            var sender = User.UserName;

            var saveFile = SaveRefFiles(model.RefFiles, model.ContractId, sender, out _);

            var response = CreateMessage(string.Format(AppProcessor.Messagor.GetMessage("Modal_Title_Upload"), $"{AppProcessor.Messagor.GetMessage("Contract_RefFile")} [{contractModel.ContractNo} - {contractModel.CusName}]"), EnumProcessType.Edit, saveFile ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Task.FromResult<ActionResult>(Json(new { status = saveFile, message = response }, JsonRequestBehavior.AllowGet));
        }

        #endregion

        #region Reject

        [AjaxOnly]
        [ActionType(Type = EnumActionType.Edit)]
        [HttpGet]
        public ActionResult Reject(Guid? contractId)
        {
            var contractModel = _contractCache.GetById(contractId);
            if (contractModel == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_contractTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var contractConfirmModel = new MajorContractRejectModel
            {
                ContractId = contractId,
                ContractNo = contractModel.ContractNo,
                ContractSignal = contractModel.ContractSignal,
                ContractNoInfo = contractModel.ContractNoInfo,
                CusName = contractModel.CusName,
                RejectOn = DateTime.Now,
                ReceivedOn = contractModel.ReceivedOn,
                UpdatedBy = User.UserName
            };

            return PartialView("_Reject", contractConfirmModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Add)]
        [ValidateInput(false)]
        public ActionResult Reject(MajorContractRejectModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_RejectBody", model);
            }

            var contractModel = _contractCache.GetById(model.ContractId);
            if (contractModel == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_contractTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            model.UpdatedBy = User.UserName;
            model.ContractStatus = (int)EnumContractStatus.Cancel;
            model.ContractStatusName =
                AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(EnumContractStatus.Cancel));

            var saveResult = _contractCache.Reject(model);

            if (saveResult > 0)
            {
                var saveBy = User.UserName;

                try
                {
                    var saveFile = SaveRefFiles(model.RefFiles, model.ContractId, saveBy, out var errMsg);
                    if (!saveFile)
                    {
                        AppProcessor.Logger.Message(errMsg);
                    }
                }
                catch (Exception ex)
                {
                    AppProcessor.Logger.Error(ex);
                }

                #region Gửi tin nhắn tới khách hàng

                var requestUrl = Request.Url;

                Task.Run(() =>
                {
                    try
                    {
                        var cusInfo = _contractCache.GetCus(model.ContractId);
                        // kiểm tra để gửi email cho khách hàng

                        if (!string.IsNullOrEmpty(cusInfo.Email))
                        {
                            var unionParent = _unionCache.GetById(contractModel.UnionId);
                            Dictionary<string, string> dictUnionInfo = null;
                            var unionInfo = unionParent.UnionInfo;
                            var nameUnion = "";
                            var addressUnion = "";
                            var emailUnion = "";
                            var phoneUnion = "";

                            // Deserialize thông tin từ đối tượng JSON thành một từ điển
                            if (!string.IsNullOrEmpty(unionInfo))
                            {
                                dictUnionInfo = JsonConvert.DeserializeObject<Dictionary<string, string>>(unionInfo);
                            }

                            if (dictUnionInfo != null)
                                foreach (var kvp in dictUnionInfo)
                                {
                                    var key = kvp.Key;
                                    var value = kvp.Value;

                                    switch (key)
                                    {
                                        case "EnterpriseName":
                                            nameUnion = value;
                                            break;
                                        case "Email":
                                            emailUnion = value;
                                            break;
                                        case "Phone":
                                            phoneUnion = value;
                                            break;
                                        case "EnterpriseAddress":
                                            addressUnion = value;
                                            break;
                                    }
                                }

                            var modelNotification = new ContentNotificationModel
                            {
                                TypeEmail = EnumTypeEmail.ContractRejection,
                                ContractInfo = new Contract
                                {
                                    ContractNo = contractModel.ContractNo,
                                    ContractSignal = contractModel.ContractSignal,
                                    ContractNoInfo = contractModel.ContractNoInfo,
                                    SearchContractDetailUrl =
                                        requestUrl.GetLeftPart(UriPartial.Authority),
                                    SearchContractUrl = requestUrl.GetLeftPart(UriPartial.Authority)
                                },
                                CusInfo = new Customer
                                {
                                    CusName = contractModel.CusName,
                                    Email = cusInfo.Email,
                                    Phone = cusInfo.Phone,
                                    TypeCus = cusInfo.TypeCus
                                },
                                UnionInfo = new Union
                                {

                                    UnionName = nameUnion,
                                    Address = addressUnion,
                                    Email = emailUnion,
                                    Phone = phoneUnion
                                }
                            };
                            SendNotificationHelper.Send(modelNotification);
                        }

                        // gửi tin nhắn

                        var isSuccess = SmsProvider.Send(out string msgErr, cusInfo.Phone, EnumContractStatusHandle.Refuse, contractModel.ContractNoInfo);
                        AppProcessor.Logger.Message($"[{contractModel.ContractNoInfo}] - Gửi tin nhắn từ chối hợp đồng tới khách hàng: {(isSuccess ? "Thành công" : $"Thất bại ({msgErr})")}");

                        //SMSProvider.Send_SMS_Contract_Refuse(cusInfo.Phone, contractModel.ContractNoInfo);

                    }
                    catch (Exception ex)
                    {
                        AppProcessor.Logger.Error(ex);
                    }
                });

                #endregion

            }

            var response = CreateMessage($"{_contractTitle} [{contractModel.ContractNo} - {contractModel.CusName}]", EnumProcessType.Edit, saveResult == 0 ? EnumMsgIcon.Error : EnumMsgIcon.Success);
            return Json(new { status = true, message = response }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region View

        [AjaxOnly]
        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        public ActionResult View(Guid? contractId)
        {
            var contractModel = _contractCache.GetById(contractId);
            if (contractModel == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_contractTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            ViewBag.AppViewerUrl = _sysConfigCache.GetViaKey(CONFIG_KEY_OFFICE_APPVIEWER_URL)?.ConfigValue;

            var rdKey = EString.RandomStringNumber(8);
            contractModel.ContractFile = $"{rdKey}.{contractModel.ContractId}";

            return PartialView("_View", contractModel);
        }

        [AjaxOnly]
        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        public ActionResult ViewContract(Guid? contractId)
        {
            var contractModel = _contractCache.GetById(contractId);
            if (contractModel == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_contractTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            //contractModel.ContractTypeEnum = _mappingTypeContractToEnum[contractModel.ContractTypeId ?? 1];
            contractModel.ContractTypeName =
                AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription((EnumContractType)contractModel.ContractTypeId));
            contractModel.CusInfo = _contractCache.GetCus(contractId);
            contractModel.ListPurposes = _purposeCache.GetAll(contractTypeIds: $"{contractModel.ContractTypeId}")
                .OrderBy(p => p.PurPoseName)
                .Select(d => new ListItem(d.PurPoseName, d.PurPoseId.ToString())).ToList();

            contractModel.ListTasks = _contractCache.GetTask(contractId);
            contractModel.ListRefFiles = _docCache.GetByObjectId($"{contractId}");

            contractModel.HasTaxForContract = contractModel.TaxRate > 0;

            contractModel.InfoDiscountContract = string.Format(_infoDiscountContract, contractModel.SubTotal.ToString("#,### đ"), contractModel.Discount.ToString("#,### đ"));

            contractModel.TaxInfo = string.Format(_taxInfoContract, contractModel.TaxRate, contractModel.TaxAmount.ToString("#,### đ"));

            return PartialView("_ViewContract", contractModel);
        }

        #endregion

        #region Payments

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

            return PartialView("_Payments", contractModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult GetPayments(Guid? contractId)
        {
            var draw = Request.Form.GetValues("draw")?[0];

            var dataPayments = _contractCache.GetPayments(contractId);
            var total = dataPayments.Count;

            var result = Json(
                new { draw = Convert.ToInt32(draw), recordsTotal = total, recordsFiltered = total, data = dataPayments },
                JsonRequestBehavior.AllowGet);
            return result;
        }

        [AjaxOnly]
        [ActionType(Type = EnumActionType.Add)]
        [HttpGet]
        public ActionResult AddPayment(Guid? contractId, int typePayment = (int)EnumTypePayment.Advance)
        {
            var contractModel = _contractCache.GetById(contractId);
            if (contractModel == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_contractTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var typeContractModel = _contractTypeCache.GetById(contractModel.ContractTypeId);
            if (typeContractModel == null)
            {
                return Json(new
                {
                    status = true,
                    message = CreateMessage(_contractTypeTitle, EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            }

            var dictPaymentStatus = new Dictionary<int, int>
            {
                { (int)EnumTypePayment.Advance, (int)EnumPaymentStatus.Received },
                { (int)EnumTypePayment.PayOff, (int)EnumPaymentStatus.Received },
                { (int)EnumTypePayment.Refunded, (int)EnumPaymentStatus.Refunded }
            };

            var dataPayments = _contractCache.GetPayments(contractId);

            DataTable dt = new DataTable();
            var discountAmount = (long)Math.Round((decimal)dt.Compute(string.Format(_defaultInvDiscountFormula, contractModel.Total), ""), 0);

            MajorContractPaymentModel paymentModel = new MajorContractPaymentModel
            {
                Ordinal = (dataPayments?.Count > 0 ? dataPayments.Max(p => p.Ordinal) : 0) + 1,
                //TypePayment = (int)((dataPayments?.Count > 0 ? dataPayments.Max(p => p.Ordinal) : 0) + 1),

                TypePayment = typePayment,
                TypePaymentName = AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription((EnumTypePayment)typePayment)),

                Status = dictPaymentStatus[typePayment],
                StatusName = AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription((EnumPaymentStatus)dictPaymentStatus[typePayment])),

                PaymentId = Guid.NewGuid(),
                ContractId = contractId,
                ContractNo = contractModel.ContractNoInfo,
                Total = contractModel.SubTotal,
                RemainingAmount = contractModel.RemainingAmount,
                PaidAmount = typePayment == (int)EnumTypePayment.Refunded ? contractModel.TotalPaidAmount : contractModel.RemainingAmount,
                FormatterPaidAmount = typePayment == (int)EnumTypePayment.Refunded ? $"{contractModel.TotalPaidAmount}" : $"{contractModel.RemainingAmount}",
                TotalPaidAmount = contractModel.TotalPaidAmount,
                LiquidationAmount = contractModel.SubTotal,
                FormatterLiquidationAmount = $"{contractModel.SubTotal}",

                PercentAdvance = typeContractModel.PercentAdvance <= 0 ? 50 : typeContractModel.PercentAdvance,

                HasTaxForContract = contractModel.TaxRate > 0, //contractModel.HasTaxForContract,

                TaxRate = contractModel.TaxRate,
                TaxAmount = contractModel.TaxAmount,
                FormatterTaxAmount = $"{contractModel.TaxAmount}",
                TaxInfo = string.Format(_taxInfoContract, contractModel.TaxRate, contractModel.TaxAmount.ToString("#,### đ")),

                DiscountRate = _defaultInvRateForCalcTax,
                DiscountAmount = discountAmount,
                FormatterDiscountAmount = $"{discountAmount}",
                DiscountFormula = _defaultInvDiscountFormula,
                InfoDiscountContract = string.IsNullOrEmpty(contractModel.InfoDiscountContract) ? string.Format(_infoDiscountContract, contractModel.SubTotal.ToString("N0"), contractModel.Discount.ToString("N0")) : contractModel.InfoDiscountContract,

                Reason = "Thêm mới"
            };

            return PartialView("_AddPayment", paymentModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Add)]
        [ValidateInput(false)]
        public ActionResult AddPayment(MajorContractPaymentModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_Payment", model);
            }

            var detailInfoPayment = (model.HasArising ?
            $"<span class=\"pl-1 text-danger-d1 text-500 d-block\"> - {model.TypeArisingName}: {model.ArisingAmount ?? 0:N0} đ</span>" : string.Empty) + (model.HasTaxForContract ? $"<span class=\"pl-1 text-danger-d1 text-500 d-block\"> ({model.TaxInfo})</span>" : model.DiscountAmount != null && model.DiscountAmount > 0 ? $"<span class=\"pl-1 text-danger-d1 text-500 d-block\"> - Miễn giảm({model.DiscountRate}%): {model.DiscountAmount.Value:N0} đ</span>" : string.Empty);

            var paymentId = _contractCache.SavePayment(new MajorContractPaymentModel
            {
                PaymentId = model.PaymentId,
                ContractId = model.ContractId,
                PaidAmount = model.PaidAmount,
                //RefDocNo = model.RefDocNo,
                PaymentInfo = $"{model.TypePaymentName} {_contractTitle} <b class='text-danger-d1 text-500'>[{model.ContractNo}]</b> với số tiền: <b class='text-danger-d1 text-500'>[{model.PaidAmount:N0} đ]</b> {detailInfoPayment}",
                TypePayment = model.TypePayment,
                TypePaymentName = model.TypePayment <= 0 ? "" : model.TypePaymentName,
                Status = model.PaidAmount < 0 ? (int)EnumPaymentStatus.Refunded : model.Status,
                StatusName = AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(model.PaidAmount < 0 ? EnumPaymentStatus.Refunded : (EnumPaymentStatus)model.Status)),

                //StatusName = model.Status == (int)EnumPaymentStatus.Received ? AppProcessor.Messagor.GetMessage("PaymentStatus_Received") : AppProcessor.Messagor.GetMessage("PaymentStatus_Refunded"),
                PercentAdvance = model.PercentAdvance,
                PaidOn = model.PaidOn,
                PaymentMethod = model.PaymentMethod,
                PaymentMethodName = model.PaymentMethodName,

                TypeArising = !model.HasArising ? null : model.TypeArising,
                TypeArisingName = !model.HasArising ? null : model.TypeArisingName,
                ArisingAmount = !model.HasArising ? null : model.ArisingAmount,

                TaxRate = model.HasTaxForContract ? model.TaxRate : 0,
                TaxAmount = model.TaxAmount,

                DiscountRate = !model.HasTaxForContract ? model.DiscountRate : null,
                DiscountAmount = !model.HasTaxForContract ? model.DiscountAmount : null,

                Note = model.Note,

                Reason = "Thêm mới",
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

            var paymentModel = _contractCache.GetPaymentById(paymentId);
            if (paymentModel == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_contractPaymentTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var realArisingValue = paymentModel.TypeArising == (int)EnumPaymentArising.Decrease ? -1 : 1;

            paymentModel.LiquidationAmount = contractModel.SubTotal + realArisingValue * paymentModel.ArisingAmount;
            paymentModel.TotalPaidAmount = contractModel.TotalPaidAmount;

            paymentModel.HasArising = paymentModel.TypeArising != null;
            paymentModel.FormatterArisingAmount = $"{paymentModel.ArisingAmount}";
            paymentModel.FormatterDiscountAmount = $"{paymentModel.DiscountAmount}";
            paymentModel.FormatterLiquidationAmount = $"{paymentModel.LiquidationAmount}";
            paymentModel.FormatterPaidAmount = $"{paymentModel.PaidAmount}";
            paymentModel.DiscountFormula = _defaultInvDiscountFormula;
            paymentModel.InfoDiscountContract = string.IsNullOrEmpty(contractModel.InfoDiscountContract)
                ? string.Format(_infoDiscountContract,
                    contractModel.SubTotal.ToString("N0"),
                    contractModel.Discount.ToString("N0"))
                : contractModel.InfoDiscountContract;

            paymentModel.FormatterTaxAmount = $"{paymentModel.TaxAmount}";
            paymentModel.TaxInfo = string.Format(_taxInfoContract, contractModel.TaxRate, contractModel.TaxAmount.ToString("N0"));

            paymentModel.HasTaxForContract = contractModel.TaxRate > 0;

            paymentModel.Total = contractModel.SubTotal;

            paymentModel.IsEdit = true;

            return PartialView("_EditPayment", paymentModel);
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
                return PartialView("_Payment", model);
            }
            string response;

            var retSave = _contractCache.SavePayment(new MajorContractPaymentModel
            {
                PaymentId = model.PaymentId,
                ContractId = model.ContractId,
                PaidAmount = model.PaidAmount,
                RefDocNo = model.RefDocNo,
                PaymentInfo = $"{model.TypePaymentName} {model.PaidAmount:N0} {_contractTitle} [{model.ContractNo}]",
                TypePayment = model.TypePayment,
                TypePaymentName = model.TypePaymentName,
                PercentAdvance = model.PercentAdvance,
                PaidOn = model.PaidOn,
                PaymentMethod = model.PaymentMethod,
                PaymentMethodName = model.PaymentMethodName,

                TypeArising = !model.HasArising ? null : model.TypeArising,
                TypeArisingName = !model.HasArising ? null : model.TypeArisingName,
                ArisingAmount = !model.HasArising ? null : model.ArisingAmount,

                DiscountRate = model.DiscountRate,
                DiscountAmount = model.DiscountAmount,

                TaxRate = model.HasTaxForContract ? model.TaxRate : 0,
                TaxAmount = model.TaxAmount,

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
            var paymentModel = _contractCache.GetPaymentById(paymentId);
            if (paymentModel == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_contractPaymentTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Delete"),
                $"<b>{_contractPaymentTitle} [{_contractTitle} {paymentModel.ContractNo}]</b>");
            return PartialView("_DeletePayment", paymentModel);
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
            var retDelete = _contractCache.DeletePayment(model);
            var response = CreateMessage($"<b>{_contractPaymentTitle} [{_contractTitle} {model.ContractNo}]</b>",
                EnumProcessType.Delete, retDelete > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response });
        }

        #endregion

        #region Tasks

        [AjaxOnly]
        [ActionType(Type = EnumActionType.Add)]
        [HttpGet]
        public ActionResult AddTask(Guid contractId, int typeContract)
        {
            var lstContractTasks = Session[$"ContractTasks-{User.UserName}-{contractId}"] as List<MajorContractTaskModel>;
            var lstMainSections = _mainSectionCache.GetAll($"{typeContract}");

            //var functionDiscountContract = string.Format(_functionDiscountContract, totalAmountC);
            //var discountAmount = Math.Round(double.Parse(dataContractTasks.Compute(functionDiscountContract, "").ToString()));

            var model = new MajorContractTaskModel
            {
                TaskId = Guid.NewGuid(),
                ContractId = contractId,
                Ordinal = lstContractTasks == null || lstContractTasks.Count <= 0 ? 1 : lstContractTasks.Max(t => t.Ordinal) + 1,
                //TypeContractEnum = _mappingTypeContractToEnum[typeContract],
                TypeContractId = typeContract,
                TypeContractName = AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription((EnumContractType)typeContract)),
                ListTypeLands = lstMainSections.OrderBy(ms => ms.MainSectionId).Select(p => new ListItem
                {
                    Text = p.MainSectionName,
                    Value = $"{p.MainSectionId}"
                }).ToList(),
                ListContentLands = _contentLandCache.GetAll($"{typeContract}").Select(p => new ListItem
                {
                    Text = p.ContentLandName,
                    Value = $"{p.ContentLandId}"
                }).ToList(),
                ListContents = lstMainSections
                    //.Where(p => lstContractTasks == null || lstContractTasks.Count <= 0 || !lstContractTasks.Exists(t => t.Contents == p.MainSectionName))
                    .Select(p => new ListItem
                    {
                        Text = p.MainSectionName,
                        Value = $"{p.MainSectionId}"
                    }).ToList()
            };
            return PartialView("_AddTask", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Add)]
        [ValidateInput(false)]
        public ActionResult AddTask(MajorContractTaskModel model)
        {
            var lstContractTasks = Session[$"ContractTasks-{User.UserName}-{model.ContractId}"] as List<MajorContractTaskModel>;

            if (!ModelState.IsValid)
            {
                var lstMainSections = _mainSectionCache.GetAll($"{model.TypeContractId}");

                //model.TypeContractEnum = _mappingTypeContractToEnum[model.TypeContractId ?? 1];

                model.ListTypeLands = lstMainSections.OrderBy(ms => ms.MainSectionId).Select(p => new ListItem
                {
                    Text = p.MainSectionName,
                    Value = $"{p.MainSectionId}"
                }).ToList();
                model.ListContentLands = _contentLandCache.GetAll($"{model.TypeContractId}").Select(p => new ListItem
                {
                    Text = p.ContentLandName,
                    Value = $"{p.ContentLandId}"
                }).ToList();
                model.ListContents = lstMainSections
                    //.Where(p => lstContractTasks == null || lstContractTasks.Count <= 0 || !lstContractTasks.Exists(t => t.Contents == p.MainSectionName))
                    .Select(p => new ListItem
                    {
                        Text = p.MainSectionName,
                        Value = $"{p.MainSectionId}"
                    }).ToList();
                return PartialView("_Task", model);
            }

            var clone = (CultureInfo)CultureInfo.InvariantCulture.Clone();
            clone.NumberFormat.CurrencySymbol = "";

            model.FormattedPrice = model.Price.ToString("C0", clone);
            model.FormattedAmount = model.Amount.ToString("C0", clone);

            lstContractTasks = lstContractTasks ?? new List<MajorContractTaskModel>();
            lstContractTasks.Add(model);

            Session[$"ContractTasks-{User.UserName}-{model.ContractId}"] = lstContractTasks;

            var jsonTasks = JsonConvert.SerializeObject(lstContractTasks);
            var response = CreateMessage($"{_contractTaskTitle}", EnumProcessType.Add, EnumMsgIcon.Success);
            return Json(new { status = true, message = response, data = jsonTasks }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult EditTask(Guid? taskId, Guid? contractId)
        {
            var lstContractTasks = Session[$"ContractTasks-{User.UserName}-{contractId}"] as List<MajorContractTaskModel>;
            lstContractTasks = lstContractTasks ?? new List<MajorContractTaskModel>();

            var contractTask = lstContractTasks.FirstOrDefault(t => t.TaskId == taskId);
            if (contractTask == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_contractTaskTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var lstMainSections = _mainSectionCache.GetAll($"{contractTask.TypeContractId}");

            contractTask.ListTypeLands = lstMainSections.Select(p => new ListItem
            {
                Text = p.MainSectionName,
                Value = $"{p.MainSectionId}"
            }).ToList();
            contractTask.ListContentLands = _contentLandCache.GetAll($"{contractTask.TypeContractId}").Select(p => new ListItem
            {
                Text = p.ContentLandName,
                Value = $"{p.ContentLandId}"
            }).ToList();
            contractTask.ListContents = lstMainSections
                //.Where(p => lstContractTasks == null || lstContractTasks.Count <= 0 || !lstContractTasks.Exists(t => t.Contents == p.MainSectionName))
                .Select(p => new ListItem
                {
                    Text = p.MainSectionName,
                    Value = $"{p.MainSectionId}"
                }).ToList();
            contractTask.IsEdit = true;
            //contractTask.TypeContractEnum = _mappingTypeContractToEnum[contractTask.TypeContractId ?? 1];

            return PartialView("_EditTask", contractTask);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Add)]
        [ValidateInput(false)]
        public ActionResult EditTask(MajorContractTaskModel model)
        {
            var lstContractTasks = Session[$"ContractTasks-{User.UserName}-{model.ContractId}"] as List<MajorContractTaskModel>;

            if (!ModelState.IsValid)
            {
                var lstMainSections = _mainSectionCache.GetAll($"{model.TypeContractId}");
                model.ListTypeLands = lstMainSections.Select(p => new ListItem
                {
                    Text = p.MainSectionName,
                    Value = $"{p.MainSectionId}"
                }).ToList();
                model.ListContentLands = _contentLandCache.GetAll($"{model.TypeContractId}").Select(p => new ListItem
                {
                    Text = p.ContentLandName,
                    Value = $"{p.ContentLandId}"
                }).ToList();
                model.ListContents = lstMainSections
                    //.Where(p => lstContractTasks == null || lstContractTasks.Count <= 0 || !lstContractTasks.Exists(t => t.Contents == p.MainSectionName))
                    .Select(p => new ListItem
                    {
                        Text = p.MainSectionName,
                        Value = $"{p.MainSectionId}"
                    }).ToList();
                model.IsEdit = true;
                //model.TypeContractEnum = _mappingTypeContractToEnum[model.TypeContractId ?? 1];

                return PartialView("_Task", model);
            }

            if (lstContractTasks == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_contractTaskTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var contractTask = lstContractTasks.FirstOrDefault(t => t.TaskId == model.TaskId);
            if (contractTask == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_contractTaskTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var clone = (CultureInfo)CultureInfo.InvariantCulture.Clone();
            clone.NumberFormat.CurrencySymbol = "";

            contractTask.Contents = model.Contents;
            contractTask.SectionId = model.SectionId;
            contractTask.Area = model.Area;
            contractTask.Unit = model.Unit;
            contractTask.Price = model.Price;
            contractTask.FormattedPrice = model.Price.ToString("C0", clone);
            contractTask.Amount = model.Amount;
            contractTask.FormattedAmount = model.Amount.ToString("C0", clone);
            contractTask.LandCalculationId = model.LandCalculationId;
            contractTask.Rate = model.Rate;
            contractTask.RateFormula = model.RateFormula;
            contractTask.Total = model.Total;
            contractTask.UpdatedBy = User.UserName;

            Session[$"ContractTasks-{User.UserName}-{model.ContractId}"] = lstContractTasks;

            var jsonTasks = JsonConvert.SerializeObject(lstContractTasks);
            var response = CreateMessage($"{_contractTaskTitle}", EnumProcessType.Edit, EnumMsgIcon.Success);
            return Json(new { status = true, message = response, data = jsonTasks }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult DeleteTask(Guid? taskId, Guid? contractId)
        {
            var lstContractTasks = Session[$"ContractTasks-{User.UserName}-{contractId}"] as List<MajorContractTaskModel>;
            lstContractTasks = lstContractTasks ?? new List<MajorContractTaskModel>();

            var contractTask = lstContractTasks.FirstOrDefault(t => t.TaskId == taskId);
            if (contractTask == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_contractTaskTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Delete"),
                $"<b>{_contractTaskTitle} [{contractTask.Contents}]</b>");

            return PartialView("_DeleteTask", contractTask);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult DeleteTask(MajorContractTaskModel model)
        {
            ModelState.Remove("ContentId");
            ModelState.Remove("Contents");
            ModelState.Remove("SubSectionId");
            ModelState.Remove("SubSectionName");

            if (!ModelState.IsValid)
            {
                ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Delete"),
                    $"<b>{_contractTaskTitle} [{model.Contents}]</b>");
                return PartialView("_DeleteTaskBody", model);
            }

            var lstContractTasks = Session[$"ContractTasks-{User.UserName}-{model.ContractId}"] as List<MajorContractTaskModel>;
            if (lstContractTasks == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_contractTaskTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            if (!lstContractTasks.Exists(t => t.TaskId == model.TaskId))
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_contractTaskTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            var deleted = lstContractTasks.RemoveAll(t => t.TaskId == model.TaskId);
            int idx = 1;
            lstContractTasks.OrderBy(t => t.Ordinal).ToList().ForEach(t =>
            {
                t.Ordinal = idx;
                idx += 1;
            });

            if (deleted > -1)
            {
                Session[$"ContractTasks-{User.UserName}-{model.ContractId}"] = lstContractTasks;
            }
            var jsonTasks = JsonConvert.SerializeObject(lstContractTasks);

            var response = CreateMessage($"{_contractTaskTitle} [{model.Contents}]",
                EnumProcessType.Delete, deleted > -1 ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response, data = jsonTasks });
        }

        #endregion

        #region Extends Function Ajax

        [AjaxOnly]
        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        public ActionResult Consumer()
        {
            var model = new MajorContractModel
            {
                CusInfo = new MajorContractCustomerModel
                {
                    ListProvinces = _provinceCache.GetAll()
                    .OrderBy(d => d.ProvinceName)
                    .Select(d => new ListItem(d.ProvinceName, d.ProvinceId.ToString())).ToList()
                }
            };
            return PartialView("_Consumer", model);
        }

        [AjaxOnly]
        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        public ActionResult Business()
        {
            var model = new MajorContractModel
            {
                CusInfo = new MajorContractCustomerModel
                {
                    ListProvinces = _provinceCache.GetAll()
                    .OrderBy(d => d.ProvinceName)
                    .Select(d => new ListItem(d.ProvinceName, d.ProvinceId.ToString())).ToList()
                }
            };
            return PartialView("_Business", model);
        }

        [AjaxOnly]
        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        public ActionResult Address(string parentId, int? provinceId, int? wardId, string streetName, string addressNo, int eleViews = 32)
        {
            var provinceModel = _provinceCache.GetByCode(_defaultProvinceCode);

            var model = new CateAddressModel
            {
                ParentId = parentId,
                ProvinceId = provinceId == null || provinceId == 0 ? provinceModel?.ProvinceId : provinceId,
                WardId = wardId,
                StreetName = streetName,
                AddressNo = addressNo,
                EleViews = eleViews,
                ListProvinces = _provinceCache.GetAll()
                    .OrderBy(d => d.ProvinceName)
                    .Select(d => new ListItem(d.ProvinceName, d.ProvinceId.ToString())).ToList()
            };
            return PartialView("_Address", model);
        }

        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        [AjaxOnly]
        public ActionResult CheckLandMap(string mapNo, string landParcelNo, int? provinceId, int? wardId)
        {
            var lstContracts = _contractCache.CheckLandMap(mapNo, landParcelNo, provinceId, wardId);
            if (lstContracts?.Count > 0)
                return Json(new { status = true, data = lstContracts });
            return Json(new { status = false, data = new List<MajorContractModel>() });
        }

        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        [AjaxOnly]
        public ActionResult CalcReturnResult(DateTime? fromDate, double? handleTime)
        {
            var giveResultOn = CalculateReturnDate(fromDate ?? DateTime.Now, handleTime ?? 0);

            return Json(new { status = true, data = giveResultOn.ToString("dd/MM/yyyy") });
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.View)]
        public JsonResult GetAreasViaTypeLand(Guid contractId, int? typeLandId, bool? isEdit = false)
        {
            var lstContractTasks = Session[$"ContractTasks-{User.UserName}-{contractId}"] as List<MajorContractTaskModel>;

            var lstAreas = _subSectionCache.GetAll(typeLandId)
            .Where(p => lstContractTasks == null || lstContractTasks.Count <= 0 || !lstContractTasks.Exists(t => t.SubSectionId == p.SubSectionId) || isEdit == true);
            var dicAreas = new Dictionary<string, List<CateSubSectionModel>>();

            lstAreas.GroupBy(d => d.MainSectionName).ToList().ForEach(g => { dicAreas.Add(g.Key, g.OrderBy(a => a.SubSectionName).ToList()); });

            return Json(new { Areas = dicAreas }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Render Contract

        [HttpGet]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult ShowRender(Guid? contractId, bool? includeQRCode, string fileType = ".pdf")
        {
            var contractModel = _contractCache.GetById(contractId);
            if (contractModel == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_contractTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            ViewBag.AppViewerUrl = _sysConfigCache.GetViaKey(CONFIG_KEY_OFFICE_APPVIEWER_URL)?.ConfigValue;

            var rdKey = EString.RandomStringNumber(8);
            contractModel.RenderContractId = $"{rdKey}.{contractId}.{contractModel.ContractNoInfo}.{(includeQRCode ?? false ? 1 : 0)}{fileType}";

            contractModel.IncludeQRCode = includeQRCode ?? false;
            contractModel.FileType = fileType;

            return PartialView("_RenderContract", contractModel);
        }

        //[AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.View)]
        [AllowAnonymous]
        public async Task<ActionResult> RenderContract(string renderId)
        {
            var arrParrams = renderId.Split('.');
            var contractId = Guid.Parse(arrParrams[1]);
            var contractNoInfo = arrParrams[2];
            bool? includeQRCode = arrParrams[3] == "1";
            var fileType = $".{arrParrams[4]}" ?? ".docx";

            var urlViewContract = Url.Action("QRContract", "Home", new { area = "", enContractId = SecurityHelper.EncryptId(contractId) }, Request.Url.Scheme);

            FERenderContractModel rendercontract = _fEContractCache.GetDataRenderContract(contractId);
            var jsondata = rendercontract.JsonContractInfo;

            // Parse jsondata thành JObject
            var jsonObject = JObject.Parse(jsondata);

            var percentValue = _sysConfigCache.GetViaKey(CONFIG_KEY_TEMPLATE_BY_PERCENT_VALUE);
            // Chuyển đổi ConfigValue thành một đối tượng JSON
            var percentJson = JsonConvert.DeserializeObject<Dictionary<string, object>>(percentValue.ConfigValue);

            // Lấy key và value từ JSON
            var percentKey = percentJson["PERCENT_KEY"].ToString();
            var durationKey = percentJson["DURATION_KEY"].ToString();
            var percentValueContent = percentJson["PERCENT_VALUE"].ToString();
            var contentValue = percentJson["CONTENT_VALUE"].ToString();

            // Thay đổi văn bản nếu `phanTram_TamUng` là "0%"
            if (jsonObject.ContainsKey(percentKey) && jsonObject[percentKey]?.ToString() == percentValueContent)
            {
                jsonObject[percentKey] = contentValue;
            }
            else
            {
                // Lấy mẫu TEMPLATE_NAME dựa vào loại hợp đồng & loại khách hàng

                if (percentJson.TryGetValue(rendercontract.ContractTypeCus, out var dataViaTypeCus))
                {
                    if (dataViaTypeCus != null)
                    {
                        if (dataViaTypeCus.GetType() == typeof(JObject))
                        {
                            var templateContent = ((JObject)dataViaTypeCus).Value<string>(rendercontract.ContractTypeCode);
                            jsonObject[percentKey] = string.Format(templateContent, jsonObject[percentKey], jsonObject[durationKey]);
                        }
                    }
                }

                // Duyệt qua các mẫu TEMPLATE_NAME và thay thế nội dung tương ứng
                //for (int i = 1; i <= 6; i++)
                //{
                //    var templateKey = i.ToString();

                //    // Truy cập vào từng mẫu bằng cách chuyển đổi giá trị từ object thành Dictionary
                //    var templateJson = JsonConvert.DeserializeObject<Dictionary<string, string>>(percentJson[templateKey].ToString());

                //    var templateName = templateJson["TEMPLATE_NAME"];
                //    var templateContent = templateJson["CONTENT"];

                //    if (rendercontract.TemplatePath.Contains(templateName))
                //    {
                //        // Thay thế các tham số trong nội dung
                //        jsonObject[percentKey] = string.Format(templateContent, jsonObject[percentKey], jsonObject[durationKey]);
                //        break;
                //    }
                //}
            }

            // Chuyển đổi lại thành chuỗi JSON
            jsondata = jsonObject.ToString();

            //byte[] fileBytes = RenderContractProvider.RenderModelToPdfAndSave(rendercontract.TemplatePath, jsondata, rendercontract.IndexTabel, rendercontract.IndexRowInTable, urlViewContract);

            byte[] fileBytes = RenderContractProvider.RenderContract(rendercontract.TemplatePath, jsondata, rendercontract.IndexTabel, rendercontract.IndexRowInTable, (bool)includeQRCode, urlViewContract, fileType);

            // Trả về file byte như là một file để tải xuống
            return File(fileBytes, ConstMIMEType.OfficeMIMETypes[fileType], $"{contractNoInfo}{fileType}");
        }
        #endregion

        #region Extend Function

        private bool SaveRefFiles(List<HttpPostedFileBase> refFiles, Guid? contractId, string saveBy, out string errMsg)
        {
            errMsg = string.Empty;

            if (refFiles == null || refFiles.Count <= 0 || refFiles[0] == null)
            {
                errMsg = "Không có tệp đính kèm";
                return false;
            }

            var lstDocContracts = new List<CateDocModel>();

            var refContractsFolderPath = $"{_refContractDocsFolderPath}/{_contractFolderName}/{contractId}";
            var refContractsFolderAbsolutePath = Server.MapPath(refContractsFolderPath);

            if (!Directory.Exists(refContractsFolderAbsolutePath))
                Directory.CreateDirectory(refContractsFolderAbsolutePath);

            foreach (var refFile in refFiles)
            {
                Image image = null;
                if (ConstMIMEType.IsImage(refFile.ContentType))
                {
                    image = Image.FromStream(refFile.InputStream);
                }

                var cateDoc = new CateDocModel
                {
                    FileId = Guid.NewGuid(),
                    TypeObject = "Major_Contracts",
                    FilePath = refContractsFolderPath,
                    FileName = Path.GetFileNameWithoutExtension(refFile.FileName),
                    FileExt = Path.GetExtension(refFile.FileName),
                    ContentType = refFile.ContentType,
                    Dimensions = image != null ? $"{image.Width}x{image.Height}" : null
                };

                lstDocContracts.Add(cateDoc);
            }

            if (lstDocContracts.Count <= 0)
            {
                errMsg = "Không có tệp đính kèm";
                return false;
            }
            var tableRefFiles = CreateTableRefFiles(lstDocContracts);

            var retSaveFile = _contractCache.SaveRefFiles(new MajorContractModel { ContractId = contractId, TableRefFiles = tableRefFiles, UpdatedBy = saveBy });

            if (retSaveFile > 0)
            {
                refFiles.ForEach(refFile =>
                {
                    var cateDoc = lstDocContracts.FirstOrDefault(c =>
                        c.FileName == Path.GetFileNameWithoutExtension(refFile.FileName) &&
                        c.FileExt == Path.GetExtension(refFile.FileName) && c.ContentType == refFile.ContentType);
                    if (cateDoc != null)
                    {
                        refFile.SaveAs(Path.Combine(refContractsFolderAbsolutePath, $"{cateDoc.FileId.ToString().ToUpper()}{cateDoc.FileExt}"));
                    }
                });
            }

            return retSaveFile > 0;
        }

        private DataTable CreateTableRefFiles(List<CateDocModel> lstDocs)
        {
            var dataRefImgs = new DataTable();
            using (var reader = ObjectReader.Create(lstDocs, "FileId", "TypeObject", "FilePath", "FileName", "FileExt", "ContentType", "Dimensions", "Version"))
            {
                dataRefImgs.Load(reader);
            }

            return dataRefImgs;
        }

        #endregion

        #region Function tính ngày hẹn trả

        private DateTime CalculateReturnDate(DateTime startDate, double waitingDays)
        {
            double iWaitingDays = waitingDays;

            if (iWaitingDays > 6)
            {
                iWaitingDays += iWaitingDays / 6 * 2;
            }

            var totalWaitingDays = (int)iWaitingDays + (iWaitingDays % (int)iWaitingDays > 0 ? 1 : 0);

            DateTime giveResultOnDate = startDate;
            var lstHolidays = _holidayCache.GetAll(fromDate: startDate, toDate: startDate.AddDays(totalWaitingDays));

            while (waitingDays > 0)
            {
                giveResultOnDate = giveResultOnDate.AddDays(1);
                if (giveResultOnDate.DayOfWeek != DayOfWeek.Saturday &&
                    giveResultOnDate.DayOfWeek != DayOfWeek.Sunday &&
                    !lstHolidays.Exists(h => h.RealDate.Subtract(giveResultOnDate).Days == 0))
                {
                    waitingDays -= 1;
                }
            }

            //if (waitingDays > 6)
            //{
            //    waitingDays += (waitingDays / 6) * 2;
            //}

            //var totalWaitingDays = (int)waitingDays + (waitingDays % (int)waitingDays > 0 ? 1 : 0);

            //var lstDate = Enumerable.Range(1, totalWaitingDays)
            //    .Select(offset => startDate.AddDays(offset))
            //    .Where(d => d.DayOfWeek != DayOfWeek.Saturday && d.DayOfWeek != DayOfWeek.Sunday && !lstHolidays.Exists(h => h.RealDate.Subtract(d).Days == 0))
            //    .ToList();

            //var giveResultOnDate = lstDate.Max();

            //// Tăng dần ngày lên và kiểm tra từng ngày
            //for (int i = 0; i < days;)
            //{
            //    returnDate = returnDate.AddDays(1);
            //    // Kiểm tra nếu ngày hẹn trả rơi vào ngày nghỉ hoặc trong danh sách ngày nghỉ thì dời sang ngày làm việc tiếp theo
            //    if (!holidayList.Contains(returnDate.Date))
            //    {
            //        i++; // Chỉ tăng số ngày nếu không phải là ngày nghỉ
            //    }
            //}

            //// Thêm số giờ và phút vào ngày hẹn trả
            //returnDate = returnDate.AddHours(hours);
            //returnDate = returnDate.AddMinutes(minutes);

            return giveResultOnDate;
        }

        #endregion

        #region Find Customer

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult FindCustomer(string typeCus, string prefix = "CusInfo")
        {
            var searchModel = new SearchCustomerModel
            {
                TypeCus = typeCus,
                PrefixCus = string.IsNullOrEmpty(prefix) ? null : $"{prefix}."
            };
            return PartialView("_FindCus", searchModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult GetListCustomer(SearchCustomerModel searchModel)
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

        #endregion

        #region Create Invoice

        private readonly string _publishInvTitle = AppProcessor.Messagor.GetMessage("PublishInv_Title");
        private readonly string _invTitle = AppProcessor.Messagor.GetMessage("Invoice_Title");

        private readonly string _invPatternTitle = AppProcessor.Messagor.GetMessage("InvPattern_Title");


        [AjaxOnly]
        [ActionType(Type = EnumActionType.Add)]
        [HttpGet]
        public ActionResult PublishInv(Guid contractId)
        {
            var eInvAcc = _invAccCache.GetByUserName(User.UserName);

            if (eInvAcc == null)
            {
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{AppProcessor.Messagor.GetMessage("Err_InvAccount_Empty")}", EnumProcessType.NonFormat, EnumMsgIcon.Error)
                });
            }

            var contractModel = _contractCache.GetById(contractId);
            if (contractModel == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_contractTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            contractModel.CusInfo = _contractCache.GetCus(contractId);
            contractModel.HasTaxForContract = contractModel.TaxRate > 0;

            var lstInvPatterns = _invPatternCache.GetAll();
            if (lstInvPatterns == null || lstInvPatterns.Count <= 0)
            {
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_invPatternTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            }
            var usingInvPattern = lstInvPatterns.First(p => p.IsActive);

            if (!contractModel.HasTaxForContract)
            {
                usingInvPattern = lstInvPatterns.FirstOrDefault(p => $"{p.Pattern}-{p.Serial}" == _invPatternForDiscountContract);
                if (string.IsNullOrEmpty(_invPatternForDiscountContract) || usingInvPattern == null)
                {
                    return Json(new
                    {
                        status = true,
                        message = CreateMessage($"{_invPatternTitle} cho Hợp đồng miễn giảm chưa được cấu hình",
                            EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                    });
                }
            }

            var sTemplateInvPath = HostingEnvironment.MapPath(Path.Combine(_invTemplateFolderPath, $"{(contractModel.HasTaxForContract ? "New_" : "")}ViewInvTemplate.xslt")) ?? string.Empty;
            if (!System.IO.File.Exists(sTemplateInvPath))
            {
                return Json(new
                {
                    status = true,
                    message = CreateMessage("Tệp template không tồn tại", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            }

            var invInfo = GenInvInfo(contractModel);
            var xmlInvInfo = XmlHelper.SerializeToString(invInfo);
            var dataXmlInvoice = GeneralXMLInvoiceView(xmlInvInfo, usingInvPattern.Pattern, usingInvPattern.Serial);

            var publishInv = new PublishInvModel
            {
                ContractId = contractModel.ContractId,
                ContractNoInfo = contractModel.ContractNoInfo,
                PatternId = usingInvPattern.PatternId,
                Pattern = usingInvPattern.Pattern,
                Serial = usingInvPattern.Serial,
                DataInvHtmlView = dataXmlInvoice,
                TemplateInvViewPath = sTemplateInvPath
            };

            return PartialView("_PublishInv", publishInv);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Add)]
        [ValidateInput(false)]
        public ActionResult PublishInv(PublishInvModel model)
        {
            var contractModel = _contractCache.GetById(model.ContractId);
            if (contractModel == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_contractTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            if (contractModel.HasInv)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_contractTitle} [{contractModel.ContractNoInfo}] đã tồn tại hoá đơn", EnumProcessType.NonFormat, EnumMsgIcon.Error)
                });

            var lstInvPatterns = _invPatternCache.GetAll();
            var usingInvPattern = lstInvPatterns.First(p => p.IsActive);
            if (!contractModel.HasTaxForContract)
            {
                usingInvPattern = lstInvPatterns.FirstOrDefault(p => $"{p.Pattern}-{p.Serial}" == _invPatternForDiscountContract);
                if (string.IsNullOrEmpty(_invPatternForDiscountContract) || usingInvPattern == null)
                {
                    return Json(new
                    {
                        status = true,
                        message = CreateMessage($"{_invPatternTitle} cho Hợp đồng miễn giảm chưa được cấu hình", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                    });
                }
            }

            var cusInfo = _contractCache.GetCus(model.ContractId);

            #region Insert Inv

            decimal taxRate = contractModel.TaxRate;

            contractModel.HasTaxForContract = taxRate > 0;
            DataTable dt = new DataTable();

            decimal taxAmount;
            decimal total = contractModel.LiquidationAmount;
            decimal amount = total;// = taxAmount + total;
            decimal discountAmount = 0;// = (long)Math.Round((decimal)dt.Compute(string.Format(_defaultInvDiscountFormula, amount), ""), 0);

            if (contractModel.HasTaxForContract)
            {
                total = Math.Round(amount * 100 / (100 + taxRate), 0);
                taxAmount = Math.Round(total * taxRate / 100, 0);
            }
            else
            {
                taxAmount = contractModel.LiquidationAmount * taxRate / 100;
                amount = taxAmount + total;
                discountAmount = (long)Math.Round((decimal)dt.Compute(string.Format(_defaultInvDiscountFormula, amount), ""), 0);
                amount -= discountAmount;
            }


            //var total = contractModel.LiquidationAmount;
            //var taxAmount = contractModel.LiquidationAmount * _defaultInvTaxRate / 100;
            //var amount = taxAmount + total;

            //var discountAmount = (long)Math.Round((decimal)dt.Compute(string.Format(_defaultInvDiscountFormula, amount), ""), 0);
            //amount -= discountAmount;

            var lstPayments = _contractCache.GetPayments(contractModel.ContractId);
            var payOffPayment = lstPayments.FirstOrDefault(p => p.TypePayment == (int)EnumTypePayment.PayOff);

            #region Cus Info

            var dataCusInfo = new DataTable();

            MajorInvCusModel invCusInfo = new MajorInvCusModel
            {
                CusCode = cusInfo.CusCode,
                CusName = cusInfo.CusName,
                Buyer = cusInfo.TypeCus == ConstsCusType.BUSINESS ? string.Empty : cusInfo.CusName,
                TypeCus = cusInfo.TypeCus,
                TypeCusName = cusInfo.TypeCus == ConstsCusType.BUSINESS ? AppProcessor.Messagor.GetMessage("CusType_Business") : AppProcessor.Messagor.GetMessage("CusType_Consumer"),
                CusTaxCode = cusInfo.TaxCode,
                CusPhone = cusInfo.Phone,
                CusIdentifierNo = cusInfo.IdentifierNo,
                CusAddress = cusInfo.Address,
                CusBankNo = string.Empty,
                CusBankName = string.Empty
            };

            using (var reader = ObjectReader.Create(new List<MajorInvCusModel> { invCusInfo }, "CusCode", "CusName", "Buyer", "TypeCus", "TypeCusName", "CusTaxCode", "CusPhone", "CusAddress", "CusBankNo", "CusBankName"))
            {
                dataCusInfo.Load(reader);
            }

            #endregion

            #region Product Info

            var dataInvProduct = new DataTable();

            var invProduct = new MajorInvProductModel
            {
                ProductCode = invCusInfo.CusCode,
                ProductName = string.Format(_defaultInvProdName, contractModel.ContractNoInfo, contractModel.ConfirmOn?.ToString("dd/MM/yyyy")),
                //ProductUnit = null,
                //ProductQuantity = null,

                ProductPrice = (long)total,
                Amount = (long)total,
                TaxRate = (double)taxRate,
                Issum = (int)EnumInvProductType.Product,
                IssumName = EnumHelper.GetDescription(EnumInvProductType.Product),
            };

            using (var reader = ObjectReader.Create(new List<MajorInvProductModel> { invProduct }, "ProductCode", "ProductName", "ProductUnit", "ProductQuantity", "ProductPrice", "TaxRate", "Amount", "Issum", "IssumName"))
            {
                dataInvProduct.Load(reader);
            }

            #endregion

            var createInvBy = User.UserName;
            var userInfo = _invAccCache.GetByUserName(createInvBy);

            var invModel = new MajorInvModel
            {
                ContractId = contractModel.ContractId,
                InvKey = InvHelper.GenFKey(),
                Pattern = usingInvPattern.Pattern,
                Serial = usingInvPattern.Serial,
                InvType = (int)EnumInvType.InvoiceTypeNormal,
                InvTypeName = EnumHelper.GetDescription(EnumInvType.InvoiceTypeNormal),
                InvStatus = (int)EnumInvStatus.InvoiceJustCreated,
                InvStatusName = EnumHelper.GetDescription(EnumInvStatus.InvoiceJustCreated),
                Note = string.Empty,
                TaxRate = (double)taxRate,
                TaxAmount = (long)taxAmount,
                DiscountAmount = (long)discountAmount,
                Amount = (long)amount,
                AmountInWord = $"{NumberHelper.UpperCaseFirst(NumberHelper.NumberToVietnamese(amount))} đồng",
                CurrencyUnit = nameof(EnumInvCurrencyUnit.VND),
                PaymentMethod = payOffPayment?.PaymentMethod == (int)EnumPaymentMethod.Cash ? EnumHelper.GetDescription(EnumInvPaymentMethob.Cash) : EnumHelper.GetDescription(EnumInvPaymentMethob.Transfer),

                Total = (long)total,

                KindOfService = contractModel.ContractNoInfo,

                InvCusInfo = invCusInfo,
                DataInvCus = dataCusInfo,
                InvProductInfo = invProduct,
                DataInvProduct = dataInvProduct,

                PaidOn = payOffPayment?.PaidOn ?? DateTime.Now,
                ConfirmPaidBy = User.UserName,
                PublishBy = userInfo.ElnvAccount,

                UpdatedBy = User.UserName
            };

            var retInsertInv = _invCache.Save(invModel);
            string response;

            if (retInsertInv == 0)
            {
                response = CreateMessage($"{_invTitle} - {_contractTitle} [{model.ContractNoInfo}]", EnumProcessType.Add, EnumMsgIcon.Error);
                return Json(new { status = true, message = response }, JsonRequestBehavior.AllowGet);
            }

            if (retInsertInv == -9)
            {
                response = CreateMessage($"{_invTitle} - {_contractTitle} [{invModel.InvKey}]", EnumProcessType.DataExisted, EnumMsgIcon.Error);
                return Json(new { status = true, message = response }, JsonRequestBehavior.AllowGet);
            }

            #endregion

            #region Calc Service Publish Inv

            Task.Run(() =>
            {
                try
                {
                    InvCustomerModel invCus = new InvCustomerModel
                    {
                        Code = invCusInfo.CusCode,
                        Name = invCusInfo.CusName,
                        Address = invCusInfo.CusAddress,
                        Phone = invCusInfo.CusPhone,
                        TaxCode = invCusInfo.CusTaxCode,
                        Email = cusInfo.Email,
                        RepresentPerson = cusInfo.TypeCus == ConstsCusType.BUSINESS ? cusInfo.RepresenterName : null,
                        CusType = cusInfo.TypeCus == ConstsCusType.BUSINESS ? "1" : "0"
                    };

                    var invInfo = GenInvInfo(invModel);

                    {
                        string sInvAccName = userInfo.ElnvAccount;
                        string sInvAccPass = userInfo.ElnvACPassword;

                        _invProvider.CreateInvoice(out var errMsg, invCus, invInfo, sInvAccName, sInvAccPass, _invServiceAccName, _invServiceAccPass, usingInvPattern.Pattern, usingInvPattern.Serial, createInvBy);

                        if (!string.IsNullOrEmpty(errMsg))
                        {
                            AppProcessor.Logger.Message(
                                $"{AppProcessor.Messagor.GetMessage("PublishInv_Title")} - {AppProcessor.Messagor.GetMessage("Contract_Title")} [{contractModel.ContractNoInfo}] thất bại: {errMsg}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    AppProcessor.Logger.Error(ex);

                    #region Xử lý rollback hoá đơn khi phát hành lỗi + thông báo người dùng

                    #region Rollback Inv

                    var retRollback = _invCache.Rollback(new InvStatusModel
                    {
                        InvKey = invModel.InvKey,
                        Reason = "Lỗi gọi service phát hành hoá đơn",
                        SavedBy = createInvBy
                    });
                    if (retRollback <= 0)
                    {
                        StringBuilder logBuilder = new StringBuilder();
                        logBuilder.AppendLine();

                        logBuilder.AppendLine($"Contract - PublishInv - {model.ContractNoInfo} - InvKey [{invModel.InvKey}]");
                        logBuilder.AppendLine($"        => Rollback Error: {retRollback}");

                        AppProcessor.Logger.Message(logBuilder.ToString());
                    }

                    #endregion

                    AppProcessor.Notifider.PushNotifyToUser("System", createInvBy, $"Phát hành hoá đơn cho {_contractTitle} [{model.ContractNoInfo}] thất bại. Vui lòng kiểm tra lại");

                    #endregion
                }
            });

            #endregion

            response = CreateMessage($"Gửi yêu cầu {_publishInvTitle} - {AppProcessor.Messagor.GetMessage("Contract_Title")} [{contractModel.ContractNoInfo}] thành công", EnumProcessType.NonFormat, EnumMsgIcon.Success);

            return Json(new { status = true, message = response }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        public ActionResult ViewInv(Guid contractId)
        {
            var invModel = _invCache.GetByContractId(contractId);
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
                InvId = invModel.InvId,
                InvNo = invModel.InvNo,
                Pattern = invModel.Pattern,
                Serial = invModel.Serial,
                InvKey = invModel.InvKey,
                HtmlInv = invViewModel.InvView
            };

            return PartialView("_ViewInv", viewInv);
        }

        #region Extend Functions

        private InvInv GenInvInfo(MajorInvModel inv)
        {
            InvInv invInfo = new InvInv
            {
                ContractId = inv.ContractId,
                FKey = inv.InvKey,
                Invoice = new InvInvoice
                {
                    //Pattern = inv.Pattern,
                    //Serial = inv.Serial,

                    Buyer = inv.InvCusInfo.TypeCus == ConstsCusType.BUSINESS ? string.Empty : inv.InvCusInfo.CusName,
                    CusName = inv.InvCusInfo.TypeCus == ConstsCusType.BUSINESS ? inv.InvCusInfo.CusName : string.Empty,
                    CusAddress = inv.InvCusInfo.CusAddress,
                    CusCode = inv.InvCusInfo.CusCode,
                    CusPhone = inv.InvCusInfo.CusPhone,
                    CusTaxCode = inv.InvCusInfo.CusTaxCode,

                    CusType = inv.InvCusInfo.TypeCus == ConstsCusType.BUSINESS ? "1" : "0",

                    CCCDan = inv.InvCusInfo.CusIdentifierNo,

                    IsPayed = true,

                    CurrencyUnit = inv.CurrencyUnit,
                    PaymentMethod = inv.PaymentMethod,//EnumHelper.GetDescription(EnumInvPaymentMethob.TransferOrCash),
                    PaymentStatus = $"{(int)EnumInvPaymentStatus.Paid}",

                    Total = $"{inv.Total}", //$"{total}",
                    TaxRate = $"{inv.TaxRate}",
                    TaxAmount = $"{inv.TaxAmount}",
                    Amount = $"{inv.Amount}",
                    AmountInWords = inv.AmountInWord,

                    KindOfService = inv.KindOfService,
                    DiscountAmount = $"{inv.DiscountAmount}",
                    Extra9 = $"{_defaultInvRateForCalcTax}",
                    Extra10 = $"{inv.DiscountAmount}",

                    VatAmount0 = $"{inv.TaxAmount}",
                    GrossValue0 = $"{inv.Total}",
                    VatAmount5 = $"{inv.TaxAmount}",
                    GrossValue5 = $"{inv.Total}",
                    VatAmount8 = $"{inv.TaxAmount}",
                    GrossValue8 = $"{inv.Total}",
                    VatAmount10 = $"{inv.TaxAmount}",
                    GrossValue10 = $"{inv.Total}",

                    Products = new InvProducts
                    {
                        ListProducts = new List<InvProduct>
                        {
                            new InvProduct
                            {
                                ProdId = Guid.NewGuid(),
                                ProdName = inv.InvProductInfo.ProductName,
                                ProdPrice = string.Empty,
                                ProdUnit = " ",
                                Price = inv.Total,
                                Amount =  $"{inv.InvProductInfo.Amount}",
                                Total = $"{inv.InvProductInfo.Amount}",
                                ProdQuantity = string.Empty,
                                TaxAmount = $"{inv.TaxAmount}",
                                IsSum = $"{(int)EnumInvProductType.Product}",
                                TaxRate = $"{inv.TaxRate}"
                            }
                        }
                    }
                }
            };

            return invInfo;
        }

        private InvInv GenInvInfo(MajorContractModel contract)
        {
            DataTable dt = new DataTable();

            decimal taxRate = contract.TaxRate;

            contract.HasTaxForContract = taxRate > 0;

            decimal taxAmount;//= contract.LiquidationAmount * _defaultInvTaxRate / 100;
            decimal total = contract.LiquidationAmount;
            decimal amount = total;// = taxAmount + total;
            decimal discountAmount = 0;// = (long)Math.Round((decimal)dt.Compute(string.Format(_defaultInvDiscountFormula, amount), ""), 0);

            if (contract.HasTaxForContract)
            {
                total = Math.Round(amount * 100 / (100 + taxRate), 0);
                taxAmount = Math.Round(total * taxRate / 100, 0);
            }
            else
            {
                taxAmount = contract.LiquidationAmount * taxRate / 100;
                amount = taxAmount + total;
                discountAmount = (long)Math.Round((decimal)dt.Compute(string.Format(_defaultInvDiscountFormula, amount), ""), 0);
                amount -= discountAmount;
            }

            var lstPayments = _contractCache.GetPayments(contract.ContractId);
            var payOffPayment = lstPayments.FirstOrDefault(p => p.TypePayment == (int)EnumTypePayment.PayOff);

            InvInv invInfo = new InvInv
            {
                FKey = InvHelper.GenFKey(),
                Invoice = new InvInvoice
                {
                    //Pattern = pattern,
                    //Serial = serial,

                    Buyer = contract.CusInfo.TypeCus == ConstsCusType.BUSINESS ? string.Empty : contract.CusInfo.CusName,
                    CusName = contract.CusInfo.TypeCus == ConstsCusType.BUSINESS ? contract.CusInfo.CusName : string.Empty,
                    CusAddress = contract.CusInfo.Address,
                    CusCode = contract.CusInfo.CusCode,
                    CusPhone = contract.CusInfo.Phone,
                    CusTaxCode = contract.CusInfo.TaxCode,
                    CusEmail = contract.CusInfo.Email,
                    CusType = contract.CusInfo.TypeCus == ConstsCusType.BUSINESS ? "1" : "0",

                    IsPayed = true,

                    CurrencyUnit = nameof(EnumInvCurrencyUnit.VND),
                    PaymentMethod = payOffPayment?.PaymentMethod == (int)EnumPaymentMethod.Cash ? EnumHelper.GetDescription(EnumInvPaymentMethob.Cash) : EnumHelper.GetDescription(EnumInvPaymentMethob.Transfer),
                    PaymentStatus = $"{(int)EnumInvPaymentStatus.Paid}",

                    Total = $"{total}",
                    TaxRate = $"{contract.TaxRate}",
                    TaxAmount = $"{taxAmount}",
                    Amount = $"{amount}",
                    AmountInWords = $"{NumberHelper.UpperCaseFirst(NumberHelper.NumberToVietnamese(amount))} đồng",

                    KindOfService = contract.ContractNoInfo,
                    DiscountAmount = $"{discountAmount}",
                    Extra9 = $"{_defaultInvRateForCalcTax}",
                    Extra10 = $"{discountAmount}",

                    CCCDan = $"{(contract.CusInfo.TypeCus == ConstsCusType.CONSUMER ? contract.CusInfo.TypeIdentifier == (int)EnumTypeIdentifier.IdCard ? contract.CusInfo.IdentifierNo : "" : "")}",

                    VatAmount0 = $"{taxAmount}",
                    GrossValue0 = $"{total}",
                    VatAmount5 = $"{taxAmount}",
                    GrossValue5 = $"{total}",
                    VatAmount8 = $"{taxAmount}",
                    GrossValue8 = $"{total}",
                    VatAmount10 = $"{taxAmount}",
                    GrossValue10 = $"{total}",

                    Products = new InvProducts
                    {
                        ListProducts = new List<InvProduct>
                        {
                            new InvProduct
                            {
                                ProdId = Guid.NewGuid(),
                                ProdName = string.Format(_defaultInvProdName, contract.ContractNoInfo, contract.ConfirmOn?.ToString("dd/MM/yyyy")),
                                ProdPrice = string.Empty,
                                ProdUnit = " ",
                                Price = (long)total,
                                Amount =  $"{total}",
                                Total = $"{total}",
                                ProdQuantity = string.Empty,
                                TaxAmount = $"{taxAmount}",
                                IsSum = $"{(int)EnumInvProductType.Product}",
                                TaxRate = $"{taxRate}"
                            }
                        }
                    }
                }
            };

            return invInfo;
        }

        private string GeneralXMLInvoiceView(string xmlInv, string pattern, string serial)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlInv);
            var invoiceEle = doc.SelectSingleNode("/Inv/Invoice");

            if (invoiceEle != null)
            {
                XmlElement patternEle = doc.CreateElement("Pattern");
                patternEle.InnerXml = pattern;

                XmlElement serialEle = doc.CreateElement("Serial");
                serialEle.InnerXml = serial;

                invoiceEle.AppendChild(patternEle);
                invoiceEle.AppendChild(serialEle);
            }

            xmlInv = doc.OuterXml;

            var xmlDataInvDoc = new XDocument(XElement.Parse(xmlInv));
            var xmlDataInvoiceDoc = new XDocument();
            var sTemplateInvPath = HostingEnvironment.MapPath(Path.Combine(_invTemplateFolderPath, "InvoiceTemplate.xslt")) ?? string.Empty;
            //var sTemplateInvPath = HostingEnvironment.MapPath(Path.Combine(_invTemplateFolderPath, $"{(hasTaxForContract ? "New_" : "")}InvoiceTemplate.xslt")) ?? string.Empty;

            if (!string.IsNullOrEmpty(sTemplateInvPath))
            {
                var xslTransformer = new XslCompiledTransform();
                xslTransformer.Load(sTemplateInvPath);
                using (var oldDocumentReader = xmlDataInvDoc.CreateReader())
                {
                    using (var newDocumentWriter = xmlDataInvoiceDoc.CreateWriter())
                    {
                        xslTransformer.Transform(oldDocumentReader, newDocumentWriter);
                    }
                }
                xmlInv = xmlDataInvoiceDoc.ToString();
            }

            xmlInv = MappingTagXMLInv(xmlInv);

            return xmlInv;
        }

        private string MappingTagXMLInv(string dataXMLInv)
        {
            var dicMappingTagInvs = new Dictionary<string, Dictionary<string, string>>
            {
                {
                    "Invoice", new Dictionary<string, string>
                    {
                        {"PaymentMethod", "Kind_of_Payment"},
                        {"VATRate", "VAT_Rate"},
                        {"AmountInWords", "Amount_words"},
                        {"VATAmount", "VAT_Amount"}
                    }
                },
                {"Product", new Dictionary<string, string> {{"Amount", "Total"}}}
            };
            var xmlDataInvDoc = new XDocument(XElement.Parse(dataXMLInv));

            dicMappingTagInvs.Keys.ToList().ForEach(k =>
            {
                var dicMappingTags = dicMappingTagInvs[k];
                foreach (var ele in xmlDataInvDoc.Descendants(k).Elements())
                {
                    var dicProduct = dicMappingTags.FirstOrDefault(kp => kp.Key == ele.Name.LocalName);
                    if (dicProduct.Key != null) ele.Name = dicProduct.Value;
                }
            });

            dataXMLInv = xmlDataInvDoc.ToString();

            return dataXMLInv;
        }

        #endregion

        #endregion

        #region Acceptant & Liquidation

        /// <summary>
        /// Nghiệm thu và thanh lý hợp đồng
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>

        [AjaxOnly]
        [ActionType(Type = EnumActionType.Edit)]
        [HttpGet]
        public ActionResult Acceptant(Guid? contractId)
        {
            var contractModel = _contractCache.GetById(contractId);
            if (contractModel == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_contractTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var cusContractInfo = _contractCache.GetCus(contractId);
            if (cusContractInfo == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_contractCusTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var contractAcceptantModel = new AcceptantContractModel
            {
                ContractId = contractId,
                ContractNoInfo = contractModel.ContractNoInfo,
                LandParcelNo = contractModel.LandParcelNo,
                MapNo = contractModel.MapNo,
                Address = contractModel.Address,
                PurposeName = contractModel.PurposeName,
                TypeCusName = contractModel.TypeCusName,
                CusName = contractModel.CusName,

                CusInfo = cusContractInfo,

                LiquidationAmount = contractModel.LiquidationAmount,
                DiscountAmount = contractModel.DiscountAmount,
                TaxAmount = contractModel.TaxAmount,
                TotalPaidAmount = contractModel.TotalPaidAmount,

                HasTaxForContract = contractModel.TaxRate > 0,
                TaxRate = contractModel.TaxRate,
                TaxInfo = string.Format(_taxInfoContract, contractModel.TaxRate, contractModel.TaxAmount.ToString("#,### đ")),

                UpdatedBy = User.UserName
            };

            return PartialView("_Acceptant", contractAcceptantModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Edit)]
        [ValidateInput(false)]
        public ActionResult Acceptant(AcceptantContractModel model)
        {
            var contractModel = _contractCache.GetById(model.ContractId);
            if (contractModel == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_contractTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            contractModel.UpdatedBy = User.UserName;
            contractModel.Status = (int)EnumContractStatus.Liquidated;
            contractModel.StatusName = AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(EnumContractStatus.Liquidated));

            var ret = _contractCache.Acceptant(contractModel);

            var response = CreateMessage($"{AppProcessor.Messagor.GetMessage("Modal_Title_Acceptant")} {_contractTitle} [{contractModel.ContractTypeName} - {model.ContractNoInfo}]",
                EnumProcessType.NonFormat, ret > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response, contractId = model.ContractId });
        }

        [HttpGet]
        [ActionType(Type = EnumActionType.View)]
        public Task<ActionResult> ShowAcceptant(Guid? contractId, string fileType = ".docx")
        {
            var contractModel = _contractCache.GetById(contractId);
            if (contractModel == null)
                return Task.FromResult<ActionResult>(Json(new
                {
                    status = true,
                    message = CreateMessage($"{_contractTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                }));

            ViewBag.AppViewerUrl = _sysConfigCache.GetViaKey(CONFIG_KEY_OFFICE_APPVIEWER_URL)?.ConfigValue;

            var rdKey = EString.RandomStringNumber(8);
            contractModel.RenderContractId = $"{rdKey}.{contractId}{fileType}";
            contractModel.FileType = fileType;

            return Task.FromResult<ActionResult>(PartialView("_RenderAcceptant", contractModel));
        }

        [HttpGet]
        [ActionType(Type = EnumActionType.View)]
        [AllowAnonymous]
        public Task<ActionResult> RenderAcceptant(string renderId)
        {
            var arrParrams = renderId.Split('.');
            var contractId = Guid.Parse(arrParrams[1]);
            var fileType = $".{arrParrams[2]}" ?? ".docx";

            var contractModel = _contractCache.GetById(contractId);

            var lstPayments = _contractCache.GetPayments(contractModel.ContractId);
            var dataInfoA = _fEContractCache.GetDataRenderContract(contractModel.ContractId);
            JObject objContractInfo = JObject.Parse(dataInfoA.JsonContractInfo);

            #region Get Union Info

            var unionModel = _unionCache.GetById(contractModel.UnionId);
            if (unionModel != null && !string.IsNullOrEmpty(unionModel.UnionInfo))
            {
                var dicUnionInfos = JsonConvert.DeserializeObject<Dictionary<string, string>>(unionModel.UnionInfo);

                foreach (string key in dicUnionInfos.Keys)
                {
                    var dictKey = key.Replace($"{contractModel.TypeCus}.", "");
                    if (_dictExtendContractInfos.TryGetValue(dictKey, out var jsonKey))
                    {
                        objContractInfo[jsonKey] = dicUnionInfos[key];
                    }
                }
            }

            #endregion

            var keyQHNS = _sysConfigCache.GetViaKey("CONFIG_KEY_QHNS");
            var keyUnionName = _sysConfigCache.GetViaKey("CONFIG_KEY_UNIONNAME");

            // Convert JObject thành đối tượng PayAcceptantRecordModel
            PayAcceptantRecordModel paymentAcceptant =
                JsonConvert.DeserializeObject<PayAcceptantRecordModel>(objContractInfo.ToString());
            paymentAcceptant.UnionName = keyUnionName.ConfigValue;
            paymentAcceptant.ContractNo = contractModel.ContractNo + "/BBNT";

            //paymentAcceptant.Day = DateTime.Now.Day.ToString();
            //paymentAcceptant.Month = DateTime.Now.Month.ToString();
            //paymentAcceptant.Year = DateTime.Now.Year.ToString();
            paymentAcceptant.Day = (contractModel.CompletedOn ?? DateTime.Now).ToString("dd");
            paymentAcceptant.Month = (contractModel.CompletedOn ?? DateTime.Now).ToString("MM");
            paymentAcceptant.Year = (contractModel.CompletedOn ?? DateTime.Now).ToString("yyyy");

            paymentAcceptant.MaQHNS = keyQHNS.ConfigValue;
            paymentAcceptant.ContractNoInfo = contractModel.ContractNoInfo;
            // Giá trị hợp đồng đã ký
            paymentAcceptant.TotalPayment = contractModel.Total.ToString("N0").Replace(",", ".") + " đồng";
            // Giá trị thanh toán
            paymentAcceptant.TotalPaymentDone =
                Math.Round((decimal)contractModel.TotalPaidAmount).ToString("N0").Replace(",", ".") + " đồng";
            // Giá trị tạm ứng
            var advanceAmount =
                lstPayments.FirstOrDefault(p => p.TypePayment == (int)EnumTypePayment.Advance)?.PaidAmount ?? 0;
            paymentAcceptant.PaymentAdvance = advanceAmount.ToString("N0").Replace(",", ".") + " đồng";

            // Giá trị phải thanh toán khi thanh lý hợp đồng
            var payOffAmount =
                lstPayments.FirstOrDefault(p => p.TypePayment == (int)EnumTypePayment.PayOff)?.PaidAmount ?? 0;

            paymentAcceptant.PayNumber = Math.Round((decimal)payOffAmount).ToString("N0").Replace(",", ".") + " đồng";
            payOffAmount = payOffAmount < 0 ? Math.Abs(payOffAmount) : payOffAmount;
            paymentAcceptant.PayText = NumberHelper.NumberToString(payOffAmount.ToString());

            var acceptantTemplatePath = Server.MapPath(_acceptantTemplateFolderPath); // Đường dẫn của tệp mẫu Word
            if (ConstMIMEType.IsPdf(fileType))
            {
                // Gọi hàm RenderModelToWordAndSave để tạo và lưu tệp Word vào một mảng byte
                byte[] fileBytes = RenderWordHelper.RenderModelToPdfAndSave(paymentAcceptant, acceptantTemplatePath);

                return Task.FromResult<ActionResult>(File(fileBytes, ConstMIMEType.OfficeMIMETypes[fileType],
                    $"{contractModel.ContractNoInfo}{fileType}"));
            }
            else
            {
                byte[] fileBytes = RenderWordHelper.RenderModelToWordAndSave(paymentAcceptant, acceptantTemplatePath);

                return Task.FromResult<ActionResult>(File(fileBytes, ConstMIMEType.OfficeMIMETypes[fileType],
                    $"{contractModel.ContractNoInfo}{fileType}"));
            }
        }

        [HttpGet]
        [ActionType(Type = EnumActionType.View)]
        [AllowAnonymous]
        public Task<ActionResult> DownloadWord(string renderId)
        {
            var arrParrams = renderId.Split('.');
            var contractId = Guid.Parse(arrParrams[1]);
            var fileType = ".docx";

            var contractModel = _contractCache.GetById(contractId);

            var lstPayments = _contractCache.GetPayments(contractModel.ContractId);
            var dataInfoA = _fEContractCache.GetDataRenderContract(contractModel.ContractId);
            JObject objContractInfo = JObject.Parse(dataInfoA.JsonContractInfo);

            #region Get Union Info

            var unionModel = _unionCache.GetById(contractModel.UnionId);
            if (unionModel != null && !string.IsNullOrEmpty(unionModel.UnionInfo))
            {
                var dicUnionInfos = JsonConvert.DeserializeObject<Dictionary<string, string>>(unionModel.UnionInfo);

                foreach (string key in dicUnionInfos.Keys)
                {
                    var dictKey = key.Replace($"{contractModel.TypeCus}.", "");
                    if (_dictExtendContractInfos.TryGetValue(dictKey, out var jsonKey))
                    {
                        objContractInfo[jsonKey] = dicUnionInfos[key];
                    }
                }
            }

            #endregion

            var keyQHNS = _sysConfigCache.GetViaKey("CONFIG_KEY_QHNS");
            var keyUnionName = _sysConfigCache.GetViaKey("CONFIG_KEY_UNIONNAME");

            // Convert JObject thành đối tượng PayAcceptantRecordModel
            PayAcceptantRecordModel paymentAcceptant =
                JsonConvert.DeserializeObject<PayAcceptantRecordModel>(objContractInfo.ToString());
            paymentAcceptant.UnionName = keyUnionName.ConfigValue;
            paymentAcceptant.ContractNo = contractModel.ContractNo + "/BBNT";

            //paymentAcceptant.Day = DateTime.Now.Day.ToString();
            //paymentAcceptant.Month = DateTime.Now.Month.ToString();
            //paymentAcceptant.Year = DateTime.Now.Year.ToString();
            paymentAcceptant.Day = (contractModel.CompletedOn ?? DateTime.Now).ToString("dd");
            paymentAcceptant.Month = (contractModel.CompletedOn ?? DateTime.Now).ToString("MM");
            paymentAcceptant.Year = (contractModel.CompletedOn ?? DateTime.Now).ToString("yyyy");

            paymentAcceptant.MaQHNS = keyQHNS.ConfigValue;
            paymentAcceptant.ContractNoInfo = contractModel.ContractNoInfo;
            // Giá trị hợp đồng đã ký
            paymentAcceptant.TotalPayment = contractModel.Total.ToString("N0").Replace(",", ".") + " đồng";
            // Giá trị thanh toán
            paymentAcceptant.TotalPaymentDone =
                Math.Round((decimal)contractModel.TotalPaidAmount).ToString("N0").Replace(",", ".") + " đồng";
            // Giá trị tạm ứng
            var advanceAmount =
                lstPayments.FirstOrDefault(p => p.TypePayment == (int)EnumTypePayment.Advance)?.PaidAmount ?? 0;
            paymentAcceptant.PaymentAdvance = advanceAmount.ToString("N0").Replace(",", ".") + " đồng";

            // Giá trị phải thanh toán khi thanh lý hợp đồng
            var payOffAmount =
                lstPayments.FirstOrDefault(p => p.TypePayment == (int)EnumTypePayment.PayOff)?.PaidAmount ?? 0;

            paymentAcceptant.PayNumber = Math.Round((decimal)payOffAmount).ToString("N0").Replace(",", ".") + " đồng";
            payOffAmount = payOffAmount < 0 ? Math.Abs(payOffAmount) : payOffAmount;
            paymentAcceptant.PayText = NumberHelper.NumberToString(payOffAmount.ToString());

            var acceptantTemplatePath = Server.MapPath(_acceptantTemplateFolderPath); // Đường dẫn của tệp mẫu Word
            byte[] fileBytes = RenderWordHelper.RenderModelToWordAndSave(paymentAcceptant, acceptantTemplatePath);

            return Task.FromResult<ActionResult>(File(fileBytes, ConstMIMEType.OfficeMIMETypes[fileType],
                $"{contractModel.ContractNoInfo}{fileType}"));
        }

        #endregion
    }
}