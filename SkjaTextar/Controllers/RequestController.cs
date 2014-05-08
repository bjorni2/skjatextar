using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SkjaTextar.DAL;
using System.Net;
using SkjaTextar.Models;
using SkjaTextar.ViewModels;

namespace SkjaTextar.Controllers
{
    public class RequestController : BaseController
    {
        //
        // GET: /Request/
        public ActionResult Index()
        {
            List<Request> model = _unitOfWork.RequestRepository.Get().OrderByDescending(m => m.ID).ToList();
			return View(model);
        }

		public ActionResult Details(int? id)
		{
			if(id.HasValue)
			{
				var model = _unitOfWork.RequestRepository.GetByID(id);
				string type = model.Media.GetType().BaseType.Name;
				switch (type)
				{
					case "Movie":
						return View("MovieRequestDetails", model);
					case "Show":
						ShowRequestViewModel showModel = new ShowRequestViewModel();
						showModel.Request = model;
						var myShow = _unitOfWork.ShowRepository.GetByID(model.MediaID);
						showModel.Show = myShow;
						return View("ShowRequestDetails", showModel);
					case "Clip":
						return View("ClipRequestDetails", model);
					default:
						return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}
			}
			return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
		}

		public ActionResult MovieRequestDetails(int? id)
		{
			if(id.HasValue)
			{
				var model = _unitOfWork.RequestRepository.GetByID(id);
				return View(model);
			}
			return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
		}

		public ActionResult ShowRequestDetails(int? id)
		{
			if (id.HasValue)
			{
				var model = _unitOfWork.RequestRepository.GetByID(id);
				ShowRequestViewModel showModel = new ShowRequestViewModel();
				showModel.Request = model;
				var myShow = _unitOfWork.ShowRepository.GetByID(model.MediaID);
				showModel.Show = myShow;
				return View(showModel);
			}
			return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
		}

		public ActionResult ClipRequestDetails(int? id)
		{
			if (id.HasValue)
			{
				var model = _unitOfWork.RequestRepository.GetByID(id);
				return View(model);
			}
			return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
		}

        // This ActionResult is used to decide what view to use when creating a request for a specific media
        public ActionResult CreateFor(int? id)
        {
            if (id.HasValue)
            {
                var model = _unitOfWork.MediaRepository.GetByID(id);
                string type = model.GetType().BaseType.Name;
                switch (type)
                {
                    case "Movie":
                        return View("CreateForMovie", model);
                    case "Show":
                        return View("CreateForShow", model);
                    case "Clip":
                        return View("CreateForClip", model);
                    default:
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // This ActionResult is used to create a request for a specific movie
        public ActionResult CreateForMovie(int? id)
        {
            if(id.HasValue)
            {
                var model = _unitOfWork.MediaRepository.GetByID(id);
                return View(model);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // This ActionResult is used to create a request for a specific Show
        public ActionResult CreateForShow(int? id)
        {
            if (id.HasValue)
            {
                var model = _unitOfWork.MediaRepository.GetByID(id);
                return View(model);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // This ActionResult is used to create a request for a specific Clip
        public ActionResult CreateForClip(int? id)
        {
            if (id.HasValue)
            {
                var model = _unitOfWork.MediaRepository.GetByID(id);
                return View(model);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // This ActionResult is used to get a create templete for a request
        public ActionResult Create(string mediaCat)
        {
            /*List<SelectListItem> Categories = new List<SelectListItem>();
            Categories.Add(new SelectListItem { Text = "Kvikmynd", Value = "Movie" });
            Categories.Add(new SelectListItem { Text = "Sjónvarpsþáttur", Value = "Show" });
            Categories.Add(new SelectListItem { Text = "Myndbrot", Value = "Clip" });
            ViewBag.Categories = Categories;*/

            // TODO: Create check for existing media and year
            List<SelectListItem> mediaType = new List<SelectListItem> 
            { 
                new SelectListItem{ Text = "Kvikmynd", Value = "Movie" },
                new SelectListItem{ Text = "Sjónvarpsþáttur", Value = "Show" },
                new SelectListItem{ Text = "Myndbrot", Value = "Clip" }
            };
            ViewBag.MediaType = new SelectList(mediaType, "Value", "Text");

			var subCategories = new SelectList(_unitOfWork.CategoryRepository.Get(), "ID", "Name");
			ViewBag.SubCategories = subCategories;

			var languages = new SelectList(_unitOfWork.LanguageRepository.Get(), "ID", "Name");
			ViewBag.Languages = languages;

            switch (mediaCat)
            {
                case "Movie":
                    ViewBag.MediaType = mediaCat;
                    return View("RequestMovie", new MovieRequestViewModel());
                case "Show":
                    ViewBag.MediaType = mediaCat;
                    return View("RequestShow", new ShowRequestViewModel());
                case "Clip":
                    ViewBag.MediaType = mediaCat;
                    return View("RequestClip", new ClipRequestViewModel());
                default:
                    return View(new Request());
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult CreateMovie(MovieRequestViewModel movieRequest)
        {
            if (ModelState.IsValid)
            {              
                _unitOfWork.MovieRepository.Insert(movieRequest.Movie);
                _unitOfWork.RequestRepository.Insert(movieRequest.Request);
                _unitOfWork.Save();
                //TODO Redirect to new request
                return RedirectToAction("Index", "Home");
            }
            return View("Create");
        }

        [Authorize]
        [HttpPost]
        public ActionResult CreateShow(ShowRequestViewModel showRequest)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.ShowRepository.Insert(showRequest.Show);
                _unitOfWork.RequestRepository.Insert(showRequest.Request);
                _unitOfWork.Save();
                //TODO Redirect to new request
                return RedirectToAction("Index", "Home");
            }
            return View("Create");
        }

        [Authorize]
        [HttpPost]
        public ActionResult CreateClip(ClipRequestViewModel clipRequest)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.ClipRepository.Insert(clipRequest.Clip);
                _unitOfWork.RequestRepository.Insert(clipRequest.Request);
                _unitOfWork.Save();
                //TODO Redirect to new request
                return RedirectToAction("Index", "Home");
            }
            return View("Create");
        }

        // This ActionResult is used to Post a brand new request
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(/*[Bind(Include="ID, MediaID, LanguageID, Score")]*/Request request)
		{
			if(ModelState.IsValid)
			{
				_unitOfWork.RequestRepository.Insert(request);
				_unitOfWork.Save();
				return RedirectToAction("Details", request);
			}
			return View(request); // þarf einhvað skoða þetta
		}
	}
}