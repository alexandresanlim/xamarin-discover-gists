using DiscoverGists.DataBase;
using DiscoverGists.Extentions;
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
            Title = "Main Page";

            _gitHubService = gitHubService;

            LoadDataCommand.Execute(null);
        }

        public ICommand LoadDataCommand => new DelegateCommand(async () =>
        {
            await LoadData();
        });

        private async Task LoadData()
        {
            try
            {
                IsBusy = true;

                var gistList = await _gitHubService.GetGistList(page: 1);

                GistList = gistList.ToObservableCollection();

                var languageColors = LanguageColors.GetList();

                foreach (var item in GistList)
                {
                    item.ColorFromLanguage = languageColors?.FirstOrDefault(x => x.Language?.ToLower() == item?.FirstFile.Value?.Language?.ToLower())?.Color ?? "#2980b9";
                }
            }
            catch (Exception e)
            {
                DialogService.Toast("Ops! Something wrong has happened");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public ICommand NavigateToFavoriteCommand => new DelegateCommand(async () =>
        {
            await NavigationService.NavigateAsync(nameof(FavoritePage));
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

            DialogService.Toast("Success!");
        });

        private ObservableCollection<Gist> _gistList;
        public ObservableCollection<Gist> GistList
        {
            set => SetProperty(ref _gistList, value);
            get => _gistList;
        }
    }
}
