﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using SkjaTextar.Models;
using SkjaTextar.ViewModels;
using SkjaTextar.DAL;
using SkjaTextar.Helpers;
using SkjaTextar.Exceptions;
using MoreLinq;

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

        /// <summary>
        /// Displays the sites home page with top lists for active users, most downloaded translations, latest translations and the top requests.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = new HomeViewModel();
            model.ActiveUsers = _unitOfWork.UserRepository.Get().ToList().OrderByDescending(u => u.Score).Take(5).ToList();

            model.TopTranslations = _unitOfWork.TranslationRepository.Get().OrderByDescending(t => t.NumberOfDownloads).Take(5).Include(t => t.Media).ToList();
            model.NewTranslations = _unitOfWork.TranslationRepository.Get().OrderByDescending(t => t.ID).Take(5).Include(t => t.Media).ToList();
            model.TopRequests = _unitOfWork.RequestRepository.Get().OrderByDescending(r => r.Score).Take(5).Include(r => r.Media).ToList();

            var mediaList = new List<Media>();
            foreach (var item in model.TopTranslations)
            {
                mediaList.Add(item.Media);
            }
            foreach (var item in model.NewTranslations)
            {
                mediaList.Add(item.Media);
            }
            foreach (var item in model.TopRequests)
            {
                mediaList.Add(item.Media);
            }

            foreach (var item in mediaList.DistinctBy(m => m.ID))
            {
                if (item.GetType().BaseType.Name == "Show")
                {
                    Show show = item as Show;
                    item.Title += " S" + show.Series + "E" + show.Episode;
                }
            }

            return View(model);
        }

        /// <summary>
        /// Displays a search result page with all media items containing a substring "searchQuery".
        /// </summary>
        /// <param name="searchQuery">The substring to search for in media titles</param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns an json object containing all media items containing a substring "term".
        /// </summary>
        /// <param name="term">the substring to search for</param>
        /// <returns></returns>
        public JsonResult Autocomplete(string term)
        {
            var results = new List<SearchResult>();
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