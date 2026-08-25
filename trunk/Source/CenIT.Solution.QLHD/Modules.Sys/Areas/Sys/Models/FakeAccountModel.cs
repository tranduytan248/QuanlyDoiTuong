using System.Collections.Generic;
using Cores.Cate.Models;
using TSFramework.App.Attributes;

namespace Modules.Sys.Areas.Sys.Models
{
    public class FakeAccountModel
    {
        [CustomDisplayName("User_Title")]
        [CustomRequired]
        public string UserName { get; set; }

        [CustomDisplayName("User_Title")]
        public string FullName { get; set; }

        public string Email { get; set; }

        [CustomDisplayName("User_Title")]
        public List<CateUnionMemberModel> ListUsers { get; set; } = new List<CateUnionMemberModel>();
    }
}