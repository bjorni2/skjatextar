using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SkjaTextar.Models
{
    public class Category
    {
        public int ID { get; set; }
        [Display(Name = "Flokkur")]
        public string Name { get; set; }

        public virtual ICollection<Media> Medias { get; set; }
    }
}