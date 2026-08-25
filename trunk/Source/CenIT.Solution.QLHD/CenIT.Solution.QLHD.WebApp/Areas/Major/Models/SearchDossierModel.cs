using Cores.Cate.Enum;
using Cores.eContract.Consts;
using Cores.Major.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using TSFramework.App.Attributes;
using TSFramework.App.Processors;
using TSFramework.Core.Helpers;

namespace Modules.Major.Areas.Major.Models
{
    public class SearchDossierModel
    {
        public string SearchValue { get; set; }

        [CustomDisplayName("Common_FromDate")]
        public DateTime? FromDate { get; set; }

        [CustomDisplayName("Common_ToDate")]
        public DateTime? ToDate { get; set; }

        [CustomDisplayName("Contract_GiveResultOn")]
        public DateTime? GiveResultFromDate { get; set; }

        [CustomDisplayName("Common_ToDate")]
        public DateTime? GiveResultToDate { get; set; }

        [CustomDisplayName("Dossier_Status")]
        public List<int> ListDossierStatusIds { get; set; }

        public string DossierStatus { get; set; } = $"{(int)EnumDossierTaskStatus.Handling}";

        [CustomDisplayName("Handle_Types")]
        public List<int> ListHandleTypeIds { get; set; }
        public string HandleTypes { get; set; } = $"{(int)EnumHandleType.MainProcessing}";

        public List<SelectListItem> ListDossierStatus
        {
            get
            {
                return Enum.GetValues(typeof(EnumDossierTaskStatus))
                    .Cast<EnumDossierTaskStatus>()
                    .OrderBy(t => AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(t)))
                    .Select(t => new SelectListItem
                    {
                        Value = ((int)t).ToString(),
                        Text = AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(t)),
                        Group = new SelectListGroup { Name = AppProcessor.Messagor.GetMessage("Dossier_Status") }
                    }).ToList();
            }
        }

        public List<SelectListItem> ListHandleTypes // List for handle types
        {
            get
            {
                return Enum.GetValues(typeof(EnumHandleType))
                    .Cast<EnumHandleType>()
                    .OrderBy(t => AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(t)))
                    .Select(t => new SelectListItem
                    {
                        Value = ((int)t).ToString(),
                        Text = AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(t)),
                        Group = new SelectListGroup { Name = AppProcessor.Messagor.GetMessage("Handle_Types") }
                    }).ToList();
            }
        }

        [CustomDisplayName("ContractType_Title")]
        public List<int> ListTypeContractIds { get; set; }

        public string TypeContractIds { get; set; }

        //public List<ListItem> ListTypeContracts { get; set; } = new List<ListItem>();

        public List<SelectListItem> ListTypeContracts
        {
            get
            {
                return Enum.GetValues(typeof(EnumContractType))
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

        [CustomDisplayName("Customer_TypeCus")]
        public List<string> ListTypeCusIds { get; set; }

        public string TypeCusIds { get; set; }

        public List<SelectListItem> ListTypeCus =>
            new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = AppProcessor.Messagor.GetMessage("CusType_Consumer"),
                    Value = ConstsCusType.CONSUMER,
                    Group = new SelectListGroup{ Name = AppProcessor.Messagor.GetMessage("Customer_TypeCus")}
                },
                new SelectListItem
                {
                    Text = AppProcessor.Messagor.GetMessage("CusType_Business"),
                    Value = ConstsCusType.BUSINESS,
                    Group = new SelectListGroup{ Name = AppProcessor.Messagor.GetMessage("Customer_TypeCus")}
                }
            };

        #region Unions

        [CustomDisplayName("Union_Manager_Title")]
        public string UnionIds { get; set; }

        [CustomDisplayName("Union_Manager_Title")]
        public List<string> ListUnionIds { get; set; }

        public List<ListItem> ListUnions { get; set; } = new List<ListItem>();

        #endregion

        public string[] Permissions { get; set; }

    }
}