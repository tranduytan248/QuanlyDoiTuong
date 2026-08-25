using Core.Inv.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using TSFramework.App.Attributes;
using TSFramework.Core.Helpers;

namespace Modules.Major.Areas.Major.Models
{
    public class SearchInvoiceModel
    {
        public string Pattern { get; set; }
        public List<string> ListSerials { get; set; }
        public string Serials { get; set; }
        public string InvNo { get; set; }
        public List<string> ListStatusInvs { get; set; }
        public string InvStatus { get; set; }
        public List<string> ListTypeInvs { get; set; }
        public string InvTypes { get; set; }
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }
        public List<string> ListCreators { get; set; }
        public string Creators { get; set; }
        public string CusName { get; set; }
        public string CusCode { get; set; }
        public string CusTaxCode { get; set; }

        public List<ListItem> ListInvPatterns { get; set; } = new List<ListItem>();
        public List<ListItem> ListInvTypes
        {
            get
            {
                return Enum.GetValues(typeof(EnumInvType))
                    .Cast<EnumInvType>()
                    .OrderBy(t => EnumHelper.GetDescription(t))
                    .Select(t => new ListItem
                    {
                        Value = ((int)t).ToString(),
                        Text = EnumHelper.GetDescription(t),
                    }).ToList();
            }
        }
        public List<ListItem> ListInvStatus {
            get
            {
                return Enum.GetValues(typeof(EnumInvStatus))
                    .Cast<EnumInvStatus>()
                    .OrderBy(t => EnumHelper.GetDescription(t))
                    .Select(t => new ListItem
                    {
                        Value = ((int)t).ToString(),
                        Text = EnumHelper.GetDescription(t),
                    }).ToList();
            }
        }
        public List<ListItem> ListUsers { get; set; } = new List<ListItem>();
        public List<ListItem> ListInvSerials { get; set; } = new List<ListItem>();

        public bool? HasNotSysInvAccount { get; set; } = false;
        public bool? IsInvServiceAccountIncorrect { get; set; } = false;

        #region Unions

        [CustomDisplayName("Union_Manager_Title")]
        public string UnionIds { get; set; }

        [CustomDisplayName("Union_Manager_Title")]
        public List<string> ListUnionIds { get; set; }

        public List<ListItem> ListUnions { get; set; } = new List<ListItem>();

        #endregion

        public string[] Permissions { get; set; }

        public List<ListItem> ListTPattern { get => ListInvPatterns; set => ListInvPatterns = value; }
        public List<ListItem> ListTType { get => ListInvTypes; set { } }
        public List<ListItem> ListUser { get => ListUsers; set => ListUsers = value; }
        public string Serial { get => Serials; set => Serials = value; }
        public string InvType { get => InvTypes; set => InvTypes = value; }
        public DateTime? CreateOn { get => CreatedFrom; set => CreatedFrom = value; }
        public DateTime? CreateTo { get => CreatedTo; set => CreatedTo = value; }
        public string CreateBy { get => Creators; set => Creators = value; }
        public string CusAddress { get; set; }
    }
}