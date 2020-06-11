using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows.UI.WebUI;

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
            sb.Replace("&#39;", "\'");
            sb.Replace("&#60;", "<");
            sb.Replace("&#62;", ">");

            sb.Replace("&quot;", "\"");
            sb.Replace("&amp;", "&");
            sb.Replace("&apos;", "\'");
            sb.Replace("&lt;", "<");
            sb.Replace("&gt;", ">");

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
