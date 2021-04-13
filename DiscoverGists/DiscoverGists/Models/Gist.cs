using Acr.UserDialogs;
using DiscoverGists.DataBase;
using LiteDB;
using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace DiscoverGists.Models
{
    public class Gist : BindableBase
    {
        [JsonProperty("id"), BsonId]
        public string Id { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("forks_url")]
        public string ForksUrl { get; set; }

        [JsonProperty("commits_url")]
        public string CommitsUrl { get; set; }

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

        [JsonIgnore]
        public DateTime AddedFavorite { get; set; }

        [JsonIgnore]
        private bool _isFavorite;
        public bool IsFavorite
        {
            set => SetProperty(ref _isFavorite, value);
            get => _isFavorite;
        }

        [JsonIgnore]
        private string _actionFavoriteText;
        public string ActionFavoriteText
        {
            set => SetProperty(ref _actionFavoriteText, value);
            get => _actionFavoriteText;
        }

        [JsonIgnore]
        private Color _actionFavoriteStarColor;
        public Color ActionFavoriteStarColor
        {
            set => SetProperty(ref _actionFavoriteStarColor, value);
            get => _actionFavoriteStarColor;
        }

        [JsonIgnore]
        private Color _starColor;
        public Color StarColor
        {
            set => SetProperty(ref _starColor, value);
            get => _starColor;
        }

        [JsonIgnore, BsonIgnore]
        public File FirstFile => Files?.Select(x => x.Value)?.FirstOrDefault() ?? new File();

        [JsonIgnore, BsonIgnore]
        public int FilesCount => Files?.Count() ?? 0;

        [JsonIgnore, BsonIgnore]
        public string FilesPresentation
        {
            get
            {
                if (string.IsNullOrEmpty(FirstFile?.Filename))
                    return "";

                var presentationReturn =
                    "Type: " + FirstFile.Type + "\n" +
                    "Name: " + FirstFile.Filename + "\n" +
                    "Size: " + FirstFile.Size + "\n" +
                    "Language: " + FirstFile.LanguagePresentation + "\n";

                return presentationReturn;
            }
        }

        [JsonIgnore, BsonIgnore]
        public string AddedFavoritePresentation => AddedFavorite.ToString("dd-MMM-yy");
    }

    public static class GistExtention
    {
        public static void SetIsFavorite(this IList<Gist> gists)
        {
            foreach (var item in gists)
            {
                item.SetIsFavorite();
            }
        }

        public static void SetIsFavorite(this Gist gist, Color? starColorNotFavorite = null)
        {
            gist.IsFavorite = !string.IsNullOrEmpty(GistDataBase.FindById(gist)?.Id);

            var yellow = Color.FromHex("#f1c40f");

            gist.StarColor = gist.IsFavorite ? yellow : (starColorNotFavorite ?? App.ThemeColors.TextOnPrimary);

            gist.ActionFavoriteText = gist.IsFavorite ? "Remover" : "Adicionar";

            gist.ActionFavoriteStarColor = gist.IsFavorite ? Color.LightGray : yellow;
        }

        public static void RemoveOrAddGistFromFavorite(this Gist gist, Color? starColorNotFavorite = null)
        {
            //var confirm = await UserDialogs.Instance.ConfirmAsync("Confirma " + (gist.IsFavorite ? "remover" : "adicionar") + " gist dos favoritos?", "Confirmação");

            //if (!confirm)
            //    return;

            if (gist.IsFavorite)
            { 
                GistDataBase.Remove(gist);

                UserDialogs.Instance.Toast(gist.FirstFile.Filename + " removido dos favoritos");
            }

            else
            { 
                GistDataBase.UpInsert(gist);

                UserDialogs.Instance.Toast(gist.FirstFile.Filename + " adicionado aos favoritos");
            }

            gist.SetIsFavorite(starColorNotFavorite);
        }
    }
}
