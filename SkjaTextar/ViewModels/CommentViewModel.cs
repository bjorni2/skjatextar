using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SkjaTextar.Models;

namespace SkjaTextar.ViewModels
{
    public class CommentViewModel
    {
        public List<Comment> Comments { get; set; }
        public Translation Translation { get; set; }
        public Comment Comment { get; set; }

        public CommentViewModel()
        {
            Comments = new List<Comment>();
        }
    }
}