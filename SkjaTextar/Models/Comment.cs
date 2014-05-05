using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkjaTextar.Models
{
	public class Comment
	{
		public int ID { get; set; }
		public string CommentText { get; set; }
		public int TranslationID { get; set; }
		public int UserID { get; set; }

		public virtual Translation Translation { get; set; }
        public virtual User User { get; set; }
	}
}