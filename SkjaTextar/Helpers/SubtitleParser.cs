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
        public static Translation Parse(string path, string format = "srt")
        {
            var translation = new Translation { TranslationSegments = new List<TranslationSegment>() };
            using (StreamReader sr = new StreamReader(path))
            {
                string nextLine;
                string tmp = "";
                while ((nextLine = sr.ReadLine()) != null)
                {
                    while (nextLine == "")
                    {
                        nextLine = sr.ReadLine();
                    }
                    TranslationSegment transSeg = new TranslationSegment();
                    transSeg.SegmentID = int.Parse(nextLine);
                    nextLine = sr.ReadLine();
                    transSeg.Timestamp = nextLine;
                    if((nextLine = sr.ReadLine()) != "" && nextLine != null)
                    {
                        transSeg.Line1 = nextLine;
						transSeg.Original1 = nextLine;
                    }
                    while ((nextLine = sr.ReadLine()) != "" && nextLine != null)
                    {
                        tmp += nextLine;
                    }
                    if(tmp != "")
                    {
                        transSeg.Line2 = tmp;
						transSeg.Original2 = tmp;
                    }
                    translation.TranslationSegments.Add(transSeg);
                    tmp = "";
                }
            }
            
            return translation;
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