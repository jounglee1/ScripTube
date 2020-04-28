using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PleaseTranscribeYouTube.Classes
{
    class YouTubeVideoData
    {
        private string mID;
        private string mLanguage;
        private List<YouTubeSubtitleData> mSubtitleDatas = new List<YouTubeSubtitleData>();

        public YouTubeVideoData(string id)
        {
            mID = id;
        }
    }
}
