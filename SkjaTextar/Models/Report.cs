using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkjaTextar.Models
{
    public class Report
    {
        public int ID { get; set; }
        public int TranslationID { get; set; }
        public string ReportText { get; set; }
        public DateTime ReportDate { get; set; }
        public virtual Translation Translation;
    }
}