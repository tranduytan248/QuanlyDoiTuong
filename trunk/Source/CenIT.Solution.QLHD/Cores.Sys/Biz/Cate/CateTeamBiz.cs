using System.Collections.Generic;
using System.Linq;
using Cores.Sys.Models.Cate;
using TSFramework.App.Models;
using TSFramework.App.Processors;

namespace Cores.Sys.Biz.Cate
{
    public class CateTeamBiz
    {
        private const string DATA_PROVIDER_NAME = "MCSProvider";

        private readonly string _cateTeamDelete = "Cate_Team_Delete";
        private readonly string _cateTeamGet = "Cate_Team_Get";
        private readonly string _cateTeamGetById = "Cate_Team_GetById";
        private readonly string _cateTeamGetByWardId = "Cate_Team_GetByWardId";
        private readonly string _cateTeamSave = "Cate_Team_Save";

        public List<CateTeamModel> LoadList(out int total, string provinceIds, string wardIds, BaseSearchModel search)
        {
            search = search ?? new BaseSearchModel
            {
                Search = null,
                Order = "1",
                OrderDir = "ASC",
                StartIndex = 0,
                PageSize = -1
            };

            var listTeams = AppProcessor.ProcedureProvider.ExecuteTypedList<CateTeamModel>(_cateTeamGet,
                DATA_PROVIDER_NAME,
                provinceIds,
                wardIds,
                search.Search,
                search.Order,
                search.OrderDir,
                search.StartIndex,
                search.PageSize);

            total = 0;
            if (listTeams != null && listTeams.Count > 0)
                total = int.Parse(listTeams.First()?.TotalRow.ToString() ?? "0");
            return listTeams;
        }

        private CateTeamModel LoadDetail(int? teamId)
        {
            var lstTeamModels =
                AppProcessor.ProcedureProvider.ExecuteScalarObject<CateTeamModel>(_cateTeamGetById, DATA_PROVIDER_NAME,
                    teamId);

            return lstTeamModels;
        }

        public int? Save(CateTeamModel model)
        {
            var id = AppProcessor.ProcedureProvider.Execute(_cateTeamSave, DATA_PROVIDER_NAME,
                model.TeamId,
                model.WardId,
                model.TeamCode,
                model.TeamName,
                model.UserCreated
            );

            return id;
        }

        public bool Delete(CateTeamModel model)
        {
            var result =
                AppProcessor.ProcedureProvider.Execute(_cateTeamDelete, DATA_PROVIDER_NAME, model.TeamId,
                    model.UserCreated);
            return result == model.TeamId;
        }

        public CateTeamModel GetById(int? teamId)
        {
            var team = LoadDetail(teamId);
            return team;
        }

        public List<CateTeamModel> GetViaWardId(string wardIds)
        {
            var listTeams =
                AppProcessor.ProcedureProvider.ExecuteTypedList<CateTeamModel>(_cateTeamGetByWardId, DATA_PROVIDER_NAME,
                    wardIds);
            return listTeams;
        }

        public List<CateTeamModel> GetAll(string provinceIds = null, string wardIds = null)
        {
            return LoadList(out var _, provinceIds, wardIds, null);
        }
    }
}