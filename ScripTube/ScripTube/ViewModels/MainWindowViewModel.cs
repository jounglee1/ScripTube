using GalaSoft.MvvmLight.CommandWpf;
using ScripTube.Models.YouTube;
using ScripTube.Utils;
using ScripTube.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ScripTube.ViewModels
{
    class MainWindowViewModel : BaseViewModel
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
                    TextUrl = "https://www.youtube.com/watch?v=qC5KtatMcUw"; //Clipboard.GetText().Trim();
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
        #endregion

        #region MainWindow Properties


        private Video mTargetVideo;
        public Video TargetVideo
        {
            get
            {
                return mTargetVideo;
            }
            set
            {
                if (mTargetVideo != value)
                {
                    mTargetVideo = value;
                    notifyPropertyChanged(nameof(TargetVideo));
                    notifyPropertyChanged(nameof(Subtitles));
                    notifyPropertyChanged(nameof(WindowTitle));
                    if (mTargetVideo.IsSubtitleExisted)
                    {
                        SelectedSubtitle = Subtitles[0];
                    }
                }
            }
        }

        private double mCurrentVideoTime;
        public double CurrentVideoTime
        {
            get
            {
                return mCurrentVideoTime;
            }
            set
            {
                mCurrentVideoTime = value;
                notifyPropertyChanged(nameof(CurrentVideoTime));
                if (mSelectedSubtitle != null)
                {
                    select(mCurrentVideoTime);
                }
            }
        }

        private double mSetVideoTime;
        public double SetVideoTime
        {
            get
            {
                return mSetVideoTime;
            }
            set
            {
                mSetVideoTime = value;
                notifyPropertyChanged(nameof(SetVideoTime));
            }
        }

        private Subtitle mSelectedSubtitle;
        public Subtitle SelectedSubtitle
        {
            get
            {
                return mSelectedSubtitle;
            }
            set
            {
                if (mSelectedSubtitle != value)
                {
                    mSelectedSubtitle = value;
                    notifyPropertyChanged(nameof(SelectedSubtitle));
                    notifyPropertyChanged(nameof(SubtitleItems));
                }
            }
        }

        public ObservableCollection<Subtitle> Subtitles
        {
            get
            {
                if (mTargetVideo != null && mTargetVideo.IsSubtitleExisted)
                {
                    return mTargetVideo.Subtitles;
                }
                return null;
            }
        }

        public ObservableCollection<SubtitleItem> SubtitleItems
        {
            get
            {
                if (mSelectedSubtitle != null)
                {
                    return mSelectedSubtitle.Items;
                }
                return null;
            }
        }

        private bool mbAutoScroll;
        public bool IsAutoScroll
        {
            get
            {
                return mbAutoScroll;
            }
            set
            {
                mbAutoScroll = value;
            }
        }

        public string WindowTitle
        {
            get
            {
                return (mTargetVideo == null ? string.Empty : mTargetVideo.Title);
            }
        }

        public ICommand ShowDialogCommand { get; }
        public ICommand ImportVideoCommand { get; set; }
        public ICommand PlayerSeekToCommand { get; }
        public ICommand SaveScriptAsTXTCommand { get; }
        public ICommand SaveScriptAsSMICommand { get; }
        public ICommand SaveScriptAsSRTCommand { get; }
        public ICommand ExecutePapagoCommand { get; }
        public ICommand CopySubtitleTextToClipboardCommand { get; }
        #endregion

        private int mLastHighlightedIndex;

        public MainWindowViewModel()
        {
            ShowDialogCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(showHostDialog);
            ImportVideoCommand = new ImportVideoCommand(this);
            PlayerSeekToCommand = new PlayerSeekToCommand(this);
            SaveScriptAsTXTCommand = new SaveScriptAsTXTCommand();
            SaveScriptAsSMICommand = new SaveScriptAsSMICommand();
            SaveScriptAsSRTCommand = new SaveScriptAsSRTCommand();
            ExecutePapagoCommand = new ExecutePapagoCommand();
            CopySubtitleTextToClipboardCommand = new CopySubtitleTextToClipboardCommand();
        }

        public void SelectAllText()
        {
            mbUrlTextAllSelected = false;
            IsUrlTextAllSelected = true;
        }

        private void showHostDialog()
        {
            IsDialogOpen = true;
        }

        private void select(double currentTime)
        {
            int index = SelectedSubtitle.GetIndexBySeconds(currentTime);
            var items = SelectedSubtitle.Items;
            items[mLastHighlightedIndex].IsHighlighted = false;
            items[index].IsHighlighted = true;
            mLastHighlightedIndex = index;
        }
    }
}
