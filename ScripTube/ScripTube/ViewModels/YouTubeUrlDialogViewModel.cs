using ScripTube.Utils;
using ScripTube.ViewModels.Commands;
using System.Windows;
using System.Windows.Input;

namespace ScripTube.ViewModels
{
    class YouTubeUrlDialogViewModel : BaseViewModel
    {
        #region DialogHost Properties
        private bool mbDialogOpen;
        public bool IsDialogOpen
        {
            get
            {
                return mbDialogOpen;
            }
            set
            {
                mbDialogOpen = value;
                notifyPropertyChanged(nameof(IsDialogOpen));
                if (mbDialogOpen)
                {
                    TextUrl = Clipboard.GetText().Trim(); // https://www.youtube.com/watch?v=qC5KtatMcUw
                }
            }
        }

        private string mTextUrl;
        public string TextUrl
        {
            get
            {
                return mTextUrl;
            }
            set
            {
                mTextUrl = value;
                notifyPropertyChanged(nameof(TextUrl));
                notifyPropertyChanged(nameof(IsValidUrl));
            }
        }

        public bool IsValidUrl
        {
            get
            {
                return YouTubeUtil.GetVideoIdByUrl(mTextUrl) != string.Empty;
            }
        }

        private bool mbUrlTextAllSelected;
        public bool IsUrlTextAllSelected
        {
            get { return mbUrlTextAllSelected; }
            set
            {
                mbUrlTextAllSelected = value;
                notifyPropertyChanged(nameof(IsUrlTextAllSelected));
            }
        }

        public string Title { get; }
        public string UnvalidURLText { get; }

        public ICommand ImportVideoCommand { get; set; }
        #endregion

        public MainWindowViewModel Parent { get; }

        public YouTubeUrlDialogViewModel(MainWindowViewModel mainWindowViewModel)
        {
            ImportVideoCommand = new ImportVideoCommand(this);
            Parent = mainWindowViewModel;

            Title = "유튜브 URL";
            UnvalidURLText = "올바른 URL이 아닙니다.";
        }

        public void SelectAllText()
        {
            mbUrlTextAllSelected = false;
            IsUrlTextAllSelected = true;
        }
    }
}
