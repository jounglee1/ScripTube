using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ScripTube.Views.Controls
{
    class SubtitleListView : ListView
    {
        public int HighlightedIndex
        {
            get { return (int)GetValue(HighlightedIndexProperty); }
            set { SetValue(HighlightedIndexProperty, value); }
        }

        public bool ScrollLock
        {
            get { return (bool)GetValue(AutoScrollProperty); }
            set { SetValue(AutoScrollProperty, value); }
        }

        public static readonly DependencyProperty HighlightedIndexProperty =
            DependencyProperty.Register("HighlightedIndex", typeof(int), typeof(SubtitleListView), new PropertyMetadata(0, notifyHighlightIndexChanged));

        public static readonly DependencyProperty AutoScrollProperty =
            DependencyProperty.Register("ScrollLock", typeof(bool), typeof(SubtitleListView), new PropertyMetadata(false));

        private static void notifyHighlightIndexChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var subtitleListView = sender as SubtitleListView;

            if (!subtitleListView.ScrollLock)
            {
                var items = new List<object>();
                foreach (var item in subtitleListView.SelectedItems)
                {
                    items.Add(item);
                }
                subtitleListView.SelectedIndex = subtitleListView.HighlightedIndex;
                subtitleListView.ScrollIntoView(subtitleListView.SelectedItem);
                subtitleListView.SetSelectedItems(items);
            }
        }
    }
}
