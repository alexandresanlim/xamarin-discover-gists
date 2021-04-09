using System;
using System.Collections.Generic;
using System.Text;

namespace DiscoverGists.Extentions
{
    public static class IconExtention
    {
        public static string GetIconFontFamily(FontAwesomeType type = FontAwesomeType.solid)
        {
            switch (type)
            {
                default:
                case FontAwesomeType.solid:
                    return "FontAwesomeSolid";

                case FontAwesomeType.brand:
                    return "FontAwesomeBrands";
            }
        }

        public enum FontAwesomeType
        {
            solid,
            brand
        }
    }
}
