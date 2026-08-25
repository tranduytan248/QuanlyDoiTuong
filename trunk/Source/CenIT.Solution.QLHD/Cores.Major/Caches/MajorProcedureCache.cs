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
    public class MajorProcedureCache : CacheLayer
    {
        private MajorProcedureBiz _procedureApi;

        private MajorProcedureBiz Api => _procedureApi ?? (_procedureApi = new MajorProcedureBiz());

        protected override string[] MasterCacheKeyArray =>
            new[] { "ProceduresCache", "DossiersCache", "CENIT.APP.Cache" };

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<MajorProcedureModel> GetAll(string unionIds = null, string typeContracts = null)
        {
            var rawKey = $"AllProcedures-{unionIds}-{typeContracts}";
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<MajorProcedureModel> procedures) return procedures;
            // Item not found in cache - retrieve it and insert it into the cache
            procedures = Api.GetAll(unionIds, typeContracts);
            AddCacheItem(rawKey, procedures);

            return procedures;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<MajorProcedureModel> Get(out int total, string unionIds = null, string typeContracts = null,
            BaseSearchModel search = null)
        {
            var objectKey = EHashMD5.FromObject(search);
            var rawKey = $"ListProcedures-{unionIds}-{typeContracts}-{objectKey}";
            var rawKeyTotal = string.Concat(rawKey, "-Total");
            var cacheTotal = (int?)GetCacheItem(rawKeyTotal);
            total = cacheTotal ?? 0;
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<MajorProcedureModel> procedures) return procedures;
            // Item not found in cache - retrieve it and insert it into the cache
            procedures = Api.Get(out total, unionIds, typeContracts, search);
            AddCacheItem(rawKey, procedures);
            AddCacheItem(rawKeyTotal, total);

            return procedures;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MajorProcedureModel GetById(Guid? procedureId)
        {
            if (procedureId == null) return null;

            var rawKey = string.Concat("ProcedureByID-", procedureId);

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is MajorProcedureModel procedure) return procedure;
            // Item not found in cache - retrieve it and insert it into the cache
            procedure = Api.GetById(procedureId);
            if (procedure != null) AddCacheItem(rawKey, procedure);

            return procedure;
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int? Save(MajorProcedureModel model)
        {
            var procedureId = Api.Save(model);
            if (procedureId > 0)
                // Invalidate the cache
                InvalidateCache();
            return procedureId;
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int? Clone(MajorProcedureModel model)
        {
            var procedureId = Api.Clone(model);
            if (procedureId > 0)
                // Invalidate the cache
                InvalidateCache();
            return procedureId;
        }

        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public int? Delete(MajorProcedureModel model)
        {
            var ret = Api.Delete(model);
            if (ret > 0)
                // Invalidate the cache
                InvalidateCache();
            return ret;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<MajorProcedureModel> GetViaUnion(Guid? unionId)
        {
            if (unionId == null) return null;

            var rawKey = string.Concat("ProcedureByUnion-", unionId);

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<MajorProcedureModel> lstProcs) return lstProcs;
            // Item not found in cache - retrieve it and insert it into the cache
            lstProcs = Api.GetViaUnion(unionId);
            if (lstProcs != null) AddCacheItem(rawKey, lstProcs);

            return lstProcs;
        }

        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool ToggleStatus(MajorProcedureModel model)
        {
            var ret = Api.ToggleStatus(model);
            if (ret)
                // Invalidate the cache
                InvalidateCache();
            return ret;
        }
    }
}