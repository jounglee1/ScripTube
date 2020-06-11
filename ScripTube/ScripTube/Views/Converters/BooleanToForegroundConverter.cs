using ScripTube.Utils;
using System;
using System.Globalization;
using System.Windows.Media;

namespace ScripTube.Views.Converters
{
    public class BooleanToForegroundConverter : ConverterMarkupExtension<BooleanToForegroundConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
            {
                return new SolidColorBrush(Colors.Gray);
            }
            return ((bool)value) ? new SolidColorBrush(Colors.OrangeRed) : new SolidColorBrush(Colors.Gray);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
