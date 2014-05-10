using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SkjaTextar.DAL;
using System.Net;
using SkjaTextar.Models;
using SkjaTextar.ViewModels;
using Microsoft.AspNet.Identity;
using MoreLinq;

namespace SkjaTextar.Controllers
{
    public class RequestController : BaseController
    {
        //
        // GET: /Request/
        public ActionResult Index()
        {
            List<Request> model = _unitOfWork.RequestRepository.Get().OrderByDescending(m => m.ID).ToList();
			var requestLoop = model.DistinctBy(r => r.MediaID);
			foreach(var item in requestLoop)
			{
				var media = item.Media;
				if(media.GetType().BaseType.Name == "Show")
				{
					Show show = media as Show;
					item.Media.Title += " S" + show.Series + "E" + show.Episode;
				}
			}
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
				var movie = movieRequest.Movie;
				Translation translation;
				Request req = null;
				var movieToCheckFor = _unitOfWork.MovieRepository.Get()
					.Where(m => m.Title == movie.Title)
					.Where(m => m.ReleaseYear == movie.ReleaseYear)
					.SingleOrDefault();
				if (movieToCheckFor == null)
				{
					_unitOfWork.MovieRepository.Insert(movieRequest.Movie);
					_unitOfWork.RequestRepository.Insert(movieRequest.Request);
					_unitOfWork.Save();
					return RedirectToAction("Index", "Request");
				}
				else if ((translation = _unitOfWork.TranslationRepository.Get()
					.Where(t => t.MediaID == movieToCheckFor.ID)
					.Where(t => t.LanguageID == movieRequest.Request.LanguageID)
					.SingleOrDefault()) == null &&
					(req = _unitOfWork.RequestRepository.Get()
					.Where(r => r.MediaID == movieToCheckFor.ID)
					.Where(r => r.LanguageID == movieRequest.Request.LanguageID)
					.SingleOrDefault()) == null)
				{

					var request = movieRequest.Request;
					request.MediaID = movieToCheckFor.ID;
					_unitOfWork.RequestRepository.Insert(request);
					_unitOfWork.Save();
					return RedirectToAction("Index", "Request");
				}
				movieRequest.Movie = movieToCheckFor;
				if(req != null)
				{
					ViewBag.Errormsg = "Þessi beiðni er nú þegar til.";
					ViewBag.ReqExist = true;
				}
				else
				{
					ViewBag.Errormsg = "Þessi þýðing er nú þegar til.";
					ViewBag.ReqExist = false;
					ViewBag.TranslationID = translation.ID;
				}
			}
			ViewBag.SubCategories = new SelectList(_unitOfWork.CategoryRepository.Get(), "ID", "Name", movieRequest.Movie.CategoryID);
			ViewBag.Languages = new SelectList(_unitOfWork.LanguageRepository.Get(), "ID", "Name");
			return View("RequestMovie");
        }

        [Authorize]
        [HttpPost]
        public ActionResult CreateShow(ShowRequestViewModel showRequest)
        {
			if (ModelState.IsValid)
			{
				var show = showRequest.Show;
				Translation translation;
				Request req = null;
				var showToCheckFor = _unitOfWork.ShowRepository.Get()
					.Where(s => s.Title == show.Title)
					.Where(s => s.ReleaseYear == show.ReleaseYear)
					.Where(s => s.Series == show.Series)
					.Where(s => s.Episode == show.Episode)
					.SingleOrDefault();
				if (showToCheckFor == null)
				{
					_unitOfWork.ShowRepository.Insert(showRequest.Show);
					_unitOfWork.RequestRepository.Insert(showRequest.Request);
					_unitOfWork.Save();
					return RedirectToAction("Index", "Request");
				}
				else if ((translation = _unitOfWork.TranslationRepository.Get()
					.Where(t => t.MediaID == showToCheckFor.ID)
					.Where(t => t.LanguageID == showRequest.Request.LanguageID)
					.SingleOrDefault()) == null &&
					(req = _unitOfWork.RequestRepository.Get()
					.Where(r => r.MediaID == showToCheckFor.ID)
					.Where(r => r.LanguageID == showRequest.Request.LanguageID)
					.SingleOrDefault()) == null)
				{

					var request = showRequest.Request;
					request.MediaID = showToCheckFor.ID;
					_unitOfWork.RequestRepository.Insert(request);
					_unitOfWork.Save();
					return RedirectToAction("Index", "Request");
				}
				showRequest.Show = showToCheckFor;
				if (req != null)
				{
					ViewBag.Errormsg = "Þessi beiðni er nú þegar til.";
					ViewBag.ReqExist = true;
				}
				else
				{
					ViewBag.Errormsg = "Þessi þýðing er nú þegar til.";
					ViewBag.ReqExist = false;
					ViewBag.TranslationID = translation.ID;
				}
			}
			ViewBag.SubCategories = new SelectList(_unitOfWork.CategoryRepository.Get(), "ID", "Name", showRequest.Show.CategoryID);
			ViewBag.Languages = new SelectList(_unitOfWork.LanguageRepository.Get(), "ID", "Name");
			return View("RequestShow");
        }

        [Authorize]
        [HttpPost]
        public ActionResult CreateClip(ClipRequestViewModel clipRequest)
        {
			if (ModelState.IsValid)
			{
				var clip = clipRequest.Clip;
				Translation translation;
				Request req = null;
				var clipToCheckFor = _unitOfWork.ClipRepository.Get()
					.Where(c => c.Title == clip.Title)
					.Where(c => c.ReleaseYear == clip.ReleaseYear)
					.SingleOrDefault();
				if (clipToCheckFor == null)
				{
					_unitOfWork.ClipRepository.Insert(clipRequest.Clip);
					_unitOfWork.RequestRepository.Insert(clipRequest.Request);
					_unitOfWork.Save();
					return RedirectToAction("Index", "Request");
				}
				else if ((translation = _unitOfWork.TranslationRepository.Get()
					.Where(t => t.MediaID == clipToCheckFor.ID)
					.Where(t => t.LanguageID == clipRequest.Request.LanguageID)
					.SingleOrDefault()) == null &&
					(req = _unitOfWork.RequestRepository.Get()
					.Where(r => r.MediaID == clipToCheckFor.ID)
					.Where(r => r.LanguageID == clipRequest.Request.LanguageID)
					.SingleOrDefault()) == null)
				{

					var request = clipRequest.Request;
					request.MediaID = clipToCheckFor.ID;
					_unitOfWork.RequestRepository.Insert(request);
					_unitOfWork.Save();
					return RedirectToAction("Index", "Request");
				}
				clipRequest.Clip = clipToCheckFor;
				if (req != null)
				{
					ViewBag.Errormsg = "Þessi beiðni er nú þegar til.";
					ViewBag.ReqExist = true;
				}
				else
				{
					ViewBag.Errormsg = "Þessi þýðing er nú þegar til.";
					ViewBag.ReqExist = false;
					ViewBag.TranslationID = translation.ID;
				}
			}
			ViewBag.SubCategories = new SelectList(_unitOfWork.CategoryRepository.Get(), "ID", "Name", clipRequest.Clip.CategoryID);
			ViewBag.Languages = new SelectList(_unitOfWork.LanguageRepository.Get(), "ID", "Name");
			return View("RequestClip");
        }

        [Authorize]
        [HttpPost]
        public ActionResult RequestVote(int? id, bool vote)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var userID = User.Identity.GetUserId();
            var request = _unitOfWork.RequestRepository.GetByID(id);
            var requestVote = _unitOfWork.RequestVoteRepository.Get().Where(r => r.RequestID == id).Where(r => r.UserID == userID).SingleOrDefault();
            if(requestVote != null)
            {
                if(requestVote.Vote == vote)
                {
                    return RedirectToAction("Index", "Request", request);
                }
                else
                {
                    if(vote == true)
                    {
                        request.Score += 2;
                        requestVote.Vote = true;
                    }
                    else
                    {
                        request.Score -= 2;
                        requestVote.Vote = false;
                    }
                    _unitOfWork.Save();
                    return RedirectToAction("Index", "Request", request);
                }
            }
            int requestID = id.Value;
            var newRequestVote = new RequestVote();
            newRequestVote.UserID = userID;
            newRequestVote.RequestID = requestID;
            newRequestVote.Vote = vote;
            if(vote == true)
            {
                request.Score++;
            }
            else
            {
                request.Score--;
            }
            _unitOfWork.RequestVoteRepository.Insert(newRequestVote);
            _unitOfWork.Save();
            return RedirectToAction("Index", "Request", request);
        }
	}
}