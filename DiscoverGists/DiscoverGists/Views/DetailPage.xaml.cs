using Xamarin.Forms;

namespace DiscoverGists.Views
{
    public partial class DetailPage : ContentPage
    {
        public DetailPage()
        {
            var navigationPage = Application.Current.MainPage as NavigationPage;
            navigationPage.BarBackgroundColor = App.ThemeColors.Secondary;

            InitializeComponent();
        }
    }
}
