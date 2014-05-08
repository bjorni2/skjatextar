using SkjaTextar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkjaTextar.ViewModels
{
    public class ClipTranslationViewModel
    {
        public Clip Clip { get; set; }
        public int LanguageID { get; set; }
    }
}