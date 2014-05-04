﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkjaTextar.Models
{
    public class RequestVote
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int RequestID { get; set; }
        public bool Vote { get; set; }
        
        public virtual Request Request;
    }
}