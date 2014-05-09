using SkjaTextar.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SkjaTextar.ViewModels
{
    public class ClipTranslationViewModel
    {
        public Clip Clip { get; set; }
        [Required(ErrorMessage = "Tungumál þýðingar verður að vera valið")]
        [DisplayName("Tungumál")]
        public int LanguageID { get; set; }
        public string MediaType { get; set; }
    }
}