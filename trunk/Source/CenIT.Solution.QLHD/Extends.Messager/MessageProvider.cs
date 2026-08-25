using System;
using System.Collections.Generic;
using Extends.Messager.Core.Caches;
using Extends.Messager.Core.Models;
using TSFramework.App.Models;

namespace Extends.Messager
{
    public class MessageProvider
    {
        private readonly SAVMessageCache _messageCache = new SAVMessageCache();

        public bool Add(string receivers, string contents, string detailUrl, string sendBy)
        {
            var ret = _messageCache.Save(new SAVMessageModel
            {
                Receivers = receivers,
                Contents = contents,
                DetailUrl = detailUrl,
                UpdatedBy = sendBy
            });
            return ret > 0;
        }

        public bool MaskAsRead(Guid? messageId)
        {
            return _messageCache.MaskAsRead(messageId);
        }

        public bool MaskAllAsRead(string userName)
        {
            return _messageCache.MaskAllAsRead(userName);
        }

        public bool Delete(Guid? messageId)
        {
            return _messageCache.Delete(messageId);
        }

        public bool DeleteAll(string userName)
        {
            return _messageCache.DeleteAll(userName);
        }

        public SAVMessageModel Get(Guid? messageId)
        {
            return _messageCache.GetById(messageId);
        }

        public List<SAVMessageModel> Get(string userName, out int iTotal, int? pageSize = 10)
        {
            return _messageCache.Get(userName, out iTotal, new BaseSearchModel
            {
                StartIndex = 0, PageSize = pageSize ?? 10
            });
        }

        public List<SAVMessageModel> Get(string userName, out int iTotal, BaseSearchModel searchModel )
        {
            return _messageCache.Get(userName, out iTotal, searchModel ?? new BaseSearchModel
            {
                StartIndex = 0,
                PageSize = 10
            });
        }
    }
}