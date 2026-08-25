using System;
using Cores.Sys.Models.Cate;
using TSFramework.App.Processors;

namespace Cores.Sys.Biz.Cate
{
    public class CateDocBiz
    {
        private const string DATA_PROVIDER_NAME = "MCSProvider";

        private readonly string _cateDocDelete = "Cate_Doc_Delete";
        private readonly string _cateDocGetById = "Cate_Doc_GetById";

        public CateDocModel GetById(Guid? fileId)
        {
            var fileInfo = AppProcessor.ProcedureProvider.ExecuteScalarObject<CateDocModel>(_cateDocGetById,
                    DATA_PROVIDER_NAME, fileId);

            return fileInfo;
        }

        public bool Delete(CateDocModel model)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_cateDocDelete, DATA_PROVIDER_NAME, model.FileId);
            return result == 1;
        }
    }
}