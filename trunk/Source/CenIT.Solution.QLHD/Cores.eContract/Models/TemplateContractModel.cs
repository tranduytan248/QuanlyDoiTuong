using Newtonsoft.Json;
using System;

namespace Cores.eContract.Models
{
    public class Products
    {
    }

    public class TemplateContractModel
    {
        [JsonProperty("contractTypeId")]
        public string ContractTypeId { get; set; }

        [JsonProperty("dateCreate")]
        public DateTime DateCreate { get; set; }

        [JsonProperty("dateUpload")]
        public object DateUpload { get; set; }

        [JsonProperty("fileName")]
        public string FileName { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("isUsed")]
        public bool IsUsed { get; set; }

        [JsonProperty("products")]
        public Products Products { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("templateFields")]
        public string TemplateFields { get; set; }

        [JsonProperty("templateName")]
        public string TemplateName { get; set; }

        [JsonProperty("templatePath")]
        public string TemplatePath { get; set; }

        [JsonProperty("templateType")]
        public string TemplateType { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }
    }
}