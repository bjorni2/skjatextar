using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using SkjaTextar.Models;
using SkjaTextar.ViewModels;
using SkjaTextar.DAL;
using SkjaTextar.Helpers;

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

        public ActionResult Search(string searchQuery)
        {
            var model = new SearchMediaViewModel();
            if(string.IsNullOrEmpty(searchQuery))
            {
                ViewBag.Count = 0;
                return View(model);
            }
            var movies = _unitOfWork.MovieRepository.Get().Where(m => m.Title.Contains(searchQuery));
            var shows = _unitOfWork.ShowRepository.Get().Where(m => m.Title.Contains(searchQuery));
            var clips = _unitOfWork.ClipRepository.Get().Where(m => m.Title.Contains(searchQuery));
            model.Movies = movies.ToList();
            model.Shows = shows.ToList();
            model.Clips = clips.ToList();
            ViewBag.Count = movies.Count() + shows.Count() + clips.Count();
            return View(model);
        }


        public JsonResult Autocomplete(string term)
        {
            var results = new List<SearchResult>();
           /* var movies = _unitOfWork.MovieRepository.Get().Where(m => m.Title.Contains(term));
            var shows = _unitOfWork.ShowRepository.Get().Where(m => m.Title.Contains(term));
            var clips = _unitOfWork.ClipRepository.Get().Where(m => m.Title.Contains(term));
            foreach (var item in movies)
            {
                var tmp = new Result { id = item.ID, label = item.Title };
                results.Add(tmp);
            }
            foreach (var item in shows)
            {
                var tmp = new Result { id = item.ID, label = item.Title };
                results.Add(tmp);
            }
            foreach (var item in clips)
            {
                var tmp = new Result { id = item.ID, label = item.Title };
                results.Add(tmp);
            }*/
            var media = _unitOfWork.MediaRepository.Get().Where(m => m.Title.Contains(term)).OrderBy(m => m.Title);
            foreach (var item in media)
            {
                var tmp = new SearchResult();
                if(item.GetType().BaseType.Name == "Show")
                {
                    var tmpitem = item as Show;
                    tmp = new SearchResult { id = item.ID, label = item.Title + " S" + tmpitem.Series + "E" + tmpitem.Episode };
                }
                else
                {
                    tmp = new SearchResult { id = item.ID, label = item.Title };                    
                }
                results.Add(tmp);
            }
            return Json(results, JsonRequestBehavior.AllowGet);
        }

    }
}