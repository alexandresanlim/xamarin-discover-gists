using DiscoverGists.DataBase;
using DiscoverGists.Extentions;
using DiscoverGists.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

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

        private void LoadData()
        {
            var list = GistDataBase.GetAll();

            if (list != null && list.Count > 0)
                GistList = list.ToObservableCollection();
        }

        private ObservableCollection<Gist> _gistList;
        public ObservableCollection<Gist> GistList
        {
            set => SetProperty(ref _gistList, value);
            get => _gistList;
        }
    }
}
