using System.ComponentModel;

namespace Cores.Base.Enums
{
    public enum EnumTypeNotification
    {
        [Description("TypeNotification_SMS")] SMS = 0,

        [Description("TypeNotification_Email")]
        Email = 1,
        [Description("TypeNotification_Zalo")] Zalo = 2
    }
}