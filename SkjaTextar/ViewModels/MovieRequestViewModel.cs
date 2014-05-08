using SkjaTextar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkjaTextar.ViewModels
{
    public class MovieRequestViewModel
    {
        public Movie Movie { get; set; }
        public Request Request { get; set; }
    }
}