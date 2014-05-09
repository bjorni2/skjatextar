using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SkjaTextar.Models
{
    public class Show : Media
    {
        [Required]
        [DisplayName("Sería")]
        public int Series { get; set; }
        [Required]
        [DisplayName("Þáttur")]
        public int Episode { get; set; }
    }
}