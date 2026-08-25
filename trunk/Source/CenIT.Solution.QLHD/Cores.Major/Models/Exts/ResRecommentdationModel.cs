using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Cores.Major.Models.Exts
{
    public class ResRecommentdationModel : RestResponse
    {
        [JsonProperty("result")] public new Result Result { get; set; }
    }

    public class FilesConclusion
    {
        [JsonProperty("conclusionId")] public int ConclusionId;

        [JsonProperty("filePath")] public string FilePath;

        [JsonProperty("filePathUrl")] public object FilePathUrl;

        [JsonProperty("fileType")] public int FileType;

        [JsonProperty("id")] public int Id;

        [JsonProperty("name")] public string Name;
    }

    public class Complain
    {
        [JsonProperty("address")] public string Address;

        [JsonProperty("code")] public string Code;

        [JsonProperty("content")] public string Content;

        [JsonProperty("fieldIconPath")] public string FieldIconPath;

        [JsonProperty("fieldId")] public int FieldId;

        [JsonProperty("fieldName")] public string FieldName;

        [JsonProperty("filesConclusion")] public List<FilesConclusion> FilesConclusion;

        [JsonProperty("fullName")] public string FullName;

        [JsonProperty("id")] public int Id;

        [JsonProperty("listFiles")] public List<ListFile> ListFiles;

        [JsonProperty("modelConclusion")] public ModelConclusion ModelConclusion;

        [JsonProperty("sendDate")] public DateTime SendDate;

        [JsonProperty("status")] public int Status;

        [JsonProperty("statusText")] public string StatusText;

        [JsonProperty("title")] public string Title;

        [JsonProperty("totalCount")] public int TotalCount;
    }

    public class ListFile
    {
        [JsonProperty("filePath")] public string FilePath;

        [JsonProperty("filePathUrl")] public string FilePathUrl;

        [JsonProperty("fileType")] public int FileType;

        [JsonProperty("id")] public int Id;

        [JsonProperty("name")] public string Name;

        [JsonProperty("recommendationId")] public int RecommendationId;
    }

    public class ModelConclusion
    {
        [JsonProperty("content")] public string Content;

        [JsonProperty("id")] public int Id;

        [JsonProperty("processingDate")] public DateTime? ProcessingDate;

        [JsonProperty("receiverId")] public int ReceiverId;

        [JsonProperty("receiverName")] public string ReceiverName;

        [JsonProperty("recommendationId")] public int RecommendationId;

        [JsonProperty("unitCreatedId")] public int UnitCreatedId;

        [JsonProperty("unitReceiverId")] public object UnitReceiverId;

        [JsonProperty("userCreatedId")] public int UserCreatedId;
    }

    public class Result
    {
        [JsonProperty("ListData")] public List<Complain> ListData;

        [JsonProperty("PageIndex")] public int PageIndex;

        [JsonProperty("PageSize")] public int PageSize;

        [JsonProperty("TotalCount")] public int TotalCount;
    }
}