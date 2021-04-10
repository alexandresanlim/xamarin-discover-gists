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
            Title = "Favorites";
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            try
            {
                IsBusy = true;

                ResetProps();

                LoadData();
            }
            catch (Exception e)
            {

                throw;
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
        }

        private void LoadData()
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
        }

        public void Search(string text)
        {
            if (string.IsNullOrEmpty(text))
                GistList = OriginalGistList.ToObservableCollection();

            else
                GistList = OriginalGistList.Where(x => x.FirstFile.Filename.ToLower().Contains(text.ToLower()))?.ToObservableCollection();
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
