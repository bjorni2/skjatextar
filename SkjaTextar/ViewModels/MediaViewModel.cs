using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SkjaTextar.Models;

namespace SkjaTextar.ViewModels
{
    public class MediaViewModel
    {
        public Media Media { get; set; }
        public List<Translation> Translations { get; set; }
    }
}