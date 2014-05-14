using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SkjaTextar.Models
{
	public class Comment
	{
		public int ID { get; set; }
        [Required(ErrorMessage = "Athugasemd óútfyllt")]
		public string CommentText { get; set; }
		public int TranslationID { get; set; }
		public string UserID { get; set; }
        public string AvatarUrl { get; set; }

		public virtual Translation Translation { get; set; }
        public virtual User User { get; set; }
	}
}