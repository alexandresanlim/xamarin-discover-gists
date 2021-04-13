using Acr.UserDialogs;
using DiscoverGists.DataBase;
using DiscoverGists.Models;
using Microsoft.AppCenter.Analytics;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace DiscoverGists.ViewModels
{
    public class ViewModelBase : BindableBase, IInitialize, INavigationAware, IDestructible
    {
        protected INavigationService NavigationService { get; private set; }

        public IUserDialogs DialogService => UserDialogs.Instance;

        public ViewModelBase(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public void SetIsLoading(bool isLoading = true, string title = "")
        {
            if (isLoading)
                DialogService.ShowLoading(title);

            else
                DialogService.HideLoading();
        }

        public void SetEvent(string text)
        {
            Analytics.TrackEvent(text);
        }

        public ICommand CommingSoonCommand => new DelegateCommand(() =>
        {
            ComingSoonMsg();
        });

        public void ComingSoonMsg()
        {
            DialogService.Toast("Em breve!");
        }

        public virtual void Initialize(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {

        }

        public virtual void Destroy()
        {

        }

        #region Props

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private bool _isBusy;
        public bool IsBusy
        {
            set => SetProperty(ref _isBusy, value);
            get => _isBusy;
        }

        private string _collectionEmptyMsg;
        public string CollectionEmptyMsg
        {
            set => SetProperty(ref _collectionEmptyMsg, value);
            get => _collectionEmptyMsg;
        }

        private bool _isLoad;
        public bool IsLoad
        {
            set => SetProperty(ref _isLoad, value);
            get => _isLoad;
        }

        #endregion
    }
}
