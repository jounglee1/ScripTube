using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScripTube.Classes
{
    public class YouTubeSubtitleData
    {
        public string Text { get; }
        public double StartSeconds { get;}
        public double DurationSeconds { get; }
        public string LanguageCode { get; }
        public string LanguageName { get; }

        public YouTubeSubtitleData(string text, string start, string duration, string languageCode, string languageName)
        {
            Text = text;
            StartSeconds = double.Parse(start);
            DurationSeconds = double.Parse(duration);
            LanguageCode = languageCode;
            LanguageName = languageName;
        }

        public string StartTime
        {
            get
            {
                int total = Convert.ToInt32(StartSeconds);
                int min = total / 60;
                int sec = total % 60;
                return $"{min.ToString("D2")}:{sec.ToString("D2")}";
            }
        }
    }
}
