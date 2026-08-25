using System;
using TSFramework.App.Attributes;

namespace Modules.Major.Areas.Major.Models
{
    public class ContractModel
    {
        [CustomDisplayName("Dossier_Title")]
        public Guid? DossierId { get; set; }

        [CustomDisplayName("Dossier_Code")]
        public string DossierCode { get; set; }

        [CustomDisplayName("Dossier_Name")]
        public string DossierName { get; set; }

        [CustomDisplayName("Contract_No")]
        public string ContractNo { get; set; }

        [CustomDisplayName("Contract_ReceivedOn")]
        public DateTime ReceivedOn { get; set; }

        [CustomDisplayName("Contract_ConfirmOn")]
        public DateTime ConfirmOn { get; set; }

        [CustomDisplayName("Contract_HandleTime")]
        public double? HandleTime { get; set; }

        [CustomDisplayName("Contract_GiveResultOn")]
        public DateTime GiveResultOn { get; set; }
    }
}