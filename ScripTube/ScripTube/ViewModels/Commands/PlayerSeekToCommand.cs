using ScripTube.Models.YouTube;
using System;
using System.Windows.Input;

namespace ScripTube.ViewModels.Commands
{
    class PlayerSeekToCommand : ICommand
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

        public PlayerSeekToCommand(MainWindowViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var subtitleItem = parameter as SubtitleItem;

            if (subtitleItem == null)
            {
                return;
            }

            ViewModel.SetVideoTime = subtitleItem.StartSeconds + 0.001;
            ViewModel.SetVideoTime = subtitleItem.StartSeconds;
        }
    }
}
