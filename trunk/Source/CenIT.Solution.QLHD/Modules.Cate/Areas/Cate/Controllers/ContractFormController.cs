using Cores.Cate.Caches;
using Cores.Cate.Models;

using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Cores.Base.Apps;
using Cores.eContract.Consts;
using TSFramework.App.Attributes;
using TSFramework.App.Models;
using TSFramework.App.Processors;
using TSFramework.Core.Enums;

namespace Modules.Cate.Areas.Cate.Controllers
{
    public class ContractFormController : AppController
    {
        private readonly CateContractTemplateCache _contractTemplateCache = new CateContractTemplateCache();
        //private readonly CateContractTypeCache _contractTypeCache = new CateContractTypeCache();
        private readonly string _contractTemplateTitle = AppProcessor.Messagor.GetMessage("ContractForm_Label_Title");
        private readonly string _pathContractTemplate = "/Contents/File/TemplateContract/";


        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        public ActionResult Index()
        {
            return View();

        }

        // Action method to display the list of contract templates
        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult Get()
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

            //var dataSearch = new ReqSearchModel();

            var data = _contractTemplateCache.Get(out int total, dataSearch);
            var result = Json(
                new { draw = Convert.ToInt32(draw), recordsTotal = total, recordsFiltered = total, data },
                JsonRequestBehavior.AllowGet);

            return result;
        }

        /// <summary>
        /// Thêm mới trạng thái hợp đồng
        /// </summary>
        /// <returns></returns>
        [AjaxOnly]
        [ActionType(Type = EnumActionType.Add)]
        [HttpGet]
        public ActionResult Add()
        {
            var model = new CateContractTemplateModel
            {
                //ListTypeContracts = _contractTypeCache.GetAll()
            };
            return PartialView("_Add", model);
        }

        /// <summary>
        /// Thêm mới quy định
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Add)]
        [ValidateInput(false)]
        public ActionResult Add(CateContractTemplateModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_ContractForm", model);
            }

            Guid guid = Guid.NewGuid();
            model.Version = Guid.NewGuid();

            // Chuyển đổi GUID thành một chuỗi hexa (hệ 16) và loại bỏ dấu gạch ngang
            model.Id = guid.ToString("N");
            List<string> arrayName = new List<string>();
            List<string> arrayTemplate = new List<string>();

            // Kiểm tra và lưu tệp RefFilesCosumer (cá nhân)
            if (model.RefFilesCosumer != null && model.RefFilesCosumer.Count > 0 && model.RefFilesCosumer[0] != null)
            {
                string fileNameCosumer = SaveRefContractFile(model.RefFilesCosumer[0], "CN", model.Id, $"{model.Version}");
                arrayName.Add(fileNameCosumer);
                arrayTemplate.Add(Path.GetFileNameWithoutExtension(fileNameCosumer));
                model.TemplatePathCosumer = Path.Combine(_pathContractTemplate, model.Id, $"{model.Version}", fileNameCosumer);
            }

            // Kiểm tra và lưu tệp RefFiles (doanh nghiệp)
            if (model.RefFiles != null && model.RefFiles.Count > 0 && model.RefFiles[0] != null)
            {
                string fileName = SaveRefContractFile(model.RefFiles[0], "DN", model.Id, $"{model.Version}");
                arrayName.Add(fileName);
                arrayTemplate.Add(Path.GetFileNameWithoutExtension(fileName));
                model.TemplatePath = Path.Combine(_pathContractTemplate, model.Id, $"{model.Version}", fileName);
            }

            model.FileName = string.Join(",", arrayName);
            model.TemplateName = string.Join(",", arrayTemplate);
            model.UpdatedBy = User.UserName;

            var result = _contractTemplateCache.Save(model);

            string response;
            if (result == 0)
                response = CreateMessage($"{_contractTemplateTitle} [{model.TemplateName}]",
                    EnumProcessType.Add,
                    EnumMsgIcon.Error);
            else if (result == -9)
                response = CreateMessage($"{_contractTemplateTitle} [{model.TemplateName}]",
                    EnumProcessType.DataExisted,
                    EnumMsgIcon.Error);
            else
                response = CreateMessage($"{_contractTemplateTitle} [{model.TemplateName}]",
                    EnumProcessType.Add,
                    EnumMsgIcon.Success);
            return Json(new { status = true, message = response }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Giao diện cập nhật trạng thái hợp đồng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult Edit(string id = null)
        {
            var model = _contractTemplateCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_contractTemplateTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            //model.ListTypeContracts = _contractTypeCache.GetAll();
            return PartialView("_Edit", model);
        }

        /// <summary>
        /// Cập nhật trạng thái hợp đồng
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionType(Type = EnumActionType.Edit)]
        [ValidateInput(false)]
        public ActionResult Edit(CateContractTemplateModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_ContractForm", model);
            }

            List<string> arrayName = new List<string>();
            List<string> arrayTemplate = new List<string>();

            // Khởi tạo danh sách tên tệp và mẫu từ chuỗi đầu vào (nếu có)
            if (model.FileName != null)
            {
                arrayName.AddRange(model.FileName.Split(','));
            }

            if (model.TemplateName != null)
            {
                arrayTemplate.AddRange(model.TemplateName.Split(','));
            }

            bool hasNewVersion = false;

            // Kiểm tra và lưu tệp RefFilesCosumer (cá nhân)
            if (model.RefFilesCosumer != null && model.RefFilesCosumer.Count > 0 && model.RefFilesCosumer[0] != null)
            {
                hasNewVersion = true;
                model.Version = Guid.NewGuid();

                string fileNameCosumer = SaveRefContractFile(model.RefFilesCosumer[0], "CN", model.Id, $"{model.Version}");

                if (arrayName.Count > 0)
                {
                    arrayName[0] = fileNameCosumer;
                }
                else
                {
                    arrayName.Add(fileNameCosumer);
                }

                if (arrayTemplate.Count > 0)
                {
                    arrayTemplate[0] = Path.GetFileNameWithoutExtension(fileNameCosumer);
                }
                else
                {
                    arrayTemplate.Add(Path.GetFileNameWithoutExtension(fileNameCosumer));
                }
                model.TemplatePathCosumer = Path.Combine(_pathContractTemplate, model.Id, $"{model.Version}", fileNameCosumer);
            }

            // Kiểm tra và lưu tệp RefFiles (doanh nghiệp)
            if (model.RefFiles != null && model.RefFiles.Count > 0 && model.RefFiles[0] != null)
            {
                if (!hasNewVersion) { model.Version = Guid.NewGuid(); }

                string fileName = SaveRefContractFile(model.RefFiles[0], "DN", model.Id, $"{model.Version}");

                if (arrayName.Count > 1)
                {
                    arrayName[1] = fileName;
                }
                else
                {
                    arrayName.Add(fileName);
                }

                if (arrayTemplate.Count > 1)
                {
                    arrayTemplate[1] = Path.GetFileNameWithoutExtension(fileName);
                }
                else
                {
                    arrayTemplate.Add(Path.GetFileNameWithoutExtension(fileName));
                }
                model.TemplatePath = Path.Combine(_pathContractTemplate, model.Id, $"{model.Version}", fileName);
            }

            model.FileName = string.Join(",", arrayName);
            model.TemplateName = string.Join(",", arrayTemplate);
            model.UpdatedBy = User.UserName;

            var contractTemplateId = _contractTemplateCache.Save(model);

            string response = CreateMessage($"{_contractTemplateTitle} [{model.TemplateName}]",
                contractTemplateId == (int)EnumStatus.Existed ? EnumProcessType.DataExisted : EnumProcessType.Edit,
                contractTemplateId > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Giao diện xóa trạng thái hợp đồng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(string id = null)
        {
            var model = _contractTemplateCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_contractTemplateTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Delete"),
                $"<b>{_contractTemplateTitle} [{model.TemplateName}]</b>");
            return PartialView("_Delete", model);
        }

        /// <summary>
        /// Xóa trạng thái hợp đồng
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(CateContractTemplateModel model)
        {
            var deleted = _contractTemplateCache.Delete(model.Id, User.UserName);

            var response = CreateMessage($"{_contractTemplateTitle} [{model.TemplateName}]",
                EnumProcessType.Delete, deleted > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response });
        }

        [HttpGet]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult Download(string id, string fileName)
        {
            var contractTemplate = _contractTemplateCache.GetById(id);
            if (contractTemplate == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_contractTemplateTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var typeCus = fileName.Contains("CN-") ? ConstsCusType.CONSUMER : ConstsCusType.BUSINESS;
            var templateFilePath = typeCus == ConstsCusType.CONSUMER
                ? contractTemplate.TemplatePathCosumer
                : contractTemplate.TemplatePath;

            var absoluteTemplateFilePath = Server.MapPath(templateFilePath);

            if (!System.IO.File.Exists(absoluteTemplateFilePath))
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_contractTemplateTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            byte[] dataTemplateContact = System.IO.File.ReadAllBytes(absoluteTemplateFilePath);
            return File(dataTemplateContact, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", $"{(typeCus == ConstsCusType.CONSUMER ? "CN-" : "DN-")}{contractTemplate.FullName}{Path.GetExtension(templateFilePath)}");
        }

        //Action method to view details of a contract template
        //[AjaxOnly]
        //[HttpGet]
        //[ActionType(Type = EnumActionType.View)]
        //public ActionResult Details(string id)
        //{
        //    var response = _contractTemplateCache.GetById(id);

        //    var response = CreateMessage($"{_contractTemplateTitle} [{model.ContractTemplateName}]",
        //        EnumProcessType.Delete, deleted ? EnumMsgIcon.Success : EnumMsgIcon.Error);
        //    return Json(new { status = true, message = response });
        //}

        /// <summary>
        /// Tạo tên file và lưu file vào ổ cứng
        /// </summary>
        /// <param name="refFile"></param>
        /// <param name="typeCus"></param>
        /// <param name="templateId"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        private string SaveRefContractFile(HttpPostedFileBase refFile, string typeCus, string templateId, string version)
        {
            var templateContractFolderPath = Server.MapPath(Path.Combine(_pathContractTemplate, templateId, version));
            if (!Directory.Exists(templateContractFolderPath))
            {
                Directory.CreateDirectory(templateContractFolderPath);
            }

            var fileName = $"{typeCus}-{templateId}{Path.GetExtension(refFile.FileName)}";
            var absoluteFilePath = Path.Combine(templateContractFolderPath, fileName);
            refFile.SaveAs(absoluteFilePath);

            return fileName;
        }
    }
}