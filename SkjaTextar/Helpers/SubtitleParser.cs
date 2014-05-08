using SkjaTextar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;

namespace SkjaTextar.Helpers
{
    public class SubtitleParser
    {
        public static Translation Parse(string path, string format)
        {
            return new Translation();
        }

        public static void Output(Translation translation, string format = "srt")
        {

            string path = HttpContext.Current.Server.MapPath("~/SubtitleStorage/m" + translation.MediaID.ToString() + "t" + translation.ID.ToString() + "." + format);
            using(StreamWriter sw = new StreamWriter(path))
            {
                foreach (var segment in translation.TranslationSegments)
                {
                    sw.WriteLine(segment.SegmentID.ToString());
                    sw.WriteLine(segment.Timestamp);
                    if (!String.IsNullOrEmpty(segment.Line1))
                    {
                        sw.WriteLine(segment.Line1);
                    }
                    if (!String.IsNullOrEmpty(segment.Line2))
                    {
                        sw.WriteLine(segment.Line2);
                    }
                    sw.WriteLine();
                }
            }
        }
    }
}