using ScripTube.Models.YouTube;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScripTube.Utils
{
    public static class YouTubeUtil
    {
        public static string GetVideoIdByUrl(string url)
        {
            try
            {
                Uri uri = new Uri(url);
                if (uri.Host == "www.youtube.com" || uri.Host == "youtu.be")
                {
                    System.Collections.Specialized.NameValueCollection query = System.Web.HttpUtility.ParseQueryString(uri.Query);
                    if (query.AllKeys.Contains("v") && query["v"].Length == Video.ID_LENGTH)
                    {
                        return query["v"];
                    }
                }                
            }
            catch (System.UriFormatException)
            {
            }
            return string.Empty;
        }
    }
}
