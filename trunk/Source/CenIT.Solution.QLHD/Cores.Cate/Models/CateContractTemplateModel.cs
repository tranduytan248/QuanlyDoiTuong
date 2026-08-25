using Cores.Cate.Enum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using TSFramework.App.Attributes;
using TSFramework.App.Models;
using TSFramework.App.Processors;
using TSFramework.Core.Helpers;

namespace Cores.Cate.Models
{
    public class CateContractTemplateModel : BaseModel
    {
        public string Id { get; set; }

        [CustomRequired]
        [CustomDisplayName("Contract_ContractType")]
        public string ContractTypeId { get; set; }

        public string FileName { get; set; }

        [CustomRequired]
        [CustomDisplayName("TemplateName")]
        public string FullName { get; set; }

        public bool IsUsed { get; set; }
        public string Status { get; set; }
        public string TemplateFields { get; set; }
        public string TemplateName { get; set; }
        public string TemplatePath { get; set; }
        public string TemplatePathCosumer { get; set; }
        public string TemplateType { get; set; }
        public string Username { get; set; }

        public Guid? Version { get; set; }

        public DateTime? LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }


        [CustomRequired]
        [CustomDisplayName("ContractTemplate_IndexTabel")]
        public int IndexTabel { get; set; }

        [CustomRequired]
        [CustomDisplayName("ContractTemplate_IndexRowInTable")]
        public int IndexRowInTable { get; set; }

        //public List<CateContractTypeModel> ListTypeContracts { get; set; } = new List<CateContractTypeModel>();

        public List<SelectListItem> ListTypeContracts
        {
            get
            {
                return System.Enum.GetValues(typeof(EnumContractType))
                    .Cast<EnumContractType>()
                    .OrderBy(t => (int)t)
                    .Select(t => new SelectListItem
                    {
                        Value = ((int)t).ToString(),
                        Text = AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(t)),
                        Group = new SelectListGroup { Name = AppProcessor.Messagor.GetMessage("ContractType_Title") }
                    }).ToList();
            }
        }

        public DataTable TableRefFile { get; set; }

        [CustomDisplayName("Contract_RefFile")]
        public List<CateDocModel> ListRefFiles { get; set; }

        [CustomDisplayName("Contract_RefFile")]
        public List<HttpPostedFileBase> RefFiles { get; set; }

        [CustomDisplayName("Contract_RefFile")]
        public List<HttpPostedFileBase> RefFilesCosumer { get; set; }
    }
}