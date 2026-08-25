using Cores.Sys.Models.Sys;
using System.Collections.Generic;
using TSFramework.App.Attributes;

namespace Modules.Sys.Areas.Sys.Models
{
    public class UserPermitModel
    {
        [CustomRequired]
        [CustomDisplayName("User_Title")]
        public int? UserId { get; set; }

        [CustomDisplayName("User_Label_OfficeName")]
        public string OfficeName { get; set; }

        [CustomDisplayName("User_Label_FullName")]
        public string FullName { get; set; }

        [CustomDisplayName("User_Label_UserName")]
        public string UserName { get; set; }

        [CustomDisplayName("User_Label_Email")]
        public string Email { get; set; }

        [CustomDisplayName("Role_Title")] public string RoleIDs { get; set; }

        [CustomDisplayName("Role_Title")] public List<int> ListRoleIDs { get; set; }

        [CustomDisplayName("Role_Title")] public List<SysRoleModel> ListRoles { get; set; } = new List<SysRoleModel>();

        [CustomDisplayName("Module_Title")] public string ModuleIDs { get; set; }

        [CustomDisplayName("Role_Title")] public List<int> ListModuleIDs { get; set; }

        [CustomDisplayName("Module_Title")] public List<SysModuleModel> ListModules { get; set; } = new List<SysModuleModel>();
    }
}