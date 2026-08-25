using System;
using System.Collections.Generic;
using System.Linq;
using Cores.Sys.Models.Sys;
using TSFramework.App.Models;
using TSFramework.App.Processors;

namespace Cores.Sys.Biz.Sys
{
    public class SysNotificationBiz
    {
        private const string DATA_PROVIDER_NAME = "MCSProvider";
        private readonly string _sys_Notification_Delete = "Sys_Notification_Delete";
        private readonly string _sys_Notification_Get = "Sys_Notification_Get";
        private readonly string _sys_Notification_GetById = "Sys_Notification_GetById";
        private readonly string _sys_Notification_Save = "Sys_Notification_Save";


        public List<SysNotificationModel> LoadList(out int total, BaseSearchModel search)
        {
            search = search ?? new BaseSearchModel
            {
                Search = null,
                Order = "1",
                OrderDir = "ASC",
                StartIndex = 0,
                PageSize = -1
            };
            var data = AppProcessor.ProcedureProvider.ExecuteTypedList<SysNotificationModel>(_sys_Notification_Get,
                DATA_PROVIDER_NAME,
                search.Search,
                search.Order,
                search.OrderDir,
                search.StartIndex,
                search.PageSize);
            total = 0;
            if (data != null && data.Count > 0)
                total = int.Parse(data.First()?.TotalRow.ToString() ?? "0");
            return data;
        }


        public List<SysNotificationModel> GetAll()
        {
            var list = LoadList(out _, null);
            return list;
        }

        public SysNotificationModel LoadDetail(Guid? ID)
        {
            var data = AppProcessor.ProcedureProvider.ExecuteScalarObject<SysNotificationModel>(
                _sys_Notification_GetById, DATA_PROVIDER_NAME, ID);
            return data;
        }

        public int Delete(Guid? ID)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_sys_Notification_Delete, DATA_PROVIDER_NAME, ID);
            return result.GetValueOrDefault(0);
        }

        public int Save(SysNotificationModel model, string savedBy)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_sys_Notification_Save, DATA_PROVIDER_NAME
                , model.NotificationId
                , model.NotificationCode
                , model.ChannelType
                , model.Situation
                , model.Receiver
                , model.Content
                , savedBy);
            return result.GetValueOrDefault(0);
        }
    }
}