using CaptchaMvc.HtmlHelpers;
using CenIT.Solution.QLHD.WebApp.Models;
using Cores.Sys.Caches.Sys;
using Cores.Sys.Models.Sys;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Web.SessionState;
using Cores.Base.Apps;
using Cores.Cate.Caches;
using TSFramework.App.Attributes;
using TSFramework.App.Principals;
using TSFramework.App.Processors;
using TSFramework.Core.Enums;
using TSFramework.Core.Members.Mail;
using TSFramework.Core.Providers;
using TSFramework.Core.Utils;

namespace CenIT.Solution.QLHD.WebApp.Controllers
{
    //[AllowAnonymous]
    public class AccountController : AppController
    {
        private const string SESSION_VARIABLE_NAME = "SessionNumber";

        private readonly SysUserCache _userCache = new SysUserCache();
        private readonly CateUnionCache _unionCache = new CateUnionCache();

        private readonly string _defaultAvatar = ConfigurationManager.AppSettings["AppAvatarDefault_URL"];
        private readonly string _avatarFolderPath = ConfigurationManager.AppSettings["AppAvatarFolder_Path"];
        
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl = "")
        {
            if (Session[SESSION_VARIABLE_NAME] == null) Session[SESSION_VARIABLE_NAME] = 0;

            Session.Clear();
            Session.Abandon();
            var ssidManager = new SessionIDManager();
            ssidManager.RemoveSessionID(System.Web.HttpContext.Current);
            var newId = ssidManager.CreateSessionID(System.Web.HttpContext.Current);
            ssidManager.SaveSessionID(System.Web.HttpContext.Current, newId, out _, out _);
            ViewBag.ReturnUrl = returnUrl;
            FormsAuthentication.SignOut();
            if (Request.IsAjaxRequest()) Response.StatusCode = 401;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl = "")
        {
            if (model.NeedCaptcha)
                if (!this.IsCaptchaValid("Captcha không chính xác"))
                    return PartialView("_LoginBody", model);

            int iRequestCount;
            if (Session[SESSION_VARIABLE_NAME] == null) // should not happen!
            {
                iRequestCount = 1;
                Session[SESSION_VARIABLE_NAME] = 1;
            }
            else
            {
                var n = (int)Session[SESSION_VARIABLE_NAME];
                n++;
                Session[SESSION_VARIABLE_NAME] = n;
                iRequestCount = n;
            }

            model.LoginFailCount = iRequestCount;

            if (iRequestCount >= 3) model.NeedCaptcha = true;

            if ((!ModelState.IsValidField("UserName") && !ModelState.IsValidField("Email")) ||
                !ModelState.IsValidField("Password")) return PartialView("_LoginBody", model);

            model.SenderIP = Request.UserHostAddress;
            model.SenderHeader = string.Join(",", Request.Headers);

            var isAuthen = Membership.ValidateUser(model.UserName, model.Password);
            if (!isAuthen)
            {
                var msgAuthIncorrect = AppProcessor.Messagor.GetMessage("Authorize_LoginIncorrect");
                SendResponseNotify("MsgLoginFail", msgAuthIncorrect, EnumProcessType.NonFormat, EnumMsgIcon.Error);
                _userCache.SaveLogin(model.UserName, false, model.SenderIP, model.SenderHeader);
                model.NeedCaptcha = iRequestCount >= 3;

                return PartialView("_LoginBody", model);
            }

            //model.UserName = "ntdthu_np";
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
            //HttpContext.Current.Response.Cookies.Add(faCookie);

            Request.RequestContext.HttpContext.Response.Cookies.Add(faCookie);

            _userCache.SaveLogin(model.UserName, true, model.SenderIP, model.SenderHeader);

            if (!Url.IsLocalUrl(returnUrl) || string.IsNullOrEmpty(returnUrl) || returnUrl == "/" || returnUrl.ToLower().Contains("/account/login") || returnUrl.ToLower().Contains("/dashboard"))
                returnUrl = Url.Action("Index", "Subject", new { area = "Major" });

            return Json(new
            {
                status = true,
                returnUrl,
                message = CreateMessage("Đăng nhập thành công.", EnumProcessType.NonFormat, EnumMsgIcon.Success, EnumMsgPlacement.TopCenter)
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AllowAnyPermission]
        public ActionResult ChangePassword(string userName)
        {
            var currentUser = _userCache.GetByUserName(userName);
            if (currentUser == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage("Tài khoản", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                }, JsonRequestBehavior.AllowGet);
            return PartialView("_ChangePassword", new ChangePasswordModel
            {
                UserName = currentUser.UserName
            });
        }

        [HttpPost]
        [AllowAnyPermission]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid) return PartialView("_PasswordModel", model);

            var isAuthen = Membership.ValidateUser(model.UserName, model.CurrentPassword);
            if (!isAuthen)
            {
                ModelState.AddModelError("CurrentPassword", "Mật khẩu hiện tại không đúng");
                return PartialView("_PasswordModel", model);
            }

            if (!Regex.IsMatch(model.NewPassword, @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\W)"))
            {
                ModelState.AddModelError("NewPassword",
                    "Mật khẩu phải ít nhất có 1 chữ hoa, 1 chữ thường và một kí tự đặt biệt");
                return PartialView("_PasswordModel", model);
            }

            var salt = UPasswordHash.GenerateSalt(model.NewPassword);
            var passwordHash = UPasswordHash.GenerateCryptoPassword(model.NewPassword, salt);

            var userId = _userCache.ResetPassword(
                model.UserName,
                passwordHash,
                salt,
                "Đổi mật khẩu",
                User.UserName
            );
            switch (userId)
            {
                case -1:
                    return Json(new
                    {
                        status = false,
                        message = CreateMessage("Tài khoản không tồn tại hoặc đã khoá.",
                            EnumProcessType.NonFormat, EnumMsgIcon.Error)
                    }, JsonRequestBehavior.AllowGet);
                default:
                    AppProcessor.Notifider.ForceLogout(model.UserName);
                    return Json(new
                    {
                        status = true,
                        message = CreateMessage("Đổi Mật khẩu thành công. Hệ thống sẽ thực hiện đăng xuất khỏi hệ thống.", EnumProcessType.NonFormat, EnumMsgIcon.Success)
                    }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ResetPassword(string userName, string token)
        {
            var currentUser = _userCache.GetByUserName(userName);
            if (currentUser == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"Tài khoản {userName}", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                }, JsonRequestBehavior.AllowGet);

            if (!currentUser.IsActive)
                return Json(new
                {
                    status = true,
                    message = CreateMessage(
                        $"Tài khoản <b>[{currentUser.FullName} - {currentUser.UserName}]</b> đã ngưng hoạt động.",
                        EnumProcessType.NonFormat, EnumMsgIcon.Error)
                });

            var resetPasswordModel = new ResetPasswordModel { UserName = currentUser.UserName };

            var decryptToken = EStringCipher.Decrypt(token, currentUser.Password, out var isCorrect);
            if (isCorrect && !string.IsNullOrEmpty(decryptToken))
            {
                var arrTokens = decryptToken.Split('-');
                var dataTicks = long.Parse(arrTokens.Length > 0 ? arrTokens[1] : "0");
                var timeExpier = new DateTime(dataTicks);
                if (timeExpier >= DateTime.Now) return PartialView("ResetPassword", resetPasswordModel);
                resetPasswordModel.ErrMessage = AppProcessor.Messagor.GetMessage("ResetPassword_Token_Expired_Message");
                resetPasswordModel.IsPermit = false;
                return PartialView("ResetPassword", resetPasswordModel);
            }

            resetPasswordModel.ErrMessage = AppProcessor.Messagor.GetMessage("ResetPassword_Token_Incorrect_Message");
            resetPasswordModel.IsPermit = false;

            return PartialView("ResetPassword", resetPasswordModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid) return PartialView("_ResetPassword", model);

            if (!Regex.IsMatch(model.NewPassword, @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\W)"))
            {
                ModelState.AddModelError("NewPassword",
                    "Mật khẩu phải ít nhất có 1 chữ hoa, 1 chữ thường và một kí tự đặt biệt");
                return PartialView("_ResetPassword", model);
            }

            var salt = UPasswordHash.GenerateSalt(model.NewPassword);
            var passwordHash = UPasswordHash.GenerateCryptoPassword(model.NewPassword, salt);

            var userId = _userCache.ResetPassword(
                model.UserName,
                passwordHash,
                salt,
                "Đặt lại mật khẩu",
                model.UserName
            );
            switch (userId)
            {
                case -1:
                    return Json(new
                    {
                        status = false,
                        message = CreateMessage("Tài khoản không tồn tại hoặc đã khoá.",
                            EnumProcessType.NonFormat, EnumMsgIcon.Error)
                    }, JsonRequestBehavior.AllowGet);
                default:
                    AppProcessor.Notifider.ForceLogout(model.UserName);
                    return Json(new
                    {
                        status = true,
                        message = CreateMessage("Mật khẩu",
                            EnumProcessType.Edit, EnumMsgIcon.Success)
                    }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [AllowAnyPermission]
        public ActionResult EditInfo(string userName)
        {
            if (userName != User.UserName)
            {
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"Tài khoản [{userName}]", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                }, JsonRequestBehavior.AllowGet);
            }

            var userInfo = _userCache.GetByUserName(userName);
            if (userInfo == null)
                return Json(new
                {
                    status = true,
                    message = CreateMessage($"Tài khoản [{userName}]", EnumProcessType.DataNotExist, EnumMsgIcon.Error)
                }, JsonRequestBehavior.AllowGet);
            return PartialView("_EditInfo", userInfo);
        }

        [HttpPost]
        [AllowAnyPermission]
        public ActionResult EditInfo(SysUserModel model)
        {
            if (!ModelState.IsValid) return PartialView("_UserInfo", model);
            if (model.AvatarFileBase != null)
            {
                model.Avatar = $"Avatar{Path.GetExtension(model.AvatarFileBase?.FileName)}";
            }

            var userId = _userCache.UpdateInfo(model, User.UserName);
            switch (userId)
            {
                case -9:
                    return Json(new
                    {
                        status = false,
                        message = CreateMessage($"Tài khoản [{model.UserName}] không tồn tại hoặc đã khoá.",
                            EnumProcessType.NonFormat, EnumMsgIcon.Error)
                    }, JsonRequestBehavior.AllowGet);
                case -1:
                    return Json(new
                    {
                        status = false,
                        message = CreateMessage($"Cập nhật thông tin tài khoản [{model.UserName}]", EnumProcessType.NonFormat, EnumMsgIcon.Error)
                    }, JsonRequestBehavior.AllowGet);
                default:
                    SaveAvatar(model.AvatarFileBase, _avatarFolderPath, userId);
                    return Json(new
                    {
                        status = true,
                        message = CreateMessage($"Cập nhật thông tin tài khoản [{model.UserName}]. Vui lòng đăng nhập lại để xem thay đổi thông tin.", EnumProcessType.NonFormat, EnumMsgIcon.Success)
                    }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult Logout(string returnUrl = "")
        {
            var typeMembershipProvider = ConfigurationManager.AppSettings["AppMembershipType"];
            var typeMembershipProviderNotRedirect = ConfigurationManager.AppSettings["MembershipProviderNotRedirect"];

            var isSuccess = Membership.DeleteUser(User.Identity.Name, true);
            if (!isSuccess) return RedirectToAction("Index", "Home");
            if (!string.IsNullOrEmpty(typeMembershipProviderNotRedirect) &&
                typeMembershipProviderNotRedirect.Split(',').Contains(typeMembershipProvider))
                return RedirectToAction("Login", "Account", new { returnUrl });
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account", new { returnUrl });
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return PartialView("_ForgotPassword", new LoginModel());
        }

        [HttpPost]
        [AjaxOnly]
        [AllowAnonymous]
        public ActionResult ForgotPassword(LoginModel model)
        {
            if (string.IsNullOrEmpty(model.Email))
                return Json(new
                {
                    status = false,
                    message = CreateMessage("Bạn chưa nhập Email.", EnumProcessType.NonFormat, EnumMsgIcon.Error)
                }, JsonRequestBehavior.AllowGet);

            if (!EString.IsValidEmail(model.Email))
                return Json(new
                {
                    status = false,
                    message = CreateMessage("Email không đúng định dạng.", EnumProcessType.NonFormat, EnumMsgIcon.Error)
                }, JsonRequestBehavior.AllowGet);

            var userModel = _userCache.GetByEmail(model.Email);
            if (userModel == null)
                return Json(new
                {
                    status = false,
                    message = CreateMessage("Thông tin tài khoản không tồn tại", EnumProcessType.NonFormat,
                        EnumMsgIcon.Error)
                }, JsonRequestBehavior.AllowGet);

            var passPharse = userModel.Password;
            var newPassword = EString.GenerateStrongPassword(8);
            userModel.HostUrl = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, "");
            var baseToken = $"{newPassword}-{DateTime.Now.AddHours(24).Ticks}";
            var tokenResetPassword = EStringCipher.Encrypt(baseToken, passPharse);
            userModel.DetailUrl = Url.Action("ResetPassword", "Account",
                new { area = "", userName = userModel.UserName, token = tokenResetPassword });

            var mailBodyHtml = RenderTemplateHtmlProvider.RenderPartialToHtml(
                HostingEnvironment.MapPath(
                    "~/Contents/Modules/Sys/EmailTemplates/_TemplateResetPassword.cshtml"), userModel);

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
                            //new WebClient().DownloadData(ConfigurationManager.AppSettings["Logo_App_Path"])
                        }
                    }
                }
            });

            return Json(new
            {
                status = true,
                message = CreateMessage($"Đã gửi yêu cầu đặt lại mật khẩu đến địa chỉ Email: <b>[{model.Email}]</b> ",
                    EnumProcessType.NonFormat, EnumMsgIcon.Success)
            }, JsonRequestBehavior.AllowGet);
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
    }
}