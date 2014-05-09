using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SkjaTextar.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SkjaTextar.ViewModels
{
    public class MovieTranslationViewModel
    {
        public Movie Movie { get; set; }
        [Required(ErrorMessage = "Tungumál þýðingar verður að vera valið")]
        [DisplayName("Tungumál")]
        public int LanguageID { get; set; }
        public string MediaType { get; set; }
    }
}