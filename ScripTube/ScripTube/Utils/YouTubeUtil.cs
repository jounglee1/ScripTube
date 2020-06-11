using System;
using System.Linq;

namespace ScripTube.Utils
{
    public static class YouTubeUtil
    {
        private static int ID_LENGTH = 11;

        public static string GetVideoIdByUrl(string urlOrNull)
        {
            try
            {
                if (urlOrNull == null)
                {
                    return string.Empty;
                }

                Uri uri = new Uri(urlOrNull);
                if (uri.Host == "www.youtube.com" || uri.Host == "youtu.be")
                {
                    System.Collections.Specialized.NameValueCollection query = System.Web.HttpUtility.ParseQueryString(uri.Query);
                    if (query.AllKeys.Contains("v") && query["v"].Length == ID_LENGTH)
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
