using System.ComponentModel;

namespace Cores.Cate.Enum
{
    public enum EnumCateType
    {
        [Description("ProcedureType_Title")] ProcedureType = 1,

        [Description("UnionType_Title")] UnionType = 2,

        [Description("TitleType_Title")] TitleType = 3,

        [Description("ConstructionType_Title")]
        ConstructionType = 4,

        [Description("DocType_Title")] DocType = 5,

        [Description("ViolationType_Title")] ViolationType = 6,

        [Description("LevelProcessing_Title")] LevelProcessing = 7,

        [Description("TypeLand_Title")] TypeLand = 8

        //[Description("FieldViolated_Title")]
        //FieldViolated = 9
    }
}