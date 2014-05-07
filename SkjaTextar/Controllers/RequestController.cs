using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

		public ActionResult Details()
		{

			return View();
		}
	}
}