using TSFramework.App.Models;

namespace Cores.Sys.Models.Sys
{
    public class SysFunctionActionModel : BaseModel
    {
        public string Area { get; set; }
        public int FunctionActionId { get; set; }
        public int FunctionId { get; set; }
        public string Function { get; set; }
        public string Action { get; set; }
        public new int? TotalRow { get; set; } = 0;
    }
}