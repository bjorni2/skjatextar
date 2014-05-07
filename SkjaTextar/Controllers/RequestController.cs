using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SkjaTextar.DAL;
using System.Net;
using SkjaTextar.Models;

namespace SkjaTextar.Controllers
{
    public class RequestController : BaseController
    {
        //
        // GET: /Request/
        public ActionResult Index()
        {
			var model = _unitOfWork.CategoryRepository.Get().ToList().OrderByDescending(m => m.ID);
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
						return View("ShowRequestDetails", model);
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
				return View(model);
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

		public ActionResult Create()
		{
			List<SelectListItem> Categories = new List<SelectListItem>();
			Categories.Add(new SelectListItem { Text = "Kvikmynd", Value = "Movie" });
			Categories.Add(new SelectListItem { Text = "Sjónvarpsþáttur", Value = "Show" });
			Categories.Add(new SelectListItem { Text = "Myndbrot", Value = "Clip" });
			ViewBag.Categories = Categories;

			return View(new Request());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include="ID, MediaID, Language, Score")]Request request)
		{
			if(ModelState.IsValid)
			{
				_unitOfWork.RequestRepository.Insert(request);
				_unitOfWork.Save();
				return RedirectToAction("Details", request.ID);
			}
			return View(request);
		}
	}
}