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

        public bool CanExecute(object parameter)
        {
            var viewModel = parameter as MainWindowViewModel;
            return (viewModel != null && viewModel.BookmarkItems != null);
        }

        public void Execute(object parameter)
        {

        }
    }
}
