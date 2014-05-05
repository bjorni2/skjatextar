using SkjaTextar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkjaTextar.ViewModels
{
    public class HomeViewModel
    {
        public List<Translation> NewTranslations { get; set; }
        public List<Translation> TopTranslations { get; set; }
        public List<User> ActiveUsers { get; set; }
        public List<Request> TopRequests { get; set; }

        public HomeViewModel()
        {
            NewTranslations = new List<Translation>();
            TopTranslations = new List<Translation>();
            ActiveUsers = new List<User>();
            TopRequests = new List<Request>();
        }
    }
}