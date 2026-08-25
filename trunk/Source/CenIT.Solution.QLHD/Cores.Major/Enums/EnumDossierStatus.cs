using System.ComponentModel;

namespace Cores.Major.Enums
{
    public enum EnumDossierStatus
    {
        [Description("HandleDossier_WaitingForApprove")]
        WaitingForApprove = 0,

        [Description("HandleDossier_WaitingForHandle")]
        WaitingForHandle = 1,

        [Description("HandleDossier_Handling")]
        Handling = 2,

        [Description("HandleDossier_Completed")]
        Completed = 3
    }
}