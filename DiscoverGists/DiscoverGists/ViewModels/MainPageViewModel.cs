using DiscoverGists.DataBase;
using DiscoverGists.Extentions;
using DiscoverGists.Helpers;
using DiscoverGists.Models;
using DiscoverGists.Services;
using DiscoverGists.Services.Interfaces;
using DiscoverGists.Views;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DiscoverGists.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private IGitHubService _gitHubService { get; }

        public MainPageViewModel(INavigationService navigationService, IGitHubService gitHubService)
            : base(navigationService)
        {
            _gitHubService = gitHubService;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.GetNavigationMode() == NavigationMode.Back && GistList?.Count > 0)
                return;

            ResetProps();

            IsBusy = true;
        }

        private void ResetProps()
        {
            LanguageColors = Helpers.LanguageColors.GetList();
        }

        public ICommand LoadDataCommand => new DelegateCommand(async () =>
        {
            try
            {
                await LoadData();
            }
            catch (Exception)
            {
                ShowDefaultErrorMsg();
            }
            finally
            {
                IsBusy = false;
            }
        });

        private async Task LoadData()
        {
            await GetGistListFromService();
        }

        private async Task GetGistListFromService()
        {
            var gistList = await _gitHubService.GetGistList(LastPage);

            if (gistList == null || gistList.Count.Equals(0))
            {
                DialogService.Toast("Final da lista");
                return;
            }

            foreach (var item in gistList)
            {
                item.FirstFile.ColorFromLanguage = LanguageColors?.FirstOrDefault(x => x.Language?.ToLower() == item?.FirstFile?.Language?.ToLower())?.Color ?? "#2980b9";
            }

            if (LastPage == 1)
                GistList = gistList.ToObservableCollection();

            else
            {
                foreach (var item in gistList)
                {
                    GistList.Add(item);
                }
            }
        }

        public ICommand LoadMoreCommand => new DelegateCommand(async () =>
        {
            try
            {
                if (IsLoad || IsBusy)
                    return;

                IsLoad = true;

                LastPage += 1;

                await GetGistListFromService();
            }
            catch (Exception e)
            {
                ShowDefaultErrorMsg();
            }
            finally
            {
                IsLoad = false;
            }
        });

        public ICommand NavigateToFavoriteCommand => new DelegateCommand(async () =>
        {
            await NavigationService.NavigateAsync(nameof(FavoritePage));
        });

        public ICommand SettingsCommand => new Command(async () =>
        {
            var preferences = new List<Acr.UserDialogs.ActionSheetOption>
            {
                new Acr.UserDialogs.ActionSheetOption(PreferenceService.Theme == "light" ? "Dark Mode" : "Light Mode", () =>
                {
                    PreferenceService.Theme = PreferenceService.Theme == "light" ? "dark" : "light";
                    App.SetThemeColorsByPreference();
                })
            };

            DialogService.ActionSheet(new Acr.UserDialogs.ActionSheetConfig
            {
                Title = "Preferências",
                Options = preferences,
                Cancel = new Acr.UserDialogs.ActionSheetOption("Cancelar", () =>
                {
                    return;
                })
            });
        });

        public DelegateCommand<Gist> NavigateToDetailCommand => new DelegateCommand<Gist>(async (gist) =>
        {
            var navigationParams = new NavigationParameters
            {
                { "gist", gist }
            };

            await NavigationService.NavigateAsync(nameof(DetailPage), navigationParams);
        });

        public DelegateCommand<Gist> AddFavoriteCommand => new DelegateCommand<Gist>((gist) =>
        {
            GistDataBase.UpInsert(gist);

            DialogService.Toast("Adicionado aos favoritos com sucesso!");
        });

        private ObservableCollection<Gist> _gistList;
        public ObservableCollection<Gist> GistList
        {
            set => SetProperty(ref _gistList, value);
            get => _gistList;
        }

        public int LastPage { get; set; } = 1;

        public List<LanguageColors> LanguageColors { get; set; }
    }
}
