using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using TSFramework.App.Attributes;

namespace Cores.Cate.Models
{
    public class CateImplementContentModel
    {
        public int ImplementContentId { get; set; }

        [CustomRequired]
        [CustomDisplayName("WorkContent")]
        public string WorkContent { get; set; }

        [CustomRequired]
        [CustomDisplayName("WorkPurpose")]
        public string WorkPurpose { get; set; }

        [CustomDisplayName("ContractForm")] public string ContractForm { get; set; }

        [CustomDisplayName("ContractForm")]
        //public List<SAVDocModel> lstFileSavDoc { get; set; }
        //[CustomDisplayName("ContractForm")]
        public string FileId { get; set; }

        [CustomDisplayName("ContractForm")] public HttpPostedFileBase ContractFormFile { get; set; }

        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string FileExt { get; set; }

        public List<ListItem> ListFileSavDoc { get; set; }

        //public List<SelectListItem> ListFileSavDoc
        //{
        //    get
        //    {
        //        var list = System.Enum.GetValues(typeof(EnumTypeContract))
        //            .Cast<EnumTypeContract>()
        //            .Where(t => (int)t >= 0)
        //           .Select(t =>
        //           {
        //               string description = EnumHelper.GetDescription(t);
        //               string value = description.Substring(0, description.IndexOf(" - ")).Trim();
        //               return new SelectListItem
        //               {
        //                   Value = value,
        //                   Text = AppProcessor.Messagor.GetMessage(t.ToString())
        //               };
        //           }).ToList();
        //        return list;
        //    }
        //}

        public int TotalRow { get; set; }
    }
}