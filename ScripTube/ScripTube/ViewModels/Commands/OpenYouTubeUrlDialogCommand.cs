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

        public MainWindowViewModel MainWindowViewModel { get; }

        public OpenYouTubeUrlDialogCommand(MainWindowViewModel mainWindowViewModel)
        {
            MainWindowViewModel = mainWindowViewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            MainWindowViewModel.YouTubeUrlDialogViewModel.IsDialogOpen = true;
            if ((bool)await DialogHost.Show(MainWindowViewModel.YouTubeUrlDialogViewModel, "RootDialog") == false)
            {
                MainWindowViewModel.YouTubeUrlDialogViewModel.IsDialogOpen = false;
            }
        }
    }
}
