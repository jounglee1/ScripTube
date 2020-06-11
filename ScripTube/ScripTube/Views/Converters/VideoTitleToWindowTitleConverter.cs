using ScripTube.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ScripTube.Views.Converters
{
    class VideoTitleToWindowTitleConverter : IValueConverter
    {
        private static string WINDOW_TITLE = "ScripTube";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string videoTitle = value as string;

            if (videoTitle != null && videoTitle.Trim() != string.Empty)
            {
                return string.Format("{0} - {1}", videoTitle, WINDOW_TITLE);
            }
            return WINDOW_TITLE;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return WINDOW_TITLE;
        }
    }
}
