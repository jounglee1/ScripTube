using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ScripTube.Classes
{
    public class YouTubeVideoData
    {
        private JObject mInformation;

        public static int VIDEO_ID_LENGTH = 11;        
        private static string VIDEO_INFO_URL = "https://www.youtube.com/get_video_info?video_id={0}&asv=2";        

        private readonly string mId;
        public string ID
        {
            get => mId;
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

        private bool mbSubtitleExisted = false;
        public bool IsSubtitleExisted
        {
            get => mbSubtitleExisted;
        }

        public string Title { get; }

        public bool IsAvailable { get; }

        public YouTubeVideoData(string id)
        {
            Debug.Assert(id != null && id.Length == VIDEO_ID_LENGTH);
            mId = id;

            mInformation = YouTubeVideoData.GetVideoInformationOrNull(mId);
            Debug.Assert(mInformation != null);            
            
            IsAvailable = mInformation["playabilityStatus"]["status"].ToString() == "OK"; // "UNPLAYABLE", "ERROR"
            Debug.Assert(IsAvailable);

            if (IsAvailable)
            {
                Title = mInformation["videoDetails"]["title"].ToString();
                loadSubtitle();
            }
        }

        public static JObject GetVideoInformationOrNull(string videoId)
        {
            string innerText = System.Web.HttpUtility.UrlDecode(get(string.Format(VIDEO_INFO_URL, videoId)));
            foreach (var attribute in innerText.Split('&'))
            {
                if (attribute.IndexOf("player_response=") == 0)
                {
                    return JObject.Parse(attribute.Substring("player_response=".Length));
                }
            }
            return null;
        }

        private void loadSubtitle()
        {
            foreach (var jObject in mInformation["captions"]["playerCaptionsTracklistRenderer"]["captionTracks"])
            {
                string url = jObject["baseUrl"].ToString();
                string langCode = jObject["languageCode"].ToString();
                string langName = jObject["name"]["simpleText"].ToString();
                var subtitles = new ObservableCollection<YouTubeSubtitleData>();
                try
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(url);
                    foreach (XmlNode xmlNode in xmlDoc.DocumentElement.ChildNodes)
                    {
                        subtitles.Add(new YouTubeSubtitleData(unescapeXml(xmlNode.FirstChild.InnerText), xmlNode.Attributes["start"].Value, xmlNode.Attributes["dur"].Value, langName, langCode));
                    }
                    mSubtitleDatas.Add(langCode, subtitles);
                }
                catch (System.Xml.XmlException)
                {
                    Debug.Fail("자막 데이터가 없습니다. 자동 생성 자막 찾아야 함");
                }
            }
        }

        private string unescapeXml(string s)
        {
            StringBuilder sb = new StringBuilder(s);
            sb.Replace("&#34;", "\"");
            sb.Replace("&#38;", "&");
            sb.Replace("&#39;", "'");
            sb.Replace("&#60;", "<");
            sb.Replace("&#62;", ">");
            return sb.ToString();
        }

        private static string get(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
