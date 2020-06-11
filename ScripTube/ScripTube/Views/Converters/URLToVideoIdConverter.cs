using ScripTube.Utils;
using System;
using System.Globalization;

namespace ScripTube.Views.Converters
{
    public class UrlToVideoIdConverter : ConverterMarkupExtension<UrlToVideoIdConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string url = value as string;
            if (url != null)
            {
                return YouTubeUtil.GetVideoIdByUrl(url);
            }
            return string.Empty;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
