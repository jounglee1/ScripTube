using MaterialDesignThemes.Wpf;
using ScripTube.Models.Bookmark;
using ScripTube.Models.Dialog;
using ScripTube.Models.YouTube;
using System;
using System.Collections;
using System.IO;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace ScripTube.ViewModels.Commands
{
    class AddBookmarkCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public MainWindowViewModel MainWindowViewModel { get; }

        public AddBookmarkCommand(MainWindowViewModel mainWindowViewModel)
        {
            MainWindowViewModel = mainWindowViewModel;
        }

        public bool CanExecute(object parameter)
        {
            var values = (object[])parameter;
            return values != null && values[0] != null;
        }

        public async void Execute(object parameter)
        {
            var values = (object[])parameter;

            if (values == null)
            {
                return;
            }

            var video = values[0] as Video;
            
            if (video == null)
            {
                return;
            }

            var currentTime = (double)values[1];
            var tray = video.BookmarkTray;

            string filename = Path.Combine(Directory.GetCurrentDirectory(), "Cache", video.ID + currentTime.GetHashCode() + ".png");

            var msg = new AddBookmarkDialog() { MemoText = "Memo" + tray.Items.Count.ToString() };
            if ((bool)await DialogHost.Show(msg, "BookmarkDialog"))
            {
                MainWindowViewModel.TargetThumbnail = new Thumbnail(filename);
                tray.AddItem(new BookmarkItem(msg.MemoText, currentTime, filename));
            }
        }
    }
}
