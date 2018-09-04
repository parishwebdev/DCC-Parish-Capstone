using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DCC_Parish_Capstone.Helpers;
using DCC_Parish_Capstone.Models;
using Microsoft.AspNet.Identity;

namespace DCC_Parish_Capstone.Controllers
{
    public class SubscriptionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        UserGamifyHelper userGamifyHelper = new UserGamifyHelper();

        // GET: Subscriptions
        public ActionResult Index()
        {
            var subscriptions = db.Subscriptions.Include(s => s.BestPracticeSub).Include(s => s.Language);
            return View(subscriptions.ToList());
        }
 

        [Authorize]
        public ActionResult Create([Bind(Include = "Id,LanguageId,BestPracticeId")] Subscription subscription)
        {
            if (ModelState.IsValid)
            {
                var currentUserId = User.Identity.GetUserId();
                subscription.AspNetUserId = currentUserId; 

                db.Subscriptions.Add(subscription);
                db.SaveChanges();
                 
                userGamifyHelper.UpdateAddUserPoints(5, subscription.AspNetUserId, db);

                return RedirectToAction("UserPortal","Manage", null);
            } 

            return RedirectToAction("UserPortal", "Manage", null);
        }
 
 

        // POST: Subscriptions/Delete/5 
        public ActionResult DeleteConfirmed(int id)
        {
            Subscription subscription = db.Subscriptions.Find(id);
            db.Subscriptions.Remove(subscription);
            db.SaveChanges();
             
            userGamifyHelper.UpdateMinusUserPoints(3, subscription.AspNetUserId, db);

            return RedirectToAction("UserPortal", "Manage", null);
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
