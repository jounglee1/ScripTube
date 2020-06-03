using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace ScripTube.Utils
{
    // https://stackoverflow.com/questions/4737970/what-does-where-t-class-new-mean
    // http://drwpf.com/blog/2009/03/17/tips-and-tricks-making-value-converters-more-accessible-in-markup/ 
    // https://blog.naver.com/vactorman/220552073364
    // https://blog.naver.com/vactorman/220516524223

    public abstract class ConverterMarkupExtension<T> : MarkupExtension, IValueConverter
        where T : class, new() // 기본 생성자를 가지는 class 타입만 T에 들어갈 수 있다
    {
        private static Lazy<T> mConverter = new Lazy<T>(() => new T());

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return mConverter.Value;
        }

        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);
        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
    }
}
