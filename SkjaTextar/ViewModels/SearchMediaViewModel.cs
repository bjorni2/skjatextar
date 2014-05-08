using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SkjaTextar.Models;

namespace SkjaTextar.ViewModels
{
    public class SearchMediaViewModel
    {
        public List<Movie> Movies { get; set; }
        public List<Show> Shows { get; set; }
        public List<Clip> Clips { get; set; }

        public SearchMediaViewModel()
        {
            Movies = new List<Movie>();
            Shows = new List<Show>();
            Clips = new List<Clip>();
        }
    }
}