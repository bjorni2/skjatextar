using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SkjaTextar.Models;
using System.Globalization;

namespace SkjaTextar.Controllers
{
    public class TranslationController : BaseController
    {
        //
        // GET: /Translation/
        public ActionResult Index(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = _unitOfWork.TranslationRepository.GetByID(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        [Authorize]
        public ActionResult Create()
        {
            List<SelectListItem> mediaType = new List<SelectListItem> 
            { 
                new SelectListItem{ Text = "Kvikmynd", Value = "Movie" },
                new SelectListItem{ Text = "Sjónvarpsþáttur", Value = "Show" },
                new SelectListItem{ Text = "Myndbrot", Value = "Clip" }
            };
            ViewBag.MediaType = new SelectList(mediaType, "Value", "Text");
            ViewBag.CategoryID = new SelectList(_unitOfWork.CategoryRepository.Get(), "ID", "Name");

            CultureInfo[] cinfo = CultureInfo.GetCultures(CultureTypes.InstalledWin32Cultures & ~CultureTypes.NeutralCultures);
            List<SelectListItem> language = new List<SelectListItem>();
            foreach(var cul in cinfo)
            {
                if(!cul.NativeName.Contains('('))
                { 
                    language.Add(new SelectListItem { Text = cul.NativeName, Value = cul.NativeName });
                }
            }
            ViewBag.LanguageSelect = new SelectList(language, "Value", "Text");
            return View(new Translation());
        }
	}
}