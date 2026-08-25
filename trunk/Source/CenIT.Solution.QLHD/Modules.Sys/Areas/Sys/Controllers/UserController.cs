using Cores.Sys.Caches.Sys;
using Cores.Sys.Models.Sys;
using Modules.Sys.Areas.Sys.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Cores.Base.Apps;
using TSFramework.App.Attributes;
using TSFramework.App.Models;
using TSFramework.App.Processors;
using TSFramework.Core.Enums;
using TSFramework.Core.Members.Mail;
using TSFramework.Core.Providers;
using TSFramework.Core.Utils;

namespace Modules.Sys.Areas.Sys.Controllers
{
    public class UserController : AppController
    {
        private readonly string _avatarFolderPath = ConfigurationManager.AppSettings["AppAvatarFolder_Path"];
        private readonly SysConfigCache _configsCache = new SysConfigCache();
        private readonly SysModuleCache _moduleCache = new SysModuleCache();
        private readonly SysRoleCache _roleCache = new SysRoleCache();
        private readonly SysElnvAccountCache _elnvCache = new SysElnvAccountCache();
        private readonly string _elnvtTitle = AppProcessor.Messagor.GetMessage("ElnvAccount_Title");

        private readonly string _roleTitle = AppProcessor.Messagor.GetMessage("Role_Title");
        private readonly SysUserCache _userCache = new SysUserCache();
        private readonly string _userTitle = AppProcessor.Messagor.GetMessage("User_Title");

        private static string[] _arrPermissionViaUser;

        // GET: User
        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        public ActionResult Index()
        {
            _arrPermissionViaUser = GetPermissionViaUser(User.UserName);

            return View(_arrPermissionViaUser);
        }

        #region Extend Functions

        private void SaveAvatar(HttpPostedFileBase avatarFileBase, string virtualAvatarFolderPath, int? employeeId)
        {
            if (avatarFileBase == null || employeeId <= 0 || string.IsNullOrEmpty(virtualAvatarFolderPath)) return;
            var absoluteAvatarFolderPath = HostingEnvironment.MapPath(virtualAvatarFolderPath);
            if (string.IsNullOrEmpty(absoluteAvatarFolderPath)) return;

            absoluteAvatarFolderPath = Path.Combine(absoluteAvatarFolderPath, employeeId.ToString());

            if (!Directory.Exists(absoluteAvatarFolderPath)) Directory.CreateDirectory(absoluteAvatarFolderPath);

            avatarFileBase.SaveAs(Path.Combine(absoluteAvatarFolderPath, $"avatar{Path.GetExtension(avatarFileBase.FileName)}"));
        }

        #endregion

        #region Main Action

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

            var searchCondititions = new BaseSearchModel
            {
                Search = string.IsNullOrEmpty(search) ? null : search,
                Order = order,
                OrderDir = orderDir,
                StartIndex = startRec,
                PageSize = pageSize
            };
            var lstUsers = _userCache.Get(out int total, searchCondititions);
            lstUsers.ForEach(u =>
            {
                u.AvatarPath = string.IsNullOrEmpty(u.Avatar)
                    ? "/Contents/Base/imgs/avatar-default.png"
                    : System.IO.File.Exists(Server.MapPath($"{_avatarFolderPath}/{u.UserId.ToString()}/{u.Avatar}")) ? $"{_avatarFolderPath}/{u.UserId.ToString()}/{u.Avatar}" : "/Contents/Base/imgs/avatar-default.png";
            });

            var result = Json(
                new { draw = Convert.ToInt32(draw), recordsTotal = total, recordsFiltered = total, data = lstUsers }, JsonRequestBehavior.AllowGet);
            return result;
        }

        [AjaxOnly]
        [ActionType(Type = EnumActionType.Add)]
        [HttpGet]
        public ActionResult Add()
        {
            var model = new SysUserModel { ListRoles = _roleCache.GetAll() };
            return PartialView("_Add", model);
        }

        [HttpPost]
        [ActionType(Type = EnumActionType.Add)]
        public ActionResult Add(SysUserModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ListRoles = _roleCache.GetAll();
                return PartialView("_User", model);
            }

            model.RoleIDs = string.IsNullOrEmpty(model.RoleIDs) ? null : model.RoleIDs.Trim(',');
            model.Password = EString.GenerateStrongPassword(8);

            var mailNewUser = new SysMailUserModel
            {
                FullName = model.FullName,
                UserName = model.UserName,
                Email = model.Email,
                Password = model.Password,
                HostUrl = Request.Url?.Host,
                SupportEmail = _configsCache.GetViaKey("Email_Support")?.ConfigValue
            };

            var salt = UPasswordHash.GenerateSalt(model.Password);
            var passwordHash = UPasswordHash.GenerateCryptoPassword(model.Password, salt);

            var userId = _userCache.Save(new SysUserModel
            {
                UserId = 0,
                FullName = model.FullName,
                UserName = model.UserName,
                Email = model.Email,
                Password = passwordHash,
                Salt = salt,
                Avatar = model.AvatarFileBase != null ? $"avatar{Path.GetExtension(model.AvatarFileBase?.FileName)}" : null,//string.IsNullOrEmpty(model.Avatar) ? model.AvatarFileBase?.FileName : model.Avatar,
                Phone = model.Phone,
                RoleIDs = model.RoleIDs,
                IsActive = true,
                Reason = "Thêm mới"
                //HostlUrl = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, "")
            }, User.UserName);
            if (userId == -9)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_userTitle} <b>[{model.UserName}]</b>", EnumProcessType.DataExisted, EnumMsgIcon.Error)
                });
            if (userId > 0)
            {
                SaveAvatar(model.AvatarFileBase, _avatarFolderPath, userId);

                var dataHtml = RenderTemplateHtmlProvider.RenderPartialToHtml(
                    HostingEnvironment.MapPath(ConfigurationManager.AppSettings["EmailTemplates_TemplateNewUser"]),
                    mailNewUser);

                AppProcessor.Mailer.PushEmail(new List<MailModel>
                {
                    new MailModel
                    {
                        From = null,
                        DisplayNameFrom = null,
                        Subject =
                            $"[{AppProcessor.Messagor.GetMessage("App_Title")}] Thông tin tài khoản {model.FullName}",
                        To = new List<string> { model.Email },
                        IsBodyHtml = true,
                        Body = dataHtml,
                        DicImgs = new Dictionary<string, byte[]>
                        {
                            {
                                "LogoVNPT",
                                System.IO.File.ReadAllBytes(
                                    $"{Server.MapPath(ConfigurationManager.AppSettings["Logo_VNPT_Path"])}")
                            },
                            {
                                "LogoApp",
                                System.IO.File.ReadAllBytes(
                                    $"{Server.MapPath(ConfigurationManager.AppSettings["Logo_App_Path"])}")
                            }
                        }
                    }
                });
            }

            var response = CreateMessage($"{_userTitle} [{model.FullName}]", EnumProcessType.Add, userId > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response });
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult Edit(int id = 0)
        {
            var model = _userCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_userTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            model.RoleIDs = string.Join(",", _userCache.GetRoles(model.UserId).Select(g => g.RoleId));
            model.ListRoles = _roleCache.GetAll();
            model.AvatarPath = string.IsNullOrEmpty(model.Avatar)
                ? null
                : $"{_avatarFolderPath}/{model.UserId.ToString()}/{model.Avatar}";
            return PartialView("_Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult Edit(SysUserModel model)
        {
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");
            if (ModelState.IsValid)
            {
                int? userId;
                model.RoleIDs = string.IsNullOrEmpty(model.RoleIDs) ? null : model.RoleIDs.Trim(',');

                if (!string.IsNullOrEmpty(model.Password))
                    userId = _userCache.Save(new SysUserModel
                    {
                        UserId = model.UserId,
                        FullName = model.FullName,
                        UserName = model.UserName,
                        Email = model.Email,
                        Phone = model.Phone,
                        RoleIDs = model.RoleIDs,
                        IsActive = true,
                        Reason = model.Reason
                    }, User.UserName);
                else
                    userId = _userCache.Save(new SysUserModel
                    {
                        UserId = model.UserId,
                        FullName = model.FullName,
                        UserName = model.UserName,
                        Email = model.Email,
                        Phone = model.Phone,
                        Password = null,
                        Salt = null,
                        Avatar = model.AvatarFileBase != null ? $"avatar{Path.GetExtension(model.AvatarFileBase?.FileName)}" : model.Avatar,//string.IsNullOrEmpty(model.Avatar) ? model.AvatarFileBase?.FileName : model.Avatar,
                        RoleIDs = model.RoleIDs,
                        IsActive = true,
                        Reason = model.Reason
                    }, User.UserName);
                if (userId == -9)
                    return Json(new
                    {
                        status = true,
                        message = CreateMessage($"{_userTitle} <b>[{model.UserName}]</b>", EnumProcessType.DataExisted, EnumMsgIcon.Error)
                    });

                SaveAvatar(model.AvatarFileBase, _avatarFolderPath, userId);

                var response = CreateMessage($"{_userTitle} [{model.FullName}]", EnumProcessType.Edit,
                    userId > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);
                return Json(new { status = true, message = response });
            }

            model.ListRoles = _roleCache.GetAll();
            return PartialView("_User", model);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(int id = 0)
        {
            var model = _userCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_userTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Delete"),
                $"<b>{_userTitle} [{model.FullName}]</b>");
            model.Reason = "Xóa tài khoản";

            return PartialView("_Delete", model);
        }

        [HttpPost]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult Delete(SysUserModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Delete"),
                    $"<b>{_userTitle} [{model.FullName}]</b>");
                return PartialView("_DeleteBody", model);
            }
            var deleted = _userCache.Delete(model, User.UserName);

            var response = CreateMessage($"{_userTitle} [{model.FullName}]", EnumProcessType.Delete,
                deleted ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response });
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult IsExistUser(string userName, int userId)
        {
            var model = _userCache.GetByUserName(userName);
            if (model == null)
                return Json(new
                {
                    status = false,
                    message = CreateMessage($"{_userTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            if (userId < 1)
                return Json(new
                {
                    status = false
                });
            if (model.UserId != userId)
                return Json(new
                {
                    status = false
                });
            return Json(new
            {
                status = true
            });
        }

        [HttpGet]
        [AjaxOnly]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult ChangePassword(int id = 0)
        {
            var currentUser = _userCache.GetById(id);
            if (currentUser == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage("Tài khoản", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                }, JsonRequestBehavior.AllowGet);
            return PartialView("_ChangePassword", new ChangePasswordModel
            {
                FullName = currentUser.FullName,
                UserName = currentUser.UserName,
                Email = currentUser.Email
            });
        }

        [HttpPost]
        [AjaxOnly]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            ModelState.Remove("CurrentPassword");
            if (!ModelState.IsValid) return PartialView("_Password", model);
            if (!Regex.IsMatch(model.NewPassword, @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\W)"))
            {
                ModelState.AddModelError("NewPassword",
                    "Mật khẩu phải ít nhất có 1 chữ hoa, 1 chữ thường và một kí tự đặt biệt");
                return PartialView("_Password", model);
            }

            var salt = UPasswordHash.GenerateSalt(model.NewPassword);
            var passwordHash = UPasswordHash.GenerateCryptoPassword(model.NewPassword, salt);

            var userId = _userCache.ResetPassword(
                model.UserName,
                passwordHash,
                salt,
                model.Reason,
                User.UserName
            );
            switch (userId)
            {
                case -1:
                    return Json(new
                    {
                        status = false,
                        message = CreateMessage("Tài khoản không tồn tại hoặc đã khoá.", EnumProcessType.NonFormat, EnumMsgIcon.Error)
                    }, JsonRequestBehavior.AllowGet);
                default:
                    //AppProcessor.Notifider.ForceLogout(model.UserName);
                    return Json(new
                    {
                        status = true,
                        message = CreateMessage("Mật khẩu", EnumProcessType.Edit, EnumMsgIcon.Success)
                    }, JsonRequestBehavior.AllowGet);
            }
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult ResetPassword(int id = 0)
        {
            var model = _userCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_userTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            if (!model.IsActive)
                return Json(new
                {
                    status = true,
                    message = CreateMessage(
                        $"{_userTitle} <b>[{model.FullName} - {model.UserName}]</b> đã ngưng hoạt động.", EnumProcessType.NonFormat, EnumMsgIcon.Error)
                });
            model.Reason = "Reset password";
            ViewBag.ConfirmMessage =
                $"Bạn muốn đặt lại mật khẩu cho tài khoản <b>[{model.FullName} - {model.UserName}]</b>?";
            return PartialView("_ResetPassword", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult ResetPassword(SysUserModel model)
        {
            var userModel = _userCache.GetById(model.UserId.GetValueOrDefault(0));
            if (userModel == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_userTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            if (!userModel.IsActive)
                return Json(new
                {
                    status = true,
                    message = CreateMessage(
                        $"{_userTitle} <b>[{model.FullName} - {model.UserName}]</b> đã ngưng hoạt động.",
                        EnumProcessType.NonFormat, EnumMsgIcon.Error)
                });

            model.HostUrl = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, "");
            var baseToken = $"{userModel.Password}-{DateTime.Now.AddHours(24).Ticks}";
            var passPharse = userModel.Password;
            var tokenResetPassword = EStringCipher.Encrypt(baseToken, passPharse);
            model.DetailUrl = Url.Action("ResetPassword", "Account",
                new { area = "", userName = model.UserName, token = tokenResetPassword });

            var mailBodyHtml = RenderTemplateHtmlProvider.RenderPartialToHtml(
                HostingEnvironment.MapPath(ConfigurationManager.AppSettings["EmailTemplates_TemplateResetPassword"]),
                model);

            AppProcessor.Mailer.PushEmail(new List<MailModel>
            {
                new MailModel
                {
                    Subject =
                        $"[{AppProcessor.Messagor.GetMessage("App_Title")}] {AppProcessor.Messagor.GetMessage("MailSubject_ResetPassword_Message")}",
                    To = new List<string> { model.Email },
                    Body = mailBodyHtml,
                    IsBodyHtml = true,
                    DisplayNameFrom = AppProcessor.Messagor.GetMessage("App_Owner_DisplayName"),
                    DicImgs = new Dictionary<string, byte[]>
                    {
                        {
                            "LogoVNPT",
                            System.IO.File.ReadAllBytes(
                                $"{Server.MapPath(ConfigurationManager.AppSettings["Logo_VNPT_Path"])}")
                        },
                        {
                            "LogoApp",
                            System.IO.File.ReadAllBytes(
                                $"{Server.MapPath(ConfigurationManager.AppSettings["Logo_App_Path"])}")
                        }
                    }
                }
            });

            return Json(new
            {
                status = true,
                message = CreateMessage($"Đã gửi yêu cầu đặt lại mật khẩu đến <b>{_userTitle} [{model.Email}]</b> ", EnumProcessType.NonFormat, EnumMsgIcon.Success)
            }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult DeActive(int id = 0)
        {
            var model = _userCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_userTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            model.Reason = string.Empty;
            ViewBag.ConfirmMessage = $"Bạn muốn ngưng hoạt động <b>{_userTitle} [{model.FullName}]</b> ?";
            return PartialView("_DeActive", model);
        }

        [HttpPost]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult DeActive(SysUserModel model)
        {
            if (ModelState.IsValidField("Reason"))
            {
                var isSuccess = _userCache.DeActive(model, User.UserName);

                var response = CreateMessage($"Ngưng hoạt động <b>{_userTitle} [{model.FullName}]</b> thành công",
                    EnumProcessType.NonFormat, isSuccess ? EnumMsgIcon.Success : EnumMsgIcon.Error);
                return Json(new { status = true, message = response });
            }

            ViewBag.ConfirmMessage = $"Bạn muốn ngưng hoạt động <b>{_userTitle} [{model.FullName}]</b>";
            return PartialView("_DeActiveBody", model);
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult Active(int id = 0)
        {
            var model = _userCache.GetById(id);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_userTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            ViewBag.ConfirmMessage = $"Bạn muốn kích hoạt lại <b>{_userTitle} [{model.FullName}]</b>";
            return PartialView("_Active", model);
        }

        [HttpPost]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult Active(SysUserModel model)
        {
            if (ModelState.IsValidField("Reason"))
            {
                var isSuccess = _userCache.Active(model, User.UserName);

                var response = CreateMessage($"Kích hoạt <b>{_userTitle} [{model.FullName}]</b> thành công",
                    EnumProcessType.NonFormat, isSuccess ? EnumMsgIcon.Success : EnumMsgIcon.Error);
                return Json(new { status = true, message = response });
            }

            ViewBag.ConfirmMessage = $"Bạn muốn kích hoạt lại <b>{_userTitle} [{model.FullName}]</b>";
            return PartialView("_ActiveBody", model);
        }

        #endregion

        #region User Via Role

        [AjaxOnly]
        [ActionType(Type = EnumActionType.View)]
        [HttpPost]
        public ActionResult GetUsersViaRole(int? roleId)
        {
            var search = Request.Form.GetValues("search[value]")?[0];
            var draw = Request.Form.GetValues("draw")?[0];
            var order = Request.Form.GetValues("order[0][column]")?[0];
            var orderDir = Request.Form.GetValues("order[0][dir]")?[0];
            var startRec = Convert.ToInt32(Request.Form.GetValues("start")?[0]);
            var pageSize = Convert.ToInt32(Request.Form.GetValues("length")?[0]);

            var searchCondititions = new BaseSearchModel
            {
                Search = string.IsNullOrEmpty(search) ? null : search,
                Order = order,
                OrderDir = orderDir,
                StartIndex = startRec,
                PageSize = pageSize
            };

            var dataUsers = _userCache.GetViaRole(roleId, out int total, searchCondititions);

            var result =
                Json(
                    new
                    {
                        draw = Convert.ToInt32(draw),
                        recordsTotal = total,
                        recordsFiltered = total,
                        data = dataUsers
                    }, JsonRequestBehavior.AllowGet);
            return result;
        }

        [AjaxOnly]
        [ActionType(Type = EnumActionType.Add)]
        [HttpGet]
        public ActionResult UsersViaRole(int roleId)
        {
            var model = _roleCache.GetById(roleId);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_roleTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            return PartialView("_UsersViaRole", model);
        }

        [AjaxOnly]
        [ActionType(Type = EnumActionType.Add)]
        [HttpGet]
        public ActionResult AddUser(int roleId)
        {
            var model = _roleCache.GetById(roleId);
            if (model == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_roleTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            var lstSelectedUsers = _userCache.GetViaRole(roleId, out _);
            model.ListUsers = _userCache.GetAll().Where(u => !lstSelectedUsers.Exists(su => su.UserId == u.UserId))
                .ToList();

            return PartialView("_AddUser", model);
        }

        [HttpPost]
        [ActionType(Type = EnumActionType.Add)]
        public ActionResult AddUser(SysRoleModel model)
        {
            if (!ModelState.IsValid)
            {
                var lstSelectedUsers = _userCache.GetViaRole(model.RoleId, out _);
                model.ListUsers = _userCache.GetAll().Where(u => !lstSelectedUsers.Exists(su => su.UserId == u.UserId))
                    .ToList();
                return PartialView("_UserRole", model);
            }

            var roleId = _roleCache.AddUser(new SysRoleModel
            {
                RoleId = model.RoleId,
                Users = model.Users
                //Users = string.Join(",", model.SelectedUsers)
            });

            var response = CreateMessage(
                string.Format(AppProcessor.Messagor.GetMessage("Add_User_To_Role"), $"[{model.Name}]"),
                EnumProcessType.Add,
                roleId > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);
            return Json(new { status = true, message = response });
        }

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult RemoveUser(int roleId, int userId)
        {
            var userModel = _userCache.GetById(userId);
            if (userModel == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_userTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var roleModel = _roleCache.GetById(roleId);
            if (roleModel == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_roleTitle}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });
            roleModel.UserId = userId;

            ViewBag.ConfirmMessage = string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Delete"),
                string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Remove_From_Role"),
                    $"<b>[{userModel.FullName}]</b>", $"<b>[{roleModel.Name}]</b>"));
            return PartialView("_RemoveUser", roleModel);
        }

        [HttpPost]
        [ActionType(Type = EnumActionType.Delete)]
        public ActionResult RemoveUser(SysRoleModel model)
        {
            var isSuccess = _roleCache.RemoveUser(model.RoleId, model.UserId);
            var userModel = _userCache.GetById(model.UserId);
            var roleModel = _roleCache.GetById(model.RoleId);

            var response = CreateMessage(
                string.Format(AppProcessor.Messagor.GetMessage("Message_Confirm_Remove_From_Role"),
                    $"<b>[{userModel.FullName}]</b>", $"<b>[{roleModel.Name}]</b>"), EnumProcessType.Delete,
                isSuccess ? EnumMsgIcon.Success : EnumMsgIcon.Error);

            return Json(new { status = true, message = response });
        }

        #endregion

        #region Permit

        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult Permit(int id = 0)
        {
            var userModel = _userCache.GetById(id);
            if (userModel == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"{_userTitle}",
                        EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                });

            var rolesViaUser = _userCache.GetRoles(userModel.UserId).Select(g => g.RoleId).ToList();
            var modulesViaUser = _moduleCache.GetByUserName(userModel.UserName).Select(g => g.ModuleId).ToList();

            var userPermitModel = new UserPermitModel
            {
                Email = userModel.Email,
                FullName = userModel.FullName,
                UserName = userModel.UserName,
                UserId = userModel.UserId,
                OfficeName = userModel.OfficeName,

                ListRoleIDs = rolesViaUser,
                RoleIDs = string.Join(",", rolesViaUser),
                ListRoles = _roleCache.GetAll(),

                ListModuleIDs = modulesViaUser,
                ModuleIDs = string.Join(",", modulesViaUser),
                ListModules = _moduleCache.GetAll()
            };

            return PartialView("_Permit", userPermitModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult Permit(UserPermitModel model)
        {
            if (ModelState.IsValid)
            {
                model.RoleIDs = string.Join(",", model.ListRoleIDs);
                model.ModuleIDs = string.Join(",", model.ListModuleIDs);

                var retInt = _userCache.Permit(model.UserId, model.RoleIDs, model.ModuleIDs);

                var response =
                    CreateMessage(
                        $"{AppProcessor.Messagor.GetMessage("Modal_Title_Accessibility")} - {_userTitle} [{model.FullName}]",
                        EnumProcessType.Edit, retInt > 0 ? EnumMsgIcon.Success : EnumMsgIcon.Error);
                return Json(new { status = true, message = response });
            }

            model.ListRoles = _roleCache.GetAll();
            model.ListModules = _moduleCache.GetAll();

            return PartialView("_PermitBody", model);
        }

        #endregion

        #region InvAccount

        // GET: Sys/ElnvAccount
        [AjaxOnly]
        [HttpGet]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult EditElnvAccount(int userId, string userName)
        {
            var dataUser = _userCache.GetById(userId);
            var dataElnv = _elnvCache.GetById(userId) ?? new SysElnvAccountModel
            {
                UserId = userId,
                EmpAccount = userName,
                FullName = dataUser.FullName
            };

            dataElnv.FullName = dataUser.FullName;
            return PartialView("_EditElnvAccount", dataElnv);
        }

        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.Edit)]
        public ActionResult EditElncAccount(SysElnvAccountModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_ElnvAccount", model);
            }
            string response;

            var retSave = _elnvCache.Save(model, User.UserName);

            if (retSave == 0)
                response = CreateMessage($"{_elnvtTitle} [{model.FullName} - {model.ElnvAccount}]",
                    EnumProcessType.Edit, EnumMsgIcon.Error);
            else if (retSave == -9)
                response = CreateMessage($"{_elnvtTitle}  [{model.FullName} - {model.ElnvAccount}]",
                    EnumProcessType.DataExisted, EnumMsgIcon.Error);
            else
                response = CreateMessage($"{_elnvtTitle}  [{model.FullName} - {model.ElnvAccount}]",
                    EnumProcessType.Edit, EnumMsgIcon.Success);
            return Json(new { status = true, message = response }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}