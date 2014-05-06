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

        public ActionResult CategoryIndex(int? id, string mediaType)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = _unitOfWork.MovieRepository.Get().Where(m => m.CategoryID == id).OrderBy(m => m.Title);
            ViewBag.Category = _unitOfWork.CategoryRepository.GetByID(id).Name;
            return View("IndexMovie", model);
        }
	}
}