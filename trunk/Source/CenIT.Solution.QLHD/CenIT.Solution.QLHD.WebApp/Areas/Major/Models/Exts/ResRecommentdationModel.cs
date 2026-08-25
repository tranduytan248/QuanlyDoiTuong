using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Modules.Major.Areas.Major.Models.Exts
{
    public class ResRecommentdationModel : RestResponse
    {
        [JsonProperty("result")]
        public new Result Result { get; set; }
    }

    public class FilesConclusion
    {
        [JsonProperty("id")]
        public int Id;

        [JsonProperty("conclusionId")]
        public int ConclusionId;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("fileType")]
        public int FileType;

        [JsonProperty("filePath")]
        public string FilePath;

        [JsonProperty("filePathUrl")]
        public object FilePathUrl;
    }

    public class Complain
    {
        [JsonProperty("id")]
        public int Id;

        [JsonProperty("fullName")]
        public string FullName;

        [JsonProperty("code")]
        public string Code;

        [JsonProperty("title")]
        public string Title;

        [JsonProperty("content")]
        public string Content;

        [JsonProperty("status")]
        public int Status;

        [JsonProperty("statusText")]
        public string StatusText;

        [JsonProperty("address")]
        public string Address;

        [JsonProperty("sendDate")]
        public DateTime SendDate;

        [JsonProperty("fieldId")]
        public int FieldId;

        [JsonProperty("fieldName")]
        public string FieldName;

        [JsonProperty("fieldIconPath")]
        public string FieldIconPath;

        [JsonProperty("totalCount")]
        public int TotalCount;

        [JsonProperty("listFiles")]
        public List<ListFile> ListFiles;

        [JsonProperty("modelConclusion")]
        public ModelConclusion ModelConclusion;

        [JsonProperty("filesConclusion")]
        public List<FilesConclusion> FilesConclusion;
    }

    public class ListFile
    {
        [JsonProperty("id")]
        public int Id;

        [JsonProperty("recommendationId")]
        public int RecommendationId;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("fileType")]
        public int FileType;

        [JsonProperty("filePath")]
        public string FilePath;

        [JsonProperty("filePathUrl")]
        public string FilePathUrl;
    }

    public class ModelConclusion
    {
        [JsonProperty("id")]
        public int Id;

        [JsonProperty("recommendationId")]
        public int RecommendationId;

        [JsonProperty("userCreatedId")]
        public int UserCreatedId;

        [JsonProperty("unitCreatedId")]
        public int UnitCreatedId;

        [JsonProperty("receiverId")]
        public int ReceiverId;

        [JsonProperty("receiverName")]
        public string ReceiverName;

        [JsonProperty("unitReceiverId")]
        public object UnitReceiverId;

        [JsonProperty("content")]
        public string Content;

        [JsonProperty("processingDate")]
        public DateTime? ProcessingDate;
    }

    public class Result
    {
        [JsonProperty("ListData")]
        public List<Complain> ListData;

        [JsonProperty("TotalCount")]
        public int TotalCount;

        [JsonProperty("PageIndex")]
        public int PageIndex;

        [JsonProperty("PageSize")]
        public int PageSize;
    }
}