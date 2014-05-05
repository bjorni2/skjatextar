using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkjaTextar.Models
{
    public class Translation
    {
        public int ID { get; set; }
        public int MediaID { get; set; }
        public int Score { get; set; }
        public int NumberOfDownloads { get; set; }
        public bool Locked { get; set; }
        public string Language { get; set; }

        public virtual Media Media { get; set; }
        public virtual ICollection<TranslationSegment> TranslationSegments { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
        public virtual ICollection<TranslationVote> TranslationVotes { get; set; }
    }
}