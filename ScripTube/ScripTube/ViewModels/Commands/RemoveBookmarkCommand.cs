using MaterialDesignThemes.Wpf;
using ScripTube.Models.Bookmark;
using ScripTube.Models.Dialog;
using ScripTube.Utils;
using System;
using System.Windows.Input;

namespace ScripTube.ViewModels.Commands
{
    class RemoveBookmarkCommand : ICommand
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

        public RemoveBookmarkCommand(MainWindowViewModel mainWindowViewModel)
        {
            MainWindowViewModel = mainWindowViewModel;
        }

        public bool CanExecute(object parameter)
        {
            var item = parameter as BookmarkItem;

            return item != null;
        }

        public async void Execute(object parameter)
        {
            var item = parameter as BookmarkItem;

            if (item == null)
            {
                return;
            }

            var msg = new RemoveBookmarkDialog();
            
            msg.Title = item.Memo;
            if (msg.Title.Length > 10)
            {
                msg.Title = item.Memo.Substring(0, 10) + "...";
            }
            msg.Message = string.Format("시간: {0}\n\n이 북마크를 삭제합니까?", TimeFormatUtil.GetHHMMSSOrMMSSPrecision(item.Seconds, true));

            if ((bool)await DialogHost.Show(msg, "BookmarkDialog"))
            {
                MainWindowViewModel.TargetVideo.BookmarkTray.RemoveItem(item);
            }
        }
    }
}
