using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace ScripTube.Utils
{
    public abstract class EventArgsConverterExtension<T> : MarkupExtension, IEventArgsConverter
        where T : class, new()
    {
        private static Lazy<T> mConverter = new Lazy<T>(() => new T());

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return mConverter.Value;
        }

        public abstract object Convert(object value, object parameter);
    }
}
