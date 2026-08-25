using System;
using System.Web;
using System.Web.Security;
using Cores.Sys.Caches.Sys;
using Cores.Sys.Models.Sys;

namespace CenIT.Providers.AppMembership
{
    public class AppMemberShipProvider : MembershipProvider
    {
        #region Private Pro

        private readonly SysUserCache _userApi = new SysUserCache();

        #endregion

        public override bool EnablePasswordRetrieval { get; }
        public override bool EnablePasswordReset { get; }
        public override bool RequiresQuestionAndAnswer { get; }
        public override string ApplicationName { get; set; }
        public override int MaxInvalidPasswordAttempts { get; }
        public override int PasswordAttemptWindow { get; }
        public override bool RequiresUniqueEmail { get; }
        public override MembershipPasswordFormat PasswordFormat { get; }
        public override int MinRequiredPasswordLength { get; }
        public override int MinRequiredNonAlphanumericCharacters { get; }
        public override string PasswordStrengthRegularExpression { get; }

        public override MembershipUser CreateUser(string username, string password, string email,
            string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey,
            out MembershipCreateStatus status)
        {
            var newUser = new SysUserModel
            {
                Email = email,
                IsActive = true,
                Password = password,
                UserName = username,
                CreatedDate = DateTime.Now
            };
            var userId = _userApi.Save(newUser, "MCSProvider");
            status = MembershipCreateStatus.ProviderError;
            if (userId > 0)
                status = MembershipCreateStatus.Success;

            newUser.UserId = userId;
            return new AppMembershipUser(newUser);
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password,
            string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override bool ValidateUser(string email, string password)
        {
            var userModel = _userApi.Login(email, password);
            if (userModel == null) return false;
            var user = (AppMembershipUser)GetUser(email, false);
            if (user == null) return false;
            if (HttpContext.Current == null) return true;

            return true;
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string email, bool userIsOnline)
        {
            var userOnline = _userApi.GetByUserName(email);
            return userOnline == null ? null : new AppMembershipUser(userOnline);
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
            FormsAuthentication.SignOut();
            return true;
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize,
            out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize,
            out int totalRecords)
        {
            throw new NotImplementedException();
        }
    }

    public class AppMembershipUser : MembershipUser
    {
        public AppMembershipUser(SysUserModel user) : base("AppMembership", user.UserName, user.UserId, user.Email,
            string.Empty, string.Empty, true, false, user.CreatedDate.GetValueOrDefault(), DateTime.Now, DateTime.Now,
            DateTime.Now, DateTime.Now)
        {
            UserId = user.UserId;
            Email = user.Email;
            FullName = user.FullName;
            UserName = user.UserName;
            Avatar = user.Avatar;
            CreationDate = user.CreatedDate.GetValueOrDefault();
        }

        #region User Properties

        public int? UserId { get; set; }
        public sealed override string Email { get; set; }
        public string FullName { get; set; }
        public override string UserName { get; }
        public string Avatar { get; }
        public override DateTime CreationDate { get; }

        #endregion
    }
}