using System;

namespace Cores.Sys.Models.Sys
{
    public class SysBlockIPModel
    {
        public string IP { get; set; }
        public string UrlRequest { get; set; }
        public int TimeRequest { get; set; }
        public bool IsLock { get; set; }
        public DateTime LastestRequest { get; set; }
    }
}