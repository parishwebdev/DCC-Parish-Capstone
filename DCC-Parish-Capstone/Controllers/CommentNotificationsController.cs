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
    public class CommentNotificationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CommentNotifications
        public ActionResult Index()
        {
            var commentNotifications = db.CommentNotifications.Include(c => c.Comment);
            return View(commentNotifications.ToList());
        }

        /*
        // GET: CommentNotifications/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommentNotification commentNotification = db.CommentNotifications.Find(id);
            if (commentNotification == null)
            {
                return HttpNotFound();
            }
            return View(commentNotification);
        }

        // GET: CommentNotifications/Create
        public ActionResult Create()
        {
            ViewBag.CommentId = new SelectList(db.Comments, "Id", "AspNetUserId");
            return View();
        }

        // POST: CommentNotifications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CommentId,AspNetUserId")] CommentNotification commentNotification)
        {
            if (ModelState.IsValid)
            {
                db.CommentNotifications.Add(commentNotification);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CommentId = new SelectList(db.Comments, "Id", "AspNetUserId", commentNotification.CommentId);
            return View(commentNotification);
        }

        // GET: CommentNotifications/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommentNotification commentNotification = db.CommentNotifications.Find(id);
            if (commentNotification == null)
            {
                return HttpNotFound();
            }
            ViewBag.CommentId = new SelectList(db.Comments, "Id", "AspNetUserId", commentNotification.CommentId);
            return View(commentNotification);
        }

        // POST: CommentNotifications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CommentId,AspNetUserId")] CommentNotification commentNotification)
        {
            if (ModelState.IsValid)
            {
                db.Entry(commentNotification).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CommentId = new SelectList(db.Comments, "Id", "AspNetUserId", commentNotification.CommentId);
            return View(commentNotification);
        }

    */

        // GET: CommentNotifications/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommentNotification commentNotification = db.CommentNotifications.Find(id);
            if (commentNotification == null)
            {
                return HttpNotFound();
            }
            return View(commentNotification);
        }

        // POST: CommentNotifications/Delete/5  
        public ActionResult DeleteConfirmed(int id)
        {
            CommentNotification commentNotification = db.CommentNotifications.Find(id);
            db.CommentNotifications.Remove(commentNotification);
            db.SaveChanges();
            return RedirectToAction("UserPortal","Manage",null);
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
