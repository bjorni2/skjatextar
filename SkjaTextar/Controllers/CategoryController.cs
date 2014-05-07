using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace SkjaTextar.Controllers
{
    public class CategoryController : BaseController
    {
        //
        // GET: /Category/
        public ActionResult Index()
        {
            var model = _unitOfWork.CategoryRepository.Get().ToList();
            return View(model);
        }

        public ActionResult MediaByCategory(int? id, string mediaType)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.Category = _unitOfWork.CategoryRepository.GetByID(id).Name;
            switch (mediaType)
            {
                case "Movie":
                    var modelMovie = _unitOfWork.MovieRepository.Get().Where(m => m.CategoryID == id).OrderBy(m => m.Title);
                    return View("MovieByCategory", modelMovie);
                case "Show":
                    var modelShow = _unitOfWork.ShowRepository.Get().Where(m => m.CategoryID == id).OrderBy(m => m.Title);
                    return View("ShowByCategory", modelShow);
                case "Clip":
                    var modelClip = _unitOfWork.ClipRepository.Get().Where(m => m.CategoryID == id).OrderBy(m => m.Title);
                    return View("ClipByCategory", modelClip);
                default:
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
        }
	}
}