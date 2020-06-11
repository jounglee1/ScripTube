using ScripTube.Utils;
using System;
using System.Globalization;

namespace ScripTube.Views.Converters
{
    class VideoAndSubtitleMultiConverter : MultiConverterMarkupExtension<VideoAndSubtitleMultiConverter>
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.Clone();
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
