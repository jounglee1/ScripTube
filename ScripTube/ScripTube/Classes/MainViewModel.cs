using ScripTube.Classes.YouTube;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ScripTube.Classes
{
    class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {

        }

        private Subtitle mSelectedSubtitle;
        public Subtitle SelectedSubtitle
        {
            get
            {
                return mSelectedSubtitle;
            }
            set
            {
                if (mSelectedSubtitle != value)
                {
                    mSelectedSubtitle = value;
                    notifyPropertyChanged("SelectedSubtitle");
                    notifyPropertyChanged("SubtitleItems");
                }
            }
        }

        private Video mVideo;
        public Video Video
        {
            get
            {
                return mVideo;
            }
            set
            {
                if (mVideo != value)
                {
                    mVideo = value;
                    notifyPropertyChanged("Video");
                    notifyPropertyChanged("Subtitles");
                    notifyPropertyChanged("Languages");
                    if (mVideo.IsSubtitleExisted)
                    {
                        SelectedSubtitle = Subtitles[0];
                    }
                }
            }
        }

        public ObservableCollection<Subtitle> Subtitles
        {
            get
            {
                if (mVideo != null && mVideo.IsSubtitleExisted)
                {
                    return mVideo.Subtitles;
                }
                return null;
            }
        }

        public ObservableCollection<SubtitleItem> SubtitleItems
        {
            get
            {
                if (SelectedSubtitle != null)
                {
                    return SelectedSubtitle.Items;
                }
                return null;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void notifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
