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
using SkjaTextar.Exceptions;
using SkjaTextar.Helpers;

namespace SkjaTextar.Controllers
{
    public class RequestController : BaseController
    {
        public RequestController() : base(new UnitOfWork())
        { 
        }
        
        /// <summary>
        /// Constructor for unit tests
        /// </summary>
        /// <param name="unitOfWork">The Data access object</param>
        public RequestController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Displays a table of all the requests on the site.
        /// </summary>
        /// <param name="sortOrder">The order in which the requests are sorted</param>
        /// <returns></returns>
        public ActionResult Index(string sortOrder)
        {
			// for toggling asc/desc sort order on columns
            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
			ViewBag.LanguageSortParm = sortOrder == "Lang" ? "lang_desc" : "Lang";
            ViewBag.ScoreSortParm = sortOrder == "score_desc" ? "Score" : "score_desc";

            var model = new List<RequestVoteViewModel>();
            var requests = _unitOfWork.RequestRepository.Get();
            var user = User.Identity.GetUserId();

			switch (sortOrder)
			{
				case "title_desc":
					requests = requests.OrderByDescending(r => r.Media.Title);
					break;
				case "Lang":
					requests = requests.OrderBy(r => r.Language.Name);
					break;
				case "lang_desc":
					requests = requests.OrderByDescending(r => r.Language.Name);
					break;
				case "Score":
					requests = requests.OrderBy(r => r.Score);
					break;
				case "score_desc":
					requests = requests.OrderByDescending(r => r.Score);
					break;
				default:
					requests = requests.OrderBy(s => s.Media.Title);
					break;
			}

			var requestList = requests.ToList();

            // Loops through all distinct media items in requestList
            // to add series and episode number to shows.
			foreach(var item in requestList.DistinctBy(r => r.MediaID))
			{
				var media = item.Media;
				if(media.GetType().BaseType.Name == "Show")
				{
					Show show = media as Show;
					item.Media.Title += " S" + show.Series + "E" + show.Episode;
				}
			}

            foreach (var item in requestList)
            {
                var tmp = new RequestVoteViewModel();
                tmp.Request = item;
                var userVote = _unitOfWork.RequestVoteRepository.Get()
                    .Where(r => r.UserID == user)
                    .Where(r => r.RequestID == item.ID)
                    .SingleOrDefault();
                if(userVote != null)
                {
                    tmp.Vote = userVote.Vote;
                }
                model.Add(tmp);
            }
			return View(model);
        }

        /// <summary>
        /// Displays a details page for request, different depending on the type of media
        /// </summary>
        /// <param name="id">The id of the request</param>
        /// <returns></returns>
		public ActionResult Details(int? id)
		{
			if(id.HasValue)
			{
				var model = _unitOfWork.RequestRepository.GetByID(id);
				if(model != null)
				{
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
							throw new ApplicationException();
					}
				}
				throw new DataNotFoundException();
			}
			throw new MissingParameterException();
		}

        /// <summary>
        /// Unused?
        /// </summary>
        /// <param name="id">The id of the media for the request</param>
        /// <returns></returns>
       /* public ActionResult CreateFor(int? id)
        {
            if (id.HasValue)
            {
                var model = _unitOfWork.MediaRepository.GetByID(id);
				if(model != null)
				{
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
							throw new ApplicationException();
					}
				}
				throw new DataNotFoundException();
            }
            throw new MissingParameterException();
        }*/

        /// <summary>
        /// Displays the appropriate view depending on the type of media the request is being made for
        /// </summary>
        /// <param name="Id">The id of the media for the request</param>
        /// <returns></returns>
        public ActionResult CreateForMedia(int? Id)
        {
            if (Id.HasValue)
            {
                ViewBag.LanguageID = new SelectList(_unitOfWork.LanguageRepository.Get(), "ID", "Name");
                var model = _unitOfWork.MediaRepository.GetByID(Id);
				if(model != null)
				{
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
							throw new ApplicationException();
					}
				}
				throw new DataNotFoundException();
            }
            throw new MissingParameterException();
        }


        /// <summary>
        /// Creates a new request for existing media
        /// </summary>
        /// <param name="Id">The id of the media to create the request for</param>
        /// <param name="languageId">The id of the language being requested</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateForMedia(int? Id, int? languageId)
        {
            if(Id == null || languageId == null)
            {
                throw new MissingParameterException();
            }
            var media = _unitOfWork.MediaRepository.Get()
                .Where(m => m.ID == Id)
                .SingleOrDefault();
            if(media == null)
            {
                throw new DataNotFoundException();
            }
            var translation = media.Translations
                .Where(t => t.LanguageID == languageId)
                .SingleOrDefault();
            var reqExists = _unitOfWork.RequestRepository.Get()
                .Where(r => r.MediaID == Id)
                .Where(r => r.LanguageID == languageId)
                .SingleOrDefault();
            if(translation == null && reqExists == null)
            {
                var request = new Request { LanguageID = languageId.Value, MediaID = Id.Value };
                _unitOfWork.RequestRepository.Insert(request);
                _unitOfWork.Save();
                TempData["UserMessage"] = "Beiðnin var stofnuð";
                return RedirectToAction("Index", "Request");
            }
            if (reqExists != null)
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
            ViewBag.LanguageID = new SelectList(_unitOfWork.LanguageRepository.Get(), "ID", "Name");
            string type = media.GetType().BaseType.Name;
            switch (type)
            {
                case "Movie":
                    return View("CreateForMovie", media);
                case "Show":
                    return View("CreateForShow", media);
                case "Clip":
                    return View("CreateForClip", media);
                default:
                    throw new ApplicationException();
            }
        }

        /// <summary>
        /// Displays the appropriate view for the specified media type
        /// and builds the selectLists for choosing the category and language
        /// </summary>
        /// <param name="mediaCat">The media type</param>
        /// <returns></returns>
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
                    TempData["UserMessage"] = "Beiðnin var stofnuð";
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
                    TempData["UserMessage"] = "Beiðnin var stofnuð";
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
                    TempData["UserMessage"] = "Beiðnin var stofnuð";
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
                    TempData["UserMessage"] = "Beiðnin var stofnuð";
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
					clipRequest.Clip.Link = "//www.youtube.com/embed/" + YoutubeParser.parseLink(clipRequest.Clip.Link);
					_unitOfWork.ClipRepository.Insert(clipRequest.Clip);
					_unitOfWork.RequestRepository.Insert(clipRequest.Request);
					_unitOfWork.Save();
                    TempData["UserMessage"] = "Beiðnin var stofnuð";
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
                    TempData["UserMessage"] = "Beiðnin var stofnuð";
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
				throw new MissingParameterException();
            }
            var userID = User.Identity.GetUserId();
            var request = _unitOfWork.RequestRepository.GetByID(id);
            var requestVote = _unitOfWork.RequestVoteRepository.Get().Where(r => r.RequestID == id).Where(r => r.UserID == userID).SingleOrDefault();
            if(requestVote != null)
            {
                if(requestVote.Vote == vote)
                {
                    return RedirectToAction("Index", "Request");
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
                    return RedirectToAction("Index", "Request");
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
            return RedirectToAction("Index", "Request");
        }
	}
}