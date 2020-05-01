using MaterialDesignThemes.Wpf;
using PleaseTranscribeYouTube.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace PleaseTranscribeYouTube
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private YouTubeVideoData mVideoData = new YouTubeVideoData("tG2GJZcBKOE");

        private ObservableCollection<YouTubeSubtitleData> mNowSubtitles;
        public ObservableCollection<YouTubeSubtitleData> NowSubtitles
        {
            get
            {
                return mNowSubtitles;
            }
            set
            {
                if (mNowSubtitles != value)
                {
                    mNowSubtitles = value;
                    notifyPropertyChanged("NowSubtitles");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void notifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }


        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
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
            if ((bool)(await DialogHost.Show(dialog, "RootDialog")))
            {
                xWebView.InvokeScript("destroyVideo");
                xWebView.InvokeScript("onYouTubeIframeAPIReady", new string[] { dialog.VideoID });
                mVideoData = new YouTubeVideoData(dialog.VideoID);
                if (mVideoData.IsSubtitleExisted)
                {
                    NowSubtitles = mVideoData.SubtitleDatas["en"];
                }
                else
                {
                    NowSubtitles = null;
                }
            }
            xWebView.Visibility = Visibility.Visible;
        }
    }
}