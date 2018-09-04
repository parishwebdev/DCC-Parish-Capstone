using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DCC_Parish_Capstone.Models;
using DCC_Parish_Capstone.Models.ViewModels;

namespace DCC_Parish_Capstone.Controllers
{
    public class LanguagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Languages
         
        public ActionResult Index()
        {
            var Languages = db.Languages.AsEnumerable();

            return View(Languages);
        }

        public ActionResult IndexWithBestPractices()
        {
            LanguageBestPracticeViewModel lBPVM = new LanguageBestPracticeViewModel();

            lBPVM.Languages = db.Languages.AsEnumerable();
            lBPVM.BestPractices = db.BestPractices.Where(bp => bp.Id != 12).AsEnumerable();

            return View(lBPVM);
        }
         
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
