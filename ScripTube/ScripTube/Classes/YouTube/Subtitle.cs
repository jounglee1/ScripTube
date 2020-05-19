using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScripTube.Classes.YouTube
{
    public class Subtitle
    {
        public string LanguageCode { get; }
        public string LanguageName { get; }
        public bool IsAutomatic { get; } // automatic-speech-recognition

        private ObservableCollection<SubtitleItem> mItems = new ObservableCollection<SubtitleItem>();
        public ObservableCollection<SubtitleItem> Items
        {
            get => mItems;
        }

        public Subtitle(JToken languageCode, JToken languageName, JToken asr)
        {
            LanguageCode = languageCode.ToString();
            LanguageName = languageName.ToString();
            IsAutomatic = (asr != null && asr.ToString() == "asr");
        }

        public void AddItem(SubtitleItem item)
        {
            mItems.Add(item);
        }
    }
}
