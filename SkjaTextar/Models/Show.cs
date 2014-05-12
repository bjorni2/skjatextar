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
        [Required(ErrorMessage = "Þú verður að slá inn númer seríu")]
        [DisplayName("Sería")]
        public int Series { get; set; }
        [Required(ErrorMessage = "Þú verður að slá inn númer þáttar")]
        [DisplayName("Þáttur")]
        public int Episode { get; set; }
    }
}