using Modules.Sys.Areas.Sys.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cores.Base.Apps;
using Cores.Sys.Caches.Sys;
using TSFramework.App.Attributes;
using TSFramework.App.Processors;
using TSFramework.Core.Enums;
using Cores.Cate.Caches;
using System.Configuration;
using System.Web.Script.Serialization;
using System.Web.Security;
using Cores.Cate.Models;
using TSFramework.App.Principals;
using System.Web.SessionState;

namespace Modules.Sys.Areas.Sys.Controllers
{
    public class SysToolController : AppController
    {
        private readonly SysUserCache _userCache = new SysUserCache();
        private readonly CateUnionCache _unionCache = new CateUnionCache();

        private readonly string _sysToolTitle = AppProcessor.Messagor.GetMessage("SysTool_Title");
        private readonly List<SysFileModel> _listFileDatas = new List<SysFileModel>();

        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult ClearCache(string returnUrl = "")
        {
            HttpRuntime.UnloadAppDomain();
            if (Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);
            return RedirectToAction("Index", "Home");
        }

        #region Main Actions

        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        public ActionResult Index()
        {
            var jsonDataFiles = ReadStructureApp();
            ViewData["DataFiles"] = jsonDataFiles;
            return View();
        }

        [ActionType(Type = EnumActionType.View)]
        [HttpPost]
        public ActionResult UploadFile(SysUploadModel model)
        {
            var isSuccess = false;

            if (model.FileUpload != null && !string.IsNullOrEmpty(model.AbsolutePath))
            {
                var absolutePathFile = Path.Combine(model.AbsolutePath, Path.GetFileName(model.FileUpload.FileName));
                model.FileUpload.SaveAs(absolutePathFile);
                isSuccess = true;
            }

            var response = CreateMessage("Tải file", EnumProcessType.Add,
                isSuccess ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response });
        }

        #region FakeAccount

        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        public ActionResult FakeAccount()
        {
            List<CateUnionMemberModel> lstStaffs = new List<CateUnionMemberModel>();

            _unionCache.GetUnionsViaManager(User.UserName).ForEach(u =>
            {
                var staffsViaUnion = _unionCache.GetMembersViaUnion(u.UnionId, true);
                if (staffsViaUnion?.Count > 0)
                {
                    lstStaffs.AddRange(staffsViaUnion);
                }
            });

            //lstStaffs = _unionCache.GetMembersViaUnion(null, null);

            var model = new FakeAccountModel
            {
                ListUsers = lstStaffs
                    .OrderBy(u => u.FullName)
                    .ToList()
            };

            return PartialView("_FakeAccount", model);
        }

        private readonly string _defaultAvatar = ConfigurationManager.AppSettings["AppAvatarDefault_URL"];

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Add)]
        [ValidateInput(false)]
        public ActionResult FakeAccount(FakeAccountModel model)
        {
            #region Xóa Session & Cookie Cũ

            Session.Clear();
            Session.Abandon();
            var ssidManager = new SessionIDManager();
            ssidManager.RemoveSessionID(System.Web.HttpContext.Current);
            var newId = ssidManager.CreateSessionID(System.Web.HttpContext.Current);
            ssidManager.SaveSessionID(System.Web.HttpContext.Current, newId, out _, out _);
            FormsAuthentication.SignOut();

            #endregion

            var avatarFolderPath = ConfigurationManager.AppSettings["AppAvatarFolder_Path"];
            var userOnline = _userCache.GetByUserName(model.UserName);

            var userAvatar = string.IsNullOrEmpty(userOnline.Avatar)
                ? _defaultAvatar
                : $"{avatarFolderPath}/{userOnline.UserId.ToString()}/{userOnline.Avatar}";

            var unionModel = _unionCache.GetUnionByMember(userOnline.UserName);

            var loginUser = new AppPrincipalSerializeModel
            {
                FullName = userOnline.FullName,
                UserName = userOnline.UserName,
                Email = userOnline.Email,
                Avatar = userAvatar,
                CreatedDate = userOnline.CreatedDate,
                UnionName = unionModel?.UnionName
            };

            var serializer = new JavaScriptSerializer();

            var userData = serializer.Serialize(loginUser);
            var authTicket = new FormsAuthenticationTicket(
                1,
                loginUser.UserName,
                DateTime.Now,
                DateTime.Now.AddMinutes(30),
                true,
                userData);

            var encTicket = FormsAuthentication.Encrypt(authTicket);
            var faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);

            Request.RequestContext.HttpContext.Response.Cookies.Add(faCookie);

            var response = CreateMessage($"Đã chuyển đăng nhập sang tài khoản [{model.UserName} - {model.FullName}] thành công", EnumProcessType.NonFormat, EnumMsgIcon.Success);
            return Json(new { status = true, message = response }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #region Internal Function

        private string ReadStructureApp()
        {
            var hostPath = Server.MapPath("~");

            var lstFolders = Directory.GetDirectories(hostPath);
            foreach (var folder in lstFolders)
            {
                DirectoryInfo folderInfo = new DirectoryInfo(folder);
                _listFileDatas.Add(new SysFileModel
                {
                    IsFolder = true,
                    IsFile = false,
                    Id = Guid.NewGuid(),
                    Name = folderInfo.Name,
                    AbsolutePath = folderInfo.FullName,
                    Icons = new Dictionary<string, string[]>
                    {
                        { "default", new[] { "<i class='fa fa-folder'></i>", "text-primary-d1" } },
                        { "open", new[] { "<i class='fa fa-folder-open'></i>", "text-orange-d1" } }
                    },
                    Childrens = GetChilds(folderInfo)
                });
            }

            var lstFiles = Directory.GetFiles(hostPath);
            foreach (var file in lstFiles)
            {
                FileInfo fileInfo = new FileInfo(file);
                _listFileDatas.Add(new SysFileModel
                {
                    IsFolder = false,
                    IsFile = true,
                    Id = Guid.NewGuid(),
                    Name = fileInfo.Name,
                    AbsolutePath = fileInfo.FullName,
                    Icons = new Dictionary<string, string[]>
                    {
                        { "default", new[] { "<i class='fas fa-file-alt'></i>", "text-danger-d1" } }
                    }
                });
            }

            return JsonConvert.SerializeObject(_listFileDatas);
        }

        private List<SysFileModel> GetChilds(DirectoryInfo folderInfo)
        {
            var lstDataFiles = new List<SysFileModel>();
            var lstChildFolders = folderInfo.GetDirectories();
            if (lstChildFolders.Length > 0)
            {
                foreach (var folder in lstChildFolders)
                {
                    lstDataFiles.Add(new SysFileModel
                    {
                        IsFolder = true,
                        IsFile = false,
                        Id = Guid.NewGuid(),
                        Name = folder.Name,
                        AbsolutePath = folder.FullName,
                        Icons = new Dictionary<string, string[]>
                        {
                            { "default", new[] { "<i class='fa fa-folder'></i>", "text-primary-d1" } },
                            { "open", new[] { "<i class='fa fa-folder-open'></i>", "text-orange-d1" } }
                        },
                        Childrens = GetChilds(folder)
                    });
                }
            }

            var lstFiles = folderInfo.GetFiles();
            if (lstFiles.Length > 0)
            {
                foreach (var fileInfo in lstFiles)
                {
                    lstDataFiles.Add(new SysFileModel
                    {
                        IsFolder = false,
                        IsFile = true,
                        Id = Guid.NewGuid(),
                        Name = fileInfo.Name,
                        AbsolutePath = fileInfo.FullName,
                        Icons = new Dictionary<string, string[]>
                        {
                            { "default", new[] { "<i class='fas fa-file-alt'></i>", "text-danger-d1" } }
                        }
                    });
                }
            }

            return lstDataFiles;
        }

        #endregion
    }
}