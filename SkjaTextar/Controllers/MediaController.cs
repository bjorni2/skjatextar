using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SkjaTextar.DAL;
using System.Net;

namespace SkjaTextar.Controllers
{
    public class MediaController : BaseController
    {
        public MediaController() : base()
        {

        }

        public MediaController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MovieIndex(int? id)
        {
            if(id.HasValue)
            {
                var model = _unitOfWork.MovieRepository.GetByID(id);
                return View(model);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult ShowIndex(int? id)
        {
            if (id.HasValue)
            {
                var model = _unitOfWork.ShowRepository.GetByID(id);
                return View(model);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult ClipIndex(int? id)
        {
            if (id.HasValue)
            {
                var model = _unitOfWork.ClipRepository.GetByID(id);
                return View(model);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
	}
}