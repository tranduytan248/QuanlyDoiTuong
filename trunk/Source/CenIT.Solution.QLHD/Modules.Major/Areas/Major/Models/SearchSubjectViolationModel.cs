using System;

namespace Modules.Major.Areas.Major.Models
{
    public class SearchSubjectViolationModel
    {
        public string Key { get; set; }
        public Guid? SubjectId { get; set; }
        public int? FieldId { get; set; }
    }
}
