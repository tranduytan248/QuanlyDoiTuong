using Cores.Cate.Caches;
using Cores.Cate.Models;

using Cores.Sys.Caches.Sys;
using System;
using System.IO;
using System.Web.Mvc;
using Cores.Base.Apps;
using TSFramework.App.Attributes;
using TSFramework.App.Processors;
using TSFramework.Core.Enums;

namespace Modules.Cate.Areas.Cate.Controllers
{
    public class DocController : AppController
    {
        private readonly SysConfigCache _configsCache = new SysConfigCache();
        private readonly CateDocCache _docCache = new CateDocCache();

        private readonly string _docTitle = AppProcessor.Messagor.GetMessage("Doc_Title");

        private const string CONFIG_KEY_OFFICEAPPVIWER_URL = "CONFIG_KEY_OFFICEAPPVIWER_URL";

        [HttpGet]
        [AjaxOnly]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult RefDoc(Guid? fileId)
        {
            var cateDoc = _docCache.GetById(fileId);
            if (cateDoc == null)
                return Json(new
                {
                    status = false,
                    message = CreateMessage($"{_docTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var fullPathFile = Server.MapPath($@"{cateDoc.FilePath}\{cateDoc.FileId}{cateDoc.FileExt}");
            if (!System.IO.File.Exists(fullPathFile))
                return Json(new
                {
                    status = false,
                    message = CreateMessage($"{_docTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            return Json(new
            {
                status = true,
                downloadPath = Url.Action("DownloadDoc", new { fileId })
            });
        }

        [HttpGet]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult DownloadDoc(Guid? fileId)
        {
            var cateDoc = _docCache.GetById(fileId);
            if (cateDoc == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_docTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var fullPathFile = Server.MapPath($@"{cateDoc.FilePath}\{cateDoc.FileId}{cateDoc.FileExt}");
            if (!System.IO.File.Exists(fullPathFile))
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_docTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            var arrBytes = System.IO.File.ReadAllBytes(fullPathFile);

            return File(arrBytes, cateDoc.ContentType, cateDoc.FileName + cateDoc.FileExt);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult DeleteDoc(Guid? fileId)
        {
            var cateDoc = _docCache.GetById(fileId);
            if (cateDoc == null)
                return Json(new
                {
                    status = false,
                    message = CreateMessage($"{_docTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            //var fullPathFile =
            //    Server.MapPath($@"{cateDoc.FilePath}\{cateDoc.FileId.ToString().ToUpper()}{cateDoc.FileExt}");
            //if (!System.IO.File.Exists(fullPathFile))
            //    return Json(new
            //    {
            //        status = false,
            //        message = CreateMessage($"{_docTitle}",
            //            EnumProcessType.DataNotExist, EnumMsgIcon.Error)
            //    });
            ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Common_ConfirmMessage"),
                $"<b>{_docTitle} [{cateDoc.FileName}{cateDoc.FileExt}]</b>");
            return PartialView("_DeleteDoc", cateDoc);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult DeleteDoc(CateDocModel model)
        {
            model.Reason = string.IsNullOrEmpty(model.Reason) ? $"Xoá {_docTitle}" : model.Reason;
            model.UpdatedBy = User.UserName;

            var deleted = _docCache.Delete(model);
            if (deleted)
            {
                var fullPathFile =
                    Server.MapPath(Path.Combine(model.FilePath, $"{model.FileId.ToString().ToUpper()}{model.FileExt}"));
                if (System.IO.File.Exists(fullPathFile)) System.IO.File.Delete(fullPathFile);
            }

            var response = CreateMessage($"{_docTitle} [{model.FileName}{model.FileExt}]",
                EnumProcessType.Delete, deleted ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response, fileId = model.FileId });
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult ViewDoc(Guid? fileId)
        {
            var cateDoc = _docCache.GetById(fileId);
            if (cateDoc == null)
                return Json(new
                {
                    status = false,
                    message = CreateMessage($"{_docTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var fullPathFile = Server.MapPath($"{cateDoc.FilePath}/{cateDoc.FileId.ToString().ToUpper()}{cateDoc.FileExt}");
            if (!System.IO.File.Exists(fullPathFile))
            {
                return Json(new
                {
                    status = false,
                    message = CreateMessage($"{_docTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            }

            ViewBag.AppViewerUrl = _configsCache.GetViaKey(CONFIG_KEY_OFFICEAPPVIWER_URL)?.ConfigValue;

            cateDoc.UrlPath = $"{Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, "")}{cateDoc.FilePath}/{cateDoc.FileId.ToString().ToUpper()}{cateDoc.FileExt}";

            return PartialView("_Viewer", cateDoc);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult ViewCateDoc(Guid? fileId)
        {
            var cateDoc = _docCache.GetById(fileId);
            if (cateDoc == null)
                return Json(new
                {
                    status = false,
                    message = CreateMessage($"{_docTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var fullPathFile = Server.MapPath($"{cateDoc.FilePath}/{cateDoc.FileId.ToString().ToUpper()}{cateDoc.FileExt}");
            if (!System.IO.File.Exists(fullPathFile))
            {
                return Json(new
                {
                    status = false,
                    message = CreateMessage($"{_docTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            }

            ViewBag.AppViewerUrl = _configsCache.GetViaKey(CONFIG_KEY_OFFICEAPPVIWER_URL)?.ConfigValue;

            cateDoc.UrlPath = $"{Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, "")}{cateDoc.FilePath}/{cateDoc.FileId.ToString().ToUpper()}{cateDoc.FileExt}";

            return PartialView("_DocViewer", cateDoc);
        }

        [HttpGet]
        [AjaxOnly]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult RefCateDoc(Guid? fileId)
        {
            var cateDoc = _docCache.GetById(fileId);
            if (cateDoc == null)
                return Json(new
                {
                    status = false,
                    message = CreateMessage($"{_docTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var fullPathFile = Server.MapPath($@"{cateDoc.FilePath}\{cateDoc.FileId}{cateDoc.FileExt}");
            if (!System.IO.File.Exists(fullPathFile))
                return Json(new
                {
                    status = false,
                    message = CreateMessage($"{_docTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            return Json(new
            {
                status = true,
                downloadPath = Url.Action("DownloadCateDoc", new { fileId })
            });
        }

        [HttpGet]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult DownloadCateDoc(Guid? fileId)
        {
            var cateDoc = _docCache.GetById(fileId);
            if (cateDoc == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_docTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var fullPathFile = Server.MapPath($@"{cateDoc.FilePath}\{cateDoc.FileId}{cateDoc.FileExt}");
            if (!System.IO.File.Exists(fullPathFile))
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_docTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            var arrBytes = System.IO.File.ReadAllBytes(fullPathFile);

            return File(arrBytes, cateDoc.ContentType, cateDoc.FileName + cateDoc.FileExt);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult ViewRefDoc(Guid? fileId)
        {
            var cateDoc = _docCache.GetById(fileId);
            if (cateDoc == null)
                return Json(new
                {
                    status = false,
                    message = CreateMessage($"{_docTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var fullPathFile = Server.MapPath($"{cateDoc.FilePath}/{cateDoc.ObjectId.ToUpper()}{cateDoc.FileExt}");
            if (!System.IO.File.Exists(fullPathFile))
            {
                return Json(new
                {
                    status = false,
                    message = CreateMessage($"{_docTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            }

            ViewBag.AppViewerUrl = _configsCache.GetViaKey(CONFIG_KEY_OFFICEAPPVIWER_URL)?.ConfigValue;

            cateDoc.UrlPath = $"{Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, "")}{cateDoc.FilePath}/{cateDoc.ObjectId.ToUpper()}{cateDoc.FileExt}";

            return PartialView("_Viewer", cateDoc);
        }
    }
}