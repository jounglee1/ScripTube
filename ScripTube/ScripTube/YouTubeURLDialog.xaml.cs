using MaterialDesignThemes.Wpf;
using ScripTube.Classes;
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
        public string VideoIDOrNull;

        public YouTubeURLDialog()
        {
            InitializeComponent();
            xTextBoxURL.Text = "https://www.youtube.com/watch?v=qC5KtatMcUw";//Clipboard.GetText().Trim();
        }

        private void xButtonOK_Click(object sender, RoutedEventArgs e)
        {
            string url = xTextBoxURL.Text;
            string idOrNull = getYouTubeVideoIDOrNull(url);
            if (idOrNull != null && idOrNull.Length == YouTubeVideoData.VIDEO_ID_LENGTH)
            {
                VideoIDOrNull = idOrNull;
                xButtonClose.Command.Execute(true); // xButtonClose.CommandParameter
            }
            else
            {
                VideoIDOrNull = null;
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

        private void xTextBoxURL_TextChanged(object sender, TextChangedEventArgs e)
        {
            string idOrNull = getYouTubeVideoIDOrNull(xTextBoxURL.Text);
            if (idOrNull != null && idOrNull.Length == YouTubeVideoData.VIDEO_ID_LENGTH)
            {
                xTextBlockError.Visibility = Visibility.Hidden;
            }
            else
            {
                xTextBlockError.Visibility = Visibility.Visible;
            }
        }
    }
}
