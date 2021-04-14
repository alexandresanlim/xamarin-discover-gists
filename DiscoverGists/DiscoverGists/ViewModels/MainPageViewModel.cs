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

        #region Commands

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
            catch (Exception ex)
            {
                ex.SendToLog();
            }
            finally
            {
                IsLoad = false;
            }
        });

        public ICommand SettingsCommand => new Command(async () =>
        {
            var preferences = new List<Acr.UserDialogs.ActionSheetOption>
            {
                new Acr.UserDialogs.ActionSheetOption(PreferenceService.Theme == "light" ? "Dark Mode" : "Light Mode", () =>
                {
                    PreferenceService.Theme = PreferenceService.Theme == "light" ? "dark" : "light";

                    App.SetThemeColorsByPreference();

                    GistList.SetIsFavorite();
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

        public DelegateCommand<Gist> AddFavoriteCommand => new DelegateCommand<Gist>((gist) =>
        {
            try
            {
                gist.RemoveOrAddGistFromFavorite();
            }
            catch (Exception ex)
            {
                ex.SendToLog();
            }
            finally
            {
                SetEvent("Add favorite");
            }
        });

        public ICommand LoadDataCommand => new DelegateCommand(async () =>
        {
            try
            {
                await LoadData();
            }
            catch (Exception ex)
            {
                ex.SendToLog();
            }
            finally
            {
                IsBusy = false;

                SetEvent("Load app");
            }
        });

        public ICommand NavigateToFavoriteCommand => new DelegateCommand(async () =>
        {
            try
            {
                SetIsLoading(true);

                await Task.Delay(1000);

                await NavigationService.NavigateAsync(nameof(FavoritePage));
            }
            catch (Exception e)
            {
                e.SendToLog();
            }
            finally
            {
                SetIsLoading(false);
            }
        });

        public DelegateCommand<Gist> NavigateToDetailCommand => new DelegateCommand<Gist>(async (gist) =>
        {
            try
            {
                SetIsLoading(true);

                await Task.Delay(1000);

                var navigationParams = new NavigationParameters
                {
                    { "gist", gist }
                };

                await NavigationService.NavigateAsync(nameof(DetailPage), navigationParams);
            }
            catch (Exception e)
            {
                e.SendToLog();
            }
            finally
            {
                SetIsLoading(false);
            }
        });

        #endregion

        private void ResetProps()
        {
            IsBusy = false;
        }

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

            gistList.SetIsFavorite();

            gistList.Select(x => x.FirstFile).ToList().SetLanguageColor();

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

        private ObservableCollection<Gist> _gistList;
        public ObservableCollection<Gist> GistList
        {
            set => SetProperty(ref _gistList, value);
            get => _gistList;
        }

        public int LastPage { get; set; } = 1;
    }
}
