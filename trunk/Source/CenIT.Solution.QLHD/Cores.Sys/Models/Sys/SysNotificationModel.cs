using System;
using System.Collections.Generic;
using TSFramework.App.Attributes;
using TSFramework.App.Models;

namespace Cores.Sys.Models.Sys
{
    public class SysNotificationModel : BaseSearchModel
    {
        public Guid? NotificationId { get; set; }

        [CustomRequired]
        [CustomDisplayName("NotificationCode_Label_Name")]
        public string NotificationCode { get; set; }

        [CustomRequired]
        [CustomDisplayName("ChannelType_Label_Name")]
        public string ChannelType { get; set; }

        [CustomRequired]
        [CustomDisplayName("Situation_Label_Name")]
        public string Situation { get; set; }

        [CustomRequired]
        [CustomDisplayName("Receiver_Label_Name")]
        public string Receiver { get; set; }

        [CustomRequired]
        [CustomDisplayName("Content_Label_Name")]
        public string Content { get; set; }

        public int TotalRow { get; set; }

        public List<ChannelTypeEnum> ChannelTypeList { get; set; }
    }

    public enum ChannelTypeEnum
    {
        SMS,
        Email,
        Notification
    }
}