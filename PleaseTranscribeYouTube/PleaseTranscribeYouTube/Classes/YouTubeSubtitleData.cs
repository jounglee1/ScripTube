using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PleaseTranscribeYouTube.Classes
{
    class YouTubeSubtitleData
    {
        private string mText;
        private double mStartTime;
        private double mDurationTime;

        public YouTubeSubtitleData(string text, string start, string duration)
        {
            mText = text;
            mStartTime = double.Parse(start);
            mDurationTime = double.Parse(duration);
        }
    }
}
