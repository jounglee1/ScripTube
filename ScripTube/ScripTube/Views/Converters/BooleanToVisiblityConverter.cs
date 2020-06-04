using ScripTube.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ScripTube.Views.Converters
{
    public class BooleanToVisibilityConverter : ConverterMarkupExtension<BooleanToVisibilityConverter>
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
                return Visibility.Hidden;
            }   
            return ((bool)value) ? Visibility.Visible : Visibility.Hidden;
        }
    }
}
