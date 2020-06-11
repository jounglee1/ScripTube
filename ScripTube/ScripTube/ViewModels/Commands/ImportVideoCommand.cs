using MaterialDesignThemes.Wpf;
using ScripTube.Enums;
using ScripTube.Models.YouTube;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace ScripTube.ViewModels.Commands
{
    class ImportVideoCommand : ICommand
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
        
        public YouTubeUrlDialogViewModel YouTubeUrlDialogViewModel { get; }

        public ImportVideoCommand(YouTubeUrlDialogViewModel importUrlDialogViewModel)
        {
            YouTubeUrlDialogViewModel = importUrlDialogViewModel;
        }

        public bool CanExecute(object parameter)
        {
            return parameter as string != string.Empty;
        }

        public void Execute(object parameter)
        {
            string id = parameter as string;

            if (id == null)
            {
                return;
            }

            var video = new Video(id);

            switch (video.Status)
            {
                case EVideoStatus.OK:
                    YouTubeUrlDialogViewModel.Parent.TargetVideo = video;
                    if (!video.IsSubtitleExisted)
                    {
                        MessageBox.Show("자막이 없는 동영상입니다.", "경고", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    DialogHost.CloseDialogCommand.Execute(true, null);
                    break;
                
                case EVideoStatus.UNPLAYABLE:
                    MessageBox.Show("저작권의 이유로 인하여 불러올 수 없는 동영상입니다.", "에러", MessageBoxButton.OK, MessageBoxImage.Error);
                    YouTubeUrlDialogViewModel.SelectAllText();
                    break;
                
                case EVideoStatus.ERROR:
                    MessageBox.Show("유효하지 않은 동영상입니다.", "에러", MessageBoxButton.OK, MessageBoxImage.Error);
                    YouTubeUrlDialogViewModel.SelectAllText();
                    break;
                    
                default:
                    Debug.Assert(false, "Invalid Video Status");
                    break;
            }
        }
    }
}
