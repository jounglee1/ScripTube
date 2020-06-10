using ScripTube.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScripTube.Views.Converters
{
    class SecondsToTimeFormatStringConverter : ConverterMarkupExtension<SecondsToTimeFormatStringConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "--:--:--";
            }

            int seconds = (int)(double)value;
            int min = seconds / 60;
            int sec = seconds % 60;

            if (seconds >= 3600)
            {
                int hour = min / 60;
                min %= 60;

                return string.Format("{0}:{1}:{2}", hour.ToString("D2"), min.ToString("D2"), sec.ToString("D2"));
            }

            return string.Format("{0}:{1}", min.ToString("D2"), sec.ToString("D2"));
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
