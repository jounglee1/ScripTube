using ScripTube.Models.Bookmark;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ScripTube.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // https://stackoverflow.com/questions/1035023/firing-a-double-click-event-from-a-wpf-listview-item-using-mvvm
        private void xListViewBookmarkItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var listViewItem = sender as ListViewItem;
            var bookmarkItem = (BookmarkItem)listViewItem.Content;

            if (bookmarkItem != null)
            {
                xVideoPlayer.SeekTo(bookmarkItem.Seconds);
            }
        }
    }
}