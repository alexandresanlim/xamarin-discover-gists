using DiscoverGists.Interfaces;
using DiscoverGists.Services;
using DiscoverGists.Services.Interfaces;
using DiscoverGists.ViewModels;
using DiscoverGists.Views;
using Prism;
using Prism.Ioc;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;

namespace DiscoverGists
{
    public partial class App
    {
        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            App.SetThemeColorsByPreference();

            await NavigationService.NavigateAsync("NavigationPage/MainPage");

            var navigationPage = Application.Current.MainPage as NavigationPage;
            navigationPage.BarBackgroundColor = App.ThemeColors.Secondary;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();
            containerRegistry.RegisterSingleton<IGitHubService, GitHubService>();
            
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<FavoritePage, FavoritePageViewModel>();
            containerRegistry.RegisterForNavigation<DetailPage, DetailPageViewModel>();
            containerRegistry.RegisterForNavigation<WebViewPage, WebViewPageViewModel>();
        }
    }
}
