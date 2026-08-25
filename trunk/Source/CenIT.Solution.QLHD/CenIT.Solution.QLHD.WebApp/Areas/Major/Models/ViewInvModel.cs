using System;

namespace Modules.Major.Areas.Major.Models
{
    public class ViewInvModel
    {
        public Guid InvId { get; set; }

        public string InvKey { get; set; }

        public string InvNo { get; set; }

        public string Pattern { get; set; }

        public string Serial { get; set; }

        public string HtmlInv { get; set; }
    }
}