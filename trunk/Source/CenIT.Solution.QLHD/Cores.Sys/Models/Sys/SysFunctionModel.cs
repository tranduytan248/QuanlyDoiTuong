using System.Collections.Generic;
using System.Web.UI.WebControls;
using TSFramework.App.Attributes;
using TSFramework.App.Models;

namespace Cores.Sys.Models.Sys
{
    public class SysFunctionModel : BaseModel
    {
        public int FunctionId { get; set; }

        [CustomDisplayName("AppModule_Label_ModuleName")]
        [CustomRequired]
        public int ModuleId { get; set; }

        [CustomDisplayName("Module_Title")] public string ModuleName { get; set; }

        //[CustomDisplayName("Function_Label_CodeType")]
        //[CustomRequired]
        //public string CodeType { get; set; }

        [CustomDisplayName("Function_Label_Area")]
        public string Area { get; set; }

        [CustomRequired]
        [CustomDisplayName("Function_Label_Function")]
        public string Name { get; set; }

        [CustomDisplayName("Function_Label_Description")]
        public string Description { get; set; }

        public bool IsDeleted { get; set; }

        [CustomRequired] public string SelectedActions { get; set; }

        public List<ListItem> Actions { get; set; }

        public List<ListItem> CodeTypes { get; set; }

        public List<ListItem> Modules { get; set; }

        public new int? TotalRow { get; set; } = 0;
    }
}