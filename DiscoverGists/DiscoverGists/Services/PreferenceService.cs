using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DiscoverGists.Services
{
    public static class PreferenceService
    {
        public static string ThemeFromSystem
        {
            get
            {
                const string light = "light";

                const string dark = "dark";

                try
                {
                    switch (Application.Current.RequestedTheme)
                    {
                        case OSAppTheme.Light:
                            return light;

                        case OSAppTheme.Dark:
                            return dark;

                        case OSAppTheme.Unspecified:
                        default:
                            return light;
                    }
                }
                catch (Exception)
                {
                    return light;
                }
            }
        }

        public static string Theme
        {
            get => Preferences.Get(nameof(Theme), ThemeFromSystem);
            set
            {
                if (Theme == value)
                    return;

                Preferences.Set(nameof(Theme), value);
            }
        }
    }
}
