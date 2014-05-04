﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkjaTextar.Models
{
    public class Request
    {
        public int ID { get; set; }
        public int MediaID { get; set; }
        public string Language { get; set; }

        public virtual Media Media { get; set; }
    }
}