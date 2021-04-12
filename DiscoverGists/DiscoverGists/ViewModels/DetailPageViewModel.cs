using DiscoverGists.DataBase;
using DiscoverGists.Extentions;
using DiscoverGists.Models;
using DiscoverGists.Services.Interfaces;
using DiscoverGists.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DiscoverGists.ViewModels
{
    public class DetailPageViewModel : ViewModelBase
    {
        private IGitHubService _gitHubService { get; }

        public DetailPageViewModel(INavigationService navigationService, IGitHubService gitHubService) : base(navigationService)
        {
            _gitHubService = gitHubService;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            var gist = parameters.GetValue<Gist>("gist");

            LoadData(gist);
        }

        private void LoadData(Gist gist)
        {
            if (!string.IsNullOrEmpty(gist?.Id))
            {
                Gist = gist;

                FileList = Gist.Files.Select(x => x.Value).ToList().ToObservableCollection();

                if (FileList.Count > 1)
                {
                    var languageColors = Helpers.LanguageColors.GetList();

                    foreach (var item in FileList)
                    {
                        item.ColorFromLanguage = !string.IsNullOrEmpty(item?.ColorFromLanguage) ? item?.ColorFromLanguage : languageColors?.FirstOrDefault(x => x.Language?.ToLower() == item?.Language?.ToLower())?.Color ?? "#2980b9";
                    }
                }

                LoadIsFavorite();
            }
        }

        private void LoadIsFavorite()
        {
            var gist = GistDataBase.FindById(Gist);

            IsFavorite = !string.IsNullOrEmpty(gist?.Id);

            StarColor = IsFavorite ? Color.Yellow : App.ThemeColors.TextOnSecondary;
        }

        public ICommand OpenUrlCommand => new DelegateCommand(async () =>
        {
            try
            {
                SetIsLoading(true);

                await Task.Delay(500);

                var navigationParams = new NavigationParameters
                {
                    { nameof(Gist.Url), Gist.HtmlUrl }
                };

                await NavigationService.NavigateAsync(nameof(WebViewPage), navigationParams);
            }
            catch (Exception e)
            {
                //e.SendToLog();
            }
            finally
            {
                SetIsLoading(false);
            }
        });

        public ICommand RemoveOrAddFavoriteCommand => new DelegateCommand(async () =>
        {
            var confirm = await DialogService.ConfirmAsync("Confirm " + (IsFavorite ? "remove" : "add") + " favorite?", "Confirmation");

            if (!confirm)
                return;

            if (IsFavorite)
                GistDataBase.Remove(Gist);

            else
                GistDataBase.UpInsert(Gist);

            LoadIsFavorite();
        });

        private Gist _gist;
        public Gist Gist
        {
            set => SetProperty(ref _gist, value);
            get => _gist;
        }

        private ObservableCollection<File> _fileList;
        public ObservableCollection<File> FileList
        {
            set => SetProperty(ref _fileList, value);
            get => _fileList;
        }

        private Color _starColor;
        public Color StarColor
        {
            set => SetProperty(ref _starColor, value);
            get => _starColor;
        }

        public bool IsFavorite { get; set; }
    }
}
