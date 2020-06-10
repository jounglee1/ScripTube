using Microsoft.Toolkit.Wpf.UI.Controls;
using ScripTube.Models.Bookmark;
using ScripTube.Models.YouTube;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScripTube.Views.Controls
{
    public partial class VideoPlayer : UserControl
    {
        public Video VideoSource
        {
            get { return (Video)GetValue(VideoSourceProperty); }
            set { SetCurrentValue(VideoSourceProperty, value); }
        }

        public double CurrentTime
        {
            get { return (double)GetValue(CurrentTimeProperty); }
            set { SetCurrentValue(CurrentTimeProperty, value); }
        }

        public double SetTime
        {
            get { return (double)GetValue(SetTimeProperty); }
            set { SetCurrentValue(SetTimeProperty, value); }
        }

        public Thumbnail GenerateThumbnail
        {
            get { return (Thumbnail)GetValue(GenerateThumbnailProperty); }
            set { SetValue(GenerateThumbnailProperty, value); }
        }

        [Obsolete]
        public VideoPlayer()
        {
            InitializeComponent();
            xWebView.NavigateToLocal("yt.html");
            mTimer.Tick += mTimer_Tick;
            mTimer.Interval = TimeSpan.FromMilliseconds(500);
        }

        public static readonly DependencyProperty VideoSourceProperty =
            DependencyProperty.Register(nameof(VideoSource), typeof(Video), typeof(VideoPlayer), new PropertyMetadata(null, notifyVideoSourcePropertyChanged));
        public static readonly DependencyProperty CurrentTimeProperty =
            DependencyProperty.Register(nameof(CurrentTime), typeof(double), typeof(VideoPlayer), new PropertyMetadata(0.0, notifyCurrentTimePropertyChanged));
        public static readonly DependencyProperty SetTimeProperty =
            DependencyProperty.Register(nameof(SetTime), typeof(double), typeof(VideoPlayer), new PropertyMetadata(0.0, notifySetTimePropertyChanged));
        public static readonly DependencyProperty GenerateThumbnailProperty =
            DependencyProperty.Register(nameof(GenerateThumbnail), typeof(Thumbnail), typeof(VideoPlayer), new PropertyMetadata(null, notifyGenerateThumbnailPropertyChanged));

        public event PropertyChangedEventHandler PropertyChanged;

        private System.Windows.Threading.DispatcherTimer mTimer = new System.Windows.Threading.DispatcherTimer();

        [Obsolete]
        public void NavigateToLocal(string relativePath)
        {
            xWebView.NavigateToLocal(relativePath);
        }

        public void SeekTo(double time)
        {
            try
            {
                xWebView.InvokeScript("seekTo", new string[] { time.ToString() });
            }
            catch (System.AggregateException)
            {
                return;
            }
        }

        public void Play()
        {
            xWebView.InvokeScript("playVideo");
        }

        public void Pause()
        {
            xWebView.InvokeScript("pauseVideo");
        }

        public void Stop()
        {
            xWebView.InvokeScript("stopVideo");
        }

        private void saveSnapShot(Thumbnail thumbnail)
        {
            var topLeftCorner = xWebView.PointToScreen(new System.Windows.Point(0, 0));
            var topLeftGdiPoint = new System.Drawing.Point((int)topLeftCorner.X, (int)topLeftCorner.Y);
            var size = new System.Drawing.Size((int)xWebView.ActualWidth, (int)xWebView.ActualHeight);
            Bitmap screenShot = new Bitmap((int)xWebView.ActualWidth, (int)xWebView.ActualHeight);
            using (var graphics = Graphics.FromImage(screenShot))
            {
                graphics.CopyFromScreen(topLeftGdiPoint, new System.Drawing.Point(), size, CopyPixelOperation.SourceCopy);
                
            }
            if (!File.Exists(thumbnail.ImagePath))
            {
                screenShot.Save(thumbnail.ImagePath, ImageFormat.Png);
            }
            //thumbnail.Bitmap = screenShot;
            //thumbnail.BitmapSource = convert(screenShot);
        }

        private BitmapSource convert(Bitmap bitmap)
        {
            var bitmapData = bitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);

            var bitmapSource = BitmapSource.Create(
                bitmapData.Width, bitmapData.Height,
                bitmap.HorizontalResolution, bitmap.VerticalResolution,
                PixelFormats.Bgr24, null,
                bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);

            bitmap.UnlockBits(bitmapData);
            return bitmapSource;
        }

        private void mTimer_Tick(object sender, EventArgs e)
        {
            if (VideoSource != null)
            {
                try
                {
                    string stringTime = xWebView.InvokeScript("getCurrentTime");
                    if (double.TryParse(stringTime, out double time))
                    {
                        CurrentTime = time;
                    }
                }
                catch (System.Exception)
                {
                }
            }
        }

        private static void notifyVideoSourcePropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs args)
        {
            var player = source as VideoPlayer;
            if (player != null && player.VideoSource.ID != string.Empty)
            {
                try
                {
                    player.xWebView.InvokeScript("destroyVideo");
                    player.xWebView.InvokeScript("onYouTubeIframeAPIReady", new string[] { player.VideoSource.ID });
                }
                catch (System.AggregateException)
                {
                    return;
                }
                player.mTimer.Start();
            }
            else
            {
                player.mTimer.Stop();
            }
        }

        private static void notifyCurrentTimePropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            var player = source as VideoPlayer;
            if (player != null)
            {
                player.notifyPropertyChanged(nameof(CurrentTime));
            }
        }

        private static void notifySetTimePropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            var player = source as VideoPlayer;
            if (player != null)
            {
                player.SeekTo((double)e.NewValue);
                player.notifyPropertyChanged(nameof(SetTime));
            }
        }

        private static void notifyGenerateThumbnailPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            var player = source as VideoPlayer;
            if (player != null)
            {
                player.saveSnapShot(player.GenerateThumbnail);
                player.notifyPropertyChanged(nameof(GenerateThumbnail));
            }
        }

        private void xWebView_NavigationCompleted(object sender, Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlNavigationCompletedEventArgs e)
        {
            mTimer.Start();
        }

        private void notifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
