using DiscoverGists.DataBase;
using DiscoverGists.Extentions;
using DiscoverGists.Models;
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

namespace DiscoverGists.ViewModels
{
    public class FavoritePageViewModel : ViewModelBase
    {
        public FavoritePageViewModel(INavigationService navigationService)
            : base(navigationService)
        {

        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            try
            {
                SetIsLoading(true);

                await Task.Delay(1000);

                LoadDataCommand.Execute(null);
            }
            catch (Exception)
            {
                ShowDefaultErrorMsg();
            }
            finally
            {
                SetIsLoading(false);
            }
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

                await Task.Delay(1000);

                ResetProps();

                GetListFromDataBase();
            }
            catch (Exception e)
            {
                ShowDefaultErrorMsg();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void ResetProps()
        {
            SearchPanelVisible = false;
            OriginalGistList = new List<Gist>();
            GistList = new ObservableCollection<Gist>();
        }

        private void GetListFromDataBase()
        {
            var list = GistDataBase.GetAll();

            if (list != null && list.Count > 0)
            {
                OriginalGistList = list;

                GistList = OriginalGistList.ToObservableCollection();

                var languageColors = LanguageColors.GetList();

                foreach (var item in GistList)
                {
                    item.FirstFile.ColorFromLanguage = languageColors?.FirstOrDefault(x => x.Language?.ToLower() == item?.FirstFile?.Language?.ToLower())?.Color ?? "#2980b9";
                }
            }

            else
                CollectionEmptyMsg = "Você ainda não adicionou nenhum item aos seus favoritos";
        }

        public void Search(string text = "")
        {
            GistList = string.IsNullOrEmpty(text) ? OriginalGistList.ToObservableCollection() : OriginalGistList.Where(x => x.FirstFile.Filename.ToLower().Contains(text.ToLower()) || x.Owner.Login.ToLower().Contains(text.ToLower()))?.ToObservableCollection();

            if (GistList.Count.Equals(0))
                CollectionEmptyMsg = "Nenhum resultado encontrado 😣";
        }

        public DelegateCommand<Gist> NavigateToDetailCommand => new DelegateCommand<Gist>(async (gist) =>
        {
            var navigationParams = new NavigationParameters
            {
                { "gist", gist }
            };

            await NavigationService.NavigateAsync(nameof(DetailPage), navigationParams);
        });

        public DelegateCommand<Gist> RemoveFromFavoriteCommand => new DelegateCommand<Gist>(async (gist) =>
        {
            var confirm = await DialogService.ConfirmAsync("Confirm remove?", "Confirmation");

            if (!confirm)
                return;

            GistDataBase.Remove(gist);

            GistList.Remove(gist);
        });

        public ICommand ShowSearchPanelCommand => new DelegateCommand(() =>
        {
            SearchPanelVisible = !SearchPanelVisible;

            if (!SearchPanelVisible)
                Search();
        });

        private ObservableCollection<Gist> _gistList;
        public ObservableCollection<Gist> GistList
        {
            set => SetProperty(ref _gistList, value);
            get => _gistList;
        }

        public List<Gist> OriginalGistList { get; set; }

        private bool _searchPanelVisible;
        public bool SearchPanelVisible
        {
            set => SetProperty(ref _searchPanelVisible, value);
            get => _searchPanelVisible;
        }
    }
}
