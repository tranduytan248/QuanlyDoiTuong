using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Cores.Major.Biz;
using Cores.Major.Models;
using TSFramework.App.Models;
using TSFramework.Core.Members.Caching;

namespace Cores.Major.Caches
{
    [DataObject]
    public class MajorDossierCache : CacheLayer
    {
        private MajorDossierBiz _dossierApi;

        private MajorDossierBiz Api => _dossierApi ?? (_dossierApi = new MajorDossierBiz());

        protected override string[] MasterCacheKeyArray => new[] { "DossiersCache", "CENIT.APP.Cache" };

        #region Dossiers

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<MajorDossierModel> GetAll(string userName, string managerUnions = null, string searchValue = null,
            DateTime? receivedFromDate = null, DateTime? receivedToDate = null, DateTime? giveResultFromDate = null,
            DateTime? giveResultToDate = null, string lstStatus = null, string handleTypes = null,
            string typeContractIds = null, string typeCusIds = null)
        {
            var rawKey =
                $"AllDossiers-{userName}-{managerUnions}-{searchValue}-{receivedFromDate}-{receivedToDate}-{giveResultFromDate}-{giveResultToDate}-{lstStatus}-{handleTypes}-{typeContractIds}-{typeCusIds}";
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<MajorDossierModel> dossiers) return dossiers;
            // Item not found in cache - retrieve it and insert it into the cache
            dossiers = Api.GetAll(userName, managerUnions, searchValue, receivedFromDate, receivedToDate,
                giveResultFromDate, giveResultToDate, lstStatus, handleTypes, typeContractIds, typeCusIds);
            AddCacheItem(rawKey, dossiers);

            return dossiers;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<MajorDossierModel> Get(out int total, string userName, string managerUnions = null,
            string searchValue = null, DateTime? receivedFromDate = null, DateTime? receivedToDate = null,
            DateTime? giveResultFromDate = null, DateTime? giveResultToDate = null, string lstStatus = null,
            string handleTypes = null, string typeContractIds = null, string typeCusIds = null,
            BaseSearchModel search = null)
        {
            #region Cache

            //var objectKey = EHashMD5.FromObject(search);
            //var rawKey = $"ListDossiers-{objectKey}-{userName}-{managerUnions}-{searchValue}-{fromDate}-{toDate}-{lstStatus}-{handleTypes}";
            //var rawKeyTotal = string.Concat(rawKey, "-Total");

            //total = 0;
            //var cacheTotal = (int?)GetCacheItem(rawKeyTotal);
            //total = cacheTotal ?? 0;

            //// See if the item is in the cache
            //if (GetCacheItem(rawKey) is List<MajorDossierModel> dossiers) return dossiers;

            //// Item not found in cache - retrieve it and insert it into the cache
            //dossiers = Api.Get(userName, managerUnions, searchValue, fromDate, toDate, lstStatus, handleTypes, out total, search);
            //AddCacheItem(rawKey, dossiers);
            //AddCacheItem(rawKeyTotal, total);

            #endregion

            #region Cache

            var dossiers = Api.Get(out total, userName, managerUnions, searchValue, receivedFromDate, receivedToDate,
                giveResultFromDate, giveResultToDate, lstStatus, handleTypes, typeContractIds, typeCusIds, search);

            #endregion

            return dossiers;
        }


        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MajorDossierModel GetById(Guid? dossierId)
        {
            if (dossierId == null) return null;

            var rawKey = string.Concat("DossierByID-", dossierId);

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is MajorDossierModel dossier) return dossier;
            // Item not found in cache - retrieve it and insert it into the cache
            dossier = Api.GetById(dossierId);
            if (dossier != null) AddCacheItem(rawKey, dossier);

            return dossier;
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int? Save(MajorDossierModel model)
        {
            var dossierId = Api.Save(model);
            if (dossierId > 0)
                // Invalidate the cache
                InvalidateCache();
            return dossierId;
        }

        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public int? Delete(MajorDossierModel model)
        {
            var ret = Api.Delete(model);
            if (ret > 0)
                // Invalidate the cache
                InvalidateCache();
            return ret;
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int? Approve(MajorApproveDossierModel model)
        {
            var dossierId = Api.Approve(model);
            if (dossierId > 0)
                // Invalidate the cache
                InvalidateCache();
            return dossierId;
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int? UpdateProcConfig(MajorDossierModel model)
        {
            var dossierId = Api.UpdateProcConfig(model);
            if (dossierId > 0)
                // Invalidate the cache
                InvalidateCache();
            return dossierId;
        }

        #endregion

        #region Task Dossier

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<MajorDossierTaskModel> GetTasks(Guid? dossierId)
        {
            if (dossierId == null) return null;

            var rawKey = string.Concat("DossierTaskByDossierId-", dossierId);

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<MajorDossierTaskModel> dossierTasks) return dossierTasks;
            // Item not found in cache - retrieve it and insert it into the cache
            dossierTasks = Api.GetTasks(dossierId);
            if (dossierTasks != null) AddCacheItem(rawKey, dossierTasks);

            return dossierTasks;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MajorDossierTaskModel GetTaskById(Guid? taskId)
        {
            if (taskId == null) return null;

            var rawKey = string.Concat("DossierTaskByID-", taskId);

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is MajorDossierTaskModel dossierTask) return dossierTask;
            // Item not found in cache - retrieve it and insert it into the cache
            dossierTask = Api.GetTaskById(taskId);
            if (dossierTask != null) AddCacheItem(rawKey, dossierTask);

            return dossierTask;
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int? SwitchHandler(Guid? taskId, string handlingComments, DataTable dataHandlers, string saveBy)
        {
            var retInt = Api.SwitchHandler(taskId, handlingComments, dataHandlers, saveBy);
            if (retInt > 0)
                // Invalidate the cache
                InvalidateCache();
            return retInt;
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int? Handle(MajorDossierTaskModel model)
        {
            var dossierId = Api.Handle(model);
            if (dossierId > 0)
                // Invalidate the cache
                InvalidateCache();
            return dossierId;
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int? Complete(MajorDossierTaskModel model)
        {
            var dossierId = Api.Complete(model);
            if (dossierId > 0)
                // Invalidate the cache
                InvalidateCache();
            return dossierId;
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int? ChangeHandler(MajorDossierTaskModel model)
        {
            var dossierId = Api.ChangeHandler(model);
            if (dossierId > 0)
                // Invalidate the cache
                InvalidateCache();
            return dossierId;
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int? PauseTask(MajorDossierTaskModel model)
        {
            var dossierId = Api.PauseTask(model);
            if (dossierId > 0)
                // Invalidate the cache
                InvalidateCache();
            return dossierId;
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int? ContinueTask(MajorDossierTaskModel model)
        {
            var dossierId = Api.ContinueTask(model);
            if (dossierId > 0)
                // Invalidate the cache
                InvalidateCache();
            return dossierId;
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int? SaveRefFiles(MajorDossierTaskModel model)
        {
            var contractId = Api.SaveRefFiles(model);
            if (contractId > 0)
                // Invalidate the cache
                InvalidateCache();
            return contractId;
        }

        #endregion
    }
}