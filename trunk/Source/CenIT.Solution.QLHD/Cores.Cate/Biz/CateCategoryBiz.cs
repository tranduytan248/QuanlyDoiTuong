using System;
using System.Collections.Generic;
using System.Linq;
using Cores.Cate.Models;
using TSFramework.App.Models;
using TSFramework.App.Processors;

namespace Cores.Cate.Biz
{
    public class CateCategoryBiz
    {
        private const string DATA_PROVIDER_NAME = "MCSProvider";

        private readonly string _cateCategoryDelete = "Cate_Category_Delete";
        private readonly string _cateCategoryGet = "Cate_Category_Get";
        private readonly string _cateCategoryGetByCode = "Cate_Category_GetByCode";
        private readonly string _cateCategoryGetById = "Cate_Category_GetById";
        private readonly string _cateCategoryGetViolationType = "Cate_Category_GetViolationType";
        private readonly string _cateCategorySave = "Cate_Category_Save";

        public List<CateCategoryModel> Get(string cateTypes, out int total, BaseSearchModel search)
        {
            search = search ?? new BaseSearchModel
            {
                Search = null,
                Order = "0",
                OrderDir = "ASC",
                StartIndex = 0,
                PageSize = -1
            };

            var listCategories = AppProcessor.ProcedureProvider.ExecuteTypedList<CateCategoryModel>(_cateCategoryGet,
                DATA_PROVIDER_NAME, cateTypes,
                search.Search,
                search.Order,
                search.OrderDir,
                search.StartIndex,
                search.PageSize);

            total = 0;
            if (listCategories != null && listCategories.Count > 0)
                total = int.Parse(listCategories.First()?.TotalRow.ToString() ?? "0");
            return listCategories;
        }

        public CateCategoryModel GetById(Guid? cateId)
        {
            var lstCategories =
                AppProcessor.ProcedureProvider.ExecuteScalarObject<CateCategoryModel>(_cateCategoryGetById,
                    DATA_PROVIDER_NAME, cateId);

            return lstCategories;
        }

        public bool Delete(CateCategoryModel model)
        {
            var result =
                AppProcessor.ProcedureProvider.Execute(_cateCategoryDelete, DATA_PROVIDER_NAME, model.CateId,
                    model.UpdatedBy);
            return result == 1;
        }

        public List<CateCategoryModel> GetAll(string cateTypes = null)
        {
            var listCategories = Get(cateTypes, out _, null);
            return listCategories;
        }

        public int? Save(CateCategoryModel model)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_cateCategorySave, DATA_PROVIDER_NAME,
                model.CateId,
                model.CateCode,
                model.CateName,
                model.CateType,
                model.CateTypeName,
                model.CateParentId,
                model.Priority,
                model.Note,
                model.UpdatedBy);
            return result;
        }

        public CateCategoryModel GetByCode(string cateCode)
        {
            var categoryInfo =
                AppProcessor.ProcedureProvider.ExecuteScalarObject<CateCategoryModel>(_cateCategoryGetByCode,
                    DATA_PROVIDER_NAME, cateCode);

            return categoryInfo;
        }

        //get danh sách lĩnh vực vi phạm
        public List<CateCategoryModel> GetViolationType()
        {
            var categoryInfo =
                AppProcessor.ProcedureProvider.ExecuteTypedList<CateCategoryModel>(_cateCategoryGetViolationType,
                    DATA_PROVIDER_NAME);

            return categoryInfo;
        }
    }
}