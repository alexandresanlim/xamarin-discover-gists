using Acr.UserDialogs;
using DiscoverGists.ViewModels;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DiscoverGists.Views
{
    public partial class WebViewPage : ContentPage
    {
        public WebViewPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            Wv.Navigating += Wv_Navigating;

            base.OnAppearing();
        }

        private async void Wv_Navigating(object sender, WebNavigatingEventArgs e)
        {
            UserDialogs.Instance.ShowLoading("");

            await Task.Delay(2000);

            UserDialogs.Instance.HideLoading();
        }
    }
}
