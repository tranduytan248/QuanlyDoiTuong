using System;
using System.Web.Mvc;
using TSFramework.App.Attributes;
using TSFramework.App.Models;

namespace Cores.Major.Models
{
    public class MajorMessageModel : BaseModel
    {
        [CustomDisplayName("Major_Message_Title")]
        public Guid? MessageId { get; set; }

        public string Receiver { get; set; }

        public string Receivers { get; set; }

        [CustomDisplayName("Major_Message_Title")]
        [AllowHtml]
        public string Title { get; set; }

        [CustomDisplayName("Major_Message_Contents")]
        [AllowHtml]
        public string Contents { get; set; }

        public string DetailUrl { get; set; }

        [CustomDisplayName("Major_Message_IsReaded")]
        public bool IsReaded { get; set; }

        [CustomDisplayName("Major_Message_ReadedOn")]
        public DateTime? ReadedOn { get; set; }

        [CustomDisplayName("Major_Message_CreatedOn")]
        public DateTime CreatedOn { get; set; }
    }
}