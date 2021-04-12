using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscoverGists.Models
{
    public class File
    {
        [JsonProperty("filename")]
        public string Filename { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("raw_url")]
        public Uri RawUrl { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonIgnore]
        public string ColorFromLanguage { get; set; }

        [JsonIgnore]
        public string LanguagePresentation => !string.IsNullOrEmpty(Language) ? Language : "Not found";
    }
}
