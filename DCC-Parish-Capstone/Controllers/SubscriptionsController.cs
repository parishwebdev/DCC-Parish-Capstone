using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DCC_Parish_Capstone.Models;
using Microsoft.AspNet.Identity;

namespace DCC_Parish_Capstone.Controllers
{
    public class SubscriptionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Subscriptions
        public ActionResult Index()
        {
            var subscriptions = db.Subscriptions.Include(s => s.BestPracticeSub).Include(s => s.Language);
            return View(subscriptions.ToList());
        }
        /*
                // GET: Subscriptions/Details/5
                //public ActionResult Details(int? id)
                //{
                //    if (id == null)
                //    {
                //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                //    }
                //    Subscription subscription = db.Subscriptions.Find(id);
                //    if (subscription == null)
                //    {
                //        return HttpNotFound();
                //    }
                //    return View(subscription);
                //}

                //// GET: Subscriptions/Create
                //public ActionResult Create()
                //{
                //    ViewBag.BestPracticeId = new SelectList(db.BestPractices, "Id", "Name");
                //    ViewBag.LanguageId = new SelectList(db.Languages, "Id", "Name");
                //    return View();
                //}
        */

        [Authorize]
        public ActionResult Create([Bind(Include = "Id,LanguageId,BestPracticeId")] Subscription subscription)
        {
            if (ModelState.IsValid)
            {
                var currentUserId = User.Identity.GetUserId();
                subscription.AspNetUserId = currentUserId; 

                db.Subscriptions.Add(subscription);
                db.SaveChanges();
                return RedirectToAction("UserPortal","Manage", null);
            }

            //ViewBag.BestPracticeId = new SelectList(db.BestPractices, "Id", "Name", subscription.BestPracticeId);
            //ViewBag.LanguageId = new SelectList(db.Languages, "Id", "Name", subscription.LanguageId);
            return RedirectToAction("UserPortal", "Manage", null);
        }
/*
        // GET: Subscriptions/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Subscription subscription = db.Subscriptions.Find(id);
        //    if (subscription == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.BestPracticeId = new SelectList(db.BestPractices, "Id", "Name", subscription.BestPracticeId);
        //    ViewBag.LanguageId = new SelectList(db.Languages, "Id", "Name", subscription.LanguageId);
        //    return View(subscription);
        //}

        //// POST: Subscriptions/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,LanguageId,BestPracticeId,AspNetUserId")] Subscription subscription)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(subscription).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.BestPracticeId = new SelectList(db.BestPractices, "Id", "Name", subscription.BestPracticeId);
        //    ViewBag.LanguageId = new SelectList(db.Languages, "Id", "Name", subscription.LanguageId);
        //    return View(subscription);
        //}

    */

        // GET: Subscriptions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subscription subscription = db.Subscriptions.Find(id);
            if (subscription == null)
            {
                return HttpNotFound();
            }
            return View(subscription);
        }

        // POST: Subscriptions/Delete/5 
        public ActionResult DeleteConfirmed(int id)
        {
            Subscription subscription = db.Subscriptions.Find(id);
            db.Subscriptions.Remove(subscription);
            db.SaveChanges();
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
