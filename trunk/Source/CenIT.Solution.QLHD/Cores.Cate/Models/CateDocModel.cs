using System;
using TSFramework.App.Attributes;
using TSFramework.App.Models;

namespace Cores.Cate.Models
{
    public class CateDocModel : BaseModel
    {
        public Guid? FileId { get; set; }

        public string TypeObject { get; set; }

        public string ObjectId { get; set; }

        public string FilePath { get; set; }

        public string FileName { get; set; }

        public string FileExt { get; set; }

        public string UrlPath { get; set; }

        public string ContentType { get; set; }

        public int Version { get; set; } = 1;

        public bool IsDeleted { get; set; }

        public bool IsClear { get; set; } = false;

        public string Dimensions { get; set; } = "800x1200";

        [CustomRequired] public new string Reason { get; set; }
    }
}