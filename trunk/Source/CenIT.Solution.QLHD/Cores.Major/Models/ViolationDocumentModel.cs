using Newtonsoft.Json;

namespace Cores.Major.Models
{
    /// <summary>
    /// Mot van ban dinh kem cua lan vi pham.
    /// Duoc luu duoi dang JSON trong cot RelatedDocuments.
    /// </summary>
    public class ViolationDocumentModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }

        /// <summary>Dung luong da doi sang dang de doc, vi du "1,2 MB".</summary>
        public string SizeText
        {
            get
            {
                if (Size <= 0) return string.Empty;
                if (Size < 1024) return Size + " B";
                if (Size < 1024 * 1024) return (Size / 1024.0).ToString("0.#") + " KB";
                return (Size / 1024.0 / 1024.0).ToString("0.#") + " MB";
            }
        }
    }
}
