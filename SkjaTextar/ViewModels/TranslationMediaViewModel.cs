using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SkjaTextar.Models;

namespace SkjaTextar.ViewModels
{
    public class TranslationMediaViewModel
    {
        public Media Media { get; set; }
        public Translation Translation { get; set; }
    }
}