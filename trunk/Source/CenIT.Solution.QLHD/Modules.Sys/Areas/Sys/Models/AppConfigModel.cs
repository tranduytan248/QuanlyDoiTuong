using TSFramework.App.Attributes;

namespace Modules.Sys.Areas.Sys.Models
{
    public class AppConfigModel
    {
        [CustomRequired]
        [CustomDisplayName("AppConfig_Label_KeyName")]
        public string AppKey { get; set; }

        [CustomRequired]
        [CustomDisplayName("AppConfig_Label_Value")]
        public string AppValue { get; set; }
    }
}