using System;
using System.Collections.Generic;
using System.Web;

namespace Modules.Major.Areas.Major.Models
{
    public class DropzoneUploadModel
    {
        public Guid? DossierId { get; set; }
        public List<HttpPostedFileBase> LstRefFiles { get; set; }
        public string TypeObject { get; set; }
    }
}