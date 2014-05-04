using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkjaTextar.Models
{
    public class Show : Media
    {
        public int Series { get; set; }
        public int Episode { get; set; }
    }
}