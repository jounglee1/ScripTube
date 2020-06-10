using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ScripTube.Views.Controls
{
    // https://stackoverflow.com/questions/18060591/select-and-copy-text-in-wpf-textbox-using-mvvm
    public class UrlTextBox : TextBox
    {
        public bool IsSelectedText
        {
            get { return (bool)GetValue(IsSelectedTextProperty); }
            set { SetValue(IsSelectedTextProperty, value); }
        }

        public static readonly DependencyProperty IsSelectedTextProperty = DependencyProperty.RegisterAttached(nameof(IsSelectedText),
             typeof(bool), typeof(UrlTextBox), new FrameworkPropertyMetadata(false, notifyIsSelectedChanged));

        private static void notifyIsSelectedChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            UrlTextBox textbox = sender as UrlTextBox;

            if ((bool)e.NewValue)
            {
                textbox.Focus();
                textbox.SelectAll();
            }
        }
    }
}
