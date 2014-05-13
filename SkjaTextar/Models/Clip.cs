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
        [Required(ErrorMessage = "Þú verður að setja inn Youtube hlekk fyrir myndbrotið")]
        [DisplayName("Youtube hlekkur")]
        public override string Link { get; set; }
    }
}