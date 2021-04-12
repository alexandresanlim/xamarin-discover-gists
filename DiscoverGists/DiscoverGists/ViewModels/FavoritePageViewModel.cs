using DiscoverGists.DataBase;
using DiscoverGists.Extentions;
using DiscoverGists.Helpers;
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
            if (parameters.GetNavigationMode() == NavigationMode.Back)
                return;

            try
            {
                //SetIsLoading(true);

                //await Task.Delay(1000);

                await LoadData();
            }
            catch (Exception)
            {
                ShowDefaultErrorMsg();
            }
            finally
            {
                //SetIsLoading(false);
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
            LanguageColors = Helpers.LanguageColors.GetList();
            Skip = 0;
            EndList = false;
        }

        private void GetListFromDataBase()
        {
            var gistList = GistDataBase.GetAll(Skip);

            if (gistList == null || gistList.Count.Equals(0))
            {
                EndList = true;
                return;
            }

            if (gistList != null && gistList.Count > 0)
            {
                foreach (var item in gistList)
                {
                    item.FirstFile.ColorFromLanguage = LanguageColors?.FirstOrDefault(x => x.Language?.ToLower() == item?.FirstFile?.Language?.ToLower())?.Color ?? "#2980b9";
                }

                if (GistList == null || GistList.Count.Equals(0))
                    GistList = gistList.ToObservableCollection();

                else
                {
                    foreach (var item in gistList)
                    {
                        GistList.Add(item);
                    }
                }

                OriginalGistList = GistList.ToList();
            }

            else
                CollectionEmptyMsg = "Você ainda não adicionou nenhum item aos seus favoritos";
        }

        public void Search(string text = "")
        {
            GistList = string.IsNullOrEmpty(text) ? OriginalGistList.ToObservableCollection() : GistDataBase.Find(x => x.Owner.Login.ToLower().Contains(text.ToLower()))?.ToObservableCollection();

            if (GistList.Count.Equals(0))
                CollectionEmptyMsg = "Nenhum resultado encontrado 😣";
        }

        public ICommand LoadMoreCommand => new DelegateCommand(async () =>
        {
            try
            {
                if (IsLoad || IsBusy || EndList)
                    return;

                IsLoad = true;

                await Task.Delay(2000);

                Skip += 5;

                GetListFromDataBase();
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

        public int Skip { get; set; }

        public bool EndList { get; set; }

        public List<LanguageColors> LanguageColors { get; set; }
    }
}
