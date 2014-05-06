using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SkjaTextar.Models;
using SkjaTextar.ViewModels;
using SkjaTextar.DAL;

namespace SkjaTextar.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public HomeController()
        {
            _unitOfWork = new UnitOfWork();
        }
        public ActionResult Index()
        {
            var model = new HomeViewModel();
            var topUser = _unitOfWork.UserRepository.Get().ToList().OrderByDescending(u => u.Score).Take(5);
            var topDownload = _unitOfWork.TranslationRepository.Get().OrderByDescending(t => t.NumberOfDownloads).Take(5);
            var newTrans = _unitOfWork.TranslationRepository.Get().OrderByDescending(t => t.ID).Take(5);
            var topRequest = _unitOfWork.RequestRepository.Get().OrderByDescending(r => r.Score).Take(5);
            model.ActiveUsers = topUser.ToList();
            model.TopTranslations = topDownload.ToList();
            model.NewTranslations = newTrans.ToList();
            model.TopRequests = topRequest.ToList();
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

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}