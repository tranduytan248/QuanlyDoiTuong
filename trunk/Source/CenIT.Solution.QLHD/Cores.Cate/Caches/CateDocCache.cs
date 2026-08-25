using System;
using System.Collections.Generic;
using System.ComponentModel;
using Cores.Cate.Biz;
using Cores.Cate.Models;
using TSFramework.Core.Members.Caching;

namespace Cores.Cate.Caches
{
    [DataObject]
    public class CateDocCache : CacheLayer
    {
        private CateDocBiz _docApi;

        private CateDocBiz Api => _docApi ?? (_docApi = new CateDocBiz());

        protected override string[] MasterCacheKeyArray =>
            new[] { "DocsCache", "CENIT.APP.Cache" };

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public CateDocModel GetById(Guid? docId)
        {
            if (docId == null || docId == Guid.Empty) return null;

            var rawKey = string.Concat("DocByID-", docId);

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is CateDocModel doc) return doc;
            // Item not found in cache - retrieve it and insert it into the cache
            doc = Api.GetById(docId);
            if (doc != null) AddCacheItem(rawKey, doc);

            return doc;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CateDocModel> GetByObjectId(string objId)
        {
            if (string.IsNullOrEmpty(objId)) return null;

            var rawKey = string.Concat("ListDocsByObjectId-", objId);

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<CateDocModel> lstDocs) return lstDocs;
            // Item not found in cache - retrieve it and insert it into the cache
            lstDocs = Api.GetByObjectId(objId);
            if (lstDocs != null) AddCacheItem(rawKey, lstDocs);

            return lstDocs;
        }

        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Delete(CateDocModel model)
        {
            var isDeleted = Api.Delete(model);
            if (isDeleted)
                // Invalidate the cache
                InvalidateCache();
            return isDeleted;
        }
    }
}