using SkjaTextar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;
using SkjaTextar.Exceptions;
using System.Text.RegularExpressions;

namespace SkjaTextar.Helpers
{
    public class SubtitleParser
    {
        /// <summary>
        /// Parses .srt files into Translations.
        /// Srt files are of the form:
        /// 
        /// ID
        /// TIMESTAMP
        /// TEXTLINE1
        /// TEXTLINE2
        /// </summary>
        /// <param name="path"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static Translation Parse(string path, string format = "srt")
        {
            var translation = new Translation { TranslationSegments = new List<TranslationSegment>() };
            using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
            {
                string nextLine;
                string tmp = "";
                while ((nextLine = sr.ReadLine()) != null)
                {
                    // Consume extra white spaces between segments in the srt file.
                    while (nextLine == "")
                    {
                        nextLine = sr.ReadLine();
                    }
                    TranslationSegment transSeg = new TranslationSegment();
                    
                    // If the segment id in the srt file is not an integer we throw an error.
                    int segmentId = 0;
                    if(!int.TryParse(nextLine, out segmentId))
                    {
                        throw new SubtitleParseException("Villa við lestur .srt skráar, auðkenni þýðingarbúts var \"" + nextLine + "\" en þarf að vera heiltala");
                    }

                    // Get rid of extra translation lines found in .srt files from subtitle sites
                    // often containing invalid timestamps and other invalid information.
                    // These  lines usually have an id of 9999 or 0.
                    // Known bug: if The translation contains more than 9999 translation segments
                    // the rest will be ignored after 9999.
					if(int.Parse(nextLine) == 9999 || int.Parse(nextLine) == 0)
					{
						return translation;
					}
                    transSeg.SegmentID = segmentId;

                    // Checking if the timestamp matches the format that is required
                    // 00:00:00,000 --> 00:00:00,000
                    nextLine = sr.ReadLine();
                    Regex rgx = new Regex(@"^\d\d:[0-5]\d:[0-5]\d,\d\d\d\s-->\s\d\d:[0-5]\d:[0-5]\d,\d\d\d$");
                    if(!rgx.IsMatch(nextLine))
                    {
                        throw new SubtitleParseException("Villa við lestur .srt skráar, tímastimpill númer " + segmentId + " var ekki á réttu formi");
                    }

                    // Read the actual text into Line1 and Line2, if the segment contains more than 2 lines, line 2-n are 
                    // concatenated into one line and stored in Line2,
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

        public static void Output2(Translation translation, string format = "srt")
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

        public static MemoryStream Output(Translation translation, string format = "srt")
        {
            // We dont have using statements around the streams
            // Couldn't get that to work
            MemoryStream ms = new MemoryStream();
            StreamWriter sw = new StreamWriter(ms);
            foreach (var segment in translation.TranslationSegments.OrderBy(ts => ts.SegmentID))
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
                sw.Flush();
            }       
            return ms;
        }
    }
}