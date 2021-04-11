using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DiscoverGists.Style
{
    public class MaterialColor
    {
        public Color Primary { get; set; }

        public Color PrimaryLight { get; set; }

        public Color PrimaryDark { get; set; }

        public Color Secondary { get; set; }

        public Color SecondaryLight { get; set; }

        public Color SecondaryDark { get; set; }

        public Color TextOnPrimary { get; set; }

        public Color TextOnSecondary { get; set; }

        public Color White => Color.White;

        public Color Black => Color.Black;

        public static MaterialColor GetByCurrentResourceThemeColor()
        {
            return new MaterialColor
            {
                Primary = (Color)App.Current.Resources["primary"],
                PrimaryDark = (Color)App.Current.Resources["primaryDark"],
                PrimaryLight = (Color)App.Current.Resources["primaryLight"],

                Secondary = (Color)App.Current.Resources["secondary"],
                SecondaryDark = (Color)App.Current.Resources["secondaryDark"],
                SecondaryLight = (Color)App.Current.Resources["secondaryLight"],

                TextOnPrimary = (Color)App.Current.Resources["textOnPrimary"],
                TextOnSecondary = (Color)App.Current.Resources["textOnSecondary"],
            };
        }

        public static MaterialColor GetDarkTheme()
        {
            return new MaterialColor
            {
                Primary = Color.FromHex("#000000"),
                PrimaryDark = Color.FromHex("#2c2c2c"),
                PrimaryLight = Color.FromHex("#2c2c2c"),

                TextOnPrimary = Color.FromHex("#ffffff"),
            };
        }

        public static MaterialColor GetLightTheme()
        {
            return new MaterialColor
            {
                Primary = Color.FromHex("#ffffff"),
                PrimaryDark = Color.FromHex("#cccccc"),
                PrimaryLight = Color.FromHex("#ecf0f1"),

                TextOnPrimary = Color.FromHex("#212121"),
            };
        }

        public static void SetOnCurrentResourceThemeColor(MaterialColor colors)
        {
            App.Current.Resources["primary"] = colors.Primary;
            App.Current.Resources["primaryLight"] = colors.PrimaryLight;
            App.Current.Resources["primaryDark"] = colors.PrimaryDark;

            App.Current.Resources["textOnPrimary"] = colors.TextOnPrimary;
        }

    }
}
