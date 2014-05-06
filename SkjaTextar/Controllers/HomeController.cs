using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using SkjaTextar.Models;
using SkjaTextar.ViewModels;
using SkjaTextar.DAL;

namespace SkjaTextar.Controllers
{
    public class HomeController : BaseController
    {

        public HomeController() : base()
        {
        }

        public HomeController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public ActionResult Index()
        {
            var model = new HomeViewModel();
            model.ActiveUsers = _unitOfWork.UserRepository.Get().ToList().OrderByDescending(u => u.Score).Take(5).ToList();
            model.TopTranslations = _unitOfWork.TranslationRepository.Get().OrderByDescending(t => t.NumberOfDownloads).Take(5).Include(t => t.Media).ToList();
            model.NewTranslations = _unitOfWork.TranslationRepository.Get().OrderByDescending(t => t.ID).Take(5).Include(t => t.Media).ToList();
            model.TopRequests = _unitOfWork.RequestRepository.Get().OrderByDescending(r => r.Score).Take(5).Include(r => r.Media).ToList();
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