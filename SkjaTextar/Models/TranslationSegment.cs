using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SkjaTextar.Models
{
    public class TranslationSegment
    {
        public int ID { get; set; }
        public int SegmentID { get; set; }
        [DisplayName("Lína 1")]
        public string Line1 { get; set; }
        [DisplayName("Lína 2")]
        public string Line2 { get; set; }
        [DisplayName("Upprunaleg lína 1")]
        public string Original1 { get; set; }
        [DisplayName("Upprunaleg lína 2")]
        public string Original2 { get; set; }
        [RegularExpression(@"^\d\d:[0-5]\d:[0-5]\d,\d\d\d\s-->\s\d\d:[0-5]\d:[0-5]\d,\d\d\d$",
         ErrorMessage = "Tímastimpill þarf að vera á forminu \"00:00:00,000 --> 99:59:59,999\".")]
        public string Timestamp { get; set; }
        public int TranslationID { get; set; }

        public virtual Translation Translation { get; set; }
    }
}