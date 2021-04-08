using DiscoverGists.Services;
using DiscoverGists.Services.Interfaces;
using Prism.Navigation;
using System;
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

        public ICommand LoadDataCommand => new Command(async () =>
        {
            await LoadData();
        });

        private async Task LoadData()
        {
            try
            {
                var octocat = await _gitHubService.GetUser();
            }
            catch (Exception e)
            {

            }
        }
    }
}
