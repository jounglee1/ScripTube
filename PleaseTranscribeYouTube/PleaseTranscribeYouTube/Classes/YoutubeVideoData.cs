using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PleaseTranscribeYouTube.Classes
{
    public class YouTubeVideoData
    {
        private string mID;
        public string ID
        {
            get => mID;
        }

        private string[] mLanguages = null;
        public string[] Languages
        {
            get => mLanguages;
        }

        private Dictionary<string, ObservableCollection<YouTubeSubtitleData>> mSubtitleDatas = new Dictionary<string, ObservableCollection<YouTubeSubtitleData>>();
        public Dictionary<string, ObservableCollection<YouTubeSubtitleData>> SubtitleDatas
        {
            get => mSubtitleDatas;
        }

        private bool mbSubtitleExisted;
        public bool IsSubtitleExisted
        {
            get => mbSubtitleExisted;
        }

        public YouTubeVideoData(string id)
        {
            mID = id;
            mbSubtitleExisted = loadSubtitles("en");
        }

        private static string unescapeXml(string s)
        {
            StringBuilder sb = new StringBuilder(s);
            if (sb.Length > 0)
            {
                sb.Replace("&#39;", "'");
                sb.Replace("&#34;", "\"");
                sb.Replace("&#62;", ">");
                sb.Replace("&#60;", "<");
                sb.Replace("&#38;", "&");
            }
            return sb.ToString();
        }

        private bool loadSubtitles(string language)
        {
            XmlDocument xmlDoc = new XmlDocument();
            var youtubeSubtitleData = new ObservableCollection<YouTubeSubtitleData>();
            try
            {
                xmlDoc.Load($"https://www.youtube.com/api/timedtext?lang={language}&v={mID}");
                foreach (XmlNode xmlNode in xmlDoc.DocumentElement.ChildNodes)
                {
                    youtubeSubtitleData.Add(new YouTubeSubtitleData(unescapeXml(xmlNode.FirstChild.InnerText), xmlNode.Attributes["start"].Value, xmlNode.Attributes["dur"].Value));
                }
                mSubtitleDatas.Add(language, youtubeSubtitleData);
                return true;
            }
            catch (System.Xml.XmlException)
            {
                Debug.Fail("자막 데이터가 없습니다. 자동 생성 자막 찾아야 함");
            }
            return false;
        }
    }
}
