using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DCC_Parish_Capstone.Models;

namespace DCC_Parish_Capstone.Controllers
{
    public class ArticleNotificationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ArticleNotifications
        public ActionResult Index()
        {
            var articleNotifications = db.ArticleNotifications.Include(a => a.Article).Include(a => a.Subscription);
            return View(articleNotifications.ToList());
        }

 

        // GET: ArticleNotifications/Create
        public ActionResult Create()
        {
            ViewBag.ArticleId = new SelectList(db.Articles, "Id", "Title");
            ViewBag.SubscriptionId = new SelectList(db.Subscriptions, "Id", "AspNetUserId");
            return View();
        }

        // POST: ArticleNotifications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ArticleId,SubscriptionId,AspNetUserId")] ArticleNotification articleNotification)
        {
            if (ModelState.IsValid)
            {
                db.ArticleNotifications.Add(articleNotification);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ArticleId = new SelectList(db.Articles, "Id", "Title", articleNotification.ArticleId);
            ViewBag.SubscriptionId = new SelectList(db.Subscriptions, "Id", "AspNetUserId", articleNotification.SubscriptionId);
            return View(articleNotification);
        }

 

        // POST: ArticleNotifications/Delete/5 
        public ActionResult DeleteConfirmed(int id)
        {
            ArticleNotification articleNotification = db.ArticleNotifications.Find(id);
            db.ArticleNotifications.Remove(articleNotification);
            db.SaveChanges();
            return RedirectToAction("UserPortal","Manage", null);
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
