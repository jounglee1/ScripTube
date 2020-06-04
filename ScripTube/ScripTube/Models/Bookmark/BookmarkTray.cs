using ScripTube.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScripTube.Models.Bookmark
{
    public class BookmarkTray
    {
        private string mVideoId;
        private List<BookmarkItem> mItems = new List<BookmarkItem>();
        private EBookmarkSortingType mSortingType = EBookmarkSortingType.SECONDS_ASCENDING;

        public BookmarkTray(string videoId)
        {
            mVideoId = videoId;
        }
    }
}
