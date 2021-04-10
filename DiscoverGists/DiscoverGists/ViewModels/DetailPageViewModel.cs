using DiscoverGists.Extentions;
using DiscoverGists.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            {
                Gist = gist;

                FileList = Gist.Files.Select(x => x.Value).ToList().ToObservableCollection();

                if (FileList.Count > 1)
                {
                    var languageColors = LanguageColors.GetList();

                    foreach (var item in FileList)
                    {
                        item.ColorFromLanguage = !string.IsNullOrEmpty(item?.ColorFromLanguage) ? item?.ColorFromLanguage : languageColors?.FirstOrDefault(x => x.Language?.ToLower() == item?.Language?.ToLower())?.Color ?? "#2980b9";
                    }
                }
            }
        }

        private Gist _gist;
        public Gist Gist
        {
            set => SetProperty(ref _gist, value);
            get => _gist;
        }

        private ObservableCollection<File> _fileList;
        public ObservableCollection<File> FileList
        {
            set => SetProperty(ref _fileList, value);
            get => _fileList;
        }
    }
}
