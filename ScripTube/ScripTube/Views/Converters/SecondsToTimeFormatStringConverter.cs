using ScripTube.Utils;
using System;
using System.Globalization;

namespace ScripTube.Views.Converters
{
    class SecondsToTimeFormatStringConverter : ConverterMarkupExtension<SecondsToTimeFormatStringConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return TimeFormatUtil.INVALID_HHMMSS;
            }
            return TimeFormatUtil.GetHHMMSSOrMMSSPrecision((double)value, true);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
