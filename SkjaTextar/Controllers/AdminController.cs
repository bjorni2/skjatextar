using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SkjaTextar.Models;
using SkjaTextar.DAL;

namespace SkjaTextar.Controllers
{
    public class AdminController : BaseController
    {
        //
        // GET: /Admin/
		public AdminController() : base()
        {
        }

        public AdminController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public ActionResult Index()
        {
			List<Report> reportList = _unitOfWork.ReportRepository.Get().OrderByDescending(r => r.ID).ToList();
            return View(reportList);
        }
	}
}