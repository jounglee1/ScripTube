/*using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PleaseTranscribeYouTube.Classes
{
    public class SubtitleViewModel : INotifyPropertyChanged
    {
        private YouTubeSubtitleData mData;

        public string Text
        {
            get => mData.Text;
            set
            {
                if (mData.Text != value)
                {
                    mData.Text = value;
                    notifyPropertyChanged("Text");
                }
            }
        }

        public double StartSeconds
        {
            get => mData.StartSeconds;
            set
            {
                if (mData.StartSeconds != value)
                {
                    mData.StartSeconds = value;
                    notifyPropertyChanged("StartSeconds");
                    notifyPropertyChanged("StartTime");
                }
            }
        }

        public double DurationSeconds
        {
            get => mData.DurationSeconds;
            set
            {
                if (mData.DurationSeconds != value)
                {
                    mData.DurationSeconds = value;
                    notifyPropertyChanged("DurationSeconds");
                }
            }
        }

        public SubtitleViewModel(YouTubeSubtitleData data)
        {
            mData = data;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void notifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
*/