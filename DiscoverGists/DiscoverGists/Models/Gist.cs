using LiteDB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace DiscoverGists.Models
{
    public class Gist
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("forks_url")]
        public string ForksUrl { get; set; }

        [JsonProperty("commits_url")]
        public string CommitsUrl { get; set; }

        [JsonProperty("id"), BsonId]
        public string Id { get; set; }

        [JsonProperty("node_id")]
        public string NodeId { get; set; }

        [JsonProperty("git_pull_url")]
        public string GitPullUrl { get; set; }

        [JsonProperty("git_push_url")]
        public string GitPushUrl { get; set; }

        [JsonProperty("html_url")]
        public string HtmlUrl { get; set; }

        [JsonProperty("files")]
        public Dictionary<string, File> Files { get; set; }

        [JsonProperty("public")]
        public bool Public { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("comments")]
        public int Comments { get; set; }

        [JsonProperty("user")]
        public object User { get; set; }

        [JsonProperty("comments_url")]
        public string CommentsUrl { get; set; }

        [JsonProperty("owner")]
        public Owner Owner { get; set; }

        [JsonProperty("truncated")]
        public bool Truncated { get; set; }

        [JsonIgnore, BsonIgnore]
        public string FilesPresentation
        {
            get
            {
                var presentationReturn = "";

                if (Files == null || Files.Count.Equals(0))
                    return presentationReturn;

                var item = Files.FirstOrDefault();

                //foreach (var item in Files)
                //{
                presentationReturn += "Type: " + item.Value.Type + " \nName: " + item.Value.Filename + " \nSize: " + item.Value.Size + " \nLanguage: " + (!string.IsNullOrEmpty(item.Value?.Language) ? item.Value?.Language : "Not found") + "\n";
                //}

                if (Files.Count > 1)
                    presentationReturn += "More " + Files.Count + " found";

                return presentationReturn;
            }
        }

        public KeyValuePair<string, File> FirstFile => Files.FirstOrDefault();

        [JsonIgnore]
        public string ColorFromLanguage { get; set; }
    }
}
