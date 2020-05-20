using MaterialDesignThemes.Wpf;
using ScripTube.Classes.YouTube;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
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
using System.Xml;
using Windows.UI.Popups;

namespace ScripTube
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

        [Obsolete]
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

        private async void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            xWebView.Visibility = Visibility.Collapsed;
            var dialog = new YouTubeURLDialog();
            if ((bool)await DialogHost.Show(dialog, "RootDialog"))
            {
                Debug.Assert(dialog.EntityOrNull != null);
                xMainViewModel.Entity = dialog.EntityOrNull;
                try
                {
                    xWebView.InvokeScript("destroyVideo");
                    xWebView.InvokeScript("onYouTubeIframeAPIReady", new string[] { dialog.EntityOrNull.ID });
                }
                catch (System.AggregateException)
                {
                    return;
                }
            }
            xWebView.Visibility = Visibility.Visible;
        }

        private void xWindow_Closing(object sender, CancelEventArgs e)
        {
            xWebView.Close();
        }
    }
}