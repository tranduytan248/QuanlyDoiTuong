using System;
using TSFramework.App.Attributes;
using TSFramework.App.Models;

namespace Modules.Major.Areas.Major.Models
{
    public class SearchContractPaymentModel : SearchModel
    {
        [CustomDisplayName("Contract_Title")]
        public Guid? ContractId { get; set; }
    }
}