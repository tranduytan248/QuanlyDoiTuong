using System;
using System.Collections.Generic;
using System.Linq;
using Cores.Major.Models;
using TSFramework.App.Models;
using TSFramework.App.Processors;

namespace Cores.Major.Biz
{
    public class MajorMessageBiz
    {
        private const string DATA_PROVIDER_NAME = "MCSProvider";

        private readonly string _majorMessageDelete = "Major_Message_Delete";
        private readonly string _majorMessageDeleteAll = "Major_Message_DeleteAll";
        private readonly string _majorMessageGet = "Major_Message_Get";
        private readonly string _majorMessageGetById = "Major_Message_GetById";
        private readonly string _majorMessageMaskAllAsRead = "Major_Message_MaskAllAsRead";
        private readonly string _majorMessageMaskAsRead = "Major_Message_MaskAsRead";
        private readonly string _majorMessageSave = "Major_Message_Save";

        public List<MajorMessageModel> Get(string userName, out int total, BaseSearchModel search)
        {
            search = search ?? new BaseSearchModel
            {
                Search = null,
                Order = "1",
                OrderDir = "ASC",
                StartIndex = 0,
                PageSize = -1
            };

            var lstMessages = AppProcessor.ProcedureProvider.ExecuteTypedList<MajorMessageModel>(_majorMessageGet,
                DATA_PROVIDER_NAME, userName,
                search.Search,
                search.Order,
                search.OrderDir,
                search.StartIndex,
                search.PageSize);

            total = 0;
            if (lstMessages != null && lstMessages.Count > 0)
                total = int.Parse(lstMessages.First()?.TotalRow.ToString() ?? "0");
            return lstMessages;
        }

        public MajorMessageModel GetById(Guid? messageId)
        {
            var lstMessages =
                AppProcessor.ProcedureProvider.ExecuteScalarObject<MajorMessageModel>(_majorMessageGetById,
                    DATA_PROVIDER_NAME, messageId);

            return lstMessages;
        }

        public bool Delete(Guid? messageId)
        {
            var result =
                AppProcessor.ProcedureProvider.Execute(_majorMessageDelete, DATA_PROVIDER_NAME, messageId);
            return result == 1;
        }

        public bool DeleteAll(string userName)
        {
            var result =
                AppProcessor.ProcedureProvider.Execute(_majorMessageDeleteAll, DATA_PROVIDER_NAME, userName);
            return result == 1;
        }

        public List<MajorMessageModel> GetAll(string userName)
        {
            var lstMessages = Get(userName, out _, null);
            return lstMessages;
        }

        public int? Save(MajorMessageModel model)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_majorMessageSave, DATA_PROVIDER_NAME,
                model.Receivers,
                model.Title,
                model.Contents,
                model.DetailUrl,
                model.UpdatedBy);

            return result;
        }

        public bool MaskAsRead(Guid? messageId)
        {
            var result =
                AppProcessor.ProcedureProvider.Execute(_majorMessageMaskAsRead, DATA_PROVIDER_NAME, messageId);
            return result == 1;
        }

        public bool MaskAllAsRead(string userName)
        {
            var result =
                AppProcessor.ProcedureProvider.Execute(_majorMessageMaskAllAsRead, DATA_PROVIDER_NAME, userName);
            return result == 1;
        }
    }
}