using DiscoverGists.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiscoverGists.ViewModels
{
    public class WebViewPageViewModel : ViewModelBase
    {
        public WebViewPageViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            var url = parameters.GetValue<string>(nameof(Gist.Url));

            CurrentUrl = new Uri(url);
        }

        private Uri _currentUrl;
        public Uri CurrentUrl
        {
            set => SetProperty(ref _currentUrl, value);
            get => _currentUrl;
        }
    }
}
