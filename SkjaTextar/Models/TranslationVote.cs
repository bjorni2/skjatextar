﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkjaTextar.Models
{
    public class TranslationVote
    {
        public int ID { get; set; }
        public int TranslationID { get; set; }
        public int UserID { get; set; }
        public bool Vote { get; set; }

        public Translation Translation { get; set; }
    }
}