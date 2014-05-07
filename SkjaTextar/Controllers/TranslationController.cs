using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SkjaTextar.Models;
using SkjaTextar.ViewModels;
using System.Globalization;

namespace SkjaTextar.Controllers
{
    public class TranslationController : BaseController
    {
        //
        // GET: /Translation/
        public ActionResult Index(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = _unitOfWork.TranslationRepository.GetByID(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        [Authorize]
        public ActionResult Create(string mediaCat)
        {
            List<SelectListItem> mediaType = new List<SelectListItem> 
            { 
                new SelectListItem{ Text = "Kvikmynd", Value = "Movie" },
                new SelectListItem{ Text = "Sjónvarpsþáttur", Value = "Show" },
                new SelectListItem{ Text = "Myndbrot", Value = "Clip" }
            };
            ViewBag.MediaType = new SelectList(mediaType, "Value", "Text");
            ViewBag.CategoryID = _unitOfWork.CategoryRepository.Get().ToList();

            CultureInfo[] cinfo = CultureInfo.GetCultures(CultureTypes.InstalledWin32Cultures & ~CultureTypes.NeutralCultures);
            List<SelectListItem> language = new List<SelectListItem>();
            foreach(var cul in cinfo)
            {
                if(!cul.NativeName.Contains('('))
                { 
                    language.Add(new SelectListItem { Text = cul.NativeName, Value = cul.NativeName });
                }
            }
            ViewBag.Language = new SelectList(language, "Value", "Text");

            switch (mediaCat)
            {
                case "Movie":
                    ViewBag.MediaType = mediaCat;
                    return View("CreateMovie", new MovieTranslationViewModel());
                case "Show":
                    ViewBag.MediaType = mediaCat;
                    return View("CreateShow", new Show());
                case "Clip":
                    ViewBag.MediaType = mediaCat;
                    return View("CreateClip", new Clip());
                default:
                    return View(new Media());
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult CreateMovie(MovieTranslationViewModel movieTranslation)
        {
            if(ModelState.IsValid)
            {
                var Movie = movieTranslation.Movie;
                Movie.Translations = new List<Translation>();
                Movie.Translations.Add(new Translation { Language = movieTranslation.Language });
                _unitOfWork.MovieRepository.Insert(Movie);
                _unitOfWork.Save();
                //TODO Redirect to new translation
                return RedirectToAction("Index", "Home");
            }
            return View("Create");
        }

        [Authorize]
        public ActionResult CreateTranslation(int? id)
        {
            if (id.HasValue)
            {
                CultureInfo[] cinfo = CultureInfo.GetCultures(CultureTypes.InstalledWin32Cultures & ~CultureTypes.NeutralCultures);
                List<SelectListItem> language = new List<SelectListItem>();
                foreach (var cul in cinfo)
                {
                    if (!cul.NativeName.Contains('('))
                    {
                        language.Add(new SelectListItem { Text = cul.NativeName, Value = cul.NativeName });
                    }
                }
                ViewBag.Language = new SelectList(language, "Value", "Text");

                var model = _unitOfWork.MediaRepository.GetByID(id);
                string type = model.GetType().BaseType.Name;
                switch (type)
                {
                    case "Movie":
                        return View("CreateMovieTranslation", model);
                    case "Show":
                        return View("ShowIndex", model);
                    case "Clip":
                        return View("ClipIndex", model);
                    default:
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [Authorize]
        [HttpPost]
        public ActionResult CreateTranslation(int id, string language)
        {
            if(ModelState.IsValid)
            {
                var media = _unitOfWork.MediaRepository.GetByID(id);
                media.Translations.Add(new Translation { Language = language });
                _unitOfWork.MediaRepository.Update(media);
                _unitOfWork.Save();
            }
            // TODO redirect to new translation
            return RedirectToAction("index", "Home");
        }
	}
}