using System.Collections.Generic;
using Cores.Base.Models;

namespace Cores.Base.Interfaces
{
    public interface INotify
    {
        string Name { get; }
        string Description { get; }

        //EnumTypeNotification TypeNotification();
        void Push(string senderName, string typeObjName, string title, List<NotifyReceiverModel> lstReceivers,
            string detailUrl, string hostName, Dictionary<string, object> extParrams);

        void Push(ContentNotifyModel model);
    }
}