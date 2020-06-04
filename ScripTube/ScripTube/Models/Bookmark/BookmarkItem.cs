using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScripTube.Models.Bookmark
{
    public class BookmarkItem
    {
        private double mSeconds;
        private string mMemo;
        private string mImagePath;
        private Bitmap mBitmap;
        private DateTime mCreatedDateTime = DateTime.Now;

        public BookmarkItem(double seconds, string imagePath)
            : this(seconds, imagePath, string.Empty)
        {
        }

        private BookmarkItem(double seconds, string imagePath, string memo)
        {
            mSeconds = seconds;
            mMemo = memo;
            mImagePath = imagePath;
        }
    }
}
