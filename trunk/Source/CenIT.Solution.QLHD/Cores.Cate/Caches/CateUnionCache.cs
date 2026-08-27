using System;
using System.Collections.Generic;
using System.ComponentModel;
using Cores.Cate.Biz;
using Cores.Cate.Models;
using TSFramework.App.Models;
using TSFramework.Core.Members.Caching;
using TSFramework.Core.Utils;

namespace Cores.Cate.Caches
{
    [DataObject]
    public class CateUnionCache : CacheLayer
    {
        private CateUnionBiz _unionApi;

        private CateUnionBiz Api => _unionApi ?? (_unionApi = new CateUnionBiz());

        protected override string[] MasterCacheKeyArray =>
            new[] { "UnionsCache", "CENIT.APP.Cache" };

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CateUnionModel> GetAll(string userName = null, string belongUnions = null, string typeUnions = null)
        {
            var rawKey = $"AllUnions-{userName}-{belongUnions}-{typeUnions}";
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<CateUnionModel> lstUnions) return lstUnions;
            // Item not found in cache - retrieve it and insert it into the cache
            lstUnions = Api.GetAll(userName, belongUnions, typeUnions);
            AddCacheItem(rawKey, lstUnions);

            return lstUnions;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CateUnionModel> Get(string userName, string belongUnions, string typeUnions, out int total,
            BaseSearchModel search = null)
        {
            var objectKey = EHashMD5.FromObject(search);
            var rawKey = string.Concat($"ListUnions-{userName}-{belongUnions}-{typeUnions}", objectKey);
            var rawKeyTotal = string.Concat(rawKey, "-Total");
            var cacheTotal = (int?)GetCacheItem(rawKeyTotal);
            total = cacheTotal ?? 0;
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<CateUnionModel> lstUnions) return lstUnions;
            // Item not found in cache - retrieve it and insert it into the cache
            lstUnions = Api.Get(userName, belongUnions, typeUnions, out total, search);
            AddCacheItem(rawKey, lstUnions);
            AddCacheItem(rawKeyTotal, total);

            return lstUnions;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CateUnionModel> GetBelong(Guid? belongUnion, int typeUnion)
        {
            var rawKey = $"ListUnionBelongs-{belongUnion}-{typeUnion}";
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<CateUnionModel> lstUnions) return lstUnions;
            // Item not found in cache - retrieve it and insert it into the cache
            lstUnions = Api.GetBelong(belongUnion, typeUnion);
            AddCacheItem(rawKey, lstUnions);

            return lstUnions;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public CateUnionModel GetById(Guid? unionId)
        {
            if (unionId == null || unionId == Guid.Empty) return null;

            var rawKey = string.Concat("UnionByID-", unionId);

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is CateUnionModel union) return union;
            // Item not found in cache - retrieve it and insert it into the cache
            union = Api.GetById(unionId);
            if (union != null) AddCacheItem(rawKey, union);

            return union;
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int? Save(CateUnionModel model)
        {
            var unionId = Api.Save(model);
            if (unionId > 0)
                // Invalidate the cache
                InvalidateCache();
            return unionId;
        }

        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Delete(CateUnionModel model)
        {
            var isSuccess = Api.Delete(model);
            if (isSuccess)
                // Invalidate the cache
                InvalidateCache();
            return isSuccess;
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public bool ToggleStatus(CateUnionModel model)
        {
            var isSuccess = Api.ToggleStatus(model);
            if (isSuccess)
                // Invalidate the cache
                InvalidateCache();
            return isSuccess;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CateUnionModel> GetNotUsingProc(Guid? procId = null, string typeUnions = null,
            int contractTypeId = 1)
        {
            var rawKey = string.Concat($"ListUnionsNotUsingProc-{procId}-{typeUnions}-{contractTypeId}");

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<CateUnionModel> lstUnions) return lstUnions;
            // Item not found in cache - retrieve it and insert it into the cache
            lstUnions = Api.GetNotUsingProc(procId, typeUnions, contractTypeId);
            AddCacheItem(rawKey, lstUnions);

            return lstUnions;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CateUnionModel> GetUsingProc(Guid? procId = null, string typeUnions = null)
        {
            var rawKey = string.Concat($"ListUnionsUsingProc-{procId}-{typeUnions}");

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<CateUnionModel> lstUnions) return lstUnions;
            // Item not found in cache - retrieve it and insert it into the cache
            lstUnions = Api.GetUsingProc(procId, typeUnions);
            AddCacheItem(rawKey, lstUnions);

            return lstUnions;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CateUnionModel> GetParents(Guid? procId = null, int? typeUnion = null)
        {
            var rawKey = string.Concat($"ListUnionsNotUsingProc-{procId}-{typeUnion}");

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<CateUnionModel> lstUnions) return lstUnions;
            // Item not found in cache - retrieve it and insert it into the cache
            lstUnions = Api.GetParents(procId, typeUnion);
            AddCacheItem(rawKey, lstUnions);

            return lstUnions;
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int? SaveInfo(CateUnionModel model)
        {
            var unionId = Api.SaveInfo(model);
            if (unionId > 0)
                // Invalidate the cache
                InvalidateCache();
            return unionId;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public CateUnionModel GetUnitByUserName(string userName)
        {
            if (string.IsNullOrEmpty(userName)) return null;

            var rawKey = string.Concat("GetUnitByUserName-", userName);

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is CateUnionModel union) return union;
            // Item not found in cache - retrieve it and insert it into the cache
            union = Api.GetUnitByUserName(userName);
            if (union != null) AddCacheItem(rawKey, union);

            return union;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public CateUnionModel GetDeptByMember(string userName)
        {
            var rawKey = $"DeptByMember-{userName}";
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is CateUnionModel union) return union;
            // Item not found in cache - retrieve it and insert it into the cache
            union = Api.GetDeptByMember(userName);
            AddCacheItem(rawKey, union);

            return union;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public CateUnionModel GetUnionByMember(string userName)
        {
            var rawKey = $"UnionByMember-{userName}";
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is CateUnionModel union) return union;
            // Item not found in cache - retrieve it and insert it into the cache
            union = Api.GetUnionByMember(userName);
            AddCacheItem(rawKey, union);

            return union;
        }

        #region Members

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int? SaveMember(CateUnionMemberModel model)
        {
            var retSave = Api.SaveMember(model);
            if (retSave > 0)
                // Invalidate the cache
                InvalidateCache();
            return retSave;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CateUnionMemberModel> GetMembers(Guid? unionId)
        {
            var rawKey = $"MembersViaUnion-{unionId}";
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<CateUnionMemberModel> lstMembers) return lstMembers;
            // Item not found in cache - retrieve it and insert it into the cache
            lstMembers = Api.GetMembers(unionId);
            AddCacheItem(rawKey, lstMembers);

            return lstMembers;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CateUnionMemberModel> GetMembersViaUnion(Guid? unionId, bool? includeChilds = false)
        {
            var rawKey = $"MembersViaUnionId-{unionId}-{includeChilds}";
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<CateUnionMemberModel> lstMembers) return lstMembers;
            // Item not found in cache - retrieve it and insert it into the cache
            lstMembers = Api.GetMembersViaUnion(unionId, includeChilds);
            AddCacheItem(rawKey, lstMembers);

            return lstMembers;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public CateUnionMemberModel GetMemberByKey(Guid? unionId, string userName)
        {
            var rawKey = $"MemberByKey-{unionId}-{userName}";
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is CateUnionMemberModel union) return union;
            // Item not found in cache - retrieve it and insert it into the cache
            union = Api.GetMemberByKey(unionId, userName);
            AddCacheItem(rawKey, union);

            return union;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public CateUnionMemberModel GetMemberByKey(string userName)
        {
            var rawKey = $"MemberByUserName-{userName}";
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is CateUnionMemberModel union) return union;
            // Item not found in cache - retrieve it and insert it into the cache
            union = Api.GetMemberByKey(null, userName);
            AddCacheItem(rawKey, union);

            return union;
        }

        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool DeleteMember(CateUnionMemberModel model)
        {
            var isSuccess = Api.DeleteMember(model);
            if (isSuccess)
                // Invalidate the cache
                InvalidateCache();
            return isSuccess;
        }


        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public CateUnionMemberModel GetMemberInfo(string userName = null)
        {
            var rawKey = $"Unions-ViaMember-{userName}";
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is CateUnionMemberModel unionModel) return unionModel;
            // Item not found in cache - retrieve it and insert it into the cache
            unionModel = Api.GetMemberByKey(null, userName);
            AddCacheItem(rawKey, unionModel);

            return unionModel;
        }

        #endregion


        #region Managers

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int? SaveManager(Guid? unionId, string users, string savedBy)
        {
            var retSave = Api.SaveManager(unionId, users, savedBy);
            if (retSave > 0)
                // Invalidate the cache
                InvalidateCache();
            return retSave;
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int? SaveManagerList(string userName, string unionIds, string savedBy)
        {
            var retSave = Api.SaveManagerList(userName, unionIds, savedBy);
            if (retSave > 0)
                InvalidateCache();
            return retSave;
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int? SaveManager(CateUnionManagerModel model)
        {
            var retSave = Api.SaveManager(model);
            if (retSave > 0)
                // Invalidate the cache
                InvalidateCache();
            return retSave;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CateUnionManagerModel> GetManagers(Guid? unionId)
        {
            var rawKey = $"ManagersUnion-{unionId}";
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<CateUnionManagerModel> lstManagers) return lstManagers;
            // Item not found in cache - retrieve it and insert it into the cache
            lstManagers = Api.GetManagers(unionId);
            AddCacheItem(rawKey, lstManagers);

            return lstManagers;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CateUnionManagerModel> GetUnionsViaManager(string userName)
        {
            var rawKey = $"UnionsViaManager-{userName}";
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<CateUnionManagerModel> lstUnions) return lstUnions;
            // Item not found in cache - retrieve it and insert it into the cache
            lstUnions = Api.GetUnionsViaManager(userName);
            AddCacheItem(rawKey, lstUnions);

            return lstUnions;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public CateUnionManagerModel GetManagerByKey(Guid? unionId, string userName)
        {
            var rawKey = $"ManagerByKey-{unionId}-{userName}";
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is CateUnionManagerModel union) return union;
            // Item not found in cache - retrieve it and insert it into the cache
            union = Api.GetManagerByKey(unionId, userName);
            AddCacheItem(rawKey, union);

            return union;
        }

        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool DeleteManager(CateUnionManagerModel model)
        {
            var isSuccess = Api.DeleteManager(model);
            if (isSuccess)
                // Invalidate the cache
                InvalidateCache();
            return isSuccess;
        }

        #endregion
    }
}