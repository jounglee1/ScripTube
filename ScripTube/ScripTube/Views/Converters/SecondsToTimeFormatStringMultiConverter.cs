using ScripTube.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ScripTube.Views.Converters
{
    class SecondsToTimeFormatStringMultiConverter : MultiConverterMarkupExtension<SecondsToTimeFormatStringMultiConverter>
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || !(values[0] is bool) || !(values[1] is double))
            {
                return TimeFormatUtil.INVALID_HHMMSS;
            }
            return TimeFormatUtil.GetHHMMSSOrMMSS((double)values[1], (bool)values[0]);
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
