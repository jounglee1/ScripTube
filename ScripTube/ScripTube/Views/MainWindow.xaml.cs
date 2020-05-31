using MaterialDesignThemes.Wpf;
using ScripTube.Models.YouTube;
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

namespace ScripTube.Views
{
    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer mDispatcherTimer = new System.Windows.Threading.DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            mDispatcherTimer.Tick += DispatcherTimer_Tick;
            mDispatcherTimer.Interval = TimeSpan.FromMilliseconds(500);
            mDispatcherTimer.Start();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (xMainWindowViewModel.SelectedSubtitle == null)
            {
                return;
            }
            try
            {
                string time = xWebView.InvokeScript("getCurrentTime");
                double d;
                if (double.TryParse(time, out d))
                {
                    var item = xMainWindowViewModel.SelectedSubtitle.HighlightSubtitleItem(d);
                    xListView.ScrollIntoView(item);
                }
            }
            catch (System.AggregateException)
            {

            }
            
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
                Debug.Assert(dialog.VideoOrNull != null);
                xMainWindowViewModel.Video = dialog.VideoOrNull;
                try
                {
                    xWebView.InvokeScript("destroyVideo");
                    xWebView.InvokeScript("onYouTubeIframeAPIReady", new string[] { dialog.VideoOrNull.ID });
                }
                catch (System.AggregateException)
                {
                    return;
                }
                if (!dialog.VideoOrNull.IsSubtitleExisted)
                {
                    MessageBox.Show("지원하는 스크립트가 없습니다.", Title, MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            xWebView.Visibility = Visibility.Visible;
        }

        private void xWindow_Closing(object sender, CancelEventArgs e)
        {
            try
            {
                mDispatcherTimer.Stop();
                xWebView.InvokeScript("stopVideo");
            }
            catch (System.AggregateException)
            {

            }
            xWebView.Close();
        }

        private void xTextBlockTimeStamp_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mDispatcherTimer.Stop();
            var textBlock = (sender as TextBlock);
            var subtitleItem = textBlock.DataContext as SubtitleItem;
            xWebView.InvokeScript("seekTo", new string[] { subtitleItem.StartSeconds.ToString() });
            xMainWindowViewModel.SelectedSubtitle.HighlightSubtitleItem(subtitleItem.StartSeconds);
            mDispatcherTimer.Start();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (sender as ListView).SelectedItem;
            var subtitle = item as SubtitleItem;
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://papago.naver.com/?sk=auto&tk=ko&st=hello+world~~");
        }

        private void MenuItem_SaveTXT(object sender, RoutedEventArgs e)
        {
            xMainWindowViewModel.SelectedSubtitle.SaveSubtitle();
        }

    }
}