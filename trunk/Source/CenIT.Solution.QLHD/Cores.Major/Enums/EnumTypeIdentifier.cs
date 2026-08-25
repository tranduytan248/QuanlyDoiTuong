using System.ComponentModel;

namespace Cores.Major.Enums
{
    public enum EnumTypeIdentifier
    {
        /// <summary>
        ///     Chứng minh thư
        /// </summary>
        [Description("TypeIdentifier_DefinitionDoc")]
        DefinitionDoc = 0,

        /// <summary>
        ///     Căn cước công dân
        /// </summary>
        [Description("TypeIdentifier_IdCard")] IdCard = 1,

        /// <summary>
        ///     Hộ chiếu
        /// </summary>
        [Description("TypeIdentifier_Passport")]
        Passport = 2
    }
}