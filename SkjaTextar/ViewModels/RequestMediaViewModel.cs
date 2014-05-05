using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SkjaTextar.Models;

namespace SkjaTextar.ViewModels
{
    public class RequestMediaViewModel
    {
        public Request Request { get; set; }
        public Media Media { get; set; }
    }
}