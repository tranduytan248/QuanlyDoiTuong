using Cores.eContract.Consts;
using Cores.Major.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Cores.Cate.Enum;
using TSFramework.App.Attributes;
using TSFramework.App.Processors;
using TSFramework.Core.Helpers;
using Syncfusion.EJ2.Base;

namespace Modules.Major.Areas.Major.Models
{
    public class SearchContractModel : DataManagerRequest
    {
        public string SearchValue { get; set; }

        [CustomDisplayName("Contract_Search_FromDate")]
        public DateTime? FromDate { get; set; }

        [CustomDisplayName("Contract_Search_ToDate")]
        public DateTime? ToDate { get; set; }

        [CustomDisplayName("Contract_GiveResultOn")]
        public DateTime? GiveResultFromDate { get; set; }

        [CustomDisplayName("Contract_Search_ToDate")]
        public DateTime? GiveResultToDate { get; set; }

        [CustomDisplayName("Contract_Status")]
        public List<int> ListContractStatusIds { get; set; }

        public string ContractStatus { get; set; }

        public List<SelectListItem> ListContractStatus
        {
            get
            {
                return Enum.GetValues(typeof(EnumContractStatus))
                    .Cast<EnumContractStatus>()
                    .OrderBy(t => (int)t)
                    .Select(t => new SelectListItem
                    {
                        Value = ((int)t).ToString(),
                        Text = AppProcessor.Messagor.GetMessage(EnumHelper.GetDescription(t)),
                        Group = new SelectListGroup { Name = AppProcessor.Messagor.GetMessage("ContractStatus_Title") }
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

        #region Time Remaining
        
        [CustomDisplayName("Contract_TimeRemaining")]
        public List<string> ListTypeTermIds { get; set; }

        public string TypeTermIds { get; set; }

        public List<ListItem> ListTypeTerms =>
            new List<ListItem>
            {
                new ListItem
                {
                    Text = AppProcessor.Messagor.GetMessage("TimeRemaining_Term_Nearly"),
                    Value = "0"
                },
                new ListItem
                {
                    Text = AppProcessor.Messagor.GetMessage("TimeRemaining_Term_Late"),
                    Value = "1"
                }
            };

        #endregion

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