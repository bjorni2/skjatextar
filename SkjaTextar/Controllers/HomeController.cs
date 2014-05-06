using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SkjaTextar.Models;
using SkjaTextar.ViewModels;

namespace SkjaTextar.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        ApplicationDbContext _context = new ApplicationDbContext();
        public ActionResult Index()
        {
            var model = new HomeViewModel();
            var topUser = _context.Users.ToList().OrderByDescending(u => u.Score).Take(5);
            var topDownload = _context.Translations.OrderByDescending(t => t.NumberOfDownloads).Take(5);
            var newTrans = _context.Translations.OrderByDescending(t => t.ID).Take(5);
            model.ActiveUsers = topUser.ToList();
            model.TopTranslations = topDownload.ToList();
            model.NewTranslations = newTrans.ToList();
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "WAZZZZUP";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}