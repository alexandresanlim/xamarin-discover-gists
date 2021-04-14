using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        public string LanguagePresentation => !string.IsNullOrEmpty(Language) ? Language : "Não encontrada";
    }

    public static class FileExtention
    {
        public static void SetLanguageColor(this IList<File> files)
        {
            var languageColors = Helpers.LanguageColors.GetList();

            foreach (var item in files)
            {
                item.ColorFromLanguage = !string.IsNullOrEmpty(item?.ColorFromLanguage) ? item?.ColorFromLanguage : languageColors?.FirstOrDefault(x => x.Language?.ToLower() == item?.Language?.ToLower())?.Color ?? "#2980b9";
            }
        }
    }
}
