using MaterialDesignThemes.Wpf;
using ScripTube.Models.Bookmark;
using ScripTube.Models.Dialog;
using System;
using System.IO;
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

        public bool CanExecute(object parameter)
        {
            var viewModel = parameter as MainWindowViewModel;
            return (viewModel != null && viewModel.BookmarkItems != null);
        }

        public async void Execute(object parameter)
        {
            var viewModel = parameter as MainWindowViewModel;
            if (viewModel != null)
            {
                var tray = viewModel.TargetVideo.BookmarkTray;
                if (tray != null)
                {
                    double time = viewModel.CurrentVideoTime;
                    string filename = Path.Combine(Directory.GetCurrentDirectory(), "Cache", viewModel.TargetVideo.ID + time.GetHashCode() + ".png");

                    var msg = new AddBookmarkDialog() { MemoText = "Memo" + tray.Items.Count.ToString() };
                    if ((bool)await DialogHost.Show(msg, "BookmarkDialog"))
                    {
                        viewModel.TargetThumbnail = new Thumbnail(filename);
                        tray.AddItem(new BookmarkItem(msg.MemoText, time, filename));
                    }
                }
            }
        }
    }
}
