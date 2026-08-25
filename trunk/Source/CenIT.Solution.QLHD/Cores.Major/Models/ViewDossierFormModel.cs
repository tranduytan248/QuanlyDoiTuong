using System;

namespace Cores.Major.Models
{
    public class ViewDossierFormModel
    {
        public int? PrevIdx { get; set; }
        public Guid? PrevForm { get; set; }
        public int? CurrentIdx { get; set; }
        public Guid? CurrentForm { get; set; }
        public int? NextIdx { get; set; }
        public Guid? NextForm { get; set; }
        public Guid? DossierId { get; set; }
        public Guid? SituationId { get; set; }
        public Guid? StepId { get; set; }
    }
}