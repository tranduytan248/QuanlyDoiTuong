using System.Collections.Generic;
using System.Linq;
using Cores.Sys.Models.Cate;
using TSFramework.App.Models;
using TSFramework.App.Processors;

namespace Cores.Sys.Biz.Cate
{
    public class CateProvinceBiz
    {
        private const string DATA_PROVIDER_NAME = "MCSProvider";
        private readonly string _cateProvinceDelete = "Cate_Province_Delete";
        private readonly string _cateProvinceGet = "Cate_Province_Get";
        private readonly string _cateProvinceGetByCode = "Cate_Province_GetByCode";
        private readonly string _cateProvinceGetById = "Cate_Province_GetById";
        private readonly string _cateProvinceGetViaWardId = "Cate_Province_GetViaWardId";
        private readonly string _cateProvinceSave = "Cate_Province_Save";

        private List<CateProvinceModel> Get(out int total, BaseSearchModel search)
        {
            search = search ?? new BaseSearchModel
            {
                Search = null,
                Order = "1",
                OrderDir = "ASC",
                StartIndex = 0,
                PageSize = -1
            };

            var listProvinces = AppProcessor.ProcedureProvider.ExecuteTypedList<CateProvinceModel>(_cateProvinceGet,
                DATA_PROVIDER_NAME,
                search.Search,
                search.Order,
                search.OrderDir,
                search.StartIndex,
                search.PageSize);

            total = 0;
            if (listProvinces != null && listProvinces.Count > 0)
                total = int.Parse(listProvinces.First()?.TotalRow.ToString() ?? "0");
            return listProvinces;
        }

        private CateProvinceModel LoadDetail(int? provinceId)
        {
            var province =
                AppProcessor.ProcedureProvider.ExecuteScalarObject<CateProvinceModel>(_cateProvinceGetById,
                    DATA_PROVIDER_NAME, provinceId);

            return province;
        }

        public bool Delete(CateProvinceModel model)
        {
            var result =
                AppProcessor.ProcedureProvider.Execute(_cateProvinceDelete, DATA_PROVIDER_NAME, model.ProvinceId);
            return result == model.ProvinceId;
        }

        public List<CateProvinceModel> GetAll()
        {
            var listProvinces = Get(out var _, null);
            return listProvinces;
        }

        public CateProvinceModel GetById(int? provinceId)
        {
            var province = LoadDetail(provinceId);
            return province;
        }

        public CateProvinceModel GetViaWard(int? wardId)
        {
            var province =
                AppProcessor.ProcedureProvider.ExecuteScalarObject<CateProvinceModel>(_cateProvinceGetViaWardId,
                    DATA_PROVIDER_NAME, wardId);

            return province;
        }

        public List<CateProvinceModel> GetList(out int total, BaseSearchModel search = null)
        {
            var listProvinces = Get(out total, search);
            return listProvinces;
        }

        public int? Save(CateProvinceModel model)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_cateProvinceSave, DATA_PROVIDER_NAME, model.ProvinceId,
                model.ProvinceCode, model.ProvinceName, model.UserCreated);

            return result;
        }

        public CateProvinceModel GetByCode(string provinceCode)
        {
            var province =
                AppProcessor.ProcedureProvider.ExecuteScalarObject<CateProvinceModel>(_cateProvinceGetByCode,
                    DATA_PROVIDER_NAME, provinceCode);

            return province;
        }
    }
}