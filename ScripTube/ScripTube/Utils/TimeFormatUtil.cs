using ScripTube.Views.Converters;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScripTube.Utils
{
    public static class TimeFormatUtil
    {
        public static readonly string INVALID_HHMMSS = "--:--:--";
        public static readonly string INVALID_MMSS = "--:--";

        public static string GetHHMMSSOrMMSS(double seconds, bool bHourNecessary)
        {
            if (seconds < 0)
            {
                return bHourNecessary ? INVALID_HHMMSS : INVALID_MMSS;
            }

            int total = (int)seconds;
            int min = total / 60;
            int sec = total % 60;

            if (bHourNecessary || total >= 3600)
            {
                int hour = min / 60;
                min %= 60;

                return string.Format("{0}:{1}:{2}", hour.ToString("D2"), min.ToString("D2"), sec.ToString("D2"));
            }

            return string.Format("{0}:{1}", min.ToString("D2"), sec.ToString("D2"));
        }

        public static string GetHHMMSSOrMMSSPrecision(double seconds, bool bHourNecessary)
        {
            if (seconds < 0)
            {
                return bHourNecessary ? INVALID_HHMMSS : INVALID_MMSS;
            }
            double remainder = seconds - (int)seconds;

            var sb = new StringBuilder();
            sb.Append(GetHHMMSSOrMMSS(seconds, bHourNecessary));
            sb.Append(',');
            sb.Append(string.Format("{0:.000}", remainder).Substring(1));

            return sb.ToString();
        }
    }
}
