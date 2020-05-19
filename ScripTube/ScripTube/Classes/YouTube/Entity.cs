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

namespace ScripTube.Classes.YouTube
{
    public class Entity
    {
        public static int VIDEO_ID_LENGTH = 11;        
        public static string VIDEO_INFO_URL = "https://www.youtube.com/get_video_info?video_id={0}&asv=2";
        private static string PLAYER_RESPONSE_NAME = "player_response=";

        private JObject mMetadata;

        public string ID { get; }

        private ObservableCollection<Subtitle> mSubtitles = new ObservableCollection<Subtitle>();
        public ObservableCollection<Subtitle> Subtitles
        {
            get => mSubtitles;
        }

        public bool IsSubtitleExisted
        {
            get => (mSubtitles.Count > 0);
        }

        public string Title { get; }

        public bool IsAvailable { get; }

        public Entity(string id)
        {
            Debug.Assert(id != null && id.Length == VIDEO_ID_LENGTH);
            ID = id;

            mMetadata = Entity.GetVideoInformationOrNull(ID);
            Debug.Assert(mMetadata != null);            
            
            IsAvailable = (mMetadata["playabilityStatus"]["status"].ToString() == "OK"); // "UNPLAYABLE", "ERROR"
            Debug.Assert(IsAvailable);

            if (IsAvailable)
            {
                Title = mMetadata["videoDetails"]["title"].ToString();
                loadSubtitles();
            }
        }

        private void loadSubtitles()
        {
            if (mMetadata["captions"] == null) // no subtitles
            {
                return;
            }
            foreach (var jToken in mMetadata["captions"]["playerCaptionsTracklistRenderer"]["captionTracks"])
            {
                var subtitle = new Subtitle(jToken["languageCode"], jToken["name"]["simpleText"], jToken["kind"]);
                try
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(jToken["baseUrl"].ToString());
                    foreach (XmlNode xmlNode in xmlDoc.DocumentElement.ChildNodes)
                    {
                        subtitle.AddItem(new SubtitleItem((xmlNode.FirstChild.InnerText), xmlNode.Attributes["start"].Value, xmlNode.Attributes["dur"].Value));
                    }
                    mSubtitles.Add(subtitle);
                }
                catch (System.Xml.XmlException)
                {
                    Debug.Fail("자막 데이터가 없습니다. 자동 생성 자막 찾아야 함");
                }
            }
        }

        public static JObject GetVideoInformationOrNull(string videoId)
        {
            string innerText = System.Web.HttpUtility.UrlDecode(getAllStreamFromURL(string.Format(VIDEO_INFO_URL, videoId)));
            foreach (var attribute in innerText.Split('&'))
            {
                if (attribute.IndexOf(PLAYER_RESPONSE_NAME) == 0)
                {
                    return JObject.Parse(attribute.Substring(PLAYER_RESPONSE_NAME.Length));
                }
            }
            return null;
        }

        private static string getAllStreamFromURL(string url)
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
