﻿using ScripTube.Utils;
using ScripTube.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public ICommand ImportVideoCommand { get; set; }

        #endregion

        public MainWindowViewModel Parent { get; }

        public YouTubeUrlDialogViewModel(MainWindowViewModel mainWindowViewModel)
        {
            ImportVideoCommand = new ImportVideoCommand(this);
            Parent = mainWindowViewModel;
        }

        public void SelectAllText()
        {
            mbUrlTextAllSelected = false;
            IsUrlTextAllSelected = true;
        }
    }
}