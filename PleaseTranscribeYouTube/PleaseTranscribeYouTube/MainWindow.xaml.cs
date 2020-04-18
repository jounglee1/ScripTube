using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Windows.UI.Popups;

namespace PleaseTranscribeYouTube
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void xWebView_Loaded(object sender, RoutedEventArgs e)
        {
            xWebView.NavigateToLocal("yt.html");
        }

        private void xButtonStart_Click(object sender, RoutedEventArgs e)
        {
            xWebView.InvokeScript("playVideo");
        }

        private void xButtonPause_Click(object sender, RoutedEventArgs e)
        {
            xWebView.InvokeScript("pauseVideo");
        }

        private void xButtonStop_Click(object sender, RoutedEventArgs e)
        {
            xWebView.InvokeScript("stopVideo");
        }

        private async void xButtonGetTime_Click(object sender, RoutedEventArgs e)
        {
            string time = await xWebView.InvokeScriptAsync("getCurrentTime");
            MessageBox.Show(time.ToString() + " 초");
        }
    }
}
