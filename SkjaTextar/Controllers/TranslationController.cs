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
            // TODO: Create check for existing media and year
            List<SelectListItem> mediaType = new List<SelectListItem> 
            { 
                new SelectListItem{ Text = "Kvikmynd", Value = "Movie" },
                new SelectListItem{ Text = "Sjónvarpsþáttur", Value = "Show" },
                new SelectListItem{ Text = "Myndbrot", Value = "Clip" }
            };
            ViewBag.MediaType = new SelectList(mediaType, "Value", "Text");
            ViewBag.CategoryID = _unitOfWork.CategoryRepository.Get().ToList();

            /*CultureInfo[] cinfo = CultureInfo.GetCultures(CultureTypes.InstalledWin32Cultures & ~CultureTypes.NeutralCultures);
            List<SelectListItem> language = new List<SelectListItem>();
            foreach(var cul in cinfo)
            {
                if(!cul.NativeName.Contains('('))
                { 
                    language.Add(new SelectListItem { Text = cul.NativeName, Value = cul.NativeName });
                }
            }*/
            ViewBag.LanguageID = new SelectList(_unitOfWork.LanguageRepository.Get(), "ID", "Name");

            switch (mediaCat)
            {
                case "Movie":
                    ViewBag.MediaType = mediaCat;
                    return View("CreateMovie", new MovieTranslationViewModel());
                case "Show":
                    ViewBag.MediaType = mediaCat;
                    return View("CreateShow", new ShowTranslationViewModel());
                case "Clip":
                    ViewBag.MediaType = mediaCat;
                    return View("CreateClip", new ClipTranslationViewModel());
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
                Movie.Translations.Add(new Translation { LanguageID = movieTranslation.LanguageID });
                _unitOfWork.MovieRepository.Insert(Movie);
                _unitOfWork.Save();
                //TODO Redirect to new translation
                return RedirectToAction("Index", "Home");
            }
            return View("Create");
        }

        [Authorize]
        [HttpPost]
        public ActionResult CreateShow(ShowTranslationViewModel showTranslation)
        {
            if (ModelState.IsValid)
            {
                var Show = showTranslation.Show;
                Show.Translations = new List<Translation>();
                Show.Translations.Add(new Translation { LanguageID = showTranslation.LanguageID });
                _unitOfWork.ShowRepository.Insert(Show);
                _unitOfWork.Save();
                //TODO Redirect to new translation
                return RedirectToAction("Index", "Home");
            }
            return View("Create");
        }

        [Authorize]
        [HttpPost]
        public ActionResult CreateClip(ClipTranslationViewModel clipTranslation)
        {
            if (ModelState.IsValid)
            {
                var Clip = clipTranslation.Clip;
                Clip.Translations = new List<Translation>();
                Clip.Translations.Add(new Translation { LanguageID = clipTranslation.LanguageID });
                _unitOfWork.ClipRepository.Insert(Clip);
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
                ViewBag.LanguageID = new SelectList(_unitOfWork.LanguageRepository.Get(), "ID", "Name");

                var model = _unitOfWork.MediaRepository.GetByID(id);
                string type = model.GetType().BaseType.Name;
                switch (type)
                {
                    case "Movie":
                        return View("CreateMovieTranslation", model);
                    case "Show":
                        return View("CreateShowTranslation", model);
                    case "Clip":
                        return View("CreateClipTranslation", model);
                    default:
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [Authorize]
        [HttpPost]
        public ActionResult CreateTranslation(int id, int languageID)
        {
            if(ModelState.IsValid)
            {
                var media = _unitOfWork.MediaRepository.GetByID(id);
                media.Translations.Add(new Translation { LanguageID = languageID });
                _unitOfWork.MediaRepository.Update(media);
                _unitOfWork.Save();
            }
            // TODO redirect to new translation
            return RedirectToAction("index", "Home");
        }
	}
}