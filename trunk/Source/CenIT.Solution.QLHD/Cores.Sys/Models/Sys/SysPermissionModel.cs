using TSFramework.App.Models;

namespace Cores.Sys.Models.Sys
{
    public class SysPermissionModel : BaseModel
    {
        public int PermissionId { get; set; }
        public int RoleId { get; set; }
        public string Area { get; set; }

        public int FunctionId { get; set; }
        public string FunctionName { get; set; }
        public string Action { get; set; }
        public string ActionName { get; set; }
        public new int? TotalRow { get; set; } = 0;
    }
}