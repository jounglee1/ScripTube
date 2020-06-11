using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;

namespace ScripTube.Models.Bookmark
{
    public class BookmarkTray : INotifyPropertyChanged
    {
        public string VideoId { get; }

        private ObservableCollection<BookmarkItem> mItems = new ObservableCollection<BookmarkItem>();
        public ObservableCollection<BookmarkItem> Items
        {
            get
            {
                return mItems;
            }
            set
            {
                mItems = value;
                notifyPropertyChanged(nameof(Items));
            }
        }

        public BookmarkTray(string videoId)
        {
            VideoId = videoId;
            createCacheFolder();
            loadAsJson();
        }

        public static readonly string CACHE_FOLDER_NAME = "Cache";

        public event PropertyChangedEventHandler PropertyChanged;

        public void AddItem(BookmarkItem item)
        {
            mItems.Add(item);
            saveAsJson();
        }

        public bool RemoveItem(BookmarkItem item)
        {
            bool bSuccess = mItems.Remove(item);
            bool bDuplicated = false;
            foreach (var other in mItems)
            {
                if (Math.Abs(item.Seconds - other.Seconds) <= double.Epsilon)
                {
                    bDuplicated = true;
                    break;
                }
            }
            if (!bDuplicated && File.Exists(item.ImagePath))
            {
                File.Delete(item.ImagePath);
            }
            saveAsJson();
            return bSuccess;
        }

        private void loadAsJson()
        {
            string path = generateJsonPath();

            if (!File.Exists(path))
            {
                return;
            }

            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                Items = JsonConvert.DeserializeObject<ObservableCollection<BookmarkItem>>(json);
            }
        }

        private void saveAsJson()
        {
            string json = JsonConvert.SerializeObject(Items, Formatting.Indented);
            System.IO.File.WriteAllText(generateJsonPath(), json);
        }

        private string generateJsonPath()
        {
            return string.Format("{0}.{1}", Path.Combine(CACHE_FOLDER_NAME, VideoId), "json");
        }

        private void createCacheFolder()
        {
            System.IO.Directory.CreateDirectory(BookmarkTray.CACHE_FOLDER_NAME);
        }

        private void notifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
