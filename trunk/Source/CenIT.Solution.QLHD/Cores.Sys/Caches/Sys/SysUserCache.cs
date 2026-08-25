using System.Collections.Generic;
using System.ComponentModel;
using Cores.Sys.Biz.Sys;
using Cores.Sys.Models.Sys;
using TSFramework.App.Models;
using TSFramework.Core.Members.Caching;
using TSFramework.Core.Utils;

namespace Cores.Sys.Caches.Sys
{
    [DataObject]
    public class SysUserCache : CacheLayer
    {
        private SysUserBiz _userApi;

        protected override string[] MasterCacheKeyArray => new[]
        {
            "SysUsersCache", "SysModuleCache", "SysRolesCache", "SysPermissionsCache", "SysMenusCache",
            "SysFunctionsCache", "CENIT.APP.Cache"
        };

        private SysUserBiz Api => _userApi ?? (_userApi = new SysUserBiz());

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public SysUserModel Login(string userName, string password)
        {
            return Api.Login(userName, password);
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public SysUserModel LoginViaEmail(string email, string password)
        {
            return Api.LoginViaEmail(email, password);
        }

        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public int? Save(SysUserModel model, string savedBy)
        {
            var userId = Api.Save(model, savedBy);
            // Invalidate the cache
            if (userId > 0) InvalidateCache();
            return userId;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<SysUserModel> Get(out int total, BaseSearchModel search = null)
        {
            var objectKey = EHashMD5.FromObject(search);

            var rawKey = string.Concat("ListUsers-", objectKey);
            var rawKeyTotal = string.Concat(rawKey, "-Total");
            total = 0;
            var cacheTotal = (int?)GetCacheItem(rawKeyTotal);
            total = cacheTotal ?? 0;
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<SysUserModel> users) return users;
            // Item not found in cache - retrieve it and insert it into the cache
            users = Api.GetList(out total, search);
            if (users == null) return null;
            AddCacheItem(rawKey, users);
            AddCacheItem(rawKeyTotal, total);

            return users;
        }

        public List<SysUserModel> GetAll()
        {
            var rawKey = "AllUsers";
            if (GetCacheItem(rawKey) is List<SysUserModel> allUsers) return allUsers;
            // Item not found in cache - retrieve it and insert it into the cache
            allUsers = Api.GetAll();
            if (allUsers == null) return null;
            AddCacheItem(rawKey, allUsers);
            return allUsers;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public SysUserModel GetById(int userId)
        {
            if (userId < 0) return null;

            var rawKey = string.Concat("UserByID-", userId);

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is SysUserModel user) return user;
            // Item not found in cache - retrieve it and insert it into the cache
            user = Api.GetById(userId);
            AddCacheItem(rawKey, user);

            return user;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public SysUserModel GetByUserName(string userName)
        {
            if (string.IsNullOrEmpty(userName)) return null;

            var rawKey = string.Concat("UserByUserName-", userName);

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is SysUserModel user) return user;
            // Item not found in cache - retrieve it and insert it into the cache
            user = Api.GetByUserName(userName);
            AddCacheItem(rawKey, user);

            return user;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public SysUserModel GetByEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) return null;

            var rawKey = string.Concat("UserByEmail-", email);

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is SysUserModel user) return user;
            // Item not found in cache - retrieve it and insert it into the cache
            user = Api.GetByEmail(email);
            if (user != null)
                AddCacheItem(rawKey, user);

            return user;
        }

        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Delete(SysUserModel model, string deletedBy)
        {
            var isDeleted = Api.Delete(model, deletedBy);
            if (isDeleted)
                // Invalidate the cache
                InvalidateCache();
            return isDeleted;
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public bool DeActive(SysUserModel model, string deactiveBy)
        {
            var isSuccess = Api.DeActive(model, deactiveBy);
            if (isSuccess)
                // Invalidate the cache
                InvalidateCache();
            return isSuccess;
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public bool Active(SysUserModel model, string activeBy)
        {
            var isSuccess = Api.Active(model, activeBy);
            if (isSuccess)
                // Invalidate the cache
                InvalidateCache();
            return isSuccess;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<SysRoleModel> GetRoles(int? userId)
        {
            var rawKey = string.Concat("RolesByUserID-", userId);
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<SysRoleModel> users) return users;
            // Item not found in cache - retrieve it and insert it into the cache
            users = Api.GetRoles(userId);
            AddCacheItem(rawKey, users);

            return users;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public int? ChangePassword(string userName, string oldPass, string newPass, string salt, string reason,
            string changeBy)
        {
            var valReturn = Api.ChangePassword(userName, oldPass, newPass, salt, reason, changeBy);
            return valReturn;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public int? ResetPassword(string userName, string newPass, string salt, string reason, string changeBy)
        {
            var valReturn = Api.ResetPassword(userName, newPass, salt, reason, changeBy);
            return valReturn;
        }

        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public int? SaveLogin(string userName, bool isValid, string senderIp, string senderHeader)
        {
            var userId = Api.SaveLogin(userName, isValid, senderIp, senderHeader);
            return userId;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<SysUserModel> GetViaRole(int? roleId, out int total, BaseSearchModel search = null)
        {
            var objectKey = EHashMD5.FromObject(search);

            var rawKey = $"ListUsersViaRole-{roleId}-{objectKey}";
            var rawKeyTotal = string.Concat(rawKey, "-Total");
            total = 0;
            var cacheTotal = (int?)GetCacheItem(rawKeyTotal);
            total = cacheTotal ?? 0;
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<SysUserModel> users) return users;
            // Item not found in cache - retrieve it and insert it into the cache
            users = Api.GetViaRole(roleId, out total, search);
            if (users == null) return null;
            AddCacheItem(rawKey, users);
            AddCacheItem(rawKeyTotal, total);

            return users;
        }

        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public int? Permit(int? userId, string roleIds, string moduleIds)
        {
            var retInt = Api.Permit(userId, roleIds, moduleIds);
            if (retInt > 0)
                InvalidateCache();
            return retInt;
        }

        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public int? UpdateInfo(SysUserModel model, string savedBy)
        {
            var userId = Api.UpdateInfo(model, savedBy);
            // Invalidate the cache
            if (userId > 0) InvalidateCache();
            return userId;
        }
    }
}