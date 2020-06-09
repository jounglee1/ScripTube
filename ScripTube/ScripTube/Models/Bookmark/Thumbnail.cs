using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ScripTube.Models.Bookmark
{
    public class Thumbnail
    {
        public string ImagePath { get; }
        public Bitmap Bitmap { get; set; }
        public BitmapSource BitmapSource { get; set; }

        public Thumbnail(string imagePath)
        {
            ImagePath = imagePath;
        }
    }
}
