using DiscoverGists.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiscoverGists.ViewModels
{
    public class DetailPageViewModel : ViewModelBase
    {
        public DetailPageViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            var gist = parameters.GetValue<Gist>("gist");

            if (!string.IsNullOrEmpty(gist?.Id))
                Gist = gist;
        }

        private Gist _gist;
        public Gist Gist
        {
            set => SetProperty(ref _gist, value);
            get => _gist;
        }
    }
}
