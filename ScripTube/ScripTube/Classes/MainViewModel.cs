using ScripTube.Classes.YouTube;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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

        private Entity mEntity;
        public Entity Entity
        {
            get
            {
                return mEntity;
            }
            set
            {
                if (mEntity != value)
                {
                    mEntity = value;
                    notifyPropertyChanged("Entity");
                    notifyPropertyChanged("Subtitles");
                    notifyPropertyChanged("SubtitleItems");
                    notifyPropertyChanged("Languages");
                }
            }
        }

        public ObservableCollection<Subtitle> Subtitles
        {
            get
            {
                if (mEntity != null && mEntity.IsSubtitleExisted)
                {
                    return mEntity.Subtitles;
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

        public List<String> Languages
        {
            get
            {
                if (mEntity == null)
                {
                    return null;
                }
                var list = new List<String>();
                for (int i = 0; i <  mEntity.Subtitles.Count; ++i)
                {
                    list.Add(mEntity.Subtitles[i].LanguageName);
                }
                return list;
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
