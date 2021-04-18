using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms.PancakeView;

namespace DiscoverGists.Controls
{
    public class CustomFrame : PancakeView
    {
        public CustomFrame()
        {
            if (DeviceInfo.Platform == DevicePlatform.iOS)
                Shadow = null;
        }
    }
}
