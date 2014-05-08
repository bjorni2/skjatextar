using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SkjaTextar.Models;

namespace SkjaTextar.ViewModels
{
    public class MovieTranslationViewModel
    {
        public Movie Movie { get; set; }
        public int LanguageID { get; set; }
    }
}