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
        public MediaController() : base(new UnitOfWork())
        { 
        }
        
        /// <summary>
        /// Constructor for unit tests
        /// </summary>
        /// <param name="unitOfWork">The Data access object</param>
        public MediaController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Displays information about a media object and it's translations
        /// </summary>
        /// <param name="id">Id of the media object to display</param>
        /// <returns></returns>
        public ActionResult Index(int? id)
        {
            if (id.HasValue)
            {
                var model = _unitOfWork.MediaRepository.GetByID(id);
                string type = model.GetType().BaseType.Name;
                switch (type)
                {
                    case "Movie":
                        return View("MovieIndex", model);
                    case "Show":
                        return View("ShowIndex", model);
                    case "Clip":
                        return View("ClipIndex", model);
                    default:
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
	}
}