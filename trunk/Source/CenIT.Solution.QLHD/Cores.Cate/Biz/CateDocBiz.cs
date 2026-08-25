using System;
using System.Collections.Generic;
using Cores.Cate.Models;
using TSFramework.App.Processors;

namespace Cores.Cate.Biz
{
    public class CateDocBiz
    {
        private const string DATA_PROVIDER_NAME = "MCSProvider";

        private readonly string _cateDocDelete = "Cate_Doc_Delete";
        private readonly string _cateDocGetById = "Cate_Doc_GetById";
        private readonly string _cateDocGetByObjectId = "Cate_Doc_GetByObjectId";

        public CateDocModel GetById(Guid? fileId)
        {
            var fileInfo = AppProcessor.ProcedureProvider.ExecuteScalarObject<CateDocModel>(_cateDocGetById,
                DATA_PROVIDER_NAME, fileId);

            return fileInfo;
        }

        public List<CateDocModel> GetByObjectId(string objectId)
        {
            var lstFileInfos = AppProcessor.ProcedureProvider.ExecuteTypedList<CateDocModel>(_cateDocGetByObjectId,
                DATA_PROVIDER_NAME, objectId);

            return lstFileInfos;
        }

        public bool Delete(CateDocModel model)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_cateDocDelete, DATA_PROVIDER_NAME, model.FileId);
            return result == 1;
        }
    }
}