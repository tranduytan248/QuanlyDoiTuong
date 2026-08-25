using System;
using System.Collections.Generic;
using System.Web;
using TSFramework.App.Attributes;

namespace Modules.Major.Areas.Major.Models
{
    public class UploadFileModel
    {
        [CustomDisplayName("Contract_Title")]
        [CustomRequired]
        public Guid? ContractId { get; set; }

        [CustomDisplayName("Contract_ContractNo")]
        [CustomRequired]
        public string ContractNo { get; set; }

        [CustomDisplayName("Contract_ContractSignal")]
        [CustomRequired]
        public string ContractSignal { get; set; }

        public string ContractNoInfo { get; set; }

        [CustomDisplayName("Customer_Title")]
        public string CusName { get; set; }

        [CustomDisplayName("Contract_RefFile")]
        [CustomRequired]
        public List<HttpPostedFileBase> RefFiles { get; set; }
    }
}