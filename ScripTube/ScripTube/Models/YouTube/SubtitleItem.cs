using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ScripTube.Models.YouTube
{
    public class SubtitleItem : INotifyPropertyChanged
    {
        public string Text { get; }
        public double StartSeconds { get;}
        public double DurationSeconds { get; }
        public bool IsOneHourExcessed { get; }

        public bool mbHighlighted;
        public bool IsHighlighted
        {
            get
            {
                return mbHighlighted;
            }
            set
            {
                mbHighlighted = value;
                notifyPropertyChanged(nameof(IsHighlighted));
            }
        }

        public string StartTimeFormat
        {
            get
            {
                int total = Convert.ToInt32(StartSeconds);
                int min = total / 60;
                int sec = total % 60;
                if (IsOneHourExcessed)
                {
                    int hour = min / 60;
                    min %= 60;
                    return $"{hour.ToString("D2")}:{min.ToString("D2")}:{sec.ToString("D2")}";
                }
                return $"{min.ToString("D2")}:{sec.ToString("D2")}";
            }
        }

        public string StartTimeFormatSRT
        {
            get
            {
                double total = StartSeconds;
                int min = (Convert.ToInt32(total)) / 60;
                double sec = total % (Convert.ToDouble(60));
                if (IsOneHourExcessed)
                {
                    int hour = min / 60;
                    min %= 60;
                    return $"{hour.ToString("D2")}:{min.ToString("D2")}:{sec.ToString("D2")}";
                }
                else
                {
                    return $"{min.ToString("D2")}:{sec.ToString("D2")}";
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public SubtitleItem(string text, string start, string duration, bool bOneHourExcessed)
        {
            Text = unescapeXML(text);
            StartSeconds = double.Parse(start);
            DurationSeconds = double.Parse(duration);
            IsOneHourExcessed = bOneHourExcessed;
        }

        private static string unescapeXML(string str)
        {
            StringBuilder sb = new StringBuilder(str);
            sb.Replace("&#34;", "\"");
            sb.Replace("&#38;", "&");
            sb.Replace("&#39;", "'");
            sb.Replace("&#60;", "<");
            sb.Replace("&#62;", ">");
            return sb.ToString();
        }

        private void notifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
