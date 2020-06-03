using ScripTube.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Data;

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
