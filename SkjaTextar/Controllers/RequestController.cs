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
        public ActionResult Create()
        {
            List<SelectListItem> Categories = new List<SelectListItem>();
            Categories.Add(new SelectListItem { Text = "Kvikmynd", Value = "Movie" });
            Categories.Add(new SelectListItem { Text = "Sjónvarpsþáttur", Value = "Show" });
            Categories.Add(new SelectListItem { Text = "Myndbrot", Value = "Clip" });
            ViewBag.Categories = Categories;

			var subCategories = new SelectList(_unitOfWork.CategoryRepository.Get(), "ID", "Name");
			ViewBag.SubCategories = subCategories;

			var languages = new SelectList(_unitOfWork.LanguageRepository.Get(), "ID", "Name");
			ViewBag.Languages = languages;

            return View(new Request());
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