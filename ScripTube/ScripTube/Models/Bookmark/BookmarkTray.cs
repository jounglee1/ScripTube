using ScripTube.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScripTube.Models.Bookmark
{
    public class BookmarkTray
    {
        public string VideoId { get; }
        public List<BookmarkItem> Items { get; set; }
        public EBookmarkSortingType SortingType { get; set; }

        public BookmarkTray(string videoId)
        {
            VideoId = videoId;
        }

        public static readonly string CACHE_FOLDER_NAME = "Cache";
    }
}
