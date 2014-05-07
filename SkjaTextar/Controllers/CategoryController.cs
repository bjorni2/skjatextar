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
            if(id == 0)
            {
                ViewBag.Category = "Allt";
            }
            else
            {
                ViewBag.Category = _unitOfWork.CategoryRepository.GetByID(id).Name;
            }

            switch (mediaType)
            {
                case "Movie":
                    var modelMovie = _unitOfWork.MovieRepository.Get();
                    if(id == 0)
                    {
                        return View("MovieByCategory", modelMovie.OrderBy(m => m.Title));
                    }
                    return View("MovieByCategory", modelMovie.Where(m => m.CategoryID == id).OrderBy(m => m.Title));
                case "Show":
                    var modelShow = _unitOfWork.ShowRepository.Get();
                    if(id == 0)
                    {
                        return View("ShowByCategory", modelShow.OrderBy(m => m.Title));
                    }
                    return View("ShowByCategory", modelShow.Where(m => m.CategoryID == id).OrderBy(m => m.Title));
                case "Clip":
                    var modelClip = _unitOfWork.ClipRepository.Get();
                    if(id == 0)
                    {
                        return View("ClipByCategory", modelClip.OrderBy(m => m.Title));
                    }
                    return View("ClipByCategory", modelClip.Where(m => m.CategoryID == id).OrderBy(m => m.Title));
                default:
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
	}
}