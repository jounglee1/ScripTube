using MaterialDesignThemes.Wpf;
using ScripTube.Enums;
using ScripTube.Models.Bookmark;
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
            string id = parameter as string;
            return (id != string.Empty);
        }

        public void Execute(object parameter)
        {
            string id = parameter as string;
            if (id != null)
            {
                var video = new Video(id);
                if (video.Status == EVideoStatus.OK)
                {
                    YouTubeUrlDialogViewModel.Parent.TargetVideo = video;
                    if (!video.IsSubtitleExisted)
                    {
                        MessageBox.Show("자막이 없는 동영상입니다.", "경고", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    DialogHost.CloseDialogCommand.Execute(true, null);
                }
                else if (video.Status == EVideoStatus.UNPLAYABLE)
                {
                    MessageBox.Show("저작권의 이유로 인하여 불러올 수 없는 동영상입니다.", "에러", MessageBoxButton.OK, MessageBoxImage.Error);
                    YouTubeUrlDialogViewModel.SelectAllText();
                }
                else if (video.Status == EVideoStatus.ERROR)
                {
                    MessageBox.Show("유효하지 않은 동영상입니다.", "에러", MessageBoxButton.OK, MessageBoxImage.Error);
                    YouTubeUrlDialogViewModel.SelectAllText();
                }
            }
        }
    }
}
