﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SkjaTextar.Models;
using SkjaTextar.DAL;
using System.Net;

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

        [HttpPost]
        public ActionResult Delete(int? id)
        {
            if(id.HasValue)
            {
                _unitOfWork.ReportRepository.Delete(id);
                _unitOfWork.Save();
                return RedirectToAction("Index", "Admin");
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
	}
}