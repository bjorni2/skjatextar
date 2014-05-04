using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkjaTextar.Models
{
    public class Category
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Media> Medias { get; set; }
    }
}