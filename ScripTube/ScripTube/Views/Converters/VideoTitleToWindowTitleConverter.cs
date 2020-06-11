using System;
using System.Globalization;
using System.Windows.Data;

namespace ScripTube.Views.Converters
{
    class VideoTitleToWindowTitleConverter : IValueConverter
    {
        private static string WINDOW_TITLE = "ScripTube";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string videoTitle = value as string;

            if (videoTitle == null || videoTitle.Trim() == string.Empty)
            {
                return WINDOW_TITLE;
            }
            
            return string.Format("{0} - {1}", videoTitle, WINDOW_TITLE);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return WINDOW_TITLE;
        }
    }
}
