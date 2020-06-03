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
            return getVisibility(value);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private object getVisibility(object value)
        {
            if (!(value is bool))
            {
                return Visibility.Visible;
            }   
            return ((bool)value) ? Visibility.Hidden : Visibility.Visible;
        }
    }
}
