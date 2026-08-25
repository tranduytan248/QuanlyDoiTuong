using System;
using System.Collections.Generic;
using System.ComponentModel;
using Cores.Sys.Biz.Sys;
using Cores.Sys.Models.Sys;
using TSFramework.App.Models;
using TSFramework.Core.Members.Caching;
using TSFramework.Core.Utils;

namespace Cores.Sys.Caches.Sys
{
    public class SysNotificationCache : CacheLayer
    {
        private SysNotificationBiz _notificationApi;
        protected override string[] MasterCacheKeyArray => new[] { "SysNotificationCache", "CENIT.APP.Cache" };
        private SysNotificationBiz Api => _notificationApi ?? (_notificationApi = new SysNotificationBiz());


        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<SysNotificationModel> Get(out int total, BaseSearchModel search = null)
        {
            var objectKey = EHashMD5.FromObject(search);
            var rawKey = string.Concat("Get_Sys_Notification", objectKey);
            var rawKeyTotal = string.Concat(rawKey, "-Total");
            total = 0;
            var cacheTotal = (int?)GetCacheItem(rawKeyTotal);
            total = cacheTotal ?? 0;
            if (GetCacheItem(rawKey) is List<SysNotificationModel> data) return data;
            data = Api.LoadList(out total, search);
            AddCacheItem(rawKey, data);
            AddCacheItem(rawKeyTotal, total);
            return data;
        }


        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public SysNotificationModel GetById(Guid id)
        {
            var rawKey = string.Concat("GetSys_NotificationByID_", id);
            if (GetCacheItem(rawKey) is SysNotificationModel data) return data;
            data = Api.LoadDetail(id);
            AddCacheItem(rawKey, data);
            return data;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<SysNotificationModel> GetAll()
        {
            var rawKey = string.Concat("GetAllSys_Notification");
            if (GetCacheItem(rawKey) is List<SysNotificationModel> data) return data;
            data = Api.GetAll();
            AddCacheItem(rawKey, data);
            return data;
        }

        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public int Delete(SysNotificationModel model)
        {
            var isDeleted = Api.Delete(model.NotificationId);
            if (isDeleted > 0) InvalidateCache();
            return isDeleted;
        }

        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public int Save(SysNotificationModel model, string savedBy)
        {
            var isSaved = Api.Save(model, savedBy);
            if (isSaved > 0) InvalidateCache();
            return isSaved;
        }
    }
}