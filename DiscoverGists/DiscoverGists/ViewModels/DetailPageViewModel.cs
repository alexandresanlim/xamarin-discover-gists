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
                    FileList.SetLanguageColor();

                Gist.SetIsFavorite(App.ThemeColors.TextOnSecondary);
            }
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
            catch (Exception ex)
            {
                ex.SendToLog();
            }
            finally
            {
                SetIsLoading(false);
            }
        });

        public ICommand RemoveOrAddFavoriteCommand => new DelegateCommand(async () =>
        {
            Gist.RemoveOrAddGistFromFavorite(App.ThemeColors.TextOnSecondary);
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
    }
}
