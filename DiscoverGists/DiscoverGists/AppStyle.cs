using DiscoverGists.Interfaces;
using DiscoverGists.Services;
using DiscoverGists.Style;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DiscoverGists
{
    public partial class App
    {
        public static MaterialColor ThemeColors => MaterialColor.GetByCurrentResourceThemeColor();

        public static void SetThemeColorsByPreference()
        {
            if (PreferenceService.Theme == "light")
                MaterialColor.SetOnCurrentResourceThemeColor(MaterialColor.GetLightTheme());

            else
                MaterialColor.SetOnCurrentResourceThemeColor(MaterialColor.GetDarkTheme());

            var service = DependencyService.Get<IStatusBar>();
            service?.SetStatusBarColor(ThemeColors.Secondary);
        }
    }
}
