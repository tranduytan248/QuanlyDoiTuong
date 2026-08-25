namespace Cores.Major.Models
{
    /// <summary>Loại đối tượng được ghi log.</summary>
    public static class ConstsChangeLogEntity
    {
        public const string Subject = "SUBJECT";
        public const string Violation = "VIOLATION";
    }

    /// <summary>Loại thao tác được ghi log.</summary>
    public static class ConstsChangeLogAction
    {
        public const string Add = "ADD";
        public const string Update = "UPDATE";
        public const string Delete = "DELETE";
    }
}
