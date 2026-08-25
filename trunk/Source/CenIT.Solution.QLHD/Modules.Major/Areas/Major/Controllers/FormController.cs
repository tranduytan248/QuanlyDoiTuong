using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Cores.Cate.Caches;
using Cores.Cate.Enum;
using Cores.Cate.Models;
using Cores.Major;
using Cores.Major.Caches;
using Cores.Major.Models;
using Cores.Sys.Apps;
using Cores.Sys.Caches.Sys;
using FastMember;
using Modules.Major.Areas.Major.Models;
using Newtonsoft.Json;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using TSFramework.App.Attributes;
using TSFramework.App.Models;
using TSFramework.App.Processors;
using TSFramework.Core.Enums;
using TSFramework.Core.Utils;

namespace Modules.Major.Areas.Major.Controllers
{
    public class FormController : AppController
    {
        private readonly CateCategoryCache _categoryCache = new CateCategoryCache();
        private readonly SAVFormCache _formCache = new SAVFormCache();
        private readonly SysConfigCache _configsCache = new SysConfigCache();

        private readonly SAVDossierCache _dossierCache = new SAVDossierCache();
        private readonly SAVMemberPermitFieldViolationCache _permitFieldViolationCache = new SAVMemberPermitFieldViolationCache();

        private readonly string _formTitle = AppProcessor.Messagor.GetMessage("Form_Title");
        private readonly string _formTemplateTitle = AppProcessor.Messagor.GetMessage("Form_Template_Title");

        private const string CONFIG_KEY_MAPPING_FORMKEY = "CONFIG_KEY_MAPPING_FORMKEY";
        private const string CONFIG_KEY_OFFICEAPPVIWER_URL = "CONFIG_KEY_OFFICEAPPVIWER_URL";

        //private readonly string _refDocsFolderPath = ConfigurationManager.AppSettings["RefDocs_PathFolder"] ?? @"/Contents/Modules/Major/RefDocs/";
        private readonly string _refTemplatesFolderPath = ConfigurationManager.AppSettings["RefTemplates_PathFolder"] ?? "/Contents/Modules/Major/Templates/";
        private readonly string _formFolderName = "Forms";

        // GET: Cate/Form
        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        public ActionResult Index()
        {
            var searchModel = new SearchFormModel
            {
                ListFormTypes = _categoryCache.GetAll(((int)EnumCateType.DocType).ToString())
                    .OrderBy(t => t.CateName)
                    .Select(c => new ListItem
                    {
                        Text = c.CateName,
                        Value = c.CateId.ToString()
                    }).ToList(),
                ListProcedureTypes = _categoryCache.GetAll(((int)EnumCateType.ProcedureType).ToString())
                    .Where(c => c.CateParentId != null)
                    .OrderBy(s => s.CateParentName).ThenBy(s => s.CateName)
                    .Select(c => new SelectListItem
                    {
                        Text = c.CateName,
                        Value = c.CateId.ToString(),
                        Group = new SelectListGroup { Name = c.CateParentName }
                    })
                    .ToList()
                //.OrderBy(t => t.CateName)
                //.Select(c => new ListItem
                //{
                //    Text = c.CateName,
                //    Value = c.CateId.ToString()
                //}).ToList()
            };
            return View(searchModel);
        }

        #region Main Function

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult Get(SearchFormModel searchModel)
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
            int total;
            var data = _formCache.Get(searchModel.FormTypes, searchModel.ProcedureTypes, out total, dataSearch);
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
            var model = new SAVFormModel
            {
                ListFormTypes = _categoryCache.GetAll(((int)EnumCateType.DocType).ToString())
                    .OrderBy(t => t.CateName)
                    .Select(c => new ListItem
                    {
                        Text = c.CateName,
                        Value = c.CateId.ToString()
                    }).ToList(),
                ListProcedureTypes = _categoryCache.GetAll(((int)EnumCateType.ProcedureType).ToString())
                    .Where(c => c.CateParentId != null)
                    .OrderBy(s => s.CateParentName).ThenBy(s => s.CateName)
                    .Select(c => new SelectListItem
                    {
                        Text = c.CateName,
                        Value = c.CateId.ToString(),
                        Group = new SelectListGroup { Name = c.CateParentName }
                    })
                    .ToList()
                //.OrderBy(t => t.CateName)
                //.Select(c => new ListItem
                //{
                //    Text = c.CateName,
                //    Value = c.CateId.ToString()
                //}).ToList()
            };

            return PartialView("_Add", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Add)]
        [ValidateInput(false)]
        public ActionResult Add(SAVFormModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ListFormTypes = _categoryCache.GetAll(((int)EnumCateType.DocType).ToString())
                    .OrderBy(t => t.CateName)
                    .Select(c => new ListItem
                    {
                        Text = c.CateName,
                        Value = c.CateId.ToString()
                    }).ToList();
                model.ListProcedureTypes = _categoryCache.GetAll(((int)EnumCateType.ProcedureType).ToString())
                    .Where(c => c.CateParentId != null)
                    .OrderBy(s => s.CateParentName).ThenBy(s => s.CateName)
                    .Select(c => new SelectListItem
                    {
                        Text = c.CateName,
                        Value = c.CateId.ToString(),
                        Group = new SelectListGroup { Name = c.CateParentName }
                    })
                    .ToList();
                //.OrderBy(t => t.CateName)
                //.Select(c => new ListItem
                //{
                //    Text = c.CateName,
                //    Value = c.CateId.ToString()
                //}).ToList();

                return PartialView("_Form", model);
            }

            string contentHtmlFormView = ContentHtmlView(model.RefViewName);

            string response;
            model.FormId = Guid.NewGuid();
            Guid? fileId = model.RefTemplate == null ? (Guid?)null : Guid.NewGuid();

            var formId = _formCache.Save(new SAVFormModel
            {
                FormId = model.FormId,
                FormCode = model.FormCode,
                FormName = model.FormName,
                FormType = model.FormType,
                FormDesc = model.FormDesc,
                ProcedureTypes = model.ProceTypes == null || model.ProceTypes.Count == 0 ? null : string.Join(",", model.ProceTypes),
                TemplateName = model.RefTemplate != null ? $"{fileId}{Path.GetExtension(model.RefTemplate.FileName)}" : null,
                ViewName = contentHtmlFormView,
                Version = model.Version ?? 1,
                RequiredInfo = model.RequiredInfo,
                Reason = "Thêm mới",
                UpdatedBy = User.UserName
            });
            if (formId == 0)
                response = CreateMessage($"{_formTitle} [{model.FormName} - {model.FormTypeName}]",
                    EnumProcessType.Add, EnumMsgIcon.Error);
            else if (formId == -9)
                response = CreateMessage($"{_formTitle} [{model.FormName} - {model.FormTypeName}]",
                    EnumProcessType.DataExisted, EnumMsgIcon.Error);
            else
            {
                response = CreateMessage($"{_formTitle} [{model.FormName} - {model.FormTypeName}]",
                    EnumProcessType.Add, EnumMsgIcon.Success);

                model.Reason = "Thêm mới";
                model.UpdatedBy = User.UserName;
                SaveUploadFile(model, fileId);
            }

            return Json(new { status = true, message = response }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult Edit(Guid? id)
        {
            var model = _formCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_formTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            model.ListFormTypes = _categoryCache.GetAll(((int)EnumCateType.DocType).ToString())
                .OrderBy(t => t.CateName)
                .Select(c => new ListItem
                {
                    Text = c.CateName,
                    Value = c.CateId.ToString()
                }).ToList();
            model.ListProcedureTypes = _categoryCache.GetAll(((int)EnumCateType.ProcedureType).ToString())
                .Where(c => c.CateParentId != null)
                .OrderBy(s => s.CateParentName).ThenBy(s => s.CateName)
                .Select(c => new SelectListItem
                {
                    Text = c.CateName,
                    Value = c.CateId.ToString(),
                    Group = new SelectListGroup { Name = c.CateParentName }
                })
                .ToList();
            //.OrderBy(t => t.CateName)
            //.Select(c => new ListItem
            //{
            //    Text = c.CateName,
            //    Value = c.CateId.ToString()
            //}).ToList();

            return PartialView("_Edit", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionType(Type = EnumActionType.Edit)]
        [ValidateInput(false)]
        public ActionResult Edit(SAVFormModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ListFormTypes = _categoryCache.GetAll(((int)EnumCateType.DocType).ToString())
                    .OrderBy(t => t.CateName)
                    .Select(c => new ListItem
                    {
                        Text = c.CateName,
                        Value = c.CateId.ToString()
                    }).ToList();
                model.ListProcedureTypes = _categoryCache.GetAll(((int)EnumCateType.ProcedureType).ToString())
                    .Where(c => c.CateParentId != null)
                    .OrderBy(s => s.CateParentName).ThenBy(s => s.CateName)
                    .Select(c => new SelectListItem
                    {
                        Text = c.CateName,
                        Value = c.CateId.ToString(),
                        Group = new SelectListGroup { Name = c.CateParentName }
                    })
                    .ToList();
                //.OrderBy(t => t.CateName)
                //.Select(c => new ListItem
                //{
                //    Text = c.CateName,
                //    Value = c.CateId.ToString()
                //}).ToList();

                return PartialView("_Form", model);
            }

            string contentHtmlFormView = ContentHtmlView(model.RefViewName);
            model.Version = model.RefTemplate != null ? model.Version + 1 : model.Version;
            Guid? fileId = model.RefTemplate == null ? (Guid?)null : Guid.NewGuid();

            string response;
            var formId = _formCache.Save(new SAVFormModel
            {
                FormId = model.FormId,
                FormCode = model.FormCode,
                FormName = model.FormName,
                FormType = model.FormType,
                FormDesc = model.FormDesc,
                ProcedureTypes = model.ProceTypes == null || model.ProceTypes.Count == 0 ? null : string.Join(",", model.ProceTypes),
                TemplateName = model.RefTemplate != null ? $"{fileId}{Path.GetExtension(model.RefTemplate.FileName)}" : null,
                ViewName = contentHtmlFormView,
                Version = model.Version,
                RequiredInfo = model.RequiredInfo,
                Reason = model.Reason,
                UpdatedBy = User.UserName
            });
            if (formId == 0)
                response = CreateMessage($"{_formTitle} [{model.FormName} - {model.FormTypeName}]",
                    EnumProcessType.Edit, EnumMsgIcon.Error);
            else if (formId == -9)
                response = CreateMessage($"{_formTitle} [{model.FormName} - {model.FormTypeName}]",
                    EnumProcessType.DataExisted, EnumMsgIcon.Error);
            else
            {
                response = CreateMessage($"{_formTitle} [{model.FormName} - {model.FormTypeName}]",
                    EnumProcessType.Edit, EnumMsgIcon.Success);

                model.UpdatedBy = User.UserName;
                SaveUploadFile(model, fileId);
            }

            return Json(new { status = true, message = response }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(Guid? id)
        {
            var model = _formCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_formTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Delete"),
                $"<b>{_formTitle} [{model.FormName} - {model.FormTypeName}]</b>");
            return PartialView("_Delete", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(SAVFormModel model)
        {
            ModelState.Remove("RefViewName");
            ModelState.Remove("RefTemplate");
            ModelState.Remove("RequiredIf");
            ModelState.Remove("RefDocs");
            if (!ModelState.IsValid)
            {
                ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Delete"),
                    $"<b>{_formTitle} [{model.FormName} - {model.FormTypeName}]</b>");
                return PartialView("_DelBody", model);
            }

            model.UpdatedBy = User.UserName;
            var deleted = _formCache.Delete(model);

            var response = CreateMessage($"{_formTitle} [{model.FormName} - {model.FormTypeName}]",
                EnumProcessType.Delete, deleted ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response });
        }

        #endregion

        #region Design Form

        [AjaxOnly]
        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        public ActionResult Check(Guid? id)
        {
            var model = _formCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_formTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            if (string.IsNullOrEmpty(model.ViewName))
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_formTemplateTitle} {_formTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var configModel = _configsCache.GetViaKey(CONFIG_KEY_MAPPING_FORMKEY);
            model.MappingFormKeys = configModel?.ConfigValue;

            return PartialView("_Check", model);
        }

        [ActionType(Type = EnumActionType.View)]
        [HttpPost]
        public ActionResult Export(SAVFormModel model)
        {
            var formModel = _formCache.GetById(model.FormId);
            if (formModel == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_formTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            NameValueCollection allNameValueCollection = Request.Form;
            if (!string.IsNullOrEmpty(model.SampleFormData))
            {
                NameValueCollection sampleFormData = JsonToNameValueCollection(model.SampleFormData);
                NameValueCollection formData = Request.Form;

                allNameValueCollection = new NameValueCollection(formData);
                foreach (string key in sampleFormData)
                {
                    allNameValueCollection[key] = sampleFormData[key];
                }
            }

            var bDatas = RenderForm(formModel.FormId, formModel.Version, formModel.TemplateName, allNameValueCollection);
            return File(bDatas, ConstMIMEType.OfficeMIMETypes[Path.GetExtension(formModel.TemplateName)], $"{formModel.FormName}{Path.GetExtension(formModel.TemplateName)}");
        }

        [ActionType(Type = EnumActionType.View)]
        [HttpPost]
        public ActionResult Preview(SAVFormModel model)
        {
            var formModel = _formCache.GetById(model.FormId);
            if (formModel == null)
                return Json(new
                {
                    status = false,
                    message = CreateMessage($"{_formTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var rdKey = EString.RandomStringNumber(8);
            var md5UserName = EHashMD5.CalculateMD5Hash(User.UserName);

            NameValueCollection allNameValueCollection = Request.Form;
            if (!string.IsNullOrEmpty(model.SampleFormData))
            {
                NameValueCollection sampleFormData = JsonToNameValueCollection(model.SampleFormData);
                NameValueCollection formData = Request.Form;

                allNameValueCollection = new NameValueCollection(formData);
                foreach (string key in sampleFormData)
                {
                    if (!allNameValueCollection.AllKeys.Contains(key))
                        allNameValueCollection[key] = sampleFormData[key];
                }
            }

            HttpContext.Application[$"{md5UserName}-FormData-Form-TmpExport-{model.FormId}"] = allNameValueCollection;

            ViewBag.AppViewerUrl = _configsCache.GetViaKey(CONFIG_KEY_OFFICEAPPVIWER_URL)?.ConfigValue;

            return PartialView("_PreviewResultForm", $"{rdKey}.{md5UserName}.{model.FormId}");
        }

        [AllowAnonymous]
        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        public ActionResult TmpExport(string fileId)
        {
            var arrFiles = fileId.Split('.');
            var md5UserName = arrFiles[1];
            Guid? formId = Guid.Parse(arrFiles[2]);

            var model = _formCache.GetById(formId);

            NameValueCollection formData = HttpContext.Application[$"{md5UserName}-FormData-Form-TmpExport-{formId}"] as NameValueCollection;

            var bDatas = RenderTmpForm(model.FormId, model.Version, model.TemplateName, formData);
            return File(bDatas, ConstMIMEType.OfficeMIMETypes[Path.GetExtension(model.TemplateName)], $"{model.FormName}{Path.GetExtension(model.TemplateName)}");
        }

        [AjaxOnly]
        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        public ActionResult Test()
        {
            var lstPermitFieldViolations = _permitFieldViolationCache.Get(User.UserName);

            var model = new SAVFormModel
            {
                ListFormTypes = _categoryCache.GetAll(((int)EnumCateType.DocType).ToString())
                    .OrderBy(t => t.CateName)
                    .Select(c => new ListItem
                    {
                        Text = c.CateName,
                        Value = c.CateId.ToString()
                    }).ToList(),
                ListForms = _formCache.GetAll().Select(u => new SelectListItem
                {
                    Text = u.FormName,
                    Value = u.FormId.ToString(),
                    Group = new SelectListGroup { Name = u.FormTypeName }
                }).OrderBy(u => u.Group.Name).ToList(),
                ListDossiers = _dossierCache.GetAll(User.UserName, null, null, null, null, null, null, null, string.Join(",", lstPermitFieldViolations.Select(f => f.FieldViolated))).Select(d => new ListItem { Text = d.Title, Value = $"{d.DossierId}" }).ToList()
            };

            return PartialView("_Test", model);
        }

        [AjaxOnly]
        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        public ActionResult ViewForms(Guid? dossierId, string forms)
        {
            var lstForms = forms.Split(';').Where(f => f.Length > 0).ToList();
            Session[$"{User.UserName}-TestForm-Forms-{dossierId}"] = forms;

            //var selectedForms = _formCache.GetAll().Where(f => lstForms.Any(sf => sf == f.FormId.ToString())).ToList();

            return PartialView("_ViewForms", new ViewDossierFormModel
            {
                CurrentIdx = lstForms.Count > 0 ? 0 : (int?)null,
                CurrentForm = lstForms.Count > 0 ? Guid.Parse(lstForms[0]) : (Guid?)null,
                PrevIdx = null,
                PrevForm = null,
                NextIdx = lstForms.Count > 1 ? 1 : (int?)null,
                NextForm = lstForms.Count > 1 ? Guid.Parse(lstForms[1]) : (Guid?)null,
                DossierId = dossierId
            });
        }

        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        public ActionResult DetailForm(Guid? id, int? currentIdx, Guid? dossierId)
        {
            string forms = Session[$"{User.UserName}-TestForm-Forms-{dossierId}"] as string;
            var formData = Session[$"{User.UserName}-TestForm-Forms-FormData-{dossierId}"] as string;

            var configModel = _configsCache.GetViaKey(CONFIG_KEY_MAPPING_FORMKEY);
            var mappingFormKeys = configModel?.ConfigValue;
            var lstForms = forms?.Split(';').Where(f => f.Length > 0).ToList() ?? new List<string>();

            var selectedForm = _formCache.GetById(id);

            if (selectedForm == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_formTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            if (string.IsNullOrEmpty(selectedForm.ViewName))
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_formTemplateTitle} {_formTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            if (string.IsNullOrEmpty(formData))
            {
                var dossierModel = _dossierCache.GetById(dossierId);
                formData = dossierModel.FormData;
                Session[$"{User.UserName}-TestForm-Forms-FormData-{dossierId}"] = formData;
            }

            var formView = new ViewFormStructureModel
            {
                DossierId = dossierId,
                ViewName = selectedForm.ViewName,
                FormData = formData,
                MappingFormKeys = mappingFormKeys,
                RequiredInfo = selectedForm.RequiredInfo,
                FormId = id
            };

            var prevIdx = currentIdx - 1;
            prevIdx = prevIdx < 0 ? null : prevIdx;

            var nextIdx = currentIdx + 1;
            nextIdx = nextIdx > lstForms.Count - 1 ? null : nextIdx;

            formView.PrevIdx = prevIdx;
            formView.PrevForm = prevIdx != null ? Guid.Parse(lstForms[(int)prevIdx]) : (Guid?)null;
            formView.NextIdx = nextIdx;
            formView.NextForm = nextIdx != null ? Guid.Parse(lstForms[(int)nextIdx]) : (Guid?)null;

            return PartialView("_DetailForm", formView);
        }

        [ActionType(Type = EnumActionType.View)]
        [HttpPost]
        public ActionResult PreviewForm(Guid? id, Guid? dossierId)
        {
            var model = _formCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = false,
                    message = CreateMessage($"{_formTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            var rdKey = EString.RandomStringNumber(8);
            var md5UserName = EHashMD5.CalculateMD5Hash(User.UserName);

            NameValueCollection formData = new NameValueCollection(Request.Form);
            var baseFormData = Session[$"{User.UserName}-TestForm-FormData-{dossierId}"] as string;

            if (!string.IsNullOrEmpty(baseFormData))
            {
                NameValueCollection dossierFormData = JsonToNameValueCollection(baseFormData);
                foreach (string key in dossierFormData)
                {
                    if (!dossierFormData.AllKeys.Contains(key))
                    {
                        formData[key] = dossierFormData[key];
                    }
                }
            }

            HttpContext.Application[$"{md5UserName}-FormData-Form-TmpExport-{id}"] = formData;
            Session[$"{User.UserName}-TestForm-FormData-{dossierId}"] = formData;

            ViewBag.AppViewerUrl = _configsCache.GetViaKey(CONFIG_KEY_OFFICEAPPVIWER_URL)?.ConfigValue;

            return PartialView("_PreviewResultForm", $"{rdKey}.{md5UserName}.{id}");
        }

        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetListForms(Guid? formType = null, string selectedForms = null)
        {
            var lstSelectedForms = string.IsNullOrEmpty(selectedForms) ? new List<string>() : selectedForms.Split(';').ToList();
            var lstForms = _formCache.GetAll(formType == null ? null : $"{formType}").Where(f => lstSelectedForms.All(sf => sf != f.FormId.ToString()))
                .GroupBy(f => f.FormTypeName)
                .Select(g => new { text = g.Key, children = g.Select(f => new { id = f.FormId, text = f.FormName }) }).ToArray();

            return Json(lstForms);
        }

        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetFormDataViaDossier(Guid? dossierId)
        {
            var dossierModel = _dossierCache.GetById(dossierId);
            Session[$"{User.UserName}-TestForm-FormData-{dossierId}"] = dossierModel?.FormData;
            return Json(dossierModel?.FormData);
        }

        [ActionType(Type = EnumActionType.View)]
        [HttpPost]
        public ActionResult SaveFormData(Guid? dossierId)
        {
            NameValueCollection formData = new NameValueCollection(Request.Form);
            var currentFormData = Session[$"{User.UserName}-TestForm-Forms-FormData-{dossierId}"] as string;

            if (!string.IsNullOrEmpty(currentFormData))
            {
                NameValueCollection dossierFormData = JsonToNameValueCollection(currentFormData);
                foreach (string key in formData)
                {
                    dossierFormData[key] = formData[key];
                }

                var allKeys = dossierFormData.AllKeys;
                foreach (string key in allKeys)
                {
                    if (formData.AllKeys.Contains(key))
                        dossierFormData[key] = formData[key];
                }

                formData = dossierFormData;
            }

            var jsonFormData = JsonConvert.SerializeObject(formData.AllKeys.ToDictionary(k => k, k => formData[k]));
            Session[$"{User.UserName}-TestForm-Forms-FormData-{dossierId}"] = jsonFormData;

            return Json(new
            {
                status = true,
            });


        }

        #endregion

        #region Extend Function

        private bool SaveUploadFile(SAVFormModel model, Guid? fileId)
        {
            if (model.RefTemplate == null || model.FormId == null) return false;

            var lstDocs = new List<SAVDocModel>();

            var refFormTemplatesFolderPath = $"{_refTemplatesFolderPath}/{_formFolderName}/{model.FormId.ToString()}/v{model.Version}";
            //Path.Combine(_refTemplatesFolderPath, _formFolderName, model.FormId.ToString(), $"v{model.Version}");
            var refFormTemplatesFolderAbsolutePath = Server.MapPath(refFormTemplatesFolderPath);

            if (!Directory.Exists(refFormTemplatesFolderAbsolutePath))
                Directory.CreateDirectory(refFormTemplatesFolderAbsolutePath);

            lstDocs.Add(new SAVDocModel
            {
                FileId = fileId,
                FilePath = refFormTemplatesFolderPath,
                FileName = Path.GetFileNameWithoutExtension(model.RefTemplate.FileName),
                FileExt = Path.GetExtension(model.RefTemplate.FileName),
                ContentType = model.RefTemplate.ContentType,
                Version = model.Version ?? 1
            });

            if (lstDocs.Count <= 0) return false;
            model.TableRefDocs = CreateTableRefDocs(lstDocs);
            var formId = _formCache.SaveDocs(model);

            if (formId > 0)
            {
                var savDoc = lstDocs.FirstOrDefault(d =>
                    d.FileName == Path.GetFileNameWithoutExtension(model.RefTemplate.FileName) && d.FileExt ==
                        Path.GetExtension(model.RefTemplate.FileName) && d.ContentType == model.RefTemplate.ContentType);

                model.RefTemplate.SaveAs(Path.Combine(refFormTemplatesFolderAbsolutePath, $"{savDoc?.FileId.ToString().ToUpper()}{savDoc?.FileExt}"));
            }
            return formId > 0;
        }

        private DataTable CreateTableRefDocs(List<SAVDocModel> lstDocs)
        {
            var dataRefDocs = new DataTable();
            using (var reader = ObjectReader.Create(lstDocs, "FileId", "TypeObject", "FilePath", "FileName", "FileExt", "ContentType", "Dimensions", "Version"))
            {
                dataRefDocs.Load(reader);
            }

            return dataRefDocs;
        }

        private byte[] RenderTmpForm(Guid? formId, int? version, string templateId, NameValueCollection formData)
        {
            var formTemplatesFolderPath = Path.Combine(_refTemplatesFolderPath, _formFolderName, formId.ToString(), $"v{version}", templateId);
            var formTemplatesFolderAbsolutePath = Server.MapPath(formTemplatesFolderPath);

            Document document = new Document();
            document.LoadFromFile(formTemplatesFolderAbsolutePath);

            foreach (var key in formData.AllKeys)
            {
                document.Replace("{" + key + "}", formData[key], true, true);
            }

            byte[] bArrays;
            using (MemoryStream stream = new MemoryStream())
            {
                document.SaveToStream(stream, FileFormat.Docx);
                bArrays = stream.ToArray();
            }

            return bArrays;
            //document.SaveToFile("Replace.docx", FileFormat.Docx);
        }

        private byte[] RenderForm(Guid? formId, int? version, string templateId, NameValueCollection formData)
        {
            var formTemplatesFolderPath = $"{_refTemplatesFolderPath}/{_formFolderName}/{formId.ToString()}/v{version}/{templateId}";
            //Path.Combine(_refTemplatesFolderPath, _formFolderName, formId.ToString(), $"v{version}", templateId);
            var formTemplatesFolderAbsolutePath = Server.MapPath(formTemplatesFolderPath);

            Document document = new Document();
            document.LoadFromFile(formTemplatesFolderAbsolutePath);

            //"\u2611" checked
            //"\u2610" uncheck
            foreach (var key in formData.AllKeys)
            {
                if (formData[key] == @"\u2611" || formData[key] == @"\u2610")
                {
                    Section section = document.AddSection();
                    Paragraph paragraph = section.AddParagraph();
                    TextRange tr = paragraph.AppendText((formData[key] == @"\u2611" ? '\u2611' : '\u2610').ToString());
                    document.Replace("{" + key + "}", tr.Text, false, true);
                }
                else
                {
                    document.Replace("{" + key + "}", formData[key], false, true);
                }
            }

            byte[] bArrays;
            using (MemoryStream stream = new MemoryStream())
            {
                document.SaveToStream(stream, FileFormat.Docx);
                bArrays = stream.ToArray();
            }

            return bArrays;
        }

        private string ContentHtmlView(HttpPostedFileBase refFormView)
        {
            if (refFormView == null) return null;
            BinaryReader b = new BinaryReader(refFormView.InputStream);
            byte[] binData = b.ReadBytes(refFormView.ContentLength);
            string result = System.Text.Encoding.UTF8.GetString(binData);
            return result;
        }

        private bool IsOnlyHexInString(string test)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(test, @"\A\b[0-9a-fA-F]+\b\Z");
        }

        private NameValueCollection JsonToNameValueCollection(string sJsonData)
        {
            var dictData = JsonConvert.DeserializeObject<Dictionary<string, string>>(sJsonData);

            NameValueCollection nvcData = null;
            if (dictData != null)
            {
                nvcData = new NameValueCollection(dictData.Count);
                foreach (var k in dictData)
                {
                    nvcData.Add(k.Key, k.Value);
                }
            }

            return nvcData;
        }

        #endregion
    }
}