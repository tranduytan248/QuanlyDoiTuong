using System;

namespace Modules.Major.Areas.Major.Models
{
    public class PublishInvModel
    {
        public Guid? ContractId { get; set; }

        public string ContractNoInfo { get; set; }

        public Guid? PatternId { get; set; }

        public string Pattern { get; set; }

        public string Serial { get; set; }

        public string DataInvHtmlView { get; set; }

        public string TemplateInvViewPath { get; set; }
    }
}