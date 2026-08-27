using System;
using System.Collections.Generic;
using System.Linq;
using Cores.Cate.Models;
using TSFramework.App.Models;
using TSFramework.App.Processors;

namespace Cores.Cate.Biz
{
    public class CateUnionBiz
    {
        private const string DATA_PROVIDER_NAME = "MCSProvider";

        private readonly string _cateUnionDelete = "Cate_Union_Delete";
        private readonly string _cateUnionGet = "Cate_Union_Get";
        private readonly string _cateUnionGetBelong = "Cate_Union_GetBelong";
        private readonly string _cateUnionGetById = "Cate_Union_GetById";
        private readonly string _cateUnionGetDeptByMember = "Cate_Union_GetDeptByMember";
        private readonly string _cateUnionGetMembers = "Cate_Union_GetMembers";
        private readonly string _cateUnionGetNotUsingProc = "Cate_Union_GetNotUsingProc";
        private readonly string _cateUnionGetParents = "Cate_Union_GetParents";
        private readonly string _cateUnionGetUnionByMember = "Cate_Union_GetUnionByMember";
        private readonly string _cateUnionGetUnit = "Cate_Union_GetUnit";
        private readonly string _cateUnionGetUsingProc = "Cate_Union_GetUsingProc";

        private readonly string _cateUnionMemberDelete = "Cate_Union_Member_Delete";
        private readonly string _cateUnionMemberGet = "Cate_Union_Member_Get";
        private readonly string _cateUnionMemberGetByKey = "Cate_Union_Member_GetByKey";
        private readonly string _cateUnionMemberSave = "Cate_Union_Member_Save";
        private readonly string _cateUnionSave = "Cate_Union_Save";
        private readonly string _cateUnionSaveInfo = "Cate_Union_SaveInfo";
        private readonly string _cateUnionToggleStatus = "Cate_Union_ToggleStatus";

        public List<CateUnionModel> Get(string userName, string belongUnions, string typeUnions, out int total,
            BaseSearchModel search)
        {
            search = search ?? new BaseSearchModel
            {
                Search = null,
                Order = "0",
                OrderDir = "ASC",
                StartIndex = 0,
                PageSize = -1
            };

            var lstUnions = AppProcessor.ProcedureProvider.ExecuteTypedList<CateUnionModel>(_cateUnionGet,
                DATA_PROVIDER_NAME, userName, belongUnions, typeUnions,
                search.Search,
                search.Order,
                search.OrderDir,
                search.StartIndex,
                search.PageSize);

            total = 0;
            if (lstUnions != null && lstUnions.Count > 0)
                total = int.Parse(lstUnions.First()?.TotalRow.ToString() ?? "0");
            return lstUnions;
        }

        public CateUnionModel GetById(Guid? unionId)
        {
            var lstUnions =
                AppProcessor.ProcedureProvider.ExecuteScalarObject<CateUnionModel>(_cateUnionGetById,
                    DATA_PROVIDER_NAME, unionId);

            return lstUnions;
        }

        public bool Delete(CateUnionModel model)
        {
            var result =
                AppProcessor.ProcedureProvider.Execute(_cateUnionDelete, DATA_PROVIDER_NAME, model.UnionId,
                    model.Reason, model.UpdatedBy);
            return result == 1;
        }

        public List<CateUnionModel> GetAll(string userName = null, string belongUnions = null, string typeUnions = null)
        {
            var lstUnions = Get(userName, belongUnions, typeUnions, out _, null);
            return lstUnions;
        }

        public int? Save(CateUnionModel model)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_cateUnionSave, DATA_PROVIDER_NAME,
                model.UnionId,
                model.UnionCode,
                model.UnionName,
                model.TypeUnion,
                model.TypeUnionName,
                model.BelongUnion,
                model.Note,
                model.Reason,
                model.UpdatedBy);

            return result;
        }

        public bool ToggleStatus(CateUnionModel model)
        {
            var result =
                AppProcessor.ProcedureProvider.Execute(_cateUnionToggleStatus, DATA_PROVIDER_NAME, model.UnionId,
                    model.IsActive, model.Reason, model.UpdatedBy);
            return result == 1;
        }

        public List<CateUnionModel> GetNotUsingProc(Guid? procId = null, string typeUnions = null,
            int contractTypeId = 1)
        {
            var lstUnions = AppProcessor.ProcedureProvider.ExecuteTypedList<CateUnionModel>(_cateUnionGetNotUsingProc,
                DATA_PROVIDER_NAME, procId, typeUnions, contractTypeId);
            return lstUnions;
        }

        public List<CateUnionModel> GetUsingProc(Guid? procId = null, string typeUnions = null)
        {
            var lstUnions = AppProcessor.ProcedureProvider.ExecuteTypedList<CateUnionModel>(_cateUnionGetUsingProc,
                DATA_PROVIDER_NAME, procId, typeUnions);
            return lstUnions;
        }

        public List<CateUnionModel> GetParents(Guid? unionId = null, int? typeUnion = null)
        {
            var lstUnions = AppProcessor.ProcedureProvider.ExecuteTypedList<CateUnionModel>(_cateUnionGetParents,
                DATA_PROVIDER_NAME, unionId, typeUnion);
            return lstUnions;
        }

        public int? SaveInfo(CateUnionModel model)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_cateUnionSaveInfo, DATA_PROVIDER_NAME,
                model.UnionId,
                model.UnionInfo,
                model.UpdatedBy);

            return result;
        }

        public CateUnionModel GetUnitByUserName(string userName)
        {
            var lstUnions =
                AppProcessor.ProcedureProvider.ExecuteScalarObject<CateUnionModel>(_cateUnionGetUnit,
                    DATA_PROVIDER_NAME, userName);

            return lstUnions;
        }

        public CateUnionModel GetDeptByMember(string userName)
        {
            var unionModel =
                AppProcessor.ProcedureProvider.ExecuteScalarObject<CateUnionModel>(_cateUnionGetDeptByMember,
                    DATA_PROVIDER_NAME, userName);

            return unionModel;
        }

        public CateUnionModel GetUnionByMember(string userName)
        {
            var unionMemberModel = AppProcessor.ProcedureProvider.ExecuteScalarObject<CateUnionModel>(
                _cateUnionGetUnionByMember,
                DATA_PROVIDER_NAME, userName);

            return unionMemberModel;
        }

        public List<CateUnionModel> GetBelong(Guid? belongUnion, int? typeUnion)
        {
            var lstUnions = AppProcessor.ProcedureProvider.ExecuteTypedList<CateUnionModel>(_cateUnionGetBelong,
                DATA_PROVIDER_NAME, belongUnion, typeUnion);
            return lstUnions;
        }

        #region Members

        public int? SaveMember(CateUnionMemberModel model)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_cateUnionMemberSave, DATA_PROVIDER_NAME,
                model.UserName,
                model.UnionId,
                model.PositionId,
                model.Permit,
                model.UpdatedBy);

            return result;
        }

        public List<CateUnionMemberModel> GetMembers(Guid? unionId)
        {
            var lstMembers = AppProcessor.ProcedureProvider.ExecuteTypedList<CateUnionMemberModel>(_cateUnionMemberGet,
                DATA_PROVIDER_NAME, unionId);

            return lstMembers;
        }

        public List<CateUnionMemberModel> GetMembersViaUnion(Guid? unionId, bool? includeChilds = false)
        {
            var lstMembers = AppProcessor.ProcedureProvider.ExecuteTypedList<CateUnionMemberModel>(_cateUnionGetMembers,
                DATA_PROVIDER_NAME, unionId, includeChilds);

            return lstMembers;
        }

        public CateUnionMemberModel GetMemberByKey(Guid? unionId, string userName)
        {
            var unionMemberModel = AppProcessor.ProcedureProvider.ExecuteScalarObject<CateUnionMemberModel>(
                _cateUnionMemberGetByKey,
                DATA_PROVIDER_NAME, unionId, userName);

            return unionMemberModel;
        }

        public bool DeleteMember(CateUnionMemberModel model)
        {
            var result =
                AppProcessor.ProcedureProvider.Execute(_cateUnionMemberDelete, DATA_PROVIDER_NAME, model.UnionId,
                    model.UserName);
            return result == 1;
        }

        #endregion

        #region Managers

        private readonly string _cateUnionManagerGetUnions = "Cate_Union_Manager_GetUnions";
        private readonly string _cateUnionManagerGetManagers = "Cate_Union_Manager_GetManagers";
        private readonly string _cateUnionManagerSave = "Cate_Union_Manager_Save";
        private readonly string _cateUnionManagerSaveList = "Cate_Union_Manager_SaveList";
        private readonly string _cateUnionManagerGetByKey = "Cate_Union_Manager_GetByKey";
        private readonly string _cateUnionManagerDeleteByKey = "Cate_Union_Manager_DeleteByKey";

        public int? SaveManagerList(string userName, string unionIds, string savedBy)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_cateUnionManagerSaveList, DATA_PROVIDER_NAME,
                userName,
                unionIds,
                savedBy);

            return result;
        }

        public int? SaveManager(Guid? unionId, string users, string savedBy)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_cateUnionManagerSave, DATA_PROVIDER_NAME,
                unionId,
                users,
                savedBy);

            return result;
        }

        public int? SaveManager(CateUnionManagerModel model)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_cateUnionManagerSave, DATA_PROVIDER_NAME,
                model.UnionId,
                model.UserName,
                model.UpdatedBy);

            return result;
        }

        public List<CateUnionManagerModel> GetManagers(Guid? unionId)
        {
            var lstUnions = AppProcessor.ProcedureProvider.ExecuteTypedList<CateUnionManagerModel>(
                _cateUnionManagerGetManagers,
                DATA_PROVIDER_NAME, unionId);

            return lstUnions;
        }

        public List<CateUnionManagerModel> GetUnionsViaManager(string userName)
        {
            var lstUnions = AppProcessor.ProcedureProvider.ExecuteTypedList<CateUnionManagerModel>(
                _cateUnionManagerGetUnions,
                DATA_PROVIDER_NAME, userName);

            return lstUnions;
        }

        public CateUnionManagerModel GetManagerByKey(Guid? unionId, string userName)
        {
            var unionMemberModel = AppProcessor.ProcedureProvider.ExecuteScalarObject<CateUnionManagerModel>(
                _cateUnionManagerGetByKey,
                DATA_PROVIDER_NAME, unionId, userName);

            return unionMemberModel;
        }

        public bool DeleteManager(CateUnionManagerModel model)
        {
            var result =
                AppProcessor.ProcedureProvider.Execute(_cateUnionManagerDeleteByKey, DATA_PROVIDER_NAME, model.UnionId,
                    model.UserName);
            return result == 1;
        }

        #endregion
    }
}