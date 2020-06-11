using MaterialDesignThemes.Wpf;
using System;
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
            ViewModel.YouTubeUrlDialogViewModel.IsDialogOpen = true;
            if ((bool)await DialogHost.Show(ViewModel.YouTubeUrlDialogViewModel, "RootDialog") == false)
            {
                ViewModel.YouTubeUrlDialogViewModel.IsDialogOpen = false;
            }
        }
    }
}
