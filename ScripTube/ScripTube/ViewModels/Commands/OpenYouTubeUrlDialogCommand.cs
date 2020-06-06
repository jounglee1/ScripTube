using MaterialDesignThemes.Wpf;
using ScripTube.Enums;
using ScripTube.Models.Bookmark;
using ScripTube.Models.DialogBox;
using ScripTube.Models.YouTube;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Input;

namespace ScripTube.ViewModels.Commands
{
    class OpenYouTubeUrlDialogCommand : ICommand
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

        public MainWindowViewModel ViewModel { get; }

        public OpenYouTubeUrlDialogCommand(MainWindowViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            RaiseCanExecuteChanged();
            ViewModel.YouTubeUrlDialogViewModel.IsDialogOpen = true;
            await DialogHost.Show(ViewModel.YouTubeUrlDialogViewModel, "RootDialog");
            ViewModel.YouTubeUrlDialogViewModel.IsDialogOpen = false;
            RaiseCanExecuteChanged();
        }

        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
