using Cores.Base.Interfaces;
using System;
using System.Collections.Generic;
using Cores.Base.Models;
using TSFramework.App.Attributes;

namespace Extends.Notifications.Zalo
{
    [ClassInfo("Extends.ZaloNotifider", "Thông báo qua Zalo")]
    public class ZaloNotifider : INotify
    {
        public string Name { get; } = "Extends.ZaloNotifider";

        public string Description { get; } = "Thông báo qua Zalo";

        public void Push(string senderName, string typeObjName, string title, List<NotifyReceiverModel> lstReceivers, string detailUrl, string hostName, Dictionary<string, object> extParrams)
        {
            throw new NotImplementedException();
        }

        public void Push(ContentNotifyModel model)
        {
            throw new NotImplementedException();
        }
    }
}
