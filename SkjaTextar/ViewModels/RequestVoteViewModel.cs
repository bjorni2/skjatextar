using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SkjaTextar.Models;

namespace SkjaTextar.ViewModels
{
    public class RequestVoteViewModel
    {
        public Request Request { get; set; }
        public bool? Vote { get; set; }
    }
}