using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ScripTube.Models.Bookmark
{
    public class BookmarkItem
    {
        public string Memo { get; set; }
        public double Seconds { get; }
        public string ImagePath { get; }
        [JsonIgnore]
        public ImageSource ImageSource { get; set; }
        public DateTime CreatedDateTime { get; }

        public BookmarkItem(string memo, double seconds, string imagePath)
        {
            Memo = memo;
            Seconds = seconds;
            ImagePath = imagePath;
            CreatedDateTime = DateTime.Now;

            var bitmap = new BitmapImage();

            if (File.Exists(imagePath))
            {
                var stream = File.OpenRead(imagePath);

                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = stream;
                bitmap.EndInit();

                stream.Close();
                stream.Dispose();
            }

            ImageSource = bitmap;
        }
    }
}
