﻿using DiscoverGists.ViewModels;
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

        protected override void OnDisappearing()
        {
            IsBack = true;

            base.OnDisappearing();
        }

        private void StackLayout_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (IsBack)
                return;

            var searchPanel = (StackLayout)sender;

            if (e.PropertyName.Equals(nameof(StackLayout.IsVisible)))
            {
                if (searchPanel.IsVisible)
                    entrySearch.Focus();

                else
                {
                    if (!string.IsNullOrEmpty(entrySearch?.Text))
                        entrySearch.Text = "";

                    entrySearch.Unfocus();
                }
            }
        }

        private void entrySearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            VM.Search(e?.NewTextValue);
        }
    }
}
