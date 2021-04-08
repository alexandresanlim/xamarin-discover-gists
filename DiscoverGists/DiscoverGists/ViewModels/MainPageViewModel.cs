using DiscoverGists.Extentions;
using DiscoverGists.Models;
using DiscoverGists.Services;
using DiscoverGists.Services.Interfaces;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
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
            }
            catch (Exception e)
            {

            }
            finally
            {
                IsBusy = false;
            }
        }

        private ObservableCollection<Gist> _gistList;
        public ObservableCollection<Gist> GistList
        {
            set => SetProperty(ref _gistList, value);
            get => _gistList;
        }
    }
}
