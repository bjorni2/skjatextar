using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SkjaTextar.Models;
using SkjaTextar.DAL;
using System.Net;
using SkjaTextar.Exceptions;

namespace SkjaTextar.Controllers
{
    public class AdminController : BaseController
    {
		public AdminController() : base()
        {
        }

        public AdminController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Displays reports from users in a table.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
			List<Report> reportList = _unitOfWork.ReportRepository.Get().OrderByDescending(r => r.ID).ToList();
            return View(reportList);
        }

        /// <summary>
        /// Deletes a report from the database.
        /// </summary>
        /// <param name="id">The id of the report to delete</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int? id)
        {
            if(id == null)
            {
                throw new DataNotFoundException();
            }

            var model = _unitOfWork.ReportRepository.GetByID(id);
            if(model == null)
            {
                throw new MissingParameterException();
            }
            _unitOfWork.ReportRepository.Delete(id);
            _unitOfWork.Save();
            return RedirectToAction("Index", "Admin");
        }
	}
}