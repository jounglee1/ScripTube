using System;
using System.Collections.Generic;
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

namespace PleaseTranscribeYouTube
{
    /// <summary>
    /// YouTubeURLDialog.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class YouTubeURLDialog : UserControl
    {
        public string VideoID;

        public YouTubeURLDialog()
        {
            InitializeComponent();
            string url = Clipboard.GetText();
            string id = getYouTubeVideoID(url);
            if (id.Length <= 1)
            {
                xTextBlockError.Visibility = Visibility.Visible;
            }
            else
            {
                xTextBoxURL.Text = url;
            }
        }

        string getYouTubeVideoID(string url)
        {
            try
            {
                var uri = new Uri(url);
                var query = HttpUtility.ParseQueryString(uri.Query);
                if (query.AllKeys.Contains("v"))
                {
                    return query["v"];
                }
                else
                {
                    return uri.Segments.Last();
                }
            }
            catch (System.UriFormatException)
            {
                return "";
            }
        }

        private void xButtonOK_Click(object sender, RoutedEventArgs e)
        {
            string url = xTextBoxURL.Text;
            string id = getYouTubeVideoID(url);
            if (id.Length <= 1)
            {
                xTextBoxURL.Text = url;
                xTextBlockError.Visibility = Visibility.Visible;
            }
            else
            {
                xTextBlockError.Visibility = Visibility.Hidden;
                VideoID = id;
            }
        }

        private void xButtonCancel_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
