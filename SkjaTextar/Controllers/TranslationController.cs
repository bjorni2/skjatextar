using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SkjaTextar.Models;
using SkjaTextar.ViewModels;
using System.Globalization;
using Microsoft.AspNet.Identity;
using System.IO;
using SkjaTextar.Helpers;
using PagedList;
using MoreLinq;
using SkjaTextar.Exceptions;

namespace SkjaTextar.Controllers
{
    public class TranslationController : BaseController
    {
        /// <summary>
        /// Displays the index page for a translation.
        /// </summary>
        /// <param name="id">Id of the translation to display</param>
        /// <returns></returns>
        public ActionResult Index(int? id, int? page)
        {
            if(id == null)
            {
				throw new MissingParameterException();
            }
            var translation = _unitOfWork.TranslationRepository.GetByID(id);
			
            if (translation == null)
            {
                throw new DataNotFoundException();
            }

			var model = translation.TranslationSegments.OrderBy(ts => ts.SegmentID);

			ViewBag.TranslationID = translation.ID;
			ViewBag.MediaTitle = translation.Media.Title;
			ViewBag.LanguageName = translation.Language.Name;
			ViewBag.MediaID = translation.MediaID;

			int pageSize = 50;
			int pageNumber = (page ?? 1);
			return View(model.ToPagedList(pageNumber, pageSize));
        }

        /// <summary>
        /// Return the create  translation page for specified media type, creating both the media and translation.
        /// </summary>
        /// <param name="mediaCat">Type of the media to create</param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Create(string mediaCat)
        {
            ViewBag.CategoryID = new SelectList(_unitOfWork.CategoryRepository.Get(), "ID", "Name");
            ViewBag.LanguageID = new SelectList(_unitOfWork.LanguageRepository.Get(), "ID", "Name");

            if(mediaCat != null)
            {
                mediaCat = mediaCat.ToLower();
            }
            switch (mediaCat)
            {
                case "movie":
                    return View("CreateMovie", new MovieTranslationViewModel { MediaType = "Kvikmynd" });
                case "show":
                    return View("CreateShow", new ShowTranslationViewModel { MediaType = "Sjónvarpsþáttur" });
                case "clip":
                    return View("CreateClip", new ClipTranslationViewModel { MediaType = "Myndbrot" });
                default:
                    throw new MissingParameterException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediaID"></param>
        /// <param name="languageID"></param>
		public void HasRequest(int mediaID, int languageID)
		{
			var request = _unitOfWork.RequestRepository.Get()
				.Where(r => r.MediaID == mediaID)
				.Where(r => r.LanguageID == languageID)
				.SingleOrDefault();
			if(request != null)
			{
                var requestVotes = _unitOfWork.RequestVoteRepository.Get().Where(r => r.RequestID == request.ID);
                foreach(var vote in requestVotes)
                {
                    _unitOfWork.RequestVoteRepository.Delete(vote);
                }
				_unitOfWork.RequestRepository.Delete(request);
				_unitOfWork.Save();
			}
		}

        /// <summary>
        /// Save the movie and translation to the database.
        /// </summary>
        /// <param name="movieTranslation">The movie and translation to create</param>
        /// <param name="file">Optional subtitle file to start off the translation</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult CreateMovie(MovieTranslationViewModel movieTranslation, HttpPostedFileBase file)
        {
            if(ModelState.IsValid)
            {
                var movie = movieTranslation.Movie;
                var movieToCheckFor = _unitOfWork.MovieRepository.Get()
                    .Where(m => m.Title == movie.Title)
                    .Where(m => m.ReleaseYear == movie.ReleaseYear)
                    .SingleOrDefault();
                if(movieToCheckFor == null)
                { 
                    movie.Translations = new List<Translation>();
                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                        file.SaveAs(path);
                        var translation = SubtitleParser.Parse(path, "srt");
                        translation.LanguageID = movieTranslation.LanguageID;
                        movie.Translations.Add(translation);
                    }
                    else
                    {
                        movie.Translations.Add(new Translation { LanguageID = movieTranslation.LanguageID });
                    }
                    _unitOfWork.MovieRepository.Insert(movie);
                    var userid = User.Identity.GetUserId();
                    var user = _unitOfWork.UserRepository.GetByID(userid);
                    user.NewTranslations++;
                    _unitOfWork.Save();
                    var newTranslation = _unitOfWork.TranslationRepository.Get().OrderByDescending(t => t.ID).First();
                    return RedirectToAction("Index", "Translation", new { id = newTranslation.ID });
                }
                else if (_unitOfWork.TranslationRepository.Get()
                    .Where(t => t.MediaID == movieToCheckFor.ID)
                    .Where(t => t.LanguageID == movieTranslation.LanguageID)
                    .SingleOrDefault() == null)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                        file.SaveAs(path);
                        var translation = SubtitleParser.Parse(path, "srt");
                        translation.LanguageID = movieTranslation.LanguageID;
                        movieToCheckFor.Translations.Add(translation);
                    }
                    else
                    {
                        movieToCheckFor.Translations.Add(new Translation { LanguageID = movieTranslation.LanguageID });
                    }
                    _unitOfWork.MovieRepository.Update(movieToCheckFor);
                    var userid = User.Identity.GetUserId();
                    var user = _unitOfWork.UserRepository.GetByID(userid);
                    user.NewTranslations++;
                    _unitOfWork.Save();
					HasRequest(movieToCheckFor.ID, movieTranslation.LanguageID);
                    var newTranslation = _unitOfWork.TranslationRepository.Get().OrderByDescending(t => t.ID).First();
                    return RedirectToAction("Index", "Translation", new { id = newTranslation.ID });
                }
                ViewBag.Errormsg = "Þessi þýðing er nú þegar til.";
            }
            ViewBag.CategoryID = new SelectList(_unitOfWork.CategoryRepository.Get(), "ID", "Name", movieTranslation.Movie.CategoryID);
            ViewBag.LanguageID = new SelectList(_unitOfWork.LanguageRepository.Get(), "ID", "Name");
            return View("CreateMovie");
        }

        [Authorize]
        [HttpPost]
        public ActionResult CreateShow(ShowTranslationViewModel showTranslation, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                var show = showTranslation.Show;
                var showToCheckFor = _unitOfWork.ShowRepository.Get()
                    .Where(s => s.Title == show.Title)
                    .Where(s => s.Series == show.Series)
                    .Where(s => s.Episode == show.Episode)
                    .SingleOrDefault();
                if (showToCheckFor == null)
                {
                    show.Translations = new List<Translation>();
                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                        file.SaveAs(path);
                        var translation = SubtitleParser.Parse(path, "srt");
                        translation.LanguageID = showTranslation.LanguageID;
                        show.Translations.Add(translation);
                    }
                    else
                    {
                        show.Translations.Add(new Translation { LanguageID = showTranslation.LanguageID });
                    }
                    _unitOfWork.ShowRepository.Insert(show);
                    var userid = User.Identity.GetUserId();
                    var user = _unitOfWork.UserRepository.GetByID(userid);
                    user.NewTranslations++;
                    _unitOfWork.Save();
                    var newTranslation = _unitOfWork.TranslationRepository.Get().OrderByDescending(t => t.ID).First();
                    return RedirectToAction("Index", "Translation", new { id = newTranslation.ID });
                }
                else if (_unitOfWork.TranslationRepository.Get()
                    .Where(t => t.MediaID == showToCheckFor.ID)
                    .Where(t => t.LanguageID == showTranslation.LanguageID)
                    .SingleOrDefault() == null)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                        file.SaveAs(path);
                        var translation = SubtitleParser.Parse(path, "srt");
                        translation.LanguageID = showTranslation.LanguageID;
                        showToCheckFor.Translations.Add(translation);
                    }
                    else
                    {
                        showToCheckFor.Translations.Add(new Translation { LanguageID = showTranslation.LanguageID });
                    }
                    _unitOfWork.ShowRepository.Update(showToCheckFor);
                    var userid = User.Identity.GetUserId();
                    var user = _unitOfWork.UserRepository.GetByID(userid);
                    user.NewTranslations++;
                    _unitOfWork.Save();
					HasRequest(showToCheckFor.ID, showTranslation.LanguageID);
                    var newTranslation = _unitOfWork.TranslationRepository.Get().OrderByDescending(t => t.ID).First();
                    return RedirectToAction("Index", "Translation", new { id = newTranslation.ID });
                }
                ViewBag.Errormsg = "Þessi þýðing er nú þegar til.";
            }
            ViewBag.CategoryID = new SelectList(_unitOfWork.CategoryRepository.Get(), "ID", "Name", showTranslation.Show.CategoryID);
            ViewBag.LanguageID = new SelectList(_unitOfWork.LanguageRepository.Get(), "ID", "Name");
            return View("CreateShow");
        }

        [Authorize]
        [HttpPost]
        public ActionResult CreateClip(ClipTranslationViewModel clipTranslation, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                var clip = clipTranslation.Clip;
                var clipToCheckFor = _unitOfWork.ClipRepository.Get()
                    .Where(c => c.Title == clip.Title)
                    .Where(c => c.ReleaseYear == clip.ReleaseYear)
                    .SingleOrDefault();
                if (clipToCheckFor == null)
                {
                    clip.Translations = new List<Translation>();
                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                        file.SaveAs(path);
                        var translation = SubtitleParser.Parse(path, "srt");
                        translation.LanguageID = clipTranslation.LanguageID;
                        clip.Translations.Add(translation);
                    }
                    else
                    {
                        clip.Translations.Add(new Translation { LanguageID = clipTranslation.LanguageID });
                    }
                    _unitOfWork.ClipRepository.Insert(clip);
                    var userid = User.Identity.GetUserId();
                    var user = _unitOfWork.UserRepository.GetByID(userid);
                    user.NewTranslations++;
                    _unitOfWork.Save();
                    var newTranslation = _unitOfWork.TranslationRepository.Get().OrderByDescending(t => t.ID).First();
                    return RedirectToAction("Index", "Translation", new { id = newTranslation.ID });
                }
                else if (_unitOfWork.TranslationRepository.Get()
                    .Where(t => t.MediaID == clipToCheckFor.ID)
                    .Where(t => t.LanguageID == clipTranslation.LanguageID)
                    .SingleOrDefault() == null)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                        file.SaveAs(path);
                        var translation = SubtitleParser.Parse(path, "srt");
                        translation.LanguageID = clipTranslation.LanguageID;
                        clipToCheckFor.Translations.Add(translation);
                    }
                    else
                    {
                        clipToCheckFor.Translations.Add(new Translation { LanguageID = clipTranslation.LanguageID });
                    }
                    _unitOfWork.ClipRepository.Update(clipToCheckFor);
                    var userid = User.Identity.GetUserId();
                    var user = _unitOfWork.UserRepository.GetByID(userid);
                    user.NewTranslations++;
                    _unitOfWork.Save();
					HasRequest(clipToCheckFor.ID, clipTranslation.LanguageID);
                    var newTranslation = _unitOfWork.TranslationRepository.Get().OrderByDescending(t => t.ID).First();
                    return RedirectToAction("Index", "Translation", new { id = newTranslation.ID });
                }
                ViewBag.Errormsg = "Þessi þýðing er nú þegar til.";
            }
            ViewBag.CategoryID = new SelectList(_unitOfWork.CategoryRepository.Get(), "ID", "Name", clipTranslation.Clip.CategoryID);
            ViewBag.LanguageID = new SelectList(_unitOfWork.LanguageRepository.Get(), "ID", "Name");
            return View("CreateClip");
        }

        [Authorize]
        public ActionResult CreateTranslation(int? id, int? languageid)
        {
            if (id.HasValue)
            {
                if(languageid.HasValue)
				{
					ViewBag.LanguageID = new SelectList(_unitOfWork.LanguageRepository.Get(), "ID", "Name", languageid);
				}
				else
				{
					ViewBag.LanguageID = new SelectList(_unitOfWork.LanguageRepository.Get(), "ID", "Name");
				}

                var model = _unitOfWork.MediaRepository.GetByID(id);
				if(model != null)
				{
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
							throw new ApplicationException();
					}
				}
				throw new DataNotFoundException();
            }
            throw new MissingParameterException();
        }

        [Authorize]
        [HttpPost]
        public ActionResult CreateTranslation(int? id, int? languageID, HttpPostedFileBase file)
        {
            var media = _unitOfWork.MediaRepository.GetByID(id);
            string type = "";
            if(languageID == null)
            {
                ViewBag.Errormsg = "Tungumál verður að vera valið.";
                ViewBag.LanguageID = new SelectList(_unitOfWork.LanguageRepository.Get(), "ID", "Name");
                type = media.GetType().BaseType.Name;
                switch (type)
                {
                    case "Movie":
                        return View("CreateMovieTranslation", media);
                    case "Show":
                        return View("CreateShowTranslation", media);
                    case "Clip":
                        return View("CreateClipTranslation", media);
                    default:
                        throw new ApplicationException();
                }
            }
            var translationToFind = _unitOfWork.TranslationRepository.Get()
                .Where(t => t.MediaID == id)
                .Where(t => t.LanguageID == languageID)
                .SingleOrDefault();
            
            if(translationToFind == null)
            {      
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                    file.SaveAs(path);
                    var translation = SubtitleParser.Parse(path, "srt");
                    translation.LanguageID = languageID.Value;
                    media.Translations.Add(translation);
                }
                else 
                {
                    media.Translations.Add(new Translation { LanguageID = languageID.Value });
                }
                _unitOfWork.MediaRepository.Update(media);
                var userid = User.Identity.GetUserId();
                var user = _unitOfWork.UserRepository.GetByID(userid);
                user.NewTranslations++;
                _unitOfWork.Save();
				HasRequest(id.Value, languageID.Value);
                var newTranslation = _unitOfWork.TranslationRepository.Get().OrderByDescending(t => t.ID).First();
                return RedirectToAction("Index", "Translation", new { id = newTranslation.ID });
            }
            ViewBag.Errormsg = "Þessi þýðing er nú þegar til.";
            ViewBag.LanguageID = new SelectList(_unitOfWork.LanguageRepository.Get(), "ID", "Name");
            type = media.GetType().BaseType.Name;
            switch (type)
            {
                case "Movie":
                    return View("CreateMovieTranslation", media);
                case "Show":
                    return View("CreateShowTranslation", media);
                case "Clip":
                    return View("CreateClipTranslation", media);
                default:
                    throw new ApplicationException();
            }
        }

        // TODO For admins only
        [Authorize]
        [HttpPost]
        public ActionResult LockTranslation(int? id)
        {
            if (id == null)
            {
				throw new MissingParameterException();
            }
            var translation = _unitOfWork.TranslationRepository.GetByID(id);
            if(translation == null)
            {
                throw new DataNotFoundException();
            }
            translation.Locked = true;
            _unitOfWork.TranslationRepository.Update(translation);
            _unitOfWork.Save();
            return RedirectToAction("Index", "Home");
        }

        // TODO For admins only
        [Authorize]
        [HttpPost]
        public ActionResult UnLockTranslation(int? id)
        {
            if (id == null)
            {
                throw new MissingParameterException();
            }
            var translation = _unitOfWork.TranslationRepository.GetByID(id);
            if (translation == null)
            {
                throw new DataNotFoundException();
            }
            translation.Locked = false;
            _unitOfWork.TranslationRepository.Update(translation);
            _unitOfWork.Save();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult CommentIndex(int? id)
        {
            if (id == null)
            {
                throw new MissingParameterException();
            }
            var translation = _unitOfWork.TranslationRepository.GetByID(id);
            if(translation == null)
            {
                throw new DataNotFoundException();
            }
            var model = new CommentViewModel();
            model.Translation = translation;
            model.Comments = translation.Comments.ToList();
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult CommentIndex(CommentViewModel commentViewModel)
        {
            if (ModelState.IsValid)
            {
                commentViewModel.Comment.UserID = User.Identity.GetUserId();
                var translation = _unitOfWork.TranslationRepository.GetByID(commentViewModel.Translation.ID);
                translation.Comments.Add(commentViewModel.Comment);
                _unitOfWork.TranslationRepository.Update(translation);
                _unitOfWork.Save();
                return RedirectToAction("CommentIndex", new { id = commentViewModel.Translation.ID });
            }
            commentViewModel.Translation = _unitOfWork.TranslationRepository.GetByID(commentViewModel.Translation.ID);
            commentViewModel.Comments = commentViewModel.Translation.Comments.ToList();
            return View(commentViewModel);
        }

        public ActionResult Download(int? translationId, int? mediaId)
        {
            if(translationId == null || mediaId == null)
            {
                throw new MissingParameterException();
            }
            var translation = _unitOfWork.TranslationRepository.GetByID(translationId);
            if(translation == null)
            {
                throw new DataNotFoundException();
            }
            translation.NumberOfDownloads++;
            _unitOfWork.TranslationRepository.Update(translation);
            _unitOfWork.Save();
            string fileName = translation.Media.Title + "(" + translation.Media.ReleaseYear + ")_" + translation.Language.Name + ".srt";
            return File(SubtitleParser.Output(translation).ToArray(), System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        public ActionResult Report(int? translationID)
        {
            if (translationID == null)
            {
                throw new MissingParameterException();
            }

            var translation = _unitOfWork.TranslationRepository.GetByID(translationID);
            if (translation == null)
            {
                throw new DataNotFoundException();
            }

			ViewBag.TranslationID = translationID;

            var report = new Report();
            report.Translation = translation;

            return View(report);
        }

        [HttpPost]
        public ActionResult Report(Report Report)
        {

            if (ModelState.IsValid)
            {
                var report = Report;
                report.TranslationID = Report.TranslationID;
                _unitOfWork.ReportRepository.Insert(report);
                _unitOfWork.Save();
                TempData["UserMessage"] = "Tilkynningu komið áleiðis";
                return RedirectToAction("Index", "Translation", new { id = Report.TranslationID });
            }
            throw new DataNotFoundException();
        }

        public ActionResult LanguageList()
        {
            var language = _unitOfWork.LanguageRepository.Get().ToList();
            return View(language);
        }

        public ActionResult ByLanguage(int? id)
        {
            if(id == null)
            {
                throw new MissingParameterException();
            }
            var translations = _unitOfWork.TranslationRepository.Get().Where(t => t.LanguageID == id).OrderBy(t => t.Media.Title).ToList();
            if(translations == null)
            {
                throw new DataNotFoundException();
            }

            var model = new SearchMediaViewModel();
            foreach (var item in translations)
            {
                string type = item.Media.GetType().BaseType.Name;
                switch (type)
                {
                    case "Movie":
                        model.Movies.Add(item.Media as Movie);
                        break;
                    case "Show":
                        model.Shows.Add(item.Media as Show);
                        break;
                    case "Clip":
                        model.Clips.Add(item.Media as Clip);
                        break;
                    default:
                        throw new ApplicationException();
                }
            }
            
           
            ViewBag.Language = _unitOfWork.LanguageRepository.GetByID(id);
            return View(model);
        }

		[HttpPost]
		public ActionResult UpdateLine(int? translationID, int? segmentID, string translationText, int line)
		{
			var translation = _unitOfWork.TranslationRepository.GetByID(translationID);
			var segment = translation.TranslationSegments.Where(t => t.SegmentID == segmentID).Single();
			if(line == 1)
			{
				segment.Line1 = translationText;
			}
			else if(line == 2)
			{
				segment.Line2 = translationText;
			}
			else
			{
				//TODO: Handle errors.
			}
			_unitOfWork.TranslationSegmentRepository.Update(segment);
            string userid = User.Identity.GetUserId();
            if(userid != null)
            {
                var user = _unitOfWork.UserRepository.GetByID(userid);
                user.Edits++;
            }
			_unitOfWork.Save();
			return null;
		}
        [Authorize]
        public ActionResult AddLine(int? Id)
        {
            var segment = new SegmentViewModel { TranslationID = Id.Value };
            return View(segment);
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddLine(SegmentViewModel segment)
        {
            if (ModelState.IsValid)
            {
                string startTime = segment.TimestampStart;
                int insertPos = 1;
                var segments = _unitOfWork.TranslationSegmentRepository.Get()
                    .Where(ts => ts.TranslationID == segment.TranslationID)
                    .OrderBy(ts => ts.SegmentID)
                    .ToList();

                // Find the insert position
                for (int i = 0; i < segments.Count; i++)
                {
                    string tmp = segments.ElementAt(i).Timestamp.Substring(0, 12);
                    if (string.Compare(tmp, startTime) > 0)
                    {
                        insertPos = segments.ElementAt(i).SegmentID;
                        break;
                    }
                    insertPos = segments.ElementAt(i).SegmentID + 1;
                }

                // increment segmentId on all segments with segmentId greater than the new segment
                for (int i = insertPos; i <= segments.Count; i++)
                {
                    segments.ElementAt(i - 1).SegmentID++;
                }

                // Insert the new segment
                var translationSegment = new TranslationSegment
                {
                    Line1 = segment.Line1,
                    Line2 = segment.Line2,
                    Original1 = segment.Original1,
                    Original2 = segment.Original2,
                    TranslationID = segment.TranslationID,
                    Timestamp = segment.TimestampStart + " --> " + segment.TimestampEnd,
                    SegmentID = insertPos
                };
                _unitOfWork.TranslationSegmentRepository.Insert(translationSegment);
                _unitOfWork.Save();
                return RedirectToAction("Index", new { id = segment.TranslationID });
            }
            return View(segment);
        }
	}
}