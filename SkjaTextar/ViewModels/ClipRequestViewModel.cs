using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SkjaTextar.Models;

namespace SkjaTextar.ViewModels
{
    public class ClipRequestViewModel
    {
        public Clip Clip { get; set; }
        public Request Request { get; set; }
    }
}