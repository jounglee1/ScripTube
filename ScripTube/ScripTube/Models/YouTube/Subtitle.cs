﻿using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;

namespace ScripTube.Models.YouTube
{
    public class Subtitle
    {
        public string LanguageCode { get; }
        public string LanguageName { get; }
        public bool IsAutoGenerated { get; } // automatic-speech-recognition

        private ObservableCollection<SubtitleItem> mItems = new ObservableCollection<SubtitleItem>();
        public ObservableCollection<SubtitleItem> Items
        {
            get { return mItems; }
        }

        private int mLastHighlightedIndex;

        public Subtitle(JToken languageCode, JToken languageName, JToken asr)
        {
            LanguageCode = languageCode.ToString();
            LanguageName = languageName.ToString();
            IsAutoGenerated = (asr != null && asr.ToString() == "asr");
        }

        public void AddItem(SubtitleItem item)
        {
            mItems.Add(item);
        }

        public void SaveSubtitle()
        {
            string dir = "";
            System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog.DefaultExt = "*.txt";
            saveFileDialog.Filter = "텍스트 파일 (*.txt)|*.txt|모든 파일(*.*)|*.*";

            string[] subs = new string[Items.Count];

            for (int i = 0; i < Items.Count; i++)
            {
                subs[i] = Items[i].StartTimeFormat + "  |  " + Items[i].Text;
            }

            if(saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                dir = saveFileDialog.FileName;

                FileStream fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.Write);
                StreamWriter streamWriter = new StreamWriter(fileStream);
                
                for (int i = 0; i < Items.Count; i++)
                {
                    streamWriter.WriteLine(subs[i]);
                }

                streamWriter.Flush();
                streamWriter.Close();
                fileStream.Close();

            }
            
        }

        private int getIndexBySeconds(double currentTime)
        {
            int left = 0;
            int right = mItems.Count - 1;
            int mid = (left + right) / 2;
            while (left < right)
            {
                if (currentTime >= mItems[mid].StartSeconds && currentTime < mItems[mid].StartSeconds + mItems[mid].DurationSeconds)
                {
                    return mid;
                }
                if (currentTime < mItems[mid].StartSeconds)
                {
                    right = mid - 1;
                }
                else if (currentTime > mItems[mid].StartSeconds)
                {
                    left = mid + 1;
                }
                mid = (left + right) / 2;
            }
            return mid;
        }

        public SubtitleItem HighlightSubtitleItem(double currentTime)
        {
            mItems[mLastHighlightedIndex].Visibility = Visibility.Hidden;
            int index = getIndexBySeconds(currentTime);
            mItems[index].Visibility = Visibility.Visible;
            mLastHighlightedIndex = index;
            return mItems[index];
        }

        
    }
}
