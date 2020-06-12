using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace ScripTube.Utils
{
    public abstract class MultiConverterMarkupExtension<T> : MarkupExtension, IMultiValueConverter
        where T : class, new()
    {
        private static Lazy<T> mConverter = new Lazy<T>(() => new T());

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return mConverter.Value;
        }

        public abstract object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);
        public abstract object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture);
    }
}