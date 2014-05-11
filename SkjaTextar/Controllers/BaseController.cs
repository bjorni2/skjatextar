using SkjaTextar.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SkjaTextar.Exceptions;
using SkjaTextar.Helpers;

namespace SkjaTextar.Controllers
{
    [HandleError]
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

        protected override void OnException(ExceptionContext fc)
        {
            // Call the base class implementation:
            base.OnException(fc);
            Exception ex = fc.Exception;

            Logger.Instance.LogException(ex);
        }

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
	}
}