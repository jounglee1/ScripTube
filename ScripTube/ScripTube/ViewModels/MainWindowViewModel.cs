﻿using ScripTube.Models.Bookmark;
using ScripTube.Models.YouTube;
using ScripTube.ViewModels.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ScripTube.ViewModels
{
    class MainWindowViewModel : BaseViewModel
    {
        public YouTubeUrlDialogViewModel YouTubeUrlDialogViewModel { get; }

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
                    BookmarkItems = mTargetVideo.BookmarkTray.Items;
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

        private ObservableCollection<BookmarkItem> mBookmarkItems;
        public ObservableCollection<BookmarkItem> BookmarkItems
        {
            get
            {
                return mTargetVideo?.BookmarkTray.Items;
            }
            set
            {
                mBookmarkItems = value;
                notifyPropertyChanged(nameof(BookmarkItems));
            }
        }

        private BookmarkItem mSelectedBookmarkItem;
        public BookmarkItem SelectedBookmarkItem
        {
            get
            {
                return mSelectedBookmarkItem;
            }
            set
            {
                mSelectedBookmarkItem = value;
                notifyPropertyChanged(nameof(SelectedBookmarkItem));
            }
        }

        private Thumbnail mTargetThumbnail;
        public Thumbnail TargetThumbnail
        {
            get
            {
                return mTargetThumbnail;
            }
            set
            {
                mTargetThumbnail = value;
                notifyPropertyChanged(nameof(TargetThumbnail));
            }
        }



        private int mLastHighlightedIndex;
        public int LastHighlightedIndex
        {
            get
            {
                return mLastHighlightedIndex;
            }
            set
            {
                if (mLastHighlightedIndex != value)
                {
                    mLastHighlightedIndex = value;
                    notifyPropertyChanged(nameof(LastHighlightedIndex));
                }
            }
        }

        public string WindowTitle
        {
            get
            {
                return (mTargetVideo == null ? string.Empty : mTargetVideo.Title);
            }
        }

        public ICommand OpenYouTubeUrlDialogCommand { get; }
        public ICommand PlayerSeekToCommand { get; }
        public ICommand SaveScriptCommand { get; }
        public ICommand AddBookmarkCommand { get; }
        public ICommand RemoveBookmarkCommand { get; }
        public ICommand ExecutePapagoCommand { get; }
        public ICommand CopySubtitleTextToClipboardCommand { get; }
        public ICommand CopyTimeAndSubtitleTextToClipboardCommand { get; }
        #endregion

        public MainWindowViewModel()
        {
            YouTubeUrlDialogViewModel = new YouTubeUrlDialogViewModel(this);
            OpenYouTubeUrlDialogCommand = new OpenYouTubeUrlDialogCommand(this);
            PlayerSeekToCommand = new PlayerSeekToCommand(this);
            SaveScriptCommand = new SaveScriptCommand();
            AddBookmarkCommand = new AddBookmarkCommand(this);
            RemoveBookmarkCommand = new RemoveBookmarkCommand(this);
            ExecutePapagoCommand = new ExecutePapagoCommand();
            CopySubtitleTextToClipboardCommand = new CopySubtitleTextToClipboardCommand();
            CopyTimeAndSubtitleTextToClipboardCommand = new CopyTimeAndSubtitleTextToClipboardCommand();
        }

        private void select(double currentTime)
        {
            int index = SelectedSubtitle.GetIndexBySeconds(currentTime);
            var items = SelectedSubtitle.Items;
            items[mLastHighlightedIndex].IsHighlighted = false;
            items[index].IsHighlighted = true;
            LastHighlightedIndex = index;
        }
    }
}
