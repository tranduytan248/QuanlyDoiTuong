using System;

namespace Cores.Sys.Models.Sys
{
    /// <summary>
    /// Mot phien lam viec dang hoat dong tren he thong.
    /// Dung cho man hinh Giam sat truc tuyen: ai dang dang nhap, dang o man hinh nao.
    /// </summary>
    public class SysUserActivityModel
    {
        public string SessionId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Avatar { get; set; }

        /// <summary>Duong dan dang mo, vi du /Major/Subject.</summary>
        public string CurrentUrl { get; set; }

        /// <summary>Ten man hinh doc duoc, vi du "Quan ly Doi tuong".</summary>
        public string ScreenName { get; set; }

        public string IpAddress { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime LastActivity { get; set; }

        /// <summary>So giay ke tu hoat dong cuoi cung.</summary>
        public int SecondsAgo { get; set; }

        /// <summary>So phut da dang nhap.</summary>
        public int MinutesOnline { get; set; }

        public string UnionName { get; set; }
        public string PositionName { get; set; }
    }
}
