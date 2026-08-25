using System.Web;

namespace Modules.Sys.Areas.Sys.Models
{
    public class SysUploadModel
    {
        public string AbsolutePath { get; set; }

        public HttpPostedFileBase FileUpload { get; set; }
    }
}