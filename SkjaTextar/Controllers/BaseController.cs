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
    /// <summary>
    /// Base controller for the application.
    /// All other controllers inherit this controller
    /// </summary>
    [HandleError]
    public class BaseController : Controller
    {
        // _unitOfWork handles data flow to/from the database through repositories
        protected IUnitOfWork _unitOfWork;

        public BaseController() : this(new UnitOfWork())
        {

        }

        /// <summary>
        /// Constructor for Unit Testing controllers
        /// </summary>
        /// <param name="unitOfWork"></param>
        public BaseController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Runs when errors are thrown in controllers
        /// </summary>
        /// <param name="fc"></param>
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