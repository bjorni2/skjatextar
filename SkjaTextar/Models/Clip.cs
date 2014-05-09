using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SkjaTextar.Models
{
    public class Clip : Media
    {
        [Required]
        [DisplayName("Hlekkur")]
        public override string Link { get; set; }
    }
}