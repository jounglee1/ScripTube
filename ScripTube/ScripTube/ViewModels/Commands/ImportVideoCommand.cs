using MaterialDesignThemes.Wpf;
using ScripTube.Enums;
using ScripTube.Models.Dialog;
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

        public async void Execute(object parameter)
        {
            string id = parameter as string;

            if (id == null)
            {
                return;
            }

            var video = new Video(id);

            DialogHost.CloseDialogCommand.Execute(true, null);

            switch (video.Status)
            {
                case EVideoStatus.OK:
                    YouTubeUrlDialogViewModel.Parent.TargetVideo = video;
                    if (!video.IsSubtitleExisted)
                    {
                        await DialogHost.Show(new MessageDialog() { Title = "경고", Message = "자막이 없는 동영상입니다." }, "RootDialog");
                    }
                    YouTubeUrlDialogViewModel.IsDialogOpen = false;
                    break;
                
                case EVideoStatus.UNPLAYABLE:
                    await DialogHost.Show(new MessageDialog() { Title = "에러", Message = "저작권의 이유로 인하여 불러올 수 없는 동영상입니다." }, "RootDialog");
                    YouTubeUrlDialogViewModel.SelectAllText();
                    YouTubeUrlDialogViewModel.Parent.OpenYouTubeUrlDialogCommand.Execute(null);
                    break;
                
                case EVideoStatus.ERROR:
                    await DialogHost.Show(new MessageDialog() { Title = "에러", Message = "유효하지 않은 동영상입니다." }, "RootDialog");
                    YouTubeUrlDialogViewModel.SelectAllText();
                    YouTubeUrlDialogViewModel.Parent.OpenYouTubeUrlDialogCommand.Execute(null);
                    break;

                default:
                    Debug.Assert(false, "Invalid Video Status");
                    break;
            }
        }
    }
}
