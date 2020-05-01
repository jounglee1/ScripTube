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
            string unxml = s;
            if (!string.IsNullOrEmpty(unxml))
            {
                // replace entities with literal values
                unxml = unxml.Replace("&#39;", "'");
                unxml = unxml.Replace("&#34;", "\"");
                unxml = unxml.Replace("&#62;", ">");
                unxml = unxml.Replace("&#60;", "<");
                unxml = unxml.Replace("&#38;", "&");
            }
            return unxml;
        }

        private bool loadSubtitles(string language)
        {
            XmlDocument xmlDoc = new XmlDocument();
            mSubtitleDatas.Add(language, new ObservableCollection<YouTubeSubtitleData>());
            try
            {
                xmlDoc.Load("https://www.youtube.com/api/timedtext?lang=" + language + "&v=" + mID);
                foreach (XmlNode xmlNode in xmlDoc.DocumentElement.ChildNodes)
                {
                    var s0 = unescapeXml(xmlNode.FirstChild.InnerText);
                    var start = xmlNode.Attributes["start"].Value;
                    var dur = xmlNode.Attributes["dur"].Value;
                    mSubtitleDatas[language].Add(new YouTubeSubtitleData(s0, start, dur));
                }
                return true;
            }
            catch (System.Xml.XmlException)
            {
                mSubtitleDatas.Remove(language);
                Debug.Fail("자막 데이터가 없습니다. 자동 생성 자막 찾아야 함");
                return false;
            }
        }
    }
}
