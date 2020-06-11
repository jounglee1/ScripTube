using System.Drawing;
using System.Windows.Media.Imaging;

namespace ScripTube.Models.Bookmark
{
    public class Thumbnail
    {
        public string ImagePath { get; }

        public Thumbnail(string imagePath)
        {
            ImagePath = imagePath;
        }
    }
}
