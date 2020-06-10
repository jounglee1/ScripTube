using ScripTube.Utils;
using System;
using System.Globalization;
using System.Windows;

namespace ScripTube.Views.Converters
{
    public class InvertBooleanToVisiblityConverter : ConverterMarkupExtension<InvertBooleanToVisiblityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
            {
                return Visibility.Visible;
            }
            return ((bool)value) ? Visibility.Hidden : Visibility.Visible;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
