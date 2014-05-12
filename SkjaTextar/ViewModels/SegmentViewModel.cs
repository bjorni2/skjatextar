using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SkjaTextar.ViewModels
{
    public class SegmentViewModel
    {
        [DisplayName("Lína 1")]
        public string Line1 { get; set; }
        [DisplayName("Lína 2")]
        public string Line2 { get; set; }
        [DisplayName("Upprunaleg lína 1")]
        public string Original1 { get; set; }
        [DisplayName("Upprunaleg lína 2")]
        public string Original2 { get; set; }
        [DisplayName("Birta frá")]
        [RegularExpression(@"^\d\d:[0-5]\d:[0-5]\d,\d\d\d$",
         ErrorMessage = "Tímastimpill þarf að vera á forminu \"00:00:00,000\".")]
        public string TimestampStart { get; set; }
        [DisplayName("Birta til")]
        [RegularExpression(@"^\d\d:[0-5]\d:[0-5]\d,\d\d\d$",
         ErrorMessage = "Tímastimpill þarf að vera á forminu \"00:00:00,000\".")]
        public string TimestampEnd { get; set; }
        public int TranslationID { get; set; }
    }
}