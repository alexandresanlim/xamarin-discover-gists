using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DiscoverGists.Controls
{
    public class CustomCollectionView : CollectionView
    {
        public CustomCollectionView()
        {
            PropertyChanged += CollectionView_PropertyChanged;
        }

        private void CollectionView_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(CollectionView.ItemsSource)))
            {
                var c = (CollectionView)sender;

                Task.Run(async () =>
                {
                    c.Opacity = 0;
                    await c.FadeTo(1, 500);
                });
            }
        }
    }
}
