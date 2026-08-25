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
    public class MajorProcedureStepCache : CacheLayer
    {
        private MajorProcedureStepBiz _stepApi;

        private MajorProcedureStepBiz Api => _stepApi ?? (_stepApi = new MajorProcedureStepBiz());

        protected override string[] MasterCacheKeyArray =>
            new[] { "ProcedureStepsCache", "ProceduresCache", "DossiersCache", "CENIT.APP.Cache" };

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<MajorProcedureStepModel> GetAll(string procedureIds = null)
        {
            var rawKey = $"AllProcedureSteps-{procedureIds}";
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<MajorProcedureStepModel> lstSteps) return lstSteps;
            // Item not found in cache - retrieve it and insert it into the cache
            lstSteps = Api.GetAll(procedureIds);
            AddCacheItem(rawKey, lstSteps);

            return lstSteps;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<MajorProcedureStepModel> Get(string procedureIds, out int total, BaseSearchModel search = null)
        {
            var objectKey = EHashMD5.FromObject(search);
            var rawKey = string.Concat($"ListProcedureSteps-{procedureIds}-", objectKey);
            var rawKeyTotal = string.Concat(rawKey, "-Total");
            var cacheTotal = (int?)GetCacheItem(rawKeyTotal);
            total = cacheTotal ?? 0;
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<MajorProcedureStepModel> lstSteps) return lstSteps;
            // Item not found in cache - retrieve it and insert it into the cache
            lstSteps = Api.Get(procedureIds, out total, search);
            AddCacheItem(rawKey, lstSteps);
            AddCacheItem(rawKeyTotal, total);

            return lstSteps;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MajorProcedureStepModel GetById(Guid? stepId)
        {
            if (stepId == null) return null;

            var rawKey = string.Concat("ProcedureStepByID-", stepId);

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is MajorProcedureStepModel step) return step;
            // Item not found in cache - retrieve it and insert it into the cache
            step = Api.GetById(stepId);
            if (step != null) AddCacheItem(rawKey, step);

            return step;
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int? Save(MajorProcedureStepModel model)
        {
            var stepId = Api.Save(model);
            if (stepId > 0)
                // Invalidate the cache
                InvalidateCache();
            return stepId;
        }

        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Delete(MajorProcedureStepModel model)
        {
            var isSuccess = Api.Delete(model);
            if (isSuccess)
                // Invalidate the cache
                InvalidateCache();
            return isSuccess;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MajorProcedureStepModel GetByKey(Guid? procId, string stepName)
        {
            if (procId == null || string.IsNullOrEmpty(stepName)) return null;

            var rawKey = $"ProcedureStepByKey-{procId}-{stepName}";

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is MajorProcedureStepModel step) return step;
            // Item not found in cache - retrieve it and insert it into the cache
            step = Api.GetByKey(procId, stepName);
            if (step != null) AddCacheItem(rawKey, step);

            return step;
        }

        #region Handler

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<MajorProcedureStepHandlerModel> GetHandlers(Guid? stepId)
        {
            var rawKey = $"ListProcedureStepsHandlers-{stepId}";
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<MajorProcedureStepHandlerModel> lstStepsHandlers) return lstStepsHandlers;
            // Item not found in cache - retrieve it and insert it into the cache
            lstStepsHandlers = Api.GetHandlers(stepId);
            AddCacheItem(rawKey, lstStepsHandlers);

            return lstStepsHandlers;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MajorProcedureStepHandlerModel GetHandlerById(Guid? stepId, Guid? unionId)
        {
            if (stepId == null) return null;

            var rawKey = $"ProcedureStepsHandlersByID-{stepId}";

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is MajorProcedureStepHandlerModel stepHandler) return stepHandler;
            // Item not found in cache - retrieve it and insert it into the cache
            stepHandler = Api.GetHandlerById(stepId, unionId);
            if (stepHandler != null) AddCacheItem(rawKey, stepHandler);

            return stepHandler;
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int? SaveHandler(MajorProcedureStepHandlerModel model)
        {
            var stepHandlerId = Api.SaveHandler(model);
            if (stepHandlerId > 0)
                // Invalidate the cache
                InvalidateCache();
            return stepHandlerId;
        }

        #endregion

        #region HandlingTime

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<MajorProcedureStepHandlingTimeModel> GetHandlingTimes(Guid? stepId)
        {
            var rawKey = $"ListProcedureStepsHandlingTimes-{stepId}";
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<MajorProcedureStepHandlingTimeModel> lstStepsHandlingTimes)
                return lstStepsHandlingTimes;
            // Item not found in cache - retrieve it and insert it into the cache
            lstStepsHandlingTimes = Api.GetHandlingTimes(stepId);
            AddCacheItem(rawKey, lstStepsHandlingTimes);

            return lstStepsHandlingTimes;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MajorProcedureStepHandlingTimeModel GetHandlingTimeById(Guid? handlingTimeById)
        {
            if (handlingTimeById == null) return null;

            var rawKey = $"ProcedureStepsHandlingTimeByID-{handlingTimeById}";

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is MajorProcedureStepHandlingTimeModel stepHandlingTime) return stepHandlingTime;
            // Item not found in cache - retrieve it and insert it into the cache
            stepHandlingTime = Api.GetHandlingTimeById(handlingTimeById);
            if (stepHandlingTime != null) AddCacheItem(rawKey, stepHandlingTime);

            return stepHandlingTime;
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int? SaveHandlingTime(MajorProcedureStepHandlingTimeModel model)
        {
            var stepHandlingTimeId = Api.SaveHandlingTime(model);
            if (stepHandlingTimeId > 0)
                // Invalidate the cache
                InvalidateCache();
            return stepHandlingTimeId;
        }

        #endregion

        #region Situation

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<MajorProcedureStepSituationModel> GetSituations(Guid? stepId)
        {
            var rawKey = $"ListProcedureStepsSituations-{stepId}";
            // See if the item is in the cache
            if (GetCacheItem(rawKey) is List<MajorProcedureStepSituationModel> lstStepsSituations)
                return lstStepsSituations;
            // Item not found in cache - retrieve it and insert it into the cache
            lstStepsSituations = Api.GetSituations(stepId);
            AddCacheItem(rawKey, lstStepsSituations);

            return lstStepsSituations;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MajorProcedureStepSituationModel GetSituationById(Guid? stepId, Guid? unionId)
        {
            if (stepId == null) return null;

            var rawKey = $"ProcedureStepsSituationsByID-{stepId}";

            // See if the item is in the cache
            if (GetCacheItem(rawKey) is MajorProcedureStepSituationModel stepSituation) return stepSituation;
            // Item not found in cache - retrieve it and insert it into the cache
            stepSituation = Api.GetSituationById(stepId, unionId);
            if (stepSituation != null) AddCacheItem(rawKey, stepSituation);

            return stepSituation;
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int? SaveSituation(MajorProcedureStepSituationModel model)
        {
            var stepSituationId = Api.SaveSituation(model);
            if (stepSituationId > 0)
                // Invalidate the cache
                InvalidateCache();
            return stepSituationId;
        }

        #endregion
    }
}