using Cores.Base.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cores.Base.Interfaces;
using TSFramework.App.Attributes;
using TSFramework.App.Processors;
using Microsoft.AspNet.SignalR;
using TSFramework.Core.Hubs;
using Cores.Major.Caches;
using Cores.Major.Models;

namespace Extends.Notifications.InSite
{
    [ClassInfo("Extends.InSiteNotifider", "Thông báo qua Notification")]
    public class InSiteNotifider : INotify
    {
        private static IHubContext _notifyHub;

        public string Name { get; } = "Extends.InSiteNotifider";

        public string Description { get; } = "Thông báo qua Notification";

        public InSiteNotifider()
        {
            _notifyHub = GlobalHost.ConnectionManager.GetHubContext<AppSignalRHub>();
        }

        public void Push(string senderName, string typeObjName, string title, List<NotifyReceiverModel> lstReceivers, string detailUrl, string hostName, Dictionary<string, object> extParrams)
        {
            throw new NotImplementedException();
        }

        public void Push(ContentNotifyModel model)
        {
            MajorMessageCache messageCache = new MajorMessageCache();

            var sMessage = AppProcessor.Notifider.CreateNotify(model.InsiteNotification.Title, model.InsiteNotification.Message, model.InsiteNotification.Icon, model.InsiteNotification.Url, model.InsiteNotification.Target, model.InsiteNotification.Placement);

            messageCache.Save(new MajorMessageModel
            {
                Receivers = model.InsiteNotification.Receiver,
                Contents = model.InsiteNotification.Message,
                DetailUrl = model.InsiteNotification.Url,
                UpdatedBy = model.InsiteNotification.Sender
            });

            Task.Run(() => _notifyHub.Clients.User(model.InsiteNotification.Receiver).OnNotify(model.InsiteNotification.Sender, sMessage));
        }
    }
}
