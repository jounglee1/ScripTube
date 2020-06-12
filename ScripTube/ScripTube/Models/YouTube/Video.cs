using Newtonsoft.Json.Linq;
using ScripTube.Enums;
using ScripTube.Models.Bookmark;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Xml;

namespace ScripTube.Models.YouTube
{
    public class Video
    {
        public string ID { get; }
        public string Title { get; }
        public EVideoStatus Status { get; }
        public int LengthSeconds { get; }

        public bool IsSubtitleExisted
        {
            get
            {
                return mSubtitles.Count > 0;
            }
        }

        private ObservableCollection<Subtitle> mSubtitles = new ObservableCollection<Subtitle>();
        public ObservableCollection<Subtitle> Subtitles
        {
            get
            {
                return mSubtitles;
            }
        }

        public BookmarkTray BookmarkTray { get; }

        public Video(string id)
        {
            ID = id;
            mMetadata = getVideoInformationOrNull(ID);

            Debug.Assert(mMetadata != null);

            Status = loadStatus(mMetadata["playabilityStatus"]["status"]);

            if (Status == EVideoStatus.OK)
            {
                Title = mMetadata["videoDetails"]["title"].ToString();
                LengthSeconds = Convert.ToInt32(mMetadata["videoDetails"]["lengthSeconds"]);
                loadSubtitles();
                BookmarkTray = new BookmarkTray(id);
            }
        }

        private static string METADATA_URL = "https://www.youtube.com/get_video_info?video_id={0}&asv=2";
        private static string PLAYER_RESPONSE_NAME = "player_response=";

        private JObject mMetadata;

        private EVideoStatus loadStatus(JToken jToken)
        {
            string str = jToken.ToString();
            if (str == "OK")
            {
                return EVideoStatus.OK;
            }
            else if (str == "UNPLAYABLE")
            {
                return EVideoStatus.UNPLAYABLE;
            }
            return EVideoStatus.ERROR;
        }

        private void loadSubtitles()
        {
            if (mMetadata["captions"] == null)
            {
                return;
            }

            var renderer = mMetadata["captions"]["playerCaptionsTracklistRenderer"];
            if (renderer == null)
            {
                return;
            }

            var tracks = renderer["captionTracks"];
            if (tracks == null)
            {
                return;
            }

            foreach (var jToken in tracks)
            {
                var subtitle = new Subtitle(jToken["languageCode"], jToken["name"]["simpleText"], jToken["kind"]);
                try
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(jToken["baseUrl"].ToString());

                    foreach (XmlNode xmlNode in xmlDoc.DocumentElement.ChildNodes)
                    {
                        if (xmlNode.FirstChild == null)
                        {
                            continue;
                        }
                        subtitle.AddItem(new SubtitleItem(xmlNode.FirstChild.InnerText, xmlNode.Attributes["start"].Value, xmlNode.Attributes["dur"].Value, LengthSeconds >= 3600));
                    }
                    mSubtitles.Add(subtitle);
                }
                catch (System.Xml.XmlException)
                {
                    Debug.Fail("자막 데이터를 불러오는 도중 실패");
                }
            }
        }

        private static JObject getVideoInformationOrNull(string videoId)
        {
            string innerText = System.Web.HttpUtility.UrlDecode(getAllStreamFromURL(string.Format(METADATA_URL, videoId)));
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
