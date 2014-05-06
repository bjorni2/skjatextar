using SkjaTextar.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SkjaTextar.Controllers
{
    public class BaseController : Controller
    {
        protected IUnitOfWork _unitOfWork;

        public BaseController() : this(new UnitOfWork())
        {

        }

        public BaseController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
	}
}