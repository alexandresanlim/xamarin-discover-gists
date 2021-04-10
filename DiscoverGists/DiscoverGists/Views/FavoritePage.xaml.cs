using DiscoverGists.ViewModels;
using Xamarin.Forms;

namespace DiscoverGists.Views
{
    public partial class FavoritePage : ContentPage
    {
        public FavoritePageViewModel VM { get; set; }

        public bool IsBack { get; set; }

        public FavoritePage()
        {
            InitializeComponent();

            VM = (FavoritePageViewModel)BindingContext;
        }

        protected override bool OnBackButtonPressed()
        {
            IsBack = true;

            return base.OnBackButtonPressed();
        }

        private void StackLayout_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var searchPanel = (StackLayout)sender;

            if (e.PropertyName.Equals(nameof(StackLayout.IsVisible)) && !IsBack)
            {
                if (searchPanel.IsVisible)
                    entrySearch.Focus();

                else
                    entrySearch.Unfocus();
            }
        }

        private void entrySearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            VM.Search(e.NewTextValue);
        }
    }
}
