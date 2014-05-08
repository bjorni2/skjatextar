using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkjaTextar.Models
{
    public class Request
    {
        public int ID { get; set; }
        public int MediaID { get; set; }
        public int LanguageID { get; set; }
        public int Score { get; set; }

        public virtual Media Media { get; set; }
		public virtual Language Language { get; set; }
    }
}