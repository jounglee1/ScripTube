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
    class SecondsToTimeFormatStringConverter : MultiConverterMarkupExtension<SecondsToTimeFormatStringConverter>
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || !(values[0] is bool) || !(values[1] is double))
            {
                return "--:--:--";
            }

            bool bHourExcessed = (bool)values[0];
            int seconds = (int)(double)values[1];

            int min = seconds / 60;
            int sec = seconds % 60;

            if (bHourExcessed)
            {
                int hour = min / 60;
                min %= 60;
                return string.Format("{0}:{1}:{2}", hour.ToString("D2"), min.ToString("D2"), sec.ToString("D2"));
            }
            return string.Format("{0}:{1}", min.ToString("D2"), sec.ToString("D2"));
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
