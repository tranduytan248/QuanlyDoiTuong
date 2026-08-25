using System;
using System.Collections.Generic;
using System.Linq;
using Extends.Messager.Core.Models;
using TSFramework.App.Models;
using TSFramework.App.Processors;

namespace Extends.Messager.Core.Biz
{
    public class ExtMessageBiz
    {
        private const string DATA_PROVIDER_NAME = "MCSProvider";

        private readonly string _savMessageDelete = "SAV_Message_Delete";
        private readonly string _savMessageDeleteAll = "SAV_Message_DeleteAll";
        private readonly string _savMessageGet = "SAV_Message_Get";
        private readonly string _savMessageGetById = "SAV_Message_GetById";
        private readonly string _savMessageMaskAllAsRead = "SAV_Message_MaskAllAsRead";
        private readonly string _savMessageMaskAsRead = "SAV_Message_MaskAsRead";
        private readonly string _savMessageSave = "SAV_Message_Save";

        public List<MessageModel> Get(string userName, out int total, BaseSearchModel search)
        {
            search = search ?? new BaseSearchModel
            {
                Search = null,
                Order = "1",
                OrderDir = "ASC",
                StartIndex = 0,
                PageSize = -1
            };

            var lstMessages = AppProcessor.ProcedureProvider.ExecuteTypedList<MessageModel>(_savMessageGet,
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

        public MessageModel GetById(Guid? messageId)
        {
            var lstCategories =
                AppProcessor.ProcedureProvider.ExecuteScalarObject<MessageModel>(_savMessageGetById,
                    DATA_PROVIDER_NAME, messageId);

            return lstCategories;
        }

        public bool Delete(Guid? messageId)
        {
            var result =
                AppProcessor.ProcedureProvider.Execute(_savMessageDelete, DATA_PROVIDER_NAME, messageId);
            return result == 1;
        }

        public bool DeleteAll(string userName)
        {
            var result =
                AppProcessor.ProcedureProvider.Execute(_savMessageDeleteAll, DATA_PROVIDER_NAME, userName);
            return result == 1;
        }

        public List<MessageModel> GetAll(string userName)
        {
            int total;
            var lstMessages = Get(userName, out total, null);
            return lstMessages;
        }

        public int? Save(MessageModel model)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_savMessageSave, DATA_PROVIDER_NAME,
                model.Receivers,
                model.Contents,
                model.DetailUrl,
                model.UpdatedBy);

            return result;
        }

        public bool MaskAsRead(Guid? messageId)
        {
            var result =
                AppProcessor.ProcedureProvider.Execute(_savMessageMaskAsRead, DATA_PROVIDER_NAME, messageId);
            return result == 1;
        }

        public bool MaskAllAsRead(string userName)
        {
            var result =
                AppProcessor.ProcedureProvider.Execute(_savMessageMaskAllAsRead, DATA_PROVIDER_NAME, userName);
            return result == 1;
        }
    }
}