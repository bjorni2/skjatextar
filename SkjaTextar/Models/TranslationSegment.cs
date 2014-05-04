using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkjaTextar.Models
{
    public class TranslationSegment
    {
        public int ID { get; set; }
        public int SegmentID { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Original1 { get; set; }
        public string Original2 { get; set; }
        public string Timestamp { get; set; }
        public int TranslationID { get; set; }

        public virtual Translation Translation { get; set; }
    }
}