using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SkjaTextar.Models
{
    public class Media
    {
        public int ID { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public int CategoryID { get; set; }
        [Required]
        public int ReleaseYear { get; set; }
        [DataType(DataType.Url)]
        public virtual string Link { get; set; }
        public bool Active { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<Translation> Translations { get; set; }
    }
}