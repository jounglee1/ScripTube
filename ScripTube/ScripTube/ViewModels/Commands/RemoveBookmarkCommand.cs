using ScripTube.Models.Bookmark;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public void Execute(object parameter)
        {
            var item = parameter as BookmarkItem;

            if (item == null)
            {
                return;
            }

            MainWindowViewModel.TargetVideo.BookmarkTray.RemoveItem(item);
        }
    }
}
