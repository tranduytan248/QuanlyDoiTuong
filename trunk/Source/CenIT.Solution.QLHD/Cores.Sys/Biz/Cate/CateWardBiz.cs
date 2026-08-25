using System.Collections.Generic;
using System.Linq;
using Cores.Sys.Models.Cate;
using TSFramework.App.Models;
using TSFramework.App.Processors;

namespace Cores.Sys.Biz.Cate
{
    public class CateWardBiz
    {
        private const string DATA_PROVIDER_NAME = "MCSProvider";
        private readonly string _cateWardDelete = "Cate_Ward_Delete";
        private readonly string _cateWardGet = "Cate_Ward_Get";
        private readonly string _cateWardGetById = "Cate_Ward_GetById";
        private readonly string _cateWardGetByProvinceCode = "Cate_Ward_GetByProvinceCode";

        private readonly string _cateWardGetByProvinceId = "Cate_Ward_GetByProvinceId";
        private readonly string _cateWardSave = "Cate_Ward_Save";

        public List<CateWardModel> LoadList(string provincesIds, out int total, BaseSearchModel search)
        {
            search = search ?? new BaseSearchModel
            {
                Search = null,
                Order = "1",
                OrderDir = "ASC",
                StartIndex = 0,
                PageSize = -1
            };

            var listWards = AppProcessor.ProcedureProvider.ExecuteTypedList<CateWardModel>(_cateWardGet,
                DATA_PROVIDER_NAME,
                provincesIds,
                search.Search,
                search.Order,
                search.OrderDir,
                search.StartIndex,
                search.PageSize);

            total = 0;
            if (listWards != null && listWards.Count > 0)
                total = int.Parse(listWards.First()?.TotalRow.ToString() ?? "0");
            return listWards;
        }

        private CateWardModel LoadDetail(int? wardId)
        {
            var lstWardModels =
                AppProcessor.ProcedureProvider.ExecuteScalarObject<CateWardModel>(_cateWardGetById, DATA_PROVIDER_NAME,
                    wardId);
            return lstWardModels;
        }

        public List<CateWardModel> GetAll(string provincesIds = null)
        {
            return LoadList(provincesIds, out _, null);
        }

        public int? Save(CateWardModel model)
        {
            var id = AppProcessor.ProcedureProvider.Execute(_cateWardSave, DATA_PROVIDER_NAME,
                model.WardId,
                model.ProvinceId,
                model.WardCode,
                model.WardName,
                model.UserCreated
            );

            return id;
        }

        public bool Delete(CateWardModel model)
        {
            var result =
                AppProcessor.ProcedureProvider.Execute(_cateWardDelete, DATA_PROVIDER_NAME, model.WardId,
                    model.UserCreated);
            return result == model.WardId;
        }

        public CateWardModel GetById(int? wardId)
        {
            var ward = LoadDetail(wardId);
            return ward;
        }

        public List<CateWardModel> GetByProvinceId(int? provinceId, out int total, BaseSearchModel search)
        {
            search = search ?? new BaseSearchModel
            {
                Search = null,
                Order = "1",
                OrderDir = "ASC",
                StartIndex = 0,
                PageSize = -1
            };
            var listWards = AppProcessor.ProcedureProvider.ExecuteTypedList<CateWardModel>(_cateWardGetByProvinceId,
                DATA_PROVIDER_NAME, provinceId,
                search.Search,
                search.Order,
                search.OrderDir,
                search.StartIndex,
                search.PageSize);
            total = 0;
            if (listWards != null && listWards.Count > 0)
                total = int.Parse(listWards.First()?.TotalRow.ToString() ?? "0");
            return listWards;
        }

        public List<CateWardModel> GetByProvinceCode(string provinceCode, out int total, BaseSearchModel search)
        {
            search = search ?? new BaseSearchModel
            {
                Search = null,
                Order = "1",
                OrderDir = "ASC",
                StartIndex = 0,
                PageSize = -1
            };
            var listWards = AppProcessor.ProcedureProvider.ExecuteTypedList<CateWardModel>(_cateWardGetByProvinceCode,
                DATA_PROVIDER_NAME, provinceCode,
                search.Search,
                search.Order,
                search.OrderDir,
                search.StartIndex,
                search.PageSize);
            total = 0;
            if (listWards != null && listWards.Count > 0)
                total = int.Parse(listWards.First()?.TotalRow.ToString() ?? "0");
            return listWards;
        }

        public List<CateWardModel> GetByProvinceId(int? provinceId)
        {
            var search = new BaseSearchModel
            {
                Search = null,
                Order = "1",
                OrderDir = "ASC",
                StartIndex = 0,
                PageSize = -1
            };
            var listWards = AppProcessor.ProcedureProvider.ExecuteTypedList<CateWardModel>(_cateWardGetByProvinceId,
                DATA_PROVIDER_NAME, provinceId,
                search.Search,
                search.Order,
                search.OrderDir,
                search.StartIndex,
                search.PageSize);

            return listWards;
        }

        public List<CateWardModel> GetByProvinceCode(string provinceCode)
        {
            var search = new BaseSearchModel
            {
                Search = null,
                Order = "1",
                OrderDir = "ASC",
                StartIndex = 0,
                PageSize = -1
            };
            var listWards = AppProcessor.ProcedureProvider.ExecuteTypedList<CateWardModel>(_cateWardGetByProvinceCode,
                DATA_PROVIDER_NAME, provinceCode,
                search.Search,
                search.Order,
                search.OrderDir,
                search.StartIndex,
                search.PageSize);

            return listWards;
        }

        public List<CateWardModel> GetByDistrict(int? districtId)
        {
            if (districtId == null) return new List<CateWardModel>();
            return AppProcessor.ProcedureProvider.ExecuteTypedList<CateWardModel>("PR_SYS_CATE_WARD_GET_BY_DISTRICT", DATA_PROVIDER_NAME, districtId) ?? new List<CateWardModel>();
        }
    }
}