using System;
using System.Collections.Generic;
using System.ComponentModel;
using Cores.Major.Biz;
using Cores.Major.Models;
using TSFramework.App.Models;
using TSFramework.Core.Members.Caching;
using TSFramework.Core.Utils;

namespace Cores.Major.Caches
{
    [DataObject]
    public class MajorMessageCache : CacheLayer
    {
        private MajorMessageBiz _messageApi;

        private MajorMessageBiz Api => _messageApi ?? (_messageApi = new MajorMessageBiz());

        protected override string[] MasterCacheKeyArray =>
            new[] { "MajorMessagesCache", "CENIT.APP.Cache" };

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<MajorMessageModel> GetAll(string userName)
        {
            var rawKey = $"AllMajorMessages-{userName}";
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<MajorMessageModel> messages) return messages;
            // Item not found in cache - retrieve it and insert it into the cache
            messages = Api.GetAll(userName);
            AddCacheItem(rawKey, messages);

            return messages;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<MajorMessageModel> Get(string userName, out int total, BaseSearchModel search = null)
        {
            var objectKey = EHashMD5.FromObject(search);
            var rawKey = string.Concat($"ListMajorMessages-{userName}", objectKey);
            var rawKeyTotal = string.Concat(rawKey, "-Total");

            total = 0;
            var cacheTotal = (int?)GetCacheItem(rawKeyTotal);
            total = cacheTotal ?? 0;
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<MajorMessageModel> messages) return messages;
            // Item not found in cache - retrieve it and insert it into the cache
            messages = Api.Get(userName, out total, search);
            AddCacheItem(rawKey, messages);
            AddCacheItem(rawKeyTotal, total);

            return messages;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MajorMessageModel GetById(Guid? messageId)
        {
            if (messageId == null || messageId == Guid.Empty) return null;

            var rawKey = string.Concat("MajorMessageByID-", messageId);

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is MajorMessageModel message) return message;
            // Item not found in cache - retrieve it and insert it into the cache
            message = Api.GetById(messageId);
            if (message != null) AddCacheItem(rawKey, message);

            return message;
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int? Save(MajorMessageModel model)
        {
            var messageId = Api.Save(model);
            if (messageId > 0)
                // Invalidate the cache
                InvalidateCache();
            return messageId;
        }

        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Delete(Guid? messageId)
        {
            var isSuccess = Api.Delete(messageId);
            if (isSuccess)
                // Invalidate the cache
                InvalidateCache();
            return isSuccess;
        }

        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool DeleteAll(string userName)
        {
            var isSuccess = Api.DeleteAll(userName);
            if (isSuccess)
                // Invalidate the cache
                InvalidateCache();
            return isSuccess;
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public bool MaskAsRead(Guid? messageId)
        {
            var isSuccess = Api.MaskAsRead(messageId);
            if (isSuccess)
                // Invalidate the cache
                InvalidateCache();
            return isSuccess;
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public bool MaskAllAsRead(string userName)
        {
            var isSuccess = Api.MaskAllAsRead(userName);
            if (isSuccess)
                // Invalidate the cache
                InvalidateCache();
            return isSuccess;
        }
    }
}