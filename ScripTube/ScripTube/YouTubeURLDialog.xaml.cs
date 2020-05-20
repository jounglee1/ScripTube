using MaterialDesignThemes.Wpf;
using ScripTube.Classes;
using ScripTube.Classes.YouTube;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScripTube
{
    public partial class YouTubeURLDialog : UserControl
    {
        public Entity EntityOrNull;

        public YouTubeURLDialog()
        {
            InitializeComponent();
            xTextBoxURL.Text = "https://www.youtube.com/watch?v=qC5KtatMcUw";//Clipboard.GetText().Trim();
        }

        private void xButtonLoad_Click(object sender, RoutedEventArgs e)
        {
            string idOrNull = getYouTubeVideoIDOrNull(xTextBoxURL.Text);
            var entity = new Entity(idOrNull);
            if (entity.Status == EVideoStatus.OK)
            {
                EntityOrNull = entity;
                xButtonClose.Command.Execute(true); // xButtonClose.CommandParameter
            }
            else if (entity.Status == EVideoStatus.UNPLAYABLE)
            {
                MessageBox.Show("저작권의 이유로 인하여 불러올 수 없는 동영상입니다.");
                xTextBoxURL.Focus();
                xTextBoxURL.SelectAll();
            }
            else if (entity.Status == EVideoStatus.ERROR)
            {
                MessageBox.Show("유효하지 않은 동영상입니다.");
                xTextBoxURL.Focus();
                xTextBoxURL.SelectAll();
            }
        }

        private void xTextBoxURL_TextChanged(object sender, TextChangedEventArgs e)
        {
            string idOrNull = getYouTubeVideoIDOrNull(xTextBoxURL.Text);
            if (isIdStringValid(idOrNull))
            {
                xButtonLoad.IsEnabled = true;
                xTextBlockError.Visibility = Visibility.Hidden;
            }
            else
            {
                xButtonLoad.IsEnabled = false;
                xTextBlockError.Visibility = Visibility.Visible;
            }
        }

        private string getYouTubeVideoIDOrNull(string url)
        {
            try
            {
                Uri uri = new Uri(url);
                string host = uri.Host;
                if (host != "www.youtube.com" && host != "youtu.be")
                {
                    return null;
                }
                NameValueCollection query = HttpUtility.ParseQueryString(uri.Query);
                if (query.AllKeys.Contains("v"))
                {
                    return query["v"];
                }
                return null;
            }
            catch (System.UriFormatException)
            {
                return null;
            }
        }

        private bool isIdStringValid(string idOrNull)
        {
            return idOrNull != null && idOrNull.Length == Entity.VIDEO_ID_LENGTH;
        }
    }
}
