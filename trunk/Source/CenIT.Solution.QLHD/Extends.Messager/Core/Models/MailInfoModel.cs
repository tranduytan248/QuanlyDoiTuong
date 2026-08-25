using System.Collections.Generic;

namespace Extends.Messager.Core.Models
{
    public class MailInfoModel
    {
        public string FullName { get; set; }

        //public string Email { get; set; }
        public string HostUrl { get; set; }
        public string SupportEmail { get; set; }

        public List<string> ListReceivers { get; set; }

        public string TypeObjectName { get; set; }
        public string InfoName { get; set; }
        public string DetailUrl { get; set; }
    }
}